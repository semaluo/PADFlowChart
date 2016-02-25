/*

FIX2016021201:
在在GraphicControl.OnMouseDown，在GraphicControl.OnMouseMove，在GraphicControl.OnMouseUp
里添加代码，当GraphicControl.Locked为真时使GraphicControl忽略鼠标消息，
以便让用户自定义的代码完全接管鼠标事件的处理

FIX2016021101:
在GraphicControl.OnPaint里添加代码，使属性IsVisuable为false的Shape不显示。

FIX2016020902:
在GraphicControl.OnMouseDown里添加代码，调用base.OnMouseDown(e),
使GraphicControl的MouseDown事件链里的其它事件处理函数能够被调用

FIX2016020901:
在GraphicControl.OnMouseDown里添加代码，使Shape能够处理鼠标左键双击事件。

*/

using System;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.CompilerServices;

using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Resources;
using Netron.GraphLib;
using Netron.GraphLib.Configuration;
using Netron.GraphLib.Attributes;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Contexts;
using System.Windows.Forms.Design;
using Netron.GraphLib.Interfaces;
using System.Runtime.InteropServices;
using Netron.GraphLib.Utils;
using Netron.GraphLib.IO;
using System.Xml;
namespace Netron.GraphLib.UI
{
	/// <summary>
	/// The graph control is the container of shape objects and is an owner-drawn control
	/// </summary>
	/// <remarks>
	/// <br>the control is listening for hits, on hitting an object the HitEntity returns the entity hit and passes it to the Hover object in the HitHover handler.</br>
	/// <br>the biggest part of the code is taken by the handlers for the mouse events</br>
	/// </remarks>
	/// 	
	[Description("This UI controls allows to display shapes and diagrams.")]
	[Designer(typeof(Netron.GraphLib.UI.GraphControlDesigner))]
	[ToolboxItem(true)]
	public class GraphControl : ScrollableControl, IGraphSite, IGraphLayout
	{

		#region Constants
		private const int WM_VSCROLL = 0x0115;
		private const int WM_HSCROLL = 0x0114;

		#endregion

		#region Delegates and events
		/// <summary>
		/// raised when a properties request is issued
		/// </summary>
		[Category("Graph"), Description("Occurs on double-clicking the entity or via the context menu."), Browsable(true)]
		public event PropertiesInfo OnShowProperties;
		/// <summary>
		/// The OnInfo allows to output general purpose info from the canvas to the form. 
		/// Added in the context of the automata applications where scripting of nodes is available and where it is useful
		/// to give the user feedback about the pulsating automata.
		/// </summary>
		[Category("Graph"), Description("General purpose info output from the control."), Browsable(true)]
		public event InfoDelegate OnInfo;
		/// <summary>
		/// raised when a new connection is added
		/// </summary>
		[Category("Graph"), Description("Occurs when a new connection is added."), Browsable(true)]
		public event ConnectionInfo OnConnectionAdded;
		/// <summary>
		/// occurse when a new shape is added to the control
		/// </summary>
		[Category("Graph"), Description("Occurs when a new shape is added."), Browsable(true)]
		public event ShapeInfo OnShapeAdded;
		/// <summary>
		/// Occurs when a shape is removed
		/// </summary>
		[Category("Graph"), Description("Occurs when a shape is removed."), Browsable(true)]
		public event ShapeInfo OnShapeRemoved;
		/// <summary>
		/// Occurs when the canvas is cleared
		/// </summary>
		[Category("Graph"), Description("Occurs when the canvas is cleared."), Browsable(true)]
		public event EventHandler OnClear;
		/// <summary>
		/// Occurs when the context-menu is shown
		/// </summary>
		[Category("Graph"), Description("Occurs when the context menu is called."), Browsable(true)]
		public event MouseEventHandler OnContextMenu;
		/// <summary>
		/// Occurs when the automata is started
		/// </summary>
		[Category("Graph"), Description("Occurs when the automata calculation is started."), Browsable(true)]
		public event EventHandler OnStartAutomata;
		/// <summary>
		/// Occurs when the automata is stopped
		/// </summary>
		[Category("Graph"), Description("Occurs when the automata calculation is stopped."), Browsable(true)]
		public event EventHandler OnStopAutomata;
		/// <summary>
		/// Occurs on trasnmitting data (related to the dataflow/automaton)
		/// </summary>
		[Category("Graph"), Description("Occurs when the automata transmits data bewteen nodes and updates the cell state."), Browsable(true)]
		public event EventHandler OnDataTransmission;
	
		/// <summary>
		/// Occurs on requesting to show the graph properties
		/// </summary>
		[Category("Graph"), Description("Occurs when the control requests the host to show the graph properties."), Browsable(true)]
		public event InfoDelegate OnShowPropertiesDialogRequest;
		/// <summary>
		/// Occurs on requesting to show the graph layers
		/// </summary>
		[Category("Graph"), Description("Occurs when the control requests the host to show the graph layers."), Browsable(true)]
		public event InfoDelegate OnShowGraphLayers;

		#region Open/save events
		/// <summary>
		/// Occurs before saving a diagram
		/// </summary>
		[Category("Graph"), Description("Occurs before saving a diagram."), Browsable(true)]
		public event Netron.GraphLib.FileInfo OnSavingDiagram;
		/// <summary>
		/// Occurs after saving a diagram
		/// </summary>
		[Category("Graph"), Description("Occurs after saving a diagram."), Browsable(true)]
		public event Netron.GraphLib.FileInfo OnDiagramSaved;
		/// <summary>
		/// Occurs on opening a file
		/// </summary>
		[Category("Graph"), Description("Occurs when the control will open a file."), Browsable(true)]
		public event Netron.GraphLib.FileInfo OnOpeningDiagram;
		/// <summary>
		/// Occurs when a file was opened
		/// </summary>
		[Category("Graph"), Description(" Occurs when a file was opened."), Browsable(true)]
		public event Netron.GraphLib.FileInfo OnDiagramOpened;
		#endregion

		#endregion

		#region Fields


		/// <summary>
		/// whether the diagram is locked
		/// </summary>
		private bool mLocked;

        #region FIX2016021401
        /// <summary>
        /// determine which object is capturing the mouse event
        /// </summary>
        private Entity mCaptureObj;
        #endregion

        /// <summary>
        /// the tooltip control
        /// </summary>
        private ToolTip mToolTip;
		/// <summary>
		/// the brush used by the selector
		/// </summary>
		private readonly Brush selectionBrush = new SolidBrush(Color.FromArgb(120,Color.SteelBlue));

		/// <summary>
		/// the linear gradient mode
		/// </summary>
		private LinearGradientMode mGradientMode = LinearGradientMode.ForwardDiagonal;

		/// <summary>
		/// allow delete boolean
		/// </summary>
		protected bool mAllowDeleteShape = true;

		/// <summary>
		/// allow move shape boolean
		/// </summary>
		protected bool mAllowMoveShape = true;

		/// <summary>
		/// allow add shape boolean
		/// </summary>
		protected bool mAllowAddShape = true;

		/// <summary>
		/// allow add connection boolean
		/// </summary>
		protected bool mAllowAddConnection = true;

		/// <summary>
		/// free, non graph dependen arrow, useful for debugging purposes
		/// </summary>
		protected FreeArrowCollection freeArrows = new FreeArrowCollection();

		/// <summary>
		/// insertion point of the context-menu
		/// </summary>
		protected PointF insertionPoint = PointF.Empty;

		/// <summary>
		/// custom format (see bug problem in the copy/paste code
		/// </summary>
		static DataFormats.Format format = DataFormats.GetFormat("swa");

		/// <summary>
		/// the property bag of the canvas
		/// </summary>
		[NonSerialized] protected PropertyBag bag;

		/// <summary>
		/// the mContextMenu of the canvas
		/// </summary>
		protected ContextMenu mContextMenu = new ContextMenu();

		/// <summary>
		/// shortcut
		/// </summary>
		protected bool CtrlShift = false;
		
		/// <summary>
		/// shortcut
		/// </summary>
		protected bool AltKey = false;		
	
		/// <summary>
		/// the default path style of the new connections
		/// </summary>
		protected string connectionPath = "Default";

		/// <summary>
		/// the default connection end
		/// </summary>
		protected ConnectionEnd connectionEnd = ConnectionEnd.NoEnds;
		
		/// <summary>
		/// the grid size
		/// </summary>
		protected int mGridSize = 20;
		
		/// <summary>
		/// show grid?
		/// </summary>
		protected bool mShowGrid = false;
		
		/// <summary>
		/// whether to mSnap to the grid
		/// </summary>
		protected bool mSnap = false;
		
		/// <summary>
		/// keeps the last shape key for easy addition via ALT+click
		/// </summary>
		protected string lastAddedShapeKey = null;
		
		/// <summary>
		/// whether to enable the context menu
		/// </summary>
		protected bool mEnableContextMenu = false;
		
		/// <summary>
		/// whether to restrict the shapes to the canvas size
		/// </summary>
		protected bool mRestrictToCanvas = true;
		
		/// <summary>
		/// the shape mLibraries
		/// </summary>
		protected GraphObjectsLibraryCollection mLibraries = null;
		
		///<summary>
		/// This constant is used when checking for the CTRL key during drop operations.
		/// This makes the code more readable for future maintenance.
		/// </summary>
		protected const int ctrlKey = 8;
		
		/// <summary>
		/// a reference to the layout factory which organizes the layout algo's
		/// </summary>
		protected LayoutFactory layoutFactory = null;
		
		/// <summary>
		/// is the layout active?
		/// </summary>
		protected bool mEnableLayout = false;
		
		/// <summary>
		/// The thread pointer which will run on the layout thread.
		/// </summary>
		protected ThreadStart ts=null;
		
		/// <summary>
		/// The thread for laying out the graph.
		/// </summary>
		protected Thread thLayout = null;
		
		/// <summary>
		/// The timer interval in milliseconds that updates the state of the automata in the plex.
		/// </summary>
		protected int mAutomataPulse = 10; 
		
		/// <summary>
		/// the kinda background the canvas has
		/// </summary>
		protected CanvasBackgroundType mBackgroundType = CanvasBackgroundType.FlatColor;
		
		/// <summary>
		/// the color of the lower gradient part
		/// </summary>
		protected Color mGradientBottom = Color.White;
		
		/// <summary>
		/// the color of the upper gradient part
		/// </summary>
		protected Color mGradientTop = Color.LightSteelBlue;
		
		/// <summary>
		/// the image path for the background
		/// </summary>
		protected string mBackgroundImagePath=null;
		
		/// <summary>
		/// the background color
		/// </summary>
		protected Color mBackgroundColor = Color.WhiteSmoke;		
		
		/// <summary>
		/// the currentZoomFactor factor
		/// </summary>
		protected  float currentZoomFactor=1F;		
		
		/// <summary>
		/// The current filename of the diagram, if any
		/// </summary>
		protected string mFileName = null;      
		
		/// <summary>
		/// the extract contains the data of the plex and its structure
		/// </summary>
		internal  GraphAbstract extract = null;
		
		/// <summary>
		/// volatile object not connected to extract structure, used for the current added or selected shape
		/// </summary>
		protected Shape shapeObject = null;               
		
		/// <summary>
		/// volatile connection, used to manipulate the current connection
		/// </summary>
		protected Connection connection = null;         
		
		/// <summary>
		/// Entity with current mouse focus. Is automatically set by the HitHover and HitEntity handlers
		/// </summary>
		protected Entity Hover = null;        
		
		/// <summary>
		/// for tracking selection state, represents the dashed selection rectangle.
		/// </summary>
		protected Selector selector = null;
		
		/// <summary>
		/// indicates track mode, i.e. moving a shape around
		/// </summary>
		protected bool mDoTrack = false;      
		
		/// <summary>
		/// the timer controlling the refresh rate of the graph
		/// </summary>
		protected System.Windows.Forms.Timer transmissionTimer = new System.Windows.Forms.Timer();

		
		/// <summary>
		/// the automata controller widget
		/// </summary>
		private IWidget automataController;
		/// <summary>
		/// whether the data-flow is running
		/// </summary>
		private bool mIsAutomataRunning;
		/// <summary>
		/// whether the controller is visible
		/// </summary>
		private bool mShowAutomataController;
		
		#endregion

		#region Properties

		#region Browsable properties


		/// <summary>
		/// Gets or sets whether the internal dataflow is runnning
		/// </summary>
		public bool ShowAutomataController
		{
			get{return mShowAutomataController;}			
			set{mShowAutomataController = value;
			Invalidate();
			}
		}

		/// <summary>
		/// Gets or sets whether the internal dataflow is runnning
		/// </summary>
		public bool IsAutomataRunning
		{
			get{return mIsAutomataRunning;}			
		}

		/// <summary>
		/// Gets or sets whether the tooltip is active.
		/// </summary>
		public bool EnableToolTip
		{
			get{return this.mToolTip.Active;}
			set{this.mToolTip.Active = value;}
		}
		/// <summary>
		/// Gets or sets whether the whole diagram is locked
		/// </summary>
		public bool Locked 
		{
			get{return mLocked;}
			set{mLocked = value;}
		}
		/// <summary>
		/// Gets or sets the tooltip
		/// </summary>
		public ToolTip ToolTip
		{
			get{return mToolTip;}
			set{mToolTip = value;}
		}
		/// <summary>
		/// Gets or sets whether shapes can be moved in the graph
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets whether shapes can be moved in the graph. Individual shapes can be fixed with the 'CanMove' property.")]
		public bool AllowMoveShape
		{
			get{return this.mAllowMoveShape;}
			set{this.mAllowMoveShape = value;}
		}

		/// <summary>
		/// Gets or sets whether shapes can be deleted from the graph
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets whether shapes can be deleted from the graph.")]
		public bool AllowDeleteShape
		{
			get{return this.mAllowDeleteShape;}
			set{this.mAllowDeleteShape = value;}
		}

		/// <summary>
		/// Gets or sets whether shapes can be added to the graph
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets whether shapes can be added to the graph.")]
		public bool AllowAddShape
		{
			get{return this.mAllowAddShape;}
			set{this.mAllowAddShape = value;}
		}

		/// <summary>
		/// Gets or sets whether connections can be added to the graph
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets whether connections can be added to the graph.")]
		public bool AllowAddConnection
		{
			get{return this.mAllowAddConnection;}
			set{this.mAllowAddConnection = value;}
		}

		/// <summary>
		/// Gets or sets the default path style or shape of the newly created connections
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets the default line end of the newly created connections.")]
		public ConnectionEnd DefaultConnectionEnd
		{
			get{return this.connectionEnd;}
			set{this.connectionEnd = value;}
		}
		
		/// <summary>
		/// Gets or sets the default path style or shape of the newly created connections
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets the default path style or shape of the newly created connections.")]
		public string DefaultConnectionPath
		{
			get{return this.connectionPath;}
			set{this.connectionPath = value;this.Invalidate();}
		}

		/// <summary>
		/// Gets or sets whether the grid is visible.
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets whether the grid is visible.")]
		public bool ShowGrid
		{
			get{return mShowGrid;}
			set{mShowGrid = value;this.Invalidate();}
		}

		/// <summary>
		/// Gets or sets the grid size.
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets the grid size.")]
		public int GridSize
		{
			get{return mGridSize;}
			set{mGridSize = value;this.Invalidate();}
		}
		/// <summary>
		/// Gets or sets whether the graph elements mSnap to the grid
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets whether the graph elements mSnap to the grid.")]
		public bool Snap
		{
			get{return mSnap;}
			set{mSnap = value;}
		}

		/// <summary>
		/// Gets or sets whether the context menu is visible
		/// </summary>
		[Category("Appearance"), Browsable(true), Description("Gets or sets whether the context menu is visible.")]
		public bool EnableContextMenu
		{
			get{return mEnableContextMenu;}
			set
			{
				mEnableContextMenu = value;
				if(value)
					ContextMenu=mContextMenu;
				else
					ContextMenu = null;
			}
		}
		/// <summary>
		/// Gets or sets the background color of the canvas.
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets the background color of the canvas.")]
		public Color BackgroundColor 
		{
			get{return mBackgroundColor;}
			set
			{
				mBackgroundColor = value;
				Invalidate();
			}
		}
		
		/// <summary>
		/// Gets or sets the upper color of the gradient.
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets the upper color of the gradient.")]
		public Color GradientTop		
		{
			get{return mGradientTop;}
			set
			{
				mGradientTop = value;
				this.Invalidate();
			}
		}
		/// <summary>
		/// Gets or sets the lower color of the gradient.
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets the lower color of the gradient.")]
		public Color GradientBottom
		{
			get{return mGradientBottom;}
			set
			{
				mGradientBottom = value;
				this.Invalidate();
			}
		}
		/// <summary>
		/// Gets or sets time interval of the automata update pulse.
		/// </summary>
		[Category("Automata"), Browsable(true), Description("Gets or sets time interval of the automata update pulse.")]
		public int AutomataPulse
		{
			get{return mAutomataPulse;}
			set
			{
				mAutomataPulse = value;
				this.transmissionTimer.Interval = value;
			}

		}
		/// <summary>
		/// Gets or sets wether the graph shapes should be kept inside the canvas frame or allowed to move/resize outside it.
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets wether the graph shapes should be kept inside the canvas frame or allowed to move/resize outside it.")]
		public  bool RestrictToCanvas
		{
			get
			{
				return mRestrictToCanvas;
			}
			set
			{
				mRestrictToCanvas = value;
			}
		}
		/// <summary>
		/// Gets or sets the kind of background the canvas has.
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets the kind of background the canvas has.")]
		public CanvasBackgroundType BackgroundType
		{
			get{return this.mBackgroundType;}
			set
			{
				this.mBackgroundType = value;
				this.Invalidate();
			}
		}
		/// <summary>
		/// Gets or sets whether layout algorithms can be applied
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets whether layout algorithms are active.")]
		public bool EnableLayout
		{
			get{return mEnableLayout;}
			set
			{
				mEnableLayout = value;
				if(mEnableLayout)				
				{
					StartLayout();
				}
				else
				{
					StopLayout();
				}
			}
		}

		/// <summary>
		/// Gets or sets the graph layout algorithm to be used.
		/// </summary>
		[ReadOnly(false), Browsable(true), Description("The layout algorithm to be used."), Category("Graph")]
		public GraphLayoutAlgorithms GraphLayoutAlgorithm
		{
			get{return layoutFactory.GraphLayoutAlgorithm;}
			set{layoutFactory.GraphLayoutAlgorithm=value; }
		}

		/// <summary>
		/// Gets or sets the background image path
		/// </summary>
		[Category("Graph"), Browsable(true), Description("Gets or sets the path to the background image (only visible or used if the background type is 'Image').")]
		public string BackgroundImagePath
		{
			get{ return mBackgroundImagePath;}
			set{mBackgroundImagePath = value;}
		}
		#endregion

		#region Non-browsable properties

		/// <summary>
		/// Gets or sets the gradient mode
		/// </summary>
		public LinearGradientMode GradientMode
		{
			get{return mGradientMode;}
			set{mGradientMode = value;}
		}
		
	
		/// <summary>
		/// Gets the shape layers
		/// </summary>
		public GraphLayerCollection Layers
		{
			get{return this.extract.Layers;}			
		}

		/// <summary>
		/// Gets or sets whether tracking is on
		/// </summary>
		public bool DoTrack
		{
			get{return mDoTrack;}
			set{mDoTrack = value;}
		}

		/// <summary>
		/// Gets the collection of loaded libraries
		/// </summary>
		public GraphObjectsLibraryCollection Libraries
		{
			get
			{
				return this.mLibraries;
			}
		}

		/// <summary>
		/// Gets the collection of selected shapes
		/// </summary>
		public ShapeCollection SelectedShapes
		{
			get
			{
				ShapeCollection shapes = new ShapeCollection();
				foreach(Shape shape in this.Abstract.Shapes)
					if(shape.IsSelected)
						shapes.Add(shape);
				return shapes;
			}
		}

		/// <summary>
		/// Gets the abstract
		/// </summary>
		public GraphAbstract Abstract
		{
			get{return extract;}
		}
		/// <summary>
		/// Gets the propertybag of the control.
		/// </summary>
		/// <remarks>This is part of the mechanism related to the propertygrid</remarks>
		public virtual PropertyBag Properties
		{
			get{return bag;}

		}
		/// <summary>
		/// Gets the graphics object of this control
		/// </summary>
		public Graphics Graphics
		{
			get{return this.CreateGraphics();}
		}

		/// <summary>
		/// Gets the center position of the control
		/// </summary>
		[Browsable(false)]
		protected PointF Center
		{
			get{return new PointF(this.Width/2,this.Height/2);}
		}

		/// <summary>
		/// Gets or sets the background image of the canvas
		/// </summary>
		/// <remarks>You need to set the background-type to make the image visible <seealso cref="BackgroundType"/></remarks>
		[Browsable(false)]
		public override Image BackgroundImage
		{
			get
			{
				return null;
			}
			set
			{
				return ;
			}
		}

		/// <summary>
		/// Sets the color of the shape
		/// </summary>
		[Browsable(false)]
		public Color ShapeColor
		{
			set
			{
				foreach(Shape so in extract.Shapes)
					if(so.IsSelected) so.ShapeColor=value;
			}
		}
		/// <summary>
		/// Sets the font to be used when drawing shape-text
		/// </summary>
		[Browsable(false)]
		public float LabelFontSize
		{
			set
			{
				foreach(Shape so in extract.Shapes)
					if(so.IsSelected) so.FontSize = value;
			}
		}
		/// <summary>
		/// Sets the color of the shape-text
		/// </summary>
		[Browsable(false)]
		public Color TextColor
		{
			set
			
			{
				foreach(Shape so in extract.Shapes)
					if(so.IsSelected) so.TextColor=value;
			}
		}
		/// <summary>
		/// Sets whether the label is visible
		/// </summary>
		[Browsable(false)]
		public bool ShowLabel
		{
			set
			{
				foreach(Shape so in extract.Shapes)
					if(so.IsSelected) so.ShowLabel=value;
			}
		}
		/// <summary>
		/// Sets the default dash-style of new connections
		/// </summary>
		[Browsable(false)]
		public DashStyle LineStyle
		{
			set
			{

				foreach(Shape so in extract.Shapes)
					foreach(Connector co in so.Connectors)
						foreach (Connection con in co.Connections)
							if(con.IsSelected) con.LineStyle=value;
			}
		}

		/// <summary>
		/// Sets the default line weight of new connections
		/// </summary>
		[Browsable(false)]
		public ConnectionWeight LineWeight
		{
			set
			{
				foreach(Shape so in extract.Shapes)
					foreach(Connector co in so.Connectors)
						foreach (Connection con in co.Connections)
							if(con.IsSelected) con.LineWeight=value;
			}
		}
		/// <summary>
		/// Sets the default line-end of new connections
		/// </summary>
		[Browsable(false)]
		public ConnectionEnd LineEnd
		{
			set
			{
				foreach(Shape so in extract.Shapes)
					foreach(Connector co in so.Connectors)
						foreach (Connection con in co.Connections)
							if(con.IsSelected) con.LineEnd=value;
			}
		}
		/// <summary>
		/// Sets the default line-color
		/// </summary>
		[Browsable(false)]
		public Color LineColor
		{
			set
			{
				foreach(Shape so in extract.Shapes)
					foreach(Connector co in so.Connectors)
						foreach (Connection con in co.Connections)
							if(con.IsSelected) con.LineColor=value;
			}
		}

		/// <summary>
		/// Gets or sets the currentZoomFactor factor
		/// </summary>
		[Browsable(false)]
		public  float Zoom
		{
			get
			{
				return currentZoomFactor;
			}
			set
			{
				if(value==currentZoomFactor) return;
				/*if(value>10 && value<500)
				{
					float amount;
					amount=((float) value/currentZoomFactor);
					if(this.extract != null)
					{					
						foreach (Shape so in this.extract.Shapes)
							so.Zoom(amount);
					}
					currentZoomFactor=value;
				}*/
				currentZoomFactor=value;
			
				//this.AutoScrollMargin = new Size
				//this.AutoScrollPosition = Point.Round(new PointF(Width*(currentZoomFactor-1)*2,Height*(currentZoomFactor-1)*2));
				
				this.Invalidate();
			}
		}
		/// <summary>
		/// Gets or sets the backcolor (redirected to the background color)
		/// </summary>
		[Browsable(false)]
		public override Color BackColor
		{
			get
			{
				return mBackgroundColor;
			}
			set
			{
				mBackgroundColor = value;
			}
		}
		/// <summary>
		/// Gets or sets the file name of the current graph
		/// </summary>
		[Browsable(false)]
		public string FileName
		{
			get{return mFileName;}
			set{mFileName = value;}
		}
		/// <summary>
		/// Gets the number of edges or connections in the graph.
		/// </summary>
		[ReadOnly(true),Browsable(false)]
		public int EdgeCount
		{
			get
			{
				return Connections.Count;
			}
		}
		/// <summary>
		/// Gets the number of nodes on the canvas
		/// </summary>
		[ReadOnly(true), Browsable(false)]
		public int NodesCount
		{
			get
			{
				return extract.Shapes.Count;				
			}
		}
		/// <summary>
		/// Gets the arraylist of nodes in the graph.
		/// </summary>
		/// 
		[ReadOnly(true), Browsable(false)]
		public ShapeCollection Shapes
		{
			get
			{
				return extract.Shapes;
			}
		}
		/// <summary>
		/// Gets the arraylist of edges in the graph. A more elegant way to implement this would be to keep the collection up to date
		/// while adding or deleting connections but it would mean a doubling since the connections are already in seperate arraylists all the time, hence this method.
		/// </summary>
		/// 
		[ReadOnly(true), Browsable(false)]
		public ConnectionCollection Connections
		{
			get
			{
				/* Explicitly:
				ArrayList cons=new ArrayList();
				foreach(Shape so in extract.Shapes)
					foreach(Connector co in so.Connectors)
						foreach(Connection con in co.Connections)
							if (!cons.Contains(con)) cons.Add(con);

				return cons;
				*/
				return extract.Connections;
			}
		}
		
		/// <summary>
		/// Removes the edge from the diagram with the given UID
		/// </summary>
		/// <param name="uid"></param>
		public void RemoveEdge(System.Guid uid)
		{
			foreach(Shape so in extract.Shapes)
				foreach(Connector co in so.Connectors)
					foreach(Connection con in co.Connections)
						if (con.UID==uid) con.Delete();
		}

		#endregion
		#endregion
		
		#region Constructor & finalize		
		/// <summary>
		/// Class constructor
		/// </summary>
		/// <remarks>
		/// Notice the double buffering directives at the end
		/// </remarks>
		public GraphControl()
		{		
			//Make sure the control repaints as it is resized
			SetStyle(ControlStyles.ResizeRedraw, true);
			//allow dragdrop
			this.AllowDrop=true;
			//instantiate the layout factory
			layoutFactory=new LayoutFactory(this);			

			//instanciating an attached extract to the canvas
			extract = new GraphAbstract();
			extract.Site=this;

			//init the transmission timer
			transmissionTimer.Interval = AutomataPulse; 
			transmissionTimer.Tick += new EventHandler(OnTransmissionTimer); //attach of the handler
			
			//enable double buffering of graphics
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			
			//instantiate the library collection
			mLibraries = new GraphObjectsLibraryCollection();

			
			this.KeyDown+=new KeyEventHandler(OnKeyDown);
			this.KeyPress+=new KeyPressEventHandler(OnKeyPress);
			
			AddBaseMenu();
			bag = new PropertyBag();
			AddProperties();

			//init the automata widget
			automataController = new AutomataController(this);

			//scrolling stuff
			this.AutoScroll=true;			
			this.HScroll=true;
			this.VScroll=true;

	
		

			try
			{
				Assembly ass= Assembly.GetExecutingAssembly();

				MouseCursors.Add = new Cursor(  ass.GetManifestResourceStream("Netron.GraphLib.Resources.Add.cur"));
				MouseCursors.Cross = new Cursor(    ass.GetManifestResourceStream("Netron.GraphLib.Resources.Cross.cur"));
				MouseCursors.Grip =  new Cursor(   ass.GetManifestResourceStream("Netron.GraphLib.Resources.Grip.cur"));
				MouseCursors.Move =  new Cursor(  ass.GetManifestResourceStream("Netron.GraphLib.Resources.Move.cur"));
				MouseCursors.Select =  new Cursor(  ass.GetManifestResourceStream("Netron.GraphLib.Resources.Select.cur"));

				//automatically add the control itself since it contains the default connection
				//which on deserialization needs to be found in the library collection
				LoadSelfLibrary();

				mToolTip = new ToolTip();
				this.mToolTip.InitialDelay = 1000;
				this.mToolTip.AutomaticDelay = 1000;
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message,"GraphControl.GraphControl");
			}
			
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				//TODO: disposing things, what?
			}
			base.Dispose( disposing );
		}
        #endregion

        #region Methods

        public ConnectionCollection ConnectionsOfLayer(string layerName)
        {
            return Abstract.ConnectionsOfLayer(layerName);
        }

        /// <summary>
        /// return the shapes of layer
        /// </summary>
        /// <param name="layerName"></param>
        /// <returns></returns>
        public ShapeCollection ShapesOfLayer(string layerName)
        {
            return Abstract.ShapesOfLayer(layerName);
        }
        #region Control logic

        #region Zoomers


        /// <summary>
        /// Zooms given point.
        /// </summary>
        /// <param name="originalPt">Point to currentZoomFactor</param>
        /// <returns>zoomed point.</returns>
        public Point ZoomPoint(Point originalPt)
		{
			Point newPt = new Point((int)(this.currentZoomFactor*originalPt.X), (int)( this.currentZoomFactor*originalPt.Y ));
			return newPt; 
		}
		/// <summary>
		/// Unzooms given point.
		/// </summary>
		/// <param name="originalPt">Point to unzoom</param>
		/// <returns>Unzoomed point.</returns>
		public Point UnzoomPoint(Point originalPt)
		{
			Point newPt = new Point((int)(originalPt.X/this.currentZoomFactor), (int)(originalPt.Y /this.currentZoomFactor));
			return newPt;
		}

		/// <summary>
		/// Zooms a given point.
		/// </summary>
		/// <param name="originalPt">Point to currentZoomFactor</param>
		/// <returns>Zoomed point.</returns>
		public PointF ZoomPoint(PointF originalPt)
		{
			PointF newPt = new PointF( this.currentZoomFactor*originalPt.X ,  this.currentZoomFactor*originalPt.Y );
			return newPt; 
		}

		/// <summary>
		/// Unzooms a given point.
		/// </summary>
		/// <param name="originalPt">Point to unzoom.</param>
		/// <returns>Unzoomed point.</returns>
		public PointF UnzoomPoint(PointF originalPt)
		{
			PointF newPt = new PointF(originalPt.X/this.currentZoomFactor, originalPt.Y/this.currentZoomFactor);
			return newPt;
		}
		/// <summary>
		/// Zooms a given rectangle.
		/// </summary>
		/// <param name="originalRect">Rectangle to currentZoomFactor.</param>
		/// <returns>Zoomed rectangle.</returns>
		public RectangleF ZoomRectangle(RectangleF originalRect)
		{
			RectangleF newRect = new RectangleF( this.currentZoomFactor*originalRect.X ,  this.currentZoomFactor*originalRect.Y , 
				originalRect.Width * this.currentZoomFactor, originalRect.Height * this.currentZoomFactor);
			return newRect; 
		}
		/// <summary>
		/// Unzooms given rectangle.
		/// </summary>
		/// <param name="originalRect">Rectangle to unzoom.</param>
		/// <returns>Unzoomed rectangle.</returns>
		public RectangleF UnzoomRectangle(RectangleF originalRect)
		{
			RectangleF newRect = new RectangleF(originalRect.X /this.currentZoomFactor, originalRect.Y /this.currentZoomFactor, 
				originalRect.Width / this.currentZoomFactor, originalRect.Height / this.currentZoomFactor);
			return newRect;
		}


		/// <summary>
		/// Zooms given rectangle.
		/// </summary>
		/// <param name="originalRect">Rectangle to currentZoomFactor</param>
		/// <returns>Zoomed rectangle</returns>
		public Rectangle ZoomRectangle(Rectangle originalRect)
		{
			Rectangle newRect = new Rectangle((int)(this.currentZoomFactor*originalRect.X ), (int)( this.currentZoomFactor*originalRect.Y ), 
				(int)(originalRect.Width * this.currentZoomFactor), (int)(originalRect.Height * this.currentZoomFactor));
			return newRect; 
		}
		/// <summary>
		/// Unzooms given rectangle. 
		/// </summary>
		/// <param name="originalRect">Rectangle to unzoom</param>
		/// <returns>Unzoomed rectangle.</returns>
		public Rectangle UnzoomRectangle(Rectangle originalRect)
		{
			Rectangle newRect = new Rectangle((int)(originalRect.X /this.currentZoomFactor), (int)(originalRect.Y /this.currentZoomFactor), 
				(int)(originalRect.Width / this.currentZoomFactor), (int)(originalRect.Height / this.currentZoomFactor));
			return newRect;
		}

		
		
		
	
		#endregion

		/// <summary>
		/// Let the site invalidate the rectangle
		/// </summary>
		/// <param name="rect">invalid rectangle</param>
		public void InvalidateRectangle( Rectangle rect )
		{
			Invalidate( rect );
		}

		/// <summary>
		/// This is the event handler for the mousemove and returns an entity when it hits anything from the plex.
		/// The Mouse coordinates are converted to a RectangleF of zero size and passed to all the entities in the plex.
		/// If one of the plex finds that the cursor is contained in itself then it recturns itself.
		/// This place can be used to give feedback through statusbar messages or any other info coming from the entities.
		/// 
		/// </summary>
		/// <param name="p">Coordinates, normally this should be the mouse coordinates.</param>
		/// <returns>An entity; shape, connector, connection.</returns>
		protected Entity HitEntity(PointF p)
		{
			if (extract ==null) return null;
			RectangleF r = new RectangleF(p.X, p.Y, 0, 0);

			foreach (Shape o in extract.Shapes)
			{
				if(!o.Layer.Visible) continue;
				foreach (Connector c in o.Connectors)
					if (c.Hit(r))
					{
						//String s = "\"" + c.Description + "\" connector of \"" + c.Shape.GetType().FullName + "\" object.";
						//Trace.WriteLine("The mouse hit the connector " +c.Description);
						return c;
					}
			}

			EntityCollection ec = extract.paintables;


			for(int k=ec.Count-1; k>-1; k--)//paintables are order from deep Z-order to high Z-order
			{
				if(!ec[k].Layer.Visible) continue;
				if(ec[k].Hit(r)) return ec[k];
			}
			/*
			#region ------------------ test connector hits
			foreach (Shape o in extract.Shapes)
			{
				if(!o.Layer.Visible) continue;
				foreach (Connector c in o.Connectors)
					if (c.Hit(r))
					{
						//String s = "\"" + c.Description + "\" connector of \"" + c.Shape.GetType().FullName + "\" object.";
						//Trace.WriteLine("The mouse hit the connector " +c.Description);
						return c;
					}
			}
			#endregion
			#region------------------ test shape hits

			ShapeCollection col = new ShapeCollection();

			foreach (Shape o in extract.Shapes)
			{
				if(!o.Layer.Visible) continue;
				if (o.Hit(r))
				{
					col.Add(o);
				}
			}
			if(col.Count==1)
				return col[0];
			else if(col.Count>1)
			{
				col.Sort("ZOrder",SortDirection.Ascending); 
				return col[0];
			}

			#endregion
			#region ---------------- test connection hits
			foreach (Shape o in extract.Shapes)
				foreach (Connector c in o.Connectors)
					foreach (Connection n in c.Connections)
					{
						if(n.From.BelongsTo.Layer.Visible && n.To.BelongsTo.Layer.Visible) 
						{
							if (n.Hit(r))	return n;
						}
					}
			#endregion
			//--------------- nothing was hit at all...
			*/
			return null;
		}

		/// <summary>
		/// Pretty much the same as the HitEntity handler, it simply checks if the HitEntity returns something different than the previous hit.
		/// If it is something different than the previous hit it sets the internal 'Hover' entity to the new one.
		/// </summary>
		/// <param name="p">In normal circumstances this should be the mouse coordinates.</param>
		protected void HitHover(PointF p)
		{
			Entity Hit = HitEntity(p);
			
			if (Hit != Hover)
			{
				if (Hover != null) Hover.IsHovered = false;
				Hover = Hit;
				//				if(Hover==null)
				//					{Trace.WriteLine(DateTime.Now.ToString() + ": changed Hover to null");}
				//				else
				//					{Trace.WriteLine(DateTime.Now.ToString() + ": changed Hover to " + Hover.ToString());};
				if (Hover != null)
				{
					Hover.IsHovered = true;
				}
			}
		}
		/// <summary>
		/// Collects all modules of the astract and sets the IsSelected to False
		/// </summary>
		
		public void Deselect()
		{
			foreach (Shape o in extract.Shapes)
			{				
				o.IsSelected=false;
				foreach (Connector c in o.Connectors)
					foreach (Connection n in c.Connections)
						n.IsSelected=false;
						
			}
		}

		/// <summary>
		/// For the given point returns a cursor
		/// </summary>
		/// <param name="p">Floating-point point, usually linked to the cursor</param>
		protected void SetCursor(PointF p)
		{
			if (connection != null)
			{
                //if ((Hover != null) && (Hover is Connector))
			    if ((Hover != null) && (Hover is Entity))
			        Cursor = Hover.GetCursor(p);
			    else
			    {
			        if (connection.To == null)
			        {
			            Cursor = MouseCursors.Cross;
			        }
			        else
			        {
			            Cursor = Cursors.Arrow;
			        }
			    }
					
			}
			else
			{
				//give priority to the widgets, if visible
				if(mShowAutomataController && automataController.Hit(new Rectangle(Point.Round(p),new Size(2,2))))
				{
					Cursor = automataController.GetCursor(p);
					return;
				}
				if (Hover != null)
					Cursor = Hover.GetCursor(p);
				else
					Cursor = Cursors.Arrow;
			}
		}


        #region FIX2016021401

        /// <summary>
        /// Make the Entity obj capture the mouse event
        /// OnMouseDown, OnMouseMove, OnMouseUp function will check mCaptureObj
        /// </summary>
        /// <param name="obj"></param>
	    public void StartCapture(Entity obj)
	    {
            mCaptureObj = obj;
	    }

        /// <summary>
        /// Release the mouse event capture
        /// </summary>
	    public void EndCapture()
	    {
            mCaptureObj = null;
	    }
        #endregion

        /// <summary>
        /// Handles the mouse down event
        /// </summary>
        /// <param name="e">Events arguments</param>

        protected override void OnMouseDown(MouseEventArgs e)
		{
            #region FIX2016020902
                base.OnMouseDown(e);
            #endregion

			//make sure the canvas has the focus			
			this.Focus();

            #region FIX2016021401
            if (mCaptureObj != null)
            {
                mCaptureObj.RaiseMouseDown(e);
                return;
            }
            #endregion

            #region FIX2016021201
            if (mLocked)
            {
                return;
            }
            #endregion

            // Get a point adjusted by the current scroll position and zoom factor
            PointF p = new PointF(e.X - this.AutoScrollPosition.X, e.Y - this.AutoScrollPosition.Y);
			//give priority to the widgets, if visible
			if(mShowAutomataController && automataController.Hit(new Rectangle(e.X,e.Y,2,2)))
				automataController.OnMouseDown(p);

			p = UnzoomPoint(Point.Round(p));

			//pass the event to the hit entity
			HitHover(p);
			if(!mLocked)
			{
				#region Ctrl+Shift left
				if((e.Button==MouseButtons.Left) && (e.Clicks==1) && (CtrlShift))
				{
					this.Zoom +=0.1F;
					CtrlShift = false;
					return;
				}
				#endregion

				#region Ctrl+Shift right
				if((e.Button==MouseButtons.Right) && (e.Clicks==1) && (CtrlShift))
				{
					this.Zoom -=0.1F;
					CtrlShift = false;
					this.ContextMenu = null;
					return;
				}

		

				#endregion

				#region Alt+Shift Click for dragdrop
				if((e.Button==MouseButtons.Left) && (e.Clicks==1) && (AltKey))
				{
					Shape sh = null;
					if(this.SelectedShapes.Count>0)
					{
						sh = this.SelectedShapes[0];					
						this.DoDragDrop(sh as IShape, DragDropEffects.Copy);
					}

					AltKey = false;
					this.ContextMenu = null;
					return;
				}

				#endregion

				#region Double click left
				//shows the properties of the underlying object
				if ((e.Button == MouseButtons.Left) && (e.Clicks == 2))
				{	
					//do we hit something under the cursor?
					HitHover(p);

					if ((Hover != null) && (typeof(Entity).IsInstanceOfType(Hover)))
					{					
						this.RaiseShowProperties(((Entity) Hover).Properties);
						Update();

                        #region FIX2016020901
                        (Hover as Entity).RaiseMouseDown(e);
                        #endregion

                        return;
					}
				}
				#endregion

				#region SINGLE click right
				if (e.Button == MouseButtons.Right)
				{
					if (Hover != null)
					{
						if (!Hover.IsSelected)
						{
							//Select s = new Select();
							Deselect();
							//s.Add(Hover, true);
							//extract.History.Do(s);
							Hover.IsSelected=true;
							Update();
						}
					}

					if(this.mEnableContextMenu )
					{
						if(OnContextMenu !=null) OnContextMenu(this, e);

						this.ContextMenu=new ContextMenu();

						ResetToBaseMenu();
					
						if(Hover is Shape)
						{
							//MenuItem[] tmp = new MenuItem[mContextMenu.MenuItems.Count];
							//mContextMenu.MenuItems.CopyTo(tmp,0);	
						
							MenuItem[] additionals = (Hover as Shape).ShapeMenu();
							if(additionals != null)	
							{
								this.ContextMenu.MenuItems.Add("-");
								this.ContextMenu.MenuItems.AddRange(additionals);
							}
						
						}
						else if(typeof(Connection).IsInstanceOfType(Hover))
						{
							if((Hover as Connection).LinePath== "Rectangular") return;
							this.insertionPoint = p;
							this.ContextMenu.MenuItems.Add("-");
							MenuItem[] subconnection = new MenuItem[2]{
																		  new MenuItem("Add point",new EventHandler(AddConnectionPoint)),
																		  new MenuItem("Delete point",new EventHandler(RemoveConnectionPoint))
																	  };
							this.ContextMenu.MenuItems.Add(new MenuItem("Connection",subconnection));

						
					
						}
					}
				
				}
				#endregion

				#region SINGLE click left

				if ((e.Button == MouseButtons.Left) && (e.Clicks == 1))
				{
					// Alt+Click allows fast creation of elements
					if ((shapeObject == null) && (ModifierKeys == Keys.Alt))
					{
						if(!this.mLocked)
							shapeObject = this.GetShapeInstance(lastAddedShapeKey);

					}
					if (shapeObject != null)
					{
						shapeObject.Invalidate();
						RectangleF r = shapeObject.Rectangle;
						shapeObject.Rectangle = new RectangleF(p.X, p.Y, r.Width, r.Height);
						shapeObject.Invalidate();					
						extract.Insert(shapeObject);
						shapeObject = null;
						return;
					}
					//reset the selector marquee
					selector = null;
					//see if something under the cursor
					HitHover(p); 
					
					if (Hover != null)//the click resulted in an object; shape or connection
					{
						if (Hover is Connector)  
						{
							//more than one connection allowed?
							if(!((Connector) Hover).AllowMultipleConnections && ((Connector) Hover).Connections.Count>0) return;
							//allowed new connections from the connector?
							if(!((Connector) Hover).AllowNewConnectionsFrom) return;
							//allow connections on the control level?
							if(!mAllowAddConnection) return;

                            #region FIX2016021301
						    if (((Connector) Hover).BelongsTo is IConnectable)
						    {
						        connection = ((IConnectable) ((Connector) Hover).BelongsTo).CreateConnection((Connector) Hover);
						    }
						    else
						    {
                                connection = new Connection(this);
						    }
                            //FIX2016021301 DEL //connection = new Connection(this);
                            #endregion

                            connection.Site=this;
							connection.From = (Connector) Hover;
							connection.ToPoint = p; //we use the mouse as To as long as we haven't a real connector, which will occur in the OnMouseUp
							Hover.Invalidate();
							Capture = true;
							Update();
							return;
						}

						// select object or add to the list of selected objects
						if (!Hover.IsSelected)
						{
							if (ModifierKeys != Keys.Shift) Deselect(); //s is empty only if shift is pushed
							//shift-click adds only ONE item while a normal click can change a set of shapes to
							//another state.
							Hover.IsSelected=true;
							Update();
						}
						//fix the node if the layout is manipulating the position
						//					if (typeof(Shape).IsInstanceOfType(Hover))
						//					{
						//						foreach(Shape sho in extract.Shapes) sho.IsFixed=false;
						//						((Shape) Hover).IsFixed=true;
						//					}

						// search tracking handle
						Point h = new Point(0, 0);
						if (extract.Shapes.Contains(Hover))
						{
							Shape o = (Shape) Hover;
							h = o.Tracker.Hit(p);
						}

						foreach (Shape j in extract.Shapes)
						{
							if (j.Tracker != null) //will only be one tracker per hit
							{
								j.Tracker.Start(p, h);
								foreach(Connector c in j.Connectors)
								{
									foreach(Connection cn in c.Connections)
										if( cn.Tracker != null )
											cn.Tracker.Start(p,Point.Empty);
								}
									
							}

						}

					
						// Search tracker handle of connection and start tracking
						if( Hover is Connection )
						{
							connection = Hover as Connection;
							h = connection.Tracker.Hit(p);
							connection.Tracker.Start(p,h);
						}

						mDoTrack = true;
                        Capture = true;

						if ((Hover != null) && (Hover is Entity))
						{
							(Hover as Entity).RaiseMouseDown(e);
						}

						SetCursor(p);
						return;
					}

					//p=new PointF(e.X,e.Y);
					selector = new Selector(p, this);
			
				
				
				
				}
				#endregion
			}
			else //allow the adornments only
			{
                #region FIX2016020901
                //				if ((e.Button == MouseButtons.Left) && (e.Clicks == 1)) //FIX2016020901 DEL
                if ((e.Button == MouseButtons.Left)) //FIX2016020901 ADD
                #endregion
                {

                    if (Hover != null)//the click resulted in an object; shape or connection
					{
						if (Hover is Entity)
						{
							(Hover as Entity).RaiseMouseDown(e);
						}

						SetCursor(p);
						return;
					}

                    //p=new PointF(e.X,e.Y);
                    #region FIX2016020903
                    //selector = new Selector(p, this); //FIX2016020903 DEL: 
                    #endregion
                }
            }


			
	

			
		}

		/// <summary>
		/// Adds a connection point to a connection
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AddConnectionPoint(object sender, EventArgs e)
		{

			//freeArrows.Add(new FreeArrow(new PointF(0,0),insertionPoint, "(" + insertionPoint.X + "," + insertionPoint.Y + ")"));			

			(Hover as Connection).AddConnectionPoint(insertionPoint);
		}

		/// <summary>
		/// Removes a connection point
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RemoveConnectionPoint(object sender, EventArgs e)
		{
			(Hover as Connection).RemoveConnectionPoint(insertionPoint);
		}
		
		/// <summary>
		/// Handles the mouse up event
		/// </summary>
		/// <param name="e">Event arguments</param>
		protected override void OnMouseUp(MouseEventArgs e)
		{	
			
			base.OnMouseUp(e);

            #region FIX2016021401
            if (mCaptureObj != null)
            {
                mCaptureObj.RaiseMouseUp(e);
                return;
            }
            #endregion


            #region FIX2016021201
            if (mLocked)
		    {
		        return;
		    }
            #endregion

			// Get current mouse point adjusted by the current scroll position and zoom factor
			PointF p = new PointF(e.X - this.AutoScrollPosition.X, e.Y - this.AutoScrollPosition.Y);
			p = UnzoomPoint(Point.Round(p));
			
			#region MouseUp to the diagram: pass the event to the entity
			HitHover(p);
			if (Hover is Entity)
			{
				(Hover as Entity).RaiseMouseUp(e);
			}
			#endregion
			
			//paint the selector if there is one
			if (selector != null) Invalidate();

			#region Left mouse button
			if (e.Button == MouseButtons.Left)
			{
				#region New connection: are we dragging a new connection?
				if (connection != null)
				{
					HitHover(p);

					connection.Invalidate();
					//if the cursor is over a connector then attach new connection
					if ((Hover != null) && (Hover is Connector))
						if (!Hover.Equals(connection.From) && (Hover as Connector).AllowNewConnectionsTo)
						{
							//check if the connector can have an extra connection					
							if(((Connector) Hover).AllowMultipleConnections || ((Connector) Hover).Connections.Count==0) 
							{
			
			
								connection.Insert(connection.From,(Connector) Hover);
								connection.LinePath = this.connectionPath; //set the default path/shape style
								connection.LineEnd = this.connectionEnd; //the default end
								if(OnConnectionAdded != null)
								{
									if(!OnConnectionAdded(this,new ConnectionEventArgs(connection,true)))
									{
										connection.Delete(); //if the (external) handler tells it's not OK we delete the connection again
									}
								}
			
							}
						}

					connection = null;
					Capture = false;
				}
				#endregion

				#region Selector: are we dragging a marquee to select shapes?
				if (selector != null)
				{					
					RectangleF r = selector.Rectangle;
					//this.OutputInfo("Selector: (" + r.X + "," + r.Y + "),  [" + r.Width + "," + r.Height + "]");
					//r = this.UnzoomRectangle(r);
					//if ((Hover == null) || (Hover.IsSelected == false))
					if (ModifierKeys != Keys.Shift) Deselect();
					ArrayList selected = new ArrayList();//the objects passed to the propertygrid
					if ((r.Width != 0) || (r.Height != 0))
					{  
						foreach (Shape o in extract.Shapes)
						{
							if (o.Hit(r)) 
							{
								o.IsSelected=true; 							
								selected.Add(o.Properties);
							}

							foreach (Connector c in o.Connectors)
								foreach (Connection n in c.Connections)
								{
									if (n.Hit(r)) 
									{										
										n.IsSelected=true;
										c.Invalidate();
									}
								}
						}
					}
					selector = null;
					Capture = false;       
					
					//edit the properties
					object[] objs = new object[selected.Count];
					for(int k=0; k<selected.Count; k++)
					{
						objs[k] = selected[k];
					}
					RaiseShowProperties( objs);
				}
				#endregion

				#region Motion: are we draggging or resizing shapes?
				if (mDoTrack)
				{	
					foreach (Shape o in extract.Shapes)
						if (o.Tracker != null)
						{
							o.Tracker.End();
							o.Invalidate();				
							o.Rectangle=o.Tracker.Rectangle;
						}
					mDoTrack = false;
                    Capture = false;
					HitHover(p);
				}
				#endregion
			}
			#endregion
			this.Invalidate(true);
			Update();
			SetCursor(p);
		}
		/// <summary>
		/// overrides the Mouse move event handler
		/// <br>if the mousemove is a dragging action on a tracker grip it will enlarge the tracker</br>
		/// 
		/// </summary>
		/// <param name="e">
		/// Mouse event arguments
		/// </param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			
			base.OnMouseMove(e);

            #region FIX2016021401
            if (mCaptureObj != null)
            {
                mCaptureObj.RaiseMouseMove(e);
                return;
            }
            #endregion


            #region FIX2016021201
            if (mLocked)
		    {
		        return;
		    }
            #endregion


			try
			{
				PointF p = new PointF(e.X - this.AutoScrollPosition.X, e.Y - this.AutoScrollPosition.Y);
				//give priority to the widgets, if visible
				if(mShowAutomataController && automataController.Hit(new Rectangle(e.X,e.Y,2,2)))
					automataController.OnMouseMove(p);
				p = UnzoomPoint(Point.Round(p));	
				//marching ants - - - - 
				if (selector != null) 
				{	
					//selector.Update(new PointF(e.X,e.Y));
					selector.Update(p);
					Invalidate();
					return; //all the rest doesnt matter
				}

				//the volatile temporary shape
				if (shapeObject != null)
				{
					shapeObject.Invalidate();            // invalidate previous rendering.
					RectangleF r = shapeObject.Rectangle;
					shapeObject.Rectangle = new RectangleF(p.X, p.Y, r.Width, r.Height);
					shapeObject.Invalidate();            // invalidate next rendering.
				}
				//are we moving a shape around, resizing?			
				if (mDoTrack && !mLocked)
				{	
					this.StopLayout();
					foreach (Shape o in extract.Shapes)
					{
						//ChangedStatus("(" + o.Rectangle.X.ToString() + "," + o.Rectangle.Y.ToString() + ")");
					
						if (o.Tracker != null && o.CanMove && this.mAllowMoveShape && !o.Layer.Locked)
						{

							o.Invalidate();							
							//for the subcontrols
							if(o.Controls != null && o.Controls.Count>0)
							{
								for(int k = 0 ; k<o.Controls.Count;k++)
								{
									//									Control c=(o.Controls[k] as Control);
									//									c.Width=Convert.ToInt32(o.Rectangle.Width)-6;
									//									c.Height=Convert.ToInt32(o.Rectangle.Height)-6;
									//									c.Left=Convert.ToInt32(o.Rectangle.Location.X)+3;
									//									c.Top=Convert.ToInt32(o.Rectangle.Location.Y)+3;

									//o.Controls[k].Location = Point.Round(p);
								}
							}
							o.Tracker.Move(p,this.Size,mSnap,mGridSize); //passing the Size of the canvas allows to keep the shapes inside the canvas!
							o.Invalidate();
							
                            
							//connection tracker
							foreach(Connector c in o.Connectors)
							{
								foreach(Connection cn in c.Connections)
								{
									cn.Invalidate();
									if( cn.Tracker != null )
										(cn.Tracker as ConnectionTracker).MoveAll(p);
									cn.Invalidate();
								}
							}

                            #region FIX2016021501
                            //Pass mouse move event to all selected shape while tracking
                            o.RaiseMouseMove(e);
                            #endregion
                        }
					}
					if( connection!=null && connection.Tracker != null )
					{
						connection.Invalidate();
						connection.Tracker.Move(p,this.Size,this.mSnap,this.mGridSize);						
						connection.Invalidate();
					}
					this.Invalidate(true);
					if(this.mEnableLayout) this.StartLayout();

				}
 
				if (connection != null)
				{
					connection.Invalidate();
					connection.ToPoint = p;
					connection.Invalidate();
					this.Invalidate(true);
				}

				HitHover(p); // set the internal Hover entity to the one hit, if any.

                if ((Hover != null) && (Hover is Entity)) 
                {
                    #region FIX2016021501
                    if ( mDoTrack && Hover is Shape )
                    { 
                        //we already call RaiseMouseMove for shape while tracing
                        //avoid duplicate call for Shape.RaiseMouseMove 
                    }
                    else
                    #endregion
                        (Hover as Entity).RaiseMouseMove(e);						
				//this.ToolTip.SetToolTip(this,(Hover as Entity).Text);
				}
				else
					SetToolTip("");
			
				//this.Invalidate(true);//thanks Lionel Cuir!
				SetCursor(p);				
				Update();
				

			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message, "GraphControl.OnMouseMove");
			}

		}

		/// <summary>
		/// Sets the tooltip of the control
		/// </summary>
		/// <param name="tip"></param>
		public void SetToolTip(string tip)
		{
			this.ToolTip.SetToolTip(this, tip);
		}

		/// <summary>
		/// Paints an arrow
		/// </summary>
		/// <param name="endPoint">the tip of the arrow</param>
		/// <param name="filled">whether to draw a filled arrow</param>
		/// <param name="g">the graphics objects</param>
		/// <param name="lineColor">the color</param>
		/// <param name="showLabel">whether to show the label</param>
		/// <param name="startPoint">the end-point of the arrow</param>
		
		public  void PaintArrow(Graphics g, PointF startPoint, PointF endPoint,Color lineColor, bool filled, bool showLabel)
		{
			try
			{
				g.DrawLine(new Pen(lineColor,1F),startPoint,endPoint);

				SolidBrush brush=new SolidBrush(lineColor);
				double angle = Math.Atan2(endPoint.Y - startPoint.Y,endPoint.X-startPoint.X);
				double length = Math.Sqrt((endPoint.X - startPoint.X)*(endPoint.X - startPoint.X)+(endPoint.Y - startPoint.Y)*(endPoint.Y - startPoint.Y))-10;
				double delta = Math.Atan2(7,length);
				PointF left = new PointF(Convert.ToSingle(startPoint.X + length*Math.Cos(angle-delta)),Convert.ToSingle(startPoint.Y+length*Math.Sin(angle-delta)));
				PointF right = new PointF(Convert.ToSingle(startPoint.X+length*Math.Cos(angle+delta)),Convert.ToSingle(startPoint.Y+length*Math.Sin(angle+delta)));

				PointF[] points={left, endPoint, right};
				if (filled)
					g.FillPolygon(brush,points);
				else
				{
					Pen p=new Pen(brush,1F);
					g.DrawLines(p,points);
				}
				if(showLabel)
				{
					g.DrawString("(" + endPoint.X + "," + endPoint.Y +")",new Font("Arial",10),brush,new PointF(endPoint.X-20,endPoint.Y-20));
				}
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message,"GraphControl.PaintArrow");
			}
				
		}
		/// <summary>
		/// Overrides the paint method and calls the paint method of the extract structure which will loop 
		/// through all the shapes, connections and connectors.
		/// Two other objects need attention here. One is an eventual shape just created and hanging at the cursor.
		/// The other is a connection fixed on a shape on one end but hanging at the cursor on the other (not attached yet).
		/// These two are painted separately.
		/// </summary>
		/// <param name="e">Paint event arguments.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			
			Graphics g =e.Graphics;
			Rectangle b = ZoomRectangle(Rectangle.Round(extract.Rectangle));

            //TODO: Calculate size for MSWord A4 
            AutoScrollMinSize = ExpandWithMSWordPageSize(b.Size, g);
            

            //prepend transformations for zooming

            g.TranslateTransform(this.AutoScrollPosition.X, this.AutoScrollPosition.Y,MatrixOrder.Append);
			g.ScaleTransform(this.currentZoomFactor,this.currentZoomFactor,MatrixOrder.Prepend);			
			
			g.SmoothingMode=SmoothingMode.HighQuality;

		    DrawPageLine(b.Size, g);
		
#if DEBUG
			freeArrows.Paint(g);
#endif

			extract.Paint(g);
			if(selector!=null) 
			{
				g.FillRectangle(selectionBrush,selector.Rectangle);
				g.DrawRectangle(Pens.DimGray,Rectangle.Round(selector.Rectangle));
			}
			if (shapeObject != null) shapeObject.Paint(g);
			if (connection != null) connection.PaintTrack(g);
			
			//Widgets are a feature for next version, this one e.g. is useful when automata are used
			if(mShowAutomataController)	automataController.Paint(g);

		}

        private void DrawPageLine(Size size, Graphics g)
        {
            Size pageSize = PageSize.GetMSWordPage((int)g.DpiX, (int)g.DpiY);
            int pageX = (int)Math.Ceiling((double)(size.Width * 1.0 / pageSize.Width));
            int pageY = (int)Math.Ceiling((double)(size.Height * 1.0 / pageSize.Height));
            int x1, y1, x2, y2;

            //Draw dot line
            Pen pen = new Pen(Color.Black, 1);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
            pen.DashPattern = new float[] { 5, 5 };

            pageX += 2;
            pageY += 2;
            //Draw horizontal lines
            for (int i = 0; i < pageX ; i++)
            {
                x1 = 0;  //left edge
                x2 = pageX * pageSize.Width; //right edge
                y1 = y2 = (int) (pageSize.Height * i);
                g.DrawLine(pen, x1, y1, x2, y2);
            }

            //Draw vertical lines
            for (int j = 0; j < pageY + 2; j++)
            {
                x1 = x2 = (int) (pageSize.Width * j);
                y1 = 0;
                y2 = pageY * pageSize.Height;
                g.DrawLine(pen, x1, y1, x2, y2);
            }
        }

        private Size ExpandWithMSWordPageSize(Size size, Graphics g)
        {
            Size sizeMSWordPage = PageSize.GetMSWordPage((int)g.DpiX, (int)g.DpiY);
            int pageX = (int)Math.Ceiling((double)(size.Width * 1.0 / sizeMSWordPage.Width));
            int pageY = (int)Math.Ceiling((double)(size.Height * 1.0 / sizeMSWordPage.Height));

            return new Size(pageX * sizeMSWordPage.Width, pageY * sizeMSWordPage.Height);
        }

        /// <summary>
        /// Paints the background of the canvas, could have been done in the paint handler as well.
        /// This is not clear in the .Net doc.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
		{
			Graphics g=e.Graphics;			
			
			PaintBackground(g, this.ClientRectangle);
		}

		/// <summary>
		/// Paints the background of the canvas
		/// </summary>
		/// <param name="r">the rectangle to paint on</param>
		/// <param name="g"></param>
		private void PaintBackground(Graphics g, Rectangle r)
		{
			
			switch (this.mBackgroundType)
			{
				case CanvasBackgroundType.Image:
					if (mBackgroundImagePath==null) return;
					if(mBackgroundImagePath.Trim()==string.Empty) return;
					try
					{
						Image im=Image.FromFile(BackgroundImagePath);
						g.DrawImage(im,0,0,Size.Width,Size.Height);
						if(mShowGrid)
						{
								ControlPaint.DrawGrid(g,r,new Size(mGridSize,mGridSize),Color.Wheat);
						}
					}
					catch (System.IO.FileNotFoundException exc)
					{
						this.mBackgroundType = CanvasBackgroundType.FlatColor;
						MessageBox.Show("The path to the background image was not found.","Error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
						Trace.WriteLine(exc.Message,"GraphControl.OnPaintBackground");
					}							
					break;
				case CanvasBackgroundType.FlatColor:
					
					g.SmoothingMode=SmoothingMode.AntiAlias;
					g.FillRectangle(new SolidBrush(mBackgroundColor), r);
					if(mShowGrid)
						ControlPaint.DrawGrid(g,r,new Size(mGridSize,mGridSize),Color.Wheat);
					break;
				case CanvasBackgroundType.Gradient:
					
					g.SmoothingMode=SmoothingMode.AntiAlias;
					//g.FillRectangle(new SolidBrush(Color.White), r);
		
					
					LinearGradientBrush lgb = new LinearGradientBrush(r,this.mGradientTop,this.mGradientBottom,mGradientMode);
					g.FillRectangle(lgb,r);
					if(mShowGrid)
						ControlPaint.DrawGrid(g,r,new Size(mGridSize,mGridSize),Color.Wheat);
					//some text on the canvas
					//g.DrawString("Netron", new Font("Verdana", 100,FontStyle.Bold),new SolidBrush(Color.White),55,55);
					
					break;

			}			
			//the ControlPaint class has interesting stuff, have a look at it!
			//ControlPaint.DrawBorder3D(g,0,0,workingSize.Width,workingSize.Height);

			
		}

		/// <summary>
		/// Loads the assembly of the graphcontrol into the library collection since on deserialization connections are being
		/// deserialized and looked up in the library collection
		/// </summary>
		/// <remarks>
		/// Deserialization related
		/// </remarks>
		private void LoadSelfLibrary()
		{
			string libPath = Path.GetDirectoryName(Application.ExecutablePath) + "\\" + Assembly.GetAssembly(typeof(GraphControl)).GetName().Name + ".dll";
			GraphObjectsLibrary lib = new GraphObjectsLibrary(libPath);
			//add the default connection
			lib.ConnectionSummaries.Add(new ConnectionSummary(libPath,"Default","A0BDE8D6-A95C-45ec-8081-A28E487F38A6","Netron.GraphLib.Connection"));
			ShapeSummary summ;
			#region add the basic shapes

			summ = new ShapeSummary(libPath,"8ED1469D-90B2-43ab-B000-4FF5C682F530","Basic shape","Basic shapes","Netron.GraphLib.BasicShapes.BasicNode","A basic shape with four connectors");
			lib.ShapeSummaries.Add(summ);			
			AddToContextMenu(summ);
			
			summ =new ShapeSummary(libPath,"57AF94BA-4129-45dc-B8FD-F82CA3B4433E","Simple shape","Basic shapes","Netron.GraphLib.BasicShapes.SimpleNode","A basic shape with one connector");
			lib.ShapeSummaries.Add(summ);
			AddToContextMenu(summ);
			
			summ = new ShapeSummary(libPath,"4F878611-3196-4d12-BA36-705F502C8A6B","Text label","Basic shapes","Netron.GraphLib.BasicShapes.TextLabel","A text label");
			lib.ShapeSummaries.Add(summ);
			AddToContextMenu(summ);
			
			Libraries.Add(lib);



			#region the following could be done but it'll loop over hundreds of class and is slowing down the loading of the control
			//ImportEntities(libPath);
			#endregion
			#endregion
		}
		
		#endregion		
		
		#region dragdrop
		/// <summary>
		/// Handles the dragdrop on the canvas
		/// </summary>
		/// <param name="drgevent"></param>
		protected override void OnDragDrop(DragEventArgs drgevent)
		{
			if(!mAllowAddShape || mLocked) return;
			if(AltKey) return;
			Shape sob = null;
			ShapeSummary summary = null;
			base.OnDragDrop (drgevent);
			// Store the data as a string so that it can be accessed from the
			// mnuCopy and mnuMove click events.
			//string sourceData = drgevent.Data.GetData(DataFormats.Text, true).ToString();
			//Shape sob=(Shape)  drgevent.Data.GetData( drgevent.Data.GetFormats()[0]);

			string[] formats = drgevent.Data.GetFormats();
			/* Doesn't work, horrible problem/bug in .Net
			if(drgevent.Data.GetDataPresent("DragImageBits"))
			{
				//Bitmap bmp = null;
				try
				{
					sob  = GetShapeInstance("47D016B9-990A-436c-ADE8-B861714EBE5A");//ImageShape
					if(sob!=null)
					{
						
					
						// The following naive approach doesn't work...
						MemoryStream o = (MemoryStream) drgevent.Data.GetData("DragImageBits");						
						o.Position = 0;
						Image bmp = Image.FromStream(o);						
						PropertyInfo info = sob.GetType().GetProperty("Image");
						info.SetValue(sob, bmp,null);
						
					}
				}
				catch(Exception exc)
				{
					this.OutputInfo(exc.Message, OutputInfoLevels.Exception);
				}
				return;
			}

			*/

			if(drgevent.Data.GetDataPresent("FileName"))
			{
				//Bitmap bmp = null;
				try
				{
					sob  = GetShapeInstance("47D016B9-990A-436c-ADE8-B861714EBE5A");//ImageShape
					if(sob!=null)
					{
						
						string[] fileNames;
						fileNames = (string[])drgevent.Data.GetData(DataFormats.FileDrop);
						string fileName = fileNames[0];						
						Image bmp = Image.FromFile(fileName);						
						PropertyInfo info = sob.GetType().GetProperty("Image");
						info.SetValue(sob, bmp,null);
						
					}
				}
				catch(Exception exc)
				{
					this.OutputInfo(exc.Message, OutputInfoLevels.Exception);
				}
			}
			else
			{
				try
				{
					summary = drgevent.Data.GetData(typeof(ShapeSummary).FullName,false) as ShapeSummary;
				}
				catch
				{
					MessageBox.Show("No valid shape data found to create.","No data error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				}
				try
				{
					sob = GetShapeInstance(summary.Key);
					if(sob==null) throw new Exception();
				}
				catch
				{
					MessageBox.Show("Shape data not found. \n (Did you add the necessary shape mLibraries?)","No data error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				}
			}
			Point currentPt = this.PointToClient( new Point(drgevent.X,drgevent.Y) );

			// Get the mouse drop location 
			PointF p = new PointF(currentPt.X - this.AutoScrollPosition.X, 
				currentPt.Y - this.AutoScrollPosition.Y);
			p = UnzoomPoint(Point.Round(p));
			if(sob != null)	this.AddShape(sob,p);
		}

		/// <summary>
		/// Handles the drag-enter event of the control
		/// </summary>
		/// <param name="drgevent"></param>
		protected override void OnDragEnter(DragEventArgs drgevent)
		{
			base.OnDragEnter (drgevent);
			if(AltKey) drgevent.Effect = DragDropEffects.None;
			if ((drgevent.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move && (drgevent.KeyState & ctrlKey) != ctrlKey)
			{
				// Show the standard Move icon.
				drgevent.Effect = DragDropEffects.Move;
			}
			else
			{
				// Show the standard Copy icon.
				drgevent.Effect = DragDropEffects.Copy;
			}
		}

		/// <summary>
		/// Returns the Summary of the given shape object
		/// </summary>
		/// <param name="sh"></param>
		/// <returns></returns>
		public ShapeSummary GetShapeSummary(Shape sh)
		{
			if(sh==null) return null;
			Type tp = sh.GetType();
			if(!tp.IsClass) return null;	
			object[] objs = null;
			ShapeSummary shapeSum = null;
			try
			{
				objs = tp.GetCustomAttributes(typeof(Netron.GraphLib.Attributes.NetronGraphShapeAttribute),false);
				if(objs.Length>=1)
				{
					NetronGraphShapeAttribute shapeAtts = objs[0] as NetronGraphShapeAttribute;
					shapeSum = new ShapeSummary("", shapeAtts.Key,shapeAtts.Name, shapeAtts.ShapeCategory,shapeAtts.ReflectionName);
					return shapeSum;
				}
			}
			catch
			{
				return null;
			}
			return null;
		}
		#endregion
	
		#region Menu related
		/// <summary>
		/// Handles the NewGraph menu item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnNewGraph(object sender, EventArgs e)
		{
			this.NewDiagram(true);
		}
		/// <summary>
		/// Handles the SelectAll menu item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnSelectAll(object sender, EventArgs e)
		{
			this.SelectAll(true);
		}

		/// <summary>
		/// Handles the Cut menu item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnCut(object sender,EventArgs e)
		{
			return;
		}
		/// <summary>
		/// Handles the Copy menu item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnCopy(object sender,EventArgs e)
		{
			this.Copy();
		}
		/// <summary>
		/// Handles the Paste menu item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnPaste(object sender,EventArgs e)
		{
			this.Paste();;
		}
		/// <summary>
		/// Handles the Properties menu item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void OnProperties(object sender,EventArgs e)
		{
			if(Hover!=null)
				this.RaiseShowProperties(Hover.Properties);
			else
				this.RaiseShowProperties(this.Properties);
		}
		/// <summary>
		/// Common gate for a delete action on the plex. Deletes the selected shapes and goes via the history (of course).
		/// </summary>		
		protected void OnDelete(object sender,EventArgs e)
		{
			try
			{
				if(!mAllowDeleteShape) return;

				ShapeCollection col = this.extract.Shapes.Clone() as ShapeCollection;
				
				foreach(Shape sh in col)
				{
					
					if (sh.IsSelected) 
					{
						if(OnShapeRemoved!=null)
							OnShapeRemoved(this,sh);
						sh.Delete();
					}
					else
						foreach(Connector c in sh.Connectors)
						{
							ConnectionCollection ar = (ConnectionCollection) c.Connections.Clone();
							foreach(Connection n in ar)
								if(n.IsSelected) n.Delete();
						}
					//extract.Delete(();
				}
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message, "GraphControl.OnDelete");
			}
		
			Update();	
		}
		/// <summary>
		/// Adds the given shape summary to the context menu
		/// <seealso cref="Netron.GraphLib.UI.CategoryMenuItem"/>
		/// </summary>
		/// <param name="summary"></param>
		/// 
		protected void AddToContextMenu(ShapeSummary summary)
		{
			MenuItem currentItem =null;
			foreach(MenuItem item in this.mContextMenu.MenuItems)
			{
				if(item.Text == summary.ShapeCategory){ currentItem = item; break;}
			}
			if(currentItem == null)
			{
				this.mContextMenu.MenuItems.Add("-");
				//use the CategoryMenuItem, it has a different CloneMenu implementation
				currentItem =new CategoryMenuItem(summary.ShapeCategory);
				this.mContextMenu.MenuItems.Add(currentItem);
			}
			GraphMenuItem i =new GraphMenuItem(summary,new EventHandler(OnContextMenuItem));			
			
			currentItem.MenuItems.Add(i);


		}		

		/// <summary>
		/// Fires when a contextmenu item is selected
		/// </summary>
		/// <param name="sender">a MenuItem</param>
		/// <param name="e">an EventArgs</param>
		protected void OnContextMenuItem(object sender, EventArgs e)
		{
			if(sender is GraphMenuItem)
			{
				ShapeSummary summary = (sender as GraphMenuItem).Summary;
				Shape shape = this.GetShapeInstance(summary.Key);
				if(shape != null & AllowAddShape)
					this.AddShape(shape);
			}
		}
	
		/// <summary>
		/// Adds the base-menu to the context-menu
		/// </summary>
		protected void AddBaseMenu()
		{
			//mContextMenu.MenuItems.Add("-");//add a separation
			MenuItem graphItem= new MenuItem("Graph");
			mContextMenu.MenuItems.Add(graphItem);
			graphItem.MenuItems.Add("New",new EventHandler(OnNewGraph));
			graphItem.MenuItems.Add("SelectAll",new EventHandler(OnSelectAll));
				
			mContextMenu.MenuItems.Add("-");//add a separation
			//Cut, copy, paste doesn't work cause of a bug in Net, hopefully fixed in Net v2
			/*
			mContextMenu.MenuItems.Add(new MenuItem("C&ut", new EventHandler(OnCut)));
			mContextMenu.MenuItems.Add(new MenuItem("&Copy", new EventHandler(OnCopy)));
			mContextMenu.MenuItems.Add(new MenuItem("&Paste", new EventHandler(OnPaste)));
			*/
			mContextMenu.MenuItems.Add(new MenuItem("&Delete", new EventHandler(OnDelete)));
			mContextMenu.MenuItems.Add("-");//add a separation
			mContextMenu.MenuItems.Add(new MenuItem("P&roperties", new EventHandler(OnProperties)));
		}

		/// <summary>
		/// Resets the menu to its base state.
		/// <see cref="OnMouseDown"/>
		/// </summary>
		protected void ResetToBaseMenu()
		{
			int howmany = mContextMenu.MenuItems.Count;
			for(int k=0; k<howmany; k++)
			{				
				MenuItem copy = mContextMenu.MenuItems[k].CloneMenu();
				this.ContextMenu.MenuItems.Add(copy);
			}
		}
		#endregion
		
		#region Printing
		/// <summary>
		/// Prints the canvas
		/// </summary>
		/// <param name="Sender"></param>
		/// <param name="e"></param>
		public void PrintCanvas(object Sender, PrintPageEventArgs e)
		{
			Graphics g = e.Graphics;
			float w = extract.Rectangle.Width;
			float h = extract.Rectangle.Height;
			float zoom = ( w >= h )? e.MarginBounds.Width / w : e.MarginBounds.Height / h;
			g.ScaleTransform( zoom , zoom  );
			this.extract.Paint(g);
		}
		/// <summary>
		/// Multi-page print of the canvas
		/// Thanks to Fabio...
		/// </summary>
		/// <param name="Sender"></param>
		/// <param name="e"></param>
		/// <param name="n"></param>
		/// <param name="m"></param>
		public void PrintCanvas(object Sender, PrintPageEventArgs e, ref int n, ref int m)
		{
			//Check input values
			if ((n < 0) | (m < 0))
			{
				throw new Exception("PrintCanvas() only accepts non negative page coordinates.");
			}

			e.Graphics.PageUnit = GraphicsUnit.Inch;
			e.Graphics.PageScale = 0.01F;

			float w = extract.Rectangle.Width;
			float h = extract.Rectangle.Height;

			//Verify whether the requested page falls inside the extract
			if (((((n + 1) * e.MarginBounds.Width) - w) > e.MarginBounds.Width) || 
				((((m + 1) * e.MarginBounds.Height) - h) > e.MarginBounds.Height))
			{
				m = n = 0;//The page requested falls outside the extract
				return;   //Return (0, 0) to signal that there's nothing to draw at such coordinates.
			}

			//Define the clipping rectangle according to the requested (n, m) page
			RectangleF ClippingRect = new RectangleF(n * e.MarginBounds.Width, 
				m * e.MarginBounds.Height, 
				e.MarginBounds.Width,
				e.MarginBounds.Height);

			//Translate the graphics in X and Y directions by the proper number of pages
			e.Graphics.TranslateTransform(e.MarginBounds.Left - n * e.MarginBounds.Width,
				e.MarginBounds.Top  - m * e.MarginBounds.Height);
	
			//Set the clipping rectangle to paint the clipped area of the canvas (one particular page)
			e.Graphics.SetClip(ClippingRect,CombineMode.Replace);

			
			this.extract.Paint(e.Graphics);

			//Calculate whether a new page will follow the current one and in case return next page coordinate
			if ((((n + 2) * e.MarginBounds.Width) - w) <= e.MarginBounds.Width)
			{
				n++; //There's a another canvas part to be trawn on the right side
			}
			else if ((((m + 2) * e.MarginBounds.Height) - h) <= e.MarginBounds.Height)
			{
				n = 0; //There's a another canvas part to be drawn on the leftmost part but one page height below the current one
				m++;
			}
			else
			{
				n = m = 0; //There's really nothing more to be drawn. You've to end up calling this method for this canvas.
			}
			
			return;
		}

		
		#endregion

		#region Automata related
		/// <summary>
		/// This starts the heart beat of the automata data flow, it simply calls the start method of the timer.
		/// </summary>
		public void StartAutomata()
		{
			if(OnStartAutomata != null) OnStartAutomata(this, EventArgs.Empty);
			transmissionTimer.Start();
			mIsAutomataRunning = true;
		}
		/// <summary>
		/// This stops the heart beat of the automata data flow.
		/// </summary>
		public void StopAutomata()
		{
			if(OnStopAutomata != null) OnStopAutomata(this, EventArgs.Empty);
			transmissionTimer.Stop();
			mIsAutomataRunning = false;
		}

		/// <summary>
		/// Resets the automata
		/// </summary>
		public void ResetAutomata()
		{
			foreach(Shape so in this.extract.Shapes)
			{
				so.InitAutomata();
			}
		}

		/// <summary>
		/// This is the event handler when the transmission timer fires. Handles the transmissions of data over the connections.
		/// 
		/// </summary>
		/// <param name="Sender"></param>
		/// <param name="e"></param>
		public void OnTransmissionTimer(object Sender, EventArgs e)
		{
			if(OnDataTransmission !=null) OnDataTransmission(this, EventArgs.Empty);
			extract.Transmit();
			extract.Update();
			
			Invalidate();		
			Application.DoEvents();
		}
		
		#endregion

		#region Layout related
		/// <summary>
		/// Starts the graph layout thread using a previously defined algorithm
		/// </summary>
		public void StartLayout()
		{
			
			if(thLayout !=null) thLayout.Abort();
			ts=null;
			try
			{
				
				ts=new ThreadStart(layoutFactory.GetRunable());
				thLayout = new Thread(ts);
				thLayout.Start();				
				Refresh();
			}
			catch
			{
				MessageBox.Show("Unable to launch the graph layout process","Graph layout exception",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
			}

		}
		/// <summary>
		/// Stops the graph layout thread 
		/// </summary>
		public void StopLayout()
		{
			if (thLayout !=null)
				if (thLayout.IsAlive) this.thLayout.Abort();
		}
		
		#endregion

		#region Input/output

		#region NML
		/// <summary>
		/// Saves the graph to NML
		/// </summary>
		/// <param name="filePath"></param>
		public bool SaveNMLAs(string filePath)
		{
			bool ret = false;
			RaiseOnSavingDiagram(filePath);

			ret =IO.NML.NMLSerializer.SaveAs(filePath, this);	

			
			 
			if(ret)
			{
				OutputInfo("The diagram was saved in NML format to '" + mFileName + "'" ,OutputInfoLevels.Info);
				RaiseOnDiagramSaved(filePath);
			}
			else
				OutputInfo("The diagram was not saved.",OutputInfoLevels.Info);
			return ret;
		}

		/// <summary>
		/// Fetches the NML of the current diagram
		/// </summary>
		/// <returns></returns>
		public string GetNML()
		{
			IO.NML.NMLSerializer ser = new IO.NML.NMLSerializer(this);
			return ser.Serialize();
		}

		/// <summary>
		/// Opens a NML file
		/// </summary>
		/// <param name="fileName">the name of the file from which to open the NML</param>
		public void OpenNML(string fileName)
		{
			//raise before opening
			RaiseOnOpeningDiagram(fileName);

			IO.NML.NMLSerializer.Open(fileName,this);
			this.mFileName = mFileName;

			//raise after opening
			RaiseOnDiagramOpened(fileName);

			//notify the outside world
			OutputInfo("The diagram was deserialized from XML from the file '" + mFileName + "'" ,OutputInfoLevels.Info);
		}

		/// <summary>
		/// Loads the given NML-string in the control		
		/// </summary>
		/// <param name="nml"></param>
		public void OpenNMLFragment(string nml)
		{
			IO.NML.NMLSerializer ser = new IO.NML.NMLSerializer(this);
			
			this.extract = ser.Deserialize(nml) as GraphAbstract;
		}
		#endregion

		/// <summary>
		/// GDI32 imported function not available in the framework,
		/// used here to save a picture of the turtle world.
		/// Can also be used, in general, to take a snapshot of a (actually) any) control.
		/// </summary>
		/// <param name="hdcDest"></param>
		/// <param name="nXDest"></param>
		/// <param name="nYDest"></param>
		/// <param name="nWidth"></param>
		/// <param name="nHeight"></param>
		/// <param name="hdcSrc"></param>
		/// <param name="nXSrc"></param>
		/// <param name="nYSrc"></param>
		/// <param name="dwRop"></param>
		/// <returns></returns>
		[DllImport("gdi32.dll")]
		private static extern bool BitBlt(

			IntPtr hdcDest, // handle to destination DC

			int nXDest, // x-coord of destination upper-left corner

			int nYDest, // y-coord of destination upper-left corner

			int nWidth, // width of destination rectangle

			int nHeight, // height of destination rectangle

			IntPtr hdcSrc, // handle to source DC

			int nXSrc, // x-coordinate of source upper-left corner

			int nYSrc, // y-coordinate of source upper-left corner

			System.Int32 dwRop // raster operation code

			);
		/// <summary>
		/// Saves an image of the canvas in JPG format.
		/// </summary>
		/// <param name="path"></param>
		public void SaveImage2(string path)
		{
			//not a simple thing to do, seemingly you need interop to do it....
			Graphics g1 = this.CreateGraphics();
			Bitmap bm=new Bitmap(this.Width,this.Height,g1);
			
			Bitmap backBM=new Bitmap(this.Width,this.Height);
			Graphics g2 = Graphics.FromImage(backBM);

			System.IntPtr dc1=g1.GetHdc();

			System.IntPtr dc2=g2.GetHdc();

			BitBlt(dc2,0,0,this.Size.Width,this.Size.Height,dc1,0,0,0x00CC0020);

			g1.ReleaseHdc(dc1);

			g2.ReleaseHdc(dc2);

			g1.Dispose();

			g2.Dispose();
			backBM.Save(path, ImageFormat.Jpeg);

		}

		/// <summary>
		///  Saves an image of the canvas in JPG format.
		/// </summary>
		/// <param name="path"></param>
		///<param name="drawBackground">wether the background should be painted as well</param>
		public void SaveImage(string path, bool drawBackground)
		{
			Bitmap bmp=new Bitmap(this.AutoScrollMinSize.Width+20,this.AutoScrollMinSize.Height+20,this.CreateGraphics());
			using(Graphics g = Graphics.FromImage(bmp))
			{
				g.SmoothingMode = SmoothingMode.AntiAlias;				
				
				//g.FillRectangle(new SolidBrush(this.BackgroundColor), 0, 0, this.AutoScrollMinSize.Width+20,this.AutoScrollMinSize.Height+20);
				if(drawBackground)
					PaintBackground(g, new Rectangle(0,0,this.AutoScrollMinSize.Width+20,this.AutoScrollMinSize.Height+20));
				extract.PaintExternal(g);
			}

			EncoderParameters eps = new EncoderParameters(1);
			int quality = 100;
			eps.Param[0] = new EncoderParameter(Encoder.Quality,quality);
			ImageCodecInfo ici = ImageCodecInfo.GetImageEncoders()[1]; //MimeType: image/jpeg
			//TODO: is this always index=1???
			bmp.Save(path, ici, eps);		
			return;
		}
	
		/// <summary>
		/// Returns a thumbnail of the diagram
		/// </summary>
		/// <param name="height">the height of the thumbnail</param>
		/// <param name="width">the width of the thumbnail</param>
		/// <returns></returns>
		[Obsolete("This version uses the BitBlt DllImport and is deprecated",false)]
		public Image GetControlThumbnail1(int width, int height)
		{
			Graphics g1 = this.CreateGraphics();
			Bitmap bm=new Bitmap(this.Width,this.Height,g1);
			
			Bitmap backBM=new Bitmap(this.Width,this.Height);
			Graphics g2 = Graphics.FromImage(backBM);

			System.IntPtr dc1=g1.GetHdc();

			System.IntPtr dc2=g2.GetHdc();

			BitBlt(dc2,0,0,this.Size.Width,this.Size.Height,dc1,0,0,0x00CC0020);

			g1.ReleaseHdc(dc1);

			g2.ReleaseHdc(dc2);

			g1.Dispose();

			g2.Dispose();
			Image.GetThumbnailImageAbort tCallback  = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);
			return backBM.GetThumbnailImage(width,height,tCallback,IntPtr.Zero);

		}

		/// <summary>
		/// Returns a thumbnail of the diagram
		/// </summary>
		/// <param name="height">the height of the thumbnail</param>
		/// <param name="width">the width of the thumbnail</param>
		/// <returns></returns>
		public Image GetControlThumbnail(int width, int height)
		{
			Bitmap bmp=new Bitmap(this.Width,this.Height,this.CreateGraphics());
			using(Graphics g = Graphics.FromImage(bmp))
			{
				extract.Paint(g);
			}
			Image.GetThumbnailImageAbort tCallback  = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);
			return bmp.GetThumbnailImage(width,height,tCallback,IntPtr.Zero);
			
		}
		/// <summary>
		/// Returns a bitmap of the whole diagram.
		/// 
		/// </summary>
		/// <remarks>The image returns also the non-visible portion of the canvas if the scrollbars are visible</remarks>
		/// <returns></returns>
		public Image GetDiagramImage()
		{
//            Bitmap bmp = new Bitmap(this.Width, this.Height, this.CreateGraphics());
            Bitmap bmp = new Bitmap((int)Abstract.Rectangle.Width + 2, (int)Abstract.Rectangle.Height + 2, this.CreateGraphics());
            using (Graphics g = Graphics.FromImage(bmp))
			{
				extract.Paint(g);
			}
			return bmp;
		}
		/// <summary>
		/// Require callback method by the thumbnail method
		/// </summary>
		/// <returns></returns>
		private bool ThumbnailCallback()
		{
			return false;
		}

		/// <summary>
		/// Loads the libraries from the application configuration
		/// </summary>
		public void LoadLibraries()
		{
			ArrayList graphLibs = ConfigurationSettings.GetConfig("GraphLibs") as ArrayList;			
			if(graphLibs==null) return ;
			if(graphLibs.Count>0)
			{
				for(int k=0; k<graphLibs.Count;k++)
				{
					this.ImportEntities(graphLibs[k] as string);
				}
			}
			

			

			
		}

		/// <summary>
		/// Adds a shape library to the collection
		/// </summary>
		/// <param name="path"></param>
		public void AddLibrary(string path)
		{
			this.ImportEntities(path);
		}

		/// <summary>
		/// Imports/reflects the entities contained in the assembly, if any
		/// </summary>
		/// <param name="path"></param>
		protected void ImportEntities(string path)
		{
			GraphObjectsLibrary library = new GraphObjectsLibrary();
			ShapeSummary shapeSum;
			ConnectionSummary conSum;
			library.Path = path;
			mLibraries.Add(library);
			
			try
			{	
				Directory.SetCurrentDirectory(Path.GetDirectoryName(Application.ExecutablePath));
				Assembly ass=Assembly.LoadFrom(path);
				if (ass==null) return;
				Type[] tps=ass.GetTypes();
			
				if (tps==null) return ;
				
				object[] objs;
				for(int k=0; k<tps.Length;k++) //loop over modules in assembly
				{
					
					if(!tps[k].IsClass) continue;	
					
					try
					{
						objs = tps[k].GetCustomAttributes(typeof(Netron.GraphLib.Attributes.NetronGraphShapeAttribute),false);
						if(objs.Length>=1)
						{
							
							//now, we are sure to have a shape object					
					
							NetronGraphShapeAttribute shapeAtts = objs[0] as NetronGraphShapeAttribute;
							shapeSum = new ShapeSummary(path, shapeAtts.Key,shapeAtts.Name, shapeAtts.ShapeCategory,shapeAtts.ReflectionName);
							
							library.ShapeSummaries.Add(shapeSum);
							if(!shapeSum.IsInternal) AddToContextMenu(shapeSum);							
							continue;//cannot be a connection and a shape at the same time
						}
						
						//try a custom connection

						objs = tps[k].GetCustomAttributes(typeof(Netron.GraphLib.Attributes.NetronGraphConnectionAttribute),false);
						if(objs.Length>=1) 						
						{
							//now, we are sure to have a connection object					
					
							NetronGraphConnectionAttribute conAtts = objs[0] as NetronGraphConnectionAttribute;
							conSum = new ConnectionSummary(path, conAtts.Name, conAtts.Key, conAtts.ReflectionName);
							library.ConnectionSummaries.Add(conSum);	
							continue;					
						}					
					}
					catch(Exception exc)
					{
						Trace.WriteLine(exc.Message);
						continue;
					}
							
				}
				
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message, "GraphControl.ImportEntities");
			}
		}

		/// <summary>
		/// Saves the plex to the selected path using the wonderful .Net serialization.
		/// </summary>
		/// <remarks><br>Would it be possible to save all this to a database?</br>
		/// <br>You can also use the XML or SOAP serialization but note that only public methods and properties will be serialized, not sure the plex will be allright on deserialization.</br></remarks>
		/// <param name="mFileName">The path to which to save the data.</param>
		public bool SaveAs(string mFileName)
		{
			bool ret= false;

			#region Bypassing problems with the current binary serialization
			
//			InfoDelegate d1 = null;
//			PropertiesInfo d2 = null;
//			ConnectionInfo d3 = null;
//			ShapeInfo d4 = null;
//			ShapeInfo d5 = null;
//			EventHandler d6 = null;
//			MouseEventHandler d7 = null;
//			EventHandler d8 = null;
//			EventHandler d9 = null;
//			EventHandler d10 = null;
//			InfoDelegate d11 = null;
//
//			if(this.OnInfo!=null)
//			{
//				d1 = OnInfo;
//				OnInfo -= d1;
//			}
//			if(this.OnShowProperties!=null)
//			{
//				d2 = this.OnShowProperties;
//				OnShowProperties-=	d2;
//			}
//			if(this.OnConnectionAdded!=null)
//			{
//				d3 = this.OnConnectionAdded;
//				OnConnectionAdded -= d3;
//			}
//			if(this.OnShapeAdded!=null)
//			{
//				d4 = this.OnShapeAdded;
//				OnShapeAdded-= d4;
//			}
//
//			if(this.OnShapeRemoved!=null)
//			{
//				d5 = this.OnShapeRemoved;
//				OnShapeRemoved -= d5;
//			}
//			if(this.OnClear!=null)
//			{
//				d6 = this.OnClear;
//				OnClear -= d6;
//			}
//			if(this.OnContextMenu !=null)
//			{
//				d7 = this.OnContextMenu;
//				OnContextMenu -= d7;
//			}
//			if(this.OnStartAutomata !=null)
//			{
//				d8 = this.OnStartAutomata;
//				OnStartAutomata -= d8;
//			}
//			if(this.OnStopAutomata !=null)
//			{
//				d9 = this.OnStopAutomata;
//				OnStopAutomata -= d9;
//			}
//			if(this.OnDataTransmission !=null)
//			{
//				d10 = this.OnDataTransmission;
//				OnDataTransmission -= d10;
//			}
//			if(this.OnOpeningDiagram !=null)
//			{
//				d11 = this.OnOpeningDiagram;
//				OnOpeningDiagram -= d11;
//			}
//
//			this.mFileName = mFileName;
//
//			if(d1!=null) OnInfo += d1;
//			if(d2!=null) OnShowProperties += d2;
//			if(d3!=null) OnConnectionAdded += d3;
//			if(d4!=null) OnShapeAdded += d4;
//			if(d5!=null) OnShapeRemoved += d5;
//			if(d6!=null) OnClear += d6;
//			if(d7!=null) OnContextMenu += d7;
//			if(d8!=null) OnStartAutomata += d8;
//			if(d9!=null) OnStopAutomata += d9;
			#endregion

			RaiseOnSavingDiagram(mFileName);

			ret = IO.Binary.BinarySerializer.SaveAs(mFileName,this);
			if(ret)
			{
				OutputInfo("The diagram was saved in binary format to '" + mFileName + "'" ,OutputInfoLevels.Info);
				RaiseOnDiagramSaved(mFileName);
			}
			else
				OutputInfo("The diagram was not saved.",OutputInfoLevels.Info);
			return ret;
		}
		/// <summary>
		/// Opens the specified file and deserializes it. In essence the extract is being filled with the saved data and the next OnPaint event simply paints its contents.
		/// </summary>
		/// <param name="mFileName"></param>
		public void Open(string mFileName)
		{
			this.NewDiagram(true);//makes it possible for the host to ask for saving first
			RaiseOnOpeningDiagram(mFileName);//do it before opening the new diagram
			IO.Binary.BinarySerializer.Open(mFileName,this);
			this.mFileName = mFileName;
			RaiseOnDiagramOpened(mFileName);
			//notify the outside world
			OutputInfo("The binary file '" + mFileName + "' was opened.", OutputInfoLevels.Info);
			
		}

		



		#region OutputInfo overrides
		/// <summary>
		/// Outputs info to the outside world where it can be displayed or
		/// logged. The info gets the 'None' information level.
		/// </summary>
		/// <param name="obj">Any data</param>
		public void OutputInfo(object obj)
		{
			if(OnInfo !=null)
				OnInfo(obj, OutputInfoLevels.None);
		}
		/// <summary>
		/// Outputs info to the outside world where it can be displayed or 
		/// logged with the specified level.
		/// </summary>
		/// <param name="obj">Any data</param>
		/// <param name="level">The informative level</param>
		public void OutputInfo(object obj, OutputInfoLevels level)
		{
			if(OnInfo !=null)
				OnInfo(obj, level);
			Trace.WriteLine(obj.ToString(), level.ToString());
		}

		/// <summary>
		/// Outputs info to the outside world where it can be displayed or 
		/// logged with the specified level.
		/// </summary>
		/// <param name="obj">Any data</param>
		/// <param name="level">The informative level</param>
		/// <param name="category">the category of the information</param>
		public void OutputInfo(object obj, string category, OutputInfoLevels level)
		{
			if(OnInfo !=null)
				OnInfo(obj, level);
			Trace.WriteLine(obj.ToString(), level.ToString() + " in " + category );
		}


		#endregion

		#region Raisers
		/// <summary>
		/// Raises the ShowProps event
		/// </summary>
		/// <param name="props"></param>
		public void RaiseShowProperties(PropertyBag props)
		{
			if(OnShowProperties !=null) 				
				OnShowProperties(this,new object[]{props});
			
		}

		/// <summary>
		/// Raises the ShowProps event
		/// </summary>
		/// <param name="props">array of propertybags</param>
		public void RaiseShowProperties(PropertyBag[] props)
		{
			if(OnShowProperties !=null) 				
				OnShowProperties(this,props);
			
		}

		/// <summary>
		/// Raises the ShowProps event
		/// </summary>
		/// <param name="props">array of propertybags</param>
		public void RaiseShowProperties(object[] props)
		{
			if(OnShowProperties !=null) 				
				OnShowProperties(this,props);
			
		}

		/// <summary>
		/// Raises the OnShapeAdded event
		/// </summary>
		/// <param name="shape"></param>
		public void RaiseOnShapeAdded(Shape shape)
		{
			if(OnShapeAdded!=null)
				OnShapeAdded(this,shape);
		}
		/// <summary>
		/// Raises the OnShowPropertiesDialogRequest event
		/// </summary>
		public void RaiseOnShowPropertiesDialogRequest()
		{
			if(OnShowPropertiesDialogRequest!=null)
				OnShowPropertiesDialogRequest(this, OutputInfoLevels.Info);
		}
		/// <summary>
		/// Raises the OnShowGraphLayers event
		/// </summary>
		public void RaiseOnShowGraphLayers()
		{
			if(OnShowGraphLayers!=null)
				OnShowGraphLayers(this, OutputInfoLevels.Info);
		}
		/// <summary>
		/// Raises the OnConnectionAdded event
		/// </summary>
		/// <param name="con"></param>
		/// <param name="manual">true is the addition was done via the user interface,
		/// false if created programmatically</param>
		public void RaiseOnConnectionAdded(Connection con, bool manual)
		{
			if(OnConnectionAdded!=null)
				OnConnectionAdded(this, new ConnectionEventArgs(con,manual));
		}

		/// <summary>
		/// Raises the OnClear event
		/// </summary>
		public void RaiseOnClear()
		{
			if(OnClear!=null)
				OnClear(this,EventArgs.Empty);
		}


		#region Open/save
		/// <summary>
		/// Raises the OnSavingDiagram event
		/// </summary>
		/// <param name="filePath"></param>
		public void RaiseOnSavingDiagram(string filePath)
		{
			if(OnSavingDiagram!=null)
				OnSavingDiagram(this, new System.IO.FileInfo(filePath));
		}

		/// <summary>
		/// Raises the OnDiagramSaved event
		/// </summary>
		/// <param name="filePath"></param>
		public void RaiseOnDiagramSaved(string filePath)
		{
			if(OnDiagramSaved!=null)
				OnDiagramSaved(this,  new System.IO.FileInfo(filePath));
		}
		
		/// <summary>
		/// Raises the OnOpeningDiagram event
		/// </summary>
		public void RaiseOnOpeningDiagram(string filePath)
		{
			if(OnOpeningDiagram!=null)
				OnOpeningDiagram(this,  new System.IO.FileInfo(filePath));
		}
		/// <summary>
		/// Raises the OnDiagramOpened event
		/// </summary>
		public void RaiseOnDiagramOpened(string filePath)
		{
			if(OnDiagramOpened!=null)
				OnDiagramOpened(this,  new System.IO.FileInfo(filePath));
		}

		#endregion

		#endregion
		/// <summary>
		/// Handles the key press
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnKeyPress(object sender, KeyPressEventArgs e)
		{
			if(mLocked) return;
			//pass the event down to the shapes
			for(int k=0; k<this.extract.Shapes.Count; k++)
			{
				extract.Shapes[k].OnKeyPress(e);
			}
		}
		/// <summary>
		/// This is the general event handler for key events
		/// </summary>
		/// <param name="e">A KeyEventArgs object</param>
		/// <param name="sender">the sender of the event</param>
		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if(mLocked) return;

			#region Detection of modifiers
			//CTRL-SHIFT
			if(e.KeyCode==Keys.ShiftKey && e.Control) 
			{
				CtrlShift = true;
			}
			//ALT
			if(e.KeyCode==Keys.ShiftKey && e.Alt)
			{
				AltKey = true;
			}

			#endregion

			#region pass the event down to the shapes
			for(int k=0; k<this.extract.Shapes.Count; k++)
			{
				extract.Shapes[k].OnKeyDown(e);
			}
			#endregion

			#region ESCAPE
			if (Keys.Escape==e.KeyData)
			{
				mDoTrack = false;
                foreach (Shape o in this.extract.Shapes)
				{
					o.IsSelected=false;
				}				
				return;
			}
            #endregion

            #region Copy & Paste
            if (e.KeyCode == Keys.C && e.Control)
            {
                this.Copy();
                return;
            }

            if (e.KeyCode == Keys.V && e.Control)
            {
                this.Paste();
                return;
            }
            #endregion


            #region CTRL-L
            if (e.KeyCode==Keys.L && e.Control)	
			{
				this.StartLayout();
				return;
			}
			#endregion

			#region CTRL-A
			if (e.KeyCode==Keys.A && e.Control)	
			{
				this.SelectAll(true);
				return;
			}
			#endregion

			#region CTRL-S
			if (e.KeyCode==Keys.S && e.Control) 
			{
				if (this.mFileName!=null) this.SaveAs(this.mFileName);
				return;
			}

			#endregion

			#region DELETE
			if(e.KeyCode==Keys.Delete)
			{
				this.OnDelete(this,EventArgs.Empty);
				return;
			}
			#endregion
			
			#region Arrows keys

			#region LEFT
			if(e.KeyCode==Keys.Left && e.Control) 
			{
				foreach (Shape o in extract.Shapes)
				{
					if(!o.IsSelected || !o.CanMove) continue;
					o.Invalidate();					
					if(mSnap)
					{
						o.X-=mGridSize;
						o.X=o.X-o.X%mGridSize;						
					}
					else
					{
						o.X-=1;
					}
					if(this.mRestrictToCanvas)	o.X = Math.Max(o.X,2);
					o.Tracker.Rectangle=o.Rectangle;
					o.Invalidate();
				}
				this.Invalidate(true);
				return;
			}
			#endregion

			#region RIGHT
			if(e.KeyCode==Keys.Right && e.Control) 
			{
				foreach (Shape o in extract.Shapes)
				{
					if(!o.IsSelected || !o.CanMove) continue;
					o.Invalidate();
					if(mSnap)
					{
						o.X+= mGridSize;						
						o.X=o.X-o.X%mGridSize;
					}
					else
					{
						o.X+=1;					
					}
					if(mRestrictToCanvas) o.X = Math.Min(o.X, this.Width-o.Width-2);
					o.Tracker.Rectangle=o.Rectangle;
					o.Invalidate();
				}
				this.Invalidate(true);
				return;
			}
			#endregion

			#region UP
			if(e.KeyCode==Keys.Up && e.Control) 
			{
				foreach (Shape o in extract.Shapes)
				{
					if(!o.IsSelected || !o.CanMove) continue;
					o.Invalidate();
					if(mSnap)
					{
						o.Y-=mGridSize;						
						o.Y=o.Y-o.Y%mGridSize;
					}
					else
					{						
						o.Y-=1;						
					}
					if(mRestrictToCanvas) o.Y = Math.Max(o.Y, 2);
					o.Tracker.Rectangle=o.Rectangle;
					o.Invalidate();
				}
				this.Invalidate(true);
				return;
			}
			#endregion

			#region DOWN
			if(e.KeyCode==Keys.Down && e.Control) 
			{
				foreach (Shape o in extract.Shapes)
				{
					if(!o.IsSelected || !o.CanMove) continue;
					o.Invalidate();
					if(mSnap)
					{
						o.Y+=mGridSize;
						o.Y=o.Y-o.Y%mGridSize;
					}
					else
					{
						o.Y+=1;
					}
					if(mRestrictToCanvas) o.Y = Math.Min(o.Y, this.Height-o.Height-2);
					o.Tracker.Rectangle=o.Rectangle;
					o.Invalidate();
				}
				this.Invalidate(true);
				return;
			}
			#endregion

			#endregion

			//the rest of the key handlers will require the pouse-position
			PointF p =this.PointToClient(MousePosition);

			#region CTRL-B: insert a basic node
			if(e.KeyCode==Keys.B && e.Control && !mLocked) 
			{
				AddBasicShape(BasicShapeType.BasicNode,"Basic shape",p);
				return;
			}
			#endregion

			#region CTRL-Y: insert a simple node
			if(e.KeyCode==Keys.Y && e.Control  && !mLocked) 
			{
				AddBasicShape(BasicShapeType.SimpleNode,"Simple shape",p);
			}
			#endregion

			#region CTRL-J: insert a text label
			if(e.KeyCode==Keys.J && e.Control  && !mLocked) 
			{
				AddBasicShape(BasicShapeType.TextLabel,"New text",p);
			}			
			#endregion

			//SetCursor(p);
		}

		/// <summary>
		/// Moves the scrollbar up-down.
		/// You can override this and let it zoom instead
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel (e);

			this.AutoScrollPosition.Offset(5*e.Delta,5*e.Delta);
			this.Invalidate();

		}

		#endregion

		#region Graph operations

		#region various overloads of AddShape
		/// <summary>
		/// Adds a given shape to the canvas at the specified position
		/// </summary>
		/// <param name="sob">a shape object</param>
		/// <param name="position">the position at twhich the shape has to be placed</param>
		/// <returns>the added shape object or null if unsuccessful</returns>
		protected Shape AddShape(Shape sob, PointF position)
		{
			try
			{
				sob.Site=this;
				//sob.Invalidate();
				RectangleF r = sob.Rectangle;
				
				sob.Rectangle = new RectangleF(position.X, position.Y, r.Width, r.Height);

				sob.Invalidate();

				extract.Insert(sob);		
			
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message,"GraphControl.AddShape");
				sob = null;
			}
			return sob ;
		}

		/// <summary>
		/// Adds a shape to the canvas
		/// </summary>
		/// <param name="key">the instantion identifier</param>
		/// <param name="position">the location where the shape will be created</param>
		/// <returns></returns>
		public Shape AddShape(string  key, PointF position)
		{
			
			Shape sob = GetShapeInstance(key);
			return AddShape(sob,position);
		}

		/// <summary>
		/// Removes a shape from the graph
		/// </summary>
		/// <param name="shape"></param>
		public void RemoveShape(Shape shape)
		{
			if(OnShapeRemoved!=null)
				OnShapeRemoved(this,shape);
		}

		/// <summary>
		/// Adds a given shape to the canvas at the mouse position
		/// </summary>
		/// <param name="sob"></param>
		/// <returns></returns>
		public Shape AddShape(Shape sob)
		{
			return this.AddShape(sob, Center);
		}
		#endregion

		#region Add node methods
		
		/// <summary>
		/// Adds a 'basic' shape to the canvas
		/// <seealso cref="Netron.GraphLib.BasicShapeType"/>
		/// </summary>
		/// <returns></returns>
		public Shape AddBasicShape()
		{
			return AddBasicShape(BasicShapeType.SimpleNode,"[Not set]", Center);						
		}
		/// <summary>
		/// Adds a 'basic' shape to the canvas
		/// <seealso cref="Netron.GraphLib.BasicShapeType"/>
		/// </summary>
		/// <param name="label"></param>
		/// <returns></returns>
		public Shape AddBasicShape(string label)
		{
			return AddBasicShape(BasicShapeType.SimpleNode,label, Center);						
		}
		/// <summary>
		/// Adds a 'basic' shape to the canvas
		/// <seealso cref="Netron.GraphLib.BasicShapeType"/>
		/// </summary>
		/// <param name="label"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		public Shape AddBasicShape(string label ,PointF position)
		{
			return AddBasicShape(BasicShapeType.SimpleNode, label,position);			
		}
		/// <summary>
		/// Adds a 'basic' shape to the canvas
		/// <seealso cref="Netron.GraphLib.BasicShapeType"/>
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public Shape AddBasicShape(PointF position)
		{
			return AddBasicShape(BasicShapeType.SimpleNode,"[Not set]",position);			
		}
		/// <summary>
		/// Adds a 'basic' shape to the canvas
		/// <seealso cref="Netron.GraphLib.BasicShapeType"/>
		/// </summary>
		/// <param name="shapeType"></param>
		/// <param name="nodeLabel"></param>
		/// <param name="position"></param>
		/// <returns></returns>
		public Shape AddBasicShape(BasicShapeType shapeType, string nodeLabel, PointF position)
		{
			
			string key =string.Empty;
			Shape sob = null;
			switch(shapeType)
			{
				case BasicShapeType.BasicNode: key = "8ED1469D-90B2-43ab-B000-4FF5C682F530"; break;
				case BasicShapeType.SimpleNode: key = "57AF94BA-4129-45dc-B8FD-F82CA3B4433E"; break;
				case BasicShapeType.TextLabel: key = "4F878611-3196-4d12-BA36-705F502C8A6B"; break;
			}			
			
			sob= GetShapeInstance(key);		
			if(sob != null)	
			{
				this.AddShape(sob,position);
				sob.Text=nodeLabel;		

				
			}
			
			return sob;
			
			//			shapeObject = new BasicNode(this);
			//
			//			if (shapeObject != null)
			//			{
			//				shapeObject.Invalidate();
			//				RectangleF r = shapeObject.Rectangle;
			//				
			//					shapeObject.Rectangle = new RectangleF(position.X, position.Y, r.Width, r.Height);
			//
			//				shapeObject.Invalidate();
			//
			//				extract.Insert(shapeObject);
			//				((BasicNode) shapeObject).ShowProps +=new PropertiesInfo(CanvasControl_ShowProps);
			//				if (NodeLabel==null)
			//				{
			//					((BasicNode) shapeObject).Label="Node " + BasicNodeCounter.ToString();
			//					BasicNodeCounter++; //just a way to label newly created nodes	
			//				}
			//				else
			//					((BasicNode) shapeObject).Label=NodeLabel;
			//				
			//				shapeObject = null;
			//					
			//			}
		}


		
		

		#endregion

	

		/// <summary>
		/// Returns whether for the given two connectors there is a connection
		/// </summary>
		/// <param name="b"></param>
		/// <param name="e"></param>
		/// <returns></returns>
		protected bool ConnectionExists(Connector b,Connector e)
		{

			foreach(Shape so in extract.Shapes)
				foreach(Connector c in so.Connectors)
					foreach(Connection n in c.Connections)
						if ((n.From.Equals(b) && n.To.Equals(e) || (n.From.Equals(e) && n.To.Equals(b) ))) return true;
		
			return false;
		}

		/// <summary>
		/// Returns the entity with the specified UID
		/// </summary>
		/// <param name="UID">The unique identitfier specifying the entity</param>
		/// <returns></returns>
		public Entity GetEntity(string UID)
		{
			foreach(Shape en in this.extract.Shapes)
			{
				if(en.UID.ToString()==UID) return en;
				foreach(Connector c in  en.Connectors)
				{
					if(c.UID.ToString()==UID) return c;	
					foreach(Connection n in c.Connections)
						if (n.UID.ToString()==UID) return n;
				}
			}
			return null;
		}
		/// <summary>
		/// Implements the copy function to the clipboard
		/// </summary>
		/// <remarks>
		/// This doesn't work, bug in Clipboard object, hopefully fixed in Net v2 
		///</remarks>
		//		public void Copy()
		//		{
		//			
		//			ShapeCollection ar = new ShapeCollection();
		//			foreach(Shape so in extract.Shapes)
		//			{	
		//				if(so.IsSelected)
		//				{
		//					ar.Add(so);					
		//				}
		//			}
		//			if(ar.Count>0)
		//			{
		//					
		//				DataObject blurb = new DataObject(format.Name,extract);			
		//				Clipboard.SetDataObject(blurb);
		//			}
		//
		//			
		//		}

		public void Cut()
		{
			EntityBundle bundle = new EntityBundle(this);

			foreach(Shape sh in this.extract.Shapes)
				if(sh.IsSelected) bundle.Shapes.Add(sh);
			foreach(Connection con in this.extract.Connections)
				if(con.IsSelected && con.From.BelongsTo.IsSelected && con.To.BelongsTo.IsSelected) //only if the connection is internal we'll cut it away, external links are dropped
					bundle.Connections.Add(con);

			DataObject data = new DataObject("Netron.GraphLib.EntityBundle", bundle.Copy());


			Clipboard.SetDataObject(data);
			bundle.Detach();
		}
		/// <summary>
		/// Copies whatever is selected on the canvas to the clipboard
		/// </summary>
		public void Copy()
		{
			EntityBundle bundle = new EntityBundle(this);

			foreach(Shape sh in this.extract.Shapes)
				if(sh.IsSelected) bundle.Shapes.Add(sh);
			foreach(Connection con in this.extract.Connections)
				if(con.IsSelected && con.From.BelongsTo.IsSelected && con.To.BelongsTo.IsSelected) //only if the connection is internal we'll cut it away, external links are dropped
					bundle.Connections.Add(con);

			
			DataObject data = new DataObject("Netron.GraphLib.EntityBundle", bundle.Copy());

            //Copy Bitmap as well for other applications
            Bitmap bmp = bundle.TakeScreenshotWithBackground(this.Graphics, Color.White);
            if (bmp != null)
            {
                data.SetImage(bmp);
            }

            Clipboard.SetDataObject(data);
		}
		/// <summary>
		/// Copies the selected elements of the diagram as an image to the clipboard
		/// </summary>
		public void CopyAsImage()
		{
			try
			{
				EntityBundle bundle = new EntityBundle(this);

				foreach(Shape sh in this.extract.Shapes)
					if(sh.IsSelected) bundle.Shapes.Add(sh);
				foreach(Connection con in this.extract.Connections)
					if(con.IsSelected && con.From.BelongsTo.IsSelected && con.To.BelongsTo.IsSelected) //only if the connection is internal we'll cut it away, external links are dropped
						bundle.Connections.Add(con);
				Bitmap bmp = bundle.TakeScreenshotWithBackground(this.Graphics, Color.White);			
				if(bmp!=null)
				{
					DataObject data = new DataObject(DataFormats.Bitmap, bmp);
					Clipboard.SetDataObject(data);			
				}
			}
			catch(Exception exc)
			{
				OutputInfo(exc.Message, OutputInfoLevels.Exception);
			}
			
		}


		/// <summary>
		/// Pastes data on the canvas. Possible formats are an EntityBundle, an image,...
		/// </summary>
		public void Paste()
		{
			IDataObject data =  Clipboard.GetDataObject();
			if(data.GetDataPresent("Netron.GraphLib.EntityBundle"))
			{
				EntityBundle bundle = data.GetData("Netron.GraphLib.EntityBundle") as EntityBundle;
				if(bundle!=null)
				{

					bundle.Site = this;
					//unwrap the bundle, this assigns the site and does some postserialization
					GraphLib.IO.Binary.BinarySerializer.UnwrapBundle(bundle, this);

					//change the UID
					if(bundle!=null)
					{
						for(int k=0; k<bundle.Shapes.Count; k++)					
						{
							bundle.Shapes[k].GenerateNewUID();
                            bundle.Shapes[k].Site = this;
                            bundle.Shapes[k].SetLayer(Abstract.CurrentLayer.Name);
							//the connectors as well
							foreach(Connector c in bundle.Shapes[k].Connectors)
							{
								c.GenerateNewUID();
							    c.Site = this;
							    c.SetLayer(Abstract.CurrentLayer.Name);
							}
						}
					    for (int k = 0; k < bundle.Connections.Count; k++)
					    {
					        bundle.Connections[k].GenerateNewUID();
                            bundle.Connections[k].Site = this;
                            bundle.Connections[k].SetLayer(Abstract.CurrentLayer.Name);

                        }
                    }

					this.Deselect();
					AddBundle(bundle);
					bundle.SelectAll();	
					bundle.Offset(10,10);					
				}
			}
			else if(data.GetDataPresent(DataFormats.Bitmap))
			{
				Bitmap bmp = data.GetData(DataFormats.Bitmap) as Bitmap;
				if(bmp!=null)
				{
					Shape shape = AddShape("47D016B9-990A-436c-ADE8-B861714EBE5A", insertionPoint);		
					PropertyInfo info = shape.GetType().GetProperty("Image");
					info.SetValue(shape, bmp, null);
					shape.Invalidate();		
				}
			}
			

		}
		/// <summary>
		/// Deletes the selected elements from the canvas
		/// </summary>
		public void Delete()
		{
			this.extract.Delete();
		}

		/// <summary>
		/// The clipboard paste action.
		/// 
		/// </summary>
		/// <remarks>
		/// This doesn't work, bug in Clipboard object, hopefully fixed in Net v2 
		///</remarks>
		//		public void Paste()
		//		{
		//			
		//			IDataObject data = Clipboard.GetDataObject();
		//			
		//			
		//			if (data.GetDataPresent(format.Name))
		//			{				
		//				//unselect all
		//				foreach(Shape sh in extract.Shapes) sh.IsSelected=false;
		//
		//				foreach(Shape so in data.GetData(format.Name) as ArrayList)
		//				{
		//					
		//					so.IsSelected=true;
		//					so.GenerateNewUID();
		//					so.Tracker.Move(new PointF(10,10),Size,mSnap,mGridSize);
		//					so.Invalidate();
		//					RectangleF r = so.Rectangle;
		//					so.Rectangle = new RectangleF(so.Rectangle.X+10, so.Rectangle.Y+10, r.Width, r.Height);
		//					so.Invalidate();
		//											
		//					so.Insert(this.extract);
		//
		//
		//		
		//					foreach(Connector c in so.Connectors)
		//					{
		//						c.GenerateNewUID();
		//						//						ArrayList connar = c.Connections.Clone();
		//						foreach(Connection n in c.Connections)
		//						{
		//							
		//							Connector s = null;
		//							Connector e = null;
		//							if (n.From==null || n.To==null) continue;
		//							if (n.From.IsGuiReset && n.To.IsGuiReset) 
		//							{
		//								s=n.From;e=n.To; 
		//							}
		//							else if(n.From.IsGuiReset) //find the existing (before paste, that is) connector UID
		//							{s=n.From;e=(Connector) GetEntity(n.To.UID.ToString());}								
		//							else if(n.To.IsGuiReset==true)
		//							{s=(Connector) GetEntity(n.From.UID.ToString());e=n.To;}
		//							//only add it if it does not exist
		//							if (!ConnectionExists(s,e))
		//							{
		//								n.GenerateNewUID();
		//								//m.AddInsertConnection(n,s,e);
		//								n.Insert(s,e);
		//								
		//							}
		//						}
		//						c.Connections.Clear();
		//					}
		//
		//				}
		//
		//				//extract.History.Do(m);				
		//			}
		//
		//			
		//
		//		}


		/// <summary>
		/// There is only one way to specify a connection: by specifying the two connectors
		/// Unless you have a single connector per shape which allows you to use the shape GUID
		/// Going via node label is possible as well if you filter out doubles on (automatic) creation.
		/// </summary>
		/// <param name="From"></param>
		/// <param name="To"></param>
		public Connection AddConnection(Connector From, Connector To)
		{
			if (From !=null && To != null)
			{
				Connection con =new Connection(this);
				con.From=From;
				con.To=To;
				From.Connections.Add(con);
				To.Connections.Add(con);
				con.LineEnd=this.DefaultConnectionEnd;
				con.LinePath = this.DefaultConnectionPath;
				extract.Insert(con);
				con.Site = this;
				Update();
				Invalidate();
				RaiseOnConnectionAdded(con,false);				
				return con;

			}
			return null;
	
		}
		
	
		
		
		/// <summary>
		/// Selects all shapes and things of the plex.
		/// </summary>
		/// <param name="includeAll">true if connectors and connection should be selected as well, otherwise only the shapes will be selected.</param>
		public void SelectAll(bool includeAll)
		{
			if(includeAll)
			{
				foreach (Shape o in extract.Shapes)
				{
				    if (!o.IsVisible || !o.Layer.Visible)
				    {
				        o.IsSelected = false;
                        foreach (Connector c in o.Connectors)
                            foreach (Connection n in c.Connections)
                                n.IsSelected = false;
                        continue;
				    }

					o.IsSelected= true;

					foreach (Connector c in o.Connectors)
						foreach (Connection n in c.Connections)
							n.IsSelected=true;
				}

			
				Update();
			}
			else
			{
				foreach (Shape so in extract.Shapes)
				{
				    if (!so.IsVisible || !so.Layer.Visible)
				    {
				        so.IsSelected = false;
                        continue;
				    }

                    so.IsSelected=true;
				}
			}
		}


		/// <summary>
		/// Gets a shape instance with the given key
		/// </summary>
		/// <param name="shapeKey"></param>
		/// <returns></returns>
		protected Shape GetShapeInstance(string shapeKey)
		{
			Shape shape = null;
			ObjectHandle handle;									
			
			for(int k=0; k<mLibraries.Count;k++)
			{
				for(int m=0; m<mLibraries[k].ShapeSummaries.Count; m++)
				{
					if(mLibraries[k].ShapeSummaries[m].Key == shapeKey)
					{
						//TODO: lightweight pattern here?
						//Assembly ass = Assembly.LoadFrom(mLibraries[k].Path);
						try
						{
								
							//activationAttributes = new object[]{new SynchronizationAttribute()};
							//Note this one! The OpenFileDialog changes the CurrentDirectory, this is the only way to make sure .Net will look in the bin directory.
							Directory.SetCurrentDirectory(Path.GetDirectoryName(Application.ExecutablePath));
							//passing the IGraphSite does not for object not inheriting from MarshalByRefObject							
							handle = Activator.CreateInstanceFrom(mLibraries[k].Path,mLibraries[k].ShapeSummaries[m].ReflectionName);
							shape = handle.Unwrap() as Shape;
							shape.Site = this;
							lastAddedShapeKey = shapeKey; //keep for ALT+click addition
							return shape;
						}
						catch(Exception exc)
						{
							Trace.WriteLine(exc.Message,"GraphControl.GetShapeInstance");
						}
					}
				}
			}
			return shape;
		}

		/// <summary>
		/// Returns a Summary object of the given entity
		/// </summary>
		/// <param name="e">An Entity object</param>
		/// <returns>A Summary object which encapsulates the essentials of the instance as far as reflection is concerned</returns>
		public Summary GetSummary(Entity e)
		{
			Type type = e.GetType();
			if(typeof(Shape).IsInstanceOfType(e))
			{
				
				for(int k=0; k<mLibraries.Count;k++)
				{
					for(int m=0; m<mLibraries[k].ShapeSummaries.Count; m++)
					{
						if(mLibraries[k].ShapeSummaries[m].ReflectionName == type.FullName)
						{
							return mLibraries[k].ShapeSummaries[m];
						}
					}   
				}
				
			}
			else if(typeof(Connection).IsInstanceOfType(e))
			{

				for(int k=0; k<mLibraries.Count;k++)
				{
					for(int m=0; m<mLibraries[k].ConnectionSummaries.Count; m++)
					{
						if(mLibraries[k].ConnectionSummaries[m].ReflectionName == type.FullName)
						{
							return mLibraries[k].ConnectionSummaries[m];
						}
					}   
				}
			}			
			
			return null;
		}
		/// <summary>
		/// Collects the selected entities into an EntityBundle. The returned bundle is a copy of the entities and can be used to serialize part of the diagram.
		/// <seealso cref="GroupSelection"/>
		/// </summary>
		/// <returns></returns>
		public EntityBundle BundleSelection()
		{
			EntityBundle bundle = new EntityBundle(this);			
			foreach(Shape sh in this.extract.Shapes)
				if(sh.IsSelected) bundle.Shapes.Add(sh);
			foreach(Connection con in extract.Connections)
				if(con.IsSelected) bundle.Connections.Add(con);

			return bundle.Copy();
		}
		/// <summary>
		/// Groups the selected entities into an EntityBundle. The returned bundle is detached from the diagram and hence not visible after this method.
		/// This method is useful to cut and use a part of the diagram somewhere else.
		/// </summary>
		/// <returns></returns>
		public EntityBundle GroupSelection()
		{
			EntityBundle bundle = new EntityBundle(this);

			foreach(Shape sh in this.extract.Shapes)
				if(sh.IsSelected) bundle.Shapes.Add(sh);
			foreach(Connection con in this.extract.Connections)
				if(con.IsSelected) bundle.Connections.Add(con);
			bundle.Name = "GroupShape";
			bundle.Description = "Testing the grouping feature.";
			bundle.TakeScreenshot(this.Graphics);
			bundle.Detach();
			return bundle;
		}
		/// <summary>
		/// Inserts an EntityBundle in the diagram
		/// </summary>
		/// <param name="bundle"></param>
		public void AddBundle(EntityBundle bundle)
		{
			if(bundle==null) return;
			for(int k=0; k<bundle.Shapes.Count; k++)
				bundle.Shapes[k].Insert(extract);
			for(int k=0; k<bundle.Connections.Count; k++)
				extract.Connections.Add(bundle.Connections[k]);
		}
		/// <summary>
		/// Returns the first shape with the given text
		/// </summary>
		/// <param name="label"></param>
		/// <returns></returns>
		public Shape GetShapeByLabel(string label)
		{
			foreach(Shape so in extract.Shapes)
				if(so.Text.ToLower()==label.ToLower())	return so;
			return null;
		}
		/// <summary>
		/// Creates a blank new canvas
		/// 
		/// </summary>
		public void NewDiagram(bool raiseEvent)
		{
			//allow the host to ask for saving the current diagram
			RaiseOnClear();
			this.extract=new GraphAbstract();
			this.extract.Site = this;
			this.mFileName=null;
			this.OutputInfo("The canvas was cleared.");
			this.Invalidate(true);
		}
	
	
		
		/// <summary>
		/// Overrides the method to invalidate the control when the user scrolls the diagram
		/// 
		/// </summary>
		/// <param name="m"></param>
		/// <remarks>Not possible with the available overridable methods of .Net as far as I know</remarks>
		protected override void WndProc(ref Message m)
		{
			if(m.Msg==WM_VSCROLL || m.Msg==WM_HSCROLL)
			{
				this.Invalidate();
			}
			base.WndProc (ref m);
		}


		#endregion

		#region Property bag

		/// <summary>
		/// Returns the list of layers as an Attribute-array.
		/// Used by the propertygrid to display a list of layers, the mechanism is rater involved.
		/// </summary>
		/// <returns></returns>
		public Attribute[] GetLayerAttributes()
		{
			//			ArrayList list = new ArrayList();	
			//			for(int k=0;k<Layers.Count;k++)
			//				list.Add(Layers[k]);
			return new Attribute[]{ new GraphLayerAttribute(Layers)};
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
				
				case "AutomataPulse": e.Value=this.mAutomataPulse;break;
				case "BackgroundColor": e.Value=this.mBackgroundColor;break;
				case "BackgroundImagePath": e.Value=this.mBackgroundImagePath;break;
				case "DefaultConnectionPath": e.Value=this.connectionPath;break;
				case "DefaultConnectionEnd": e.Value=this.connectionEnd;break;
				case "EnableContextMenu": e.Value=this.mEnableContextMenu;break;
				case "EnableLayout": e.Value=this.mEnableLayout;break;
				case "GradientBottom": e.Value=this.mGradientBottom;break;
				case "GradientTop": e.Value=this.mGradientTop;break;
				case "GraphLayoutAlgorithm": e.Value=this.layoutFactory.GraphLayoutAlgorithm;break;
				case "GridSize": e.Value=this.mGridSize;break;
				case "RestrictToCanvas": e.Value=this.mRestrictToCanvas;break;
				case "ShowGrid": e.Value=this.mShowGrid;break;
				case "Snap": e.Value=this.mSnap;break;
				case "BackgroundType": e.Value=this.mBackgroundType;break;
				case "AllowAddShape": e.Value=this.mAllowAddShape; break;
				case "AllowAddConnection": e.Value=this.mAllowAddConnection; break;
				case "AllowDeleteShape": e.Value = this.mAllowDeleteShape; break;
				case "AllowMoveShape": e.Value = this.mAllowMoveShape; break;
				case "GradientMode": e.Value = this.mGradientMode; break;
				case "Locked" : e.Value = this.mLocked; break;
				case "EnableTooltip" : e.Value = this.EnableToolTip; break;
				case "ShowAutomataController": e.Value = this.ShowAutomataController; break;
					break;
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
				case "AutomataPulse": 
					this.AutomataPulse = (int) e.Value;										
					break;
				case "BackgroundColor": 
					this.mBackgroundColor = (Color) e.Value;					
					this.Invalidate();
					break;
				case "BackgroundImagePath": 
					this.mBackgroundImagePath = (string) e.Value;					
					this.Invalidate();
					break;
				case "DefaultConnectionPath": 
					this.connectionPath = (string) e.Value;										
					break;
				case "DefaultConnectionEnd": 
					this.connectionEnd = (ConnectionEnd) e.Value;										
					break;
				case "EnableContextMenu": 
					this.EnableContextMenu = (bool) e.Value;					
					this.Invalidate();
					break;
				case "EnableLayout": 
					this.EnableLayout = (bool) e.Value;
					break;
				case "GradientBottom": 
					this.mGradientBottom = (Color) e.Value;					
					this.Invalidate();
					break;
				case "GradientTop": 
					this.mGradientTop = (Color) e.Value;					
					this.Invalidate();
					break;
				case "GraphLayoutAlgorithm": 
					this.layoutFactory.GraphLayoutAlgorithm = (GraphLayoutAlgorithms) e.Value;										
					break;
				case "GridSize": 
					this.mGridSize = (int) e.Value;					
					this.Invalidate();
					break;				
				case "RestrictToCanvas": 
					this.mRestrictToCanvas = (bool) e.Value;										
					break;
				case "ShowGrid": 
					this.mShowGrid = (bool) e.Value;					
					this.Invalidate();
					break;
				case "Snap": 
					this.mSnap = (bool) e.Value;					
					this.Invalidate();
					break;
				case "BackgroundType": 
					this.mBackgroundType= (CanvasBackgroundType) e.Value;					
					this.Invalidate();
					break;
				case "AllowAddShape":
					this.mAllowAddShape = (bool) e.Value;
					break;
				case "AllowAddConnection":
					this.mAllowAddConnection = (bool) e.Value;
					break;
				case "AllowDeleteShape":
					this.mAllowDeleteShape = (bool) e.Value;
					break;
				case "AllowMoveShape":
					this.mAllowMoveShape = (bool) e.Value;
					break;
				case "GradientMode":
					this.mGradientMode = (LinearGradientMode) Enum.Parse(typeof(LinearGradientMode),e.Value.ToString());
					break;
				case "Locked":
					this.mLocked = (bool) e.Value;
					break;
				case "EnableTooltip":
					this.EnableToolTip = (bool) e.Value;
					break;
				case "ShowAutomataController":
					this.ShowAutomataController = (bool) e.Value;
					break;
			}
		}

		
		/// <summary>
		/// Adds the propertygrid visible properties to the bag
		/// </summary>
		protected void AddProperties()
		{
			try
			{					
				bag.GetValue+=new PropertySpecEventHandler(GetPropertyBagValue);
				bag.SetValue+=new PropertySpecEventHandler(SetPropertyBagValue);
				bag.Properties.Add(new PropertySpec("AutomataPulse",typeof(int),"Automata","Gets or sets time interval of the automata update pulse.",10));
				bag.Properties.Add(new PropertySpec("BackgroundColor",typeof(Color),"Appearance","Gets or sets the background color of the canvas.",Color.SteelBlue));
				bag.Properties.Add(new PropertySpec("BackgroundType",typeof(CanvasBackgroundType),"Appearance","Gets or sets the kind of background the canvas has.",CanvasBackgroundType.FlatColor));
				bag.Properties.Add(new PropertySpec("BackgroundImagePath",typeof(string),"Appearance","Gets or sets the path to the background image (only visible or used if the background type is 'Image').",""));
				bag.Properties.Add(new PropertySpec("DefaultConnectionPath",typeof(string),"Graph","Gets or sets the default path style or shape of the newly created connections.","Default"));
				bag.Properties.Add(new PropertySpec("DefaultConnectionEnd",typeof(ConnectionEnd),"Graph","Gets or sets the default line end of the newly created connections.",ConnectionEnd.NoEnds));
				bag.Properties.Add(new PropertySpec("EnableContextMenu",typeof(bool),"Appearance","Gets or sets whether the context menu is visible.",true));
				bag.Properties.Add(new PropertySpec("EnableLayout",typeof(bool),"Graph","Gets or sets whether layout algorithms are active.",false));
				bag.Properties.Add(new PropertySpec("GradientBottom",typeof(Color),"Appearance","Gets or sets the lower color of the gradient.",Color.SteelBlue));
				bag.Properties.Add(new PropertySpec("GradientTop",typeof(Color),"Appearance","Gets or sets the upper color of the gradient.",Color.White));
				bag.Properties.Add(new PropertySpec("GraphLayoutAlgorithm",typeof(GraphLayoutAlgorithms),"Graph","The layout algorithm to be used.",GraphLayoutAlgorithms.SpringEmbedder));
				bag.Properties.Add(new PropertySpec("GridSize",typeof(int),"Appearance","Gets or sets the grid size.",20));				
				bag.Properties.Add(new PropertySpec("RestrictToCanvas",typeof(bool),"Appearance","Gets or sets wether the graph shapes should be kept inside the canvas frame or allowed to move/resize outside it.",true));
				bag.Properties.Add(new PropertySpec("ShowGrid",typeof(bool),"Appearance","Gets or sets whether the grid is visible.",false));
				bag.Properties.Add(new PropertySpec("Snap",typeof(bool),"Appearance","Gets or sets whether the graph elements mSnap to the grid.",false));
				bag.Properties.Add(new PropertySpec("AllowAddShape",typeof(bool),"Graph","Gets or sets whether shapes can be added to the graph.",true));
				bag.Properties.Add(new PropertySpec("AllowAddConnection",typeof(bool),"Graph","Gets or sets whether connections can be added to the graph.",true));
				bag.Properties.Add(new PropertySpec("AllowDeleteShape",typeof(bool),"Graph","Gets or sets whether shapes can be deleted from the graph.",true));
				bag.Properties.Add(new PropertySpec("GradientMode",typeof(LinearGradientMode),"Appearance","Gets or sets the direction of the gradient.",LinearGradientMode.ForwardDiagonal));
				bag.Properties.Add(new PropertySpec("AllowMoveShape",typeof(bool),"Graph","Gets or sets whether shapes can be moved.",true));
				bag.Properties.Add(new PropertySpec("Locked",typeof(bool),"Graph","Gets or sets whether the diagram is locked.",false));
				bag.Properties.Add(new PropertySpec("EnableTooltip",typeof(bool),"Appearance","Gets or sets whether the tooltip is active.",true));
				bag.Properties.Add(new PropertySpec("ShowAutomataController",typeof(bool),"Appearance","Gets or sets whether the automata controller widget is visible.",false));
				//					PropertySpec spec=new PropertySpec("MDI children",typeof(string[]));
				//					spec.Attributes=new Attribute[]{new System.ComponentModel.ReadOnlyAttribute(true)};
				//					bag.Properties.Add(spec);
							
					
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message,"GraphControl.AddProperties");
			}

		}
		
		#endregion

		

		#endregion

		
	}
}

