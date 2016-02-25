using System;
using System.Collections;
namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Set interface
	/// </summary>
	public interface ISet: ISearchableContainer, IContainer, IComparable, IEnumerable
	{

		/// <summary>
		/// Return the union of the set with the given one
		/// </summary>
		/// <param name="set"></param>
		/// <returns></returns>
		ISet Union(ISet set);
		/// <summary>
		/// Intersection of the set with the given one
		/// </summary>
		/// <param name="set"></param>
		/// <returns></returns>
		ISet Intersection(ISet set);
		/// <summary>
		/// Difference of the set with the given one
		/// </summary>
		/// <param name="set"></param>
		/// <returns></returns>
		ISet Difference(ISet set);
		/// <summary>
		/// Whether the set if equal to the given one
		/// </summary>
		/// <param name="set"></param>
		/// <returns></returns>
		bool Equals(ISet set);
		/// <summary>
		/// Returns whether the given set is a subset of this set
		/// </summary>
		/// <param name="set"></param>
		/// <returns></returns>
		bool IsSubset(ISet set);
	}
}
