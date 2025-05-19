using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace AvaloniaApplication4
{
    public class WatermarkConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string typeName)
            {
                return typeName switch
                {
                    "String" when typeName == "name" => "Имя файла или папки",
                    "String" => "Имя или путь (например, SubFolder/NewFolder)",
                    "Int32" => "Целое число",
                    "Int64" => "Размер файла",
                    _ => "Значение"
                };
            }
            return "Значение";
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}