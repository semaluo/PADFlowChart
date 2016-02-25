using System;
using System.Windows.Forms;
using System.Drawing;
namespace Netron.GraphLib.UI
{
	/// <summary>
	/// Mirrors the GraphLayer class and encapsulates it in a ListViewItem
	/// to be displayed in a ListView. When the Commit() method is called the
	/// values are transmitted to the referenced layer.
	/// The constructor gets a layer as a ref-parameter.
	/// </summary>
	public class LayerListItem : ListViewItem
	{

		#region Fields

		private readonly string constChecked = "Y";
		private readonly string constUnchecked = "N";

		private bool mFloating = false;

		/// <summary>
		/// the referenced layer
		/// </summary>
		protected GraphLayer mLayer;
		/// <summary>
		/// opacity in percent
		/// </summary>
		protected int mOpacity = 100;
		/// <summary>
		/// the layer's color
		/// </summary>
		protected Color mLayerColor = Color.Red;
		/// <summary>
		/// whether the shapes on this layer should use the layer's color
		/// instead of their own
		/// </summary>
		protected bool mUseColor = true;
		/// <summary>
		/// the name of the layer
		/// </summary>
		protected string mName = string.Empty;
		/// <summary>
		/// whether the layer is locked
		/// </summary>
		protected bool mLocked = false;
		/// <summary>
		/// the layer's number in the collection
		/// </summary>
		protected int mNumber ;
		/// <summary>
		/// whether the layer is visible
		/// </summary>
		protected bool mVisible = true;
		#endregion

		
		#region Properties
		/// <summary>
		/// Gets the layer corresponding to this listview item
		/// </summary>
		public GraphLayer Layer
		{
			get{return mLayer;}
		}
		/// <summary>
		/// Gets or sets whether the item is already part of the graph-control
		/// If true, the item has to be added to the layers collection
		/// </summary>
		public bool Floating
		{
			get{return mFloating;}
			set{mFloating = value;}
		}

		/// <summary>
		/// Gets or sets whether the layer is visible
		/// </summary>
		public bool Visible
		{
			get{return mVisible;}
			set{mVisible = value;}

		}

		/// <summary>
		/// Gets the number of the layer in the collection
		/// </summary>
		public int Number
		{
			get{return mNumber;}
			set{mNumber = value;
			this.SubItems[0].Text = value.ToString();
			}
		}
		/// <summary>
		/// Gets or sets the name of the layer
		/// </summary>
		public string Name
		{
			get{return mName;}
			set{mName = value;
			this.Text = value;
			}
		}

		/// <summary>
		/// Gets or sets whether the shapes on this layer should use the layer's color
		/// instead of their own
		/// </summary>
		public bool UseColor
		{
			get{return mUseColor;}
			set{mUseColor = value;}
		}
		/// <summary>
		/// Gets or sets the opacity of the layer
		/// </summary>
		public int Opacity
		{
			get{return mOpacity;}
			set{mOpacity = value;
				
			}
		}

		/// <summary>
		/// Gets or sets the color of the alyer
		/// </summary>
		public Color LayerColor
		{
			get{return mLayerColor;}
			set{mLayerColor = value;}
		}

		/// <summary>
		/// Gets or sets whether the layer is locked
		/// </summary>
		public bool Locked
		{
			get{return mLocked;}
			set{mLocked = value;}
		}
		#endregion
		
		/// <summary>
		/// Default ctor
		/// </summary>
		/// <param name="layer">ref-ed GraphLayer from the graphcontrol</param>
		public LayerListItem(ref GraphLayer layer)
		{
			
			mLayer = layer;
			this.Locked = layer.Locked;
			this.Name = layer.Name;
			this.Number = layer.Number;
			this.Opacity = layer.Opacity;
			this.UseColor = layer.UseColor;
			this.Visible = layer.Visible;
			this.LayerColor = layer.LayerColor;
			this.Text = Name;
			this.SubItems.Add(Number.ToString());
			if(Visible)
				this.SubItems.Add(constChecked,Color.Black,Color.Empty,new Font("Wingdings",11f));
			else
				this.SubItems.Add(constUnchecked,Color.Black,Color.Empty,new Font("Wingdings",11f));

			if(Locked)
				this.SubItems.Add(constChecked,Color.Black,Color.Empty,new Font("Wingdings",11f));
			else
				this.SubItems.Add(constUnchecked,Color.Black,Color.Empty,new Font("Wingdings",11f));

			if(UseColor)
				this.SubItems.Add(constChecked,Color.Black,Color.Empty,new Font("Wingdings",11f));
			else
				this.SubItems.Add(constUnchecked,Color.Black,Color.Empty,new Font("Wingdings",11f));

			UseItemStyleForSubItems = false;		
			
			
		}

		/// <summary>
		/// Commits the changes to the referenced layer of the graphcontrol
		/// </summary>
		public void Commit()
		{
			if(mFloating) //not committed to the control yet
			{
				
			}
			else
			{
				mLayer.Locked = this.Locked; 
				mLayer.Name = this.Name;
				mLayer.SetNumber( this.Number);
				mLayer.Opacity = this.Opacity;
				mLayer.UseColor = this.UseColor;
				mLayer.Visible = this.Visible;
				mLayer.LayerColor = this.mLayerColor;
			}

		}


	}
}
