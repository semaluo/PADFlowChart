using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Reflection;
using System.Configuration;
using System.IO;
using System.Diagnostics;
using Netron.GraphLib.Attributes;
using Netron.GraphLib.Configuration;

namespace Netron.GraphLib.UI
{
	/// <summary>
	/// UserControl listing in two ways the available/loaded shapes.
	/// </summary>
	[ToolboxItem(true)]
	public class GraphShapesView : System.Windows.Forms.UserControl, ISupportInitialize
	{		
		#region Fields
		/// <summary>
		/// the shape libraries
		/// </summary>
		protected GraphObjectsLibraryCollection libraries;
		/// <summary>
		/// the description label
		/// </summary>
		private System.Windows.Forms.Label DescriptionLabel;
		/// <summary>
		/// show-listview button
		/// </summary>
		private System.Windows.Forms.Button ShowListView;
		/// <summary>
		/// show-tree button
		/// </summary>
		private System.Windows.Forms.Button ShowTree;
		/// <summary>
		/// the main panel
		/// </summary>
		private System.Windows.Forms.Panel MainPanel;
		/// <summary>
		/// the treeview
		/// </summary>
		private System.Windows.Forms.TreeView treeView;
		/// <summary>
		/// the tabcontrol
		/// </summary>
		private System.Windows.Forms.TabControl tabControl;
		/// <summary>
		/// the images
		/// </summary>
		protected ImageList imageList;
		/// <summary>
		/// the description panel
		/// </summary>
		private System.Windows.Forms.Panel panelDescription;
		/// <summary>
		/// the lower splitter
		/// </summary>
		private System.Windows.Forms.Splitter lowerSplitter;
		/// <summary>
		/// the buttons panel
		/// </summary>
		private System.Windows.Forms.Panel panelButtons;
		/// <summary>
		/// current mView of the list
		/// </summary>
		protected View mView = View.LargeIcon;
		#endregion
		
		#region Properties
		/// <summary>
		/// Gets or sets the representation of the listview
		/// </summary>
		public View View
		{
			get{return mView;}
			set{
				mView = value;
				for(int k = 0 ; k<this.tabControl.TabPages.Count; k++)
				{
					(this.tabControl.TabPages[k] as ShapesTab).View = mView;					
				}
			}
		}
		
		#endregion
		
		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		public GraphShapesView()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			imageList=new ImageList();
			
			this.libraries=new GraphObjectsLibraryCollection();
			
			imageList.Images.Add(this.GetDefaultThumbnail());
		}
		#endregion
		
		#region Methods
		/// <summary>
		/// Adds a shape library to the collection
		/// </summary>
		/// <param name="path"></param>
		public void AddLibrary(string path)
		{
			this.ImportEntities(path);
		}
		/// <summary>
		/// Loads the shapes from the assembly at the given path
		/// </summary>
		/// <param name="path"></param>
		protected void ImportEntities(string path)
		{
			GraphObjectsLibrary library = new GraphObjectsLibrary();
			ShapeSummary summary;
			ShapesTab tab;
			TreeNode categoryNode;
			library.Path = path;
			libraries.Add(library);
			int currentImageId;
			try
			{
				Directory.SetCurrentDirectory(Path.GetDirectoryName(Application.ExecutablePath));
				Assembly ass=Assembly.LoadFrom(path);
				if (ass==null) return;
				Type[] tps=ass.GetTypes();
			
				if (tps==null) return ;
				Shape shapeInstance=null;
				object[] objs;
				for(int k=0; k<tps.Length;k++) //loop over modules in assembly
				{
					
					if(!tps[k].IsClass) continue;		
					objs = tps[k].GetCustomAttributes(typeof(Netron.GraphLib.Attributes.NetronGraphShapeAttribute),false);
					if(objs.Length<1) continue;							
					//now, we are sure to have a shape object					
					
					try
					{
						//normally you'd need the constructor passing the Site but this instance will not actually live on the canvas and hence cause no problem
						//but you do need a ctor with no parameters!
						shapeInstance=(Shape) ass.CreateInstance(tps[k].FullName);
						NetronGraphShapeAttribute shapeAtts = objs[0] as NetronGraphShapeAttribute;
						summary = new ShapeSummary(path, shapeAtts.Key,shapeAtts.Name, shapeAtts.ShapeCategory, shapeAtts.ReflectionName, shapeAtts.Description);
						library.ShapeSummaries.Add(summary);

						#region For the listview
						tab=this.GetTab(summary.ShapeCategory);

						//if not override the Shape gives a default bitmap
						Bitmap bmp = null;
						try
						{
							bmp = shapeInstance.GetThumbnail();
						}
						catch
						{
							//if the resource was not well embedded we'll still add the item to the list, just to be nice...
						}
						imageList.ImageSize=new Size(48,48);
						imageList.ColorDepth=ColorDepth.Depth8Bit;
						if(bmp !=null)
						{
							
							//imageList.TransparentColor=Color.White;								
							currentImageId=imageList.Images.Add(bmp,Color.Empty);	
						}
						else
						{
							currentImageId = 0;
						}
						//this.pictureBox1.Image=bmp;
						//bmp.Save("c:\\temp\\test.gif",System.Drawing.Imaging.ImageFormat.Gif);
						tab.LargeImageList=imageList;
						tab.AddItem(summary,currentImageId);
						
						#endregion

						#region For the treeview
						categoryNode = this.GetTreeNode(summary.ShapeCategory);
						categoryNode.Nodes.Add(new GraphTreeNode(summary));

						#endregion
					}
					catch(Exception exc)
					{
						Trace.WriteLine(exc.Message,"GraphShapesView.ImportEntities");
						continue;
					}						
					
							
				}
				
			}
			catch(Exception exc)
			{
				Trace.WriteLine(exc.Message,"GraphShapesView.ImportEntities");
			}
		}

		/// <summary>
		/// Returns the default thumbnail
		/// </summary>
		/// <returns></returns>
		public virtual Bitmap GetDefaultThumbnail()
		{
			Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.GraphLib.Resources.UnknownShape.gif");
					
			Bitmap bmp= Bitmap.FromStream(stream) as Bitmap;
			stream.Close();
			stream=null;
			return bmp;
				 
		}
		/// <summary>
		/// Gets a the tabpage with the given name or a new one if it does not yet exist
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		private ShapesTab GetTab(string text)
		{
			ShapesTab tab = null;
			for(int k = 0 ; k<this.tabControl.TabPages.Count; k++)
			{
				tab = this.tabControl.TabPages[k] as ShapesTab;
				if(tab.Text == text) return tab;
			}
			//didn't find it, let's make one
			tab=new ShapesTab(text);
			tab.View = mView;
			this.tabControl.TabPages.Add(tab);
			tab.ShowDescription+=new ItemDescription(ShowDescription);
			return tab;

		}
		/// <summary>
		/// Gets the (first) node with the given text
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		private TreeNode GetTreeNode(string text)
		{
			TreeNode node = null;
			for(int k = 0; k<treeView.Nodes.Count; k++)
			{
				if(treeView.Nodes[k].Text == text) return treeView.Nodes[k];
			}
			node = new TreeNode(text);
			this.treeView.Nodes.Add(node);
			return node;

		}
		/// <summary>
		/// Adds a dummy (for testing purposes)
		/// </summary>
		/// <param name="tab"></param>
		/// <returns></returns>
		private ShapesTab AddDummyShapes(ShapesTab tab)
		{
			
			try
			{
				tab.LargeImageList=this.imageList;
				for(int k=0; k<5;k++)
				{
					imageList.ImageSize=new Size(48,48);
					imageList.ColorDepth=ColorDepth.Depth8Bit;
					tab.AddItem("Item" + k);
				}
			}
			catch(Exception exc)
			{
				System.Diagnostics.Trace.WriteLine(exc.Message,"GraphShapesView.AddDummyShapes");
			}
			return tab;
		}


		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				
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
			this.panelDescription = new System.Windows.Forms.Panel();
			this.DescriptionLabel = new System.Windows.Forms.Label();
			this.lowerSplitter = new System.Windows.Forms.Splitter();
			this.panelButtons = new System.Windows.Forms.Panel();
			this.ShowTree = new System.Windows.Forms.Button();
			this.ShowListView = new System.Windows.Forms.Button();
			this.MainPanel = new System.Windows.Forms.Panel();
			this.treeView = new System.Windows.Forms.TreeView();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.panelDescription.SuspendLayout();
			this.panelButtons.SuspendLayout();
			this.MainPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelDescription
			// 
			this.panelDescription.Controls.Add(this.DescriptionLabel);
			this.panelDescription.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panelDescription.Location = new System.Drawing.Point(0, 416);
			this.panelDescription.Name = "panelDescription";
			this.panelDescription.Size = new System.Drawing.Size(304, 80);
			this.panelDescription.TabIndex = 1;
			// 
			// DescriptionLabel
			// 
			this.DescriptionLabel.BackColor = System.Drawing.SystemColors.Control;
			this.DescriptionLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.DescriptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DescriptionLabel.Location = new System.Drawing.Point(0, 0);
			this.DescriptionLabel.Name = "DescriptionLabel";
			this.DescriptionLabel.Size = new System.Drawing.Size(304, 80);
			this.DescriptionLabel.TabIndex = 2;
			// 
			// lowerSplitter
			// 
			this.lowerSplitter.Cursor = System.Windows.Forms.Cursors.HSplit;
			this.lowerSplitter.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lowerSplitter.Location = new System.Drawing.Point(0, 413);
			this.lowerSplitter.Name = "lowerSplitter";
			this.lowerSplitter.Size = new System.Drawing.Size(304, 3);
			this.lowerSplitter.TabIndex = 2;
			this.lowerSplitter.TabStop = false;
			// 
			// panelButtons
			// 
			this.panelButtons.Controls.Add(this.ShowTree);
			this.panelButtons.Controls.Add(this.ShowListView);
			this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelButtons.Location = new System.Drawing.Point(0, 0);
			this.panelButtons.Name = "panelButtons";
			this.panelButtons.Size = new System.Drawing.Size(304, 32);
			this.panelButtons.TabIndex = 3;
			// 
			// ShowTree
			// 
			this.ShowTree.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.ShowTree.Location = new System.Drawing.Point(88, 4);
			this.ShowTree.Name = "ShowTree";
			this.ShowTree.TabIndex = 1;
			this.ShowTree.Text = "Tree View";
			this.ShowTree.Click += new System.EventHandler(this.ShowTree_Click);
			// 
			// ShowListView
			// 
			this.ShowListView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.ShowListView.Location = new System.Drawing.Point(8, 4);
			this.ShowListView.Name = "ShowListView";
			this.ShowListView.TabIndex = 0;
			this.ShowListView.Text = "List View";
			this.ShowListView.Click += new System.EventHandler(this.ShowListView_Click);
			// 
			// MainPanel
			// 
			this.MainPanel.Controls.Add(this.treeView);
			this.MainPanel.Controls.Add(this.tabControl);
			this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MainPanel.Location = new System.Drawing.Point(0, 32);
			this.MainPanel.Name = "MainPanel";
			this.MainPanel.Size = new System.Drawing.Size(304, 381);
			this.MainPanel.TabIndex = 4;
			// 
			// treeView
			// 
			this.treeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView.HotTracking = true;
			this.treeView.ImageIndex = -1;
			this.treeView.Location = new System.Drawing.Point(0, 0);
			this.treeView.Name = "treeView";
			this.treeView.SelectedImageIndex = -1;
			this.treeView.Size = new System.Drawing.Size(304, 381);
			this.treeView.TabIndex = 7;
			this.treeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TreeMouseDown);
			this.treeView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TreeMouseMove);
			// 
			// tabControl
			// 
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl.Location = new System.Drawing.Point(0, 0);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(304, 381);
			this.tabControl.TabIndex = 6;
			this.tabControl.Visible = false;
			// 
			// GraphShapesView
			// 
			this.Controls.Add(this.MainPanel);
			this.Controls.Add(this.panelButtons);
			this.Controls.Add(this.lowerSplitter);
			this.Controls.Add(this.panelDescription);
			this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "GraphShapesView";
			this.Size = new System.Drawing.Size(304, 496);
			this.panelDescription.ResumeLayout(false);
			this.panelButtons.ResumeLayout(false);
			this.MainPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region ISupportInitialize Members

		/// <summary>
		/// ISupportInitialize.BeginInit method
		/// </summary>
		public void BeginInit()
		{
		
		}
		/// <summary>
		/// ISupportInitialize.EndInit method
		/// </summary>
		public void EndInit()
		{
			
		}

		#endregion

		/// <summary>
		/// OnLoad actions
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			if(this.DesignMode)
			{
				this.tabControl.TabPages.Add(AddDummyShapes(new ShapesTab("Dummy")));
			}
			
			
			//this.Invalidate();
		}

		/// <summary>
		/// Loads the shapes from the libraries specified in the application configuration file.
		/// </summary>
		public void LoadLibraries()
		{
			ArrayList graphLibs = ConfigurationSettings.GetConfig("GraphLibs") as ArrayList;
			if(graphLibs.Count>0)
			{
				for(int k=0; k<graphLibs.Count;k++)
				{
					this.ImportEntities(graphLibs[k] as string);
				}
			}
		}

		/// <summary>
		/// Show the shape description in the lower part of the control
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ShowDescription(object sender, InfoEventArgs e)
		{
			this.DescriptionLabel.Text = e.Message;
		}

		/// <summary>
		/// Shows the listview
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ShowListView_Click(object sender, System.EventArgs e)
		{
			ShowAs(ShapesView.Icons);
		}
		/// <summary>
		/// Changes the view to the tree-mode
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ShowTree_Click(object sender, System.EventArgs e)
		{
			ShowAs(ShapesView.Tree);
		}

		/// <summary>
		/// Switches the view between tree and icons
		/// </summary>
		/// <param name="mView"></param>
		public void ShowAs(ShapesView mView)
		{
			switch(mView)
			{
			
				case ShapesView.Icons:
					this.treeView.Visible=false;
					this.tabControl.Visible = true;
					break;
				case ShapesView.Tree:
					this.treeView.Visible=true;
					this.tabControl.Visible = false;
					break;
			}
		}

		/// <summary>
		/// MouseDown handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TreeMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			TreeNode testitem = this.treeView.GetNodeAt(e.X,e.Y);
			if(testitem != null)			
			{				
				try
				{
					GraphTreeNode item = testitem as GraphTreeNode;
					if(item != null)
					{
						this.DoDragDrop(item.Summary, DragDropEffects.Copy);
					}
				}
				catch(Exception exc)
				{
					Trace.WriteLine(exc.Message,"GraphShapesView.TreeMouseDown");
				}
			}
			//this.Invalidate();
		}

		/// <summary>
		/// MouseMove handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TreeMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			TreeNode testitem = this.treeView.GetNodeAt(e.X,e.Y);
			if(testitem != null)			
			{				
				try
				{
					GraphTreeNode item = testitem as GraphTreeNode;
					if(item != null)
					{
						this.ShowDescription(this, new InfoEventArgs( item.Summary.Description));
					}
				}
				catch(Exception exc)
				{
					Trace.WriteLine(exc.Message,"GraphShapesView.TreeMouseMove");
				}
			}
		}
		#endregion
		
	
	}


	
}
