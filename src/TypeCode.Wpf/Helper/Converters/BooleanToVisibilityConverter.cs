using System.Windows;

namespace TypeCode.Wpf.Helper.Converters;

public sealed class BooleanToVisibilityConverter : BooleanConverter<Visibility>
{
	public BooleanToVisibilityConverter() : base(Visibility.Visible, Visibility.Collapsed) { }
}