using System.Windows;
using System.Windows.Controls;
using Vartumyan.Wpf.MVVM.Core.Commands;
namespace Vartumyan.Wpf.Controls.Controls
{
	public interface IDialogHost
	{
		CornerRadius Radius
		{
			set;
		}
		double RadiusLT
		{
			get;
			set;
		}
		double RadiusRT
		{
			get;
			set;
		}
		double RadiusLB
		{
			get;
			set;
		}
		double RadiusRB
		{
			get;
			set;
		}
		double Opacity
		{
			get;
			set;
		}
		ContentControl Content
		{
			get;
			set;
		}
		Command Command
		{
			get;
			set;
		}
	}
}
