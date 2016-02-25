using System;
using System.Collections;
using System.Text;
namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Abstract base class for the diverse graph implementations
	/// </summary>
	public abstract class AbstractGraph : AbstractContainer, IGraph, IContainer, IComparable, IEnumerable
	{

		#region Fields

		/// <summary>
		/// the number of vertices
		/// </summary>
		protected int mNumberOfVertices;
		/// <summary>
		/// the number of edges
		/// </summary>
		protected int mNumberOfEdges;
		/// <summary>
		/// the vertex collection
		/// </summary>
		protected IVertex[] vertex;
		/// <summary>
		/// whether the graph is cyclic (has cycles)
		/// </summary>
		protected bool mIsCyclic;

		#endregion

		#region Properties

		/// <summary>
		/// Gets an enumerator for the vertices of the graph
		/// </summary>
		public virtual IEnumerable Vertices
		{
			get
			{
				return new Enumerable(new VertexEnumerator(this));
			}
		}
		/// <summary>
		/// Gets an enumerator for the edges of the graph
		/// </summary>
		public abstract IEnumerable Edges
		{
			get;
		}

		/// <summary>
		/// Gets whether the graph is directed
		/// </summary>
		public  virtual bool IsDirected
		{
			get
			{
				return this is IDigraph;
			}
		}
		/// <summary>
		/// Gets the number of vertices
		/// </summary>
		public  virtual int NumberOfVertices
		{
			get
			{
				return mNumberOfVertices;
			}
		}

		/// <summary>
		/// Gets the number of edges
		/// </summary>
		public  virtual int NumberOfEdges
		{
			get
			{
				return mNumberOfEdges;
			}
		}

		/// <summary>
		/// Gets whether the graph is connected
		/// </summary>
		public  virtual bool IsConnected
		{
			get
			{
				CountingVisitor countingVisitor = new CountingVisitor();
				DepthFirstTraversal(new PreOrderVisitor(countingVisitor), 0);
				return countingVisitor.Count == mNumberOfVertices;
			}
		}

		/// <summary>
		/// Gets whether the graph is strongly connected
		/// </summary>
		public  virtual bool IsStronglyConnected
		{
			get
			{
				bool flag;

				for (int i = 0; i < mNumberOfVertices; i++)
				{
					CountingVisitor countingVisitor = new CountingVisitor();
					DepthFirstTraversal(new PreOrderVisitor(countingVisitor), i);
					if (countingVisitor.Count == mNumberOfVertices)
					{
						return true;
					}
					flag = false;
					return flag;
				}
				flag = true;
				return flag;
			}
		}

		/// <summary>
		/// Gets whether the graph is cyclic
		/// </summary>
		public  virtual bool IsCyclic
		{
			get
			{
				CountingVisitor countingVisitor = new CountingVisitor();
				TopologicalOrderTraversal(countingVisitor);
				mIsCyclic = countingVisitor.Count == mNumberOfVertices == false;
				return mIsCyclic;
			}
		}

		#endregion

		#region Constructor
		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="size">the size of the graph, i.e. the number of nodes</param>
		protected AbstractGraph(int size)
		{
			if(size<1)
				throw new Exception("The given size is not valid, the diagram has no nodes.");
			vertex = new IVertex[size];
			mCount = size;
		}
		#endregion

		#region Methods
		
		#region Abstract methods
		/// <summary>
		/// Gets an IEnumerable for the incident egdes of the given vertex. 
		/// These are the edges with their end-point in the given vertex.
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		protected abstract IEnumerable GetIncidentEdges(int v);
		/// <summary>
		/// Gets the IEnumerable for the emanating edges of the given vertex.
		/// There are the edges with their starting-point in the given vertex.
		/// </summary>
		/// <param name="v"></param>
		/// <returns></returns>
		protected abstract IEnumerable GetEmanatingEdges(int v);
		/// <summary>
		/// Adds an edge to the graph
		/// </summary>
		/// <param name="edge">an IEdge object</param>
		protected abstract void AddConnection(IEdge edge);
		/// <summary>
		/// Returns the edge, if any, with end-point and start-point given by the indeices.
		/// </summary>
		/// <param name="v"></param>
		/// <param name="w"></param>
		/// <returns></returns>
		public abstract IEdge GetEdge(int v, int w);
		/// <summary>
		/// Returns wether there is an edge between the two given vertex numbers.
		/// </summary>
		/// <param name="v">a vertex number</param>
		/// <param name="w">a vertex number</param>
		/// <returns></returns>
		public abstract bool IsEdge(int v, int w);
		#endregion

		#region AddVertex overloads
		/// <summary>
		/// Adds an  IVertex to the graph
		/// </summary>
		/// <param name="v"></param>
		protected virtual void AddVertex(IVertex v)
		{
			if (mNumberOfVertices == (int)vertex.Length)
			{
				throw new Netron.GraphLib.Analysis.ContainerEmptyException();
			}
			if (v.Number != mNumberOfVertices)
			{
				throw new ArgumentException("invalid vertex mNumber");
			}
			vertex[mNumberOfVertices] = v;
			mNumberOfVertices++;
		}

		/// <summary>
		/// Adds a weighted vertex to the graph
		/// </summary>
		/// <param name="v"></param>
		/// <param name="mWeight"></param>
		public virtual void AddVertex(int v, object mWeight)
		{
			AddVertex(new GraphVertex(this, v, mWeight));
		}

		/// <summary>
		/// Adds a vertex to the graph
		/// </summary>
		/// <param name="v"></param>
		public virtual void AddVertex(int v)
		{
			AddVertex(v, null);
		}
	
		#endregion

		#region AddConnection overloads
		/// <summary>
		/// Adds an edge to the graph
		/// </summary>
		/// <param name="v"></param>
		/// <param name="w"></param>
		/// <param name="mWeight"></param>
		public virtual void AddConnection(int v, int w, object mWeight)
		{
			AddConnection(new GraphEdge(this, v, w, mWeight));
			mIsCyclic |= v == w;
		}
		
		/// <summary>
		/// Adds an edge to the graph
		/// </summary>
		/// <param name="v"></param>
		/// <param name="w"></param>
		public virtual void AddConnection(int v, int w)
		{
			AddConnection(v, w, null);
		}

		#endregion
				
		#region Traversals
		/// <summary>
		/// Performs a DFT of the graph with the given visitor starting at the given vertex number.
		/// </summary>
		/// <param name="visitor">an IVisitor object</param>
		/// <param name="start">the index of the vertex where the visiting starts</param>
		public virtual void DepthFirstTraversal(IPrePostVisitor visitor, int start)
		{
			bool[] flags = new bool[mNumberOfVertices];
			for (int i = 0; i < mNumberOfVertices; i++)
			{
				flags[i] = false;
			}
			DepthFirstTraversal(visitor, vertex[start], flags);
		}

		/// <summary>
		/// Implements the DFT algorithm
		/// </summary>
		/// <param name="visitor"></param>
		/// <param name="v"></param>
		/// <param name="visited"></param>
		protected virtual void DepthFirstTraversal(IPrePostVisitor visitor, IVertex v, bool[] visited)
		{
			if (!visitor.IsDone)
			{
				visitor.PreVisit(v);
				visitor.Visit(v);
				visited[v.Number] = true;
				IEnumerator iEnumerator = v.Successors.GetEnumerator();
				try
				{
					while (iEnumerator.MoveNext())
					{
						IVertex vertex = (IVertex)iEnumerator.Current;
						if (!visited[vertex.Number])
						{
							DepthFirstTraversal(visitor, vertex, visited);
						}
					}
				}
				finally
				{
					IDisposable iDisposable = iEnumerator as IDisposable;
					if (iDisposable != null)
					{
						iDisposable.Dispose();
					}
				}
				visitor.PostVisit(v);
			}
		}

		/// <summary>
		/// Performs a BFT of the graph with the given visitor and starting at the given vertex number.
		/// </summary>
		/// <param name="visitor"></param>
		/// <param name="start"></param>
		public  virtual void BreadthFirstTraversal(IPrePostVisitor visitor, int start)
		{
			bool[] flags = new bool[mNumberOfVertices];
			for (int i = 0; i < mNumberOfVertices; i++)
			{
				flags[i] = false;
			}
			IQueue queue = new QueueAsLinkedList();
			queue.Enqueue(vertex[start]);
			flags[start] = true;
			while (!(queue.IsEmpty || visitor.IsDone))
			{
				IVertex vertex1 = (IVertex)queue.Dequeue();
				visitor.PreVisit(vertex1);
				Console.WriteLine("being in " + vertex1.Number);
				IEnumerator iEnumerator = vertex1.Successors.GetEnumerator();
				try
				{
					while (iEnumerator.MoveNext())
					{
						IVertex vertex2 = (IVertex)iEnumerator.Current;
						if (!flags[vertex2.Number])
						{
							queue.Enqueue(vertex2);
							visitor.Visit(vertex2);
							Console.WriteLine("engueueing " + vertex2.Number);
							flags[vertex2.Number] = true;
						}
					}

				}
				finally
				{
					IDisposable iDisposable = iEnumerator as IDisposable;
					if (iDisposable != null)
					{
						iDisposable.Dispose();
					}
				}
				visitor.PostVisit(vertex1);
			}
		}

		/// <summary>
		/// This traversal visits the nodes of a directed graph in the order specified by a topological sort. 
		/// Informally, a topological sort is a list of the vertices of a (directed) graph in which all the successors
		///  of any given vertex appear in the sequence after that vertex. 
		/// </summary>
		/// <param name="visitor"></param>
		public  virtual void TopologicalOrderTraversal(IVisitor visitor)
		{
			int[] nums1 = new int[(uint)mNumberOfVertices];
			for (int i1 = 0; i1 < mNumberOfVertices; i1++)
			{
				nums1[i1] = 0;
			}
			IEnumerator iEnumerator = Edges.GetEnumerator();
			try
			{
				while (iEnumerator.MoveNext())
				{
					int[] nums2;

					int k;

					IVertex vertex1 = ((IEdge)iEnumerator.Current).V1;
					(nums2 = nums1)[k = vertex1.Number]++;
				}
			}
			finally
			{
				IDisposable iDisposable = iEnumerator as IDisposable;
				if (iDisposable != null)
				{
					iDisposable.Dispose();
				}
			}
			IQueue queue = new QueueAsLinkedList();
			// a cyclic graph might not have an entry point without in-edges
			//Thanks to Morton Mertner to fix this morten@mertner.com
			int inEdges = 0;
			while( queue.IsEmpty || inEdges > mNumberOfEdges )
			{
				for (int j = 0; j < mNumberOfVertices; j++)
				{
					bool selectionCriteria = nums1[j] == inEdges;
					if( inEdges > 0 ) // cyclic graph - add nodes with cyclic refs first
					{
						foreach( IEdge e in vertex[ j ].EmanatingEdges )
						{
							selectionCriteria &= e.V0.Number == e.V1.Number;
						}
					}
					if( selectionCriteria )
					{
						queue.Enqueue(vertex[j]);                       
					}
				}
				inEdges++;
			}

			while(!(queue.IsEmpty || visitor.IsDone))
			{
				IVertex vertex2 = (IVertex)queue.Dequeue();
				visitor.Visit(vertex2);
				iEnumerator = vertex2.Successors.GetEnumerator();
				try
				{
					while (iEnumerator.MoveNext())
					{
						int[] nums2;

						int k;

						IVertex vertex3 = (IVertex)iEnumerator.Current;
						if ((nums2 = nums1)[k = vertex3.Number]-- == 0)
						{
							queue.Enqueue(vertex3);
						}
					}
				}
				catch(Exception exc)
				{
					System.Diagnostics.Trace.WriteLine(exc.Message,"AbstractGraph.TopologicalOrderTraversal");
				}
				finally
				{
					IDisposable iDisposable = iEnumerator as IDisposable;
					if (iDisposable != null)
					{
						iDisposable.Dispose();
					}
				}
			}
		}

		#endregion
		
		/// <summary>
		/// Return the vertex with the given number
		/// </summary>
		/// <param name="v">a number inside the range of available vertices</param>
		/// <returns>an IVertex object</returns>
		public  virtual IVertex GetVertex(int v)
		{
			Bounds.Check(v, 0, mNumberOfVertices);
			return vertex[v];
		}
		
		/// <summary>
		/// Empties this container
		/// </summary>
		public override void Purge()
		{
			for (int i = 0; i < mNumberOfVertices; i++)
			{
				vertex[i] = null;
			}
			mNumberOfVertices = 0;
			mNumberOfEdges = 0;
		}

		/// <summary>
		/// Accepts a visitor
		/// </summary>
		/// <param name="visitor"></param>
		public override void Accept(IVisitor visitor)
		{
			int i;

			for (i = 0; i < mNumberOfVertices && !visitor.IsDone; i++)
			{
				visitor.Visit(vertex[i]);
			}
		}
		
		/// <summary>
		/// Overrides the ToString method to print out an overview of the graph
		/// </summary>
		/// <returns></returns>
		
		public override string ToString()
		{
			ToStringVisitor toStringVisitor = new ToStringVisitor();
			base.Accept(toStringVisitor);
			return String.Concat(new object[]{base.GetType().FullName, " {\r\n", toStringVisitor, "}"});
		}

		/// <summary>
		/// Gets an IEnumerator for this graph
		/// </summary>
		/// <returns></returns>
		public override IEnumerator GetEnumerator()
		{
			return Vertices.GetEnumerator();
		}

		
		#endregion

		#region Classes

		#region VertexEnumerator
		/// <summary>
		/// Implements the vertex enumeration
		/// </summary>
		private class VertexEnumerator: IEnumerator
		{
			#region Fields
			/// <summary>
			/// the underlying graph
			/// </summary>
			private AbstractGraph graph;
			/// <summary>
			/// the current vertex number
			/// </summary>
			protected int v;
			#endregion

			#region Properties
			/// <summary>
			/// Gets the current vertex
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
						return graph.vertex[v];
					}
				}
			}
			#endregion

			#region Constructor
			/// <summary>
			/// Default constructor
			/// </summary>
			/// <param name="graph">the AbstractGraph on which the enumerator is based.</param>
			internal VertexEnumerator(AbstractGraph graph)
			{
				v = -1;

				this.graph = graph;
			}

			#endregion

			#region Methods
			/// <summary>
			/// Moves the internal pointer to the next element.
			/// </summary>
			/// <returns></returns>
			public  virtual bool MoveNext()
			{
				v++;
				if (v == graph.mNumberOfVertices)
				{
					v = -1;
				}
				bool flag = v < 0 == false;
				return flag;
			}

			/// <summary>
			/// Resets the internal pointer to the beginning of the series.
			/// </summary>
			public  virtual void Reset()
			{
				v = -1;
			}
			#endregion
		}

		#endregion
	
		#region GraphVertex
		/// <summary>
		/// Implements a vertex
		/// </summary>
		protected class GraphVertex : ComparableObject, IVertex, IComparable
		{

			#region Fields
			/// <summary>
			/// the AbstractGraph the vertex belongs to
			/// </summary>
			protected AbstractGraph graph;
			/// <summary>
			/// the number of the vertex
			/// </summary>
			protected int mNumber;
			/// <summary>
			/// the weight of the vertex
			/// </summary>
			protected object mWeight;
			#endregion

			#region Properties
			/// <summary>
			/// Gets the number of the vertex
			/// </summary>
			public virtual int Number
			{
				get
				{
					return mNumber;
				}
			}
			/// <summary>
			/// Gets the weight of the vertex
			/// </summary>
			public virtual object Weight
			{
				get
				{
					return mWeight;
				}
			}
			/// <summary>
			/// Gets an IEnumerable of the incident edges
			/// </summary>
			public  virtual IEnumerable IncidentEdges
			{
				get
				{
					return graph.GetIncidentEdges(mNumber);
				}
			}
			/// <summary>
			/// Gets an IEnumerable of the emanating edges
			/// </summary>
			public  virtual IEnumerable EmanatingEdges
			{
				get
				{
					return graph.GetEmanatingEdges(mNumber);
				}
			}

			/// <summary>
			/// Gets an IEnumerable of the predecessors
			/// </summary>
			public  virtual IEnumerable Predecessors
			{
				get
				{
					return new Enumerable(new PredecessorEnumerator(this));
				}
			}

			/// <summary>
			/// Gets an IEnumerable of the successors
			/// </summary>
			public  virtual IEnumerable Successors
			{
				get
				{
					return new Enumerable(new SuccessorEnumerator(this));
				}
			}

			#endregion

			#region PredecessorEnumerator
			private class PredecessorEnumerator: IEnumerator
			{
				#region Fields
				private GraphVertex vertex;

				private IEnumerator edges;

				#endregion

				#region Properties
				public  virtual object Current
				{
					get
					{
						return ((IEdge)edges.Current).MateOf(vertex);
					}
				}


				#endregion

				#region Constructor
				internal PredecessorEnumerator(GraphVertex vertex)
				{
					this.vertex = vertex;
					edges = vertex.IncidentEdges.GetEnumerator();
				}

				#endregion

				#region Methods
				public  virtual bool MoveNext()
				{
					return edges.MoveNext();
				}

				public  virtual void Reset()
				{
					edges.Reset();
				}
				#endregion
			}
			#endregion

			#region SuccessorEnumerator
			/// <summary>
			/// Implements the successor enumertion
			/// </summary>
			private class SuccessorEnumerator: IEnumerator
			{
				#region Fields
				private GraphVertex vertex;

				private IEnumerator edges;

				#endregion

				#region Properties
				public  virtual object Current
				{
					get
					{
						return ((IEdge)edges.Current).MateOf(vertex);
					}
				}

				#endregion

				#region Constructor
				internal SuccessorEnumerator(GraphVertex vertex)
				{
					this.vertex = vertex;
					edges = vertex.EmanatingEdges.GetEnumerator();
				}

				#endregion
				#region Methods
				public  virtual bool MoveNext()
				{
					return edges.MoveNext();
				}

				public  virtual void Reset()
				{
					edges.Reset();
				}
				#endregion
			}

			#endregion

			#region Constructor
			/// <summary>
			/// Default constructor
			/// </summary>
			/// <param name="graph">a graph object</param>
			/// <param name="mNumber">the number of the vertex</param>
			/// <param name="mWeight">the weight of the vertex</param>
			public GraphVertex(AbstractGraph graph, int mNumber, object mWeight)
			{
				this.graph = graph;
				this.mNumber = mNumber;
				this.mWeight = mWeight;
			}
			#endregion


			
			#region Methods
			/// <summary>
			/// Returns the hashcode for the vertex
			/// </summary>
			/// <returns></returns>
			public override int GetHashCode()
			{
				int i = mNumber;
				if (mWeight != null)
				{
					i += mWeight.GetHashCode();
				}
				int j = i;
				return j;
			}
			/// <summary>
			/// Returns info about the vertex
			/// </summary>
			/// <returns></returns>
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(String.Concat("IVertex {", mNumber));
				if (mWeight != null)
				{
					stringBuilder.Append(String.Concat(", mWeight = ", mWeight));
				}
				stringBuilder.Append("}");
				return stringBuilder.ToString();
			}

			/// <summary>
			/// Not implemented yet
			/// </summary>
			/// <param name="obj"></param>
			/// <returns></returns>
			public override int CompareTo(object obj)
			{
				throw new  NotImplementedException();
			}
			#endregion
		}

		#endregion
		
		#region GraphEdge
		/// <summary>
		/// Implements the edge/connection
		/// </summary>
		protected class GraphEdge : ComparableObject, IEdge, IComparable
		{
			#region Fields
			/// <summary>
			/// the graph
			/// </summary>
			protected AbstractGraph graph;
			/// <summary>
			/// the start index
			/// </summary>
			protected int startIndex;
			/// <summary>
			/// the end index
			/// </summary>
			protected int endIndex;
			/// <summary>
			/// the weight
			/// </summary>
			protected object mWeight;
			#endregion

			#region Properties
			/// <summary>
			/// Gets the start-vertex
			/// </summary>
			public virtual IVertex V0
			{
				get
				{
					return graph.vertex[startIndex];
				}
			}
			/// <summary>
			/// Gets the end-vertex
			/// </summary>
			public virtual IVertex V1
			{
				get
				{
					try
					{
						return graph.vertex[endIndex];
					}
					catch
					{
						return null;
					}
				}
			}
			/// <summary>
			/// Gets the weight of the edge
			/// </summary>
			public virtual object Weight
			{
				get
				{
					return mWeight;
				}
			}
			/// <summary>
			/// Gets whether the edge is directed
			/// </summary>
			public virtual bool IsDirected
			{
				get
				{
					return graph.IsDirected;
				}
			}

			#endregion

			#region Constructor
			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="graph"></param>
			/// <param name="startIndex"></param>
			/// <param name="endIndex"></param>
			/// <param name="mWeight"></param>
			public GraphEdge(AbstractGraph graph, int startIndex, int endIndex, object mWeight)
			{
				this.graph = graph;
				this.startIndex = startIndex;
				this.endIndex = endIndex;
				this.mWeight = mWeight;
			}

			#endregion

			#region Methods
			/// <summary>
			/// Returns whether the given vertex is the complementary vertex of the edge
			/// </summary>
			/// <param name="v"></param>
			/// <returns></returns>
			public virtual IVertex MateOf(IVertex v)
			{
				IVertex vertex;

				if (v.Number == startIndex)
				{
					vertex = graph.vertex[endIndex];
				}
				else
				{
					if (v.Number != endIndex)
					{
						throw new ArgumentException("invalid vertex");
					}
					vertex = graph.vertex[startIndex];
				}
				return vertex;
			}
			/// <summary>
			/// Returns the hashcode of the edge
			/// </summary>
			/// <returns></returns>
			public override int GetHashCode()
			{
				int i = startIndex * graph.mNumberOfVertices + endIndex;
				if (mWeight != null)
				{
					i += mWeight.GetHashCode();
				}
				int j = i;
				return j;
			}
			/// <summary>
			/// Overrides the base method to return the collection of edges
			/// </summary>
			/// <returns></returns>
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(String.Concat("IEdge {", startIndex));
				if (this.IsDirected)
				{
					stringBuilder.Append(String.Concat("->", endIndex));
				}
				else
				{
					stringBuilder.Append(String.Concat("--", endIndex));
				}
				if (mWeight != null)
				{
					stringBuilder.Append(String.Concat(", mWeight = ", mWeight));
				}
				stringBuilder.Append("}");
				return stringBuilder.ToString();
			}

			/// <summary>
			/// Not implemented yet
			/// </summary>
			/// <param name="obj"></param>
			/// <returns></returns>
			public override int CompareTo(object obj)
			{
				throw new NotImplementedException();
			}
			#endregion
		}

		#endregion
		
		#region ToStringVisitor
		private class ToStringVisitor : AbstractVisitor
		{
			#region Fields
			private StringBuilder s;
			#endregion


			#region Constructor
			public ToStringVisitor()
			{
				s = new StringBuilder();
	
			}
			#endregion

			#region Methods
			public override void Visit(object obj)
			{
				IVertex vertex = (IVertex)obj;
				s.Append(String.Concat(vertex, "\r\n"));
				IEnumerator iEnumerator = vertex.EmanatingEdges.GetEnumerator();
				try
				{
					while (iEnumerator.MoveNext())
					{
						IEdge edge = (IEdge)iEnumerator.Current;
						s.Append(String.Concat("    ", edge, "\r\n"));
					}
				}
				finally
				{
					IDisposable iDisposable = iEnumerator as IDisposable;
					if (iDisposable != null)
					{
						iDisposable.Dispose();
					}
				}
			}

			public override string ToString()
			{
				return s.ToString();
			}
			#endregion

		}

		#endregion
		
		#region CountingVisitor
		/// <summary>
		/// Counting visitor implementation
		/// </summary>
		protected class CountingVisitor : AbstractVisitor
		{
			#region Fields
			/// <summary>
			/// counter
			/// </summary>
			private int count;
			#endregion

			#region Constructor
			/// <summary>
			/// Constructor
			/// </summary>
			public CountingVisitor()
			{
				count = 0;

			}
			#endregion

			#region Methods

			/// <summary>
			/// Gets the value of the counter
			/// </summary>
			public int Count
			{
				get
				{
					return count;
				}
			}
			/// <summary>
			/// Implements the actual count
			/// </summary>
			/// <param name="obj"></param>
			public override void Visit(object obj)
			{
				count++;
			}

			#endregion
			
		}

		#endregion

		#region PrintingVisitor
		private class PrintingVisitor : AbstractVisitor
		{
			#region Fields
			private bool comma;
			#endregion

			#region Methods

			public override void Visit(object obj)
			{
				if (comma)
				{
					Console.Write(", ");
				}
				Console.Write(obj);
				comma = true;
			}

			public void Finish()
			{
				Console.WriteLine();
				comma = false;
			}
			#endregion
		}
		#endregion
		
		#endregion
	
	}
}
