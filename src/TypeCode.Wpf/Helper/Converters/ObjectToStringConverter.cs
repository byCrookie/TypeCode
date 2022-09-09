using System.Globalization;
using System.Windows.Data;

namespace TypeCode.Wpf.Helper.Converters;

public class ObjectToStringConverter : IValueConverter
{
	public virtual object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return value.ToString();
	}

	public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new Exception();
	}
}