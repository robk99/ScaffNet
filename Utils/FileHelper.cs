using Microsoft.Extensions.Logging;
using Logger = ScaffNet.Utils.ScaffLogger;

namespace ScaffNet.Utils
{
    static class FileHelper
    {
        internal static void DeleteAllFileInstances(string solutionpath, string fileName)
        {
            if (!Directory.Exists(solutionpath))
            {
                Logger.Default.LogWarning($"Path not found: {solutionpath}");
                return;
            }

            try
            {
                var files = Directory.GetFiles(solutionpath, fileName, SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    Logger.Default.LogDebug($"Deleting: {file}");
                    File.Delete(file);
                }

                Logger.Default.LogDebug("Deletion complete.");
            }
            catch (Exception ex)
            {
                Logger.Default.LogError($"Error deleting files: {ex.Message}");
            }
        }

        internal static void DeleteRedundantDefaultFiles(string solutionPath, string[] files)
        {
            Logger.Default.LogDebug("\n--------- Deleting redundant files: ---------\n");

            foreach (var fileName in files)
            {
                DeleteAllFileInstances(solutionPath, fileName);
            }
        }
    }
}
