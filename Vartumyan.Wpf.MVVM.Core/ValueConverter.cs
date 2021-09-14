using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace Vartumyan.Wpf.MVVM.Core
{
    public class ValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
