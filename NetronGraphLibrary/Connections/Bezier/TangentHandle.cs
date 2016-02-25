using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.Serialization;
namespace Netron.GraphLib
{
	/// <summary>
	/// This is the tangential manipulator that allows you to change the curvature or bending of the Bezier curve at the handles
	/// </summary>
	[Serializable] public class TangentHandle : BezierEntity, ISerializable
	{
		#region Fields
		/// <summary>
		/// the pen to draw the mHandle
		/// </summary>
		[NonSerialized] Pen pen;
		/// <summary>
		/// the mHandle to which this tangent-mHandle belongs
		/// </summary>
		BezierHandle mHandle;
		/// <summary>
		/// the cotangent
		/// </summary>
		TangentHandle mCotangent;
		/// <summary>
		/// whether this mHandle is enabled
		/// </summary>
		protected bool mEnabled = true;
		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets whether the mHandle is enabled
		/// </summary>
		public bool Enabled
		{
			get{return mEnabled;}
			set{mEnabled = value;}
		}
		/// <summary>
		/// Gets or sets the cotangent
		/// </summary>
		public TangentHandle Cotangent
		{
			get{return mCotangent;}
			set{mCotangent = value;}
		}
		
		/// <summary>
		/// Gets or sets the handle to which this tangent belongs
		/// </summary>
		public BezierHandle Handle
		{
			get{return mHandle;}
			set{mHandle = value;}
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="mHandle"></param>
		/// <param name="point"></param>
		public TangentHandle(BezierHandle mHandle, PointF point)
		{
			this.mHandle = mHandle;
			this.CurrentPoint = point;			
			Rectangle = new RectangleF(point,new SizeF(5,5));
			pen = new Pen(Color.Orange,1F);
		}

		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected TangentHandle(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			pen = new Pen(Color.Orange,1F);
		}
		#endregion


		#region Methods
		/// <summary>
		/// Paints the mHandle
		/// </summary>
		/// <param name="g"></param>
		internal override void Paint(Graphics g)
		{
			if(mEnabled)
			{
				g.DrawLine(pen,mHandle.CurrentPoint,CurrentPoint);
				if(this.Hovered)
					//g.FillRectangle(Brushes.Red,mRectangle);
					g.DrawRectangle(Pens.Turquoise,System.Drawing.Rectangle.Round(new RectangleF(this.CurrentPoint,new SizeF(10,10))));
				else
					g.FillRectangle(Brushes.Green,this.Rectangle);
			}
		}

		/// <summary>
		/// Changes the location of the mHandle to the given point
		/// </summary>
		/// <param name="p"></param>
		public override void ChangeLocation(PointF p)
		{
			//Trace.WriteLine(CurrentPoint.X + "->" + p.X);
			CurrentPoint = p;

			Rectangle = new RectangleF(CurrentPoint,new SizeF(5,5));
			
		}
		/// <summary>
		/// Changes the location of the cotangent
		/// </summary>
		/// <param name="p"></param>
		internal void ChangeCotangent(PointF p)
		{
			if(mHandle.HandleType == HandleTypes.Symmetric)
			{
				mCotangent.CurrentPoint=new PointF(2*mHandle.CurrentPoint.X-CurrentPoint.X,2*mHandle.CurrentPoint.Y - CurrentPoint.Y);				
			}
		}
		/// <summary>
		/// Returns whether this object was hit by the mouse
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		internal override bool Hit(PointF p)
		{
			RectangleF r = new RectangleF(p, new SizeF(0,0));
			RectangleF env = new RectangleF(this.CurrentPoint,new SizeF(10,10));
			env.Offset(-5,-5);
			Hovered = env.Contains(r);
			//Trace.WriteLine("(" + p.X + "," + p.Y + ") c " + "(" + CurrentPoint.X + ","  + CurrentPoint.Y +")");
			return Hovered;
		}

		#endregion

		#region ISerializable Members

		/// <summary>
		/// Serializator
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			//the CurrentPoint will be serialized in the base class
		}
		

		#endregion
	}
}
