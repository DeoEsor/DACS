using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vartumyan.Wpf.MVVM
{
    public class MarkupConvert : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void RaisePropertiesChanged(params string[] propertiesNames)
        {
            foreach (var propertyName in propertiesNames)
                RaisePropertyChanged(propertyName);
        }

    }
}
