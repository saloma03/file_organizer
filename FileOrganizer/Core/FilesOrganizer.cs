using FileOrganizer.Comands;
using FileOrganizer.Interfaces;
using FileOrganizer.Logging;
using FileOrganizer.Models;

namespace FileOrganizer.Core
{
    public class FilesOrganizer
    {
        private FileManager fileManager;
        private ActionLogger actionLogger;
        private UndoManager undoManager;
        private Dictionary<string, int> categoryCounts = new Dictionary<string, int>();

        public FilesOrganizer(FileManager fileManager, SettingManager settingManager, UndoManager undoManager)
        {
            this.fileManager = fileManager;
            this.undoManager = undoManager;
            this.actionLogger = new ActionLogger(); // يمكن إنشاؤه هنا إذا لم يكن له تبعيات معقدة
        }


        public void StartOrganization(string folderPath, bool simulate = false)
        {
            categoryCounts.Clear();
            var files = fileManager.ScanFolder(folderPath);
            foreach (var file in files)
            {
                string destinationFolder = DetermineDestination(file);

                // Update category count
                if (categoryCounts.ContainsKey(destinationFolder))
                {
                    categoryCounts[destinationFolder]++;
                }
                else
                {
                    categoryCounts[destinationFolder] = 1;
                }

                if (simulate)
                {
                    actionLogger.Log($"SIMULATE: Would move {file.Name} to {destinationFolder}");
                }
                else
                {
                    ICommand command = new OrganizeCommand(file, destinationFolder, fileManager);
                    undoManager.Execute(command);
                    actionLogger.Log($"Moved {file.Name} to {destinationFolder}");
                }
                LogCategorySummary();
            }
        }

        private string DetermineDestination(File file)
        {
            // Add logic to determine the appropriate folder for the file
            if (file.Extension == ".jpg" || file.Extension == ".png")
                return "Images";
            else if (file.Extension == ".pdf" || file.Extension == ".txt")
                return "Documents";
            else if (file.Extension == ".mp4" || file.Extension == ".avi")
                return "Videos";
            else
                return "Others";
        }

        public void UndoLastOperation()
        {
            undoManager.Undo();
        }
        private void LogCategorySummary()
        {
            actionLogger.Log("\n=== Organization Summary ===");
            foreach (var kvp in categoryCounts)
            {
                actionLogger.Log($"{kvp.Key}: {kvp.Value} files");
            }
            actionLogger.Log("==========================\n");
        }
    }
}
