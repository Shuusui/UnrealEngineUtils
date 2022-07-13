using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealExtension
{
    public class PluginObject
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public List<string> SupportedTargetPlatforms { get; set; }
    }
}
