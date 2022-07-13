using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using UnrealExtension.Commands.ControlCommands;

namespace UnrealExtension.Windows
{


    /// <summary>
    /// Interaction logic for PluginManagerToolWindowControl.
    /// </summary>
    public partial class PluginManagerToolWindowControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginManagerToolWindowControl"/> class.
        /// </summary>
        public PluginManagerToolWindowControl()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            PluginManagerInstance = new PluginManager();
            InitializeComponent();
            DataContext = PluginManagerInstance;

            PluginManagerInstance.Solution = (IVsSolution)Package.GetGlobalService(typeof(IVsSolution));
        }
        public PluginManager PluginManagerInstance { get; set; }
    }
}
