using ScaffNet.Utils.Messages;
using EventHandler = ScaffNet.Utils.EventHandling.ScaffEventHandler;

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
            EventHandler.Default.OnInfo("\nDeleting all the files and folders!");

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
                EventHandler.Default.OnInfo(FileMessages.BaseDeleteAllInstanceMessage(fileName) + $" Path not found: {solutionpath}");
                return;
            }

            var files = Directory.GetFiles(solutionpath, fileName, SearchOption.AllDirectories);

            foreach (var file in files)
            {
                EventHandler.Default.OnDebug($"Deleting: {file}");
                File.Delete(file);
            }

            EventHandler.Default.OnInfo(FileMessages.BaseDeleteAllInstanceMessage(fileName) + " Deletion complete.");
        }

        internal static void DeleteRedundantDefaultFiles(string solutionPath, string[] files)
        {
            EventHandler.Default.OnDebug("\n--------- Deleting redundant default files: ---------\n");

            foreach (var fileName in files)
            {
                DeleteAllFileInstances(solutionPath, fileName);
            }
        }
    }
}
