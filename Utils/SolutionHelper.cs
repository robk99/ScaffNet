using ScaffNet.Features;
using EventHandler = ScaffNet.Utils.EventHandling.ScaffEventHandler;

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

            EventHandler.Default.OnInfo("Solution BUILT!");
        }
    }
}
