using System;
using System.Globalization;
using System.Windows;
using Vartumyan.Wpf.MVVM.Core.Converter;

namespace Vartumyan.Wpf.MVVM.Converters
{
    internal class MultiArithmeticConvertors : MultiValueConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is string operation))
                throw new ArgumentException("Parameter incorrect", nameof(parameter));

            foreach (var item in values)
                if (item == DependencyProperty.UnsetValue)
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

    internal class ArithmeticConvertors : ValueConverter
    {
        public override object Convert(object values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == DependencyProperty.UnsetValue)
                return DependencyProperty.UnsetValue;

            if (targetType.Name == "String")
                return values.ToString();

            else
                throw new NotImplementedException();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
           switch(targetType.Name)
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
