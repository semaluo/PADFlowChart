using System;
using System.Collections;
using System.Runtime.Serialization;
namespace Netron.GraphLib
{
	/// <summary>
	/// Collection of shape objects
	/// </summary>
	/// <remarks>
	/// Note that the naive ISerializable does not work without the IDeserializationCallback interface for some reasons, see http://msdn.microsoft.com/msdnmag/issues/02/07/net/
	/// </remarks>
	[Serializable] public class ShapeCollection : CollectionBase, ISerializable, IDeserializationCallback 
	{	
		#region Events
		/// <summary>
		/// Occurs when a shape is added to the collection
		/// </summary>
		public event ShapeInfo OnShapeAdded;
		/// <summary>
		/// Occurs when a shape is removed from the collection
		/// </summary>
		public event ShapeInfo OnShapeRemoved;
		#endregion

		#region Fields
		/// <summary>
		/// necessary intermediate deserialization array
		/// </summary>
		private ArrayList ar; 
		#endregion

		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		public ShapeCollection()
		{
			
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="newarray"></param>
		private ShapeCollection(ArrayList newarray)
		{
			InnerList.Clear();
			foreach( Shape sh in newarray)
			{
				Add(sh);
			}
		}

		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected ShapeCollection(SerializationInfo info, StreamingContext context)
		{
			ar = info.GetValue("CollectionBase+list", typeof(ArrayList)) as ArrayList;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Sorts the collection
		/// </summary>
		/// <param name="sortParameter">the property upon which the sorting is based</param>
		/// <param name="direction">the SortDirection enum of the sorting algorithm</param>
		public void Sort( string sortParameter, SortDirection direction)
		{
			ArrayList newlist = (ArrayList)InnerList.Clone();
			ClassSorter sorter = new ClassSorter( sortParameter, SortByType.Property, direction);
			newlist.Sort( sorter);
			InnerList.Clear();
			foreach( Shape sh in newlist)
			{
				Add(sh);
			}
		}

		/// <summary>
		/// Raises the OnShapeAdded event
		/// </summary>
		/// <param name="shape"></param>
		internal void RaiseOnShapeAdded(Shape shape)
		{
			if(OnShapeAdded!=null)
				OnShapeAdded(this, shape);
		}
		/// <summary>
		/// Raises the OnShapeRemoved event
		/// </summary>
		/// <param name="shape"></param>
		internal void RaiseOnShapeRemoved(Shape shape)
		{
			if(OnShapeRemoved!=null)
				OnShapeRemoved(this,shape);
		}
		/// <summary>
		/// Serialization method
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("CollectionBase+list", this.InnerList);
		}

		
		/// <summary>
		/// IDeserializationCallback implementation, necessary to have ISerializable work on CollectionBase inherited class.
		/// </summary>
		/// <param name="sender"></param>
		public void OnDeserialization(object sender) 
		{
			if(ar==null || ar.Count==0) return;
			InnerList.AddRange(ar);
		} 
		/// <summary>
		/// Clones the collection
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			return new ShapeCollection(InnerList);
		}
		/// <summary>
		/// Adds an item to the collection
		/// </summary>
		/// <param name="shape"></param>
		/// <returns></returns>
		public int Add(Shape shape)
		{
			if(shape==null) return -1;
			RaiseOnShapeAdded(shape);
			return this.InnerList.Add(shape);
		}
		/// <summary>
		/// Integer indexer
		/// </summary>
		public Shape this[int index]
		{
			get{
				if(index>-1 && index<this.InnerList.Count)
					return this.InnerList[index] as Shape;
				else
					return null;
			}
		}
		/// <summary>
		/// Returns whether the given item in contained in the collection
		/// </summary>
		/// <param name="shape"></param>
		/// <returns></returns>
		public bool Contains(object shape)
		{
			if(shape is Shape)
				return this.InnerList.Contains(shape);
			else
				return false;
		}

		

		/// <summary>
		/// Removes an item from the collection 
		/// </summary>
		/// <param name="shape"></param>
		public void Remove(Shape shape)
		{

			this.InnerList.Remove(shape);
			RaiseOnShapeRemoved(shape);
		}

		#endregion
		
		}
}
