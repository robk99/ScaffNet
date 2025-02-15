using System.Diagnostics;
using Logger = ScaffNet.Utils.ScaffLogger;

namespace ScaffNet.Utils
{
    public record RunCommandArgs()
    {
        public string Command { get; set; }
        public string Arguments { get; set; }
        public string? Message { get; set; }
    }

    internal static class Commander
    {
        internal static void RunCommand(RunCommandArgs args, string solutionPath)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = args.Command,
                Arguments = args.Arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using Process process = new Process { StartInfo = psi };

            process.OutputDataReceived += (sender, e) => { if (!string.IsNullOrEmpty(e.Data)) Logger.Default.LogDebug(e.Data); };
            process.ErrorDataReceived += (sender, e) => { if (!string.IsNullOrEmpty(e.Data)) Logger.Default.LogError(e.Data); };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            // TODO: Build global exception handler and deleting inside it and here just throw an exception

            if (process.ExitCode != 0)
            {
                Logger.Default.LogWarning("\nRollbacking all the changes since there were errors!");

                DirectoryInfo directoryInfo = new DirectoryInfo(solutionPath);

                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
                {
                    dir.Delete(true);
                }

                Environment.Exit(1);
            }
        }
    }
}
