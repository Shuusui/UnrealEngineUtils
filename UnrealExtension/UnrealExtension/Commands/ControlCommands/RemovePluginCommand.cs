using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UnrealExtension.Windows;

namespace UnrealExtension.Commands.ControlCommands
{
    public class RemovePluginCommand : ICommand
    {
        public RemovePluginCommand(PluginManager pluginManager)
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
            Plugin _plugin = (Plugin)parameter;
            if (_plugin == null)
            {
                return;
            }
            string _pluginPath = System.IO.Path.Combine(m_pluginManager.PluginsDir, _plugin.Name);
            if (System.IO.Directory.Exists(_pluginPath))
            {
                System.IO.Directory.Delete(_pluginPath, true);
                _ = m_pluginManager.RemovePlugin(_plugin.Name);
            }
        }
        private PluginManager m_pluginManager;
    }
}
