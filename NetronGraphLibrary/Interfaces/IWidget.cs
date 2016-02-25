using System;
using System.Drawing;
using System.Windows.Forms;
namespace Netron.GraphLib.Interfaces
{
	/// <summary>
	/// Describes the elements of a widget
	/// </summary>
	public interface IWidget : IPaintable
	{
		/// <summary>
		/// Gets the cursor when the mouse is hovering the given point in the entity
		/// </summary>
		/// <param name="p"></param>
		/// <returns></returns>
		Cursor GetCursor(PointF p);
		/// <summary>
		/// Says wether, for the given rectangle, the underlying shape is contained in it.
		/// </summary>
		/// <param name="r"></param>
		/// <returns></returns>
		bool Hit(RectangleF r);
		/// <summary>
		/// Handles the mouse down event on the widget
		/// </summary>
		/// <param name="p"></param>
		void OnMouseDown(PointF p);
		/// <summary>
		/// Handles the mouse move event on the widget
		/// </summary>
		/// <param name="p"></param>
		void OnMouseMove(PointF p);
		/// <summary>
		/// Reacts to the transmission event
		/// </summary>
		void OnTransmission();
	}
}
