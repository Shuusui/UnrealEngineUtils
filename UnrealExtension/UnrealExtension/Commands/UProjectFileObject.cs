using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealExtension.Commands
{
    public class Module
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string LoadingPhase { get; set; }
    }
    public class Plugin
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public List<string> SupportedTargetPlatforms { get; set; }
    }
    public class UProjectFileObject
    {
        public string FileVersion { get; set; }
        public string EngineAssociation { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public List<Module> Modules { get; set; }
        public List<Plugin> Plugins { get; set; }
    }
}
