using AvaloniaApplication4.Models;

namespace AvaloniaApplication4.Services
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