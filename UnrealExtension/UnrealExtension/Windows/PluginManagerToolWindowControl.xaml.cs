using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace UnrealExtension.Windows
{
    public class Plugin : INotifyPropertyChanged
    {
        private string m_name = string.Empty;
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                SetPropertyValue(ref m_name, value);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void SetPropertyValue<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (value == null ? field == null : value.Equals(field))
            {
                return;
            }
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Module : INotifyPropertyChanged
    {
        private string m_name = string.Empty;
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                SetPropertyValue(ref m_name, value);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void SetPropertyValue<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (value == null ? field == null : value.Equals(field))
            {
                return;
            }
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class PluginManager : INotifyPropertyChanged
    {
        private ObservableCollection<Plugin> m_plugins = new ObservableCollection<Plugin>();
        public ObservableCollection<Plugin> Plugins
        {
            get
            {
                return m_plugins;
            }
            set
            {
                SetPropertyValue(ref m_plugins, value);
            }
        }

        private ObservableCollection<Module> m_modules = new ObservableCollection<Module>();
        public ObservableCollection<Module> Modules
        {
            get
            {
                return m_modules;
            }
            set
            {
                SetPropertyValue(ref m_modules, value);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void SetPropertyValue<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (value == null ? field == null : value.Equals(field))
            {
                return;
            }
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Interaction logic for PluginManagerToolWindowControl.
    /// </summary>
    public partial class PluginManagerToolWindowControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginManagerToolWindowControl"/> class.
        /// </summary>
        public PluginManagerToolWindowControl()
        {
            PluginManagerInstance = new PluginManager();
            PluginManagerInstance.Plugins = new ObservableCollection<Plugin>();
            PluginManagerInstance.Modules = new ObservableCollection<Module>();
            this.InitializeComponent();
            DataContext = PluginManagerInstance;
            m_solution = (IVsSolution)Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(IVsSolution));
            m_solution.GetSolutionInfo(out string _slnDir, out string _solutionName, out string _suoDir);
            m_pluginsDir = System.IO.Path.Combine(_slnDir, "Plugins");
            FillPluginsList();
        }

        private void FillPluginsList()
        {
            ObservableCollection<Plugin> _plugins = new ObservableCollection<Plugin>();
            if (System.IO.Directory.Exists(m_pluginsDir))
            {
                string[] _pluginDirs = System.IO.Directory.GetDirectories(m_pluginsDir);
                foreach (string _pluginDir in _pluginDirs)
                {
                    string[] _pluginFilePaths = System.IO.Directory.GetFiles(_pluginDir, "*.uplugin");
                    if (_pluginFilePaths.Length > 0)
                    {
                        _plugins.Add(new Plugin
                        {
                            Name = System.IO.Path.GetFileNameWithoutExtension(_pluginFilePaths[0])
                        });
                    }
                }
            }
            PluginManagerInstance.Plugins = _plugins;
        }

        public PluginManager PluginManagerInstance { get; set; }
        private IVsSolution m_solution;
        private string m_pluginsDir;
    }
}
