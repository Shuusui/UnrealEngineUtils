using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealExtension.Commands
{
    public class Installation
    {
        public string InstallLocation { get; set; }
        public string NamespaceId { get; set; }
        public string ItemId { get; set; }
        public string ArtifactId { get; set; }
        public string AppVersion { get; set; }
        public string AppName { get; set; }
    }
    public class LauncherInstalledObject
    {
        public List<Installation> InstallationList { get; set; } = new List<Installation>();
    }
}
