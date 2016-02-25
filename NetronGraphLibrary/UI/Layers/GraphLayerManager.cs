using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Netron.GraphLib;
namespace Netron.GraphLib.UI
{
	/// <summary>
	/// This UserControl allows you to manage the graph-layers
	/// </summary>
	[ToolboxItem(true)]
	public class GraphLayerManager : System.Windows.Forms.UserControl
	{
		#region Fields
		private GraphLayerCollection deletedItems = new GraphLayerCollection();
		private System.Windows.Forms.ColumnHeader colName;
		private System.Windows.Forms.ColumnHeader colNumber;
		private System.Windows.Forms.ColumnHeader colVisible;
		private System.Windows.Forms.ColumnHeader colLock;
		private System.Windows.Forms.ColumnHeader colColor;
		private ListViewEx listView;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button RemoveButton;
		private System.Windows.Forms.Button RenameButton;
		private System.Windows.Forms.Button NewLayerButton;
		private System.Windows.Forms.Button ChooseColorButton;
		private System.Windows.Forms.Label CurrentColor;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox TransText;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TrackBar TransGauge;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private GraphLayerCollection mLayers;
		private GraphControl mSite;
		#endregion

		#region Properties

		#endregion
		
		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		public GraphLayerManager()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}
		#endregion

		#region Methods

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.listView = new Netron.GraphLib.UI.ListViewEx();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colVisible = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLock = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colColor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.RenameButton = new System.Windows.Forms.Button();
            this.NewLayerButton = new System.Windows.Forms.Button();
            this.ChooseColorButton = new System.Windows.Forms.Button();
            this.CurrentColor = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TransText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TransGauge = new System.Windows.Forms.TrackBar();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TransGauge)).BeginInit();
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colNumber,
            this.colVisible,
            this.colLock,
            this.colColor});
            this.listView.Cursor = System.Windows.Forms.Cursors.Hand;
            this.listView.FullRowSelect = true;
            this.listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(8, 8);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(496, 160);
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            this.listView.Click += new System.EventHandler(this.listView_Click);
            // 
            // colName
            // 
            this.colName.Text = "Name";
            this.colName.Width = 157;
            // 
            // colNumber
            // 
            this.colNumber.Text = "#";
            this.colNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // colVisible
            // 
            this.colVisible.Text = "Visible";
            this.colVisible.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colVisible.Width = 91;
            // 
            // colLock
            // 
            this.colLock.Text = "Lock";
            this.colLock.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colLock.Width = 87;
            // 
            // colColor
            // 
            this.colColor.Text = "Use color";
            this.colColor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colColor.Width = 101;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.RemoveButton);
            this.groupBox1.Controls.Add(this.RenameButton);
            this.groupBox1.Controls.Add(this.NewLayerButton);
            this.groupBox1.Controls.Add(this.ChooseColorButton);
            this.groupBox1.Controls.Add(this.CurrentColor);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.TransText);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.TransGauge);
            this.groupBox1.Location = new System.Drawing.Point(8, 176);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(496, 104);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            // 
            // RemoveButton
            // 
            this.RemoveButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.RemoveButton.Location = new System.Drawing.Point(88, 24);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(75, 23);
            this.RemoveButton.TabIndex = 20;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // RenameButton
            // 
            this.RenameButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.RenameButton.Location = new System.Drawing.Point(168, 24);
            this.RenameButton.Name = "RenameButton";
            this.RenameButton.Size = new System.Drawing.Size(75, 23);
            this.RenameButton.TabIndex = 19;
            this.RenameButton.Text = "Rename...";
            this.RenameButton.Click += new System.EventHandler(this.RenameButton_Click);
            // 
            // NewLayerButton
            // 
            this.NewLayerButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.NewLayerButton.Location = new System.Drawing.Point(8, 24);
            this.NewLayerButton.Name = "NewLayerButton";
            this.NewLayerButton.Size = new System.Drawing.Size(75, 23);
            this.NewLayerButton.TabIndex = 18;
            this.NewLayerButton.Text = "New...";
            this.NewLayerButton.Click += new System.EventHandler(this.NewLayerButton_Click);
            // 
            // ChooseColorButton
            // 
            this.ChooseColorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ChooseColorButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ChooseColorButton.Location = new System.Drawing.Point(448, 21);
            this.ChooseColorButton.Name = "ChooseColorButton";
            this.ChooseColorButton.Size = new System.Drawing.Size(25, 23);
            this.ChooseColorButton.TabIndex = 17;
            this.ChooseColorButton.Text = "...";
            this.ChooseColorButton.Click += new System.EventHandler(this.ChooseColorButton_Click);
            // 
            // CurrentColor
            // 
            this.CurrentColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CurrentColor.BackColor = System.Drawing.Color.Brown;
            this.CurrentColor.Location = new System.Drawing.Point(352, 21);
            this.CurrentColor.Name = "CurrentColor";
            this.CurrentColor.Size = new System.Drawing.Size(88, 23);
            this.CurrentColor.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(256, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 23);
            this.label2.TabIndex = 15;
            this.label2.Text = "Layer color:";
            // 
            // TransText
            // 
            this.TransText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TransText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TransText.Location = new System.Drawing.Point(448, 66);
            this.TransText.MaxLength = 3;
            this.TransText.Name = "TransText";
            this.TransText.Size = new System.Drawing.Size(32, 21);
            this.TransText.TabIndex = 14;
            this.TransText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TransText_KeyPress);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(256, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 23);
            this.label1.TabIndex = 13;
            this.label1.Text = "Transparency:";
            // 
            // TransGauge
            // 
            this.TransGauge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.TransGauge.LargeChange = 10;
            this.TransGauge.Location = new System.Drawing.Point(344, 52);
            this.TransGauge.Maximum = 100;
            this.TransGauge.Name = "TransGauge";
            this.TransGauge.Size = new System.Drawing.Size(104, 45);
            this.TransGauge.TabIndex = 12;
            this.TransGauge.TickFrequency = 10;
            this.TransGauge.Scroll += new System.EventHandler(this.TransGauge_Scroll);
            // 
            // GraphLayerManager
            // 
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.listView);
            this.Name = "GraphLayerManager";
            this.Size = new System.Drawing.Size(512, 288);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TransGauge)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Loads the layers in the listview 
		/// </summary>
		//public void LoadLayers(ref GraphLayerCollection layers)
		public void LoadLayers(GraphControl site)
		{
			mLayers = site.Layers;
			mSite = site;
			// Name | # | Visible | Lock | Color
			GraphLayer layer;
			for(int k=0;k< mLayers.Count; k++)
			{
				layer = mLayers[k];
				LayerListItem item = CreateItem(ref layer);				
				listView.Items.Add(item);
				AddCheckbox(2,k,item, "Visible", layer.Visible);
				AddCheckbox(3,k,item, "Locked", layer.Locked);
				AddCheckbox(4,k,item, "UseColor", layer.UseColor);
			}
			
	
			  
		}

		/// <summary>
		/// Adds a checkbox in the listview
		/// </summary>
		/// <param name="col"></param>
		/// <param name="row"></param>
		/// <param name="item"></param>
		/// <param name="name"></param>
		/// <param name="isChecked"></param>
		private void AddCheckbox(int col, int row, LayerListItem item, string name, bool isChecked)
		{
			
			CheckBox b = new CheckBox();
			b.Text = "";
			b.BackColor = this.listView.BackColor;			
			b.FlatStyle = FlatStyle.Flat;
			b.CheckAlign = ContentAlignment.TopCenter;
			listView.AddEmbeddedControl(b, col, row);
			b.Tag = new CheckBoxTag(item,name);;
			b.Checked = isChecked;
			b.Click+=new EventHandler(checkbox_Click);
			
		}


		/// <summary>
		/// Returns the collection of layers
		/// </summary>
		/// <returns></returns>
		public GraphLayerCollection GetLayers()
		{
			GraphLayer layer;
			ListViewItem item;
			GraphLayerCollection col =new GraphLayerCollection();
			for(int k=0; k<listView.Items.Count; k++)
			{
				item = listView.Items[k];
				layer = new GraphLayer();
				layer.SetNumber(k);
				layer.Name = item.SubItems[0].Text;
				col.Add(layer);
			}
			return col;
		}
		/// <summary>
		/// Commits the layer-changes to the graphcontrol
		/// </summary>
		public void UpdateLayerData()
		{
			
			LayerListItem item;
			for(int k=0; k<listView.Items.Count; k++)
			{
				item = listView.Items[k] as LayerListItem;
				if(item.Floating)
				{
					mLayers.Add(item.Layer);
					item.Floating = false;
				}
				else
					item.Commit();
				
			}
			for(int k=0; k<deletedItems.Count; k++)
			{
				//move the shapes to the default layer if it belonged to the layer
				foreach(Shape shape in mSite.Shapes)
					if(shape.Layer==deletedItems[k])
						shape.SetLayer("Default");
				//finally, remove it from the graphcontrol
				mLayers.Remove(deletedItems[k]);
			}
		}



		private LayerListItem CreateItem(ref GraphLayer layer)
		{
//			string[] items = new string[6]{
//											  name,
//											  number.ToString(),
//											  visible.ToString(),
//											  locked.ToString(),
//											  usecolor.ToString(),
//											  opacity};
//			ListViewItem item = new ListViewItem(items);			
			LayerListItem item = new LayerListItem(ref layer);			
			return item;
		}

		

		private void NewLayerButton_Click(object sender, System.EventArgs e)
		{
			string name = string.Empty; 
		
				using(LayerEdit edit = new LayerEdit(name))
				{
					DialogResult res =  edit.ShowDialog(this);
					if(res==DialogResult.OK)
						name = edit.LayerName;
					else
						return;
				}
			GraphLayer newlayer = new GraphLayer(name);
			
			LayerListItem item =  CreateItem(ref newlayer);
			item.Floating = true; //uncommitted to the graphcontrol until clicked OK or Apply
			listView.Items.Add(item);
			listView.SelectedItems.Clear();
			listView.Items[item.Index].Selected = true;
			
		}

		private void RenameButton_Click(object sender, System.EventArgs e)
		{
			string name; 
			if(listView.SelectedItems.Count>0)
			{
				name = listView.SelectedItems[0].SubItems[0].Text;
				using(LayerEdit edit = new LayerEdit(name))
				{
					DialogResult res =  edit.ShowDialog(this);
					if(res==DialogResult.OK)
						name = edit.LayerName;
					else
						return;
				}
				(listView.SelectedItems[0] as LayerListItem).Name = name;
			}


		}

		private void listView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(listView.SelectedIndices.Count>0)
			{
				int index = listView.SelectedIndices[0];				
				SetAdditionals(listView.Items[index] as LayerListItem);
			}
			else
				CurrentColor.BackColor = Color.Empty;
		}
		private void SetAdditionals(LayerListItem item)
		{
				TransGauge.Value = 100 - item.Opacity;
				TransText.Text = TransGauge.Value.ToString() + "%";
				CurrentColor.BackColor = item.LayerColor;
		}

		private void TransGauge_Scroll(object sender, System.EventArgs e)
		{
			if(listView.SelectedItems.Count>0)
				(listView.SelectedItems[0] as LayerListItem).Opacity =100-TransGauge.Value;
			TransText.Text = TransGauge.Value.ToString() + "%";
		}

		private void TransText_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char) Keys.Enter)
			{
				//is it a valid value?
				try
				{
					string input = TransText.Text.Trim();
					input = input.Replace("%","").Trim();
					int val = int.Parse(input);
					TransGauge.Value = val;
					TransText.Text = val.ToString() + "%";
					(listView.SelectedItems[0] as LayerListItem).Text =Convert.ToString(100-val);
				}
				catch
				{
					MessageBox.Show("This is not a valid value for the transparency.","Not valid", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
		}

		private void ChooseColorButton_Click(object sender, System.EventArgs e)
		{
			ColorDialog dia = new ColorDialog();
			dia.Color = CurrentColor.BackColor;
			DialogResult res = dia.ShowDialog(this);
			if(res==DialogResult.OK)
			{
				CurrentColor.BackColor = dia.Color;
				if(listView.SelectedItems.Count>0)
					(listView.SelectedItems[0] as LayerListItem).LayerColor = dia.Color;
			}

		}

		/// <summary>
		/// Removes a layer from the list.
		/// Note that this is only committed when the UpdateLayerData  method is called.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RemoveButton_Click(object sender, System.EventArgs e)
		{
			if(listView.SelectedItems.Count>0)
			{
				LayerListItem item = listView.SelectedItems[0] as LayerListItem;
				deletedItems.Add(item.Layer);
				listView.Items.Remove(item);
			}
		}

		private void listView_Click(object sender, System.EventArgs e)
		{
		
		}

		private void checkbox_Click(object sender, EventArgs e)
		{
			try
			{
				CheckBox chk = sender as CheckBox;
				if(chk==null) return;
				CheckBoxTag item = (CheckBoxTag) chk.Tag ;
				
				switch(item.Name)
				{
					case "Visible":
						item.Item.Visible = chk.Checked;
						break;
					case "Locked":
						item.Item.Locked = chk.Checked;
						break;
					case "UseColor":
						item.Item.UseColor = chk.Checked;
						break;
				}
			}
			catch
			{}

		}
#endregion
	}

	/// <summary>
	/// Utility class to put in the Tag-member of the listview checkboxes,
	/// it references the listitem and the property being edit of the layer.
	/// </summary>
	public struct CheckBoxTag
	{
		/// <summary>
		/// a list item
		/// </summary>
		private LayerListItem mItem;
		/// <summary>
		/// the name
		/// </summary>
		private string mName;

		/// <summary>
		/// Gets or sets the name
		/// </summary>
		public string Name
		{
			get{return mName;}
			set{mName = value;}
		}

		/// <summary>
		/// Gets or sets the item
		/// </summary>
		public LayerListItem Item
		{
			get{return mItem;}
			set{mItem = value;}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="item"></param>
		/// <param name="name"></param>
		public CheckBoxTag(LayerListItem item, string name)
		{
			mItem = item;
			mName = name;
		}

		/// <summary>
		/// Overrides the base method
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if(typeof(LayerListItem).IsInstanceOfType(obj))
				return ((CheckBoxTag) obj).Name==mName;
			else
				return false;
		}

		/// <summary>
		/// Overrides the base method 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return mName.GetHashCode();
		}


	}


}

