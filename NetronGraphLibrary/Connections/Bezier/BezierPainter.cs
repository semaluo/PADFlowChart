using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
namespace Netron.GraphLib
{
	/// <summary>
	/// A Bezier curve painter
	/// </summary>
	[Serializable] public class BezierPainter : ConnectionPainter, ISerializable
	{
		#region Fields
		/// <summary>
		/// whether to show the manipulators
		/// </summary>
		//private bool mShowManipulators;

		/// <summary>
		/// the subdivision
		/// </summary>
		protected readonly int division = 25;
		/// <summary>
		/// the intermediate steps count
		/// </summary>
		private int mStepsCount;
		/// <summary>
		/// the collection of handles
		/// </summary>
		private BezierHandleCollection mHandles ;
		/// <summary>
		/// whether tracking is on
		/// </summary>
		private bool mTracking;
	

	

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets whether tracking is on
		/// </summary>
		public bool Tracking
		{
			get{return mTracking;}
			set{mTracking = value;}
		}

		/// <summary>
		/// Gets or sets where the painter starts
		/// </summary>
		public PointF From 
		{
			get{return mHandles[0].CurrentPoint;}
			set{mHandles[0].CurrentPoint = value;}
		}
		/// <summary>
		/// Gets or sets where the painter ends
		/// </summary>
		public PointF To
		{
			get{return mHandles[mHandles.Count-1].CurrentPoint;}
			set{mHandles[mHandles.Count-1].CurrentPoint = value;}
		}

	
		/// <summary>
		/// Gets or sets the collection of handles
		/// </summary>
		public BezierHandleCollection Handles
		{
			get{return mHandles;}
			set
			{
				mHandles = value;
				for(int k =0; k<mHandles.Count; k++)
				{mHandles[k].Curve = this;}
			}
		}

		/// <summary>
		/// Overrides the default since this Bezier thing is based on handles rather than simple points
		/// </summary>
		public override PointF[] Points
		{
			get{return base.Points;}
			set{
				if(value.Length!=mHandles.Count)
					throw new Exception("Length of given handle locations must be equal to the amount of handles.");
				for(int k=0;k<value.Length;k++)
				{
					mHandles[k].ChangeLocation( value[k]);
				}
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="connection">the connection this painter is painting</param>
		public BezierPainter(Connection connection) : base(connection)
		{
			base.Points = Connection.GetConnectionPoints();
			if(Points.Length>1)
			{
				mHandles = new BezierHandleCollection();
				BezierHandle hd;
					
				for(int k=1; k<Points.Length-1; k++) //this range is linked to the fact that connections have an adjacent point a little of the shape's edge
				{
					if(k==1 || k==Points.Length-2) //start and final handle should be single-type					
						hd = new BezierHandle(Points[k],HandleTypes.Single);					
					else
						hd = new BezierHandle(Points[k],HandleTypes.Symmetric);

					hd.Curve = this;
					mHandles.Add(hd);
				}
			}
			else
				throw new Exception("A curve requires at least two handles");

			Init();
		}

		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected BezierPainter(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			mHandles = info.GetValue("mHandles", typeof(BezierHandleCollection)) as BezierHandleCollection;			
		}
		
		#endregion

		#region Methods

		/// <summary>
		/// Adds an intermediate connection point
		/// </summary>
		/// <param name="p">the location of the additional connection-point</param>
		internal override void AddConnectionPoint(PointF p)
		{

			//figure out where the new point is in the series
			int before = 0;
			for(int h =0;h<mHandles.Count-1; h++)
			{
				RectangleF rec = RectangleF.Union(new RectangleF(mHandles[h].CurrentPoint,new Size(5,5)),new RectangleF(mHandles[h+1].CurrentPoint,new Size(5,5)));
				if(rec.Contains(p))
				{
					before = h; break;
				}

			}
			BezierHandle hd = new BezierHandle(p);
			hd.Curve = this;
			this.mHandles.Insert(before+1,hd);
			Init();
		}


		/// <summary>
		/// Removes a connection point
		/// </summary>
		/// <param name="p">the location of the connection-point to be removed</param>
		internal override void RemoveConnectionPoint(PointF p)
		{
			for(int k=0; k<this.mHandles.Count; k++)
			{
				if(mHandles[k].Hit(p)) RemoveHandle(mHandles[k]);
			}
		}

		/// <summary>
		/// Removes an handle
		/// </summary>
		/// <param name="handle">the handle to be removed</param>
		internal void RemoveHandle(BezierHandle handle)
		{
			if(mHandles.Count>2)
			{
				mHandles.Remove(handle);
				Init();
			}
			else
				throw new Exception("A curves requires at least to handles");		
		}
		/// <summary>
		/// Initalizes the painter
		/// </summary>
		internal void Init()
		{
			Pen =this.Connection.Pen;
			base.Points = new PointF[(mHandles.Count-1) * (division+2) ];
			mStepsCount = Convert.ToInt32(100F/(division+1));
		}

		/// <summary>
		/// Post deserialization actions
		/// </summary>
		public override void PostDeserialization()
		{
			base.PostDeserialization ();
			//some necessary cross-references
			//note that this cannnot be done at deserialization-time because not all objects are instantiated at the same time.
			for(int k=0; k<this.mHandles.Count; k++)
			{
				this.mHandles[k].Curve = this;
				
				this.mHandles[k].Tangent1.Handle = this.mHandles[k];
				this.mHandles[k].Tangent2.Handle = this.mHandles[k];

				this.mHandles[k].Tangent2.Cotangent = this.mHandles[k].Tangent1;
				this.mHandles[k].Tangent1.Cotangent = this.mHandles[k].Tangent2;

			}
			Init();
		}

		
		/// <summary>
		/// Paints the entity on the canvas
		/// </summary>
		/// <param name="g">the graphics object to paint on</param>
		public override void Paint(Graphics g)
		{
			float perc;
		
			if(Connection.From.ConnectorLocation!=ConnectorLocation.Unknown) 
				g.DrawLine(Pen,Connection.From.Location,mHandles[0].CurrentPoint);
	
			for(int h =0;h<mHandles.Count-1; h++)
			{
				//define the subpoints
				for(int k =0; k<division+2; k++)				
				{
					if(k==division+1) 
						perc = 1F;
					else
						perc = ((float) k * mStepsCount)/100F;

					Points[h*(division+2) + k] = GetBezier(perc,mHandles[h+1],mHandles[h]);									
				}
			}
			
			//g.FillPolygon(Brushes.LightSlateGray,points);
			//draw the Bezier line			
			g.DrawLines(Pen,Points);
			
//			if(mHover || mShowManipulators  || mSelected)
//			{
//				for(int m =0;m <mHandles.Count; m++)
//				{
//					mHandles[m].Paint(g);
//				}
//			}

			if(Connection.To.ConnectorLocation!=ConnectorLocation.Unknown) 
				g.DrawLine(Pen,Connection.To.Location,mHandles[mHandles.Count-1].CurrentPoint);
		}

		#region Bezier calculations
		/// <summary>
		/// the B1 coefficient
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		private static float B1(float t) { return t*t*t; }
		/// <summary>
		/// the B2 coefficient
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		private static float B2(float t) { return  3*t*t*(1-t); }
		/// <summary>
		/// the B3 coefficient
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		private static float B3(float t) { return   3*t*(1-t)*(1-t) ; }
		/// <summary>
		/// the B4 coefficient
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		private static float B4(float t) { return   (1-t)*(1-t)*(1-t) ; }
		#endregion

		/// <summary>
		/// Gets for the given sub-elemts of the Bezier curve an intermediate point
		/// </summary>
		/// <param name="percent">the percentage along the curve</param>
		/// <param name="handle1">the first handle of the segment</param>
		/// <param name="handle2">the second handle of the segment</param>
		/// <returns></returns>
		private PointF GetBezier(float percent, BezierHandle handle1, BezierHandle handle2)
		{
			TangentHandle tangent1 = null, tangent2=null;
			if(handle1.HandleType==HandleTypes.Single)
			{
				if(handle1.Tangent1.Enabled) 
					tangent1 = handle1.Tangent1;
				else
					tangent1 = handle1.Tangent2;
			}
			else
				tangent1 = handle1.Tangent2;
			if(handle2.HandleType==HandleTypes.Single)
			{
				if(handle2.Tangent1.Enabled) 
					tangent2 = handle2.Tangent1;
				else
					tangent2 = handle2.Tangent2;
			}
			else
				tangent2 = handle2.Tangent1;
			return GetBezier(percent,handle1.CurrentPoint,tangent1.CurrentPoint,tangent2.CurrentPoint,handle2.CurrentPoint);
		}

		/// <summary>
		/// Gets an intermediate point of the Bezier curve
		/// </summary>
		/// <param name="percent">the percentage along the curve</param>
		/// <param name="C1">the C1 point</param>
		/// <param name="C2">the C2 point</param>
		/// <param name="C3">the C3 point</param>
		/// <param name="C4">the C4 point</param>
		/// <returns></returns>
		private static PointF GetBezier(float percent, PointF C1, PointF C2,PointF C3,PointF C4) 
		{
			 
			float X = C1.X*B1(percent) + C2.X*B2(percent) + C3.X*B3(percent) + C4.X*B4(percent);
			float Y = C1.Y*B1(percent) + C2.Y*B2(percent) + C3.Y*B3(percent) + C4.Y*B4(percent);
			return new PointF(X,Y);
		}

	
	

			
			
		


		/// <summary>
		/// Returns whether the Bezier curve is hit by the mouse
		/// </summary>
		/// <param name="p">a point</param>
		/// <returns>whether the given point hits the connection</returns>
		/// <remarks>The curve is really a collection of segments, hence the hit is a linear combination of the basic linear Hit.</remarks>
		public override bool Hit(PointF p)
		{
			bool join = false;
			PointF p1, p2;
			RectangleF r1, r2;
			float o, u;
			PointF s;
			for(int v = 0; v<Points.Length-1; v++)
			{
						
				//this is the usual segment test
				//you can do this because the PointF object is a value type!
				p1 = Points[v]; p2 = Points[v+1];
	
				// p1 must be the leftmost point.
				if (p1.X > p2.X) { s = p2; p2 = p1; p1 = s; }

				r1 = new RectangleF(p1.X, p1.Y, 0, 0);
				r2 = new RectangleF(p2.X, p2.Y, 0, 0);
				r1.Inflate(3, 3);
				r2.Inflate(3, 3);
				//this is like a topological neighborhood
				//the connection is shifted left and right
				//and the point under consideration has to be in between.						
				if (RectangleF.Union(r1, r2).Contains(p))
				{
                    #region FIX2016021302
                    if ((int)p1.Y == (int)p2.Y)
                    {
                        return true;
                    }
                    #endregion

                    if (p1.Y < p2.Y) //SWNE
					{
						o = r1.Left + (((r2.Left - r1.Left) * (p.Y - r1.Bottom)) / (r2.Bottom - r1.Bottom));
						u = r1.Right + (((r2.Right - r1.Right) * (p.Y - r1.Top)) / (r2.Top - r1.Top));
						join |= ((p.X > o) && (p.X < u));
					}
					else //NWSE
					{
						o = r1.Left + (((r2.Left - r1.Left) * (p.Y - r1.Top)) / (r2.Top - r1.Top));
						u = r1.Right + (((r2.Right - r1.Right) * (p.Y - r1.Bottom)) / (r2.Bottom - r1.Bottom));
						join |= ((p.X > o) && (p.X < u));
					}
				}


			}
			return join;
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
			info.AddValue("mHandles", mHandles, typeof(BezierHandleCollection));
		}


		#endregion
	}
}
