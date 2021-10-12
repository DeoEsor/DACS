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
    internal class MultiLogicalConvertors : MultiValueConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is string operation))
            {
                throw new ArgumentException("TODO", nameof(parameter));
            }

            foreach (var item in values)
                if (item == DependencyProperty.UnsetValue)
                     return DependencyProperty.UnsetValue;

            dynamic[] array = new dynamic[values.Length];
            for (int i = 0; i < values.Length; i++)
                array[i] = (dynamic)values[i];
            dynamic sum = default;
            switch (operation)
            {
                case "||":
                    for (int i = 0; i < values.Length; i++)
                        sum = sum || array[i];
                    return sum;
                case "&&":
                    for (int i = 0; i < values.Length; i++)
                        sum = sum && array[i];
                    return sum;
                default:
                    throw new ArgumentException("Incorrect operation", operation);
            }
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class LogicalConvertors : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is string operation))
                throw new ArgumentException("TODO", nameof(parameter));
            
            
            if (value == DependencyProperty.UnsetValue)
                return DependencyProperty.UnsetValue;

            var item = (dynamic)value;

            switch(operation)
            {
                case "!":
                    return !item;

                default:
                    throw new ArgumentException("Incorrect operation", operation);
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
