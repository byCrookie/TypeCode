using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TypeCode.Wpf.Helper.Converters;

public sealed class StringCompareToForegroundConverter : IMultiValueConverter
{
	public StringCompareToForegroundConverter()
	{
		Match = new SolidColorBrush(Colors.WhiteSmoke);
		NotMatch = new SolidColorBrush(Colors.Black);
	}
	
	public Brush Match { get; set; }
	public Brush NotMatch { get; set; }

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		return values[0].Equals(values[1]) ? Match : NotMatch;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		return Array.Empty<object>();
	}
}