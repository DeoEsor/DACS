using System.Windows;
using System.Windows.Controls;
using Vartumyan.Wpf.MVVM.Core.Commands;

namespace Vartumyan.Wpf.Controls.Controls
{

	public partial class MessageBox : UserControl, IDialogHost
	{
		public MessageBox()
		{
			InitializeComponent();
			Radius = new CornerRadius(RadiusLT, RadiusRT, RadiusRB, RadiusLB);
		}
		#region Fields
		public ButtonsType ButtonType { get; set; }

		public static readonly DependencyProperty ButtonsTypeProperty =
			DependencyProperty.Register("ButtonType", typeof(ButtonsType), typeof(MessageBox), null);

		public CornerRadius Radius
		{
			get;
			set;
		}
		public double RadiusLT
		{
			get;
			set;
		}
		public double RadiusRT
		{
			get;
			set;
		}
		public double RadiusLB
		{
			get;
			set;
		}
		public double RadiusRB
		{
			get;
			set;
		}
		public ContentControl Content
		{
			get;
			set;
		}
		
		public static readonly DependencyProperty CommandProperty =
			DependencyProperty.Register("BackgroundCommand", typeof(Command), typeof(DialogHost), null);

		public Command Command
		{
			get;
			set;
		}
  #endregion
	}
}

