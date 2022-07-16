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
    /// Interaction logic for AddPluginWindow.xaml
    /// </summary>
    public partial class AddPluginWindow : NotifiableDialogWindow
    {
        public AddPluginWindow(PluginManager pluginManager)
        {
            m_pluginManager = pluginManager;
            InitializeComponent();
        }
        private ICommand m_addPluginCommand;
        public ICommand AddPluginCommand
        {
            get
            {
                if (m_addPluginCommand == null)
                {
                    m_addPluginCommand = new AddPluginCommand(m_pluginManager);
                }
                return m_addPluginCommand;
            }
        }
        private UPluginFileObject m_pluginInformation = new UPluginFileObject();
        public UPluginFileObject PluginInformation
        {
            get { return m_pluginInformation; }
            set
            {
                SetPropertyValue(ref m_pluginInformation, value);
            }
        }
        private PluginManager m_pluginManager;
    }
}
