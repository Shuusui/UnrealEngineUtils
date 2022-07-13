using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealExtension
{
    public class UPluginFileObject
    {
        public int FileVersion { get; set; } = 3;
        public int Version { get; set; } = 1;
        public string VersionName { get; set; } = "1.0";
        public string FriendlyName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public string CreatedByURL { get; set; } = string.Empty;
        public string DocsURL { get; set; } = string.Empty;
        public string MarketplaceURL { get; set; } = string.Empty;
        public string SupportURL { get; set; } = string.Empty;
        public bool CanContainContent { get; set; } = false;
        public bool IsBetaVersion { get; set; } = false;
        public bool IsExperimentalVersion { get; set; } = false;
        public bool Installed { get; set; } = false;
        public List<ModuleObject> Modules { get; set; } = new List<ModuleObject>();
        public List<PluginObject> Plugins { get; set; } = new List<PluginObject>();
    }
}
