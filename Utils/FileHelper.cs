using ScaffNet.Dtos;
using Logger = ScaffNet.Utils.ScaffLogger;

namespace ScaffNet.Utils
{
    static class FileHelper
    {
        /// <summary>
        /// Deletes all the files and folders in the given path
        /// </summary>
        /// <param name="solutionPath"></param>
        public static void DeleteAllCreatedFilesAndFolders(string solutionPath)
        {
            Logger.Default.LogInfo("\nDeleting all the files and folders!");

            DirectoryInfo directoryInfo = new DirectoryInfo(solutionPath);

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in directoryInfo.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        internal static void DeleteAllFileInstances(string solutionpath, string fileName)
        {
            if (!Directory.Exists(solutionpath))
            {
                Logger.Default.LogWarning($"Path not found: {solutionpath}");
                return;
            }

            var files = Directory.GetFiles(solutionpath, fileName, SearchOption.AllDirectories);

            foreach (var file in files)
            {
                Logger.Default.LogDebug($"Deleting: {file}");
                File.Delete(file);
            }

            Logger.Default.LogDebug("Deletion complete.");
        }

        internal static void DeleteRedundantDefaultFiles(string solutionPath, string[] files)
        {
            Logger.Default.LogDebug("\n--------- Deleting redundant default files: ---------\n");

            foreach (var fileName in files)
            {
                DeleteAllFileInstances(solutionPath, fileName);
            }
        }
    }
}
