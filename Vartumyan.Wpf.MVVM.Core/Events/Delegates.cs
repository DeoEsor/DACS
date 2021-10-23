using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vartumyan.Wpf.MVVM.Core.Events
{
    public delegate void CancelCommandEventHandler(object sender, EventArgs e);
    public delegate void CommandEventHandler(object sender, EventArgs e);
}
