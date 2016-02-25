using System;
using System.Text;
using System.Collections;
namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Implementation of a graph based on a list structure
	/// </summary>
	public class GraphAsLists : AbstractGraph
	{
		#region Classes
		/// <summary>
		/// Encapsulates the edge enumerator
		/// </summary>
		private class EdgeEnumerator: IEnumerator
		{
			private GraphAsLists graph;

			private int v;

			private LinkedList.Element ptr;


			public  virtual object Current
			{
				get
				{
					if (ptr == null)
					{
						throw new InvalidOperationException();
					}
					else
					{
						return ptr.Datum;
					}
				}
			}

			internal EdgeEnumerator(GraphAsLists graph)
			{
				v = -1;
				ptr = null;
				
				this.graph = graph;
			}

			public  virtual bool MoveNext()
			{
				bool flag;

				if (ptr != null)
				{
					ptr = ptr.Next;
				}
				if (ptr != null)
				{
					flag = true; 
					return flag;
				}

				for (v++; v < graph.mNumberOfVertices; v++)
				{
					ptr = graph.adjacencyList[v].Head;
					if (ptr == null)
					{
						continue;
					}
					flag = true;
					return flag;
				}
				v = -1;
				return false;
			}



			public  virtual void Reset()
			{
				v = -1;
				ptr = null;
			}
		}


		/// <summary>
		/// Encapsulates the enumerator of emanating edges
		/// </summary>
		private class EmanatingEdgeEnumerator: IEnumerator
		{
			private GraphAsLists graph;

			private int v;

			private LinkedList.Element ptr;


			public  virtual object Current
			{
				get
				{
					if (ptr == null)
					{
						throw new InvalidOperationException();
					}
					else
					{
						return ptr.Datum;
					}
				}
			}

			internal EmanatingEdgeEnumerator(GraphAsLists graph, int v)
			{
				ptr = null;

				this.graph = graph;
				this.v = v;
			}

			public  virtual bool MoveNext()
			{
				if (ptr == null)
				{
					ptr = graph.adjacencyList[v].Head;
				}
				else
				{
					ptr = ptr.Next;
				}
				
				return (ptr != null);
			}

			public  virtual void Reset()
			{
				ptr = null;
			}
		}

		/// <summary>
		/// Encapsulates the enumerator of incident edges
		/// </summary>
		private class IncidentEdgeEnumerator: IEnumerator
		{
			private GraphAsLists graph;

			private int v;

			private LinkedList.Element ptr;

			private LinkedList list;

			public  virtual object Current
			{
				get
				{
					if (ptr == null)
					{
						throw new InvalidOperationException();
					}
					else
					{
						return ptr.Datum;
					}
				}
			}

			internal IncidentEdgeEnumerator(GraphAsLists graph, int v)
			{
				ptr = null;

				this.graph = graph;
				list = new LinkedList();
				
				
					IEnumerator numer =  graph.Edges.GetEnumerator();
					while(numer.MoveNext())
					{
						IEdge edge = numer.Current as IEdge;
						if(edge.V1.Number==v)
							list.Append(edge);
					}
				
				this.v = v;
			}

			public  virtual bool MoveNext()
			{
				if (ptr == null)
				{
					ptr = list.Head;
				}
				else
				{
					ptr = ptr.Next;
				}
				
				return (ptr != null);
			}

			public  virtual void Reset()
			{
				ptr = null;
			}
		}

		#endregion

		#region Fields
		/// <summary>
		/// the linked list upon which the structure is based
		/// </summary>
		protected LinkedList[] adjacencyList;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the edges of the graph
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
		/// The constructor
		/// </summary>
		/// <param name="size">the number of vertices</param>
		public GraphAsLists(int size) : base(size)
		{
			adjacencyList = new LinkedList[size];
			for (int i = 0; i < size; i++)
			{
				adjacencyList[i] = new LinkedList();
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Purges the graph
		/// </summary>
		public override void Purge()
		{
			for (int i = 0; i < mNumberOfVertices; i++)
			{
				adjacencyList[i].Purge();
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
			adjacencyList[i].Append(edge);
			mNumberOfEdges++;
		}

		/// <summary>
		/// Gets the edge between the given vertices
		/// </summary>
		/// <param name="v"></param>
		/// <param name="w"></param>
		/// <returns></returns>
		public override IEdge GetEdge(int v, int w)
		{
			Bounds.Check(v, 0, mNumberOfVertices);
			Bounds.Check(w, 0, mNumberOfVertices);
			for (LinkedList.Element element = adjacencyList[v].Head; element != null; element = element.Next)
			{
				IEdge edge1 = (IEdge)element.Datum;
				if (edge1.V1.Number == w)
				{
					return edge1;
				}
			}
			throw new ArgumentException("edge not found");
		}

		/// <summary>
		/// Returns true if there is an edge between the given vertices
		/// </summary>
		/// <param name="v"></param>
		/// <param name="w"></param>
		/// <returns></returns>
		/// <remarks>Thanks to Morton Mertner again here</remarks>
		public override bool IsEdge(int v, int w)
		{
			bool isEdge = false;
			Bounds.Check(v, 0, mNumberOfVertices);
			Bounds.Check(w, 0, mNumberOfVertices);
			for (LinkedList.Element element = adjacencyList[v].Head; element != null; element = element.Next)
			{
				isEdge |= ((IEdge)element.Datum).V1.Number == w;
				if( isEdge )
					break;
			}
			return isEdge;
		}


		/// <summary>
		/// Gets the incident edges of the given vertex
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		protected override IEnumerable GetIncidentEdges(int v)
		{
			return new  Enumerable(new IncidentEdgeEnumerator(this, v));
			
			
		}

		/// <summary>
		/// Gets the emanating edges of the given vertex
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		protected override IEnumerable GetEmanatingEdges(int v)
		{
			return new  Enumerable(new EmanatingEdgeEnumerator(this, v));
		}

		/// <summary>
		/// Implements the IComparable interface
		/// </summary>
		/// <param name="arg"></param>
		/// <returns></returns>
		public override int CompareTo(object arg)
		{
			throw new NotImplementedException();
		}
		/// <summary>
		/// Returns the adjacency matrix
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < this.mCount; i++)
			{
				sb.Append(adjacencyList[i].ToString() + Environment.NewLine);
			}
			return sb.ToString();
		}


		

		#endregion

		/// <summary>
		/// Prints the graph or adjacency matrix to matrix form
		/// </summary>
		/// <returns></returns>
public string MatrixForm()
{
	StringBuilder sb=new StringBuilder();
	for(int v=0;v<mNumberOfVertices; v++)
	{
		sb.Append("(");
		sb.Append(adjacencyList[v].ToString());
		sb.Append(")");
		sb.Append(Environment.NewLine); 
	}
	return sb.ToString();
}
	}
}
