using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Vartumyan.Wpf.MVVM.Core.Sorts
{
	public static partial class Sort
	{
		/// <summary>
		/// Сортировка Шелла
		/// </summary>
		/// <typeparam name="T">Любой тип реализующий IComparable<T></typeparam>
		public static class ShellSort
		{
			public static  IEnumerator<T> Sort<T>(IEnumerable<T> itemsenum)
				where  T : IComparable<T>
			{
				List<T> items = itemsenum.ToList();
				var d = items.Count() / 2;
				while (d >= 1)
				{
					for (var i = d; i < items.Count; i++)
					{
						var j = i;
						while ((j >= d) && (items[j - d].CompareTo(items[j]) > 0))
						{
							(items[j],  items[j - d]) = (items[j - d],  items[j]);
							j = j - d;
						}
						Task.Delay(500);
					}

					d = d / 2;
				}

				return items.GetEnumerator() as IEnumerator<T>;
			}
		}	
	}
}
