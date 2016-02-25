using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
namespace Netron.GraphLib
{
	/// <summary>
	/// Abstract base class to paint a connection
	/// </summary>
	[Serializable] public abstract class ConnectionPainter : ISerializable
	{
		#region Fields
		/// <summary>
		/// the underlying connection of this painter
		/// </summary>
		private Connection mConnection;
		/// <summary>
		/// the pen to draw with
		/// </summary>
		[NonSerialized] private Pen mPen;
		/// <summary>
		/// whether the connection is selected
		/// </summary>
		private bool mSelected ;
		/// <summary>
		/// whether the connection is hovered
		/// </summary>
		private bool mIsHovered ;
		/// <summary>
		/// the set of points to use when drawing
		/// </summary>
		private PointF[] mPoints;
		#endregion

		#region Constructor

		/// <summary>
		/// Creates a connection painter based on the given connection
		/// </summary>
		/// <param name="connection"></param>
		protected ConnectionPainter(Connection connection)
		{
			mConnection = connection;
			mPen = connection.Pen;
			mPoints = connection.GetConnectionPoints();
		}
		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected ConnectionPainter(SerializationInfo info, StreamingContext context)
		{
			//nothing; the connection is set via the property and this will init the mPoints
		}
		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the connection this painter paints
		/// </summary>
		public Connection Connection
		{
			get{return mConnection;}
			set{
				//this property set is not supposed to be used but cannot make it separately internal from the get property in Net1.1
				mConnection = value;
				mPen = value.Pen;
				mPoints = value.GetConnectionPoints();
			}
		}

		/// <summary>
		/// Gets or sets the points upon which the painting of this connection painter is based
		/// </summary>
		public virtual PointF[] Points
		{
			get{return mPoints;}
			set{mPoints = value;}
		}
		/// <summary>
		/// Gets or sets whether the mouse is hovering over this object
		/// </summary>
		internal virtual bool IsHovered
		{
			get{return mIsHovered;}
			set
			{
				mIsHovered = value;
				if(value) mPen.Width=2F; else mPen.Width=1F;				
			}
		}

		/// <summary>
		/// Gets or sets whether the connection is selected
		/// </summary>
		public virtual bool Selected
		{
			get{return mSelected;}
			set
			{
				mSelected = value;
				mPen = mConnection.Pen;
				//showManips = value;
			}
		}
		/// <summary>
		/// Gets or sets the pen used by the painter
		/// </summary>
		public Pen Pen
		{
			get{return mPen;}
			set{mPen = value;}
		}
		#endregion

		#region Methods

		/// <summary>
		/// Post-deserialization actions
		/// </summary>
		public virtual void PostDeserialization()
		{
			mPen = mConnection.Pen;
		}
		/// <summary>
		/// Handles the addition of a new (intermediate) connection point
		/// </summary>
		/// <param name="p"></param>
		internal virtual void AddConnectionPoint(PointF p){}
		/// <summary>
		/// Handles the removal of an (intermediate) connection point
		/// </summary>
		/// <param name="p"></param>
		internal virtual void RemoveConnectionPoint(PointF p){}

		/// <summary>
		/// Paints the connection on the canvas
		/// </summary>
		/// <param name="g"></param>
		public virtual void Paint(Graphics g){}

		/// <summary>
		/// Returns true if the given point hit the connection
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public virtual bool Hit(PointF p){return false;}
		#endregion

		#region ISerializable Members
		/// <summary>
		/// Serializator
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			//nothing; the connection is serialized elsewhere
		}

		#endregion
	}
}
