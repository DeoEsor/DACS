using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using Vartumyan.Wpf.MVVM.Core.Converter;

namespace Vartumyan.Wpf.MVVM.Converters
{

    public class MultiEqualityConvertors : MultiValueConverter<MultiEqualityConvertors>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is string operation))
                throw new ArgumentException("Parameter incorrect", nameof(parameter));

            if (values[0] == DependencyProperty.UnsetValue ||
                values[1] == DependencyProperty.UnsetValue)
                return DependencyProperty.UnsetValue;

            switch (operation)
            {
                case "==":
                    for (int i = 0; i < values.Length - 1; i++)
                        if (values[i] != values[i + 1])
                            return false;
                    return true;
                //TODO show to Ilya
                case "!=":
                    return values.Length == Enumerable.Count(Enumerable.Distinct(values));

                default:
                    throw new ArgumentException("Incorrect operation", operation);
            }
            return true;
        }
    }
}
