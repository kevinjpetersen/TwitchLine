using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TwitchLine.Data
{
    public class TwitchLineService
    {
        protected Process CmdProcess { get; set; }
        public CancellationTokenSource CTS { get; set; } = new CancellationTokenSource();

        public Action<string, string> CommandFinishedEvent { get; set; }
        public bool ProcessStarted { get; set; } = false;

        public TwitchLineService()
        {
            CmdProcess = new Process();
            CmdProcess.StartInfo.FileName = "cmd.exe";
            CmdProcess.StartInfo.RedirectStandardInput = true;
            CmdProcess.StartInfo.RedirectStandardOutput = true;
            CmdProcess.StartInfo.RedirectStandardError = true;
            CmdProcess.StartInfo.CreateNoWindow = true;
            CmdProcess.StartInfo.UseShellExecute = false;
        }

        public void KillProcess()
        {
            if(ProcessStarted) CmdProcess.Kill();
        }

        public bool ExecuteCommand(string command)
        {
            if (CommandFinishedEvent == null) return false;

            try
            {
                Task.Run(async () =>
                {
                    ProcessStarted = CmdProcess.Start();

                    await CmdProcess.StandardInput.WriteLineAsync(command);
                    await CmdProcess.StandardInput.WriteLineAsync("EXIT");

                    CommandFinishedEvent.Invoke(await CmdProcess.StandardOutput.ReadToEndAsync(), await CmdProcess.StandardError.ReadToEndAsync());
                    CmdProcess.Close();
                    ProcessStarted = false;
                });
            } catch
            {
                CommandFinishedEvent.Invoke(null, null);
                CmdProcess.Close();
                ProcessStarted = false;
            }

            return true;
        }
    }
}
