using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnrealExtension
{
    public class Utils
    {
        public static System.Diagnostics.Process RunProgramm(
            string programFile,
            string args, EventHandler onExit = null,
            System.Diagnostics.DataReceivedEventHandler onOutputDataReceived = null,
            System.Diagnostics.DataReceivedEventHandler onErrorReceived = null,
            bool waitForCompletion = false)
        {
            System.Diagnostics.ProcessStartInfo _startInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = programFile,
                Arguments = args,
            };

            if (onOutputDataReceived != null)
            {
                _startInfo.RedirectStandardOutput = true;
                _startInfo.RedirectStandardInput = true;
                _startInfo.UseShellExecute = false;
                _startInfo.CreateNoWindow = true;
            }

            if (onErrorReceived != null)
            {
                _startInfo.RedirectStandardError = true;
            }

            System.Diagnostics.Process _programProcess;

            try
            {

                _programProcess = new System.Diagnostics.Process
                {
                    StartInfo = _startInfo
                };
                if (onExit != null || _startInfo.RedirectStandardOutput || _startInfo.RedirectStandardError)
                {
                    _programProcess.EnableRaisingEvents = true;
                }

                if (onExit != null)
                {
                    _programProcess.Exited += onExit;
                }

                if (_startInfo.RedirectStandardOutput)
                {
                    _programProcess.OutputDataReceived += onOutputDataReceived;
                }
                if (_startInfo.RedirectStandardError)
                {
                    _programProcess.ErrorDataReceived += onErrorReceived;
                }

                _programProcess.Start();

                if (_startInfo.RedirectStandardOutput)
                {
                    _programProcess.BeginOutputReadLine();
                }

                if (_startInfo.RedirectStandardError)
                {
                    _programProcess.BeginErrorReadLine();
                }

                if (waitForCompletion)
                {
                    while ((_programProcess != null) && (!_programProcess.HasExited))
                    {
                        _programProcess.WaitForExit(20);
                    }
                }
            }
            catch (Exception _e)
            {
                _programProcess = null;
            }

            return _programProcess;
        }

        public static void CreateModule(Plugin plugin, string moduleName)
        {
            if (!System.IO.Directory.Exists(plugin.PluginSourcePath))
            {
                System.IO.Directory.CreateDirectory(plugin.PluginSourcePath);
            }
            string _modulePath = System.IO.Path.Combine(plugin.PluginSourcePath, moduleName);
            System.IO.Directory.CreateDirectory(_modulePath);
            GenerateBuildCsFile _generateBuildCsFile = new GenerateBuildCsFile(moduleName);
            System.IO.File.WriteAllLines(
                System.IO.Path.Combine(
                    _modulePath, $"{moduleName}.Build.cs"),
                _generateBuildCsFile.GetGeneratedFileContent());

            string _publicModulePath = System.IO.Path.Combine(_modulePath, "Public");
            System.IO.Directory.CreateDirectory(_publicModulePath);
            GenerateHeaderFile _generateHeaderFile = new GenerateHeaderFile(moduleName);
            System.IO.File.WriteAllLines(
                System.IO.Path.ChangeExtension(
                    System.IO.Path.Combine(
                        _publicModulePath, moduleName),
                    ".h"),
                _generateHeaderFile.GetGeneratedFileContent());

            string _privateModulePath = System.IO.Path.Combine(_modulePath, "Private");
            System.IO.Directory.CreateDirectory(_privateModulePath);
            GenerateSourceFile _generateSourceFile = new GenerateSourceFile(moduleName);
            System.IO.File.WriteAllLines(
                System.IO.Path.ChangeExtension(
                    System.IO.Path.Combine(
                        _privateModulePath, moduleName),
                    ".cpp"),
                _generateSourceFile.GetGeneratedFileContent());
            plugin.PluginFileObject.Modules.Add(new ModuleObject
            {
                Name = moduleName
            });
            plugin.AddModule(moduleName);
        }
    }
}
