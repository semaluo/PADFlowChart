using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using Netron.GraphLib.Configuration;
using System.Windows.Forms;
using System.ComponentModel;
namespace Netron.GraphLib.UI
{
	/// <summary>
	/// A tabpage implementation for the ShapeViewer
	/// </summary>
	[ToolboxItem(false)]
	public class ShapesTab : System.Windows.Forms.TabPage
	{
		#region Events
		/// <summary>
		/// Occurs when te description is shown
		/// </summary>
		public event ItemDescription ShowDescription;
		#endregion

		#region Fields
		/// <summary>
		/// the inner listview
		/// </summary>
		protected ListView listView;
		/// <summary>
		/// the collection of shape summaries
		/// </summary>
		protected ShapeSummaryCollection summaries;
		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the view or the representation of the shapes
		/// </summary>
		public View View
		{
			get{return listView.View;}
			set{listView.View = value;}
		}
		/// <summary>
		/// the image-list of large images
		/// </summary>
		public System.Windows.Forms.ImageList LargeImageList
		{
			get{return this.listView.LargeImageList;}
			set{this.listView.LargeImageList=value;}
		}

		#endregion

		#region constructor
		/// <summary>
		/// Constructor
		/// </summary>
		public ShapesTab()
		{
			listView = new ListView();
			listView.Dock=DockStyle.Fill;
			listView.BorderStyle=System.Windows.Forms.BorderStyle.FixedSingle;
			listView.MouseDown+=new MouseEventHandler(ListViewMouseDown);
			listView.Size=new Size(100,100);
			listView.Location = new Point(0,0);
			listView.View=View.LargeIcon;
			listView.Name="";
			listView.MultiSelect=false;
			listView.Activation = ItemActivation.OneClick;
			listView.ForeColor=Color.DarkGreen;
			this.listView.MouseMove+=new MouseEventHandler(listView_MouseMove);
			this.Controls.Add(listView);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="tabText"></param>
		public ShapesTab(string tabText)
		{
			listView = new ListView();
			listView.Dock=DockStyle.Fill;
			listView.BorderStyle=System.Windows.Forms.BorderStyle.FixedSingle;
			listView.MouseDown+=new MouseEventHandler(ListViewMouseDown);
			this.Text=tabText;
			listView.Size=new Size(100,100);
			listView.Location = new Point(0,0);
			listView.View=View.LargeIcon;
			listView.Name="";
			listView.Activation = ItemActivation.OneClick;
			listView.ForeColor=Color.DarkGreen;
			listView.MultiSelect=false;
			this.listView.MouseMove+=new MouseEventHandler(listView_MouseMove);
			this.Controls.Add(listView);
			//AddDummy();
		}
		#endregion

		#region Methods
		/// <summary>
		/// Adds an item to the list
		/// </summary>
		/// <param name="text"></param>
		public void AddItem(string text)
		{
			this.listView.Items.Add(text,0);
			
		}
		/// <summary>
		/// Adds an item to the list
		/// </summary>
		/// <param name="summary"></param>
		public void AddItem(ShapeSummary summary)
		{
				this.listView.Items.Add(new ShapesTabItem(summary,0));
			
		}
		/// <summary>
		/// Adds an item to the list
		/// </summary>
		/// <param name="summary"></param>
		/// <param name="imageIndex"></param>
		public void AddItem(ShapeSummary summary, int imageIndex)
		{			
			this.listView.Items.Add(new ShapesTabItem(summary,imageIndex));

		}
		

		/// <summary>
		/// Only useful for debugging
		/// </summary>
		private void AddDummy()
		{
			this.listView.Items.Add(new ListViewItem("Item1",0));
			this.listView.Items.Add(new ListViewItem("Item2",1));
			this.listView.Items.Add(new ListViewItem("Item3",2));
		}
		private void ListViewMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{

			ListViewItem testitem = this.listView.GetItemAt(e.X,e.Y);
			if(testitem != null)			
			{
				testitem.Selected = true;
				try
				{
					ShapesTabItem item = testitem as ShapesTabItem;
					if(item != null)
					{
						this.DoDragDrop(item.Summary, DragDropEffects.Copy);
					}
				}
				catch(Exception exc)
				{
					Trace.WriteLine(exc.Message,"ShapesTab.ListViewMouseDown");
				}
			}
			//this.Invalidate();
		}

		

		private void listView_MouseMove(object sender, MouseEventArgs e)
		{
			ListViewItem testitem = this.listView.GetItemAt(e.X,e.Y);
			if(testitem != null)			
			{
				testitem.Selected = true;
				try
				{
					ShapesTabItem item = testitem as ShapesTabItem;
					if(item != null)
					{
						RaiseShowDescription(item.Summary.Description);
						//Trace.WriteLine(item.Summary.Description);
					}
				}
				catch(Exception exc)
				{
					Trace.WriteLine(exc.Message,"ShapesTab.listView_MouseMove");
				}

			}
			//this.Invalidate();
		}

		private void RaiseShowDescription(string message)
		{
			if(ShowDescription != null)
				ShowDescription(this,new InfoEventArgs(message));
		}
		#endregion
	}
}
