using System.Windows.Controls;
using Vartumyan.Wpf.Controls.ViewModels;
using Vartumyan.Wpf.MVVM.Data.Lab3Models;

namespace Vartumyan.Wpf.Controls.Controls
{
	public partial class SortElement : UserControl
	{
		public SortElement()
		{
			this.DataContext = new SortElement<int>();
			InitializeComponent();
			
		}
	}
}

