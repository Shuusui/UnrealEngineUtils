using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealExtension
{
    public class GenerateBuildCsFile : IGenerateFile
    {
        public GenerateBuildCsFile(string moduleName)
        {
            ModuleName = moduleName;
        }

        public string[] GetGeneratedFileContent()
        {
            const string _ot = "\t";
            const string _dt = "\t\t";
            const string _tt = "\t\t\t";
            const string _qt = "\t\t\t\t";
            return new string[]
            {
                "// Copyright . All Rights Reserved.",
                string.Empty,
                "using UnrealBuildTool;",
                string.Empty,
                $"public class {ModuleName} : ModuleRules",
                "{",
                $"{_ot}public {ModuleName}(ReadOnlyTargetRules Target) : base(Target)",
                $"{_ot}{{",
                $"{_dt}PCHUsage = ModuleRules.PCHUsageMode.UseExplicitOrSharedPCHs;",
                string.Empty,
                $"{_dt}PublicIncludePaths.AddRange(",
                $"{_tt}new string[] {{",
                $"{_qt}// ... add public include paths required here ...",
                $"{_tt}}}",
                $"{_tt});",
                string.Empty,
                string.Empty,
                $"{_dt}PrivateIncludePaths.AddRange(",
                $"{_tt}new string[] {{",
                $"{_qt}// ... add other private include paths required here ...",
                $"{_tt}}}",
                $"{_tt});",
                string.Empty,
                string.Empty,
                $"{_dt}PublicDependencyModuleNames.AddRange(",
                $"{_tt}new string[]{{",
                $"{_qt}\"Core\",",
                $"{_qt}// ... add other public dependencies that you statically link with here ...",
                $"{_tt}}}",
                $"{_tt});",
                string.Empty,
                string.Empty,
                $"{_dt}PrivateDependencyModuleNames.AddRange(",
                $"{_tt}new string[]{{",
                $"{_qt}\"CoreUObject\",",
                $"{_qt}\"Engine\"",
                $"{_qt}// ... add private dependencies that you statically link with here ...",
                $"{_tt}}}",
                $"{_tt});",
                string.Empty,
                string.Empty,
                $"{_dt}DynamicallyLoadedModuleNames.AddRange(",
                $"{_tt}new string[]{{",
                $"{_qt}// ... add any modules that your module loads dynamically here ...",
                $"{_tt}}}",
                $"{_tt});",
                $"{_ot}}}",
                "}"
            };
        }

        public string ModuleName { get; set; }
    }
}
