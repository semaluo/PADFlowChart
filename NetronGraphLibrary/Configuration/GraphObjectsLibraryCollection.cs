using System;
using System.Collections;
namespace Netron.GraphLib.Configuration
{
	/// <library>
	/// Strongly typed collection of shape libraries
	/// </library>
	public class GraphObjectsLibraryCollection : CollectionBase
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public GraphObjectsLibraryCollection()
		{			
		}

		/// <summary>
		/// Adds an item to the collection
		/// </summary>
		/// <param name="library"></param>
		/// <returns></returns>
		public int Add(GraphObjectsLibrary library)
		{
			return this.InnerList.Add(library);
		}
		/// <summary>
		/// Integer indexer
		/// </summary>
		public GraphObjectsLibrary this[int index]
		{
			get{return this.InnerList[index] as GraphObjectsLibrary;}
		}
		/// <summary>
		/// Returns the summary for a shape with the given key
		/// </summary>
		/// <param name="shapeKey"></param>
		/// <returns></returns>
		public ShapeSummary GetShapeSummary(string shapeKey)
		{
			for(int k=0; k<this.InnerList.Count; k++)
			{
				for(int m=0; m<this[k].ShapeSummaries.Count; m++)
					if(this[k].ShapeSummaries[m].Key==shapeKey)
						return this[k].ShapeSummaries[m];
			}
			return null;
		}

		/// <summary>
		/// Returns the summary for the connection with the given key
		/// </summary>
		/// <param name="connectionName"></param>
		/// <returns></returns>
		public ConnectionSummary GetConnectionSummary(string connectionName)
		{
			for(int k=0; k<this.InnerList.Count; k++)
			{
				for(int m=0; m<this[k].ConnectionSummaries.Count; m++)
					if(this[k].ConnectionSummaries[m].Name==connectionName)
						return this[k].ConnectionSummaries[m];
			}
			return null;
		}


	}
}
