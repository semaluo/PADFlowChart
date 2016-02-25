using System;
using System.Text;
using System.Collections;
namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Implementation of a graph data structure as a matrix
	/// </summary>
	public class GraphAsMatrix : AbstractGraph
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
			private GraphAsMatrix graph;
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
					{
						return graph.matrix[v, w];
					}
				}
			}

			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="graph"></param>
			internal EdgeEnumerator(GraphAsMatrix graph)
			{
				v = -1;
				w = -1;

				this.graph = graph;
			}

			/// <summary>
			/// Moves over the edges via the upper half of entries above the diagonal since the graph is symmetric
			/// </summary>
			/// <returns></returns>
			public  virtual bool MoveNext()
			{
				//TODO: bit awkward logic, should be better
				bool flag;
				if(v==-1) v++;
				if(w==-1) w = v;
				for (w++; w < graph.mNumberOfVertices; w++)
				{
					if (graph.matrix[v, w] == null ) 
					{
						if(w==graph.NumberOfVertices-1) { v++; w=v;}
						if(v==graph.NumberOfVertices) break;
						continue;

					}
					Console.WriteLine("Current edge: [" + v + "," + w + "]");
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

		#region Emanating edge enumerator
		/// <summary>
		/// Emanating edge enumerator
		/// </summary>
		private class EmanatingEdgeEnumerator: IEnumerator
		{
			/// <summary>
			/// the inner graph
			/// </summary>
			private GraphAsMatrix graph;
			/// <summary>
			/// the start-vertex
			/// </summary>
			private int v;
			/// <summary>
			/// the end-vertex
			/// </summary>
			private int w;

			/// <summary>
			///  Gets the current element in the enumeration
			/// </summary>
			public  virtual object Current
			{
				get
				{
					if (w < 0)
					{
						throw new InvalidOperationException();
					}
					else
					{
						return graph.matrix[v, w];
					}
				}
			}

			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="graph"></param>
			/// <param name="v"></param>
			internal EmanatingEdgeEnumerator(GraphAsMatrix graph, int v)
			{
				w = -1;
				
				this.graph = graph;
				this.v = v;
			}

			/// <summary>
			/// Moves the enumeration pointer to the next element
			/// </summary>
			/// <returns></returns>
			public  virtual bool MoveNext()
			{
				bool flag;

				for (w++; w < graph.mNumberOfVertices; w++)
				{
					if (graph.matrix[v, w] == null)
					{
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
				w = -1;
			}
		}
		#endregion

		#region Incident edge enumerator
		/// <summary>
		/// Incident edge enumeration
		/// </summary>
		private class IncidentEdgeEnumerator: IEnumerator
		{
			/// <summary>
			/// inner graph
			/// </summary>
			private GraphAsMatrix graph;
			/// <summary>
			/// the start-vertex
			/// </summary>
			private int v;
			/// <summary>
			/// the end-vertex
			/// </summary>
			private int w;

			/// <summary>
			///  Gets the current element in the enumeration
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
					{
						return graph.matrix[v, w];
					}
				}
			}

			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="graph"></param>
			/// <param name="w"></param>
			internal IncidentEdgeEnumerator(GraphAsMatrix graph, int w)
			{
				v = -1;
	
				this.graph = graph;
				this.w = w;
			}

			/// <summary>
			/// Moves the enumeration pointer to the next element
			/// </summary>
			/// <returns></returns>
			public  virtual bool MoveNext()
			{

				bool flag;

				for (v++; v < graph.mNumberOfVertices; v++)
				{
					if (graph.matrix[v, w] == null && graph.matrix[w,v]==null) //TODO: check this logic!
					{
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
			}
		}
		#endregion

		#region Fields
		/// <summary>
		/// the 2x2 matrix of IEdge elements (adjacency matrix)
		/// </summary>
		protected IEdge[,] matrix;
		#endregion

		#region Properties
		/// <summary>
		/// Gets the enumerable edge collection
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
		/// Default ctor
		/// </summary>
		/// <param name="size"></param>
		public GraphAsMatrix(int size) : base(size)
		{
			matrix = new IEdge[(uint)size, (uint)size];
		}

		#endregion

		
		#region Methods
		/// <summary>
		/// Empties the graph
		/// </summary>
		public override void Purge()
		{
			for (int i = 0; i < mNumberOfVertices; i++)
			{
				for (int j = 0; j < mNumberOfVertices; j++)
				{
					matrix[i, j]= null;
				}
			}
			base.Purge();
		}

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
			if (i == j)
			{
				throw new ArgumentException("loops not allowed");
			}
			matrix[i, j]= edge;
			matrix[j, i]= edge; //TODO: the DigraphAsMatrix does'nt do this, of course
			
			mNumberOfEdges++;
			Console.WriteLine(mNumberOfEdges);
		}

		/// <summary>
		/// Gets the IEdge with the given end-points
		/// </summary>
		/// <param name="v"></param>
		/// <param name="w"></param>
		/// <returns></returns>
		public override IEdge GetEdge(int v, int w)
		{
			Bounds.Check(v, 0, mNumberOfVertices);
			Bounds.Check(w, 0, mNumberOfVertices);
			if (matrix[v, w] == null)
			{
				throw new ArgumentException("edge not found");
			}
			else
			{
				return matrix[v, w];
			}
		}

		/// <summary>
		/// Returns whether the edge with the given end-points is in the graph
		/// </summary>
		/// <param name="v"></param>
		/// <param name="w"></param>
		/// <returns></returns>
		public override bool IsEdge(int v, int w)
		{
			Bounds.Check(v, 0, mNumberOfVertices);
			Bounds.Check(w, 0, mNumberOfVertices);
			return (matrix[v, w] != null);
		}

		/// <summary>
		/// Returns an enumerable collection of emanating edges for the given vertex
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		protected override IEnumerable GetEmanatingEdges(int v)
		{
			return new Enumerable(new EmanatingEdgeEnumerator(this, v));
		}

		/// <summary>
		/// Returns an enumerable collection of incident edges for the given vertex
		/// </summary>
		/// <param name="w"></param>
		/// <returns></returns>
		protected override IEnumerable GetIncidentEdges(int w)
		{
			return new Enumerable(new IncidentEdgeEnumerator(this, w));
		}

		/// <summary>
		/// Not implemented yet
		/// </summary>
		/// <param name="arg"></param>
		/// <returns></returns>
		public override int CompareTo(object arg)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Prints the graph in the traditional matrix form
		/// </summary>
		/// <returns></returns>
		public string MatrixForm()
		{
			StringBuilder sb=new StringBuilder();
			for(int v=0;v<mNumberOfVertices; v++)
			{
				sb.Append("(");
				for(int w=0;w<mNumberOfVertices; w++)
				{
					sb.Append(matrix[v,w]==null? "0": "1");
					sb.Append(" ");
				}
				sb.Append(")");
				sb.Append(Environment.NewLine);
			}
			return sb.ToString();
		}

		#endregion
		
	
	}
}
