using System;
using System.Drawing;
namespace Netron.GraphLib
{
	/// <summary>
	/// The rectangular connection painter
	/// </summary>
	public class RectangularPainter : ConnectionPainter
	{

		/// <summary>
		/// start and end of the painter
		/// </summary>
		protected PointF s,e;
		

		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="connection"></param>
		public RectangularPainter(Connection connection) : base(connection){}
		#endregion

		#region Methods

		/// <summary>
		/// Paints the connection on the canvas
		/// </summary>
		/// <param name="g"></param>
		public override void Paint(System.Drawing.Graphics g)
		{
			PointF[] points = new PointF[5];			
			switch(Connection.From.ConnectorLocation)
			{
				case ConnectorLocation.North:case ConnectorLocation.South:
					points[0] = Points[0];

					if(Connection.From.ConnectorLocation == ConnectorLocation.Unknown)
						points[1] = Points[0];
					else
						points[1] = Points[1];
								
					if(Connection.To==null || Connection.To.ConnectorLocation == ConnectorLocation.Unknown)
						points[3] = Points[Points.Length-1];
					else
						points[3] = Points[Points.Length-2];
								
					points[2] = new PointF(points[1].X,points[3].Y);
					points[4] = Points[Points.Length-1];
								
					g.DrawLines(Pen,points);
					break;
				case ConnectorLocation.East: case ConnectorLocation.West: case ConnectorLocation.Unknown:
					points[0] = Points[0];

					if(Connection.From.ConnectorLocation == ConnectorLocation.Unknown)
						points[1] = Points[0];
					else
						points[1] = Points[1];

					if(Connection.To==null || Connection.To.ConnectorLocation == ConnectorLocation.Unknown)
						points[3] = Points[Points.Length-1];
					else
						points[3] = Points[Points.Length-2];
								
					points[2] = new PointF(points[3].X,points[1].Y);
					points[4] = Points[Points.Length-1];
								
					g.DrawLines(Pen,points);
					break;

							
			}
		}

		/// <summary>
		/// Returns whether the connection is hit by the mouse
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public override bool Hit(System.Drawing.PointF p)
		{
			
			
			if (Connection.From == null) return false;
			RectangleF r1=RectangleF.Empty, r2=RectangleF.Empty, r3=RectangleF.Empty;
			PointF[] points;		
			if (((int)Points[0].X != (int)Points[Points.Length-1].X) || ((int)Points[0].Y != (int)Points[Points.Length-1].Y)) 
			{
				points = new PointF[5];
				switch(Connection.From.ConnectorLocation)
				{
					case ConnectorLocation.North:case ConnectorLocation.South:
						points[0] = Points[0];

						if(Connection.From.ConnectorLocation == ConnectorLocation.Unknown)
							points[1] = Points[0];
						else
							points[1] = Points[1];								
								
								
						if(Connection.To==null || Connection.To.ConnectorLocation == ConnectorLocation.Unknown)
							points[3] = Points[Points.Length-1];
						else
							points[3] = Points[Points.Length-2];
								
						points[2] = new PointF(points[1].X,points[3].Y);
						points[4] = Points[Points.Length-1];
						r1 = new RectangleF(points[1].X-2,points[1].Y,10,10);
						r2 = new RectangleF(points[2].X-2,points[2].Y-2,10,10);
						r3 = new RectangleF(points[3].X-2,points[3].Y-2,10,10);
								
						break;
					case ConnectorLocation.East: case ConnectorLocation.West:
						points[0] = Points[0];

						if(Connection.From.ConnectorLocation == ConnectorLocation.Unknown)
							points[1] = Points[0];
						else
							points[1] = Points[1];
								
								
								
						if(Connection.To==null || Connection.To.ConnectorLocation == ConnectorLocation.Unknown)
							points[3] = Points[Points.Length-1];
						else
							points[3] = Points[Points.Length-2];
								
						points[2] = new PointF(points[3].X,points[1].Y);
						points[4] = Points[Points.Length-1];
								
						r1 = new RectangleF(points[1].X,points[1].Y-2,10,10);
						r2 = new RectangleF(points[2].X-2,points[2].Y-2,10,10);
						r3 = new RectangleF(points[3].X-2,points[3].Y-2,10,10);
								
						break;

				}
							

				return RectangleF.Union(r1,r2).Contains(p) || RectangleF.Union(r2,r3).Contains(p);

			}
			return false;
		}


		#endregion
	}
}
