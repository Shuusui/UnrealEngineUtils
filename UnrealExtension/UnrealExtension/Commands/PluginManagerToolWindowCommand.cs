using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using UnrealExtension.Windows;
using Task = System.Threading.Tasks.Task;

namespace UnrealExtension.Commands
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class PluginManagerToolWindowCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x101;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("df59dc26-3f34-408f-b9af-33dccb6c0a1d");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginManagerToolWindowCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private PluginManagerToolWindowCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static PluginManagerToolWindowCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        public DTE DTE { get; private set; }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in PluginManagerToolWindowCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);


            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new PluginManagerToolWindowCommand(package, commandService);
            Instance.DTE = await package.GetServiceAsync(typeof(DTE)) as DTE;
        }

        /// <summary>
        /// Shows the tool window when the menu item is clicked.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            IVsSolution _solution = (IVsSolution)Package.GetGlobalService(typeof(IVsSolution));
            _solution.GetSolutionInfo(out string _slnDir, out string _, out string _);
            string[] _uprojectFilePaths = GetUProjectFiles(_slnDir);
            ProjectFileInfos = new List<UProjectFileInfo>();
            if (_uprojectFilePaths.Length > 0)
            {
                string _projectFilePath = _uprojectFilePaths[0];
                ProjectFileInfos.Add(new UProjectFileInfo()
                {
                    DirPath = _slnDir,
                    ProjectName = System.IO.Path.GetFileNameWithoutExtension(_projectFilePath),
                    ProjectFilePath = _projectFilePath
                });
            }
            else
            {
                string[] _directories = System.IO.Directory.GetDirectories(_slnDir);
                foreach (string _directory in _directories)
                {
                    string[] _dirUprojectFilePaths = GetUProjectFiles(_directory);
                    if (_dirUprojectFilePaths.Length > 0)
                    {
                        string _projectFilePath = _dirUprojectFilePaths[0];
                        ProjectFileInfos.Add(new UProjectFileInfo()
                        {
                            DirPath = _directory,
                            ProjectName = System.IO.Path.GetFileNameWithoutExtension(_projectFilePath),
                            ProjectFilePath = _projectFilePath
                        });
                    }
                }
                if (ProjectFileInfos.Count <= 0)
                {
                    VsShellUtilities.ShowMessageBox(package,
                        "This solution seems to be not one of an unreal engine project",
                        "No Unreal Engine project detected",
                        OLEMSGICON.OLEMSGICON_CRITICAL,
                        OLEMSGBUTTON.OLEMSGBUTTON_OK, OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
                }
            }

            for (int i = 0; i < ProjectFileInfos.Count; ++i)
            {
                foreach (string _startupProject in (Array)DTE.Solution.SolutionBuild.StartupProjects)
                {
                    if (ProjectFileInfos[i].ProjectName == _startupProject)
                    {
                        if (i != 0)
                        {
                            (ProjectFileInfos[0], ProjectFileInfos[i]) = (ProjectFileInfos[i], ProjectFileInfos[0]);
                        }
                        goto Found;
                    }
                }
            }
        Found:

            // Get the instance number 0 of this tool window. This window is single instance so this instance
            // is actually the only one.
            // The last flag is set to true so that if the tool window does not exists it will be created.
            PluginManagerToolWindow _window = (PluginManagerToolWindow)package.FindToolWindow(typeof(PluginManagerToolWindow), 0, true);
            if ((null == _window) || (null == _window.Frame))
            {
                throw new NotSupportedException("Cannot create tool window");
            }
            IVsWindowFrame _windowFrame = (IVsWindowFrame)_window.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(_windowFrame.Show());
        }
        private string[] GetUProjectFiles(string dirPath)
        {
            return System.IO.Directory.GetFiles(dirPath, "*.uproject");
        }

        public List<UProjectFileInfo> ProjectFileInfos { get; set; }
    }
}
