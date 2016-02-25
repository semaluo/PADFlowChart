using System;
using System.Collections;

namespace Netron.GraphLib
{
	/// <summary>
	/// Implements a strongly typed collection of connectors
	/// </summary>
	[Serializable] public class ConnectorCollection : CollectionBase
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public ConnectorCollection()
		{
			
		}
		/// <summary>
		/// Adds an item to the collection
		/// </summary>
		/// <param name="connector"></param>
		/// <returns></returns>
		public int Add(Connector connector)
		{
			return this.InnerList.Add(connector);
		}
		/// <summary>
		/// Adds a range to the collection
		/// </summary>
		/// <param name="collection"></param>
		public void AddRange(ConnectorCollection collection)
		{
			this.InnerList.AddRange(collection);
		}
		/// <summary>
		/// Integer indexer
		/// </summary>
		public Connector this[int index]
		{
			get{
				return this.InnerList[index] as Connector;
			}
			set{
				if(value==null) return;
				this.InnerList[index] = value;
			}
		}

		/// <summary>
		/// String indexer
		/// </summary>
		public Connector this[string name]
		{
			get{
				for(int k=0; k< this.InnerList.Count; k++)
				{
					if((this.InnerList[k] as Connector).Text==name)
						return (this.InnerList[k] as Connector);
				}
				return null;
				}
		}
	}
}
