using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Vartumyan.Wpf.MVVM.Core.Converter
{
    public abstract class ValueConverter<T> : MarkupExtension, IValueConverter
	where T: class, new()
	{
		private static T _valueToProvide;
		public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

		public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return _valueToProvide ??= new T();
		}
	}
}
