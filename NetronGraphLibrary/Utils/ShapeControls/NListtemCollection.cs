using System;
using System.Collections;
namespace Netron.GraphLib.Utils
{
	/// <summary>
	/// ListView implementation for shapes
	/// </summary>
	public class NListItemCollection : CollectionBase
	{

		#region Delegates and events
		/// <summary>
		/// ListItem info delegate
		/// </summary>
		public delegate void NListChange(object sender, NListEventArgs e);
		/// <summary>
		/// Occurs when an item is added to the list
		/// </summary>
		public event NListChange OnItemAdded;
		/// <summary>
		/// Occurs when an item is removed from the list
		/// </summary>
		public event NListChange OnItemRemoved;
		#endregion

		#region Properties
		/// <summary>
		/// Integer indexer
		/// </summary>
		public NListItem this[int index]
		{
			get{return this.InnerList[index] as NListItem;}
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		public NListItemCollection(){}
		#endregion

		#region Methods
		/// <summary>
		/// Adds an item to the list
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int Add(NListItem item)
		{
			return this.InnerList.Add(item);
		}
		/// <summary>
		/// Raises the OnInsert event
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		protected override void OnInsert(int index, object value)
		{
			base.OnInsert (index, value);
			RaiseOnItemAdded(value as NListItem, new NListEventArgs(index));
		}
		/// <summary>
		/// Raises the OnRemove event
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		protected override void OnRemove(int index, object value)
		{
			base.OnRemove (index, value);
			RaiseOnItemRemoved(value as NListItem,new NListEventArgs(index));
		}		
		#region Raisers
		/// <summary>
		/// Raises the OnItemAdded event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RaiseOnItemAdded(object sender, NListEventArgs e)
		{
			if(OnItemAdded!=null) 
				OnItemAdded(sender, e);
		}
		/// <summary>
		/// Raises the OnItemRemoved event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void RaiseOnItemRemoved(object sender, NListEventArgs e)
		{
			if(OnItemRemoved!=null) 
				OnItemRemoved(sender, e);
		}
		#endregion
		#endregion
		
	}
	/// <summary>
	/// NList event argument encapsulation
	/// </summary>
	public class NListEventArgs : EventArgs
	{
		/// <summary>
		/// the index
		/// </summary>
		private int mIndex;
		/// <summary>
		/// Gets or sets the index of the listitem
		/// </summary>
		public int Index
		{
			get{return mIndex;}
			set{mIndex = value;}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="index"></param>
		public NListEventArgs(int index)
		{
			mIndex = index;
		}
		/// <summary>
		/// Constructor
		/// </summary>
		public NListEventArgs()
		{}
	}
}
