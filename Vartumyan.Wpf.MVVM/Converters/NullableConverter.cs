using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vartumyan.Wpf.MVVM.Core.Converter;

namespace Vartumyan.Wpf.MVVM.Converters
{
    public class MultiNullableConverter : MultiValueConverter<MultiNullableConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Any(item => item is null))
            {
                var unknown = new {};
                return true;
            }
            return false;

        }
    }

    public sealed class NullableConverter : ValueConverter<NullableConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is null;
    }
}
