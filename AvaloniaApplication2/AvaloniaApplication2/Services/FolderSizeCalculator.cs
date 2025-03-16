using AvaloniaApplication2.Models;

namespace AvaloniaApplication2.Services
{
    public class FolderSizeCalculator
    {
        public long CalculateSize(Folder folder)
        {
            long totalSize = 0;
            foreach (var item in folder.Items)
            {
                totalSize += item.Size;
            }

            return totalSize;
        }
    }
}