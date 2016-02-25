using System;

namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Searchable container interface
	/// </summary>
	public interface ISearchableContainer : IContainer
	{
		/// <summary>
		/// Returns wether the given object is in the container
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		bool IsMember(ComparableObject obj);
		/// <summary>
		/// Inserts the given object in the container
		/// </summary>
		/// <param name="obj"></param>
		void Insert(ComparableObject obj);
		/// <summary>
		/// Removes the given object from the container
		/// </summary>
		/// <param name="obj"></param>
		void Withdraw(ComparableObject obj);
		/// <summary>
		/// Searches the given object in the container
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		ComparableObject Find(ComparableObject obj);
	}
}
