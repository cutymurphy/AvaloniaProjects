using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace Task3_2
{
    public class TrafficLightConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string state)
            {
                return state == "GreenForCars" ? Brushes.Green : Brushes.Red;
            }
            return Brushes.Gray;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TrafficLightConverterVertical : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string state)
            {
                return state == "GreenForCars" ? Brushes.Green : Brushes.Red;
            }
            return Brushes.Gray;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}