﻿using ScaffNet.Utils.ExceptionHandling;
using ScaffNet.Utils.MessageHandling;
using System.Diagnostics;

using EventHandler = ScaffNet.Utils.EventHandling.ScaffEventHandler;

namespace ScaffNet.Utils
{
    public record RunCommandArgs()
    {
        public string Command { get; set; }
        public string Arguments { get; set; }
        public string? SolutionPath { get; set; }
    }

    internal static class Commander
    {
        internal static void RunCommand(RunCommandArgs args)
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
            string errorOutput = "";

            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    // catching dotnet build errors
                    if (e.Data.Contains("error CS", StringComparison.OrdinalIgnoreCase))
                        errorOutput += e.Data;
                    else EventHandler.Default.OnDebug(e.Data);
                }
            };
            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    errorOutput += e.Data;
            };

            try
            {
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                errorOutput += ex.Message;
            }


            if (process.ExitCode != 0 || !string.IsNullOrEmpty(errorOutput))
            {
                var fullCommand = $"{args.Command} {args.Arguments}";
                EventHandler.Default.OnError(ErrorMessages.CommandError(fullCommand, errorOutput));
                throw new ScaffNetCommandException(fullCommand, errorOutput);
            }
        }
    }
}
