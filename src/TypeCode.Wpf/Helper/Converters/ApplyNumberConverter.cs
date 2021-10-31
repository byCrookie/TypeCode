using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using TypeCode.Wpf.Main.Sidebar;

namespace TypeCode.Wpf.Helper.Converters
{
	public class ApplyNumberConverter : IValueConverter
	{
		public double Number { get; set; }
		
		public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is double b)
			{
				var result = b + Number;
				return result;
			}
			
			return value;
		}

		public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}
