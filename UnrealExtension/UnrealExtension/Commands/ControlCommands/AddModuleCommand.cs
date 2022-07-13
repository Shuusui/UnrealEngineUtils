using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using UnrealExtension.Windows;

namespace UnrealExtension.Commands.ControlCommands
{
    public class AddModuleCommand : ICommand
    {
        public AddModuleCommand(PluginManager pluginManager)
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
            if (parameter == null)
            {
                throw new ArgumentNullException();
            }
            string _moduleName = parameter as string;
            if (string.IsNullOrEmpty(_moduleName))
            {
                MessageBox.Show("There is no module name configured",
                    "Module name null",
                    MessageBoxButtons.OK);
                return;
            }
            string _modulePath = System.IO.Path.Combine(m_pluginManager.SelectedPlugin.PluginSourcePath,
                _moduleName);
            if (System.IO.Directory.Exists(_modulePath))
            {
                DialogResult _result = MessageBox.Show($"A module with the name {_moduleName} in plugin {m_pluginManager.SelectedPlugin.Name} " +
                    $"already exists. Do you want to replace it with a default one?",
                    "Module already exists", MessageBoxButtons.YesNo);
                if (_result == DialogResult.Yes)
                {
                    System.IO.Directory.Delete(_modulePath, true);
                }
                else
                {
                    return;
                }
            }
            Utils.CreateModule(m_pluginManager.SelectedPlugin, _moduleName);
            m_pluginManager.SelectedPlugin.SaveUPluginFile();
        }
        private PluginManager m_pluginManager;
    }
}
