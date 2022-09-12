using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using TypeCode.Wpf.Main.Sidebar;

namespace TypeCode.Wpf.Helper.Converters;

public sealed class ItemToActiveConverter : IValueConverter
{
	public ItemToActiveConverter()
	{
		Active = new SolidColorBrush(Colors.WhiteSmoke);
		NotActive = new SolidColorBrush(Colors.Black);
	}
	
	public Brush Active { get; set; }
	public Brush NotActive { get; set; }

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return value is ActiveItem b && b.Equals(parameter) ? Active : NotActive;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return value is ActiveItem variable && variable.Equals(parameter) ? NotActive : Active;
	}
}