using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Security;
using System.Security.Permissions;

using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Netron.GraphLib.UI;
using Netron.GraphLib.Interfaces;
using Netron.GraphLib.Attributes;
using Netron.GraphLib.Configuration;
namespace Netron.GraphLib
{
	/// <summary>
	/// Abstract base class for everything that participates in the diagram/graph (connection, connector...)	
	/// </summary>
	[Serializable] public abstract class Entity : IEntity, IDisposable, ISerializable
	{	

		#region Events

		/// <summary>
		/// Occurs when the mouse is pressed on this entity
		/// </summary>
		public event MouseEventHandler OnMouseDown;

		/// <summary>
		/// Occurs when the mouse is released while above this entity
		/// </summary>
		public event MouseEventHandler OnMouseUp;

		/// <summary>
		/// Occurs when the mouse is moved while above this entity
		/// </summary>
		public event MouseEventHandler OnMouseMove;
		#endregion

		#region Fields

		/// <summary>
		/// the layer to which the entity belongs,
		/// the default layer is a static unique layer defined in the GraphAbstract.		
		/// </summary>		
		private GraphLayer mLayer;
		/// <summary>
		/// volatile all-purpose tag
		/// </summary>
		[NonSerialized] private object mTag;
		/// <summary>
		/// whether to recalculate the shape size, speed up the rendering
		/// </summary>
		private bool mRecalculateSize ;
		/// <summary>
		/// the property bag
		/// </summary>
		[NonSerialized] private PropertyBag mBag;
		/// <summary>
		/// default blue mPen, speeds up rendering
		/// Note that the Pen is not serialzable!
		/// </summary>
		[NonSerialized] private Pen mBluePen;
		/// <summary>
		/// default black mPen, speeds up rendering
		/// </summary>
		[NonSerialized] private Pen mBlackPen;
		/// <summary>
		/// the mPen's width
		/// </summary>
		private float mPenWidth = 1F;
		/// <summary>
		/// default mPen
		/// </summary>
		[NonSerialized] private Pen mPen;
		/// <summary>
		/// whether the entity is reset
		/// </summary>
		private bool mIsGuiReset;
		/// <summary>
		/// the font family
		/// </summary>
		private string mFontFamily = "Verdana";
		/// <summary>
		/// whether the entity is selected
		/// </summary>
		private bool mIsSelected;
		/// <summary>
		/// whether this entity is being hovered
		/// </summary>
		private bool mIsHovered ;
		/// <summary>
		/// the unique identitfier
		/// </summary>
		private Guid mUID ;
		/// <summary>
		/// mText or label
		/// </summary>
		private string mText = "[Not set]";
		/// <summary>
		/// whether to show the mText label
		/// </summary>
		private bool mShowLabel = true;		
		/// <summary>
		/// the default mText color
		/// </summary>
		private Color mTextColor = Color.Black;
		/// <summary>
		/// the default font size in points
		/// </summary>
		private float mFontSize = 7.8F;
		/// <summary>
		/// the default font for entities
		/// </summary>
		private Font mFont;
		/// <summary>
		/// the mSite of the entity
		/// </summary>
		[NonSerialized] private IGraphSite mSite;
			

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the pen-object to paint and draw
		/// </summary>
		public Pen Pen
		{
			get{return mPen;}
			set{mPen = value;}
		}

		/// <summary>
		/// Gets the property-bag
		/// </summary>
		/// <remarks>The bag acts as a proxy-object for the properties and his part of the propertybag mechanism.</remarks>
		protected PropertyBag Bag
		{
			get{return mBag;}
		}
		
		/// <summary>
		/// Gets or sets whether the next painting roun will have to recalculate the size of the entity
		/// </summary>
		protected bool RecalculateSize
		{
			get{return mRecalculateSize;}
			set{mRecalculateSize = value;}
		}
		/// <summary>
		/// Gets the default black mPen for drawing text and lines
		/// </summary>
		protected Pen BlackPen
		{
			get{return mBlackPen;}
		}

		/// <summary>
		/// Gets or sets the font-family used by derived class to draw and paint on the canvas
		/// </summary>
		protected virtual string FontFamily
		{
			get{return mFontFamily;}
			set{mFontFamily = value;}
		}

		/// <summary>
		/// Gets the layer ths shape is on. If null, the shape is in the default layer.
		/// </summary>
		/// <remarks>User the SetLayer() method to set or change the layer.
		/// </remarks>
		public  GraphLayer Layer
		{
			get
			{
				return mLayer;
			}			
		}
		

		/// <summary>
		/// Gets or sets a general purpose tag object
		/// </summary>
		public object Tag
		{
			get{return mTag;}
			set{mTag = value;}
		}
		/// <summary>
		/// Gets or sets the mPen width
		/// </summary>
		[GraphMLData]public float PenWidth
		{
			get{return mPenWidth;}
			set{mPenWidth = value;}
		}

		/// <summary>
		/// Gets or sets whether the shape label should be shown.
		/// </summary>
		[GraphMLData]public virtual bool ShowLabel
		{
			get
			{
					
				return mShowLabel;
			}
			set
			{
				mShowLabel=value; this.Invalidate();
			}
		}

	    /// <summary>
		/// Allows to view/change the properties of the shape, most probably on double-clicking it.
		/// </summary>			
		public virtual PropertyBag Properties
		{
			get{return mBag;}

		}

		/// <summary>
		/// Gets or sets the entity label
		/// </summary>
		[GraphMLData]public virtual string Text
		{

			get
			{

				return mText;
			}
			set
			{
				if (value!=null)	mText=value;

			}
		}

		/// <summary>
		/// Gets or sets the mSite of the entity
		/// </summary>
		public IGraphSite Site
		{
			get{return mSite;}
			set{mSite = value;}
		}
		/// <summary>
		/// Tells wether the entity (shape) is selected
		/// </summary>
		public virtual bool IsSelected
		{
			set { Invalidate(); mIsSelected = value; Invalidate(); }
			get { return mIsSelected; }
		}
		/// <summary>
		/// Gets or sets whether the entity's UID is reset
		/// </summary>
		/// <remarks>USed in the cotext of copy/paste</remarks>
		protected internal virtual bool IsGuiReset
		{
			get{return mIsGuiReset;}
			set{mIsGuiReset = value;}
		}
		/// <summary>
		/// Gives true if the mouse is hovering over this entity
		/// </summary>
		protected internal virtual bool IsHovered
		{
			set { Invalidate(); mIsHovered = value; Invalidate(); }
			get { return mIsHovered; }
		}

		/// <summary>
		/// Gets or sets the mText color
		/// </summary>
		public virtual Color TextColor
		{
			get
			{					
				return mTextColor;
			}
			set
			{
				mTextColor=value;
			}
		}

		/// <summary>
		/// Gets or sets the font size of the mText
		/// </summary>
		protected virtual float FontSize
		{
			get
			{
					
				return mFontSize;
			}
			set
			{
				mFontSize=value;
			}
		}
	

		
		/// <summary>
		/// Gets or sets the font to be used when drawing text-data
		/// </summary>
		protected Font Font
		{
			get{return this.mFont;}
			set
			{
				this.mFont = value;
				this.mFontFamily = value.FontFamily.ToString();
				this.mFontSize = value.Size;				
			}
		
		}

		/// <summary>
		/// Gets or sets the unique identifier for the shape.
		/// </summary>
		public Guid UID
		{
			get
			{
				return mUID;
			}
			set{mUID = value;}
		}

		/// <summary>
		/// Gets the Summary for this entity
		/// </summary>
		public Summary Summary
		{
			get
			{
				
				return this.Site.GetSummary(this);
			}
		}

		/// <summary>
		/// Gets the tracker of the entity
		/// </summary>
		public virtual Tracker Tracker
		{
			get{return null;}
			set{}
		}
		#endregion
		
		#region Constructors
		/// <summary>
		/// Constructor for the entity class, initializes a new GUID for the entity
		/// </summary>
		protected Entity()
		{
			InitEntity();
		}
		/// <summary>
		/// Creates a new entity, specifying the mSite 
		/// </summary>
		/// <param name="site"></param>
		protected Entity(IGraphSite site)
		{
			InitEntity();
			this.mSite = site;
			
		}
		/// <summary>
		/// Deserialization constructor
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected Entity(SerializationInfo info, StreamingContext context)
		{
			InitEntity();
			//overwrite some members with the serialized data
			this.mUID = new Guid(info.GetString("mUID"));
			this.mText = info.GetString("mText");
			this.mShowLabel = info.GetBoolean("mShowLabel");
			this.mTextColor = (Color) info.GetValue("mTextColor",typeof(Color));
			try
			{
				this.mFont = (Font) info.GetValue("mFont", typeof(Font));
			}
			catch
			{
				//font is set by default in the member definition
			}

		    Tag =  info.GetString("mLayer");
			//this.mIsSelected = info.GetBoolean("mIsSelected");
			
		}
		#endregion

		#region Methods
		/// <summary>
		/// IDispose implementation
		/// </summary>
		public void Dispose()
		{
			this.mFont.Dispose();
			this.mPen.Dispose();
			this.mBlackPen.Dispose();
			this.mBluePen.Dispose();
		}
		/// <summary>
		/// Sets the layer the entity belongs to
		/// </summary>
		/// <param name="layer"></param>
		protected virtual void SetLayer(GraphLayer layer)
		{
			mLayer = layer;
			return;			
		}

		/// <summary>
		/// Sets the shape in a layer.
		/// Use "default" to set the shape in the default layer.
		/// </summary>
		/// <param name="name"></param>
		public void SetLayer(string name)
		{
		    GraphLayer layer = Site.Abstract.Layers[name];
		    if (layer == null)
		    {
				SetLayer(Site.Abstract.DefaultLayer);
		    }
		    else
		    {
				SetLayer(Site.Abstract.Layers[name]);
		    }
		}
		/// <summary>
		/// Sets the shape in a layer.
		/// Layer 0 is the default layer.
		/// </summary>
		/// <param name="index"></param>
		public void SetLayer(int index)
		{
            SetLayer(Site.Abstract.Layers[index]);
		}


		/// <summary>
		/// Determines which properties are accessible via the property grid
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void GetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{
			switch(e.Property.Name)
			{
				case "Text": e.Value=this.mText;break;
				case "ShowLabel": e.Value=this.ShowLabel; break;
			}
		}


		/// <summary>
		/// Sets the values passed by the property grid
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected virtual void SetPropertyBagValue(object sender, PropertySpecEventArgs e)
		{

			switch(e.Property.Name)
			{
				case "ShowLabel": this.ShowLabel=(bool) e.Value; break;
				case "Text": 
					//use the logic and the constraint of the object that is being reflected
					if(e.Value.ToString() != null)					
					{
						this.mText=(string) e.Value;
					}
					else
						MessageBox.Show("Not a valid label","Invalid label",MessageBoxButtons.OK,MessageBoxIcon.Warning);					
					
					break;
			}
			this.Invalidate();
		}

		/// <summary>
		/// When overriden, allows user defined entities to get custom properties
		/// </summary>
		public virtual void AddProperties()
		{
			return;
		}
		/// <summary>
		/// Initializes the class. This method is necessary when deserializing since various elements like
		/// the Pen cannot be serialized to file and have to be, hence, set after deserialization.
		/// </summary>
		protected internal virtual void InitEntity()
		{

		    if (Site != null)
		    {
		        mLayer = Site.Abstract.DefaultLayer;
		    }
			mBluePen= new Pen(Brushes.DarkSlateBlue,1F);
			mBlackPen = new Pen(Brushes.Black,1F);
			mUID = Guid.NewGuid();
			mRecalculateSize = false;
			mFont = new Font(mFontFamily,mFontSize,FontStyle.Regular,GraphicsUnit.Point);
			mPen=new Pen(Brushes.Black, mPenWidth);
//			mLayer = Site.Abstract.DefaultLayer;//everything is initially in the default (static) layer
			mBag=new PropertyBag(this);
			try
			{					
				mBag.GetValue+=new PropertySpecEventHandler(GetPropertyBagValue);
				mBag.SetValue+=new PropertySpecEventHandler(SetPropertyBagValue);
				mBag.Properties.Add(new PropertySpec("Text",typeof(string),"Appearance","The text attached to the entity","[Not set]"));
				mBag.Properties.Add(new PropertySpec("ShowLabel",typeof(bool),"Appearance","Gets or sets whether the label will be shown."));
				//					PropertySpec spec=new PropertySpec("MDI children",typeof(string[]));
				//					spec.Attributes=new Attribute[]{new System.ComponentModel.ReadOnlyAttribute(true)};
				//					mBag.Properties.Add(spec);
				//AddProperties(); //add the user defined shape properties					
					
			}
			catch(Exception exc) //TODO: catch only those exceptions that you can handle gracefully
			{				
				Trace.WriteLine(exc.Message, "Entity.InitEntity");
			}
			catch
			{				
				Trace.WriteLine("Non-CLS compliant exception caught.", "Entity.InitEntity");
			}


		
		}

		/// <summary>
		/// creates the actual visual element on screen
		/// </summary>
		/// <param name="g"></param>
		public abstract void Paint(Graphics g);
		/// <summary>
		/// Gets the cursor for the current position of the mouse
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public abstract Cursor GetCursor(PointF p);

		/// <summary>
		/// GraphAbstract delete method; deletes the entity from the plex
		/// </summary>
		internal protected abstract void Delete();
		/// <summary>
		/// Says wether, for the given rectangle, the underlying shape is contained in it.
		/// </summary>
		/// <param name="r"></param>
		/// <returns></returns>
		public abstract bool Hit(RectangleF r);
		/// <summary>
		/// Invalidating refreshes part or all of a control
		/// </summary>
		public abstract void Invalidate();
		/// <summary>
		/// Allows to paints additional things like the clickable elements on shapes
		/// independently of the shape's design
		/// </summary>
		/// <param name="g"></param>
		public abstract void PaintAdornments(Graphics g);

		/// <summary>
		/// Paints the tracker of the entity
		/// </summary>
		/// <param name="g"></param>
		public void PaintTracker(Graphics g)
		{
			if(this.IsSelected) this.Tracker.Paint(g);
		}
		/// <summary>
		/// Regenerates a GUID for this entity
		/// </summary>
		public void GenerateNewUID()
		{
			mUID=Guid.NewGuid();
			mIsGuiReset=true;
		}


		/// <summary>
		/// Raises the mouse down event
		/// </summary>
		/// <param name="e"></param>
		internal virtual void RaiseMouseDown(MouseEventArgs e)
		{
			if(OnMouseDown != null) OnMouseDown(this, e);
		}

		/// <summary>
		/// Raises the mouse up event
		/// </summary>
		/// <param name="e"></param>
		internal void RaiseMouseUp(MouseEventArgs e)
		{
			if(OnMouseUp != null) OnMouseUp(this, e);
		}

		internal void RaiseMouseMove(MouseEventArgs e)
		{
			if(OnMouseMove!=null) OnMouseMove(this,e);
		}

		
		
		#endregion

		#region ISerializable Members

		/// <summary>
		/// ISerializable implementation
		/// </summary>
		/// <param name="info">the serialization info</param>
		/// <param name="context">the streaming context</param>
		[SecurityPermissionAttribute(SecurityAction.Demand,SerializationFormatter=true)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("mUID",this.mUID.ToString());			

			info.AddValue("mTextColor",this.mTextColor,typeof(Color));			

			info.AddValue("mText",this.mText);			

			info.AddValue("mShowLabel",this.mShowLabel);
			
			info.AddValue("mFont", this.mFont, typeof(Font));

			//additional check in case we're making a deep copy of a bundle
			if(mLayer==null)
				info.AddValue("mLayer","Default");
			else
				info.AddValue("mLayer",this.mLayer.Name);
			

		}
		/// <summary>
		/// Post-deserialization actions
		/// </summary>
		public virtual void PostDeserialization()
		{
			//add the properties
			this.AddProperties();
            if (Tag is string)
            {
                SetLayer((string)Tag);
                Tag = null; //be nice to the host/user
            }

        }

        #endregion
    }
}

