using System;
using System.Collections;
using System.Drawing;
namespace Netron.GraphLib.Utils
{
	/// <summary>
	/// Collects free arrows.
	/// <seealso cref="Netron.GraphLib.Utils.FreeArrow"/>
	/// </summary>
	public class FreeArrowCollection : CollectionBase
	{

		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		public FreeArrowCollection()
		{
			
		}
		#endregion

		#region Properties
		/// <summary>
		/// Integer indexer
		/// </summary>
		public FreeArrow this[int index]
		{
			get{return this.InnerList[index] as FreeArrow;}
		}

		/// <summary>
		/// String indexer
		/// </summary>
		public FreeArrow this[string name]
		{
			get
			{
				for(int k=0; k<this.InnerList.Count; k++)
				{
					if(this[k].Name == name)
						return this[k];
				}
				return null;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Adds a FreeArrow to the collection
		/// </summary>
		/// <param name="arrow"></param>
		/// <returns></returns>
		public int Add(FreeArrow arrow)
		{
			return this.InnerList.Add(arrow);
		}
		/// <summary>
		/// Paints the collection of arrows on the given graphics
		/// </summary>
		/// <param name="g"></param>
		public void Paint(Graphics g)
		{
			for(int k=0; k<this.InnerList.Count; k++)
			{
				(this.InnerList[k] as FreeArrow).PaintArrow(g);
			}
		}
		#endregion

	}
}
