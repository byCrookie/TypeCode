using System.Globalization;
using System.Windows.Data;

namespace TypeCode.Wpf.Helper.Converters;

public abstract class BooleanConverter<T> : IValueConverter where T : notnull
{
	protected BooleanConverter(T trueValue, T falseValue)
	{
		True = trueValue;
		False = falseValue;
	}

	public T True { get; set; }
	public T False { get; set; }

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is bool boolean)
		{
			return boolean ? True : False;
		}

		return False;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return value is T variable && EqualityComparer<T>.Default.Equals(variable, True);
	}
}