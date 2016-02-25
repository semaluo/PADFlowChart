using System;

namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Interface of a directed graph
	/// </summary>
	public interface IDigraph : IGraph
	{
		/// <summary>
		/// Whether the graph is strongly connected
		/// </summary>
		bool IsStronglyConnected { get; }
		/// <summary>
		/// A topological sort is an ordering of the nodes of a directed graph. This traversal visits the nodes of a directed graph in the order specified by a topological sort. 
		/// </summary>
		/// <param name="visitor"></param>
		void TopologicalOrderTraversal(IVisitor visitor);
	}
}
