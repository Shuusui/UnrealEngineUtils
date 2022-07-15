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
        public List<string> AvailableTypes { get; } = new List<string>
        {
            "Runtime",
            "Editor"
        };
        public List<string> AvailableLoadingPhase { get; } = new List<string>
        {

            "EarliestPossible",
            "PostConfigInit",
            "PostSplashScreen",
            "PreEarlyLoadingScreen",
            "PreLoadingScreen",
            "PreDefault",
            "Default",
            "PostDefault",
            "PostEngineInit",
            "None"
        };
    }
}
