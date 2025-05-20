using System;
using System.IO;

namespace FileSystemLibrary.Models
{
    public class File : FileSystemItem
    {
        public long FileSize { get; }

        public File(string name, long size) : base(name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Имя файла не может быть пустым.");
            if (!HasExtension(name))
                throw new ArgumentException("Имя файла должно содержать расширение, например, .txt или .png.");
            if (size < 0)
                throw new ArgumentException("Размер файла не может быть отрицательным.");
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
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Имя файла для удаления не может быть пустым.");
            if (ParentFolder == null)
                throw new InvalidOperationException("Файл не находится в папке и не может быть удален.");
            ParentFolder.Remove(this);
        }

        private bool HasExtension(string name)
        {
            return Path.HasExtension(name);
        }
    }
}