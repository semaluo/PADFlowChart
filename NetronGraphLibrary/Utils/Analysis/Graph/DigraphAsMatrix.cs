using System;
using System.Collections;
namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Implementation of a digraph as a matrix
	/// </summary>
	public class DigraphAsMatrix : GraphAsMatrix, IDigraph, IGraph, IContainer, IComparable, IEnumerable
	{

		#region Edge enumerator
		/// <summary>
		/// Edge enumerator
		/// </summary>
		private class EdgeEnumerator: IEnumerator
		{
			/// <summary>
			/// the inner graph
			/// </summary>
			private DigraphAsMatrix graph;
			/// <summary>
			/// the start-vertex
			/// </summary>
			private int v;	
			/// <summary>
			/// the end-vertex
			/// </summary>
			private int w;

			/// <summary>
			/// Gets the current element in the enumeration
			/// </summary>
			public  virtual object Current
			{
				get
				{
					if (v < 0)
					{
						throw new InvalidOperationException();
					}
					else
					{Console.WriteLine("Current edge: [" + v + "," + w + "]");
						return graph.matrix[v, w];
						
					}
				}
			}

			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="graph"></param>
			internal EdgeEnumerator(DigraphAsMatrix graph)
			{
				v = -1;
				w = -1;

				this.graph = graph;
			}

			/// <summary>
			/// Moves over all edges
			/// <seealso cref="GraphAsMatrix"/>
			/// </summary>
			/// <returns></returns>
			public  virtual bool MoveNext()
			{

				bool flag;
				if(v==-1) v++;
				for (w++; w < graph.NumberOfVertices; w++)
				{
					if (graph.matrix[v, w] == null) 
					{
						if(w==graph.NumberOfVertices-1) { v++; w=-1;}
						if(v==graph.NumberOfVertices) break;
						continue;
					}
					flag = true;
					return flag;
				}
				w = -1;
				flag = false;
				return flag;


			}
			/// <summary>
			/// Resets the enumeration pointer
			/// </summary>
			public  virtual void Reset()
			{
				v = -1;
				w = -1;
			}
		}

		#endregion
		
		#region Properties
		/// <summary>
		/// Gets whether the graph is a tree
		/// </summary>
		public bool IsTree
		{
			get
			{
				throw new NotImplementedException();
				//				CountingVisitor countingVisitor = new CountingVisitor();
				//				this.DepthFirstTraversal(countingVisitor,0);
				//				if(countingVisitor.Count==this.mNumberOfEdges)
				//					return true;
				//				else
				//					return false;
			}
		}

		/// <summary>
		/// Gets the enumerable collection of edges
		/// </summary>
		public override IEnumerable Edges
		{
			get
			{
				return new Enumerable(new EdgeEnumerator(this));
			}
		}

		#endregion
		
		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="size"></param>
		public DigraphAsMatrix(int size) : base(size)
		{
		}

		#endregion


		#region Methods
		/// <summary>
		/// Adds an edge to the graph
		/// </summary>
		/// <param name="edge"></param>
		protected override void AddConnection(IEdge edge)
		{
			int i = edge.V0.Number;
			int j = edge.V1.Number;
			if (matrix[i, j] != null)
			{
				throw new ArgumentException("duplicate edge");
			}
			matrix[i, j]= edge;
			mNumberOfEdges++;
		}

		#endregion
		
	
	}
}
