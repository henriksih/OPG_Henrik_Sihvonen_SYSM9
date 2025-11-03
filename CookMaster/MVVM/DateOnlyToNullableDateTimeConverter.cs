using System;
using System.Globalization;
using System.Windows.Data;

namespace CookMaster.MVVM
{
    [ValueConversion(typeof(DateOnly), typeof(DateTime?))]
    public class DateOnlyToNullableDateTimeConverter : IValueConverter
    {
        //private bool ndt;

        // DateOnly -> DateTime?
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateOnly dateOnly)
                return dateOnly.ToDateTime(System.TimeOnly.MinValue);
            return null;
        }

        // DateTime? -> DateOnly
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is DateTime dt)
                return DateOnly.FromDateTime(dt);
            if (value is DateTime ndt)
                return DateOnly.FromDateTime(ndt);

            // If your VM property is non-nullable DateOnly, return a default value.
            // If you prefer null semantics, change the VM property to DateOnly? and return null here.
            return default(DateOnly);
        }
    }
}
