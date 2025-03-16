using System;
using System.Collections.ObjectModel;

namespace AvaloniaApplication2.Models
{
    public abstract class FileSystemItem
    {
        public string Name { get; set; }
        public Folder? ParentFolder { get; set; }
        public string Location => ParentFolder == null ? "/" : ParentFolder.Location + Name + "/";
        public abstract FileSystemItemType ItemType { get; }
        public abstract long Size { get; }

        protected FileSystemItem(string name)
        {
            Name = name;
        }

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
}