using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UnrealExtension
{
    public class Module : INotifyPropertyChanged
    {
        public Module(Plugin associatedPlugin)
        {
            m_associatedPlugin = associatedPlugin;
        }
        private Plugin m_associatedPlugin;
        public Plugin AssociatedPlugin
        {
            get { return m_associatedPlugin; }
            set
            {
                SetPropertyValue(ref m_associatedPlugin, value);
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
        public event PropertyChangedEventHandler PropertyChanged;
        protected void SetPropertyValue<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (value == null ? field == null : value.Equals(field))
            {
                return;
            }
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
