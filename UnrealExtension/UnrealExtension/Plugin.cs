using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnrealExtension.Commands.ControlCommands;
using UnrealExtension.Windows;

namespace UnrealExtension
{
    public class Plugin : INotifyPropertyChanged
    {
        public Plugin(PluginManager pluginManager, string pluginName)
        {
            Name = pluginName;
            m_pluginManager = pluginManager;
            PluginPath = System.IO.Path.Combine(pluginManager.SelectedProject.PluginsDir, pluginName);
            PluginSourcePath = System.IO.Path.Combine(PluginPath, "Source");
            UPluginFilePath = System.IO.Path.ChangeExtension(System.IO.Path.Combine(PluginPath, Name), ".uplugin");
            if (System.IO.Directory.Exists(PluginSourcePath))
            {
                string[] _modulePaths = System.IO.Directory.GetDirectories(PluginSourcePath);
                foreach (string _modulePath in _modulePaths)
                {
                    string[] _buildCsFiles = System.IO.Directory.GetFiles(_modulePath, "*.Build.cs");
                    if (_buildCsFiles.Length > 0)
                    {
                        m_modules.Add(new Module(this)
                        {
                            Name = System.IO.Path.GetFileName(_modulePath)
                        });
                    }
                }
            }
        }
        private ObservableCollection<Module> m_modules = new ObservableCollection<Module>();
        public ObservableCollection<Module> Modules
        {
            get { return m_modules; }
            set
            {
                SetPropertyValue(ref m_modules, value);
            }
        }
        public bool AddModule(string name)
        {
            foreach (Module _module in m_modules)
            {
                if (_module.Name == name)
                {
                    return false;
                }
            }
            Modules.Add(new Module(this)
            {
                Name = name
            });
            return true;
        }
        public bool RemoveModule(string name)
        {
            for (int i = m_modules.Count - 1; i >= 0; --i)
            {
                if (m_modules[i].Name == name)
                {
                    Modules.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
        private Module m_selectedModule;
        public Module SelectedModule
        {
            get
            {
                return m_selectedModule;
            }
            set
            {
                SetPropertyValue(ref m_selectedModule, value);
            }
        }
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
        public string AvailableModulesTitle
        {
            get
            {
                return $"Available Modules of {m_name}:";
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private System.Windows.Input.ICommand _openAddModuleWindowCommand;
        public System.Windows.Input.ICommand OpenAddModuleWindowCommand
        {
            get
            {
                if (_openAddModuleWindowCommand == null)
                {
                    _openAddModuleWindowCommand = new OpenAddModuleWindowCommand(m_pluginManager);
                }
                return _openAddModuleWindowCommand;
            }
        }
        private System.Windows.Input.ICommand _removeModuleCommand;
        public System.Windows.Input.ICommand RemoveModuleCommand
        {
            get
            {
                if (_removeModuleCommand == null)
                {
                    _removeModuleCommand = new RemoveModuleCommand(m_pluginManager);
                }
                return _removeModuleCommand;
            }
        }
        protected void SetPropertyValue<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (value == null ? field == null : value.Equals(field))
            {
                return;
            }
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private PluginManager m_pluginManager;
        public string PluginPath { get; private set; }
        public string PluginSourcePath { get; private set; }
        public string UPluginFilePath { get; private set; }
        private UPluginFileObject m_pluginFileObject = null;
        public UPluginFileObject PluginFileObject
        {
            get
            {
                if (m_pluginFileObject == null)
                {
                    if (System.IO.File.Exists(UPluginFilePath))
                    {
                        using (System.IO.StreamReader _reader = new System.IO.StreamReader(UPluginFilePath))
                        {
                            m_pluginFileObject = Newtonsoft.Json.JsonConvert.DeserializeObject<UPluginFileObject>(_reader.ReadToEnd());
                        }
                    }
                }
                return m_pluginFileObject;
            }
            set
            {
                m_pluginFileObject = value;
            }
        }
        public void SaveUPluginFile()
        {
            if (m_pluginFileObject == null)
            {
                return;
            }
            System.IO.File.WriteAllText(UPluginFilePath, Newtonsoft.Json.JsonConvert.SerializeObject(m_pluginFileObject, Newtonsoft.Json.Formatting.Indented));
        }
    }
}
