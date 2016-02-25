using System;
using System.Diagnostics;
using System.Windows.Forms;
using Netron.GraphLib.Configuration;
namespace Netron.GraphLib.UI
{
	/// <mSummary>
	/// 
	/// </mSummary>
	public class GraphMenuItem : MenuItem
	{
		#region Fields
		/// <summary>
		/// pointer the the Click handler
		/// </summary>
		EventHandler mHandler = null;

		

		
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
			get{return mSummary;}
			set{mSummary = value;}
		}
		/// <summary>
		/// Gets the Click event handler
		/// <seealso cref="CategoryMenuItem.CloneMenu"/>
		/// </summary>
		public EventHandler ClickHandler
		{
			get{return mHandler;}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Constructor
		/// </summary>
		public GraphMenuItem()
		{
					
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="summary"></param>
		public GraphMenuItem(ShapeSummary summary):base(summary.Name)
		{
			this.mSummary = summary;			
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="mSummary"></param>
		/// <param name="handler"></param>
		public GraphMenuItem(ShapeSummary mSummary, EventHandler handler) : base(mSummary.Name,handler)
		{
			this.mSummary = mSummary;
			this.mHandler = handler;			
			
		}
		#endregion

	}
}
