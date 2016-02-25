using System;
using System.Collections;
namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Hashtable interface
	/// </summary>
	public interface IHashTable: ISearchableContainer, IContainer, IComparable, IEnumerable
	{

		/// <summary>
		/// Gets the load-factor of the hashtable
		/// </summary>
		double LoadFactor
		{
			get;
		}
	}
}
