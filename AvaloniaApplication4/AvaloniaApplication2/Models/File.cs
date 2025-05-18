namespace AvaloniaApplication4.Models
{
    public class File : FileSystemItem
    {
        public long FileSize { get; }

        public File(string name, long size) : base(name)
        {
            FileSize = size;
        }

        public override FileSystemItemType ItemType => FileSystemItemType.File;
        public override long Size => FileSize;
    }
}