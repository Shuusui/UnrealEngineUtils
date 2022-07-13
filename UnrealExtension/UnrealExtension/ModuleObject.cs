using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealExtension
{
    public class ModuleObject
    {
        public string Name { get; set; }
        public string Type { get; set; } = "Runtime";
        public string LoadingPhase { get; set; } = "Default";
    }
}
