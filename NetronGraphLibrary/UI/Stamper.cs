using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;
using Netron.GraphLib;
using Netron.GraphLib.Configuration;
using Netron.GraphLib.Attributes;
namespace Netron.GraphLib.UI
{
	/// <summary>
	/// Summary description for NetronOverview.
	/// </summary>
	[ToolboxItem(true)]
	public class Stamper : System.Windows.Forms.UserControl
	{
		internal GraphControl graphControl = null;
		private float      zoom = 0.2f;      
		private Rectangle   drawingBounds = Rectangle.Empty;
		private bool        dragging = false;
		private Point       currentPoint = Point.Empty;
		private System.Windows.Forms.Timer timer1;
		private System.ComponentModel.IContainer components;
		private Pen	 dashPen;
		/// <summary>
		/// Default constructor.
		/// </summary>
		public Stamper()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);

			dashPen = new Pen(Brushes.Gray,1F);
			dashPen.DashStyle=DashStyle.Dot;
		}

		/// <summary>
		/// Gets or sets the panel a Overview control belongs to.
		/// </summary>
		public GraphControl GraphControl
		{
		
			set 
			{ 
				if( value != null )
				{
					timer1.Enabled = true;
					graphControl = value;  
				}
			}
		}

		/// <summary>
		/// Gets or sets the zoom factor to be used for overview drawing. 
		/// </summary>
		public Single Zoom
		{
			get { return this.zoom;  }
			set { this.zoom = value; }
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			// 
			// timer1
			// 
			this.timer1.Interval = 700;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// NetronOverview
			// 
			this.AutoScroll = true;
			this.Name = "NetronOverview";
			this.Size = new System.Drawing.Size(224, 248);

		}
		#endregion

		/// <summary>
		/// Starts dragging of the drawing bounding rectangle
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			Point pt = new Point( e.X, e.Y );
			pt = this.PointToClient(pt);
			
			currentPoint = pt;
			dragging = true;						
		}

		/// <summary>
		/// Perform dragging on mouse move if enabled
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if( dragging )
			{
				Point pt = new Point( e.X, e.Y );
				pt = this.PointToClient(pt);

				drawingBounds.Offset( (int)((pt.X-currentPoint.X)/(0.2*zoom)), (int)((pt.Y-currentPoint.Y)/(0.2*zoom)) );
				graphControl.AutoScrollPosition =drawingBounds.Location;
				
				currentPoint = pt;
				Invalidate();
				this.graphControl.Invalidate();
			}
		}

		/// <summary>
		/// Ends dragging of drawings bounding rectangle.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			dragging = false;
		}

		/// <summary>
		/// Event handler called on a key is hold down.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			Point pt = Point.Empty;
			switch (e.KeyCode )
			{
				case Keys.Left:
					pt = new Point(-1,0);
					break;
				case Keys.Right:
					pt = new Point(1,0);
					break;
				case Keys.Down:
					pt = new Point(0,1);
					break;
				case Keys.Up:
					pt = new Point(0,-1);
					break;
			}

			if( pt != Point.Empty )
			{
				drawingBounds.Offset( pt );
				graphControl.AutoScrollPosition = drawingBounds.Location;
				Invalidate();
			}

			base.OnKeyDown(e);
		}


		/// <summary>
		/// Performs painting
		/// </summary>
		/// <param name="e">Paint-Event arguments</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			if( graphControl != null )
			{
				Graphics g = e.Graphics;


				


				Rectangle b = ZoomRectangle(Rectangle.Round(graphControl.extract.Rectangle));
				AutoScrollMinSize = b.Size;

				//GC.Collect();		this is expensive!!		

				Rectangle r = Rectangle.Round(g.ClipBounds);

				// Scaling
				g.ScaleTransform(this.zoom,this.zoom);
			
				g.DrawRectangle(dashPen,0,0,graphControl.Width,graphControl.Height);

				// Paint structure of abstract 
				graphControl.extract.Paint(g);

				// Paint netron panel bounding rectangle.
				Pen p = new Pen( Color.Blue, 2 );

				System.Drawing.Drawing2D.GraphicsContainer gc = g.BeginContainer();
				drawingBounds = graphControl.ClientRectangle;
				
				//this rectangle represents the page
				//Pen ppage = new Pen(Brushes.Gray,1F);
				//ppage.DashStyle = DashStyle.Dash;
				//g.DrawRectangle(ppage, (drawingBounds.Width-graphControl.PageSize.Width)/2,(drawingBounds.Height-graphControl.PageSize.Height)/2,graphControl.PageSize.Width,graphControl.PageSize.Height);
				
				g.TranslateTransform(this.AutoScrollPosition.X, this.AutoScrollPosition.Y,MatrixOrder.Append);
				g.ScaleTransform(1/graphControl.Zoom,1/graphControl.Zoom,MatrixOrder.Prepend);				
				//if(graphControl.Zoom!=1)					g.TranslateTransform(graphControl.Width*(1-1/graphControl.Zoom)/2,graphControl.Height*(1-1/graphControl.Zoom)/2,MatrixOrder.Append);
				//drawingBounds.Offset( -graphControl.AutoScrollPosition.X, -graphControl.AutoScrollPosition.Y );
				
				
//				g.TranslateTransform(this.AutoScrollPosition.X, this.AutoScrollPosition.Y,MatrixOrder.Append);
//				g.ScaleTransform(this.currentZoomFactor,this.currentZoomFactor,MatrixOrder.Append);
//				g.TranslateTransform(-Width*(currentZoomFactor-1)/2,-Height*(currentZoomFactor-1)/2,MatrixOrder.Append);


				drawingBounds.Offset( -graphControl.AutoScrollPosition.X, -graphControl.AutoScrollPosition.Y );
				g.DrawRectangle(p,drawingBounds );
				
				g.EndContainer(gc);
			}
		}

		/// <summary>
		/// Timer invalidates the overview frequently to update its content
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timer1_Tick(object sender, System.EventArgs e)
		{
			Invalidate();
		}

		#region INetronSite implementation

		

		/// <summary>
		/// Gets the current scroll position
		/// </summary>
		public Point ScrollPosition 
		{ 
			get { return this.AutoScrollPosition; }
		}

		/// <summary>
		/// Create a graphics context
		/// </summary>
		public Graphics Graphics
		{
			get { return CreateGraphics(); }
		}

		/// <summary>
		/// Let the site invalidate the rectangle
		/// </summary>
		/// <param name="rect">invalid rectangle</param>
		public void InvalidateRectangle( Rectangle rect )
		{
			Invalidate( rect );
		}

		/// <summary>
		/// Zooms given point.
		/// </summary>
		/// <param name="originalPt">Point to zoom</param>
		/// <returns>zoomed point.</returns>
		public Point ZoomPoint(Point originalPt)
		{
			Point newPt = new Point((int)(originalPt.X * this.zoom), (int)(originalPt.Y * this.zoom));
			return newPt; 
		}

		/// <summary>
		/// Unzooms given point.
		/// </summary>
		/// <param name="originalPt">Point to unzoom</param>
		/// <returns>Unzoomed point.</returns>
		public Point UnzoomPoint(Point originalPt)
		{
			Point newPt = new Point((int)(originalPt.X / this.zoom), (int)(originalPt.Y / this.zoom));
			return newPt;
		}

		/// <summary>
		/// Zooms given rectangle.
		/// </summary>
		/// <param name="originalRect">Rectangle to zoom</param>
		/// <returns>Zoomed rectangle</returns>
		public Rectangle ZoomRectangle(Rectangle originalRect)
		{
			Rectangle newRect = new Rectangle((int)(originalRect.X * this.zoom), (int)(originalRect.Y * this.zoom), 
				(int)(originalRect.Width * this.zoom), (int)(originalRect.Height * this.zoom));
			return newRect; 
		}

		/// <summary>
		/// Unzooms given rectangle. 
		/// </summary>
		/// <param name="originalRect">Rectangle to unzoom</param>
		/// <returns>Unzoomed rectangle.</returns>
		public Rectangle UnzoomRectangle(Rectangle originalRect)
		{
			Rectangle newRect = new Rectangle((int)(originalRect.X / this.zoom), (int)(originalRect.Y / this.zoom), 
				(int)(originalRect.Width / this.zoom), (int)(originalRect.Height / this.zoom));
			return newRect;
		}

		/// <summary>
		/// Zooms a given point.
		/// </summary>
		/// <param name="originalPt">Point to zoom</param>
		/// <returns>Zoomed point.</returns>
		public PointF ZoomPoint(PointF originalPt)
		{
			PointF newPt = new PointF(originalPt.X * this.zoom, originalPt.Y * this.zoom);
			return newPt; 
		}

		/// <summary>
		/// Unzooms a given point.
		/// </summary>
		/// <param name="originalPt">Point to unzoom.</param>
		/// <returns>Unzoomed point.</returns>
		public PointF UnzoomPoint(PointF originalPt)
		{
			PointF newPt = new PointF(originalPt.X / this.zoom, originalPt.Y / this.zoom);
			return newPt;
		}

		/// <summary>
		/// Zooms a given rectangle.
		/// </summary>
		/// <param name="originalRect">Rectangle to zoom.</param>
		/// <returns>Zoomed rectangle.</returns>
		public RectangleF ZoomRectangle(RectangleF originalRect)
		{
			RectangleF newRect = new RectangleF(originalRect.X * this.zoom, originalRect.Y * this.zoom, 
				originalRect.Width * this.zoom, originalRect.Height * this.zoom);
			return newRect; 
		}

		/// <summary>
		/// Unzooms given rectangle.
		/// </summary>
		/// <param name="originalRect">Rectangle to unzoom.</param>
		/// <returns>Unzoomed rectangle.</returns>
		public RectangleF UnzoomRectangle(RectangleF originalRect)
		{
			RectangleF newRect = new RectangleF(originalRect.X / this.zoom, originalRect.Y / this.zoom, 
				originalRect.Width / this.zoom, originalRect.Height / this.zoom);
			return newRect;
		}
		#endregion

	}
}
