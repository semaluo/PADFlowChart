using System;
using System.Drawing;
using System.Collections;
using System.Threading;
using System.Diagnostics;
using Netron.GraphLib.UI;
using Netron.GraphLib.Interfaces;
using Netron.GraphLib.Analysis;
namespace Netron.GraphLib
{
	/// <summary>
	/// Organizes the diagram in a tree structure
	/// </summary>
	public class TreeLayout: GraphLayout
	{
		
		#region Fields
		private Random rnd = new Random();
		/// <summary>
		/// the space between the nodes
		/// </summary>
		protected int wordSpacing = 50;

		/// <summary>
		/// the height between branches
		/// </summary>
		protected int branchHeight = 50;

		VisitCollection visited = new VisitCollection();
		VisitCollection positioned = new VisitCollection();
		#endregion

		#region Properties

		#endregion

		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="site"></param>
		public TreeLayout(IGraphSite site):base(site)
		{
			this.mSite = site;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Returns the adjacent shapes on the basis of a structuring IGraph, usually
		/// what is being returned by Prim's algorithm.
		/// This is equal to the adjacent nodes if the graph is a tree.
		/// Whether a connected nodes is really a 'child' is the sense of being positioned 
		/// on a lower level cannot be decided here but belongs to the layout
		/// </summary>
		/// <param name="structure"></param>
		/// <param name="shape"></param>
		/// <returns>A ShapeCollection of shapes</returns>
		private ShapeCollection AdjacentNodes(IGraph structure, Shape shape)
		{
			try
			{
				mSite.OutputInfo("Adjacents of '" + shape.Text +"':");
				int index = (int) shape.Tag;
				IVertex vertex;
				//TODO: take direction into account
				IEnumerator numer = structure.GetVertex(index).Successors.GetEnumerator();
				ShapeCollection shapes = new ShapeCollection();
				while(numer.MoveNext())
				{
					vertex = numer.Current as IVertex;
					shapes.Add(nodes[vertex.Number]);
					mSite.OutputInfo("\t - '" + nodes[vertex.Number].Text +"'");
				}
				numer = structure.GetVertex(index).Predecessors.GetEnumerator();
				while(numer.MoveNext())
				{
					vertex = numer.Current as IVertex;
					shapes.Add(nodes[vertex.Number]);
					mSite.OutputInfo("\t - '" + nodes[vertex.Number].Text +"'");
				}


				return shapes;

			}
			catch
			{
				//don't return null, make the collection just empty
				return new ShapeCollection();
			}
		}
		

		/// <summary>
		/// Feeds the layout thread
		/// </summary>
		public override void StartLayout()
		{
			if(nodes.Count<=1) return; //nothing to do
			//if there are cycles in the graph we have infinite loops, make sure the layout is finite
			//by checking this list
			for(int k=0; k<nodes.Count;k++)
			{
				visited[nodes[k].UID.ToString()]=false;
				positioned[nodes[k].UID.ToString()]=false;
				nodes[k].Tag = k;//makes sure we can get the index in the collection
			}
			//the structure depends on a spanning tree (Prim's algorithm here)
			if (extract==null)
				throw new Exception("The layout algorithm doesn't have a GraphAbstract to work with.");
			GraphAnalyzer analyzer = new GraphAnalyzer(extract, true);
			IGraph g = Algorithms.PrimsAlgorithm(analyzer,0);//TODO: allow to modify the starting vertex			
			//IGraph g = Algorithms.KruskalsAlgorithm(analyzer);
			mSite.OutputInfo("Prim's:" + Environment.NewLine);
			mSite.OutputInfo(g.ToString());
			if(g==null) return;//TODO: notify the failure to find a spanning tree
			VerticalDrawTree(g,nodes[0],true,30,30);
			//VerticalDrawTree(g,nodes[rnd.Next(0,nodes.Count-1)],true,30,30);


		}
		/// <summary>
		/// Positions everything underneath the node and returns the total width of the kids
		/// </summary>
		/// <param name="containerNode"></param>
		/// <param name="first"></param>
		/// <param name="shiftLeft"></param>
		/// <param name="shiftTop"></param>
		/// <param name="structure"></param>
		/// <returns></returns>
		private float VerticalDrawTree(IGraph structure, Shape containerNode, bool first, float shiftLeft, float shiftTop)
		{
			bool isFirst = false;
			ShapeCollection adj = AdjacentNodes(structure,containerNode);
			bool isParent = adj.Count>0? true: false;
			float childrenWidth = 0;
			float thisX, thisY;		
			float returned = 0;
			float verticalDelta = branchHeight ; //the applied vertical shift of the child depends on the Height of the containerNode
			
			visited[containerNode.UID.ToString()] = true;
			
			
			#region Children width
			for(int i =0; i<adj.Count; i++)
			{
				//determine the width of the label
				if(i==0)			
					isFirst = true;				
				else 				
					isFirst = false;
				if(adj[i].IsVisible && !visited[adj[i].UID.ToString()])
				{
					if((branchHeight - containerNode.Height) < 30) //if too close to the child, shift it with 40 units
						verticalDelta = containerNode.Height + 40;
					returned = VerticalDrawTree(structure, adj[i], isFirst, shiftLeft + childrenWidth, shiftTop + verticalDelta );									
					childrenWidth += returned;
					
				}		

			}
			if(childrenWidth>0 && containerNode.IsExpanded)
				childrenWidth=Math.Max(Convert.ToInt32(childrenWidth + (containerNode.Width-childrenWidth)/2), childrenWidth); //in case the length of the containerNode is bigger than the total length of the children
			#endregion

			if(childrenWidth==0) //there are no children; this is the branch end
				childrenWidth = containerNode.Width+wordSpacing;
			
			#region Positioning
			thisY = shiftTop;			
			if(adj.Count>0 && containerNode.IsExpanded)
			{
				float firstChild = float.MaxValue; //for comparison to find the smallest
				float lastChild = float.MinValue;
				float tmp;
				//loop over the adjacent nodes and take the first node that is positioned lower than the current
				//this is the zero-th entry if the graph is a tree but not in general
				int f = -1, l = -1;
				for(int k=0;k<adj.Count; k++)
				{
					if(positioned[adj[k].UID.ToString()])
					{
						tmp = adj[k].Rectangle.Left+ adj[k].Width/2;
						if(tmp<firstChild)
						{
							firstChild = tmp;
							f = k;
						}
						if(tmp>lastChild)
						{
							lastChild = tmp;
							l = k;
						}
					}
				}
				/* informative but costly debugging info*/
				mSite.OutputInfo("               ");
				mSite.OutputInfo("Positioning : '" + containerNode.Text + "'");
				if(f>-1)
					mSite.OutputInfo("First child: '" + adj[f].Text + "'");
				else
					mSite.OutputInfo("First child: not found");
				if(l>-1)
					mSite.OutputInfo("Last child: '" + adj[l].Text  + "'");
				else
					mSite.OutputInfo("Last child: not found");
				
				if(firstChild<float.MaxValue) //there is an adjacent node and it's not positioned above this one
				{
					if(firstChild==lastChild)
					{
						thisX = firstChild - containerNode.Width/2;
					}
					else
					{
					
						//the following max in case the containerNode is larger than the childrenWidth
						thisX = Math.Max(firstChild + (lastChild -firstChild - containerNode.Width)/2, firstChild);
					}
				}
				else
					thisX = shiftLeft;
			}
			else
			{
				thisX = shiftLeft;		
				
			}
			
			containerNode.X = thisX;
			containerNode.Y = thisY;
			positioned[containerNode.UID.ToString()] = true;
			mSite.Invalidate();
			#endregion

			return childrenWidth;
		}

		/// <summary>
		/// Horizontal layout algorithm
		/// </summary>
		/// <param name="containerNode"></param>
		/// <param name="first"></param>
		/// <param name="shiftLeft"></param>
		/// <param name="shiftTop"></param>
		/// <returns></returns>
		private float HorizontalDrawTree(Shape containerNode, bool first, float shiftLeft, float shiftTop)
		{
			bool isFirst = false;
			bool isParent = containerNode.AdjacentNodes.Count>0? true: false;
			float childrenHeight = 0;
			float thisX, thisY;		
			float returned = 0;			
			float horizontalDelta = branchHeight;

			visited[containerNode.UID.ToString()] = true;

			#region Children height
			for(int i =0; i<containerNode.AdjacentNodes.Count; i++)
			{
				//determine the width of the label
				if(i==0)			
					isFirst = true;				
				else 				
					isFirst = false;
				if(containerNode.AdjacentNodes[i].IsVisible && !visited[containerNode.AdjacentNodes[i].UID.ToString()])
				{
					if((branchHeight - containerNode.Width) < 30) //if too close to the child, shift it with 40 units
						horizontalDelta = containerNode.Width + 40;
					returned = HorizontalDrawTree(containerNode.AdjacentNodes[i], isFirst, shiftLeft + horizontalDelta , shiftTop + childrenHeight );					
					childrenHeight += returned;
				}
				

			}
			#endregion

			if(childrenHeight==0) //there are no children; this is the branch end
				childrenHeight = containerNode.Height+wordSpacing;
			
			#region Positioning
			thisX = shiftLeft;			
			if(containerNode.AdjacentNodes.Count>0 && containerNode.IsExpanded)
			{
				
				float firstChild = containerNode.AdjacentNodes[0].Y;
				float lastChild = containerNode.AdjacentNodes[containerNode.AdjacentNodes.Count-1].Y;
				thisY = Convert.ToInt32(firstChild + (lastChild - firstChild)/2);
				
			}
			else
			{
				thisY = Convert.ToInt32(shiftTop);		
				
			}
			
			containerNode.X = thisX;
			containerNode.Y = thisY;
			#endregion

			return childrenHeight;
		}



		#endregion

	}
}

