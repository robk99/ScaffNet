namespace ScaffNet.Features
{
    public class FileSystemArgs
    {
        public required string SolutionPath { get; set; }
        public required string SolutionName { get; set; }
        public string? SourceFolder { get; set; } = "";
    }
}
