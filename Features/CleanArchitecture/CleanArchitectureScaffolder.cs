using ScaffNet.Utils;
using Logger = ScaffNet.Utils.ScaffLogger;

namespace ScaffNet.Features.CleanArchitecture
{
    public class CleanArchitectureArgs : FileSystemArgs
    {

    }
    public static class CleanArchitectureScaffolder
    {
        private static readonly string _app = "Application";
        private static readonly string _infra = "Infrastructure";
        private static readonly string _domain = "Domain";
        private static readonly string _webApi = "WebApi";

        public static void Create(CleanArchitectureArgs args)
        {
            CreateSolutionAndProjects(args);
            FileHelper.DeleteRedundantDefaultFiles(args.SolutionPath, ["Class1.cs"]);
            AddDi(args);

            Commander.RunCommand(
                    new RunCommandArgs()
                    {
                        Command = "dotnet",
                        Arguments = $"restore \"{args.SolutionPath}/{args.SolutionName}.sln\"",
                        SolutionPath = args.SolutionPath
                    }

                );

            SolutionHelper.BuildSolution(args);

            Logger.Default.LogWarning("Clean Architecture solution DONE!");
        }


        private static void CreateSolutionAndProjects(FileSystemArgs args)
        {

            Logger.Default.LogInfo($"Creating Clean Architecture solution: {args.SolutionName}");

            Commander.RunCommand(new RunCommandArgs() { 
                Command = "dotnet", 
                Arguments = $"new sln -n {args.SolutionName} -o \"{args.SolutionPath}\"",
                   SolutionPath = args.SolutionPath
            });

            string[] classLibraryProjects = { _domain, _app, _infra };

            foreach (string project in classLibraryProjects)
            {
                Commander.RunCommand(
                    new RunCommandArgs()
                    {
                        Command = "dotnet",
                        Arguments = $"new classlib -n {args.SolutionName}.{project} -o \"{Path.GetFullPath(Path.Combine(args.SolutionPath, args.SourceFolder, project))}\"",
                        SolutionPath = args.SolutionPath
                    });
                Commander.RunCommand(
                    new RunCommandArgs()
                    {
                        Command = "dotnet",
                        Arguments = $"sln \"{args.SolutionPath}/{args.SolutionName}.sln\" add \"{Path.GetFullPath(Path.Combine(args.SolutionPath, args.SourceFolder, project, args.SolutionName))}.{project}.csproj\"",
                        SolutionPath = args.SolutionPath
                    });
            }

            Commander.RunCommand(
                new RunCommandArgs()
                {
                    Command = "dotnet",
                    Arguments = $"new webapi -n {args.SolutionName}.{_webApi} -controllers -o \"{Path.GetFullPath(Path.Combine(args.SolutionPath, args.SourceFolder, _webApi))}\"",
                 SolutionPath = args.SolutionPath
                });
            Commander.RunCommand(
                new RunCommandArgs()
                {
                    Command = "dotnet",
                    Arguments = $"sln \"{args.SolutionPath}/{args.SolutionName}.sln\" add \"{Path.GetFullPath(Path.Combine(args.SolutionPath, args.SourceFolder, _webApi, args.SolutionName))}.{_webApi}.csproj\"",
                     SolutionPath = args.SolutionPath
                });

            var applicationCsProjFilePath = Path.GetFullPath(Path.Combine(args.SolutionPath, args.SourceFolder, _app, args.SolutionName)) + $".{_app}.csproj";
            var infrastructureCsProjFilePath = Path.GetFullPath(Path.Combine(args.SolutionPath, args.SourceFolder, _infra, args.SolutionName)) + $".{_infra}.csproj";
            var domainCsProjFilePath = Path.GetFullPath(Path.Combine(args.SolutionPath, args.SourceFolder, _domain, args.SolutionName)) + $".{_domain}.csproj";
            var webApiCsProjFilePath = Path.GetFullPath(Path.Combine(args.SolutionPath, args.SourceFolder, _webApi, args.SolutionName)) + $".{_webApi}.csproj";

            Commander.RunCommand(
                new RunCommandArgs()
                {
                    Command = "dotnet",
                    Arguments = $"add \"{applicationCsProjFilePath}\" reference \"{domainCsProjFilePath}\"",
                    SolutionPath = args.SolutionPath
                });
            Commander.RunCommand(
                new RunCommandArgs()
                {
                    Command = "dotnet",
                    Arguments = $"add \"{infrastructureCsProjFilePath}\" reference \"{applicationCsProjFilePath}\"",
                     SolutionPath = args.SolutionPath
                });
            Commander.RunCommand(
                new RunCommandArgs()
                {
                    Command = "dotnet",
                    Arguments = $"add \"{webApiCsProjFilePath}\" reference \"{infrastructureCsProjFilePath}\"",
                SolutionPath = args.SolutionPath
                });

            Logger.Default.LogInfo($"Solution: {args.SolutionName} ; CREATED");
        }

        private static void AddDi(FileSystemArgs args)
        {
            var applicationCsProjFilePath = Path.GetFullPath(Path.Combine(args.SolutionPath, args.SourceFolder, _app, args.SolutionName)) + $".{_app}.csproj";
            var infrastructureCsProjFilePath = Path.GetFullPath(Path.Combine(args.SolutionPath, args.SourceFolder, _infra, args.SolutionName)) + $".{_infra}.csproj";
            var templatePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Assets/CleanArchitecture/", "DependencyInjection.tpl"));
            string[] projectsForDi = { _app, _infra };
            var namespaceReplaceText = "REPLACEME_NS";
            var methodReplaceText = "REPLACEME_MT";
            var diNugetPackage = "Microsoft.Extensions.DependencyInjection";

            foreach (string project in projectsForDi)
            {
                Commander.RunCommand(
                    new RunCommandArgs()
                    {
                        Command = "dotnet",
                        Arguments = $"add \"{applicationCsProjFilePath}\" package \"{diNugetPackage}\"",
                   SolutionPath = args.SolutionPath
                    });

                string content = File.ReadAllText(templatePath);
                content = content
                    .Replace(namespaceReplaceText, $"{args.SolutionName}.{project}")
                    .Replace(methodReplaceText, project);

                string targetFilePath = Path.GetFullPath(Path.Combine(args.SolutionPath, args.SourceFolder, project, "DependencyInjection.cs"));
                File.WriteAllText(targetFilePath, content);
                Logger.Default.LogDebug($"Created: {targetFilePath}");
            }

            string programCsPath = $"{Path.GetFullPath(Path.Combine(args.SolutionPath, args.SourceFolder, _webApi))}/Program.cs";

            var lines = File.ReadAllLines(programCsPath).ToList();
            for (int i = 0; i < lines.Count; i++)
            {
                if (i == 0)
                {
                    lines.Insert(i, $"using {args.SolutionName}.{_app};");
                    lines.Insert(i + 1, $"using {args.SolutionName}.{_infra};");
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
            Logger.Default.LogInfo($"Dependency injection created for projects: {string.Join(",", projectsForDi)}");
        }
    }
}
