using System;
using System.Collections;
namespace Netron.GraphLib
{
	/// <summary>
	/// Collection of entity objects
	/// </summary>
	[Serializable] public class EntityCollection : CollectionBase
	{	
		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		public EntityCollection()
		{
			
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="newarray"></param>
		private EntityCollection(ArrayList newarray)
		{
			InnerList.Clear();
			foreach( Entity sh in newarray)
			{
				Add(sh);
			}
		}

		#endregion

		#region Methods
		/// <summary>
		/// Clones the collection
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			return new EntityCollection(InnerList);
		}
		/// <summary>
		/// Adds an item to the collection
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public int Add(Entity entity)
		{
			return this.InnerList.Add(entity);
		}
		/// <summary>
		/// Adds or merges a collection into this collection
		/// </summary>
		/// <param name="collection"></param>
		public void AddRange(ShapeCollection collection)
		{
			for(int k=0; k<collection.Count; k++)
				this.Add(collection[k]);
		}
		/// <summary>
		/// Adds a range of connections to the collection
		/// </summary>
		/// <param name="collection"></param>
		public void AddRange(ConnectionCollection collection)
		{		
			for(int k=0; k<collection.Count; k++)
				this.Add(collection[k]);
		}
		/// <summary>
		/// Integer indexer
		/// </summary>
		public Entity this[int index]
		{
			get{
				if(index>-1 && index<this.InnerList.Count)
					return this.InnerList[index] as Entity;
				else
					return null;
			}
		}
		/// <summary>
		/// Returns whether the given item in contained in the collection
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public bool Contains(object entity)
		{
			if(entity is Entity)
				return this.InnerList.Contains(entity);
			else
				return false;
		}

		

		/// <summary>
		/// Removes an item from the collection 
		/// </summary>
		/// <param name="entity"></param>
		public void Remove(Entity entity)
		{
			this.InnerList.Remove(entity);
		}

		#endregion

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
			foreach( Entity sh in newlist)
			{
				Add(sh);
			}
		}
	}
}
