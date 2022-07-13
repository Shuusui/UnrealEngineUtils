using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UnrealExtension.Windows;
using UnrealExtension.Windows.ControlWindows;

namespace UnrealExtension.Commands.ControlCommands
{
    public class OpenAddModuleWindowCommand : ICommand
    {
        public OpenAddModuleWindowCommand(PluginManager pluginManager)
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
            AddModuleWindow _addModuleWindow = new AddModuleWindow(m_pluginManager);
            _addModuleWindow.ShowDialog();
            m_pluginManager.UpdatePluginsList();
        }

        private PluginManager m_pluginManager;
    }
}
