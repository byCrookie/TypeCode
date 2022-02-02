using System.Globalization;
using System.Windows.Data;

namespace TypeCode.Wpf.Helper.Converters;

public class AttributeConverter : IValueConverter
{
    public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return $"{parameter}=\"{value}\"";
    }

    public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}