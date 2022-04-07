using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TypeCode.Wpf.Helper.Converters;

public class AddTicknessConverter : IValueConverter
{
    public double Left { get; set; }
    public double Top { get; set; }
    public double Right { get; set; }
    public double Bottom { get; set; }
    
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Thickness thickness)
        {
            thickness.Left += Left;
            thickness.Top += Top;
            thickness.Right += Right;
            thickness.Bottom += Bottom;
            return thickness;
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}