using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vartumyan.Wpf.MVVM.Core.Events
{
    public class CommandEventArgs : EventArgs
    {
        private object parameter;

        public object Parameter { get => parameter; set => parameter = value; }
    }

    public class CommandEventArgs<T> : EventArgs<T>
    {
        private T parameter;

        public CommandEventArgs(T value) : base(value)
        {
        }

        public T Parameter { get => parameter; set => parameter = value; }
    }
}
