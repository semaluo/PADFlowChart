using System;
using System.Collections;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace Netron.GraphLib
{
	/// <summary>
	/// Tracker for Bezier connection.
	/// This tracker consists of the tangential handles and the handles on the curve itself
	/// </summary>
	[Serializable]
	public class BezierTracker : ConnectionTracker
	{

		#region Fields
		/// <summary>
		/// the painter attached to the curve
		/// </summary>
		private BezierPainter mCurve;
		/// <summary>
		/// the collection of handles
		/// </summary>
		private BezierHandleCollection mHandles;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the underlying Bezier curve
		/// </summary>
		public BezierPainter Curve
		{
			get{return mCurve;}
			set{mCurve = value;}
		}

		/// <summary>
		/// Gets the handle collection
		/// </summary>
		public BezierHandleCollection Handles
		{
			get{return mHandles;}
		}

		#endregion

		#region Constructors
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="l"></param>
		/// <param name="resizable"></param>
		public BezierTracker(ArrayList l, bool resizable) :base(l,resizable)
		{}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="l"></param>
		public BezierTracker(ArrayList l):base(l)
		{}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="curve"></param>
		public BezierTracker(BezierPainter curve) : base(curve.Points)
		{
			mCurve = curve;
			mHandles = curve.Handles;
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="connection"></param>
		public BezierTracker(Connection connection): base(connection.GetConnectionPoints())
		{
			mCurve = connection.ConnectionPainter as BezierPainter;
			mHandles = mCurve.Handles;
		}

		#endregion

		#region Methods
		/// <summary>
		/// Paints the tracker on the canvas
		/// </summary>
		/// <param name="g"></param>
		public override void Paint(Graphics g)
		{
			
			if (!Resizable) 
				return;
			for(int m =0;m <mHandles.Count; m++)
			{
				mHandles[m].Paint(g);
			}
		}

		/// <summary>
		/// Returns whether the tracker is hit by the mouse
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public override Point Hit(PointF p)
		{
			for(int m =0;m <mHandles.Count; m++)
			{
				if(mHandles[m].Hit(p))
				{
					mHandles[m].Tracking = true; 
					mTrack = true;
					//return Point.Round(p);		
					return new Point(m,0);
				}
				if(mHandles[m].Tangent1.Hit(p))
				{
					mHandles[m].Tangent1.Tracking = true; 
					mTrack = true;
					//return Point.Round(p);		
					return new Point(m,1);
				}
				if(mHandles[m].Tangent2.Hit(p))
				{
					mHandles[m].Tangent2.Tracking = true; 
					mTrack = true;
					//return Point.Round(p);
					return new Point(m,2);
				}
			}
			mTrack = false;			
			return Point.Empty;
		}

		/// <summary>
		/// Start the tracking
		/// </summary>
		/// <param name="p"></param>
		/// <param name="h"></param>
		public override void Start(PointF p, Point h)
		{
			mCurrentHandle = h;
			mCurrentPoint = p;
			mTrack = true;
			return;
		}
		/// <summary>
		/// Moves the tracker to another location
		/// </summary>
		/// <param name="p"></param>
		/// <param name="maxSize"></param>
		/// <param name="snap"></param>
		/// <param name="snapSize"></param>
		public override void Move(PointF p, Size maxSize, bool snap, int snapSize)
		{	
			if(!mTrack) return;
			if(mCurrentHandle==Point.Empty) return;

			if(mCurrentHandle.Y==0)
			{
					mHandles[mCurrentHandle.X].ChangeLocation(p);
					//Trace.WriteLine("(" + p.X + "," + p.Y + ") ");
			}
			else if (mCurrentHandle.Y ==1)
			{
				mHandles[mCurrentHandle.X].Tangent1.ChangeLocation(p);
				mHandles[mCurrentHandle.X].Tangent1.ChangeCotangent(p);
			}
			else if (mCurrentHandle.Y ==2)
			{
				mHandles[mCurrentHandle.X].Tangent2.ChangeLocation(p);
				mHandles[mCurrentHandle.X].Tangent2.ChangeCotangent(p);
			}


			return;

		}

		/// <summary>
		/// Moves the whole connnection (including the handles)
		/// </summary>
		/// <param name="p"></param>
		public override void MoveAll(PointF p)
		{
			

			for( int i=0; i<this.mHandles.Count; i++) 
			{
				PointF pt = mHandles[i].CurrentPoint; 

				PointF delta = new PointF( p.X - mCurrentPoint.X, p.Y - mCurrentPoint.Y);

				pt.X += delta.X;
				pt.Y += delta.Y;
				mHandles[i].ChangeLocation( pt);
			}			
			base.MoveAll (p); //in case the user switches back to the base connection tracker
		}

		#endregion
	}
}
