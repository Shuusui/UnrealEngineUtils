using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealExtension
{
    public class GenerateSourceFile : IGenerateFile
    {
        public GenerateSourceFile(string moduleName)
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
                $"#include \"{ModuleName}.h\"",
                string.Empty,
                $"#define LOCTEXT_NAMESPACE \"F{ModuleName}Module\"",
                string.Empty,
                $"void F{ModuleName}Module::StartupModule()",
                "{",
                $"\t// This code will execute after your module is loaded into memory; the exact timing is specified in the .uplugin file per-module",
                "}",
                string.Empty,
                $"void F{ModuleName}Module::ShutdownModule()",
                "{",
                "\t// This function may be called during shutdown to clean up your module.  For modules that support dynamic reloading,",
                "\t// we call this function before unloading the module.",
                "}",
                string.Empty,
                "#undef LOCTEXT_NAMESPACE",
                string.Empty,
                $"IMPLEMENT_MODULE(F{ModuleName}Module, {ModuleName})"
            };
        }
    }
}
