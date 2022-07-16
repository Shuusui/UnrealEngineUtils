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
        private UProjectFileObject m_projectFileObject = null;
        public UProjectFileObject ProjectFileObject
        {
            get
            {
                if (m_projectFileObject == null && !string.IsNullOrEmpty(ProjectFilePath))
                {
                    try
                    {
                        using (System.IO.StreamReader _reader = new System.IO.StreamReader(ProjectFilePath))
                        {
                            m_projectFileObject = Newtonsoft.Json.JsonConvert.DeserializeObject<UProjectFileObject>(_reader.ReadToEnd());
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                return m_projectFileObject;
            }
            set
            {
                m_projectFileObject = value;
            }
        }
    }
}
