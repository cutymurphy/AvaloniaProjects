using System;
using System.IO;

namespace FileSystemLibrary.Models
{
    public class File : FileSystemItem
    {
        public long FileSize { get; }

        public File(string name, long size) : base(name)
        {
            if (!HasExtension(name))
                throw new ArgumentException("Имя файла должно содержать расширение (например, .txt, .png).");
            if (size < 0) throw new ArgumentException("Размер файла не может быть отрицательным.");
            FileSize = size;
        }

        public override FileSystemItemType ItemType => FileSystemItemType.File;
        public override long Size => FileSize;

        public static File Create(string name, long size)
        {
            return new File(name, size);
        }

        public void Delete(string name)
        {
            if (ParentFolder == null)
                throw new InvalidOperationException("Файл не находится в папке.");
            if (Name != name)
                throw new InvalidOperationException($"Имя файла '{name}' не совпадает с текущим файлом '{Name}'.");
            ParentFolder.Remove(this);
        }

        private bool HasExtension(string name)
        {
            return Path.HasExtension(name);
        }
    }
}