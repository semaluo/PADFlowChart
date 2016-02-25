using System;
using System.Collections;
namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Search-tree interface
	/// </summary>
	public interface ISearchTree: ITree, ISearchableContainer, IContainer, IComparable, IEnumerable
	{

		/// <summary>
		/// Gets the min
		/// </summary>
		ComparableObject Min
		{
			get;
		}
		/// <summary>
		/// Gets the max
		/// </summary>
		ComparableObject Max
		{
			get;
		}
	}
}
