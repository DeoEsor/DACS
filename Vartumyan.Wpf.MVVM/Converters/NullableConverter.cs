using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vartumyan.Wpf.MVVM.Core.Converter;

namespace Vartumyan.Wpf.MVVM.Converters
{
    internal class MultiNullableConverter : MultiValueConverter
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var _ in from item in values
                              where item is null
                              select new { })
                return true;

            return false;
        }
    }

    internal class NullableConverter : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is null;
    }
}
