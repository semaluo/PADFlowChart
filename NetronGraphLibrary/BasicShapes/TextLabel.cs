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
	/// A text label shape
	/// </summary>
	[Serializable]
	[Description("Text label")] 
	[NetronGraphShape("Text label","4F878611-3196-4d12-BA36-705F502C8A6B","Basic shapes","Netron.GraphLib.BasicShapes.TextLabel",
		 "Simple label shape with no connector.")]
	public class TextLabel : Shape
	{
			
		#region Fields
		/// <summary>
		/// the alignment of the text
		/// </summary>
		protected StringAlignment stringAlignment;
		private bool mShowPage = true;
		private RectangleF textRectangle;
		#endregion

		#region Properties

	

		/// <summary>
		/// Gets or sets whether the 'page' is drawn. If set to false the shape will appear as a text-only container.
		/// </summary>
		public bool ShowPage
		{
			get{return mShowPage;}
			set{mShowPage = value;}
		}

		#endregion

		#region Constructors
		/// <summary>
		/// This is the default constructor of the class.
		/// </summary>
		public TextLabel() : base()
		{
			Rectangle = new RectangleF(0, 0, 70, 80);
			ShapeColor = Color.LightYellow;
			stringAlignment = StringAlignment.Near;
			textRectangle = Rectangle;
			textRectangle.Inflate(-2,-2);
			
				
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="site"></param>
		public TextLabel(IGraphSite site) : base(site)
		{
			Rectangle = new RectangleF(0, 0, 70, 20);
			ShapeColor = Color.LightYellow;
			textRectangle = Rectangle;
			textRectangle.Inflate(-2,-2);
				
		}

		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected TextLabel(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.stringAlignment = (StringAlignment) info.GetValue("stringAlignment", typeof(StringAlignment));
			try
			{
				this.mShowPage = info.GetBoolean("mShowPage");
			}
			catch
			{
				this.mShowPage = true;
			}
		}
		#endregion

		#region Methods
		

		/// <summary>
		/// Returns a thumbanil representation of this shape
		/// </summary>
		/// <returns></returns>
		public override Bitmap GetThumbnail()
		{
			Bitmap bmp=null;
			try
			{
				Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.GraphLib.BasicShapes.Resources.TextLabel.gif");
					
				bmp= Bitmap.FromStream(stream) as Bitmap;
				stream.Close();
				stream=null;
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message, "TextLabel.GetThumbnail");
			}
			return bmp;
		}

		/// <summary>
		/// Paints the shape of the person object in the plex. Here you can let your imagination go.
		/// </summary>
		/// <param name="g">The graphics canvas onto which to paint</param>
		public override void Paint(Graphics g)
		{

//			if(RecalculateSize)
//			{
//				textRectangle = Rectangle;
//				textRectangle.Inflate(-2,-2);
////				SizeF s = g.MeasureString(this.Text,Font);
////				Rectangle = new RectangleF(Rectangle.X,Rectangle.Y,s.Width,Math.Max(s.Height+10,Rectangle.Height));	
//				RecalculateSize = false; //very important!				
//			}			
			
			if(ShowPage)
			{
				PointF[] pts = new PointF[6];
				pts[0] = new PointF(Rectangle.Left,Rectangle.Top);
				pts[1] = new PointF(Rectangle.Right - 10,Rectangle.Top);
				pts[2] = new PointF(Rectangle.Right,Rectangle.Top + 10);
				pts[3] = new PointF(Rectangle.Right,Rectangle.Bottom);
				pts[4] = new PointF(Rectangle.Left,Rectangle.Bottom);
				pts[5] = new PointF(Rectangle.Left,Rectangle.Top);
				g.FillPolygon(new SolidBrush(this.ShapeColor),pts);
				g.DrawPolygon(Pen,pts);

				g.DrawLine(Pen,pts[1],new PointF(Rectangle.Right-10,Rectangle.Top+10));
				g.DrawLine(Pen,new PointF(Rectangle.Right-10,Rectangle.Top+10),pts[2]);
			}
			if (ShowLabel)
			{
				
				StringFormat sf = new StringFormat();
				textRectangle = Rectangle;
				textRectangle.Inflate(-2,-2);
				
				sf.Alignment = stringAlignment;
				switch(stringAlignment)
				{
					case StringAlignment.Center:
						g.DrawString(Text, Font, TextBrush, textRectangle, sf); break;
					case StringAlignment.Far:
						g.DrawString(Text, Font, TextBrush, textRectangle, sf); break;
					case StringAlignment.Near:
						g.DrawString(Text, Font, TextBrush, textRectangle, sf); break;
				}
			}			
			base.Paint(g);
		}




		/// <summary>
		/// Changes the default context-menu
		/// </summary>
		/// <returns></returns>
		public override MenuItem[] ShapeMenu()
		{
			MenuItem[] subitems = new MenuItem[]{new MenuItem("First one",new EventHandler(TheHandler)),new MenuItem("Second one",new EventHandler(TheHandler))};

			MenuItem[] items = new MenuItem[]{new MenuItem("Special menu",subitems)};

			return items;
		}

		private void TheHandler(object sender, EventArgs e)
		{
			MessageBox.Show("Just an example.");
		}

		/// <summary>
		/// ISerializable serialization
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData (info, context);

			info.AddValue("stringAlignment", this.stringAlignment, typeof(StringAlignment));

			info.AddValue("mShowPage", this.mShowPage);
		}


		#region Propertygrid
		/// <summary>
		/// Adds additional properties to the shape
		/// </summary>
		public override void AddProperties()
		{
			base.AddProperties ();
			//replace the default text editing with something more extended for a label
			Bag.Properties.Remove("Text");
			Bag.Properties.Add(new PropertySpec("Text",typeof(string),"Appearance","The text attached to the entity","[Not set]",typeof(TextUIEditor),typeof(TypeConverter)));
			Bag.Properties.Add(new PropertySpec("ShowPage",typeof(bool),"Appearance","Whether the page should be shown",true));
			Bag.Properties.Add(new PropertySpec("Alignment",typeof(StringAlignment),"Graph","Gets or sets the string alignment.",StringAlignment.Near));
			Bag.Properties.Add(new PropertySpec("Font",typeof(Font),"Appearance","Gets or sets the font."));
			Bag.Properties.Add(new PropertySpec("TextColor",typeof(Color),"Appearance","Gets or sets the text color."));
			
		}
		
		/// <summary>
		/// Allows the propertygrid to access/set the properties of this shape
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.SetPropertyBagValue (sender, e);
			switch(e.Property.Name)
			{
				case "Alignment":
					this.stringAlignment = (StringAlignment) e.Value; this.Invalidate(); break;
				case "ShowPage":
					this.ShowPage = (bool) e.Value; this.Invalidate(); break;
				case "Font":
					this.Font = (Font) e.Value; this.Invalidate(); break;
				case "TextColor":
					this.TextColor = (Color) e.Value; this.Invalidate(); break;
			}
		}

		/// <summary>
		/// Allows the propertygrid to access the properties of this shape
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.GetPropertyBagValue (sender, e);
			switch(e.Property.Name)
			{
				case "Alignment":
					e.Value = this.stringAlignment; break;
				case "ShowPage":
					e.Value = ShowPage; break;
				case "Font":
					e.Value = this.Font; break;
				case "TextColor":
					e.Value = this.TextColor; break;

			}
		}
		
		#endregion
		#endregion

		
		
	}

}
