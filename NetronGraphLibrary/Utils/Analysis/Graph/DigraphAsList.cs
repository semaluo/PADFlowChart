using System;
using System.Collections;
namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Digraph structure based on a list
	/// </summary>
	public class DigraphAsLists : GraphAsLists, IDigraph, IGraph, IContainer, IComparable, IEnumerable
	{

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="size">The number of vertices</param>
		public DigraphAsLists(int size) : base(size)
		{
		}

		
	}
}
