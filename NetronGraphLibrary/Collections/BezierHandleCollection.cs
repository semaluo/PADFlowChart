using System;
using System.Collections;
using System.Drawing;
namespace Netron.GraphLib
{
	/// <summary>
	/// STC of Bezier handles
	/// </summary>
	[Serializable] public class BezierHandleCollection : CollectionBase
	{

		#region Fields
		/// <summary>
		/// the curve's painter
		/// </summary>
		private BezierPainter mCurve ;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the BezierPainter attached to this handle collection
		/// </summary>
		public BezierPainter Curve
		{
			get{return mCurve;}
			set{mCurve = value;}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Constructs a collection and assigns the collection to the given curve
		/// </summary>
		/// <param name="curve"></param>
		public BezierHandleCollection(BezierPainter curve)
		{
			this.mCurve = curve;
		}
		/// <summary>
		/// Default constructor
		/// </summary>
		public BezierHandleCollection()
		{			
		}
		/// <summary>
		/// Constrcuts a collection on the basis of a PointF collection
		/// </summary>
		/// <param name="list">An ArrayList of PointF's</param>
		public BezierHandleCollection(ArrayList list)
		{
			BezierHandle hl;
			for(int k=0;k<list.Count; k++)
			{
				hl = new BezierHandle((PointF) list[k]);
				Add(hl);
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
		public bool Contains(BezierHandle value) 
		{
			return ((IList)this).Contains((object) value);
		}


		/// <summary>
		/// Provide the strongly typed member for ICollection
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		public void CopyTo(BezierHandle[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}
		/// <summary>
		/// Returns the index of the given handle
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int IndexOf(BezierHandle value) 
		{
			return ((IList)this).IndexOf((object) value);
		}



		/// <summary>
		/// Removes and item from the collection
		/// </summary>
		/// <param name="handle"></param>
		public void Remove(BezierHandle handle)
		{
			this.InnerList.Remove(handle);
		}
		/// <summary>
		/// Adds an item to the collection
		/// </summary>
		/// <param name="handle"></param>
		/// <returns></returns>
		public int Add(BezierHandle handle)
		{
			return this.InnerList.Add(handle);
		}
		/// <summary>
		/// Inserts an item in the collection
		/// </summary>
		/// <param name="index"></param>
		/// <param name="handle"></param>
		public void Insert(int index, BezierHandle handle)
		{
			this.InnerList.Insert(index, handle);
		}
		/// <summary>
		/// Integer indexer
		/// </summary>
		public BezierHandle this[int index]
		{
			get{return this.InnerList[index] as BezierHandle;}
		}

		#endregion
	}
}
