using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Data;

namespace Duplifinder.Converter
{
	class FilenameConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var valueStr = value as string;
			var filename = Path.GetFileName(valueStr);
			var path = Path.GetDirectoryName(valueStr);
			return $"{filename} ({path})";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
