
using System.IO;
using System.IO.Abstractions; 

namespace FileOrganizer.Core
{
    public class FileManager
    {
        private readonly IFileSystem _fileSystem;
        public FileManager(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        // تعديل: استخدام Models.File بدلاً من System.IO.File
        public List<Models.File> ScanFolder(string path)
        {
            var files = new List<Models.File>();

            // استخدام _fileSystem بدلاً من Directory مباشرة
            foreach (var filePath in _fileSystem.Directory.GetFiles(path))
            {
                files.Add(new Models.File
                {
                    Name = _fileSystem.Path.GetFileName(filePath),
                    Path = filePath,
                    Extension = _fileSystem.Path.GetExtension(filePath),
                    OriginalPath = filePath
                });
            }
            return files;
        }

        public void MoveFile(Models.File file, string destinationFolder)
        {
            string destDirectory = _fileSystem.Path.Combine(_fileSystem.Path.GetDirectoryName(file.Path), destinationFolder);
            string destPath = _fileSystem.Path.Combine(destDirectory, file.Name);

            if (!_fileSystem.Directory.Exists(destDirectory))
            {
                _fileSystem.Directory.CreateDirectory(destDirectory);
            }

            if (_fileSystem.File.Exists(destPath))
            {
                HandleFileConflict(ref destPath);
            }
            if (string.IsNullOrEmpty(file.OriginalPath))
            {
                file.OriginalPath = file.Path;
            }

            _fileSystem.File.Move(file.Path, destPath);
            file.Path = destPath;
        }
        // تعديل: استخدام Models.File بدلاً من System.IO.File
        public void MoveFileToFullPath(Models.File file, string fullPath)
        {
            try
            {
                // 1. إنشاء المجلد الأصلي إذا لم يكن موجوداً
                var originalDir = Path.GetDirectoryName(fullPath);
                if (!_fileSystem.Directory.Exists(originalDir))
                {
                    _fileSystem.Directory.CreateDirectory(originalDir);
                }

                // 2. التعامل مع حالات تضارب الملفات
                if (_fileSystem.File.Exists(fullPath))
                {
                    HandleFileConflict(ref fullPath);
                }

                // 3. نقل الملف
                _fileSystem.File.Move(file.Path, fullPath);
                file.Path = fullPath;

                // 4. التحقق مما إذا كان المجلد الهدف فارغاً وحذفه إذا كان كذلك
                var sourceDir = Path.GetDirectoryName(file.Path);
                if (_fileSystem.Directory.Exists(sourceDir) &&
                    !_fileSystem.Directory.GetFiles(sourceDir).Any() &&
                    !_fileSystem.Directory.GetDirectories(sourceDir).Any())
                {
                    _fileSystem.Directory.Delete(sourceDir);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while moving file: {ex.Message}");
                throw;
            }
        }

        private void HandleFileConflict(ref string destinationPath)
        {
            // Option 1: Skip (just return without moving)
            // return;

            // Option 2: Overwrite (delete existing)
            // File.Delete(destinationPath);

            // Option 3: Rename 
            string dir = Path.GetDirectoryName(destinationPath);
            string fileName = Path.GetFileNameWithoutExtension(destinationPath);
            string ext = Path.GetExtension(destinationPath);
            destinationPath = Path.Combine(dir, $"{fileName}_{DateTime.Now:yyyyMMddHHmmss}{ext}");
        }
        public IFileSystem GetFileSystem()
        {
            return _fileSystem;
        }
    }
}
