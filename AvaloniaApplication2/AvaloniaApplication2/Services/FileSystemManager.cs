using System.Collections.ObjectModel;
using AvaloniaApplication2.Models;

namespace AvaloniaApplication2.Services
{
    public class FileSystemManager
    {
        public ObservableCollection<FileSystemItem> FileSystemItems { get; } = new();

        public void Initialize()
        {
            var rootFolder = new Folder("Root");
            var subFolder = new Folder("SubFolder");
            var file1 = new File("file1.txt", 1024);
            var file2 = new File("file2.txt", 2048);

            subFolder.Add(file1);
            rootFolder.Add(subFolder);
            rootFolder.Add(file2);

            FileSystemItem.Copy(file2, subFolder);

            FileSystemItems.Add(rootFolder);
        }
    }
}