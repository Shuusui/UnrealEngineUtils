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
    }
}
