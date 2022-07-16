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
using UnrealExtension.Commands;
using UnrealExtension.Commands.ControlCommands;

namespace UnrealExtension
{
    public class PluginManager : NotifiableProperty
    {
        public PluginManager()
        {
            UpdatePluginsList();
        }
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
        public string AvailablePluginsTitle
        {
            get
            {
                return $"Available Plugins of {SelectedProject.ProjectName}:";
            }
        }
        private UProjectFileInfo m_selectedProject;
        public UProjectFileInfo SelectedProject
        {
            get
            {
                if (m_selectedProject == null)
                {
                    m_selectedProject = PluginManagerToolWindowCommand.Instance.ProjectFileInfos[0];
                }
                return m_selectedProject;
            }
            set
            {
                SetPropertyValue(ref m_selectedProject, value);
                UpdatePluginsList();
            }
        }
        public List<UProjectFileInfo> Projects
        {
            get
            {
                return PluginManagerToolWindowCommand.Instance.ProjectFileInfos;
            }
            private set
            {
                PluginManagerToolWindowCommand.Instance.ProjectFileInfos = value;
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
            if (System.IO.Directory.Exists(SelectedProject.PluginsDir))
            {
                string[] _pluginDirs = System.IO.Directory.GetDirectories(SelectedProject.PluginsDir);
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
    }
}
