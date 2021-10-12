using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace Vartumyan.Wpf.MVVM.Markups
{
    public class MarkupConvert : MarkupExtension
    {
        #region Variables & Properties
        private Collection<BindingBase> bindings;
        public Collection<BindingBase> Bindings
        {
            get => bindings;
            set
            {
                bindings = value;
            }
        }

        private IMultiValueConverter multiconverter;
        public IMultiValueConverter MultiConverter
        {
            get => multiconverter;
            set
            {
                multiconverter = value;
            }
        }

        private IValueConverter valueconverter;
        public IValueConverter Valueconverter
        {
            get => valueconverter;
            set
            {
                valueconverter = value;
            }
        }

        private object converterParameter;
        public object ConverterParameter
        {
            get => converterParameter;
            set
            {
                converterParameter = value;
            }
        }

        private CultureInfo converterCulture;
        public CultureInfo ConverterCulture
        {
            get => converterCulture;
            set
            {
                converterCulture = value;
            }
        }
        #endregion

        #region Overloads
        public MarkupConvert()
        {
            Bindings = new Collection<BindingBase>();
        }
        #endregion

        #region Methods
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (!Bindings.Any())
                throw new ArgumentNullException(nameof(Bindings));
            if (MultiConverter == null)
                throw new ArgumentNullException(nameof(MultiConverter));

            var target = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            if (target.TargetObject is Collection<BindingBase>)
            {
                var binding = new Binding
                {
                    Source = this
                };
                return binding;
            }

            var multiBinding = new MultiBinding
            {
                Mode = BindingMode.OneWay
            };
            multiBinding.Converter = multiconverter;

            return multiBinding.ProvideValue(serviceProvider);
        }
        #endregion
    }
}
