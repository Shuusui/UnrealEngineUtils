using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UnrealExtension.Commands.ControlCommands;
using UnrealExtension.Windows.DialogWindows;

namespace UnrealExtension.Windows.ControlWindows
{
    /// <summary>
    /// Interaction logic for AddModuleWindow.xaml
    /// </summary>
    public partial class AddModuleWindow : NotifiableDialogWindow
    {
        public AddModuleWindow(PluginManager pluginManager)
        {
            m_pluginManager = pluginManager;
            InitializeComponent();
        }
        private ICommand m_addModuleCommand;
        public ICommand AddModuleCommand
        {
            get
            {
                if (m_addModuleCommand == null)
                {
                    m_addModuleCommand = new AddModuleCommand(m_pluginManager);
                }
                return m_addModuleCommand;
            }
        }
        private string m_moduleName;
        public string ModuleName
        {
            get { return m_moduleName; }
            set
            {
                SetPropertyValue(ref m_moduleName, value);
            }
        }
        private PluginManager m_pluginManager;
    }
}
