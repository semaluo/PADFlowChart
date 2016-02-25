using System;
using System.Collections;
using System.Runtime.Serialization;
namespace Netron.GraphLib
{
	/// <summary>
	/// STC of connections
	/// </summary>
	[Serializable] public class ConnectionCollection : CollectionBase, ISerializable, IDeserializationCallback
	{

		#region events
		/// <summary>
		/// Occurs when a connection is added to the collection
		/// </summary>
		public  event ConnectionInfo OnConnectionAdded;
		/// <summary>
		/// Occurs when a connection is removed from the collection
		/// </summary>
		public event ConnectionInfo OnConnectionRemoved;

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
		public ConnectionCollection()
		{
			
		}

		/// <summary>
		/// Constructor based on an existing array of Connection objects
		/// </summary>
		/// <param name="newarray"></param>
		public ConnectionCollection(ArrayList newarray)
		{
			InnerList.Clear();
			foreach(Connection con in newarray)
			{
				Add(con);
			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// Integer indexer
		/// </summary>
		public Connection this[int index]
		{
			get
			{
				return this.InnerList[index] as Connection;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Adds an item to the collection
		/// </summary>
		/// <param name="connection"></param>
		/// <returns></returns>
		public int Add(Connection connection)
		{
			if(connection==null) return -1;
			RaiseOnConnectionAdded(connection, true);
			return this.InnerList.Add(connection);
		}
		/// <summary>
		/// Returns whether the given item is contained in the collection
		/// </summary>
		/// <param name="connection"></param>
		/// <returns></returns>
		public bool Contains(object connection)
		{
			if(connection is Connection)
				return this.InnerList.Contains(connection);
			else
				return false;
		}		
		/// <summary>
		/// Removes an item from the collection
		/// </summary>
		/// <param name="connection"></param>
		public void Remove(Connection connection)
		{
			RaiseOnConnectionRemoved(connection, true);
			this.InnerList.Remove(connection);
		}
		/// <summary>
		/// Clones the collection
		/// </summary>
		/// <returns></returns>
		public ConnectionCollection Clone()
		{
			return new ConnectionCollection(InnerList);
		}

		/// <summary>
		/// Raises the OnConnectionAdded event
		/// </summary>
		/// <param name="con"></param>
		/// <param name="manual"></param>
		internal void RaiseOnConnectionAdded(Connection con, bool manual)
		{
			if(OnConnectionAdded!=null)
				OnConnectionAdded(this, new ConnectionEventArgs(con, manual));
		}
		/// <summary>
		/// Raises the OnShapeRemoved event
		/// </summary>
		/// <param name="con"></param>
		/// <param name="manual"></param>
		internal void RaiseOnConnectionRemoved(Connection con, bool manual)
		{
			if(OnConnectionRemoved!=null)
				OnConnectionRemoved(this, new ConnectionEventArgs(con, manual));
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
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected ConnectionCollection(SerializationInfo info, StreamingContext context)
		{
			ar = info.GetValue("CollectionBase+list", typeof(ArrayList)) as ArrayList;
		}

		#endregion		

		#region IDeserializationCallback Members

		/// <summary>
		/// IDeserializationCallback implementation
		/// </summary>
		/// <param name="sender"></param>
		public void OnDeserialization(object sender)
		{
			if(ar==null || ar.Count==0) return;
			InnerList.AddRange(ar);
		}

		#endregion
	}
}
