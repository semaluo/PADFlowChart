using System;
using System.Collections;
namespace Netron.GraphLib
{
	/// <summary>
	/// Sorting utilities
	/// </summary>
	public class Sorting
	{
		#region Constructor
		/// <summary>
		///Default ctor 
		/// </summary>
		public Sorting()
		{		
		}
		#endregion		

		#region Methods
		/// <summary>
		/// Quick Sort Algorithm
		/// http://www.publicjoe.f9.co.uk/csharp/sort05.html
		/// </summary>
		/// <param name="shapes">a shapes collection</param> 
		public static void QuickSort(ref ShapeCollection shapes)
		{
			QuickSort( 0, shapes.Count-1, ref shapes );
		}

		/// <summary>
		/// Quicksort implementation
		/// </summary>
		/// <param name="lowerIndex"></param>
		/// <param name="upperIndex"></param>
		/// <param name="shapes"></param>
		public static void QuickSort( int lowerIndex, int upperIndex, ref ShapeCollection shapes )
		{
			
			int pivot, leftHold, rightHold;

			leftHold = lowerIndex;
			rightHold = upperIndex;
			pivot = shapes[lowerIndex].ZOrder;

			while( lowerIndex < upperIndex )
			{
				while( (shapes[upperIndex].ZOrder >= pivot) && (lowerIndex < upperIndex) )
				{
					upperIndex--;
				}

				if( lowerIndex != upperIndex )
				{
					shapes[lowerIndex].ZOrder = shapes[upperIndex].ZOrder;
					lowerIndex++;
				}

				while( (shapes[lowerIndex].ZOrder <= pivot) && (lowerIndex < upperIndex) )
				{
					lowerIndex++;
				}

				if( lowerIndex != upperIndex )
				{
					shapes[upperIndex].ZOrder = shapes[lowerIndex].ZOrder;
					upperIndex--;
				}
			}

			shapes[lowerIndex].ZOrder = pivot;
			pivot = lowerIndex;
			lowerIndex = leftHold;
			upperIndex = rightHold;

			if( lowerIndex < pivot )
			{
				QuickSort( lowerIndex, pivot-1, ref shapes );
			}

			if( upperIndex > pivot )
			{
				QuickSort( pivot+1, upperIndex, ref shapes );
			}
		} 



		#endregion

	}
	/// <summary>
	/// IComparer implementation to sort properties
	/// </summary>
	public class ClassSorter: IComparer
	{

		#region Fields
		/// <summary>
		/// which property to use
		/// </summary>
		protected string sortBy;
		/// <summary>
		/// the sort type
		/// </summary>
		protected SortByType sortByType;
		/// <summary>
		/// the sort direction
		/// </summary>
		protected SortDirection sortDirection;

		#endregion

		#region Constructors
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="sortBy"></param>
		/// <param name="sortByType"></param>
		/// <param name="sortDirection"></param>
		public ClassSorter(string sortBy, SortByType sortByType, SortDirection
			sortDirection)
		{
			this.sortBy = sortBy;
			this.sortByType = sortByType;
			this.sortDirection = sortDirection;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Compares two objects
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="comparer"></param>
		/// <returns></returns>
		int Compare( object x, object y, string comparer)
		{
			if ( comparer.IndexOf( ".") != -1 )
			{
				//split the string
				string[] parts = comparer.Split( new char[]{ '.'} );
				return Compare( x.GetType().GetProperty( parts[0]).GetValue(x, null) ,
					y.GetType().GetProperty( parts[0]).GetValue(y, null) , parts[1]
					);
			}
			else
			{
				IComparable icx, icy;
				icx =
					(IComparable)x.GetType().GetProperty( comparer).GetValue(x, null);
				icy =
					(IComparable)y.GetType().GetProperty( comparer).GetValue(y, null);

				if ( x.GetType().GetProperty(comparer).PropertyType  ==
					typeof(System.String) )
				{
					icx = (IComparable) icx.ToString().ToUpper();
					icy = (IComparable) icy.ToString().ToUpper();
				}

				if(this.sortDirection == SortDirection.Descending)
					return icy.CompareTo(icx);
				else
					return icx.CompareTo(icy);
			} 
		}

		/// <summary>
		/// Compares two objects
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public int Compare(object x, object y)
		{
			return Compare( x, y, sortBy);
		} 
		#endregion
	}

}
