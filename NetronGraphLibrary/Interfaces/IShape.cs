using System;
using System.Drawing;
using System.Windows.Forms;
namespace Netron.GraphLib.Interfaces
{
	/// <summary>
	/// The Shape interface
	/// </summary>
	public interface IShape : ILayoutElement, IEntity
	{
		#region Properties
		/// <summary>
		/// Gets or sets the z-order of the shape
		/// </summary>
		int ZOrder {get; set;}		
		/// <summary>
		/// Gets or sets whether the shape is resizable
		/// </summary>
		bool IsResizable {get; set;}

		/// <summary>
		/// Gets or sets whether the shape can be moved around.
		/// Note that the Fixed property is used to set whether the shape
		/// participates in the layout-process.
		/// </summary>
		bool CanMove {get; set;}
		/// <summary>
		/// Gets or sets the color of the shape
		/// </summary>
		Color ShapeColor {get; set;}
		/// <summary>
		/// Gets or sets the location of the shape on the canvas
		/// </summary>
		PointF Location {get; set;}
		#endregion

		#region Methods
		/// <summary>
		/// Returns a thumbnail to be shown in the shape-viewer
		/// </summary>
		/// <returns></returns>
		Bitmap GetThumbnail();
		/// <summary>
		/// Adapts the shape's width and height to fit the text
		/// 
		/// </summary>
		/// <param name="square">true to make the shape square</param>
		void FitSize(bool square);
		
		/// <summary>
		/// Allows to extend the default menu on a per-shape basis
		/// </summary>
		/// <returns></returns>
		MenuItem[] ShapeMenu();
		/// <summary>
		/// Additional actions on key-down for a shape
		/// </summary>
		/// <param name="e"></param>
		void OnKeyDown(KeyEventArgs e);
		/// <summary>
		/// Additional actions on key-press for a shape
		/// </summary>
		/// <param name="e"></param>
		void OnKeyPress(KeyPressEventArgs e);


		#endregion
	}
}
