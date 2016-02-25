using System;
using System.Collections;
namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Partition interface
	/// </summary>
	public interface IPartition: ISet, ISearchableContainer, IContainer, IComparable, IEnumerable
	{
		/// <summary>
		/// Searches an item in the partition
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		ISet Find(int item);
		/// <summary>
		/// Joins two sets
		/// </summary>
		/// <param name="set1"></param>
		/// <param name="set2"></param>
		void Join(ISet set1, ISet set2);
	}
}
