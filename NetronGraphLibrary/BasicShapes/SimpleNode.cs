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
using System.Security;
using System.Security.Permissions;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Netron.GraphLib.Attributes;
using Netron.GraphLib.UI;
using Netron.GraphLib.Interfaces;

namespace Netron.GraphLib.BasicShapes
{
	/// <summary>
	/// A simple rectangular shape with one connector.
	/// </summary>
	[Serializable]
	[Description("Basic node")]
	[NetronGraphShape("Simple node","57AF94BA-4129-45dc-B8FD-F82CA3B4433E","Basic shapes","Netron.GraphLib.BasicShapes.SimpleNode",
		 "A simple rectangular, non-resizable shape with a single connector.")]
	public class SimpleNode : Shape, ISerializable
	{
		#region Fields	
		//the only connector on this shape
		private Connector leftConnector;

		#endregion

		#region Properties
		

		#endregion	
		
		#region Constructors
		

		/// <summary>
		/// This is the default constructor of the class.
		/// </summary>
		public SimpleNode() : base()
		{
			//set the default size
			Rectangle = new RectangleF(0, 0, 70, 20);
			//set the connector
			leftConnector = new Connector(this, "Connector", true);
			leftConnector.ConnectorLocation = ConnectorLocation.West;
			Connectors.Add(leftConnector);			
			//cannot be resized
			IsResizable=true;
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="site"></param>
		public SimpleNode(IGraphSite site) : base(site)		
		{
			//set the default size
			Rectangle = new RectangleF(0, 0, 70, 20);
			//set the connector
			leftConnector = new Connector(this, "Connector", true);
			leftConnector.ConnectorLocation = ConnectorLocation.West;
			Connectors.Add(leftConnector);
			//cannot be resized
			IsResizable=true;
		}
		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected SimpleNode(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			//set the connector
			//leftConnector = new Connector(this, "Connector", true);
			//leftConnector.ConnectorLocation = ConnectorLocation.West;

			leftConnector = (Connector) info.GetValue("leftConnector", typeof(Connector));
			leftConnector.BelongsTo = this;
			Connectors.Add(leftConnector);			
			IsResizable=true;
			//Add here the necessary custom deserialization if you add new private members to the shape
		}
		#endregion
		
		#region Methods
		/// <summary>
		/// Overrides the default thumbnail used in the shape viewer
		/// </summary>
		/// <returns></returns>
		public override Bitmap GetThumbnail()
		{
			Bitmap bmp=null;
			try
			{
				Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.GraphLib.BasicShapes.Resources.SimpleNode.gif");
					
				bmp= Bitmap.FromStream(stream) as Bitmap;
				stream.Close();
				stream=null;
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message,"SimpleNode.GetThumbnail");
			}
			return bmp;
		}

		/// <summary>
		/// Paints the shape of the person object in the plex. Here you can let your imagination go.
		/// </summary>
		/// <param name="g">The graphics canvas onto which to paint</param>
		public override void Paint(Graphics g)
		{			
			base.Paint(g);
			if(RecalculateSize)
			{
				Rectangle = new RectangleF(new PointF(Rectangle.X,Rectangle.Y),
											g.MeasureString(this.Text,Font));	
				Rectangle = System.Drawing.RectangleF.Inflate(Rectangle, 10,10);
				RecalculateSize = false; //very important!
			}
			GraphicsPath path = new GraphicsPath();			
			path.AddArc(Rectangle.X, Rectangle.Y, 20, 20, -180, 90);			
			path.AddLine(Rectangle.X + 10, Rectangle.Y, Rectangle.X + Rectangle.Width - 10, Rectangle.Y);			
			path.AddArc(Rectangle.X + Rectangle.Width - 20, Rectangle.Y, 20, 20, -90, 90);			
			path.AddLine(Rectangle.X + Rectangle.Width, Rectangle.Y + 10, Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height - 10);			
			path.AddArc(Rectangle.X + Rectangle.Width - 20, Rectangle.Y + Rectangle.Height - 20, 20, 20, 0, 90);			
			path.AddLine(Rectangle.X + Rectangle.Width - 10, Rectangle.Y + Rectangle.Height, Rectangle.X + 10, Rectangle.Y + Rectangle.Height);			
			path.AddArc(Rectangle.X, Rectangle.Y + Rectangle.Height - 20, 20, 20, 90, 90);			
			path.AddLine(Rectangle.X, Rectangle.Y + Rectangle.Height - 10, Rectangle.X, Rectangle.Y + 10);			
			//shadow
			Region darkRegion = new Region(path);
			darkRegion.Translate(5, 5);
			g.FillRegion(new SolidBrush(Color.FromArgb(20, Color.Black)), darkRegion);
			
			//background
			g.FillPath(new SolidBrush(ShapeColor), path);

//			g.FillRectangle(BackgroundBrush, Rectangle.X, Rectangle.Y, Rectangle.Width + 1, Rectangle.Height + 1);
//			g.DrawRectangle(Pen, Rectangle.X, Rectangle.Y, Rectangle.Width + 1, Rectangle.Height + 1);
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
			
			if (c == leftConnector) return new PointF(Rectangle.Left, Rectangle.Top +(Rectangle.Height*1/2));			
			return new PointF(0, 0);
		}

	
		#endregion

		#region ISerializable Members
		/// <summary>
		/// ISerializable implementation
		/// </summary>
		/// <param name="info">the serialization info</param>
		/// <param name="context">the streaming context</param>
		[SecurityPermissionAttribute(SecurityAction.Demand,SerializationFormatter=true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);

			info.AddValue("leftConnector", this.leftConnector, typeof(Connector));
			//Add here the necessary custom serialization if you add new private members to the shape
		}

		#endregion
	}

	
	


}