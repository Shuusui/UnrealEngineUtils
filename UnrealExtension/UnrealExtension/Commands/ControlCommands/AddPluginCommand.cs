using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using UnrealExtension.Windows;
using UnrealExtension.Windows.ControlWindows;

namespace UnrealExtension.Commands.ControlCommands
{
    public class AddPluginCommand : ICommand
    {
        public AddPluginCommand(PluginManager pluginManager)
        {
            m_pluginManager = pluginManager ?? throw new NotSupportedException("Cannot execute add plugin command if plugin manaager is null");
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
            UPluginFileObject _pluginFileObject = (UPluginFileObject)parameter;
            if (string.IsNullOrEmpty(_pluginFileObject.PluginName))
            {
                MessageBox.Show("There is no plugin name configured",
                    "Plugin name null",
                    MessageBoxButtons.OK);
                return;
            }
            Plugin _plugin = new Plugin(m_pluginManager, _pluginFileObject.PluginName);
            if (System.IO.File.Exists(_plugin.PluginPath))
            {
                DialogResult _result = MessageBox.Show($"A plugin with the name {_pluginFileObject.PluginName} does already exist. Do you want to replace it with a default one?"
                    , "Plugin already exists", MessageBoxButtons.YesNo);
                if (_result == DialogResult.Yes)
                {
                    System.IO.Directory.Delete(_plugin.PluginPath, true);
                }
                else
                {
                    return;
                }
            }
            _plugin.PluginFileObject = _pluginFileObject;
            System.IO.Directory.CreateDirectory(_plugin.PluginPath);

            Utils.CreateModule(_plugin, _pluginFileObject.PluginName);
            _plugin.SaveUPluginFile();
            _ = m_pluginManager.AddPlugin(_plugin);
        }

        private PluginManager m_pluginManager;

    }
}
