using System;
using System.Security;
using System.Security.Permissions;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Netron.GraphLib.UI;
using Netron.GraphLib.Interfaces;
using Netron.GraphLib.Attributes;
using Netron.GraphLib.Utils;

namespace Netron.GraphLib
{
	/// <summary>
	/// Template class definition to be inherited by all shapes you want to insert and use in your plex
	/// </summary>
	[Serializable]public abstract class Shape : Entity, IAutomataCell, IShape, ISerializable
	{
		
		#region Fields

		
		/// <summary>
		/// the URL icon
		/// </summary>
		private Image urlImage;
		/// <summary>
		/// hyperlink to web or file
		/// </summary>
		private string mURL = string.Empty;
		/// <summary>
		/// whether the shape is square
		/// </summary>
		private bool mSquare;
		/// <summary>
		/// the z-order of the shapes
		/// </summary>
		private int mZOrder;
		/// <summary>
		/// whether you can move the shape
		/// </summary>
		private bool mCanMove = true;		
		/// <summary>
		/// the array list of .Net mControls the shape contains
		/// </summary>
		private NetronGraphControlCollection mControls;
		/// <summary>
		/// the infinitesimal x-shift used by layout
		/// </summary>
		private double mDeltaX;
		/// <summary>
		/// the infinitesimal y-shift used by layout
		/// </summary>
		private double mDeltaY;
		/// <summary>
		/// fixed node boolean
		/// </summary>
		private bool mIsFixed;
		/// <summary>
		/// the default node color
		/// </summary>
		private Color mShapeColor = Color.WhiteSmoke;
		/// <summary>
		/// The Abstarct object to which this shape belongs.
		/// </summary>
		/// <remarks>
		/// A shape cannot exist on its own and will not be drawn outside an abstract structure
		/// </remarks>
		//private GraphAbstract mAbstract;
		/// <summary>
		/// tells wether or not the user can resize the mRectangle
		/// </summary>
		private bool mIsResizable = true;
		/// <summary>
		/// The internal collection of mConnectors attached to this shape object
		/// </summary>
		/// <remarks>
		/// Note that mConnectors are sub-ordinated to the shapes and thus do not have to be deleted or taken care off independently
		/// </remarks>
		private ConnectorCollection mConnectors = new ConnectorCollection();
		/// <summary>
		/// This is the floating-point mRectangle associated to the shape
		/// It determines the shape's size or boundaries
		/// </summary>
		private RectangleF mRectangle = new RectangleF(0,0,100,100);
		/// <summary>
		/// The internal tracker object, representing the mRectangle and grips with which one can resize the shape.
		/// </summary>
		private Tracker mShapeTracker;
		/// <summary>
		/// whether the shape is visible
		/// </summary>
		private bool mIsVisible = true;
		/// <summary>
		/// whether the shape's adjacent nodes are expanded
		/// </summary>
		private bool mIsExpanded = true;
		#endregion

		#region Constructor related
		/// <summary>
		/// Default constructor
		/// </summary>
		protected Shape() : base()
		{
			mControls = new NetronGraphControlCollection();

			LoadURLImage();
			

		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="site"></param>
		protected Shape(IGraphSite site) : base(site)
		{
			mControls = new NetronGraphControlCollection();
		
		}

		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected Shape(SerializationInfo info, StreamingContext context)  : base(info, context)
		{

			mControls = new NetronGraphControlCollection();

			//this.SetLayer(Site.Abstract.DefaultLayer);

			this.mCanMove = info.GetBoolean("mCanMove");

			this.mIsExpanded = info.GetBoolean("mIsExpanded");

			this.mIsFixed = info.GetBoolean("mIsFixed");

			this.mIsResizable = info.GetBoolean("mIsResizable");

			this.mIsVisible = info.GetBoolean("mIsVisible");

			this.mRectangle = (RectangleF) info.GetValue("mRectangle",typeof(RectangleF));

			this.mShapeColor = (Color) info.GetValue("mShapeColor",typeof(Color));
			
			this.mZOrder = info.GetInt32("mZOrder");

			try
			{
				this.mURL = info.GetString("mURL");
			}
			catch
			{
				this.mURL = "";
			}

			//this.mConnectors = (ConnectorCollection) info.GetValue("mConnectors",typeof(ConnectorCollection));
//			Tag = info.GetString("mLayer");
			
			LoadURLImage();
			
		}
		/// <summary>
		/// Additional actions after deserialization
		/// </summary>
		//public override void PostDeserialization()
		//{
		//	base.PostDeserialization ();
  //          if (Tag is string)
  //          {
  //              SetLayer((string)Tag);
  //              Tag = null; //be nice to the host/user
  //          }
  //      }


		#endregion

		#region Properties


		/// <summary>
		/// Gets or sets the hyperlink of this shape
		/// </summary>
		public string URL
		{
			get{return mURL;}
			set{mURL = value;}
		}

		/// <summary>
		/// Gets or sets whether width and height are equal (square shape)
		/// </summary>
		public bool IsSquare
		{
			get{return mSquare;}
			set{mSquare = value;}
		}

		/// <summary>
		/// Gets the abstract of the graph
		/// </summary>
		public GraphAbstract Abstract
		{
			get{return this.Site.Abstract;}
		}

		/// <summary>
		/// Gets or sets the font size
		/// </summary>
		/// /// <remarks>Redefines the FontSize property of the Entity class as public <see cref="Netron.GraphLib.Entity"/></remarks>
		public new float FontSize
		{
			get
			{					
				return base.FontSize;
			}
			set
			{
				base.FontSize=value;
			}
		}
		/// <summary>
		/// Gets or sets the font to be used for drawing text
		/// 
		/// </summary>
		/// <remarks>Redefines the Font property of the Entity class as public <see cref="Netron.GraphLib.Entity"/></remarks>
		public new Font Font
		{
			get{return base.Font;}
			set{base.Font = value;}
		}

		/// <summary>
		/// Gets or sets the z-order of the shape
		/// </summary>
		[GraphMLData]public virtual int ZOrder
		{
			get{return mZOrder;}
			set{mZOrder = value;
				Site.Abstract.SortPaintables(); 
				this.Invalidate();
			}
		}
		/// <summary>
		/// Gets or sets whether the shape can be resized
		/// </summary>
		public bool IsResizable
		{
			get{return mIsResizable;}
			set{mIsResizable = value;}
		}

		/// <summary>
		/// Gets or sets whether the shape can be moved around.
		/// Note that the Fixed property is used to set whether the shape
		/// participates in the layout-process.
		/// </summary>
		[GraphMLData]public bool CanMove
		{
			get{return mCanMove;}
			set{mCanMove = value;}
		}
		
	
		/// <summary>
		/// Zooms the shape
		/// </summary>
		/// <param name="amount"></param>
		internal void Zoom(float amount)
		{
			mShapeTracker=null;
			this.IsSelected=false;
			
			mRectangle.Width*=amount;mRectangle.Height*=amount;
			mRectangle.X*=amount;mRectangle.Y*=amount;
				
		}
		/// <summary>
		/// Override the Entity.SetLayer to adapt the shape's appearance in function of the 
		/// layer's parameters.
		/// </summary>
		/// <param name="layer">a pre-defined or added graph-layer</param>
		protected override void SetLayer(GraphLayer layer)
		{
			if(layer==null) return;
			base.SetLayer (layer);
			if(layer == Site.Abstract.DefaultLayer)
			{
				mShapeColor = Color.FromArgb(255,ShapeColor);
			    if (Pen == null)
			    {
                    Pen = new Pen(Brushes.Black, PenWidth);
			    }
				TextColor =  Color.FromArgb(255,TextColor);
			}
			else
			{
				int alpha = (int) (Layer.Opacity*255f/100);
				if(Layer.UseColor)				
				{
					mShapeColor = Color.FromArgb(alpha ,Layer.LayerColor);								
				}
				else
				{
					mShapeColor = Color.FromArgb((int) (Layer.Opacity*255f/100),ShapeColor);
				}

			    if (Pen == null)
			    {
				    Pen=new Pen(Color.FromArgb(alpha,Color.Black), PenWidth);
			    }
			    else
			    {
                    Pen = new Pen(Color.FromArgb(alpha, Pen.Color), PenWidth);
                }
                TextColor =  Color.FromArgb(alpha,TextColor);
			}
		}

		/// <summary>
		/// Returns the associated mRectangle for this shape
		/// </summary>
		/// <remarks>
		/// The need for this becomes more clear if start to use non-rectangular shapes.
		/// </remarks>
		public virtual RectangleF Rectangle
		{
			set 
			{
				//Invalidate();
				mRectangle = value;
				if (mShapeTracker != null) mShapeTracker.Rectangle = mRectangle;
				//Invalidate();
			}
			get
			{
				return (mShapeTracker != null) && (mShapeTracker.Track) ? mShapeTracker.Rectangle : mRectangle;
			}
		}
		/// <summary>
		/// Returns the collection of mConnectors for this shape object
		/// </summary>
		public ConnectorCollection Connectors
		{
			get { return mConnectors; }
		}
		/// <summary>
		/// Is the shape selected?
		/// </summary>
		public  override bool IsSelected
		{
			set
			{
				//the base keeps the value
				base.IsSelected = value;

				if (value)
				{
					mShapeTracker = new ShapeTracker(this.Site,Rectangle, IsResizable, IsSquare, this);
					Invalidate();
				}
				else
				{
					Invalidate();
					mShapeTracker = null;
				}
			}
			get
			{
				return base.IsSelected;
			}
		}

		/// <summary>
		/// The list of Controls the shape contains
		/// </summary>
		public NetronGraphControlCollection Controls
		{
			get{return mControls;}
		}

		/// <summary>
		/// Returns the tracker, which represents the grips and mRectangle with which one can resize the shape.
		/// </summary>
		public override Tracker Tracker
		{
			get {return mShapeTracker;}
			set{ mShapeTracker = value;}
		}

		/// <summary>
		/// Gets or sets the x-coordinate of the shape
		/// </summary>
		[GraphMLData]public float X
		{
			get
			{
					
				return this.mRectangle.X;
			}
			set
			{
				this.mRectangle.X=value;
			}
		}

		/// <summary>
		/// Gets or sets the y-coordinate
		/// </summary>
		[GraphMLData]public float Y
		{
			get
			{
					
				return this.mRectangle.Y;
			}
			set
			{
				this.mRectangle.Y=value;
			}
		}

		/// <summary>
		/// Gets or sets the location of the shape
		/// </summary>
		public PointF Location
		{
			get{return new PointF(mRectangle.X,mRectangle.Y);}
			set{mRectangle.X = value.X; mRectangle.Y = value.Y;}
		}

		/// <summary>
		/// Increment in x for the graph layout
		/// </summary>
		public double dx
		{
			get
			{
					
				return mDeltaX;
			}
			set
			{
				mDeltaX=value;
			}
		}

		/// <summary>
		/// Increment in y for the graph layout
		/// </summary>
		public double dy
		{
			get
			{
				return mDeltaY;
			}
			set
			{
				mDeltaY=value;
			}
		}

		/// <summary>
		/// Gets or sets whether the shape is fixed/unmovable.
		/// Note that the CanMove property is used to enable/disable the shape moves
		/// via the mouse while this property enables/disable the layout-process for this shape.
		/// </summary>
		[GraphMLData]public bool IsFixed
		{
			get
			{
				return mIsFixed;
			}
			set
			{
				mIsFixed=value;
			}
		}

		/// <summary>
		/// Gets or sets the node color
		/// </summary>
		[GraphMLData]public virtual Color ShapeColor
		{
			get
			{				
				return mShapeColor;
			}
			set
			{
                if (Layer != null && Layer.UseColor)//only set the color if the layer it's on doesn't enforce a color
                    return;

                mShapeColor = value;
                //				else
                //					MessageBox.Show("The shape is part of layer and enforces a color. \n\nIf you want to change the color, unset the color flag of the layer or assign the shape to the default layer","Cannot change the color",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
		}

		/// <summary>
		/// Gets the background brush
		/// </summary>
		[GraphMLData]protected internal virtual Brush BackgroundBrush
		{
			get{
				
				if(IsSelected)
					return new SolidBrush(ControlPaint.Light(mShapeColor)); 
				else
				{
					if(Layer != Site.Abstract.DefaultLayer)
						 return new SolidBrush(Color.FromArgb((int) Math.Round(Layer.Opacity*255f/100),mShapeColor));
					else
						return new SolidBrush(mShapeColor);
				}
			}	
		}

		/// <summary>
		/// Gets the text brush
		/// </summary>
		[GraphMLData]protected internal virtual Brush TextBrush
		{
			get{return new SolidBrush(TextColor);}
		}

			
		/// <summary>
		/// Width of a shape.
		/// </summary>
		[Browsable(false),GraphMLData]
		public virtual float Width
		{
			get { return this.Rectangle.Width; }
			set 
			{ 
				RectangleF r = this.Rectangle;
				this.Rectangle = new RectangleF(r.X,r.Y,value,r.Height);
			}
		}

		/// <summary>
		/// Height of a shape.
		/// </summary>
		[Browsable(false),GraphMLData]
		public virtual float Height
		{
			get { return this.Rectangle.Height; }
			set 
			{ 
				RectangleF r = this.Rectangle;
				this.Rectangle = new RectangleF(r.X,r.Y,r.Width,value);
			}
		}
		/// <summary>
		/// Gets the collection of nodes attached to this node
		/// </summary>
		public ShapeCollection AdjacentNodes
		{
			get
			{
				ShapeCollection shapes = new ShapeCollection();
				foreach(Connector c in this.mConnectors)
				{
					foreach(Connection con in c.Connections)
					{
						if(con.From.BelongsTo.Equals(this))
							shapes.Add(con.To.BelongsTo);
						else
							shapes.Add(con.From.BelongsTo);
					}
					
				}
				return shapes;
			}
		}

		/// <summary>
		/// Gets or sets whether the shape is visible on the canvas
		/// </summary>
		public bool IsVisible
		{
			get
			{
				return mIsVisible;
			}
			set
			{
				mIsVisible = value;
			}

		}

		/// <summary>
		/// Gets or sets whether the shape shows its sub-shapes
		/// This property, its validity, depends on the overal topology of the graph
		/// </summary>
		public bool IsExpanded
		{
			get
			{
				return mIsExpanded;
			}
			set
			{
				mIsExpanded = value;
			}

		}
		#endregion
		
		#region Methods

		/// <summary>
		/// Sets whether the connectors of this shape allow new connections to be attached to this shape
		/// </summary>
		/// <param name="value">if true, all connectors allow new connection to this shape</param>
		public void NewConnectionsTo(bool value)
		{
			foreach(Connector c in this .Connectors)
				c.AllowNewConnectionsTo = value;
		}
		/// <summary>
		/// Sets whether the connectors of this shape allow new connections to be launched from this shape
		/// </summary>
		/// <param name="value">if true, all connectors allow new connection to this shape</param>
		public void NewConnectionsFrom(bool value)
		{
			foreach(Connector c in this .Connectors)
				c.AllowNewConnectionsFrom = value;
		}

		private void LoadURLImage()
		{
			try
			{
				Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.GraphLib.Resources.URL.gif");
				urlImage= Bitmap.FromStream(stream) as Bitmap;
				stream.Close();
				stream=null;
			}
			catch
			{
				
			}
		}

		#region Automata related methods
		/// <summary>
		/// This method represents the transmission of data over a connection. Once the data is transmitted to the mConnectors the senders' value is reset
		/// </summary>
		/// <remarks>This method is not strictly part of the plex structure but belongs to the possible applications.</remarks>
		public virtual void Transmit()
		{				
			foreach (Connector c in Connectors)
			{
				foreach (Connection n in c.Connections)
					n.Transmit();
				c.Sends.Clear();
			}
		}
		/// <summary>
		/// Actions to perform before the update
		/// </summary>
		public virtual void BeforeUpdate(){}
		/// <summary>
		/// Actions to perform after the update
		/// </summary>
		public virtual void AfterUpdate(){}
		/// <summary>
		/// The method allows to update the dynamical state of the plex, to compute something on the basis of the received values and to set the new send values.
		/// 
		/// </summary>
		public virtual void Update()
		{
				
			//here you can calculate the state and set the new send values before resetting the receive.
			//for example:
			//				clear the sends values
			//				foreach (Connector c in Connectors) c.Sends.Clear();
			//				calculate things with the Receives and set internal state of the automata
			//			    set new values of Sends
			//			    finally, clean the receives before the nex transmit
			//				foreach (Connector c in Connectors) c.Receives.Clear();
		}
		#endregion

		/// <summary>
		/// Returns whether the shape is connected to a given shape
		/// </summary>
		/// <param name="shape"></param>
		/// <returns></returns>
		public bool IsConnectedTo(Shape shape)
		{
			foreach(Connector c in this.mConnectors)
			{
				foreach(Connection con in c.Connections)
				{
					if(con.To.BelongsTo.Equals(shape) || con.From.BelongsTo.Equals(shape)) return true;
					
				}
			}
			return false;
		}


		/// <summary>
		/// Required interface implementation
		/// </summary>
		public virtual void InitAutomata()
		{
			
		}
		/// <summary>
		/// Adds the shape to an GraphAbstract collection
		/// </summary>
		/// <param name="p"></param>
		internal   void Insert(GraphAbstract p)
		{			
			Site.Abstract.Shapes.Add(this);
			Invalidate();
            Site.Abstract.IsDirty = true;
		}
		/// <summary>
		/// Removes itself from an GraphAbstract. The mConnectors are deleted as part of this deletion process.
		/// </summary>
		 internal protected override void Delete()
		{
			Invalidate();
    	
			// throw the connections away
            ArrayList toDelete = new ArrayList();

            // throw the connections away. 
            // Action is done in two steps because calling Delete() of a connection manipulates the connector
            // collections. To Prevent an exception the connections are iterated and moved to a deletion list first.   
            foreach (Connector c in Connectors)
            {
	            foreach (Connection cn in c.Connections)
	            {
		            toDelete.Add(cn);
	            }
            }

            foreach( Connection cn in toDelete )
            {
	            cn.Delete();
            }

            if (Site.Abstract.Shapes.Contains(this))
            {
                Site.Abstract.Shapes.Remove(this);
                Site.Abstract.IsDirty = true;
            }

		    Invalidate();
		}
		/// <summary>
		/// Returns true if the given mRectangle contains the shape (this)
		/// </summary>
		/// <param name="r">A floating-point mRectangle object</param>
		/// <returns>True if contained</returns>
		public override bool Hit(RectangleF r)
		{
			//if mRectangle is point like
			if ((r.Width == 0) && (r.Height == 0))
			{   //hit the mRectangle of the shape
				if (Rectangle.Contains(r.Location)) return true;
				//hit the tracker
				if (mShapeTracker != null)
				{
					Point h = mShapeTracker.Hit(r.Location);
					if ((h.X >= -1) && (h.X <= +1) && (h.Y >= -1) && (h.Y <= +1)) return true;
				}
				//hit a connector
				foreach (Connector c in Connectors)
					if (c.Hit(r)) return true;

				return false;
			}
			//if not point-like then use the normal method
			return r.Contains(Rectangle);
		}
		/// <summary>
		/// Returns the coordinates of a given connector attached to this shape
		/// </summary>
		/// <param name="c">A connector object</param>
		/// <returns>A floating-point point (pointF)</returns>
		public virtual PointF ConnectionPoint(Connector c)
		{
			return PointF.Empty;
		}

		/// <summary>
		/// Returns the thumbnail of the shape (for the shape viewer)
		/// </summary>
		/// <returns></returns>
		public virtual Bitmap GetThumbnail()
		{
			Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.GraphLib.Resources.UnknownShape.gif");
			Bitmap bmp= Bitmap.FromStream(stream) as Bitmap;
			stream.Close();
			stream=null;
			return bmp;
				 
		}
		/// <summary>
		/// Overrides the paint method
		/// </summary>
		/// <remarks>
		/// Do not forget to call this via base.Paint to paint the tracker.
		/// </remarks>
		/// <param name="g">The graphics canvas onto which to paint</param>
		public override void Paint(Graphics g)
		{			
			//let the shapes implement the design and painting
		}
		/// <summary>
		/// Paints the adornments (URL link etc.)
		/// </summary>
		/// <param name="g"></param>
		public override void PaintAdornments(Graphics g)
		{
			if(URL!="") 
			{
				g.DrawImage(urlImage, Rectangle.Right-16,Rectangle.Bottom-16,16,16);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="square">if true the shape will be square with the maximum otherwise only the width will be resized to fit the content</param>
		public void FitSize(bool square)
		{
			SizeF newSize = this.Site.Graphics.MeasureString(this.Text,Font);
			newSize.Width=Math.Max(newSize.Height,newSize.Width) + 4;
			if(square) newSize.Height=newSize.Width;
			newSize.Height+=7;
			Rectangle = new RectangleF(new PointF(Rectangle.X,Rectangle.Y),newSize);	
		}

		/// <summary>
		/// Refreshes the shape
		/// </summary>
		public override void Invalidate()
		{
			if (Site == null) return;
			if (Connectors == null) return;

			// Invalidate the shape mRectangle, pay attention on scroll position and zoom
			RectangleF r = this.Rectangle;
			r.Inflate(+3, +3); // padding for selection frame.

			System.Drawing.Rectangle r2 = System.Drawing.Rectangle.Round(r);
			r2.Offset( Site.AutoScrollPosition.X, Site.AutoScrollPosition.Y );

			r2 = Site.ZoomRectangle(r2);
			r2.Inflate(2,2);

			Site.InvalidateRectangle(r2);

			// Invalidate each connector
			foreach (Connector c in Connectors)
			{
				c.Invalidate();

				if (Tracker != null)
					foreach (Connection n in c.Connections)
						n.Invalidate();
			}

			// Invalidate tracker
			if (Tracker != null)
			{
				RectangleF a = Tracker.Grip(new Point(-1, -1));
				RectangleF b = Tracker.Grip(new Point(+1, +1));
				r2 = System.Drawing.Rectangle.Round(RectangleF.Union(a, b));
				r2.Offset( Site.AutoScrollPosition.X, Site.AutoScrollPosition.Y );
				r2 = Site.ZoomRectangle(r2);
				r2.Inflate(2,2);

				Site.InvalidateRectangle(r2);
			}
		}
		/// <summary>
		/// Returns the cursor for this shape
		/// </summary>
		/// <param name="p">A floaint-point point</param>
		/// <returns>A cursor object</returns>
		public override Cursor GetCursor(PointF p)
		{
			//for the adornment
			if(URL!=string.Empty && new RectangleF(Rectangle.Right -16, Rectangle.Bottom -16, 16,16).Contains(p)) 
			{
				Site.SetToolTip(URL);
				return Cursors.Hand;
			}

			if (mShapeTracker != null)
			{
				
				//for the tracker handles
				Cursor c = mShapeTracker.Cursor(p);
				if (c != Cursors.No) return c;
			}

			if (Control.ModifierKeys == Keys.Shift)
				return MouseCursors.Add;

			
			Site.SetToolTip(this.Text);
			return Cursors.Arrow;//MouseCursors.Select;
		}

		internal override void RaiseMouseDown(MouseEventArgs e)
		{
			//for the adornment
			if(URL!=string.Empty && new RectangleF(Rectangle.Right -16, Rectangle.Bottom -16, 16,16).Contains(e.X,e.Y)) 
			{
				HandleURL();
				this.IsSelected = false;
				
				return;//suppose this has preference over the custom shape mouse event handler
			}
			//call if after to raise the event after our handling
			base.RaiseMouseDown (e);
		}

		/// <summary>
		/// Handles the different recognized URL types:
		/// http://  - opens the default browser
		/// netron:// - opens the diagram from the specified location
		/// showgraphproperties - the graph properties are displayed
		/// showgraphlayers - the graph layers are displayed
		/// </summary>
		private void HandleURL()
		{
			if(URL.ToLower().StartsWith("http://")) 
			{
				Site.OutputInfo("Opening the URL '" + URL +"' (shape URL)", OutputInfoLevels.Info);
				Process.Start(URL);				
			}
			else if(URL.ToLower().StartsWith("netron://"))
			{
				
				//some checks
				string path = Path.GetFullPath(URL.Replace("netron://",""));
				if(File.Exists(path))
				{
					Site.OutputInfo("Opening the diagram '" + URL.Replace("netron://","") +"' (shape URL)", OutputInfoLevels.Info);
					Site.Open(path);
				}
				else
					Site.OutputInfo("The file '" + path +"' does not exist.", OutputInfoLevels.Info);
			}
			else if(URL.ToLower().StartsWith("showgraphproperties"))
			{
				Site.OutputInfo("Showing the graph properties on URL request.", OutputInfoLevels.Info);
				Site.RaiseOnShowPropertiesDialogRequest();
			}
			else if(URL.ToLower().StartsWith("showgraphlayers"))
			{
				Site.OutputInfo("Showing the graph layers on URL request.", OutputInfoLevels.Info);
				Site.RaiseOnShowGraphLayers();
			}
			else if(URL.ToLower().StartsWith("process://"))
			{
				string path = Path.GetFullPath(URL.Replace("process://",""));
				if(File.Exists(path))
				{
					Site.OutputInfo("Using default  system application to launch the URL '" + URL +"' ", OutputInfoLevels.Info);
					Process.Start(URL);				
				}
			}


			}

		/// <summary>
		/// Moves the shape controls (if any) when the shape has been moved
		/// </summary>
		public virtual void MoveControls(){}

		#region Properties related
		/// <summary>
		/// Adds the basic properties of the shape
		/// </summary>
		public  override void AddProperties()
		{
			base.AddProperties ();
			//the size of the shape
			Bag.Properties.Add(new PropertySpec("Size",typeof(SizeF),"Layout","The size of the shape",SizeF.Empty,typeof(System.Drawing.Design.UITypeEditor),typeof(Netron.GraphLib.SizeFTypeConverter)));
			//the location of the shape
			Bag.Properties.Add(new PropertySpec("Location",typeof(PointF),"Layout","The location of the shape",PointF.Empty,typeof(System.Drawing.Design.UITypeEditor),typeof(Netron.GraphLib.PointFTypeConverter)));
			
			//graph specs		
			//Bag.Properties.Add(new PropertySpec("Connectors",typeof(ConnectorCollection),"Graph","The connector collection of the shape",SizeF.Empty));
			//the node's color
			Bag.Properties.Add(new PropertySpec("ShapeColor",typeof(Color),"Appearance","The background color of the shape",Color.Gray));
			//the layer			
			PropertySpec spec = new PropertySpec("Layer",typeof(string),"Appearance","Gets or sets the line shape.","Default",typeof(LayerUITypeEditor),typeof(TypeConverter));
			spec.Attributes = Site.GetLayerAttributes();
			Bag.Properties.Add(spec);

			//the z-order
			Bag.Properties.Add(new PropertySpec("Z-order",typeof(int),"Layout","The z-order of the shape",0));

			//the URL
			Bag.Properties.Add(new PropertySpec("URL",typeof(string),"Layout","An hyperlink to a site or a file",""));


		}
		/// <summary>
		/// Allows the propertygrid to set new values
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.GetPropertyBagValue (sender, e);
			switch(e.Property.Name)
			{
				case "Size": e.Value=this.mRectangle.Size;break;
				case "Location": e.Value=this.mRectangle.Location;break;
				//case "Connectors": e.Value=this.mConnectors;break;
				case "ShapeColor": e.Value=this.mShapeColor;break;
				case "Layer" : 
					if(Layer==null)
						e.Value = Site.Abstract.DefaultLayer;
					else
						e.Value = this.Layer; 
					break;
				case "Z-order":
					e.Value = this.ZOrder;
					break;
				case "URL":
					e.Value = this.URL;
					break;
			}
		}

		/// <summary>
		/// Allows the propertygrid to set new values
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			base.SetPropertyBagValue (sender, e);

			switch(e.Property.Name)
			{
				case "Size": 
					if(this.IsResizable) this.mRectangle.Size=(SizeF) e.Value;
					else
						MessageBox.Show("The shape is defined as non-resizable.","Not allowed", MessageBoxButtons.OK,MessageBoxIcon.Information);
					if(this.Tracker!=null) 
					{
						this.Tracker.Rectangle=this.mRectangle;
					}
					RecalculateSize = false;					
					this.Invalidate();
					break;
				case "Location": 
					this.mRectangle.Location=(PointF) e.Value;
					if(this.Tracker!=null)
					{
						(Tracker as ShapeTracker).ChangeLocation((PointF) e.Value);						
					}
					RecalculateSize = false;					
					this.Invalidate();
					break;
				case "Z-order":
					this.ZOrder = (int) e.Value;
					
					break;
				case "ShapeColor":
					ShapeColor=(Color) e.Value;					
					RecalculateSize = false;					
					this.Invalidate();
					break;
				case "Layer":
					this.SetLayer( e.Value as GraphLayer);									
					this.Invalidate();
					break;
				case "URL":
					this.URL = (string) e.Value;
					break;
			}
			if(e.Property.Name=="Text" && this.IsResizable) RecalculateSize = true;

		}

		#endregion

		/// <summary>
		/// Allows to extend the default canvas menu with additional items
		/// </summary>
		/// <returns></returns>
		public virtual MenuItem[] ShapeMenu()
		{
			return null;
		}
		/// <summary>
		/// Overridable OnKeyDown handler
		/// </summary>
		/// <param name="e"></param>
		public virtual void OnKeyDown(KeyEventArgs e)
		{return;}
		/// <summary>
		/// Overridable OnKeyPress handler
		/// </summary>
		/// <param name="e"></param>
		public virtual void OnKeyPress(KeyPressEventArgs e)
		{return;}



		#endregion

		#region ISerializable Members

		/// <summary>
		/// ISerializable implementation
		/// </summary>
		/// <param name="info">the serialization info</param>
		/// <param name="context">the streaming context</param>
		[SecurityPermissionAttribute(SecurityAction.Demand,SerializationFormatter=true)]
		public override  void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);

			info.AddValue("mCanMove",this.mCanMove);

			info.AddValue("mIsExpanded",this.mIsExpanded);

			info.AddValue("mIsFixed",this.mIsFixed);

			info.AddValue("mIsResizable",this.mIsResizable);

			info.AddValue("mIsVisible",this.mIsVisible);

			info.AddValue("mRectangle",this.mRectangle);

			info.AddValue("mShapeColor",this.mShapeColor);

			info.AddValue("mZOrder",this.mZOrder);

			info.AddValue("mURL", this.mURL);


			//info.AddValue("mConnectors", this.mConnectors);





		}

		#endregion
	}
	

	
}
