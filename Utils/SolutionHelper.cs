using ScaffNet.Features;
using Logger = ScaffNet.Utils.ScaffLogger;

namespace ScaffNet.Utils
{
    internal class SolutionHelper
    {
        internal static void BuildSolution(FileSystemArgs args)
        {
            Commander.RunCommand(
                new RunCommandArgs()
                {
                    Command = "dotnet",
                    Arguments = $"build \"{args.SolutionPath}\"",
                    SolutionPath = args.SolutionPath
                });

            Logger.Default.LogInfo("Solution BUILT!");
        }
    }
}
