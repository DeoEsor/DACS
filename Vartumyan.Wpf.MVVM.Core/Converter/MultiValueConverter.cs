﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Vartumyan.Wpf.MVVM.Core.Converter
{
    public abstract class MultiValueConverter : MarkupExtension, IMultiValueConverter
    {
        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        public abstract object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
