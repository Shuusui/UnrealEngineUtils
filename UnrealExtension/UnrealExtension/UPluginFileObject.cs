using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UnrealExtension
{
    public class UPluginFileObject : NotifiableProperty
    {
        private bool m_friendlyNameEdited = false;
        public int FileVersion { get; set; } = 3;
        public int Version { get; set; } = 1;
        public string VersionName { get; set; } = "1.0";

        private string m_pluginName = string.Empty;
        public string PluginName
        {
            get { return m_pluginName; }
            set
            {
                bool _moduleNameEdited = false;
                if (Modules.Count > 0)
                {
                    if (Modules[0].Name != m_pluginName)
                    {
                        _moduleNameEdited = true;
                    }
                }
                SetPropertyValue(ref m_pluginName, value);
                if (!m_friendlyNameEdited)
                {
                    FriendlyName = m_pluginName;
                }
                if (m_friendlyName == m_pluginName)
                {
                    m_friendlyNameEdited = false;
                }
                if (Modules.Count > 0 && !_moduleNameEdited)
                {
                    Modules[0].Name = m_pluginName;
                }
            }
        }
        private string m_friendlyName = string.Empty;
        public string FriendlyName
        {
            get { return m_friendlyName; }
            set
            {
                SetPropertyValue(ref m_friendlyName, value);
                if (m_friendlyName != m_pluginName)
                {
                    m_friendlyNameEdited = true;
                }
                else
                {
                    m_friendlyNameEdited = false;
                }
            }
        }
        private string m_description = string.Empty;
        public string Description
        {
            get { return m_description; }
            set
            {
                SetPropertyValue(ref m_description, value);
            }
        }
        private string m_category = "Other";
        public string Category
        {
            get { return m_category; }
            set
            {
                SetPropertyValue(ref m_category, value);
            }
        }
        private string m_createdBy = string.Empty;
        public string CreatedBy
        {
            get { return m_createdBy; }
            set
            {
                SetPropertyValue(ref m_createdBy, value);
            }
        }
        private string m_createdByURL = string.Empty;
        public string CreatedByURL
        {
            get { return m_createdByURL; }
            set
            {
                SetPropertyValue(ref m_createdByURL, value);
            }
        }
        private string m_docsURL = string.Empty;
        public string DocsURL
        {
            get { return m_docsURL; }
            set
            {
                SetPropertyValue(ref m_docsURL, value);
            }
        }
        private string m_marketplaceURL = string.Empty;
        public string MarketplaceURL
        {
            get { return m_marketplaceURL; }
            set
            {
                SetPropertyValue(ref m_marketplaceURL, value);
            }
        }
        private string m_supportURL = string.Empty;
        public string SupportURL
        {
            get { return m_supportURL; }
            set
            {
                SetPropertyValue(ref m_supportURL, value);
            }
        }
        private bool m_canContainContent = false;
        public bool CanContainContent
        {
            get { return m_canContainContent; }
            set
            {
                SetPropertyValue(ref m_canContainContent, value);
            }
        }
        private bool m_isBetaVersion = false;
        public bool IsBetaVersion
        {
            get { return m_isBetaVersion; }
            set
            {
                SetPropertyValue(ref m_isBetaVersion, value);
            }
        }
        private bool m_isExperimentalVersion = false;
        public bool IsExperimentalVersion
        {
            get { return m_isExperimentalVersion; }
            set
            {
                SetPropertyValue(ref m_isExperimentalVersion, value);
            }
        }
        private bool m_installed = false;
        public bool Installed
        {
            get { return m_installed; }
            set
            {
                SetPropertyValue(ref m_installed, value);
            }
        }
        public List<ModuleObject> Modules { get; set; } = new List<ModuleObject>() { new ModuleObject() };
        public List<PluginObject> Plugins { get; set; } = new List<PluginObject>();
    }
}
