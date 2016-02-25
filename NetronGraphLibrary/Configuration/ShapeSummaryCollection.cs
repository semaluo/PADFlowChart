using System;
using System.Collections;
namespace Netron.GraphLib.Configuration
{
	/// <summary>
	/// Implements a strongly typed collection of shape summaries
	/// </summary>
	public class ShapeSummaryCollection : CollectionBase
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public ShapeSummaryCollection()
		{
			
		}
		/// <summary>
		/// Adds an item to the collection
		/// </summary>
		/// <param name="summary"></param>
		/// <returns></returns>
		public int Add(ShapeSummary summary)
		{
			return this.InnerList.Add(summary);
		}
		/// <summary>
		/// Integer indexer
		/// </summary>
		public ShapeSummary this[int index]
		{
			get{return this.InnerList[index] as ShapeSummary;}
		}
	}
}
