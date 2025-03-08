# ScaffNet

A lightweight, efficient .NET project scaffolder that generates industry-standard project templates with minimal overhead. ScaffNet is designed to streamline the initial setup process for .NET projects, allowing developers to focus on building features rather than configuring project structures.

## Features

- **Architecture Scaffolder**: 
<br>Clean Architecture:
Generate a complete Clean Architecture solution with a single command, including:
  - Domain layer
  - Application layer
  - Infrastructure layer
  - WebAPI layer
  - Proper project references and dependencies

- **Minimal Overhead**: ScaffNet is designed to be lightweight and efficient, with minimal dependencies and a straightforward API.

- **Customizable Event Handling**: Override the default console logging event handler with your own implementation using `ScaffUtils.SetEventHandler()`.

- **Cross-Platform**: Works on Windows, macOS, and Linux.

## Installation

Install the ScaffNet NuGet package:

```bash
dotnet add package ScaffNet
```

Or add it directly to your project file:

```xml
<PackageReference Include="ScaffNet" Version="1.0.1" />
```

## Usage

### Clean Architecture Template

```csharp
using ScaffNet.Features.CleanArchitecture;
using ScaffNet.Utils;

// Create a new Clean Architecture solution
var response = CleanArchitectureScaffolder.Create(new CleanArchitectureArgs
{
    SolutionName = "MyAwesomeProject",
    SolutionPath = "C:/Projects"
});

Console.WriteLine(response.Message);
```

### Custom Event Handler

By default, ScaffNet logs events to the console. You can override this behavior by implementing the `IScaffEventHandler` interface and setting it as the default handler:

```csharp
using ScaffNet.Utils.EventHandling;

// Create a custom event handler
public class MyCustomEventHandler : IScaffEventHandler
{
    public void OnDebug(string message) => CustomLogMethod(message, "DEBUG");
    public void OnInfo(string message) => CustomLogMethod(message, "INFO");
    public void OnWarning(string message) => CustomLogMethod(message, "WARNING");
    public void OnError(string message) => CustomLogMethod(message, "ERROR");
    public void OnCritical(string message) => CustomLogMethod(message, "CRITICAL");
    
    private void CustomLogMethod(string message, string level)
    {
        // Your custom logging implementation
        Console.WriteLine($"[{level}] {DateTime.Now}: {message}");
    }
}

// Set the custom event handler
ScaffUtils.SetEventHandler(new MyCustomEventHandler());
```

## Related Projects

- [ScaffNetConsole](https://github.com/robk99/ScaffNetConsole): A .NET CLI tool that implements ScaffNet as a command-line interface with enhanced visual experience using Spectre.Console.Cli.
- [ScaffNetAPI](https://github.com/robk99/ScaffNetAPI): A RESTful API that exposes ScaffNet functionality as a web service.
- [ScaffNetAngular](https://github.com/robk99/ScaffNetAngular): A modern Angular 18 application that provides a user-friendly interface for ScaffNet.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.
