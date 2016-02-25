using System;
using System.Drawing;
using Netron.GraphLib.Interfaces;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
namespace Netron.GraphLib.UI
{
	/// <summary>
	/// Summary description for AutomataController.
	/// </summary>
	public class AutomataController : IWidget
	{
		#region Fields
		/// <summary>
		/// base rectangle
		/// </summary>
		private Rectangle rectangle;
		/// <summary>
		/// the site to which the widget belongs
		/// </summary>
		private GraphControl site;

		/// <summary>
		/// the stop image
		/// </summary>
		private Bitmap stopImage;
		/// <summary>
		/// the start image
		/// </summary>
		private Bitmap startImage;
		/// <summary>
		/// the refresh image
		/// </summary>
		private Bitmap refreshImage;
		#endregion

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="site"></param>
		public AutomataController(GraphControl site)
		{
			rectangle = new Rectangle(10,10,65,30);
			this.site = site;

			LoadImages();
		}

		private void LoadImages()
		{
			Bitmap bmp=null;
			try
			{
				Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.GraphLib.Resources.stop.gif");					
				bmp= Bitmap.FromStream(stream) as Bitmap;
				stream.Close();
				stopImage = bmp;

				stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.GraphLib.Resources.start.gif");					
				bmp= Bitmap.FromStream(stream) as Bitmap;
				stream.Close();
				startImage = bmp;

				stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.GraphLib.Resources.refresh.gif");					
				bmp= Bitmap.FromStream(stream) as Bitmap;
				stream.Close();
				refreshImage = bmp;

				stream=null;
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message,"AutomataController.GetThumbnail");
			}			
		}
		
		/// <summary>
		/// Returns a cursor in function of the mouse location above this widget
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public System.Windows.Forms.Cursor GetCursor(System.Drawing.PointF p)
		{			
			if(new Rectangle(15,15,20,20).Contains(Point.Round(p)))
			{
				return Cursors.Hand;
			}
			else if(new Rectangle(40,15,20,20).Contains(Point.Round(p)))
			{
				if(!site.IsAutomataRunning)
					return Cursors.Hand;
			}
			return null;
		}


		/// <summary>
		/// Returns whether this widget is hit by the mouse on the given location
		/// </summary>
		/// <param name="r"></param>
		/// <returns></returns>
		public bool Hit(System.Drawing.RectangleF r)
		{
			
			return rectangle.Contains(Rectangle.Round(r));
		}

		

		#region IPaintable Members

		public IGraphSite Site
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		/// <summary>
		/// Invalidates the widget
		/// </summary>
		public void Invalidate()
		{
		}

		public void Paint(System.Drawing.Graphics g)
		{
			g.FillRectangle(Brushes.WhiteSmoke,rectangle);
			g.DrawRectangle(Pens.DimGray,rectangle);
			if(site.IsAutomataRunning)
			{
				g.DrawImage(stopImage,15,15);
				g.DrawRectangle(Pens.Silver,40,15,20,20);
				//g.DrawString("Running..." + site.AutomataPulse.ToString() + "ms", site.Font, Brushes.DimGray, 65,20);
			}
			else
			{
				g.DrawImage(startImage,15,15);
				g.DrawImage(refreshImage,40,15);
			}
			
		}

		/// <summary>
		/// Handles the mouse down event
		/// </summary>
		/// <param name="p"></param>
		public void OnMouseDown(PointF p)
		{
			//the control checks if this controller is visible, no need to do this again
			if(new Rectangle(15,15,20,20).Contains(Point.Round(p)))
			{
				if(site.IsAutomataRunning)
					site.StopAutomata();
				else
					site.StartAutomata();
			}
			else if(new Rectangle(40,15,20,20).Contains(Point.Round(p)))
			{
				if(!site.IsAutomataRunning)
					site.ResetAutomata();
			}
		}

		public void OnMouseMove(PointF p)
		{
			if(new Rectangle(15,15,20,20).Contains(Point.Round(p)))
			{
				site.SetToolTip("Start/stop the dataflow");
			}
			else if(new Rectangle(40,15,20,20).Contains(Point.Round(p)))
			{
				if(!site.IsAutomataRunning)
					site.SetToolTip("Resets the parameters of the automata shapes.");
			}
		}

		public void OnTransmission()
		{
		
		}

		#endregion
	}
}
