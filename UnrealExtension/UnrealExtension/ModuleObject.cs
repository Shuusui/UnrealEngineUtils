using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UnrealExtension
{
    public class ModuleObject : NotifiableProperty
    {
        private string m_name = string.Empty;
        public string Name
        {
            get { return m_name; }
            set
            {
                SetPropertyValue(ref m_name, value);
            }
        }
        private string m_type = "Runtime";
        public string Type
        {
            get { return m_type; }
            set
            {
                SetPropertyValue(ref m_type, value);
            }
        }
        private string m_loadingPhase = "Default";
        public string LoadingPhase
        {
            get { return m_loadingPhase; }
            set
            {
                SetPropertyValue(ref m_loadingPhase, value);
            }
        }
        private List<string> m_whitelistPlatforms = new List<string>();
        public List<string> WhitelistPlatforms
        {
            get { return m_whitelistPlatforms; }
            set
            {
                SetPropertyValue(ref m_whitelistPlatforms, value);
            }
        }
        private List<string> m_blacklistPlatforms = new List<string>();
        public List<string> BlacklistPlatforms
        {
            get { return m_blacklistPlatforms; }
            set
            {
                SetPropertyValue(ref m_blacklistPlatforms, value);
            }
        }
        [JsonIgnore]
        public List<string> AvailableTypes { get; } = new List<string>
        {
            "Runtime",
            "RuntimeNoCommandlet",
            "RuntimeAndProgram",
            "CookedOnly",
            "UncookedOnly",
            "Developer",
            "DeveloperTool",
            "Editor",
            "EditorNoCommandlet",
            "EditorAndProgram",
            "Program",
            "ServerOnly",
            "ClientOnly",
            "ClientOnlyNoCommandlet"
        };
        [JsonIgnore]
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
        [JsonIgnore]
        public List<string> AvailablePlatforms { get; } = new List<string>
        {
            "Win64",
            "HoloLens",
            "Mac",
            "IOS",
            "Android",
            "Linux",
            "LinuxArm64",
            "TVOS"
        };
    }
}
