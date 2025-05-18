using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace AvaloniaApplication3
{
    public class OffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double baseValue && double.TryParse(parameter?.ToString(), out double offset))
            {
                return baseValue + offset;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}