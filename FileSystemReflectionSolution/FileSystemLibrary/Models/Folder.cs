using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FileSystemLibrary.Models;

namespace FileSystemLibrary.Models
{
    [ObservableObject]
    public partial class Folder : FileSystemItem
    {
        private readonly FolderSizeCalculator _sizeCalculator = new FolderSizeCalculator();

        [ObservableProperty]
        private ObservableCollection<FileSystemItem> _items = new ObservableCollection<FileSystemItem>();

        public Folder(string name) : base(name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Имя папки не может быть пустым.");
            Items.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(Size));
                NotifyParentFolders();
            };
        }

        public override FileSystemItemType ItemType => FileSystemItemType.Folder;

        public override long Size => _sizeCalculator.CalculateSize(this);

        public void Add(FileSystemItem item)
        {
            if (item == null)
                throw new ArgumentNullException("Элемент не может быть пустым.");
            if (string.IsNullOrEmpty(item.Name))
                throw new ArgumentException("Имя элемента не может быть пустым.");
            if (Items.Any(i => i.Name == item.Name))
                throw new InvalidOperationException($"Элемент с именем '{item.Name}' уже существует в папке '{Name}'.");

            Items.Add(item);
            item.ParentFolder = this;
        }

        public void Remove(FileSystemItem item)
        {
            if (item == null)
                throw new ArgumentNullException("Элемент не может быть пустым.");
            if (!Items.Contains(item))
                throw new InvalidOperationException($"Элемент с именем '{item.Name}' не найден в папке '{Name}'.");

            Items.Remove(item);
            item.ParentFolder = null;
        }

        private void NotifyParentFolders()
        {
            var parent = ParentFolder;
            while (parent != null)
            {
                parent.OnPropertyChanged(nameof(Size));
                parent = parent.ParentFolder;
            }
        }
    }
}