using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealExtension
{
    public class UProjectFileInfo
    {
        public string DirPath { get; set; }
        public string ProjectName { get; set; }
        public string ProjectFilePath { get; set; }
        public string PluginsDir
        {
            get
            {
                return System.IO.Path.Combine(DirPath, "Plugins");
            }
        }
    }
}
