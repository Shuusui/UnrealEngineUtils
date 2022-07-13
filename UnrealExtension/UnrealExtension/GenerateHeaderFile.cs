using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealExtension
{
    public class GenerateHeaderFile : IGenerateFile
    {
        public GenerateHeaderFile(string moduleName)
        {
            ModuleName = moduleName;
        }
        public string ModuleName { get; set; }

        public string[] GetGeneratedFileContent()
        {
            return new string[]
            {
                "// Copyright . All Rights Reserved.",
                string.Empty,
                "#pragma once",
                string.Empty,
                "#include \"CoreMinimal.h\"",
                "#include \"Modules/ModuleManager.h\"",
                string.Empty,
                $"class F{ModuleName}Module : public IModuleInterface",
                "{",
                "public:",
                string.Empty,
                $"\t/** IModuleInterface implementation */",
                $"\tvirtual void StartupModule() override;",
                $"\tvirtual void ShutdownModule() override;",
                "};"
            };
        }
    }
}
