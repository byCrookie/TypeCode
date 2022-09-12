using System.Globalization;
using System.Windows.Data;

namespace TypeCode.Wpf.Helper.Converters;

public sealed class AttributeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return $"{parameter}=\"{value}\"";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}