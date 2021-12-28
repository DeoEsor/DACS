using System;
using System.Windows;
namespace Vartumyan.Wpf.MVVM.Data.Lab3Models
{
	public interface ISortElement<T> : IComparable<ISortElement<T>>
	{
		T Value
		{
			get;
			set;
		}
		int Index
		{
			get;
			set;
		}
	}
	//TODO ПОЧЕМУ не работает конвариантность?
	public sealed class SortElement<T> : ViewModelBase, ISortElement<T>, IComparable<SortElement<T>> where T : IComparable<T>
	{
		private T value;
		
		private int index;
		
		public T Value
		{
			get => value;
			set
			{
				this.value = value;
				
			}
		}

		public int Index
		{
			get => index;
			set
			{
				index = value;
				
			}
		}
		
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(T), 
				typeof(SortElement<T>));
		
		public static readonly DependencyProperty IndexProperty =
			DependencyProperty.Register("Index", typeof(int), 
				typeof(SortElement<T>));
		
		public int CompareTo(ISortElement<T> other) => CompareTo(other as SortElement<T>);

		public int CompareTo(SortElement<T> other)
		{
			if (ReferenceEquals(this, other))
				return 0;
			if (ReferenceEquals(null, other))
				return 1;
			var valueComparison = value.CompareTo(other.value);
			if (valueComparison != 0)
				return valueComparison;
			return index.CompareTo(other.index);
		}
	}
}
