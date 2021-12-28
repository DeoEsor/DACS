using System;
using System.Collections.Generic;
namespace Vartumyan.Wpf.MVVM.Core.Sorts
{
	public static partial class Sort
	{
		/*
		 
		 interface ISort
		 {
		 }
		 
		 public SortDelegate<T> this[Sorts sort, Type type]
		{
			
		}
		*/
		public delegate IEnumerator<T> SortDelegate<T>(IEnumerable<T> list);
		public enum Sorts
		{
			BubbleSort,
			ShellSort
		}	
	}
}
