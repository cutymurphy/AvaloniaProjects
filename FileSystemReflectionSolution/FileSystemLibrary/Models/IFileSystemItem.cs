namespace FileSystemLibrary.Models
{
    public interface IFileSystemItem
    {
        string Name { get; set; }
        Folder? ParentFolder { get; set; }
        string Location { get; }
        FileSystemItemType ItemType { get; }
        long Size { get; }
    }
}