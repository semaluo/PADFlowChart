using System;
using Netron.GraphLib.Interfaces;
using System.Collections;
namespace Netron.GraphLib
{
	/// <summary>
	/// STC of IWidget objects
	/// </summary>
	public class WidgetCollection : CollectionBase
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public WidgetCollection()	{}
		/// <summary>
		/// Adds a widget to the collection
		/// </summary>
		/// <param name="widget"></param>
		/// <returns></returns>
		public int Add(IWidget widget)
		{
			return this.InnerList.Add(widget);
		}

		/// <summary>
		/// Integer indexer
		/// </summary>
		public IWidget this[int index]
		{
			get{
				return this.InnerList[index] as IWidget;
			}
		}
	}
}
