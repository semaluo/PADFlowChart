using System;
using Netron.GraphLib.UI;
using Netron.GraphLib.Configuration;
using System.Windows.Forms;
namespace Netron.GraphLib.UI
{
	/// <mSummary>
	/// ListViewItem in the shapes-viewer
	/// </mSummary>
	public class ShapesTabItem : ListViewItem
	{
		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		public ShapesTabItem()
		{
			this.ImageIndex = 0; //there is a default image in slot 0
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="mSummary"></param>
		/// <param name="imageIndex"></param>
		public ShapesTabItem(ShapeSummary mSummary, int imageIndex)
		{
			this.mSummary=mSummary;
			this.ImageIndex=imageIndex;
			this.Text = mSummary.Name;
			
		}

		#endregion
		
		#region Fields
		/// <summary>
		/// the summary
		/// </summary>
		protected ShapeSummary mSummary = null;
	
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the ShapeSummary
		/// </summary>
		public ShapeSummary Summary
		{
			get{return mSummary;}
			set{mSummary = value;}
		}
			
		#endregion
	}
}

