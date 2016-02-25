using System;

namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Edge interface
	/// </summary>
	public interface IEdge : IComparable
	{
		/// <summary>
		/// The head or from vertex
		/// </summary>
		IVertex V0 { get; }
		/// <summary>
		/// The tail or to vertex
		/// </summary>
		IVertex V1 { get; }
		/// <summary>
		/// A weight
		/// </summary>
		object Weight { get; }
		/// <summary>
		/// Whether this is a directed edge
		/// </summary>
		bool IsDirected { get; }
		/// <summary>
		/// Returns the head or tail if the given vertex is part this edge
		/// </summary>
		/// <param name="vertex"></param>
		/// <returns></returns>
		IVertex MateOf(IVertex vertex);
	}
}
