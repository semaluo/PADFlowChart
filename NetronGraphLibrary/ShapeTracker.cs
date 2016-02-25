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
//using System.Runtime.Serialization.Formatters.Soap;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Netron.GraphLib.UI;
using Netron.GraphLib.Interfaces;
namespace Netron.GraphLib
{
	/// <summary>
	/// The tracker implements the possibility to resize the plex boxes, it shows the grips with which you can drag and resize the rectangle.
	/// </summary>
	


	[Serializable] public class ShapeTracker : Tracker
	{
		#region Fields
		

		private bool mSquare;
		private Shape mShape;

		#endregion

		#region Properties
		

		#endregion

		#region Constructor
		/// <summary>
		/// Class Constructor 
		/// </summary>
		/// <param name="r"></param>
		/// <param name="s"></param>
		public ShapeTracker(RectangleF r, bool s)
		{
			mRectangle = r;
			Resizable = s;
			mTrack = false;
		}
		/// <summary>
		/// Constructs a new ShapeTracker for the given IGraphSite, based on the given rectangle and whether it's resizable.
		/// </summary>
		/// <param name="site"></param>
		/// <param name="r"></param>
		/// <param name="resizable"></param>
		/// <param name="square">whether the tracker should constraint the shape to square proportions</param>
		/// <param name="shape">the shape to which this tracker applies</param>
		public ShapeTracker(IGraphSite site, RectangleF r, bool resizable, bool square, Shape shape)
		{
			mRectangle = r;
			Resizable = resizable;
			mTrack = false;
			this.mSite= site;
			this.mSquare = square;
			this.mShape = shape;
			
			
		}
		#endregion

		#region Methods
		/// <summary>
		/// Changes the location of the tracker
		/// </summary>
		/// <param name="p"></param>
		public void ChangeLocation(PointF p)
		{
			this.mRectangle.X = p.X;
			this.mRectangle.Y = p.Y;
		}
		
		/// <summary>
		/// Returns an integer point if hit with a given floating-point point
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public override Point Hit(PointF p)
		{
			// (0, 0) Canvas, (-1, -1) TopLeft, (+1, +1) BottomRight

			if (Resizable)
				for (int x = -1; x <= +1; x++)
					for (int y = -1; y <= +1; y++)
						if ((x != 0) || (y != 0))
						{
							Point h = new Point(x, y);
							if (Grip(h).Contains(p)) return h;
						}

			if (mRectangle.Contains(p)) return new Point(0, 0);

			return new Point(-2, -2);
		}
		/// <summary>
		/// Returns the cursor for the given point
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public override Cursor Cursor(PointF p)
		{

			Point h = Hit(p);
			if ((h.X < -1) || (h.X > +1) || (h.Y < -1) || (h.Y > +1)) return Cursors.No;

			if (Track)
			{
				return (h == new Point(0, 0)) ? MouseCursors.Move : MouseCursors.Cross;
			}   			
			else
			{
				if (Resizable)
				{
					if ((h == new Point(-1, -1)) || (h == new Point(+1, +1))) return Cursors.SizeNWSE;
					if ((h == new Point(-1, +1)) || (h == new Point(+1, -1))) return Cursors.SizeNESW;
					if ((h == new Point(-1,  0)) || (h == new Point(+1,  0))) return Cursors.SizeWE;
					if ((h == new Point( 0, +1)) || (h == new Point( 0, -1))) return Cursors.SizeNS;
				}
			
				if (h == Point.Empty) return MouseCursors.Select;
				
			}

			return Cursors.No;
		}
		/// <summary>
		/// Starting point of the tracker
		/// </summary>
		/// <param name="p">floating-point point</param>
		/// <param name="h">handle (point)</param>
		public override void Start(PointF p, Point h)
		{
			if ((h.X < -1) || (h.X > +1) || (h.Y < -1) || (h.Y > +1)) return;

			mCurrentHandle = h;
			mCurrentPoint = p;
			mTrack = true;
		}
		/// <summary>
		/// Ends the tracking action
		/// </summary>
		public override void End()
		{
			mTrack = false;
		}
		/// <summary>
		/// Moves the tracker to the specified location, normally attached to the mouse move.
		/// </summary>
		/// <param name="p">a point, should be the cursor location</param>
		/// <param name="snap"></param>
		/// <param name="snapSize"></param>
		/// <param name="maxSize">the maximum size allowed for tracking to the right. Should be the size of the canvas.</param>
		public override void Move(PointF p,Size maxSize, bool snap, int snapSize)
		{

			if(snap)
			{
				p=new PointF(p.X-p.X%snapSize,p.Y-p.Y%snapSize);
			}

			int minsize =(int) Math.Floor(mSite.Zoom * 30/100) ;
			Point h = mCurrentHandle;

			PointF a = new PointF(0, 0);
			PointF b = new PointF(0, 0);

			if ((h.X == -1) || ((h.X == 0) && (h.Y == 0))) a.X = p.X - mCurrentPoint.X;
			if ((h.Y == -1) || ((h.X == 0) && (h.Y == 0))) a.Y = p.Y - mCurrentPoint.Y;
			if ((h.X == +1) || ((h.X == 0) && (h.Y == 0))) b.X = p.X - mCurrentPoint.X;
			if ((h.Y == +1) || ((h.X == 0) && (h.Y == 0))) b.Y = p.Y - mCurrentPoint.Y;

			PointF tl = new PointF(mRectangle.Left, mRectangle.Top);
			PointF br = new PointF(mRectangle.Right, mRectangle.Bottom);

			tl.X += a.X;
			tl.Y += a.Y;
			br.X += b.X;
			br.Y += b.Y;

			
			if (mSite.RestrictToCanvas)
			{
				if (br.X<maxSize.Width) mRectangle.X = Math.Max(tl.X,2); //keep the object a little from the edge of the canvas
				if (br.Y<maxSize.Height) mRectangle.Y = Math.Max(tl.Y,2);
			}
			else
			{
				mRectangle.X = tl.X;
				mRectangle.Y = tl.Y;
			}

			//reshaping form when hitting the edges
		    if (mSite.RestrictToCanvas)
		    {
			    if (br.X<maxSize.Width && (br.X-tl.X)>minsize) mRectangle.Width =  br.X - tl.X;
			    if (br.Y<maxSize.Height && (br.Y - tl.Y)>minsize) mRectangle.Height = br.Y - tl.Y;			
		    }
		    else
		    {
                mRectangle.Width = br.X - tl.X;
                mRectangle.Height = br.Y - tl.Y;
		    }

			//if the shape requires the rectangle to be square:
			if(mSquare) mRectangle.Width = mRectangle.Height;

			mCurrentPoint = p;

			
		}
		/// <summary>
		/// this draws the actual little rectangles of the grips
		/// </summary>
		/// <param name="g"></param>
		public override void Paint(Graphics g)
		{
			//only paint something if the shape is resizable
			if (!Resizable || !mShape.IsSelected) return;
				
			Point p = new Point();
			//steps of one, gives nine points but we drop the (0,0) coordinate which
			//lies in the middle
			for (p.X = -1; p.X <= +1; p.X++)
				for (p.Y = -1; p.Y <= +1; p.Y++)
				{
					if ((p.X != 0) || (p.Y != 0))
					{
						RectangleF r = Grip(p);
						//now fill the rectangle and make a rectangle around in a lighter shade
						g.FillRectangle(new SolidBrush(Color.LightGray), r.X, r.Y, r.Width - 1, r.Height - 1);
						g.DrawRectangle(new Pen(Color.Gray, 1), r.X, r.Y, r.Width - 1, r.Height - 1);
					}
				}

			
		}
		/// <summary>
		/// Given a point p this returns a grip or adornment on which the user
		/// can pull to enlarge the shape
		/// </summary>
		/// <param name="p">=integer point</param>
		/// <returns>A floating-point rectangle</returns>
		public override RectangleF Grip(Point p)
		{
			//this is the basic shape
			RectangleF r = new RectangleF(0, 0, 7, 7);
			//now determine where it has to be drawn, shifted here and there
			if (p.X < 0)  r.X = mRectangle.X - 8;
			if (p.X == 0) r.X = mRectangle.X + (float) Math.Round(mRectangle.Width / 2) - 3;
			if (p.X > 0)  r.X = mRectangle.X + mRectangle.Width + 1;
			if (p.Y < 0)  r.Y = mRectangle.Y - 8;
			if (p.Y == 0) r.Y = mRectangle.Y + (float) Math.Round(mRectangle.Height / 2) - 3;
			if (p.Y > 0)  r.Y = mRectangle.Y + mRectangle.Height + 1;

			return r;
		}
		#endregion
	}
}

