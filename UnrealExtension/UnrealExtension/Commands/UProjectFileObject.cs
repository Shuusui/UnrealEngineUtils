using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealExtension.Commands
{
    public class UProjectFileObject
    {
        public string FileVersion { get; set; }
        public string EngineAssociation { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public List<ModuleObject> Modules { get; set; }
        public List<PluginObject> Plugins { get; set; }
    }
}
