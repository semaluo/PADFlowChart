using System;

namespace Netron.GraphLib.Analysis
{
	/// <summary>
	/// Decorates the abstract of a diagram with the necessary stuff to analyze it with the analysis tools
	/// </summary>
	public class GraphAnalyzer : GraphAsMatrix
	{
		#region Fields
		GraphAbstract extract = null;
		/// <summary>
		/// glues the vertex-index to a shape
		/// </summary>
		private Shape[] shapeGluon;
		/// <summary>
		/// glues a edge-index to a connection
		/// </summary>
		private Connection[,] connectionGluon;
		#endregion

		#region Properties
		
		#endregion

		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="extract"></param>
		/// <param name="discardFixed">whether to discard the ixed shapes in the analysis</param>
		public GraphAnalyzer( GraphAbstract extract, bool discardFixed) : base(extract.Shapes.Count)
		{
			
			this.extract = extract;
			
			 
			shapeGluon = new Shape[extract.Shapes.Count];
			connectionGluon = new Connection[extract.Shapes.Count,extract.Shapes.Count];
			//we assign the index in the Shapes collection to the vertex
			int m =0;
			foreach(Shape shape in extract.Shapes)
			{
				if(shape.IsFixed && discardFixed)
					continue;
				else
				{
					AddVertex(m);
					shapeGluon[m] = extract.Shapes[m];		
					extract.Shapes[m].Tag = m;
					m++;
				}
			}
			mCount = extract.Shapes.Count;
			int v,w;
			for(int k =0; k<extract.Connections.Count;k++)
			{
				try
				{
					v = (int) extract.Connections[k].From.BelongsTo.Tag;
					w = (int) extract.Connections[k].To.BelongsTo.Tag;
				
					this.AddConnection(v , w);
					connectionGluon[v,w] = extract.Connections[k];
					connectionGluon[w,v] = extract.Connections[k];
				}
				catch{continue;}
			}
		}
		#endregion

		#region Methods

		/// <summary>
		/// Returns the Shape object corresponding to the index in the IGraph
		/// </summary>
		/// <param name="k"></param>
		/// <returns></returns>
		public Shape GetShape(int k)
		{
			if(k>-1 && k<mCount)
				return shapeGluon[k];				
			else
				return null;
		}
		/// <summary>
		/// Returns the Connection corresponding to the vw-index in the IGraph
		/// </summary>
		/// <param name="v"></param>
		/// <param name="w"></param>
		/// <returns></returns>
		public Connection GetConnection(int v, int w)
		{
			try
			{
				return connectionGluon[v,w];
			}
			catch
			{return null;}
		}

		#endregion



	}
}
