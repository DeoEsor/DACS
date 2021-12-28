using System;
using System.Globalization;
using System.Windows;
using Vartumyan.Wpf.MVVM.Core.Converter;

namespace Vartumyan.Wpf.MVVM.Converters
{
	public class MultiStrikinglyConvertors : MultiValueConverter<MultiStrikinglyConvertors>
		{
			public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
			{
				if (!(parameter is string operation))
					throw new ArgumentException("Parameter incorrect", nameof(parameter));

				foreach (object item in values)
					if (item == DependencyProperty.UnsetValue)
						return DependencyProperty.UnsetValue;

				dynamic[] array = new dynamic[values.Length];
				for (int i = 0; i < values.Length; i++)
					array[i] = (dynamic)values[i];

				dynamic sum = default;

				switch (operation)
				{
					case "|":
						for (int i = 0; i < values.Length; i++)
							sum |= array[i];
						return sum;

					case "&":
						for (int i = 0; i < values.Length; i++)
							sum &= array[i];
						return sum;

					case "^":
						for (int i = 0; i < values.Length; i++)
							sum ^= array[i];
						return sum;

					default:
						throw new ArgumentException("Incorrect operation", operation);
				}
			}
		}

		class StrikinglyConvertors : ValueConverter<StrikinglyConvertors>
		{
			public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			{
				if (!(parameter is string operation))
					throw new ArgumentException("Parameter incorrect", nameof(parameter));

				if (value == DependencyProperty.UnsetValue)
					return DependencyProperty.UnsetValue;

				var item = (dynamic)value;

				switch (operation)
				{
					case "~":
						return ~item;

					default:
						throw new ArgumentException("Incorrect operation", operation);
				}
			}
		}

}
