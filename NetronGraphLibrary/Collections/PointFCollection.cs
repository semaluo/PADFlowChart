using System;
using System.Collections;
using System.Drawing;
namespace Netron.GraphLib
{
	/// <summary>
	/// STC of PointF
	/// </summary>
	public class PointFCollection : CollectionBase
	{

		#region Fields
		

		#endregion

		#region Properties

		
		#endregion

		#region Constructors
		/// <summary>
		/// Constructs a collection and assigns the array to the collection
		/// </summary>
		/// <param name="points"></param>
		public PointFCollection(PointF[] points)
		{
			for(int k=0;k<points.Length; k++)
			{
				InnerList.Add(points[k]);
			}
		}
		/// <summary>
		/// Default constructor
		/// </summary>
		public PointFCollection()
		{			
		}
		/// <summary>
		/// Constrcuts a collection on the basis of a PointF collection
		/// </summary>
		/// <param name="list">An ArrayList of PointF's</param>
		public PointFCollection(ArrayList list)
		{
			for(int k=0;k<list.Count; k++)
			{				
				Add((PointF) list[k]);
			}
		}
		#endregion

		#region Methods

		/// <summary>
		/// Copies the collection to the given array, starting at the given position
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		void CopyTo(Array array, int index)
		{
			InnerList.CopyTo(array, index);
		}
		/// <summary>
		/// Returns whether the given handle is contained in the collection
		/// </summary>
		/// <param name="value">a Bezier handle</param>
		/// <returns>true if withing the collection, otherwise false</returns>
		public bool Contains(PointF value) 
		{
			return ((IList)this).Contains((object) value);
		}


		/// <summary>
		/// Provide the strongly typed member for ICollection
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		public void CopyTo(PointF[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}
		/// <summary>
		/// Returns the index of the given handle
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int IndexOf(PointF value) 
		{
			return ((IList)this).IndexOf((object) value);
		}



		/// <summary>
		/// Removes and item from the collection
		/// </summary>
		/// <param name="p">a point to be removed from the collection</param>
		public void Remove(PointF p)
		{
			this.InnerList.Remove(p);
		}
		/// <summary>
		/// Adds an item to the collection
		/// </summary>
		/// <param name="p">the point to be added to the collection</param>
		/// <returns></returns>
		public int Add(PointF p)
		{
			return this.InnerList.Add(p);
		}
		/// <summary>
		/// Inserts an item in the collection
		/// </summary>
		/// <param name="index"></param>
		/// <param name="p"></param>
		public void Insert(int index, PointF p)
		{
			this.InnerList.Insert(index, p);
		}
		/// <summary>
		/// Integer indexer
		/// </summary>
		public PointF this[int index]
		{
			get{return (PointF) this.InnerList[index];}
		}

		#endregion
	}
}
