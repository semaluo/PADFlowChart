using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.Serialization;
namespace Netron.GraphLib
{
	/// <summary>
	/// Abstract base class related to the Bezier connection
	/// </summary>
	[Serializable] public abstract class BezierEntity : ISerializable
	{

		#region Fields
		/// <summary>
		/// whether tracking is on
		/// </summary>
		private bool mTracking;
		/// <summary>
		/// the current point
		/// </summary>
		private PointF mCurrentPoint;
		/// <summary>
		/// the rectangle underlying the entity
		/// </summary>
		private RectangleF mRectangle ;

		/// <summary>
		/// whether hovered by the mouse
		/// </summary>
		private bool mHovered ;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the underlying rectangle
		/// </summary>
		public RectangleF Rectangle
		{
			get{return mRectangle;}
			set{mRectangle = value;}
		}

		/// <summary>
		/// Gets or sets whether the mouse is hovering this entity
		/// </summary>
		public bool Hovered
		{
			get{return mHovered;}
			set{mHovered = value;}
		}

		/// <summary>
		/// Gets or sets whether the tracking is on
		/// </summary>
		public bool Tracking
		{
			get{return mTracking;}
			set{mTracking = value;}		
		}
		/// <summary>
		/// Gets or sets the current point of this entity
		/// </summary>
		public PointF CurrentPoint
		{
			get{return mCurrentPoint;}
			set{
				mCurrentPoint = value;								
				Rectangle = new RectangleF(value, Rectangle.Size);
			}
		}

		#endregion

		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		protected BezierEntity(){}

		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected BezierEntity(SerializationInfo info, StreamingContext context) 
		{
			mCurrentPoint = (PointF) info.GetValue("mCurrentPoint", typeof(PointF));			
			mRectangle = (RectangleF) info.GetValue("mRectangle", typeof(RectangleF));
		}
		#endregion

		#region Methods
		/// <summary>
		/// Returns whether the entity is hit by the mouse at the given location
		/// </summary>
		/// <param name="p">a point</param>
		/// <returns></returns>
		internal abstract bool Hit(PointF p);
		/// <summary>
		/// Paints the entity on the canvas
		/// </summary>
		/// <param name="g">the graphics object to paint on</param>
		internal abstract void Paint(Graphics g);
		/// <summary>
		/// Changes the location of the entity to the given point
		/// </summary>
		/// <param name="p">a point</param>
		public abstract void ChangeLocation(PointF p);


		#endregion

		#region ISerializable Members
		/// <summary>
		/// ISerializable implementation
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("mCurrentPoint", mCurrentPoint, typeof(PointF));
			info.AddValue("mRectangle", mRectangle, typeof(RectangleF));
		}

		#endregion
	}
}
