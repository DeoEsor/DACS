using System;
using System.Globalization;
using System.Windows;
using Vartumyan.Wpf.MVVM.Core.Converter;

namespace Vartumyan.Wpf.MVVM.Converters
{

    internal class MultiEqualityConvertors : MultiValueConverter
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
                    break;

                case "!=":
                    for (int i = 0; i < values.Length - 2; i++)
                        if (values[i] == values[i + 1])
                            return false;
                    break;

                default:
                    throw new ArgumentException("Incorrect operation", operation);
            }
            return true;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
