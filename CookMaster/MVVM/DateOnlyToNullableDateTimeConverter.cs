using System.Globalization;
using System.Windows.Data;

namespace CookMaster.MVVM
{
    //Klass för att hantera konverteringen från DateTime till DateOnly och v.v.

    [ValueConversion(typeof(DateOnly), typeof(DateTime?))]
    public class DateOnlyToNullableDateTimeConverter : IValueConverter
    {
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

            // Returnera ett defaultvärde annars.
            return default(DateOnly);
        }
    }
}
