using System.Globalization;
using System.Windows.Data;

namespace TypeCode.Wpf.Helper.Converters;

public class EqualsConverter : IValueConverter
{
	public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return value.Equals(parameter);
	}

	public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return value.Equals(parameter);
	}
}