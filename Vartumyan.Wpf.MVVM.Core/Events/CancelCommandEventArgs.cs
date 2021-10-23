using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vartumyan.Wpf.MVVM.Core.Events
{
    public class CancelCommandEventArgs : EventArgs
    {
        private object _value;
        private bool cancel;
        
        public bool Cancel { get => cancel; set => cancel = value; }
        public object Value { get => _value; set => _value = value; }
    }

    public class CancelCommandEventArgs<T> : EventArgs<T>
    {
        private bool cancel;

        public CancelCommandEventArgs(T value) : base(value)
        {
        }

        public bool Cancel { get => cancel; set => cancel = value; }
    }
}
