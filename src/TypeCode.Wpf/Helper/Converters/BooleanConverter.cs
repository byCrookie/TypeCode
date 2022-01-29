using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace TypeCode.Wpf.Helper.Converters;

public class BooleanConverter<T> : IValueConverter
{
	public BooleanConverter(T trueValue, T falseValue)
	{
		True = trueValue;
		False = falseValue;
	}

	public T True { get; set; }
	public T False { get; set; }

	public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return value is true ? True : False;
	}

	public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return value is T variable && EqualityComparer<T>.Default.Equals(variable, True);
	}
}