using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using Vartumyan.Wpf.MVVM.Core.Converter;
using System.Xaml;
namespace Vartumyan.Wpf.MVVM.Converters
{
    public class MultiArithmeticConvertors : MultiValueConverter<MultiArithmeticConvertors>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is string operation))
                throw new ArgumentException("Parameter incorrect", nameof(parameter));

            if (values.Any(item => item == DependencyProperty.UnsetValue))
                return DependencyProperty.UnsetValue;

            dynamic[] array = new dynamic[values.Length];

            for (int i = 0; i < values.Length; i++)
                array[i] = (dynamic)values[i];

            dynamic sum = default;

            switch (operation)
            {
                case "+":
                    foreach (var item in array)
                        sum += item;
                    return sum;

                case "-":
                    foreach (var item in array)
                        sum += item;
                    return sum;

                case "*":
                    foreach (var item in array)
                        sum *= item;
                    return sum;

                case "/":
                    foreach (var item in array)
                        sum /= item;
                    return sum;

                case "%":
                    foreach (var item in array)
                        sum %= item;
                    return sum;

                default: throw new ArgumentException("Invalid operation", nameof(operation));
            }
        }
    }

    public class ArithmeticConvertors : ValueConverter<ArithmeticConvertors>
    {
        public override object Convert(object values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values is int || values == DependencyProperty.UnsetValue)
                return values;
            if (!int.TryParse(values.ToString(), out int value))
                return DependencyProperty.UnsetValue;
            if (targetType == null || targetType == typeof(String))
                return values.ToString();
            
            throw new NotImplementedException();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //TODO redo if targettype
           switch(targetType.FullName)
            {
                case "Int32":
                    if (int.TryParse(value.ToString(), out int a))
                        return a;

                    else throw new ArgumentException("Invalid value", nameof(value));

                case "Double":
                    if (double.TryParse(value.ToString(), out double d))
                        return d;

                    else throw new ArgumentException("Invalid value", nameof(value));

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
