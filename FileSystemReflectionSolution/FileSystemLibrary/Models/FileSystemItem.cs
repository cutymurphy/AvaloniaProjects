using System;

namespace FileSystemLibrary.Models
{
    public abstract class FileSystemItem : IFileSystemItem
    {
        public string Name { get; set; }
        public Folder? ParentFolder { get; set; }

        public string Location
        {
            get
            {
                if (ParentFolder == null) return Name;
                return $"{ParentFolder.Location}/{Name}";
            }
        }

        public abstract FileSystemItemType ItemType { get; }
        public abstract long Size { get; }

        protected FileSystemItem(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public static void Copy(FileSystemItem source, Folder destination)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (destination == null) throw new ArgumentNullException(nameof(destination));
            if (destination.Items.Any(i => i.Name == source.Name))
                throw new InvalidOperationException($"Элемент с именем '{source.Name}' уже существует в папке '{destination.Name}'.");

            FileSystemItem copy;
            if (source is File file)
            {
                copy = new File(file.Name, file.Size);
            }
            else if (source is Folder folder)
            {
                copy = new Folder(folder.Name);
                foreach (var item in folder.Items)
                {
                    Copy(item, (Folder)copy);
                }
            }
            else
            {
                throw new NotSupportedException("Неизвестный тип элемента.");
            }

            destination.Add(copy);
        }
    }
}