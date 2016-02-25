using System;
using System.IO;
using System.Collections;
namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Collects diverse algorithms related to graphs and other data structures
	/// </summary>
	public class Algorithms
	{
		#region Structs and classes
		private struct Entry
		{
			public bool known;

			public int distance;

			public int predecessor;


			public Entry(bool known, int distance, int predecessor)
			{
				this.known = known;
				this.distance = distance;
				this.predecessor = predecessor;
			}
		}


		private class EarliestTimeVisitor : AbstractVisitor
		{
			private int[] earliestTime;


			internal EarliestTimeVisitor(int[] earliestTime)
			{
				this.earliestTime = earliestTime;
			}

			public override void Visit(object obj)
			{
				IVertex vertex = (IVertex)obj;
				int i = earliestTime[0];
				IEnumerator iEnumerator = vertex.IncidentEdges.GetEnumerator();
				try
				{
					while (iEnumerator.MoveNext())
					{
						IEdge edge = (IEdge)iEnumerator.Current;
						i = Math.Max(i, earliestTime[edge.V0.Number] + (int)edge.Weight);
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
				earliestTime[vertex.Number] = i;
			}
		}


		private class LatestTimeVisitor : AbstractVisitor
		{
			private int[] latestTime;


			internal LatestTimeVisitor(int[] latestTime)
			{
				this.latestTime = latestTime;
			}

			public override void Visit(object obj)
			{
				IVertex vertex = (IVertex)obj;
				int i = latestTime[(int)latestTime.Length - 1];
				IEnumerator iEnumerator = vertex.EmanatingEdges.GetEnumerator();
				try
				{
					while (iEnumerator.MoveNext())
					{
						IEdge edge = (IEdge)iEnumerator.Current;
						i = Math.Min(i, latestTime[edge.V1.Number] - (int)edge.Weight);
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
				latestTime[vertex.Number] = i;
			}
		}


		private class Counter : ComparableInt32
		{

			internal Counter(int value) : base(value)
			{
			}

			public static Counter operator ++ (Counter counter)
			{
				counter.obj = ((int)counter.obj + 1);
				return counter;
			}
		}

		#endregion

		#region Algorithms
		/// <summary>
		/// 
		/// </summary>
		/// <param name="tree"></param>
		public static void BreadthFirstTraversal(ITree tree)
		{
			IQueue queue = new QueueAsLinkedList();
			if (!tree.IsEmpty)
			{
				queue.Enqueue(tree);
			}
			while (!queue.IsEmpty)
			{
				ITree tree2 = (ITree)queue.Dequeue();
				Console.WriteLine(tree2.Key);
				for (int i = 0; i < tree2.Degree; i++)
				{
					ITree tree3 = tree2.GetSubtree(i);
					if (!tree3.IsEmpty)
					{
						queue.Enqueue(tree3);
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="writer"></param>
		public static void EquivalenceClasses(TextReader reader, TextWriter writer)
		{
			string str = reader.ReadLine();
			IPartition partition = new PartitionAsForest(Convert.ToInt32(str));
			while ((str = reader.ReadLine()) != null)
			{
				string[] strs = str.Split(null);
				int j = Convert.ToInt32(strs[0]);
				int k = Convert.ToInt32(strs[1]);
				ISet set1 = partition.Find(j);
				ISet set2 = partition.Find(k);
				if (set1 != set2)
				{
					partition.Join(set1, set2);
				}
				else
				{
					writer.WriteLine("redundant pair:{0},{1}", j, k);
				}
			}
			writer.WriteLine(partition);
		}

		/// <summary>
		/// Thanks to Morton Mertner for a fix here
		/// </summary>
		/// <param name="g"></param>
		/// <param name="s"></param>
		/// <returns></returns>
		public static IDigraph DijkstrasAlgorithm(IDigraph g, int s)
		{
			int n = g.NumberOfVertices;
			Entry[] table = new Entry[n];
			for (int v = 0; v < n; ++v)
				table[v] = new Entry(false,
					int.MaxValue, int.MaxValue);
			table[s].distance = 0;
			IPriorityQueue queue = new BinaryHeap(g.NumberOfEdges);
			queue.Enqueue(new Association(0, g.GetVertex(s)));
			int vertexCount = 0; // MM fix
			while (!queue.IsEmpty)
			{
				Association assoc = (Association)queue.DequeueMin();
				IVertex v0 = (IVertex)assoc.Value;
				if (!table[v0.Number].known)
				{
					table[v0.Number].known = true;
					vertexCount++; // MM fix
					foreach (IEdge e in v0.EmanatingEdges)
					{
						IVertex v1 = e.MateOf(v0);
						int d = table[v0.Number].distance + (e.Weight != null ? (int)e.Weight : 0);
						if (table[v1.Number].distance > d)
						{
							table[v1.Number].distance = d;
							table[v1.Number].predecessor = v0.Number;
							queue.Enqueue(new Association(d, v1));
						}
					}
				}
			}
			// MM fixed loop to filter out unused edges and vertices
			IDigraph result = new DigraphAsLists( vertexCount );
			int cv = 0;
			int[] v2cv = new int[ n ];
			for( int v = 0; v < n; ++v )
			{
				if( table[v].known )
				{
					result.AddVertex( cv, table[v].distance );
					v2cv[ v ] = cv++;
				}
			}
			for (int v = 0; v < n; ++v)
			{
				if (v != s && table[v].known && table[v].distance < int.MaxValue )
				{
					result.AddConnection( v2cv[ v ], v2cv[ table[v].predecessor ] );
				}
			}
			return result;
		}

		/// <summary>
		/// An algorithm for finding the shortest path between two graph vertices
		/// </summary>
		/// <param name="g"></param>
		/// <returns></returns>
		public static IDigraph FloydsAlgorithm(IDigraph g)
		{
			int vertexCount = g.NumberOfVertices;
			int[,] distance = new int[vertexCount, vertexCount];
			for (int j1 = 0; j1 < vertexCount; j1++)
			{
				for (int k1 = 0; k1 < vertexCount; k1++)
				{
					distance[j1, k1]= int.MaxValue; //means infinity actually
				}
			}
			IEnumerator iEnumerator = g.Edges.GetEnumerator();
			try
			{
				while (iEnumerator.MoveNext())
				{
					IEdge edge = (IEdge)iEnumerator.Current;
					distance[edge.V0.Number, edge.V1.Number]= (int)edge.Weight;
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
			
				
				for (int j2 = 0; j2 < vertexCount; j2++)
				{
					
					for (int k2 = 0; k2 < vertexCount; k2++)
					{Console.WriteLine(j2 + "->" + k2);
						for (int i2 = 0; i2 < vertexCount; i2++)
						{
						
						if (distance[j2, i2] != int.MaxValue && distance[i2, k2] != int.MaxValue)
						{
							int i3 = distance[j2, i2] + distance[i2, k2];
							if (distance[j2, k2] > i3)
							{
								distance[j2, k2]= i3;
								Console.WriteLine("  " + i2);
							}
						}
					}
				}
			}
			IDigraph digraph1 = new DigraphAsMatrix(vertexCount);
			for (int j3 = 0; j3 < vertexCount; j3++)
			{
				digraph1.AddVertex(j3);
			}
			for (int k3 = 0; k3 < vertexCount; k3++)
			{
				for (int i4 = 0; i4 < vertexCount; i4++)
				{
					if (distance[k3, i4] != int.MaxValue)
					{
						digraph1.AddConnection(k3, i4, distance[k3, i4]);
					}
				}
			}
			return digraph1;
		}

		/// <summary>
		/// Computes a spanning tree
		/// </summary>
		/// <param name="g"></param>
		/// <param name="s"></param>
		/// <returns></returns>
		public static IGraph PrimsAlgorithm(IGraph g, int s)
		{
			int i1 = g.NumberOfVertices;
			Entry[] entrys = new Entry[i1];
			for (int j1 = 0; j1 < i1; j1++)
			{
				entrys[j1] = new Entry(false,int.MaxValue, int.MaxValue);
			}
			entrys[s].distance = 0;
			IPriorityQueue priorityQueue = new BinaryHeap(g.NumberOfEdges);
			priorityQueue.Enqueue(new Association(0, g.GetVertex(s)));


			while (!priorityQueue.IsEmpty)
			{
				IVertex vertex1 = (IVertex)((Association)priorityQueue.DequeueMin()).Value;
				if (entrys[vertex1.Number].known)
				{
					continue;
				}
				entrys[vertex1.Number].known = true;

				IEnumerator iEnumerator = vertex1.EmanatingEdges.GetEnumerator();
				try
				{
					while (iEnumerator.MoveNext())
					{
						IEdge edge = (IEdge)iEnumerator.Current;
						IVertex vertex2 = edge.MateOf(vertex1);
						
						int k = (edge.Weight==null? 0 :(int) edge.Weight );
						if (!entrys[vertex2.Number].known && entrys[vertex2.Number].distance > k)
						{
							entrys[vertex2.Number].distance = k;
							entrys[vertex2.Number].predecessor = vertex1.Number;
							priorityQueue.Enqueue(new Association(k, vertex2));
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
			}
			
				IGraph graph1 = new GraphAsLists(i1);
				for (int i2 = 0; i2 < i1; i2++)
				{
					graph1.AddVertex(i2);
				}
				for (int j2 = 0; j2 < i1; j2++)
				{
					if (j2 != s)
					{
						graph1.AddConnection(j2, entrys[j2].predecessor);
					}
				}
				return graph1;
			
		}

		/// <summary>
		/// Kruskal's algorithm is an algorithm that finds a minimum spanning tree for a connected weighted graph. 
		/// This means it finds a subset of the edges that forms a tree that includes every vertex, 
		/// where the total weight of all the edges in the tree is minimized. 
		/// If the graph is not connected, then it finds a minimum spanning forest (a minimum spanning tree for each connected component). 
		/// </summary>
		/// <param name="g"></param>
		/// <returns></returns>
		public static IGraph KruskalsAlgorithm(IGraph g)
		{
			Console.WriteLine("Starting...");
			int i1 = g.NumberOfVertices;
			IGraph graph1 = new GraphAsLists(i1);
			for (int j1 = 0; j1 < i1; j1++)
			{
				graph1.AddVertex(j1);
			}
			IPriorityQueue priorityQueue = new BinaryHeap(g.NumberOfEdges);
			IEnumerator iEnumerator = g.Edges.GetEnumerator();
			Console.WriteLine("got the edge enumerator...");
			try
			{
				while (iEnumerator.MoveNext())
				{
					IEdge edge1 = (IEdge)iEnumerator.Current;
					int k;
					//the casting depends on the datatype of the weight, here you are on your own
					//we'll assume that an int will do as an example
					if(edge1.Weight==null)
						k = 0;
					else
						try
						{
							k = (int)edge1.Weight;
						}
						catch
						{
							k = 0;
						}

					priorityQueue.Enqueue(new Association(k, edge1));
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
			Console.WriteLine("after the edge enumerator...");
			Console.WriteLine(priorityQueue.ToString());
			IPartition partition = new PartitionAsForest(i1);
			Console.WriteLine("The partition: " + partition.Count);
			while (!priorityQueue.IsEmpty && (partition.Count > 1))
			{
				IEdge edge2 = (IEdge)((Association)priorityQueue.DequeueMin()).Value;
				Console.WriteLine(edge2.ToString());
				int i2 = edge2.V0.Number;
				int j2 = edge2.V1.Number;
				Console.WriteLine("got vertices (" + i2 + "," + j2 + ")");
				ISet set1 = partition.Find(i2);
				ISet set2 = partition.Find(j2);
				
				if (set1 != set2)
				{
					partition.Join(set1, set2);
					graph1.AddConnection(i2, j2);
				}
			}
			return graph1;
		}

		/// <summary>
		/// Critical path analysis algorithm
		/// </summary>
		/// <param name="g"></param>
		/// <returns></returns>
		public static IDigraph CriticalPathAnalysis(IDigraph g)
		{
			//TODO: more info here
			int i = g.NumberOfVertices;
			int[] nums1 = new int[(uint)i];
			nums1[0] = 0;
			g.TopologicalOrderTraversal(new EarliestTimeVisitor(nums1));
			int[] nums2 = new int[(uint)i];
			nums2[i - 1] = nums1[i - 1];
			g.DepthFirstTraversal(new PostOrder(new LatestTimeVisitor(nums2)), 0);
			IDigraph digraph1 = new DigraphAsLists(i);
			for (int j = 0; j < i; j++)
			{
				digraph1.AddVertex(j);
			}
			IEnumerator iEnumerator = g.Edges.GetEnumerator();
			try
			{
				while (iEnumerator.MoveNext())
				{
					IEdge edge = (IEdge)iEnumerator.Current;
					int k = nums2[edge.V1.Number] - nums1[edge.V0.Number] - (int)edge.Weight;
					digraph1.AddConnection(edge.V0.Number, edge.V1.Number, (int)edge.Weight);
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
			return DijkstrasAlgorithm(digraph1, 0);
		}

		/// <summary>
		/// Calculator
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="writer"></param>
		public static void Calculator(TextReader reader, TextWriter writer)
		{
			//TODO: more info here
			int i1;

			IStack stack = new StackAsLinkedList();
			while ((i1 = reader.Read()) > 0)
			{
				char ch = (char)i1;
				if (Char.IsDigit(ch))
				{
					stack.Push((ch - '0'));
				}
				else if (ch == '+')
				{
					int j1 = (int)stack.Pop();
					int k1 = (int)stack.Pop();
					stack.Push((k1 + j1));
				}
				else if (ch == '*')
				{
					int i2 = (int)stack.Pop();
					int j2 = (int)stack.Pop();
					stack.Push((j2 * i2));
				}
				else if (ch == '=')
				{
					int k2 = (int)stack.Pop();
					writer.WriteLine(k2);
				}
			}
		}
		/// <summary>
		/// Word counter
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="writer"></param>
		public static void WordCounter(TextReader reader, TextWriter writer)
		{
			string str1;

			ChainedHashTable hashTable =new ChainedHashTable(1031) ;
			while ((str1 = reader.ReadLine()) != null)
			{
				string[] strs = str1.Split(null);
				Counter counter;

				for (int i = 0; i < (int)strs.Length; i++)
				{
					string str2 = strs[i];
					Association association = (Association)hashTable.Find(new Association(str2));
					if (association == null)
					{
						hashTable.Insert(new Association(str2, new Counter(1)));
					}
					else
					{
						counter = (Counter)association.Value;
						counter = ++counter;
					}
				}
			}
			writer.WriteLine(hashTable);
		}
		/// <summary>
		/// Translation
		/// </summary>
		/// <param name="dictionary"></param>
		/// <param name="inputText"></param>
		/// <param name="outputText"></param>
		public static void Translate(TextReader dictionary, TextReader inputText, TextWriter outputText)
		{
			string str1;

			ISearchTree searchTree = new AVLTree();
			while ((str1 = dictionary.ReadLine()) != null)
			{
				string[] strs1 = str1.Split(null);
				if ((int)strs1.Length != 2)
				{
					throw new InvalidOperationException();
				}
				searchTree.Insert(new Association(strs1[0], strs1[1]));
			}
			while ((str1 = inputText.ReadLine()) != null)
			{
				string[] strs2 = str1.Split(null);
				for (int i = 0; i < (int)strs2.Length; i++)
				{
					string str2 = strs2[i];
					ComparableObject comparableObject = searchTree.Find(new Association(str2));
					if (comparableObject == null)
					{
						outputText.Write("?{0}? ", str2);
					}
					else
					{
						Association association = (Association)comparableObject;
						outputText.Write("{0} ", association.Value);
					}
				}
				outputText.WriteLine();
			}
		}
		#endregion
	}
}
