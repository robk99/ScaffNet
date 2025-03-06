using ScaffNet.Features.CleanArchitecture;

namespace Tests.ArchitectureTests
{
    public class CleanArchitectureTests
    {
        [Fact]
        public void Create_DefaultArgs_ShouldGenerateBuildAndDeleteSolution()
        {
            var solutionName = "TestCleanArchitectureSolution";
            var args = new CleanArchitectureArgs()
            {
                SolutionName = solutionName,
                SolutionPath = solutionName
            };
            
            var exception = Record.Exception(() => CleanArchitectureScaffolder.Create(args));

            Assert.Null(exception);

            var testSolutionFolder = Path.Combine(AppContext.BaseDirectory, solutionName);
            string expectedSolutionPath = Path.Combine(testSolutionFolder, $"{solutionName}.sln");
            Assert.True(File.Exists(expectedSolutionPath), "Solution file should have been created.");

            Directory.Delete(testSolutionFolder, true);
            Assert.False(Directory.Exists(testSolutionFolder));
        }
    }
}
