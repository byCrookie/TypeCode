using System.Globalization;
using System.Windows.Data;
using Framework.Extensions.Enum;

namespace TypeCode.Wpf.Helper.Converters;

public class EnumToDescriptionConverter : IValueConverter
{
	public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (parameter is Enum enumSource)
		{
			return enumSource.GetDescription();
		}

		throw new ArgumentException($"{parameter.GetType().Name} is not of type {nameof(Enum)}");
	}

	public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new Exception();
	}
}