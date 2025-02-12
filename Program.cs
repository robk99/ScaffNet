using System.Diagnostics;

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
            CreateCleanArchitecture();
        }
        else
        {
            Console.WriteLine("Wrong argument.");
        }

    }

    private static string? _solutionPath { get; set; } = "";
    private static string? _sourceFolder { get; set; } = "";

    public record RunCommandArgs()
    {
        public string Command { get; set; }
        public string Arguments { get; set; }
        public string? Message { get; set; }
        public bool ShowDefaultOutputMessages { get; set; } = false;
    }

    static void CreateCleanArchitecture()
    {
        string solutionName = "MyCleanArchitectureApp";

        // TODO: If you don't pick a folder, it will create the solution in the current directory
        //string solutionPath = Path.Combine(Directory.GetCurrentDirectory(), solutionName);

        _solutionPath = @"C:\ScaffNet";
        _sourceFolder = "/src";

        Console.WriteLine($"Creating Clean Architecture solution: {solutionName}");

        // Create the solution
        RunCommand(new RunCommandArgs() { Command = "dotnet", Arguments = $"new sln -n {solutionName} -o \"{_solutionPath}\"" });


        // Create project directories
        string[] projects = { "Domain", "Application", "Infrastructure", "Web.Api" };
        foreach (string project in projects)
        {
            string projectPath = Path.Combine(_solutionPath, _sourceFolder, project);
            Directory.CreateDirectory(projectPath);
        }

        // Create class library projects
        RunCommand(new RunCommandArgs() { Command = "dotnet", Arguments = $"new classlib -n {solutionName}.Domain -o \"{_solutionPath}{_sourceFolder}/Domain\"" });
        RunCommand(new RunCommandArgs() { Command = "dotnet", Arguments = $"new classlib -n {solutionName}.Application -o \"{_solutionPath}{_sourceFolder}/Application\"" });
        RunCommand(new RunCommandArgs() { Command = "dotnet", Arguments = $"new classlib -n {solutionName}.Infrastructure -o \"{_solutionPath}{_sourceFolder}/Infrastructure\"" });

        // Create Web Web.Api project
        RunCommand(new RunCommandArgs() { Command = "dotnet", Arguments = $"new webapi -n {solutionName}.Web.Api -o \"{_solutionPath}{_sourceFolder}/Web.Api\"" });

        // Add projects to solution
        RunCommand(new RunCommandArgs() { Command = "dotnet", Arguments = $"sln \"{_solutionPath}/{solutionName}.sln\" add \"{_solutionPath}{_sourceFolder}/Domain/{solutionName}.Domain.csproj\"" });
        RunCommand(new RunCommandArgs() { Command = "dotnet", Arguments = $"sln \"{_solutionPath}/{solutionName}.sln\" add \"{_solutionPath}{_sourceFolder}/Application/{solutionName}.Application.csproj\"" });
        RunCommand(new RunCommandArgs() { Command = "dotnet", Arguments = $"sln \"{_solutionPath}/{solutionName}.sln\" add \"{_solutionPath}{_sourceFolder}/Infrastructure/{solutionName}.Infrastructure.csproj\"" });
        RunCommand(new RunCommandArgs() { Command = "dotnet", Arguments = $"sln \"{_solutionPath}/{solutionName}.sln\" add \"{_solutionPath}{_sourceFolder}/Web.Api/{solutionName}.Web.Api.csproj\"" });

        // Add project references
        RunCommand(new RunCommandArgs() { Command = "dotnet", Arguments = $"add \"{_solutionPath}{_sourceFolder}/Application/{solutionName}.Application.csproj\" reference \"{_solutionPath}{_sourceFolder}/Domain/{solutionName}.Domain.csproj\"" });
        RunCommand(new RunCommandArgs() { Command = "dotnet", Arguments = $"add \"{_solutionPath}{_sourceFolder}/Infrastructure/{solutionName}.Infrastructure.csproj\" reference \"{_solutionPath}{_sourceFolder}/Application/{solutionName}.Application.csproj\"" });
        RunCommand(new RunCommandArgs() { Command = "dotnet", Arguments = $"add \"{_solutionPath}{_sourceFolder}/Infrastructure/{solutionName}.Infrastructure.csproj\" reference \"{_solutionPath}{_sourceFolder}/Domain/{solutionName}.Domain.csproj\"" });
        RunCommand(new RunCommandArgs() { Command = "dotnet", Arguments = $"add \"{_solutionPath}{_sourceFolder}/Web.Api/{solutionName}.Web.Api.csproj\" reference \"{_solutionPath}{_sourceFolder}/Application/{solutionName}.Application.csproj\"" });
        RunCommand(new RunCommandArgs() { Command = "dotnet", Arguments = $"add \"{_solutionPath}{_sourceFolder}/Web.Api/{solutionName}.Web.Api.csproj\" reference \"{_solutionPath}{_sourceFolder}/Infrastructure/{solutionName}.Infrastructure.csproj\"" });


        DeleteRedundantFiles();

        //AddDI();


        // Restore dependencies
        RunCommand(new RunCommandArgs() { Command = "dotnet", Arguments = $"restore \"{_solutionPath}/{solutionName}.sln\"" });

        Console.WriteLine("Clean Architecture solution successfully created!");
    }

    static void DeleteRedundantFiles()
    {
        Console.WriteLine("\n--------- Deleting redundant files: ---------\n");
        DeleteAllFileInstances("Class1.cs");
    }

    static void AddDI()
    {
        var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "templates/clean-architecture/", "DependencyInjection.tpl");
        string[] projects = { "Application", "Infrastructure" };

        foreach (string project in projects)
        {
            string projectPath = Path.Combine(_solutionPath, _sourceFolder, project);


            // TODO: Finish this. targetFilePath is not correct!
            string targetFilePath = Path.Combine(_solutionPath, projectPath, "DependencyInjection.cs");

            try
            {
                string content = File.ReadAllText(templatePath);
                content = content.Replace("REPLACEME", $"MyCleanArchitectureApp.{project}");

                File.WriteAllText(targetFilePath, content);
                Console.WriteLine($"Created: {targetFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing {project}: {ex.Message}");
            }

            // TODO: Add DI code to Web.Api's Program.cs
        }
    }

    static void DeleteAllFileInstances(string fileName)
    {
        if (!Directory.Exists(_solutionPath))
        {
            Console.WriteLine($"Path not found: {_solutionPath}");
            return;
        }

        try
        {
            var files = Directory.GetFiles(_solutionPath, fileName, SearchOption.AllDirectories);

            foreach (var file in files)
            {
                Console.WriteLine($"Deleting: {file}");
                File.Delete(file);
            }

            Console.WriteLine("Deletion complete.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting files: {ex.Message}");
        }
    }

    static void RunCommand(RunCommandArgs args)
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

        if (args.ShowDefaultOutputMessages) process.OutputDataReceived += (sender, e) => { if (!string.IsNullOrEmpty(e.Data)) Console.WriteLine(e.Data); };
        process.ErrorDataReceived += (sender, e) => { if (!string.IsNullOrEmpty(e.Data)) Console.WriteLine("ERROR: " + e.Data); };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();

        // TODO: Build global exception handler and deleting inside it and here just throw an exception

        if (process.ExitCode != 0)
        {
            Console.WriteLine("\nRollbacking all the changes since there were errors!");


            DirectoryInfo di = new DirectoryInfo(_solutionPath);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }

            Environment.Exit(1);
        }
    }
}