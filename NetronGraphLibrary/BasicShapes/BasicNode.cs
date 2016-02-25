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
using Netron.GraphLib.Attributes;
using Netron.GraphLib.UI;
using Netron.GraphLib.Interfaces;
namespace Netron.GraphLib.BasicShapes
{
	/// <summary>
	/// A simple rectangular shape with four connectors.
	/// </summary>
	[Serializable]
	[Description("Basic node")]
	[NetronGraphShape("Basic node","8ED1469D-90B2-43ab-B000-4FF5C682F530","Basic shapes","Netron.GraphLib.BasicShapes.BasicNode",
		 "A simple rectangle with four connectors.")]
	public class BasicNode : Shape
	{
		#region Fields

		#region the connectors
		private Connector TopNode;
		private Connector BottomNode;
		private Connector LeftNode;
		private Connector RightNode;		
		#endregion
		#endregion
		
		#region Constructor

		/// <summary>
		/// Constructor
		/// </summary>
		public BasicNode() : base()
		{
			Rectangle = new RectangleF(0, 0, 70, 20);

			TopNode = new Connector(this, "Top", true);
			TopNode.ConnectorLocation = ConnectorLocation.North;
			Connectors.Add(TopNode);

			BottomNode = new Connector(this, "Bottom", true);
			BottomNode.ConnectorLocation = ConnectorLocation.South;
			Connectors.Add(BottomNode);

			LeftNode = new Connector(this, "Left", true);
			LeftNode.ConnectorLocation = ConnectorLocation.West;
			Connectors.Add(LeftNode);

			RightNode = new Connector(this, "Right", true);
			RightNode.ConnectorLocation = ConnectorLocation.East;
			Connectors.Add(RightNode);	

			IsResizable=true;
		}
		/// <summary>
		/// This is the default constructor of the class.
		/// </summary>
		public BasicNode(IGraphSite site) : base(site)
		{
			//set the default size
			Rectangle = new RectangleF(0, 0, 70, 20);
			//add the connectors
			TopNode = new Connector(this, "Top", true);
			TopNode.ConnectorLocation = ConnectorLocation.North;
			Connectors.Add(TopNode);

			BottomNode = new Connector(this, "Bottom", true);
			BottomNode.ConnectorLocation = ConnectorLocation.South;
			Connectors.Add(BottomNode);

			LeftNode = new Connector(this, "Left", true);
			LeftNode.ConnectorLocation = ConnectorLocation.West;
			Connectors.Add(LeftNode);

			RightNode = new Connector(this, "Right", true);
			RightNode.ConnectorLocation = ConnectorLocation.East;
			Connectors.Add(RightNode);		

			IsResizable=true;
		}

		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected BasicNode(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			TopNode = (Connector) info.GetValue("TopNode", typeof(Connector));
			TopNode.BelongsTo = this;
			Connectors.Add(TopNode);			

			BottomNode = (Connector) info.GetValue("BottomNode", typeof(Connector));
			BottomNode.BelongsTo = this;
			Connectors.Add(BottomNode);			

			LeftNode = (Connector) info.GetValue("LeftNode", typeof(Connector));
			LeftNode.BelongsTo = this;
			Connectors.Add(LeftNode);			

			RightNode = (Connector) info.GetValue("RightNode", typeof(Connector));
			RightNode.BelongsTo = this;
			Connectors.Add(RightNode);			
		}
		#endregion	

		#region Properties	
	
		#endregion

		#region Methods
		/// <summary>
		/// Overrides the default bitmap used in the shape viewer
		/// </summary>
		/// <returns></returns>
		public override Bitmap GetThumbnail()
		{
			Bitmap bmp=null;
			try
			{
				Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.GraphLib.BasicShapes.Resources.BasicNode.gif");
					
				bmp= Bitmap.FromStream(stream) as Bitmap;
				stream.Close();
				stream=null;
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message,"BasicNode.GetThumbnail");
			}
			return bmp;
		}
		
		/// <summary>
		/// Paints the shape of the person object in the plex. Here you can let your imagination go.
		/// MAKE IT PERFORMANT, this is a killer method called 200.000 times a minute!
		/// </summary>
		/// <param name="g">The graphics canvas onto which to paint</param>
		public override void Paint(Graphics g)
		{
			base.Paint(g);
			if(RecalculateSize)
			{
				Rectangle = new RectangleF(new PointF(Rectangle.X,Rectangle.Y),
					g.MeasureString(this.Text,Font));	
				Rectangle = System.Drawing.RectangleF.Inflate(Rectangle,10,10);
				RecalculateSize = false; //very important!
			}
			if(ShapeColor!=Color.Transparent)
			{
				GraphicsPath path = new GraphicsPath();			
				path.AddLine(Rectangle.X, Rectangle.Y, Rectangle.Right - 10, Rectangle.Y);			
				path.AddArc(Rectangle.X + Rectangle.Width - 20, Rectangle.Y, 20, 20, -90, 90);			
				path.AddLine(Rectangle.Right, Rectangle.Y + 10, Rectangle.Right, Rectangle.Bottom);			
				path.AddLine(Rectangle.Right, Rectangle.Bottom, Rectangle.Left + 10, Rectangle.Bottom);			
				path.AddArc(Rectangle.X, Rectangle.Y + Rectangle.Height - 20, 20, 20, 90, 90);			
				path.AddLine(Rectangle.X, Rectangle.Y + Rectangle.Height - 10, Rectangle.X, Rectangle.Y );			
				//shadow
				Region darkRegion = new Region(path);
				darkRegion.Translate(5, 5);

				g.FillRegion(new SolidBrush(Color.FromArgb(20, Color.Black)), darkRegion);

				g.FillPath(new SolidBrush(ShapeColor),path);
			}
			if (ShowLabel)
			{
				StringFormat sf = new StringFormat();
				sf.Alignment = StringAlignment.Center;
				g.DrawString(Text, Font, TextBrush, Rectangle.X + (Rectangle.Width / 2), Rectangle.Y + 3, sf);
			}		
			
		}

		/// <summary>
		/// Returns a floating-point point coordinates for a given connector
		/// </summary>
		/// <param name="c">A connector object</param>
		/// <returns>A floating-point pointF</returns>
		public override PointF ConnectionPoint(Connector c)
		{
			
			if (c == TopNode) return new PointF(Rectangle.Left + (Rectangle.Width * 1/2), Rectangle.Top);
			if (c == BottomNode) return new PointF(Rectangle.Left + (Rectangle.Width * 1/2), Rectangle.Bottom);
			if (c == LeftNode) return new PointF(Rectangle.Left , Rectangle.Top +(Rectangle.Height*1/2));
			if (c == RightNode) return new PointF(Rectangle.Right, Rectangle.Top +(Rectangle.Height*1/2));			
			return new PointF(0, 0);
			
		}


		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData (info, context);

			info.AddValue("TopNode", TopNode, typeof(Connector));

			info.AddValue("BottomNode", BottomNode, typeof(Connector));

			info.AddValue("LeftNode", LeftNode, typeof(Connector));

			info.AddValue("RightNode", RightNode, typeof(Connector));
		}

		


		
		#endregion
	}

}







		
