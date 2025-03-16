using System.Collections.ObjectModel;
using AvaloniaApplication2.Models;
using AvaloniaApplication2.Services;

namespace AvaloniaApplication2.ViewModels
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