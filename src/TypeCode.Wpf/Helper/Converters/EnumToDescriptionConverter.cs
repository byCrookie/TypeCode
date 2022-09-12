using System.Globalization;
using System.Windows.Data;
using Framework.Extensions.Enum;

namespace TypeCode.Wpf.Helper.Converters;

public sealed class EnumToDescriptionConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (parameter is Enum enumSource)
		{
			return enumSource.GetDescription();
		}

		throw new ArgumentException($"{parameter.GetType().Name} is not of type {nameof(Enum)}");
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new Exception();
	}
}