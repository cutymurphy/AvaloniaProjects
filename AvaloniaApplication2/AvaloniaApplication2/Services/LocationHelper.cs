using AvaloniaApplication2.Models;

namespace AvaloniaApplication2.Services
{
    public static class LocationHelper
    {
        public static string GetLocation(FileSystemItem item)
        {
            return item.ParentFolder == null ? "/" : item.ParentFolder.Location + item.Name + "/";
        }
    }
}