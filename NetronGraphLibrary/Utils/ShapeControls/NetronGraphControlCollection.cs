using System;
using System.Collections;
namespace Netron.GraphLib.Utils
{

	/// <summary>
	/// STC of GraphControls
	/// </summary>
	[Serializable] public class NetronGraphControlCollection : CollectionBase
	{
		/// <summary>
		/// Adds an item to the collection
		/// </summary>
		/// <param name="control"></param>
		/// <returns></returns>
		public int Add(NetronGraphControl control)
		{
			return this.InnerList.Add(control);
		}
		/// <summary>
		/// Integer indexer
		/// </summary>
		public NetronGraphControl this[int index]
		{
			get{return this.InnerList[index] as NetronGraphControl;}
		}

	}
}
