// FileSystemModule.cs
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AvaloniaApplication2.Models
{
    public enum FileSystemItemType
    {
        Folder,
        File
    }

    public abstract class FileSystemItem
    {
        // Свойства
        public string Name { get; set; }
        public Folder? ParentFolder { get; set; }
        public string Location => ParentFolder == null ? "/" : ParentFolder.Location + Name + "/";
        public abstract FileSystemItemType ItemType { get; }
        public abstract long Size { get; }

        // Конструктор
        protected FileSystemItem(string name)
        {
            Name = name;
        }

        // Статические методы
        public static void Copy(FileSystemItem item, Folder destination)
        {
            if (item is File file)
            {
                var copiedFile = new File(file.Name, file.Size);
                destination.Add(copiedFile);
            }
            else if (item is Folder folder)
            {
                var copiedFolder = new Folder(folder.Name);
                foreach (var child in folder.Items)
                {
                    Copy(child, copiedFolder);
                }
                destination.Add(copiedFolder);
            }
        }

        public static void Move(FileSystemItem item, Folder destination)
        {
            if (item.ParentFolder != null)
            {
                item.ParentFolder.Remove(item);
            }
            destination.Add(item);
            item.ParentFolder = destination;
        }
    }

    public class Folder : FileSystemItem
    {
        // Список элементов в папке
        public ObservableCollection<FileSystemItem> Items { get; } = new();

        // Конструктор
        public Folder(string name) : base(name) { }

        // Реализация абстрактных свойств
        public override FileSystemItemType ItemType => FileSystemItemType.Folder;
        public override long Size => CalculateSize();

        // Метод добавления элемента
        public void Add(FileSystemItem item)
        {
            Items.Add(item);
            item.ParentFolder = this;
        }

        // Метод удаления элемента
        public void Remove(FileSystemItem item)
        {
            Items.Remove(item);
            item.ParentFolder = null;
        }

        // Вычисление размера папки
        private long CalculateSize()
        {
            long totalSize = 0;
            foreach (var item in Items)
            {
                totalSize += item.Size;
            }
            return totalSize;
        }
    }

    public class File : FileSystemItem
    {
        // Размер файла
        public long FileSize { get; }

        // Конструктор
        public File(string name, long size) : base(name)
        {
            FileSize = size;
        }

        // Реализация абстрактных свойств
        public override FileSystemItemType ItemType => FileSystemItemType.File;
        public override long Size => FileSize;
    }
}