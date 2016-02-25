using System;
using System.Collections;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;

namespace Netron.GraphLib
{
	/// <summary>
	/// Tracker for connection objects.
	/// </summary>
	[Serializable]
	public class ConnectionTracker : Tracker
	{
		#region Fields
		private ArrayList grips = new ArrayList();
		#endregion

		#region Constructor
		/// <summary>
		/// Default constructor.
		/// </summary>
		public ConnectionTracker() : base() {}

		/// <summary>
		/// Constructor for a connection tracker with a list of connection segment points for the handles
		/// </summary>
		/// <param name="l">list of points for the handles</param>
		public ConnectionTracker(ArrayList l) : base()
		{
            if (l != null)
            {
                grips = l;
            }
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="points"></param>
        public ConnectionTracker(PointF[] points) :base()
		{
			for(int k=0; k<points.Length; k++)
			{
				grips.Add(points[k]);
			}
		}

		/// <summary>
		/// Constructor for a connection tracker with a list of connection segment points for 
		/// the handles and a resize state.
		/// </summary>
		/// <param name="l">list of points for the handles</param>
		/// <param name="resizable">Resize state.</param>
		public ConnectionTracker(ArrayList l, bool resizable) : base(resizable)
		{
		    if (l != null)
		    {
		        grips = l;  
		    }
			
		}

		#endregion

		#region Methods
		/// <summary>
		/// Starts tracking.
		/// </summary>
		/// <param name="p">Current mouse location</param>
		/// <param name="h">Handle</param>
		public override void Start(PointF p, Point h)
		{
			if( h.X < 0 || h.X > grips.Count )
				return;

			base.Start( p, h );
		}

		/// <summary>
		/// Performs a hit check.
		/// </summary>
		/// <param name="p">Current mouse location</param>
		/// <returns>Grip</returns>
		public override Point Hit(PointF p)
		{
			if( Resizable )
			{
				for (int i=0; i<grips.Count; i++ )
				{
					PointF pt = (PointF) grips[i];
					RectangleF r = new RectangleF(pt.X,pt.Y,0,0);
					r.Inflate(3,3);

					if( r.Contains( p ) )
						return new Point(i+1,0);
				}
			}

			return Point.Empty;
		}

		/// <summary>
		/// Returns cursor to a point
		/// </summary>
		/// <param name="p">Current mouse location</param>
		/// <returns>Cursor</returns>
		public override Cursor Cursor(PointF p)
		{
			Point h = Hit(p);
			if (h == Point.Empty) return Cursors.No;

			return MouseCursors.Grip;
		}

		/// <summary>
		/// Moves the tracking rectangle
		/// </summary>
		/// <param name="p">Current mouse location</param>
		/// <param name="maxSize"></param>
		/// <param name="snap"></param>
		/// <param name="snapSize"></param>
		public override void Move(PointF p,Size maxSize, bool snap, int snapSize)
		{
			Point h = mCurrentHandle;
			if( h.X < 0 || h.X > grips.Count )
				return;


			if( h.X == 0 )
			{
				for( int i=0; i<grips.Count; i++) 
				{
					PointF pt = (PointF)grips[i]; 

					PointF a = new PointF(0,0);
					a.X = p.X - mCurrentPoint.X;
					a.Y = p.Y - mCurrentPoint.Y;

					pt.X += a.X;
					pt.Y += a.Y;
					grips[i] = pt;
				}
			}
			else
			{
				PointF pt = (PointF)grips[h.X-1]; 

				PointF a = new PointF(0,0);
				a.X = p.X - mCurrentPoint.X;
				a.Y = p.Y - mCurrentPoint.Y;

				pt.X += a.X;
				pt.Y += a.Y;
				grips[h.X-1] = pt;
			}
			mCurrentPoint = p;
		}

		/// <summary>
		/// Moves the tracking rectangle
		/// </summary>
		/// <param name="p">Current mouse location</param>
		public virtual void MoveAll(PointF p)
		{
			for( int i=0; i<grips.Count; i++) 
			{
				PointF pt = (PointF)grips[i]; 

				PointF a = new PointF(0,0);
				a.X = p.X - mCurrentPoint.X;
				a.Y = p.Y - mCurrentPoint.Y;

				pt.X += a.X;
				pt.Y += a.Y;
				grips[i] = pt;
			}
			mCurrentPoint = p;
		}

		/// <summary>
		/// Paints the tracker.
		/// </summary>
		/// <param name="g"></param>
		public override void Paint(Graphics g)
		{
			if (!Resizable) 
				return;

			foreach (PointF p in grips )
			{
				if ((p.X != 0) || (p.Y != 0))
				{
					RectangleF r = new RectangleF(p.X-4,p.Y-4,8,8);
					g.FillRectangle(new SolidBrush(Color.Green), r.X, r.Y, r.Width - 1, r.Height - 1);
					g.DrawRectangle(new Pen(Color.Black, 1), r.X, r.Y, r.Width - 1, r.Height - 1);
				}
			}
		}

		/// <summary>
		/// Gets the grip rectangle
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public override RectangleF Grip(Point p)
		{
			RectangleF r = new RectangleF(0, 0, handleSize.Width, handleSize.Height);

			PointF pt = (PointF)grips[p.X-1];

			r.X = pt.X - handleSize.Width/2+1;
			r.Y = pt.Y - handleSize.Height/2+1;
			return r;
		}
		#endregion
	}
}
