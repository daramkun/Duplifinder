using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Duplifinder.Converter
{
	class FindingMethodConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			switch (value)
			{
				case FindingMethod.FilenameSimiliarity:
					return "Filename Similarity";

				case FindingMethod.HashValueSameness:
					return "Hash value Sameness";
			}

			return "Unknown method";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
