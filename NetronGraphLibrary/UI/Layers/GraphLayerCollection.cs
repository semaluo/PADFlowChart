using System;
using System.Collections;
namespace Netron.GraphLib
{
	/// <summary>
	/// STC of graphlayers
	/// </summary>
	[Serializable] public class GraphLayerCollection : CollectionBase
	{
		#region Events
		/// <summary>
		/// Occurs when the collection has been emptied
		/// </summary>
		public event EventHandler ClearComplete;
		
		#endregion

		#region Properties
		/// <summary>
		/// Return a layer on the basis of its index in the collection.
		/// </summary>
		public GraphLayer this[int index]
		{
			get
			{
				if(index<Count && index>=0)
					return this.InnerList[index] as GraphLayer;
				else
					return null;
			}
		}

		/// <summary>
		/// Returns a layer item on the basis of a layer name
		/// </summary>
		public GraphLayer this[string name]
		{
			get
			{
				for(int k =0; k<this.InnerList.Count; k++)
					if(this[k].Name==name)
						return this[k];
				return null;
			}
		}
		#endregion
		
		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		public GraphLayerCollection()
		{
			
		}
		#endregion

		#region Methods

		/// <summary>
		/// Overrides the base method to set all shapes in the default layer
		/// </summary>
		protected override void OnClearComplete()
		{
			base.OnClearComplete ();
			if(ClearComplete!=null)
				ClearComplete(this,EventArgs.Empty);
			
		}

		/// <summary>
		/// Adds a layer to the collection
		/// </summary>
		/// <param name="layer"></param>
		/// <returns></returns>
		public int Add(GraphLayer layer)
		{
		    for(int i = 0; i< InnerList.Count; i++)
		    {
                if (layer == InnerList[i])
		        {
                    return i;
		        }
		    }

			int index =  this.InnerList.Add(layer);
			layer.SetNumber( index);
			return index;
		}

		/// <summary>
		/// Removes a graph-layer from the collection
		/// </summary>
		/// <param name="layer"></param>
		public void Remove(GraphLayer layer)
		{
			InnerList.Remove(layer);
		}

		/// <summary>
		/// Adds a collection range to this collection
		/// </summary>
		/// <param name="col"></param>
		public void AddRange(GraphLayerCollection col)
		{
			InnerList.AddRange(col);
		}

/*
		internal void DetachHandlers()
		{

			if(ClearComplete!=null)
			{
				handler = ClearComplete;
				ClearComplete -= handler;
			}
		}

		internal void AttachHandlers()
		{
			if(handler!=null)
				ClearComplete += handler;
		}

*/
		#endregion
	}
}
