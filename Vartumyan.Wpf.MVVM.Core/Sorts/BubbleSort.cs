using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StaticInterface; 
namespace Vartumyan.Wpf.MVVM.Core.Sorts
{
	public static partial class Sort
	{
		/// <summary>
		/// Сортировка Шелла
		/// </summary>
		/// <typeparam name="T">Любой тип реализующий IComparable<T></typeparam>
		public static class BubbleSort
		{
			public static  IEnumerator<T> Sort<T>(IEnumerable<T> itemsenum)
			where  T : IComparable<T>
			{
				List<T> items = itemsenum.ToList();
				for (var i = 1; i < items.Count; i++)
						for (var j = 0; j < items.Count - i; j++)
							if (items[j].CompareTo(items[j + 1]) > 0)
							{
								(items[j],  items[j + 1]) = (items[j + 1],  items[j]);
								Task.Delay(500);
							}

				return items.GetEnumerator() as IEnumerator<T>;
			}
		}	
	}
}
