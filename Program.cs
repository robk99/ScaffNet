using Microsoft.Extensions.Logging;
using ScaffNet.Features.CleanArchitecture;
using ScaffNet.Utils;
using ScaffNet.Utils.ErrorHandling;

internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No arguments provided");
                return;
            }

            if (args[0] == "arch-cl")
            {
                var solutonName = args.Length > 1 ? args[1] : Constants.SOLUTION_NAME;
                var solutionPath = args.Length > 2 ? args[2] : Constants.SOLUTION_PATH;

                //CleanArchitectureScaffolder.Create(new CleanArchitectureArgs
                //{
                //    SolutionName = solutonName,
                //    SolutionPath = args.Length > 2 ? args[2] : Constants.SOLUTION_PATH,
                //    SourceFolder = "src"
                //});

                TestingCLI.Test(solutonName, solutionPath);
            }
            else
            {
                Console.WriteLine("Wrong argument.");
            }
        }
        catch (ScaffNetCommandException ex)
        {
            // clients will handle this further
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            ScaffLogger.Default.LogError("Unhandled exception: " + ex.Message);
            throw new Exception("Unhandled error happened!");
            // clients will handle this further
        }
    }
}

public class CLILogger : ScaffLogger
{
    private ILogger<CLILogger> _logger { get; set; }

    public CLILogger(ILogger<CLILogger> logger, LogLevel minimalLevel) : base(minimalLevel)
    {
        _logger = logger;
    }

    sealed protected override void LogDebugBehaviour(string message) =>
        _logger.LogDebug($"[DEBUG] {message}");

    sealed protected override void LogInfoBehaviour(string message) =>
        _logger.LogInformation($"[INFO] {message}");

    sealed protected override void LogWarningBehaviour(string message) =>
        _logger.LogWarning($"[WARNING] {message}");

    sealed protected override void LogErrorBehaviour(string message) =>
        _logger.LogError($"[ERROR] {message}");

    sealed protected override void LogCriticalBehaviour(string message) =>
        _logger.LogCritical($"[CRITICAL] {message}");
}

public static class TestingCLI
{
    public static void Test(string solutionName, string solutionPath)
    {
        var minimalLevel = LogLevel.Information;
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
            .SetMinimumLevel(minimalLevel)
            .AddConsole();
        });

        var myCliLogger = new CLILogger(loggerFactory.CreateLogger<CLILogger>(), minimalLevel);

        Utility.SetLogger(myCliLogger);

        CleanArchitectureScaffolder.Create(new CleanArchitectureArgs
        {
            SolutionName = solutionName,
            SolutionPath = solutionPath,
            SourceFolder = "src"
        });
    }
}