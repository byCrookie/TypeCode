using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using TypeCode.Wpf.Jab.Main.Sidebar;

namespace TypeCode.Wpf.Jab.Helper.Converters;

public class ItemToActiveConverter : IValueConverter
{
	public Brush Active { get; set; }
	public Brush NotActive { get; set; }
		

	public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return value is ActiveItem b && b.Equals(parameter) ? Active : NotActive;
	}

	public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return value is ActiveItem variable && variable.Equals(parameter) ? NotActive : Active;
	}
}