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
    public partial class AddPluginWindow : BaseDialogWindow, INotifyPropertyChanged
    {
        public AddPluginWindow(PluginManager pluginManager)
        {
            m_pluginManager = pluginManager;
            InitializeComponent();
        }
        public event PropertyChangedEventHandler PropertyChanged;
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
        private string m_pluginName = string.Empty;
        public string PluginName
        {
            get { return m_pluginName; }
            set
            {
                SetPropertyValue(ref m_pluginName, value);
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
    }
}
