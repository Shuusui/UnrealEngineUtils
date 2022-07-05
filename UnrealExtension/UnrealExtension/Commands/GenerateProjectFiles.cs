using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace UnrealExtension.Commands
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class GenerateProjectFiles
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("df59dc26-3f34-408f-b9af-33dccb6c0a1d");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        private readonly MenuCommand m_generateProjectFilesButton;

        private bool m_canGenerateProjectFiles = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateProjectFiles"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private GenerateProjectFiles(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            m_generateProjectFilesButton = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(m_generateProjectFilesButton);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static GenerateProjectFiles Instance
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

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in GenerateProjectFiles's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new GenerateProjectFiles(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            IVsSolution _solution = (IVsSolution)Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(IVsSolution));
            _solution.GetSolutionInfo(out string _slnDir, out string _solutionName, out string _suoDir);
            string[] _uprojectFiles = System.IO.Directory.GetFiles(_slnDir, "*.uproject");
            if (_uprojectFiles.Length <= 0)
            {
                string[] _generateProjectFiles = System.IO.Directory.GetFiles(_slnDir, "GenerateProjectFiles.bat");
                if (_generateProjectFiles.Length > 0)
                {
                    ProcessStartInfo _generateProjectFilesStartInfo = new ProcessStartInfo
                    {
                        FileName = _generateProjectFiles[0]
                    };
                    Process _generateProjectFilesProc = new Process
                    {
                        StartInfo = _generateProjectFilesStartInfo
                    };
                    _generateProjectFilesProc.Start();
                    return;
                }

                VsShellUtilities.ShowMessageBox(
                    package,
                    "There is no uproject file or GenerateProjectFiles.bat in the solution folder. Make sure the directory is of an unreal engine project or the unreal engine source folder",
                    "No unreal engine directory",
                    OLEMSGICON.OLEMSGICON_CRITICAL,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
                return;
            }
            else if (_uprojectFiles.Length > 1)
            {
                VsShellUtilities.ShowMessageBox(
                    package,
                    $"Found multiple uproject files. We'll select the first ({_uprojectFiles[0]}) to update the solution",
                    "Multiple uproject files",
                    OLEMSGICON.OLEMSGICON_WARNING,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST
                    );
            }

            string _uprojectFile = _uprojectFiles[0];
            using (System.IO.StreamReader _uprojectReader = new System.IO.StreamReader(_uprojectFile))
            {
                string _uprojectJson = _uprojectReader.ReadToEnd();

                UProjectFileObject _uprojectFileObject = JsonConvert.DeserializeObject<UProjectFileObject>(_uprojectJson);
                if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
                {
                    string _enginePath = string.Empty;
                    if (Guid.TryParse(_uprojectFileObject.EngineAssociation, out Guid _))
                    {
                        const string REGISTRY_ENTRY_PATH = "HKEY_CURRENT_USER\\SOFTWARE\\Epic Games\\Unreal Engine\\Builds";
                        _enginePath = Microsoft.Win32.Registry.GetValue(REGISTRY_ENTRY_PATH, _uprojectFileObject.EngineAssociation, "").ToString();
                        if (string.IsNullOrEmpty(_enginePath))
                        {
                            VsShellUtilities.ShowMessageBox(
                                package,
                                "Under the associated unreal engine GUID, we could not find any install location registered in registry. Make sure you've installed the source build properly by following instructions on the github repository of the engine",
                                "GUID not in registry",
                                OLEMSGICON.OLEMSGICON_CRITICAL,
                                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
                            return;
                        }
                    }
                    else
                    {
                        string _programDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                        string _launcherInstalledPath = System.IO.Path.Combine(_programDataFolderPath, "Epic", "UnrealEngineLauncher", "LauncherInstalled.dat");
                        if (System.IO.File.Exists(_launcherInstalledPath))
                        {
                            using (System.IO.StreamReader _launcherInstalledReader = new System.IO.StreamReader(_launcherInstalledPath))
                            {
                                string _launcherInstalledJson = _launcherInstalledReader.ReadToEnd();
                                LauncherInstalledObject _launcherInstalledObject = JsonConvert.DeserializeObject<LauncherInstalledObject>(_launcherInstalledJson);
                                foreach (Installation _installation in _launcherInstalledObject.InstallationList)
                                {
                                    if (_installation.NamespaceId == "ue" && _installation.AppName.Contains(_uprojectFileObject.EngineAssociation))
                                    {
                                        _enginePath = _installation.InstallLocation;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            VsShellUtilities.ShowMessageBox(
                                package,
                                "There is no unreal engine version registered. Make sure you've installed an unreal engine version properly",
                                "No unreal engine registered",
                                OLEMSGICON.OLEMSGICON_CRITICAL,
                                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
                            return;
                        }
                    }
                    if (!string.IsNullOrEmpty(_enginePath))
                    {
                        string _ubtPath = System.IO.Path.Combine(_enginePath, "Engine", "Binaries", "DotNET", "UnrealBuildTool", "UnrealBuildTool.exe");
                        if (System.IO.Directory.Exists(_enginePath) && System.IO.File.Exists(_ubtPath))
                        {
                            string _args = CombineStrings(" ", new string[]
                            {
                                "/C",
                                _ubtPath,
                                "-projectfiles",
                                $"-project={_uprojectFile}",
                                "-game",
                                "-engine",
                                "-progress"
                            });

                            Utils.RunProgramm("cmd.exe", _args, ProcessUbtExited, ProcessUbtOutput, ProcessUbtOutput);
                            ToggleGenerateProjectFilesButtonState();
                        }
                        else
                        {
                            if (!System.IO.Directory.Exists(_enginePath))
                            {
                                VsShellUtilities.ShowMessageBox(
                                    package,
                                    "The engine directory found to the associated engine of the project is an invalid path. Please check your installation directory and that your project is associated right",
                                    "Unreal engine path invalid",
                                    OLEMSGICON.OLEMSGICON_CRITICAL,
                                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
                                return;
                            }
                            if (!System.IO.File.Exists(_ubtPath))
                            {
                                VsShellUtilities.ShowMessageBox(
                                    package,
                                    "The path to unreal build tool exe is invalid. Please make sure that the Unreal Build tool also got build before generating project files",
                                    "Unreal build tool invalid path",
                                    OLEMSGICON.OLEMSGICON_CRITICAL,
                                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
                            }
                        }
                    }
                    else
                    {
                        VsShellUtilities.ShowMessageBox(
                        package,
                        "Could not find an engine installation. Make sure you have unreal engine installed and associated with the project you want to update files",
                        "No Unreal Engine install location",
                        OLEMSGICON.OLEMSGICON_CRITICAL,
                        OLEMSGBUTTON.OLEMSGBUTTON_OK,
                        OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
                    }
                }
            }
        }

        private string CombineStrings(string delim, params string[] args)
        {
            StringBuilder _sb = new StringBuilder(args[0]);
            for (int i = 1; i < args.Length; ++i)
            {
                _sb.Append(delim);
                _sb.Append(args[i]);
            }
            return _sb.ToString();
        }
        private void ToggleGenerateProjectFilesButtonState()
        {
            m_canGenerateProjectFiles = !m_canGenerateProjectFiles;
            m_generateProjectFilesButton.Enabled = m_canGenerateProjectFiles;
        }
        private void ProcessUbtExited(object sender, EventArgs args)
        {
            ToggleGenerateProjectFilesButtonState();
        }
        private void ProcessUbtOutput(object sender, DataReceivedEventArgs args)
        {
            Microsoft.VisualStudio.Threading.JoinableTask _task = ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
               {
                   await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                   IVsOutputWindowPane _pane = package.GetOutputPane(Microsoft.VisualStudio.VSConstants.OutputWindowPaneGuid.BuildOutputPane_guid, "Build");
                   if (_pane != null)
                   {
                       _pane.OutputStringThreadSafe(args.Data + "\n");
                   }
               });
            _task.Join();
        }
    }
}
