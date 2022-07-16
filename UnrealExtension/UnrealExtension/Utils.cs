using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnrealExtension.Commands;

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
            plugin.AddModule(moduleName);
        }
        public enum EnginePathError
        {
            None,
            GUIDPathNotFound,
            InstalledEngineNotFound
        }
        public static string GetEnginePath(UProjectFileObject uprojectFileObject, out string errorMsg, out EnginePathError enginePathError)
        {
            errorMsg = string.Empty;
            enginePathError = EnginePathError.None;
            string _enginePath = string.Empty;
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                if (Guid.TryParse(uprojectFileObject.EngineAssociation, out Guid _))
                {
                    const string REGISTRY_ENTRY_PATH = "HKEY_CURRENT_USER\\SOFTWARE\\Epic Games\\Unreal Engine\\Builds";
                    _enginePath = Microsoft.Win32.Registry.GetValue(REGISTRY_ENTRY_PATH, uprojectFileObject.EngineAssociation, "").ToString();
                    if (string.IsNullOrEmpty(_enginePath))
                    {
                        errorMsg = "Under the associated unreal engine GUID, we could not find any install location registered in registry. Make sure you've installed the source build properly by following instructions on the github repository of the engine";
                        enginePathError = EnginePathError.GUIDPathNotFound;
                        return _enginePath;
                    }
                }
                else
                {
                    string _programDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                    string _launcherInstalledPath = System.IO.Path.Combine(_programDataFolderPath, "Epic", "UnrealEngineLauncher", "LauncherInstalled.dat");
                    if (System.IO.File.Exists(_launcherInstalledPath))
                    {
                        using (System.IO.StreamReader _launcherInstalledReader = new System.IO.StreamReader(_launcherInstalledPath))
                        {
                            string _launcherInstalledJson = _launcherInstalledReader.ReadToEnd();
                            LauncherInstalledObject _launcherInstalledObject = JsonConvert.DeserializeObject<LauncherInstalledObject>(_launcherInstalledJson);
                            foreach (Installation _installation in _launcherInstalledObject.InstallationList)
                            {
                                if (_installation.NamespaceId == "ue" && _installation.AppName.Contains(uprojectFileObject.EngineAssociation))
                                {
                                    _enginePath = _installation.InstallLocation;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        errorMsg = "There is no unreal engine version registered. Make sure you've installed an unreal engine version properly";
                        enginePathError = EnginePathError.InstalledEngineNotFound;
                        return _enginePath;
                    }
                }
            }
            return _enginePath;
        }
    }
}
