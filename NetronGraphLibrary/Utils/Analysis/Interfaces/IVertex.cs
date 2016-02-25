using System;
using System.Collections;
namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Summary description for IVertex.
	/// </summary>
	public interface IVertex : IComparable
	{
		/// <summary>
		/// The unique number of this vertex
		/// </summary>
		int Number { get; }
		/// <summary>
		/// A weight factor this vertex has
		/// </summary>
		object Weight { get; }

		/// <summary>
		/// The edges finishing into this vertex
		/// </summary>
		IEnumerable IncidentEdges { get; }
		/// <summary>
		/// The set of edges leaving the vertex
		/// </summary>
		IEnumerable EmanatingEdges { get; }
		/// <summary>
		/// The vertices with an arrow towards this vertex
		/// </summary>
		IEnumerable Predecessors { get; }
		/// <summary>
		/// The vertices connected to this one with an arrow leaving this vertex
		/// </summary>
		IEnumerable Successors { get; }
	}
}
