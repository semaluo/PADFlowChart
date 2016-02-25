using System;
using System.Collections;
namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Abstract base class of a searchable container
	/// </summary>
	public abstract class AbstractSearchableContainer : AbstractContainer, ISearchableContainer, IContainer, IComparable, IEnumerable
	{
		/// <summary>
		/// Returns whether a given obkect is in the container
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public abstract bool IsMember(ComparableObject obj);
		/// <summary>
		/// Inserts an object in the container
		/// </summary>
		/// <param name="obj"></param>
		public abstract void Insert(ComparableObject obj);
		/// <summary>
		/// Removes an object from the container
		/// </summary>
		/// <param name="obj"></param>
		public abstract void Withdraw(ComparableObject obj);
		/// <summary>
		/// Searches an object in the container
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public abstract ComparableObject Find(ComparableObject obj);
	}

}
