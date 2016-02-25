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
	/// A handle with which the connection can be manipulated
	/// </summary>
	[Serializable] public class BezierHandle : BezierEntity, IDisposable, ISerializable
	{
		#region Fields

		/// <summary>
		/// handle type
		/// </summary>
		private HandleTypes mHandleType = HandleTypes.Symmetric;
		/// <summary>
		/// first handle
		/// </summary>
		private TangentHandle mTangent1;
		/// <summary>
		/// second handle
		/// </summary>
		private TangentHandle mTangent2;
		/// <summary>
		/// the pen to draw lines
		/// </summary>
		[NonSerialized] private Pen pen ;
		/// <summary>
		/// the Bezier painter
		/// </summary>
		[NonSerialized]  private BezierPainter mCurve ;
		/// <summary>
		/// whether the handle's shifts should be constrained to the vertical only
		/// </summary>
		private bool mVerticalConstraint;

		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the curve to which this handle belongs
		/// </summary>
		public BezierPainter Curve
		{
			get{return mCurve;}
			set{mCurve = value;}
		}
		/// <summary>
		/// Gets or sets the handle type
		/// </summary>
		public HandleTypes HandleType
		{
			get{return mHandleType;}
			set{
				mHandleType = value;
				if(value==HandleTypes.Symmetric)
				{
					this.Tangent1.ChangeCotangent(Tangent1.CurrentPoint);
				}
			}
		}
		/// <summary>
		/// Gets or sets the first tangent
		/// </summary>
		public TangentHandle Tangent1
		{
			get{return mTangent1;}
			set{mTangent1 = value;}
		}
		/// <summary>
		/// Gets or sets the second tangent
		/// </summary>
		public TangentHandle Tangent2
		{
			get{return mTangent2;}
			set{mTangent2 = value;}
		}

		/// <summary>
		/// Gets or sets whether the change should be constrained
		/// </summary>
		/// <remarks>This property is not used in the graph library and is useful if you use
		///the Bezier stuff in a chart or similar 
		///</remarks>
		public bool VerticalConstraint
		{
			get{return mVerticalConstraint;}
			set{mVerticalConstraint = value;}
		}

		#endregion

		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="x">the x-coordinate of the handle</param>
		/// <param name="y">the y-coordinate of the handle</param>
		public BezierHandle(float x, float y)
		{
			this.CurrentPoint = new PointF(x,y);
			Init();
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="x">the x-coordinate of the handle</param>
		/// <param name="y">the y-coordinate of the handle</param>
		/// <param name="type">the handle type</param>
		public BezierHandle(float x, float y, HandleTypes type)
		{
			this.CurrentPoint = new PointF(x,y);
			this.mHandleType = type;
			Init();
			if(type == HandleTypes.Single)
			{
				this.Tangent1.Enabled = true;
				this.Tangent2.Enabled = false;
			}
			
		}
		/// <summary>
		/// Constructor
		/// </summary>
		public BezierHandle()
		{
			CurrentPoint = PointF.Empty;
			Init();
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="p">a point</param>
		public BezierHandle(PointF p)
		{
			this.CurrentPoint = p;
			Init();
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="p">a point</param>
		/// <param name="type">the handle type</param>
		public BezierHandle(PointF p, HandleTypes type)
		{
			this.CurrentPoint = p;
			this.mHandleType = type;
			Init();
			if(type == HandleTypes.Single)
			{
				this.Tangent1.Enabled = true;
				this.Tangent2.Enabled = false;
			}
			
		}


		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected BezierHandle(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			mTangent1 = info.GetValue("mTangent1", typeof(TangentHandle)) as TangentHandle;
			mTangent2 = info.GetValue("mTangent2", typeof(TangentHandle)) as TangentHandle;

			mTangent1.Cotangent = mTangent2;
			mTangent2.Cotangent = mTangent1;


			pen = new Pen(Color.OrangeRed);
		}
		#endregion
		
		#region Methods

		/// <summary>
		/// Initializes the handle
		/// </summary>
		private void Init()
		{
			this.Rectangle = new RectangleF(CurrentPoint,new SizeF(6,6));
			mTangent1 = new TangentHandle(this, new PointF(CurrentPoint.X-40,CurrentPoint.Y+40));
			mTangent2 = new TangentHandle(this, new PointF(CurrentPoint.X+40,CurrentPoint.Y-40));
			
			//this helps the symmetric behavior
			mTangent1.Cotangent = mTangent2;
			mTangent2.Cotangent = mTangent1;

			//mTangent1.Enabled = false;
			pen = new Pen(Color.OrangeRed);

		}

		/// <summary>
		/// Returns whether the mouse hit this handle
		/// </summary>
		/// <param name="p">a point</param>
		/// <returns>returns whether the given point hit the handle</returns>
		internal override bool Hit(PointF p)
		{		
			RectangleF r = new RectangleF(p, new SizeF(0,0));
			RectangleF env = new RectangleF(CurrentPoint,new SizeF(8,8));
			//env.Offset(-4,-4);
			return env.Contains(r);
		}

		/// <summary>
		/// Changes the location of the handle to the given point
		/// </summary>
		/// <param name="p">the new location of the handle</param>
		public override void ChangeLocation(PointF p)
		{	
			if(!mVerticalConstraint)
			{
				mTangent1.CurrentPoint=new PointF(p.X + Tangent1.CurrentPoint.X-CurrentPoint.X, p.Y+Tangent1.CurrentPoint.Y-CurrentPoint.Y);
				mTangent2.CurrentPoint=new PointF(p.X + Tangent2.CurrentPoint.X-CurrentPoint.X, p.Y+Tangent2.CurrentPoint.Y-CurrentPoint.Y);
				//this.CurrentPoint.X = p.X;
				this.CurrentPoint = new PointF(p.X,CurrentPoint.Y);
			}
			else
			{
				mTangent1.CurrentPoint=new PointF( Tangent1.CurrentPoint.X , p.Y+Tangent1.CurrentPoint.Y-CurrentPoint.Y);
				mTangent2.CurrentPoint=new PointF( Tangent1.CurrentPoint.X , p.Y+Tangent2.CurrentPoint.Y-CurrentPoint.Y);
			}
			this.CurrentPoint = new PointF(CurrentPoint.X, p.Y);
			this.Rectangle = new RectangleF(CurrentPoint,new SizeF(5,5));
				
		}


		/// <summary>
		/// Paints the handle
		/// </summary>
		/// <param name="g">the graphics objects to paint on</param>
		internal override void Paint(Graphics g)
		{
			
			if(this.Hovered) 			
				g.DrawRectangle(pen,System.Drawing.Rectangle.Round(new RectangleF(CurrentPoint,new SizeF(8,8))));
			else
				g.FillRectangle(Brushes.Green,this.Rectangle);

			this.mTangent1.Paint(g);
			this.mTangent2.Paint(g);
		}
		#region IDisposable Members

		/// <summary>
		/// Disposes the graphics object
		/// </summary>
		public void Dispose()
		{
			this.pen.Dispose();
		}

		#endregion

		/// <summary>
		/// Serializator
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
	
			info.AddValue("mTangent1", mTangent1, typeof(TangentHandle));
			info.AddValue("mTangent2", mTangent2, typeof(TangentHandle));
		}

		#endregion
	}
}
