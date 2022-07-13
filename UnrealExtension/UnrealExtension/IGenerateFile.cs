using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealExtension
{
    public interface IGenerateFile
    {
        string ModuleName { get; set; }
        string[] GetGeneratedFileContent();
    }
}
