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

    public class OpenAddPluginWindowCommand : ICommand
    {
        private class PluginInfo
        {
            public string PluginDirName { get; set; }
            public int UPluginFileAmount { get; set; }

            public static bool operator ==(PluginInfo lhs, PluginInfo rhs)
            {
                return lhs.UPluginFileAmount == rhs.UPluginFileAmount
                    && lhs.PluginDirName == rhs.PluginDirName;
            }
            public static bool operator !=(PluginInfo lhs, PluginInfo rhs)
            {
                return lhs.UPluginFileAmount != rhs.UPluginFileAmount
                    || lhs.PluginDirName != rhs.PluginDirName;
            }
            public override bool Equals(object obj)
            {
                return obj is PluginInfo _info ? this == _info : base.Equals(obj);
            }
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
        public OpenAddPluginWindowCommand(PluginManager pluginManager)
        {
            m_pluginManager = pluginManager ?? throw new NotSupportedException("Cannot execute add plugin command if plugin manager is null");
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            AddPluginWindow _addPluginWindow = new AddPluginWindow(m_pluginManager);
            PluginInfo[] _preAddWindowClosedPlugins = GetPlugins();
            _addPluginWindow.ShowDialog();
            if (!IsSame(_preAddWindowClosedPlugins, GetPlugins()))
            {
                m_pluginManager.UpdatePluginsList();
            }
        }
        private bool IsSame(PluginInfo[] prePlugins, PluginInfo[] postPlugins)
        {
            if (prePlugins.Length != postPlugins.Length)
            {
                return false;
            }
            for (int i = 0; i < prePlugins.Length; ++i)
            {
                if (prePlugins[i] != postPlugins[i])
                {
                    return false;
                }
            }
            return true;
        }
        private PluginInfo[] GetPlugins()
        {
            if (!System.IO.Directory.Exists(m_pluginManager.SelectedProject.PluginsDir))
            {
                return new PluginInfo[0];
            }
            string[] _directories = System.IO.Directory.GetDirectories(m_pluginManager.SelectedProject.PluginsDir).OrderBy(str => str).ToArray();
            PluginInfo[] _pluginInfos = new PluginInfo[_directories.Length];
            for (int i = 0; i < _directories.Length; ++i)
            {
                int _upluginFileAmount = System.IO.Directory.GetFiles(_directories[i], "*.uplugin").Length;
                _pluginInfos[i] = new PluginInfo
                {
                    PluginDirName = System.IO.Path.GetDirectoryName(_directories[i]),
                    UPluginFileAmount = _upluginFileAmount
                };
            }
            return _pluginInfos;
        }

        private PluginManager m_pluginManager;
    }
}
