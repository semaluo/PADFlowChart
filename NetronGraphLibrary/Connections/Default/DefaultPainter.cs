/*

FIX2016021302:
修复DefaultPainter的Hit方法中判断点是否在斜线形成的窄矩形时，
斜线如果成了水平线，则会出现被0除的错误。

*/

using System;
using System.Drawing;
namespace Netron.GraphLib
{
	/// <summary>
	/// The default connection painter
	/// </summary>
	[Serializable] public class DefaultPainter : ConnectionPainter
	{
		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="connection"></param>
		public DefaultPainter(Connection connection) : base(connection){}
		#endregion

		#region Methods
		/// <summary>
		/// Returns whether the default connection is hit by the mouse
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public override bool Hit(System.Drawing.PointF p)
		{
			bool join = false;
//			points = new PointF[2+insertionPoints.Count];
//			points[0] = p1;
//			points[2+insertionPoints.Count-1] = p2;
//			for(int m=0; m<insertionPoints.Count; m++)
//			{
//				points[1+m] = (PointF)  insertionPoints[m];
//			}
			PointF[] points = Points;

			PointF p1 = this.Connection.From.AdjacentPoint;
			PointF p2 = this.Connection.To.AdjacentPoint; 

			PointF s;
				float o, u;
			RectangleF r1=RectangleF.Empty, r2=RectangleF.Empty, r3=RectangleF.Empty;

			for(int v = 0; v<points.Length-1; v++)
			{
						
				//this is the usual segment test
				//you can do this because the PointF object is a value type!
				p1 = points[v]; p2 = points[v+1];
	
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

		/// <summary>
		/// Paints the connection on the canvas
		/// </summary>
		/// <param name="g"></param>
		public override void Paint(System.Drawing.Graphics g)
		{
			g.DrawLines(Pen,Points);	
			//g.DrawBeziers(pen,mPoints);
		}

		#endregion


	}
}
