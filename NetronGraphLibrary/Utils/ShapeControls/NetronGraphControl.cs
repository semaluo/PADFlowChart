using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
namespace Netron.GraphLib
{
	/// <summary>
	/// Base class for a (graph) shape embedded control 
	/// Not to be confused with the GraphControl control, <seealso cref="Netron.GraphLib.UI.GraphControl"/>
	/// </summary>
	public abstract class NetronGraphControl
	{

		#region Fields
		/// <summary>
		/// the font to be used
		/// </summary>
		protected Font mFont;
		/// <summary>
		/// the owner of the control
		/// </summary>
		protected Shape parent;
		/// <summary>
		/// the location of the control
		/// </summary>
		protected Point mLocation;
		/// <summary>
		/// the width
		/// </summary>
		protected int mWidth = 100;
		/// <summary>
		/// the height
		/// </summary>
		protected int mHeight = 15;

		#endregion

		#region Properties
		/// <summary>
		/// Gets the base rectangle of the control
		/// </summary>
		public Rectangle Rectangle
		{
			get{return new Rectangle(mLocation.X,mLocation.Y,mWidth,mHeight);}
			
		}
		/// <summary>
		/// Gets or sets the location of the control
		/// </summary>
		public Point Location
		{
			get{return mLocation;}
			set{mLocation = value;}
		}
		/// <summary>
		/// Gets or sets the height of the control
		/// </summary>
		public int Height
		{
			get{return mHeight;}
			set{mHeight = value;}
		}
		/// <summary>
		/// Gets or sets the width of the control
		/// </summary>
		public int Width
		{
			get{return mWidth;}
			set{mWidth = Math.Max(value,mHeight);}
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="parent"></param>
		protected NetronGraphControl(Shape parent)
		{	
			this.parent = parent;
			Init();
		}
		#endregion

		#region Methods
		/// <summary>
		/// Initializes the control
		/// </summary>
		private void Init()
		{
			mFont = parent.Font;
		}

		#region Abstract methods
		/// <summary>
		/// Paints the control
		/// </summary>
		/// <param name="g"></param>
		public abstract void Paint(Graphics g);
		/// <summary>
		/// Action(s) on mouse-down
		/// </summary>
		/// <param name="e"></param>
		public abstract void OnMouseDown(MouseEventArgs e);
		/// <summary>
		/// Action(s) on mouse move
		/// </summary>
		/// <param name="e"></param>
		public abstract void OnMouseMove(MouseEventArgs e);
		/// <summary>
		/// Action(s) on hitting the control
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		public abstract bool Hit(Point p);
		#endregion

		#region Virtual methods
		/// <summary>
		/// Action(s) on key-down
		/// </summary>
		/// <param name="e"></param>
		public virtual void OnKeyDown(KeyEventArgs e){}
		/// <summary>
		/// Action(s) on key-press
		/// </summary>
		/// <param name="e"></param>
		public virtual void OnKeyPress(KeyPressEventArgs e){}
		#endregion
		#endregion

	}

	
}
