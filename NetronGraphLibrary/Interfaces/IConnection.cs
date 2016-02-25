using System;
using System.Drawing;
using System.Windows.Forms;
using Netron.GraphLib.Configuration;
using Netron.GraphLib;
using System.Drawing.Drawing2D;
namespace Netron.GraphLib.Interfaces
{
	/// <summary>
	/// Connection interface
	/// </summary>
	public interface IConnection : IEntity
	{
		#region Properties

		
		/// <summary>
		/// Gets or sets the line-path (rectangular, Bezier...) of the connection
		/// </summary>
		string LinePath {get; set;}
		/// <summary>
		/// Gets or sets the line-style (dashed, continuous...) of the connection
		/// </summary>
		DashStyle LineStyle {get; set;}
		/// <summary>
		/// Gets or sets the line-end (arrows etc.) of the connection
		/// </summary>
		ConnectionEnd LineEnd {get; set;}
		/// <summary>
		/// Gets or sets the line-color of the connection
		/// </summary>
		Color LineColor {get; set;}
		/// <summary>
		/// Gets or sets the start-connector of the connection
		/// </summary>
		Connector From {get; set;}
		/// <summary>
		/// Gets or sets the end-connector of the connection
		/// </summary>
		Connector To {get; set;}
		#endregion

		#region Methods
		/// <summary>
		/// Adds a connection point to the collection of intermediate connection points
		/// </summary>
		/// <param name="point"></param>
		void AddConnectionPoint(PointF point);
		/// <summary>
		/// Removes an intermediate connection point
		/// </summary>
		/// <param name="point"></param>
		void RemoveConnectionPoint(PointF point);
		#endregion

		
	}
}
