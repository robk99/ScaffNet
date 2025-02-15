using Microsoft.Extensions.Logging;
using ScaffNet.Features.CleanArchitecture;
using ScaffNet.Utils;

internal class Program
{
    private static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("No arguments provided");
            return;
        }

        if (args[0] == "clean-a")
        {
            //CleanArchitectureScaffolder.Create(new CleanArchitectureArgs
            //{
            //    SolutionName = "MY_FUNKY_SOLUTION",
            //    SolutionPath = "C:/ScaffNet/",
            //    SourceFolder = "src"
            //});

            TestingCLI.Test();
        }
        else
        {
            Console.WriteLine("Wrong argument.");
        }
    }
}


public class TestLogger : CLILogger
{
    public TestLogger(ILogger<CLILogger> logger, LogLevel minimallevel) : base(logger, minimallevel)
    {
        
    }

    public void LogDebug(string message) => base.LogDebugBehaviour(message);
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
        _logger.LogCritical($"[ERROR] {message}");
}

public static class TestingCLI
{
    public static void Test()
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
        });

        var myCliLogger = new CLILogger(loggerFactory.CreateLogger<CLILogger>(), LogLevel.Debug);

        Utilities.SetLogger(myCliLogger);

        CleanArchitectureScaffolder.Create(new CleanArchitectureArgs
        {
            SolutionName = "MY_FUNKY_SOLUTION22",
            SolutionPath = "C:/ScaffNet/",
            SourceFolder = "src"
        });
    }
}