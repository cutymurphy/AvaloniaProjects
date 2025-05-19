using System.Collections.ObjectModel;
using FileSystemLibrary.Models;

namespace FileSystemLibrary.Services
{
    public class FileSystemManager
    {
        public ObservableCollection<FileSystemItem> FileSystemItems { get; } = new();

        public void Initialize()
        {
            var rootFolder = new Folder("Root");
            var subFolder = new Folder("SubFolder");
            var file1 = new Models.File("file1.txt", 1024);
            var file2 = new Models.File("file2.txt", 2048);

            subFolder.Add(file1);
            rootFolder.Add(subFolder);
            rootFolder.Add(file2);

            FileSystemItem.Copy(file2, subFolder);

            FileSystemItems.Add(rootFolder);
        }
    }
}