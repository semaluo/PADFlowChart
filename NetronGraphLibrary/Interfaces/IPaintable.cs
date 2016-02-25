using System;
using System.Drawing;
using System.Windows.Forms;
namespace Netron.GraphLib.Interfaces
{
	/// <summary>
	/// Describes a paintable element of the graph-control
	/// </summary>
	public interface IPaintable
	{
		/// <summary>
		/// Gets or sets the site (or graph control) to which the entity belongs
		/// </summary>
		IGraphSite Site {get; set;}

		
		/// <summary>
		/// Invalidating/refreshes part or all of a control
		/// </summary>
		void Invalidate();

		/// <summary>
		/// Paints the entity on the canvas
		/// </summary>
		/// <param name="g"></param>
		void Paint(Graphics g);
	}
}
