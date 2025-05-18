using System.Collections.ObjectModel;
using AvaloniaApplication4.Models;
using AvaloniaApplication4.Services;

namespace AvaloniaApplication4.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<FileSystemItem> FileSystemItems => _fileSystemManager.FileSystemItems;

        private readonly FileSystemManager _fileSystemManager = new();

        public MainViewModel()
        {
            _fileSystemManager.Initialize();
        }
    }
}