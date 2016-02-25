using System;

namespace Netron.GraphLib.Interfaces
{
	/// <summary>
	/// Describes a collection of shapes and connections;
	/// this can be the whole GraphAbstract or a subset of a diagram.
	/// In general, anything that can be (de)serialized to/from the diagram.
	/// </summary>
	public interface IEntityBundle
	{
		/// <summary>
		/// Gets the connection collection of the bundle
		/// </summary>
		ConnectionCollection Connections {get;}
		/// <summary>
		/// Gets the shape collection of the bundle
		/// </summary>
		ShapeCollection Shapes {get;}
		
	}
}
