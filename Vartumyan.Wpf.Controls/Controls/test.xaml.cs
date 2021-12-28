using System.Windows.Controls;
using System.Windows.Markup;

namespace Vartumyan.Wpf.Controls.Controls
{
	public partial class test : UserControl
	{
		public test()
		{
			InitializeComponent();
			this.DataContext = this;
		}
	}
}

