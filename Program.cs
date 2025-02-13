using System.Diagnostics;

internal class Program
{
    private static readonly string _solutionPath = @"C:/ScaffNet/";
    private static readonly string _solutionName = "MyCleanArchitectureApp";
    private static readonly string _sourceFolder = "src";
    private static readonly string _app = "Application";
    private static readonly string _infra = "Infrastructure";
    private static readonly string _domain = "Domain";
    private static readonly string _webApi = "Web.Api";

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

    public record RunCommandArgs()
    {
        public string Command { get; set; }
        public string Arguments { get; set; }
        public string? Message { get; set; }
        public bool ShowDefaultOutputMessages { get; set; } = false;
    }

    static void CreateCleanArchitecture()
    {
        CreateSolutionAndProjects();
        DeleteRedundantDefaultFiles();
        AddDi();

        RunCommand(new RunCommandArgs() { Command = "dotnet", Arguments = $"restore \"{_solutionPath}/{_solutionName}.sln\"" });

        Console.WriteLine("Clean Architecture solution successfully created!");
    }

    static void CreateSolutionAndProjects()
    {
        Console.WriteLine($"Creating Clean Architecture solution: {_solutionName}");

        RunCommand(new RunCommandArgs() { Command = "dotnet", Arguments = $"new sln -n {_solutionName} -o \"{_solutionPath}\"" });

        string[] classLibraryProjects = { _domain, _app, _infra };

        foreach (string project in classLibraryProjects)
        {
            RunCommand(new RunCommandArgs() { Command = "dotnet", Arguments = $"new classlib -n {_solutionName}.{project} -o \"{Path.GetFullPath(Path.Combine(_solutionPath, _sourceFolder, project))}\"" });
            RunCommand(new RunCommandArgs() { Command = "dotnet", Arguments = $"sln \"{_solutionPath}/{_solutionName}.sln\" add \"{Path.GetFullPath(Path.Combine(_solutionPath, _sourceFolder, project, _solutionName))}.{project}.csproj\"" });
        }

        RunCommand(new RunCommandArgs() { Command = "dotnet", Arguments = $"new webapi -n {_solutionName}.{_webApi} -controllers -o \"{Path.GetFullPath(Path.Combine(_solutionPath, _sourceFolder, _webApi))}\"" });
        RunCommand(new RunCommandArgs() { Command = "dotnet", Arguments = $"sln \"{_solutionPath}/{_solutionName}.sln\" add \"{Path.GetFullPath(Path.Combine(_solutionPath, _sourceFolder, _webApi, _solutionName))}.{_webApi}.csproj\"" });

        var applicationCsProjFilePath = Path.GetFullPath(Path.Combine(_solutionPath, _sourceFolder, _app, _solutionName)) + $".{_app}.csproj";
        var infrastructureCsProjFilePath = Path.GetFullPath(Path.Combine(_solutionPath, _sourceFolder, _infra, _solutionName)) + $".{_infra}.csproj";
        var domainCsProjFilePath = Path.GetFullPath(Path.Combine(_solutionPath, _sourceFolder, _domain, _solutionName)) + $".{_domain}.csproj";
        var webApiCsProjFilePath = Path.GetFullPath(Path.Combine(_solutionPath, _sourceFolder, _webApi, _solutionName)) + $".{_webApi}.csproj";

        RunCommand(new RunCommandArgs() { Command = "dotnet", Arguments = $"add \"{applicationCsProjFilePath}\" reference \"{domainCsProjFilePath}\"" });
        RunCommand(new RunCommandArgs() { Command = "dotnet", Arguments = $"add \"{infrastructureCsProjFilePath}\" reference \"{applicationCsProjFilePath}\"" });
        RunCommand(new RunCommandArgs() { Command = "dotnet", Arguments = $"add \"{webApiCsProjFilePath}\" reference \"{infrastructureCsProjFilePath}\"" });
    }

    static void DeleteRedundantDefaultFiles()
    {
        Console.WriteLine("\n--------- Deleting redundant files: ---------\n");
        DeleteAllFileInstances("Class1.cs");
    }

    static void AddDi()
    {
        var applicationCsProjFilePath = Path.GetFullPath(Path.Combine(_solutionPath, _sourceFolder, _app, _solutionName)) + $".{_app}.csproj";
        var infrastructureCsProjFilePath = Path.GetFullPath(Path.Combine(_solutionPath, _sourceFolder, _infra, _solutionName)) + $".{_infra}.csproj";
        var templatePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "templates/clean-architecture/", "DependencyInjection.tpl"));
        string[] projectsForDi = { _app, _infra };
        var namespaceReplaceText = "REPLACEME_NS";
        var methodReplaceText = "REPLACEME_MT";
        var diNugetPackage = "Microsoft.Extensions.DependencyInjection";

        foreach (string project in projectsForDi)
        {
            RunCommand(new RunCommandArgs() { Command = "dotnet", Arguments = $"add \"{applicationCsProjFilePath}\" package \"{diNugetPackage}\"" });

            string content = File.ReadAllText(templatePath);
            content = content
                .Replace(namespaceReplaceText, $"{_solutionName}.{project}")
                .Replace(methodReplaceText, project);

            string targetFilePath = Path.GetFullPath(Path.Combine(_solutionPath, _sourceFolder, project, "DependencyInjection.cs"));
            File.WriteAllText(targetFilePath, content);
            Console.WriteLine($"Created: {targetFilePath}");
        }

        string programCsPath = $"{Path.GetFullPath(Path.Combine(_solutionPath, _sourceFolder, _webApi))}/Program.cs";

        var lines = File.ReadAllLines(programCsPath).ToList();
        for (int i = 0; i < lines.Count; i++)
        {
            if (i == 0)
            {
                lines.Insert(i, $"using {_solutionName}.{_app};");
                lines.Insert(i + 1, $"using {_solutionName}.{_infra};");
                lines.Insert(i + 2, "");
            }

            if (lines[i].Contains("builder.Services.AddControllers();"))
            {
                lines.Insert(i + 1, "builder.Services");
                lines.Insert(i + 2, $"\t\t.Add{_app}()");
                lines.Insert(i + 3, $"\t\t.Add{_infra}();");
                lines.Insert(i + 4, "");
                break;
            }
        }

        File.WriteAllLines(programCsPath, lines);
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