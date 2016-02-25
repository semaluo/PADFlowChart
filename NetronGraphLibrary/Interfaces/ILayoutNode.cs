using System;

namespace Netron.GraphLib.Interfaces
{
	/// <summary>
	/// This interfaces lays out the basic elements necessary to participate in a layout procedure
	/// </summary>
	public interface ILayoutElement
	{
		/// <summary>
		/// Gets or sets the x-coordinate of the shape
		/// </summary>
		float X {get;set;}
		/// <summary>
		/// Gets or sets the y-coordinate of the shape
		/// </summary>
		float Y {get;set;}
		/// <summary>
		/// Gets or sets an infinitesimal change in the x-direction
		/// </summary>
		double dx {get;set;}
		/// <summary>
		/// Gets or sets an infinitesimal change in the y-direction
		/// </summary>
		double dy {get;set;}
		/// <summary>
		/// Gets or sets whether the shape participates to the layout process
		/// </summary>
		bool IsFixed {get;set;}
	}
}
