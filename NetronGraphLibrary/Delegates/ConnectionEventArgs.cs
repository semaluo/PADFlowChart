using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;

	namespace Netron.GraphLib
	{
	
		/// <summary>
		/// Provides data for the new connection event
		/// </summary>
		public class ConnectionEventArgs : EventArgs
		{
			#region Fields
			/// <summary>
			/// the end of the connection
			/// </summary>
			private Connector to;
			/// <summary>
			/// the start of the connection
			/// </summary>
			private Connector from;
			/// <summary>
			/// the connection under consideration
			/// </summary>
			private Connection connection;
			/// <summary>
			/// whether it was added via the mouse
			/// </summary>
			private bool manual = false;
				
			#endregion

			/// <summary>
			/// Gets whether the new connection was created manually, i.e. via user interaction.
			/// If false it means that the connection was created programmatically.
			/// </summary>
			public bool Manual
			{
				get{return manual;}
			}

			/// <summary>
			/// Initializes a new instance of the ConnectionEventArgs class.
			/// </summary>			
			public ConnectionEventArgs(Connection connection)
			{
				this.connection=connection;
				this.to=connection.To;
				this.from=connection.From;
			}

			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="connection"></param>
			/// <param name="manual"></param>
			public ConnectionEventArgs(Connection connection, bool manual)
			{
				this.connection=connection;
				this.to=connection.To;
				this.from=connection.From;
				this.manual = manual;
			}

			/// <summary>
			/// Gets the newly created connection
			/// </summary> 
			public Connection Connection
			{
				get
				{ 
					return connection;
				}
			}

			/// <summary>
			/// Gets the 'to' connector of the connection
			/// </summary> 
			public Connector To
			{
				get
				{ 
					return to;
				}
			}
			/// <summary>
			/// Gets the 'from' connector of the connection
			/// </summary> 
			public Connector From
			{
				get
				{ 
					return from;
				}
			}
			
		}

	}


