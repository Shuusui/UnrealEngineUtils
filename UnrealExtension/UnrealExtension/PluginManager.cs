using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnrealExtension.Commands.ControlCommands;

namespace UnrealExtension
{
    public class PluginManager : INotifyPropertyChanged
    {
        public bool AddPlugin(Plugin plugin)
        {
            foreach (Plugin _plugin in m_plugins)
            {
                if (_plugin.PluginPath == plugin.PluginPath)
                {
                    return false;
                }
            }
            m_plugins.Add(plugin);
            return true;
        }
        public bool RemovePlugin(string name)
        {
            for (int i = m_plugins.Count - 1; i >= 0; i--)
            {
                if (m_plugins[i].Name == name)
                {
                    m_plugins.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
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
        private Plugin m_selectedPlugin;
        public Plugin SelectedPlugin
        {
            get
            {
                return m_selectedPlugin;
            }
            set
            {
                SetPropertyValue(ref m_selectedPlugin, value);
            }
        }
        private string m_projectName = string.Empty;
        public string ProjectName
        {
            get
            {
                return m_projectName;
            }
            set
            {
                SetPropertyValue(ref m_projectName, value);
            }
        }
        public string AvailablePluginsTitle
        {
            get
            {
                return $"Available Plugins of {m_projectName}:";
            }
        }
        private IVsSolution m_solution;
        public IVsSolution Solution
        {
            get { return m_solution; }
            set
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                value.GetSolutionInfo(out string _slnDir, out string _, out string _);
                PluginsDir = System.IO.Path.Combine(_slnDir, "Plugins");
                string[] _uprojectFilePaths = System.IO.Directory.GetFiles(_slnDir, "*.uproject");
                if (_uprojectFilePaths.Length > 0)
                {
                    ProjectName = System.IO.Path.GetFileNameWithoutExtension(_uprojectFilePaths[0]);
                    SetPropertyValue(ref m_solution, value);
                    UpdatePluginsList();
                }
            }
        }
        private System.Windows.Input.ICommand _openAddPluginWindowCommand;
        public System.Windows.Input.ICommand OpenAddPluginWindowCommand
        {
            get
            {
                if (_openAddPluginWindowCommand == null)
                {
                    _openAddPluginWindowCommand = new OpenAddPluginWindowCommand(this);
                }
                return _openAddPluginWindowCommand;
            }
        }
        private System.Windows.Input.ICommand _removePluginCommand;
        public System.Windows.Input.ICommand RemovePluginCommand
        {
            get
            {
                if (_removePluginCommand == null)
                {
                    _removePluginCommand = new RemovePluginCommand(this);
                }
                return _removePluginCommand;
            }
        }


        public void UpdatePluginsList()
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
                        _plugins.Add(new Plugin(this, System.IO.Path.GetFileNameWithoutExtension(_pluginFilePaths[0])));
                    }
                }
            }
            Plugins = _plugins;
        }

        private string m_pluginsDir;
        public string PluginsDir
        {
            get { return m_pluginsDir; }
            set
            {
                SetPropertyValue(ref m_pluginsDir, value);
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
}
