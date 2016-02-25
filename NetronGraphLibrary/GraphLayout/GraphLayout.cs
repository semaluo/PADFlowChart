using System;
using System.Drawing;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Diagnostics;
using Netron.GraphLib.UI;
using Netron.GraphLib.Interfaces;
using Netron.GraphLib.Maths;
namespace Netron.GraphLib
{
	/// <summary>
	/// Abstract base class for the implementation of a layout algorithm
	/// </summary>
	public abstract class GraphLayout: IGraphLayout
	{

		#region Fields
		/// <summary>
		/// the IGraphSite or control
		/// </summary>
		[NonSerialized]  protected IGraphSite mSite;
		/// <summary>
		/// the number of nodes
		/// </summary>
		protected int nnodes = 0;
		/// <summary>
		/// the number of edges
		/// </summary>
		protected int nedges=0;
		/// <summary>
		/// the canvas size
		/// </summary>
		protected Size CanvasSize ;
		/// <summary>
		/// the nodes
		/// </summary>
		protected ShapeCollection nodes=null;
		/// <summary>
		/// the edges
		/// </summary>
		protected ConnectionCollection edges=null;
		/// <summary>
		/// the abstract
		/// </summary>
		protected GraphAbstract extract = null;
		#endregion
		
		#region Properties
		/// <summary>
		/// Gets or sets the mSite to which the layout belongs
		/// </summary>
		public IGraphSite Site
		{
			get{return mSite;}
			set
			{
				if(mSite==null) return;
				mSite = value;
				nnodes = mSite.Shapes.Count;
				nedges=mSite.Connections.Count;
				CanvasSize = mSite.Size;
				nodes=mSite.Shapes;
				edges=mSite.Connections;
				extract = value.Abstract;
			}
		}

		#endregion
		
		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="mSite"></param>
		protected GraphLayout(IGraphSite mSite)
		{
			if(mSite==null) throw new Exception("No mSite specified in graph layout constructor.");;
			this.mSite = mSite;
			nnodes = mSite.Shapes.Count;
			nedges=mSite.Connections.Count;
			CanvasSize = mSite.Size;
			nodes=mSite.Shapes;
			edges=mSite.Connections;
			extract = mSite.Abstract;
		}

		#endregion

		#region Methods
		/// <summary>
		/// Overridable start layout method
		/// </summary>
		public virtual void StartLayout()
		{
			
		}

		/// <summary>
		/// Overridable stop layout method
		/// </summary>
		public virtual void StopLayout()
		{}

		/// <summary>
		/// Overridable, returns the center of the graph
		/// </summary>
		/// <returns></returns>
		protected virtual NetronVector GraphCenter()
		{
			return new NetronVector(0,0,0);
		}
		#endregion

	}
}