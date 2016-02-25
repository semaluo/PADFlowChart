using System;
using System.Windows.Forms;
using Netron.GraphLib.Configuration;
namespace Netron.GraphLib.UI
{
	/// <mSummary>
	/// Custom TreeNode implementation for the shapes-viewer
	/// </mSummary>
	public class GraphTreeNode : TreeNode
	{
		#region Fields
		/// <summary>
		/// the summary
		/// </summary>
		protected ShapeSummary mSummary;

		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the shape summary
		/// </summary>
		public ShapeSummary Summary
		{
			get{return this.mSummary;}
			set{mSummary = value;}
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		public GraphTreeNode()	{}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="mSummary"></param>
		public GraphTreeNode(ShapeSummary mSummary)
		{
			this.mSummary = mSummary;
			this.Text = mSummary.Name;
		}
		#endregion
		
	}
}
