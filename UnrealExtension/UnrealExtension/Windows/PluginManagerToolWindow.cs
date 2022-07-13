using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;

namespace UnrealExtension.Windows
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("580a506d-c2d0-46a4-a82b-760ca6b7cfb1")]
    public class PluginManagerToolWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginManagerToolWindow"/> class.
        /// </summary>
        public PluginManagerToolWindow() : base(null)
        {
            this.Caption = "Plugin Manager";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new PluginManagerToolWindowControl();
        }
    }
}
