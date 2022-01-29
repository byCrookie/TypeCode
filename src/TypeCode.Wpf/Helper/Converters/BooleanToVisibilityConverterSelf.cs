using System.Windows;

namespace TypeCode.Wpf.Helper.Converters;

public class BooleanToVisibilityConverterSelf : BooleanConverter<Visibility>
{
	public BooleanToVisibilityConverterSelf() : base(Visibility.Visible, Visibility.Collapsed) { }
}