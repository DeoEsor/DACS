using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Vartumyan.Wpf.MVVM.Core.Converter;

namespace Vartumyan.Wpf.MVVM.Converters
{
	public class OrderConvertors : MultiValueConverter<OrderConvertors>
	{
		public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(parameter is string operation))
				throw new ArgumentException("Parameter incorrect", nameof(parameter));

			if (values.Length != 2)
				throw new ArgumentException("Number of values not suitable for this operation",
											$"Number of values : {values.Length}");

			if (values[0] == DependencyProperty.UnsetValue ||
				values[1] == DependencyProperty.UnsetValue)
					return DependencyProperty.UnsetValue;

			dynamic left = (dynamic)values[0];
			dynamic right = (dynamic)values[1];

			switch (operation)
			{
				case "<": return left < right;

				case "<=": return left <= right;

				case ">": return left > right;

				case ">=": return left >= right;

				default:
					throw new ArgumentException("Incorrect operation", operation);
			}
		}
    }
}
