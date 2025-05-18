using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Task3_2
{
    public class EmergencyConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // Пример логики конвертера
            if (value is bool isEmergency && isEmergency)
            {
                return true; // Логика для аварийного случая
            }
            return false;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}