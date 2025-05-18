using System.Collections.ObjectModel;
using AvaloniaApplication4.Services;

namespace AvaloniaApplication4.Models
{
    public class Folder : FileSystemItem
    {
        public ObservableCollection<FileSystemItem> Items { get; } = new();
        private readonly FolderSizeCalculator _sizeCalculator = new();

        public Folder(string name) : base(name)
        {
        }

        public override FileSystemItemType ItemType => FileSystemItemType.Folder;
        public override long Size => _sizeCalculator.CalculateSize(this);

        public void Add(FileSystemItem item)
        {
            Items.Add(item);
            item.ParentFolder = this;
        }

        public void Remove(FileSystemItem item)
        {
            Items.Remove(item);
            item.ParentFolder = null;
        }
    }
}