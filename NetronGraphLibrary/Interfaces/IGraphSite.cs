using System;
using System.Drawing;
using System.Collections;
using Netron.GraphLib.Configuration;
namespace Netron.GraphLib.Interfaces
{
	/// <summary>
	/// Interface of a graph site (control) 
	/// </summary>
	public interface IGraphSite
	{
		#region Properties
		/// <summary>
		/// Gets the layers defined in the control
		/// </summary>
		GraphLayerCollection Layers {get;}
		/// <summary>
		/// Gets the graph-abstract
		/// </summary>
		GraphAbstract Abstract {get;}
		/// <summary>
		/// Gets the libraries defined and loaded of custom objects
		/// </summary>
		GraphObjectsLibraryCollection Libraries{get;}
		/// <summary>
		/// Gets the collection of shapes active in the control
		/// </summary>
		ShapeCollection Shapes {get;}
		/// <summary>
		/// Gets the collection of connection active in the control
		/// </summary>
		ConnectionCollection Connections  {get;}
		/// <summary>
		/// Gets the size of the control
		/// </summary>
		Size Size {get;}
		/// <summary>
		/// Gets or sets the zoom-value
		/// </summary>
		float Zoom {get; set;}
		/// <summary>
		/// Gets or sets whether to restrict to the canvas
		/// </summary>
		bool RestrictToCanvas {get; set;}
		/// <summary>
		/// Gets or sets the AutoScrollPosition
		/// <seealso cref="System.Windows.Forms.ScrollableControl.AutoScrollPosition"/>
		/// </summary>
		/// 
		Point AutoScrollPosition {get; set;}
		/// <summary>
		/// Gets or sets whether tracking is on
		/// </summary>
		bool DoTrack {get; set;}
		/// <summary>
		/// Gets the width of the control
		/// </summary>
		int Width {get;}
		/// <summary>
		/// Gets the height of the control
		/// </summary>
		int Height {get;}
		/// <summary>
		/// Gets the Graphics object used by the control
		/// </summary>
		Graphics Graphics {get;}
		#endregion

		#region Methods

		#region Invalidate overloads
		/// <summary>
		/// Invalidates the given rectangle and its children
		/// </summary>
		/// <param name="r">A System.Drawing.Rectangle object that represents the region to invalidate. </param>
		/// <param name="b">invalidateChildren: true to invalidate the control's child controls; otherwise, false.</param>
		void Invalidate(Rectangle r, bool b);
		/// <summary>
		/// Invalidates the given rectangle
		/// </summary>
		/// <param name="r"></param>
		void Invalidate(Rectangle r);
		/// <summary>
		/// Invalidate the whole control
		/// </summary>
		void Invalidate();
		#endregion
		
		/// <summary>
		/// Let the site invalidate the rectangle
		/// </summary>
		/// <param name="rect">invalid rectangle</param>
		void InvalidateRectangle( Rectangle rect );

		/// <summary>
		/// Zooms a point
		/// </summary>
		Point ZoomPoint(Point originalPt);

		/// <summary>
		/// Unzooms a point.
		/// </summary>
		Point UnzoomPoint(Point originalPt);

		/// <summary>
		/// Zooms a rectangle.
		/// </summary>
		Rectangle ZoomRectangle(Rectangle originalRect);

		/// <summary>
		/// Unzooms a rectangle.
		/// </summary>
		Rectangle UnzoomRectangle(Rectangle originalRect);

		/// <summary>
		/// Zooms a point
		/// </summary>
		PointF ZoomPoint(PointF originalPt);

		/// <summary>
		/// Unzooms a point.
		/// </summary>
		PointF UnzoomPoint(PointF originalPt);

		/// <summary>
		/// Zooms a rectangle.
		/// </summary>
		RectangleF ZoomRectangle(RectangleF originalRect);

		/// <summary>
		/// Unzooms a rectangle.
		/// </summary>
		RectangleF UnzoomRectangle(RectangleF originalRect);

		/// <summary>
		/// Paints an arrow
		/// </summary>
		/// <param name="g"></param>
		/// <param name="startPoint"></param>
		/// <param name="endPoint"></param>
		/// <param name="lineColor"></param>
		/// <param name="filled"></param>
		/// <param name="showLabel"></param>
		void PaintArrow(Graphics g, PointF startPoint, PointF endPoint,Color lineColor, bool filled, bool showLabel);
		
		/// <summary>
		/// Gets the entity with the given UID
		/// </summary>
		/// <param name="UID"></param>
		/// <returns></returns>
		Entity GetEntity(string UID);
		
		/// <summary>
		/// Raises the ShowProps event
		/// </summary>
		/// <param name="props"></param>
		void RaiseShowProperties(PropertyBag props);
		
		/// <summary>
		/// Raises the OnShapeAdded event
		/// </summary>
		/// <param name="shape"></param>
		void RaiseOnShapeAdded(Shape shape);
		
		/// <summary>
		/// Raises the OnConnectionAdded event
		/// </summary>
		/// <param name="connection"></param>
		/// <param name="manual"></param>
		void RaiseOnConnectionAdded(Connection connection, bool manual);

        /// <summary>
        /// Raises the OnShowGraphLayers event
        /// </summary>
        void RaiseOnShowGraphLayers();
		/// <summary>
		/// Raises the OnShowPropertiesDialogRequest event
		/// </summary>
		void RaiseOnShowPropertiesDialogRequest();


		/// <summary>
		/// Gets the Summary of the given entity
		/// </summary>
		/// <param name="e"></param>
		/// <returns></returns>
		Summary GetSummary(Entity e);
		
		/// <summary>
		/// Returns the layers as a Attribute-array
		/// </summary>
		/// <returns></returns>
		Attribute[]	GetLayerAttributes();
		/// <summary>
		/// Sets the tooltip of the control
		/// </summary>
		/// <param name="tip"></param>
		void SetToolTip(string tip);

		#region Overloads of the output function
		/// <summary>
		/// Outputs info to the outside world
		/// </summary>
		/// <param name="obj"></param>
		void OutputInfo(object obj);
		/// <summary>
		/// Outputs info to the outside world
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="level"></param>
		void OutputInfo(object obj, OutputInfoLevels level);

		#endregion

		/// <summary>
		/// Opens a binary saved diagram
		/// </summary>
		/// <param name="fileName"></param>
		void Open(string fileName);
		
		#endregion

		
	}
}
