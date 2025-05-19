using System;
using System.Globalization;
using Avalonia.Data.Converters;
using FileSystemLibrary.Models;

namespace AvaloniaApplication4
{
    public class IsFileSystemItemConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is Type type && type == typeof(File);
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}