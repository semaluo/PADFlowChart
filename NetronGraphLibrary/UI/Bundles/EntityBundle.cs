using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using Netron.GraphLib.Interfaces;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Netron.GraphLib.UI;
namespace Netron.GraphLib
{
	/// <summary>
	/// Collects shapes and connections into a logical unit,
	/// this is a subset of the GraphAbstract. The EntityBundle can be saved into the favorites
	/// or duplicated or serialized to file for later use.
	/// </summary>
	[Serializable] public class EntityBundle : IEntityBundle, ISerializable
	{
		#region Fields
		private ConnectionCollection mConnections;
		private ShapeCollection mShapes;
		private GraphControl mSite;
		private string mName;
		private string mDescription;
		private Image mBundleImage;
		#endregion

		#region Properties
		/// <summary>
		/// Gets the embracing rectangle of the bundle
		/// </summary>
		public RectangleF Rectangle
		{
			get{
				if(Shapes.Count==0) return RectangleF.Empty;
				else
				{
					RectangleF rec = Shapes[0].Rectangle;
					for(int k=0; k<this.Shapes.Count; k++)
					{
						rec = RectangleF.Union(rec, Shapes[k].Rectangle);
					}

				    rec.Inflate(1, 1);
					return rec;
				}
			}
		}
		/// <summary>
		/// Gets or sets the name of the bundle
		/// </summary>
		public string Name
		{
			get{return mName;}
			set{mName = value;}
		}
		/// <summary>
		/// Gets or sets the description of the bundle
		/// </summary>
		public string Description
		{
			get{return mDescription;}
			set{mDescription = value;}
		}

		/// <summary>
		/// Gets or sets the site of the bundle
		/// </summary>
		public GraphControl Site
		{
			get{return this.mSite;}
			set{mSite = value;}
		}

		/// <summary>
		/// Gets or sets the screenshot of the bundle
		/// </summary>
		public Image BundleImage
		{
			get{return mBundleImage;}
			set{mBundleImage = value;}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Default constructor
		/// </summary>
		public EntityBundle(GraphControl mSite)
		{
			mConnections = new ConnectionCollection();
			mShapes = new ShapeCollection();
			this.mSite = mSite;
		}		

		#endregion

		#region Methods
		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected  EntityBundle(SerializationInfo info, StreamingContext context) 
		{
			this.mShapes = info.GetValue("mShapes", typeof(ShapeCollection)) as ShapeCollection;
			this.mConnections = info.GetValue("mConnections", typeof(ConnectionCollection)) as ConnectionCollection;
			this.mDescription = info.GetString("mDescription");
			this.mName = info.GetString("mName");
			try
			{
				this.mBundleImage = info.GetValue("mBundleImage",typeof(Image)) as Image;
			}
			catch
			{
				//leave the image to null
			}
		}

		/// <summary>
		/// Gets the connections contained in this bundle
		/// </summary>
		public ConnectionCollection Connections
		{
			get
			{
				return mConnections;
			}			
		}

		/// <summary>
		/// Gets the shapes contained in this bundle
		/// </summary>
		public ShapeCollection Shapes
		{
			get
			{
				return mShapes;
			}
		}

		/// <summary>
		/// Detaches the bundle from the site
		/// </summary>
		public void Detach()
		{
//			for(int k=0;k<Connections.Count; k++)
//			{
//				Site.Abstract.Connections.Remove(Connections[k]);
//			}
			for(int k=0;k<Shapes.Count; k++)
			{
				Shapes[k].Delete();
			}
		}

		/// <summary>
		/// Returns a copy of the bundle
		/// (generates new UID and so on).
		/// Note however that the connections are not connected to the connectors but contain a reference to the UID.
		/// The regeneration of UID's has to be performed after the unwrapping on the Site.
		/// </summary>
		/// <returns></returns>
		public EntityBundle Copy()
		{
			EntityBundle newobj = null;
			try
			{
				BinaryFormatter bFormatter = new BinaryFormatter(); 
				MemoryStream stream = new MemoryStream(); 
				bFormatter.Serialize(stream, this); 
				stream.Seek(0, SeekOrigin.Begin); 
				newobj = bFormatter.Deserialize(stream) as EntityBundle; 
				stream.Close();
		
			}
			catch(Exception exc)
			{
				mSite.OutputInfo(exc.Message,OutputInfoLevels.Exception);
			}
			return newobj; //not attached to the mSite and needs to be unwrapped!
		}


		/// <summary>
		/// Deselects all entities from this bundle
		/// </summary>
		internal void DeselectAll()
		{
			for(int k=0; k<Connections.Count;k++)
				Connections[k].IsSelected = false;
			for(int k=0; k<Shapes.Count; k++)
				Shapes[k].IsSelected = false;
		}
		/// <summary>
		/// Selects all entities from this bundle
		/// </summary>
		internal void SelectAll()
		{
			for(int k=0; k<Connections.Count;k++)
				Connections[k].IsSelected = true;
			for(int k=0; k<Shapes.Count; k++)
				Shapes[k].IsSelected = true;
		}

		/// <summary>
		/// Takes a screenshot of the bundle and stores it in the BundleImage <see cref="BundleImage"/> property
		/// </summary>
		/// <param name="g">The Graphics object to draw the entities on</param>
		public void TakeScreenshot(Graphics g)
		{
			DeselectAll();
			Bitmap bmp = Site.GetDiagramImage() as Bitmap;
			Bitmap TheClippedBmp = new Bitmap((int) Rectangle.Width,(int) Rectangle.Height)  ;   
			Graphics Gra = Graphics.FromImage(TheClippedBmp)  ;
			Gra.DrawImage(bmp, new Rectangle(0, 0,(int) Rectangle.Width,(int) Rectangle.Height), Rectangle, GraphicsUnit.Pixel) ; 
			this.mBundleImage = TheClippedBmp;
			
			/* Old implementation
			if(Rectangle==RectangleF.Empty) return;
			Bitmap bmp=new Bitmap((int) Math.Ceiling(this.Rectangle.Width),(int) Math.Ceiling(this.Rectangle.Height),g);
			
			using(Graphics gbis = Graphics.FromImage(bmp))
			{
				//gbis.Transform.Translate(Rectangle.X,Rectangle.Y, MatrixOrder.Append);
				//gbis.RenderingOrigin = new Point	((int) Rectangle.X, (int) Rectangle.Y);				
				gbis.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
				//suppose to have a transparent backgound
				//g.FillRectangle(Brushes.White, 0, 0, this.AutoScrollMinSize.Width+20,this.AutoScrollMinSize.Height+20);
				
				for(int k =0; k<Connections.Count;k++)
				{					
					Connections[k].Paint(gbis);
				}
				for(int k=0; k<Shapes.Count;k++)
				{					
					Shapes[k].Paint(gbis);
				}
			}			
			this.mBundleImage = bmp;
			*/
			return;
		}
		/// <summary>
		/// Takes a screenshot of the bundle and stores it in the BundleImage <see cref="BundleImage"/> property.
		/// The background is filled with the given color.
		/// </summary>
		/// <param name="g">The Graphics object to draw the entities on</param>
		/// <param name="backgroundColor">the color to fill the background with</param>
		public Bitmap TakeScreenshotWithBackground(Graphics g, Color backgroundColor)
		{
			DeselectAll();
			Bitmap bmp = Site.GetDiagramImage() as Bitmap;
			Bitmap TheClippedBmp = new Bitmap((int) Rectangle.Width,(int) Rectangle.Height)  ;   
			Graphics Gra = Graphics.FromImage(TheClippedBmp);
			Gra.FillRectangle(new SolidBrush(backgroundColor), 0,0,(int) Rectangle.Width,(int) Rectangle.Height);
			Gra.DrawImage(bmp, new Rectangle(0, 0,(int) Rectangle.Width,(int) Rectangle.Height), Rectangle, GraphicsUnit.Pixel) ; 
			return TheClippedBmp;
		}
		/// <summary>
		/// Offsets the bundle
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void Offset(int x, int y)
		{			
			for(int k=0; k<Shapes.Count; k++)
			{
				
				//if(Shapes[k].Tracker!=null) Shapes[k].Tracker.Move(new PointF(Shapes[k].Tracker.Rectangle.X+x, Shapes[k].Tracker.Rectangle.Y+y), Site.Size, Site.Snap, Site.GridSize);
				Shapes[k].Rectangle = new RectangleF(Shapes[k].Rectangle.X+x,Shapes[k].Rectangle.Y+ y,Shapes[k].Rectangle.Width,Shapes[k].Rectangle.Height);
				
			}
		}
		/// <summary>
		/// ISerializable implementation
		/// </summary>
		/// <param name="info">the serialization info</param>
		/// <param name="context">the streaming context</param>		
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("mShapes",this.mShapes);			
			info.AddValue("mConnections",this.mConnections);			
			info.AddValue("mName", this.mName);
			info.AddValue("mDescription", this.mDescription);
			info.AddValue("mBundleImage", this.mBundleImage, typeof(Image));
		}

		#endregion
	}
}
