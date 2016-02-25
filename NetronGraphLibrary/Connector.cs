using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Netron.GraphLib.Interfaces;
using System.Security;
using System.Security.Permissions;
namespace Netron.GraphLib
{
	/// <summary>
	/// A Connector aka 'connection point' is the spot on a shape where a line (connection) will attach itself.
	/// Lits up when cursor is nearby and can contain in/outflow data that can propagate through the connections.
	/// </summary>
	/// <remarks>
	/// Things you can do:
	/// <br>- making the connector blink or flash when hit</br>
	/// <br>- show an extensive information box when hovered</br>
	/// <br>- attach a status message when hovered</br>
	/// <br>- differentiate different connector on their propagation type or their parnet/child relation</br>
	///</remarks>
	
		
	[Serializable] public  class Connector : Entity, ISerializable
	{

		#region Fields
		/// <summary>
		/// gives a little displacement between the connection and the connector
		/// </summary>
		private float mConnectionShift = 15F;

		/// <summary>
		/// determines the place of the connection shift
		/// </summary>
		private ConnectorLocation mConnectorLocation = ConnectorLocation.Unknown;

		/// <summary>
		/// the shift point
		/// </summary>
		private PointF mAdjacentPoint = PointF.Empty;

		/// <summary>
		/// only 1 connection allowed if false
		/// </summary>
		private bool mAllowMultipleConnections;  	
        	
		/// <summary>
		/// object this connector belongs to.
		/// </summary>
		private Shape mBelongsTo; 

		/// <summary>
		/// connections attached to this connector
		/// </summary>
		private ConnectionCollection mConnections; 

		/// <summary>
		/// collection of objects that the connector propagates
		/// </summary>
		private ArrayList mSendList;  

		/// <summary>
		/// collection of values/objects that the connector receives from other connectors
		/// </summary>
		private ArrayList mReceiveList; 

		/// <summary>
		/// name of the connector
		/// </summary>
		private string mName;

		/// <summary>
		/// allow new connections to be launched from this connector
		/// </summary>
		private bool mAllowNewConnectionsFrom = true;

		/// <summary>
		/// allow new connection to be attached to this connector
		/// </summary>
		private bool mAllowNewConnectionsTo = true;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets whether to allow new connections to be launched from this connector
		/// </summary>
		public bool AllowNewConnectionsFrom
		{
			get{return mAllowNewConnectionsFrom;}
			set{mAllowNewConnectionsFrom = value;}
		}

		/// <summary>
		/// Gets or sets whether to allow new connections to be attached to this connector
		/// </summary>
		public bool AllowNewConnectionsTo
		{
			get{return mAllowNewConnectionsTo;}
			set{mAllowNewConnectionsTo = value;}
		}
		/// <summary>
		/// Gets or sets the name of the connector.
		/// 
		/// </summary>
		/// <remarks>
		/// This property makes it possible to deserialize a connector, 
		/// it's the only way to find back where a 
		/// serialized connector came from.
		/// </remarks>
		public string Name
		{
			get{return mName;}
			set{mName = value;}
		}

		/// <summary>
		/// Gets or sets the connection shift with respect to this connector.
		/// If the type is 'Omni' it's an offset in the direction of the connection,
		/// otherwise it creates a little shift/break in the connection 
		/// in the direction specified by the ConnectorLocation.
		/// </summary>
		public float ConnectionShift
		{
			get{return mConnectionShift;}
			set{mConnectionShift = value;}
		}

		/// <summary>
		/// The location of the connector
		/// </summary>
		public PointF Location
		{
			get{return mBelongsTo.ConnectionPoint(this);}
		}

		/// <summary>
		/// Gets or sets whether the connector can have multiple connection attached
		/// </summary>	
		public bool AllowMultipleConnections
		{
			get{return mAllowMultipleConnections;}
			[Browsable(false)] set{mAllowMultipleConnections = value;}
		}

		/// <summary>
		/// Gets the connections of a connector
		/// </summary>
		public ConnectionCollection Connections
		{
			get
			{
				return mConnections;
			}
		}

		/// <summary>
		/// The values/objects that the connector propagates
		/// </summary>
		public ArrayList Sends
		{
			get
			{
				return mSendList;
			}
//			[Browsable(false)] set
//			{
//				mSendList= value;
//			}
		}

		/// <summary>
		/// The values/objects that the connectors receives from other connectors
		/// </summary>
		public ArrayList Receives
		{
			get
			{
				return mReceiveList;
			}
//			[Browsable(false)]set
//			{
//				mReceiveList= value;
//			}
		}

		/// <summary>
		/// The get/Set the ShapeObjects this connector is attached to
		/// </summary>
		public Shape BelongsTo
		{
			get
			{
				return mBelongsTo;
			}
			[Browsable(false)] set
			{
				mBelongsTo = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the adjacent point which allows to have a little distance 
		/// between shapes and connections
		/// </summary>
		public PointF AdjacentPoint
		{
			get
			{
				
				PointF p = mBelongsTo.ConnectionPoint(this);
				switch(this.mConnectorLocation)
				{
					case ConnectorLocation.North: this.mAdjacentPoint= new PointF(p.X,p.Y - mConnectionShift); break;
					case ConnectorLocation.East: this.mAdjacentPoint= new PointF(p.X+mConnectionShift,p.Y); break;
					case ConnectorLocation.South: this.mAdjacentPoint= new PointF(p.X,p.Y + mConnectionShift); break;
					case ConnectorLocation.West: this.mAdjacentPoint= new PointF(p.X-mConnectionShift,p.Y); break;					
					case ConnectorLocation.Omni: case ConnectorLocation.Unknown: this.mAdjacentPoint= p; break;
				}

				
				return mAdjacentPoint;
			}
		}


		/// <summary>
		/// Gets or sets the location of the connector which will determine where the adjacent point will be
		/// </summary>
		public ConnectorLocation ConnectorLocation
		{
			get{return mConnectorLocation;}
			[Browsable(false)] set{mConnectorLocation = value;}
		}

		#endregion	
		
		#region Constructors
		/// <summary>
		/// Constructor of the connector clss
		/// </summary>
		/// <param name="o">the underlying shape to which the connector belongs</param>
		/// <param name="connectorName">the name of the connector</param>
		/// <param name="multipleConnections">whether the connector allows multiple connections to be added or connected to it</param>		
		public Connector(Shape o, string connectorName, bool multipleConnections) : base()
		{
			mBelongsTo = o;
			mName = Text = connectorName;
			mConnections = new ConnectionCollection();
			mSendList = new ArrayList();
			mReceiveList = new ArrayList();
			mAllowMultipleConnections = multipleConnections;				
		}

		/// <summary>
		/// Internal constructor, related to deserialization
		/// </summary>
		/// <param name="uid"></param>
		internal Connector(string uid) : base()
		{
			this.UID = new Guid(uid);
		}

		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected Connector(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.mName = info.GetString("mName");

			this.mAllowMultipleConnections = info.GetBoolean("mAllowMultipleConnections");

			this.ConnectorLocation = (ConnectorLocation) info.GetValue("mConnectorLocation", typeof(ConnectorLocation));

			this.mConnectionShift = info.GetSingle("mConnectionShift");

			try
			{
				this.mAllowNewConnectionsFrom = info.GetBoolean("mAllowNewConnectionsFrom");
			}
			catch
			{
				this.mAllowNewConnectionsFrom = true;
			}
			try
			{
				this.mAllowNewConnectionsTo = info.GetBoolean("mAllowNewConnectionsTo");
			}
			catch
			{
				this.mAllowNewConnectionsTo = true;
			}

			mConnections = new ConnectionCollection();
			mSendList = new ArrayList();
			mReceiveList = new ArrayList();
		}
		#endregion

		#region Methods

		/// <summary>
		/// Implements the abstract method of the Entity class
		/// </summary>
		/// <param name="g"></param>
		public override void PaintAdornments(Graphics g)
		{
			return;
		}

		/// <summary>
		/// Says wether the given RectangleF is contained inside this connector
		/// </summary>
		/// <param name="r">the RectangleF as a candidate, usually the mouse coordinates converted to a zero sized rectangle.</param>
		/// <returns>True/false</returns>
		public override bool Hit(RectangleF r)
		{
			if (((int)r.Width == 0) && ((int)r.Height == 0))
				return ConnectionGrip().Contains(r.Location); 

			return r.Contains(ConnectionGrip());
		}
		/// <summary>
		/// Overrides the Paint of the control and paint a little connection point 
		/// or a highlighted connecting widget to 
		/// show the user that a connection is possible.
		/// </summary>
		/// <remarks>
		/// The parent's Hover boolean can be used to check if the mouse is currently hovering over this object. This enables a status message or a different shape.
		/// </remarks>
		/// <param name="g">The Graphics or canvas onto which to paint.</param>
		public override void Paint(Graphics g)
		{
			Rectangle r = Rectangle.Round(ConnectionGrip());
				
			Color Line = Color.White;
			if (IsHovered) Line = Color.Black;
      
			Color Fill = Color.FromArgb(49, 69, 107); // dark blue
			if (IsHovered)
			{
				
				if ((mAllowMultipleConnections) || (mConnections.Count < 1))
					Fill = Color.FromArgb(0, 192, 0); // medium green
				else
					Fill = Color.FromArgb(255, 0, 0); // red
			}

			g.FillRectangle(new SolidBrush(Fill), r);
			g.DrawRectangle(new Pen(Line, 1), r);

			/*This piece of code has been replaced by the tooltip
			 * 
			if (IsHovered)
			{
				Font f = new Font("Tahoma", 8.25f);
				Size s = g.MeasureString(Text + " [" + this.Connections.Count + "]", f).ToSize();
				Rectangle a = new Rectangle(r.X - (s.Width / 2), r.Y + s.Height + 6, s.Width, s.Height + 1);
				Rectangle b = a;
				a.Inflate(+3, +2);
      
				g.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, 231)), a);
				g.DrawRectangle(new Pen(Color.Black, 1), a);
				g.DrawString(Text + " [" + this.Connections.Count + "]", f, new SolidBrush(Color.Black), b.Location);
			}
			*/

		}
		/// <summary>
		/// Necessary implementation of the abstract delete method defined in Entity
		/// </summary>
		protected internal override void Delete()
		{

		}

		/// <summary>
		/// Update/refresh the connector's appearance
		/// </summary>
		public override void Invalidate()
		{
			if (mBelongsTo == null) return;
			IGraphSite c = mBelongsTo.Site;//should return the underlying canvasobject
			if (c == null) return;
			RectangleF r = ConnectionGrip();//get the neighborhood of the connector
			if (IsHovered) r.Inflate(+100, +100); // make sure a sufficient piece of the neighborhood will be refreshed.
			c.Invalidate(Rectangle.Round(r)); //...and refresh it by calling the control's invalidate method.
		}
		/// <summary>
		/// Returns the cursor for the current connector
		/// </summary>
		/// <param name="p">The cursor location</param>
		/// <returns>A grip cursor, looks like a focus/target</returns>
		public override Cursor GetCursor(PointF p)
		{
				
			return MouseCursors.Grip; 
		}
		/// <summary>
		/// Represents the spot around a connector that lits up and where the connections is attaching itself
		/// The color is determined by various things, can be red, grey or green. See the Hover conditions in the paint handler for this.
		/// </summary>
		/// <returns>A little rectangleF (3x3)</returns>
		public RectangleF ConnectionGrip()
		{
			PointF p = mBelongsTo.ConnectionPoint(this);
			RectangleF r = new RectangleF(p.X, p.Y, 0, 0);
			r.Inflate(+3, +3);
			return r;
		}


		
		#endregion

		#region ISerializable Members

		/// <summary>
		/// ISerializable implementation
		/// </summary>
		/// <param name="info">the serialization info</param>
		/// <param name="context">the streaming context</param>
		[SecurityPermissionAttribute(SecurityAction.Demand,SerializationFormatter=true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);

			info.AddValue("mName", this.mName);

			info.AddValue("mAllowMultipleConnections", this.mAllowMultipleConnections);

			info.AddValue("mConnectorLocation", this.mConnectorLocation,typeof(ConnectorLocation));

			info.AddValue("mConnectionShift", this.mConnectionShift, typeof(float));

			info.AddValue("mAllowNewConnectionsFrom", this.mAllowNewConnectionsFrom);

			info.AddValue("mAllowNewConnectionsTo", this.mAllowNewConnectionsTo);

		}

		#endregion
	}
}

