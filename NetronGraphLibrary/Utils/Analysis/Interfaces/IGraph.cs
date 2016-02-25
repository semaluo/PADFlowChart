using System;
using System.Collections;
namespace Netron.GraphLib.Analysis
{

	/// <summary>
	/// Interface of a graph object
	/// </summary>
	public interface IGraph : IContainer
	{
		/// <summary>
		/// The number of edges this graph has
		/// </summary>
		int NumberOfEdges { get; }
		/// <summary>
		/// The number of vertices of this graph
		/// </summary>
		int NumberOfVertices { get; }
		/// <summary>
		/// Whether this is a directed graph
		/// </summary>
		bool IsDirected { get; }
		/// <summary>
		/// Adds a vertex to the vertices
		/// </summary>
		/// <param name="v"></param>
		void AddVertex(int v);
		/// <summary>
		/// Adds a weighted vertex to the vertices
		/// </summary>
		/// <param name="v"></param>
		/// <param name="weight"></param>
		void AddVertex(int v, object weight);
		/// <summary>
		/// Gets a vertex from this graph
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		IVertex GetVertex(int v);
		/// <summary>
		/// Adds an edge to this graph
		/// </summary>
		/// <param name="v"></param>
		/// <param name="w"></param>
		void AddConnection(int v, int w);
		/// <summary>
		/// Adds a weighted edge to this graph
		/// </summary>
		/// <param name="v"></param>
		/// <param name="w"></param>
		/// <param name="weight"></param>
		void AddConnection(int v, int w, object weight);
		/// <summary>
		/// Gets an edge from the edges
		/// </summary>
		/// <param name="v"></param>
		/// <param name="w"></param>
		/// <returns></returns>
		IEdge GetEdge(int v, int w);
		/// <summary>
		/// Given two vertices, returns an edge (if any)
		/// </summary>
		/// <param name="v"></param>
		/// <param name="w"></param>
		/// <returns></returns>
		bool IsEdge(int v, int w);
		/// <summary>
		/// Whether the graph is connected
		/// </summary>
		bool IsConnected { get; }
		/// <summary>
		/// Whether the graph is cyclic
		/// </summary>
		bool IsCyclic { get; }
		/// <summary>
		/// Gets all vertices
		/// </summary>
		IEnumerable Vertices { get; }
		/// <summary>
		/// Gets all edges
		/// </summary>
		IEnumerable Edges { get; }
		/// <summary>
		/// This methods accepts two arguments--a IPrePostVisitor and an integer. The integer specifies the starting vertex for a depth-first traversal of the graph. 
		/// </summary>
		/// <param name="visitor"></param>
		/// <param name="start"></param>
		void DepthFirstTraversal(IPrePostVisitor visitor, int start);
		/// <summary>
		/// This methods accepts two arguments--a IVisitor and an integer. The integer specifies the starting vertex for a breadth-first traversal of the graph.
		/// </summary>
		/// <param name="visitor"></param>
		/// <param name="start"></param>
		void BreadthFirstTraversal(IPrePostVisitor visitor, int start);
	}
}
