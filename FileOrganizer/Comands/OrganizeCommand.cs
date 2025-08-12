using System.Diagnostics;
using System.IO.Abstractions;
using FileOrganizer.Core;
using FileOrganizer.Interfaces;

namespace FileOrganizer.Comands
{
    internal class OrganizeCommand : ICommand
    {
        private Models.File file;
        private string originalPath;
        private string destination;
        private FileManager fileManager;
        private readonly IFileSystem fileSystem;

        public OrganizeCommand(Models.File file, string destination, FileManager fileManager)
        {
            if (string.IsNullOrEmpty(file.OriginalPath))
            {
                file.OriginalPath = file.Path;
            }
            this.file = file;
            this.destination = destination;
            this.fileManager = fileManager;
            this.fileSystem = fileManager.GetFileSystem(); // يجب إضافة هذه الدالة في FileManager

        }

        public void Execute()
        {
            fileManager.MoveFile(file, destination);

        }

        public void Undo()
        {
            if (string.IsNullOrEmpty(file.OriginalPath))
            {
                return;
            }

            try
            {
                // التأكد من وجود المجلد الأصلي
                var originalDir = fileSystem.Path.GetDirectoryName(file.OriginalPath);
                if (!fileSystem.Directory.Exists(originalDir))
                {
                    fileSystem.Directory.CreateDirectory(originalDir);
                }

                fileManager.MoveFileToFullPath(file, file.OriginalPath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to undo move for {file.Name}: {ex.Message}");
                throw;
            }
        }
    }



}
