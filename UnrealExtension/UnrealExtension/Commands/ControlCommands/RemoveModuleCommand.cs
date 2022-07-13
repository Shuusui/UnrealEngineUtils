using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UnrealExtension.Windows;

namespace UnrealExtension.Commands.ControlCommands
{
    public class RemoveModuleCommand : ICommand
    {
        public RemoveModuleCommand(PluginManager pluginManager)
        {
            m_pluginManager = pluginManager;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Module _module = (Module)parameter;
            if (_module == null)
            {
                return;
            }
            string _modulePath = System.IO.Path.Combine(m_pluginManager.PluginsDir, m_pluginManager.SelectedPlugin.Name, "Source", _module.Name);
            if (System.IO.Directory.Exists(_modulePath))
            {
                System.IO.Directory.Delete(_modulePath, true);
            }
            _module.AssociatedPlugin.RemoveModule(_module.Name);
            _module.AssociatedPlugin.SaveUPluginFile();
        }
        private PluginManager m_pluginManager;
    }
}
