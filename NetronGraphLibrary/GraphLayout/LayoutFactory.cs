using System;
using System.Drawing;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Diagnostics;
using Netron.GraphLib.UI;
using Netron.GraphLib.Interfaces;
namespace Netron.GraphLib
{
	/// <summary>
	/// Factory of graph layouts
	/// </summary>
	public class LayoutFactory
	{

		#region Fields
		/// <summary>
		/// Delegate for running a spearate thread, used by the layout process
		/// </summary>
		public delegate void  runableDelegate();
		/// <summary>
		/// the layout algorithm
		/// </summary>
		private  GraphLayoutAlgorithms mGraphLayoutAlgorithm=GraphLayoutAlgorithms.SpringEmbedder;
		/// <summary>
		/// the graph site
		/// </summary>
		[NonSerialized]  protected IGraphSite mSite;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the graph layout algorithm
		/// </summary>
		public  GraphLayoutAlgorithms GraphLayoutAlgorithm
		{
			get{return mGraphLayoutAlgorithm;}
			set{mGraphLayoutAlgorithm = value;}
		}
		/// <summary>
		/// Gets or sets the IGraphSite or control the factory refers to
		/// </summary>
		public IGraphSite Site
		{
			get{return mSite;}
			set{mSite = value;}
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="mSite"></param>
		public LayoutFactory(IGraphSite mSite)
		{
		
			this.mSite = mSite;
		}

		#endregion
		
		#region Methods
		/// <summary>
		/// Return a delegate the layout-thread can run
		/// </summary>
		/// <returns></returns>
		public  runableDelegate GetRunable()
		{
			switch (GraphLayoutAlgorithm)
			{
				case GraphLayoutAlgorithms.SpringEmbedder:
					SpringEmbedder emb=new SpringEmbedder(mSite);
					return new runableDelegate(emb.StartLayout);							
				case GraphLayoutAlgorithms.Tree:
					TreeLayout tl = new TreeLayout(mSite);
					return new runableDelegate(tl.StartLayout);
				case GraphLayoutAlgorithms.Randomizer:
					RandomizerLayout rl = new RandomizerLayout(mSite);
					return new runableDelegate(rl.StartLayout);

				default:
					throw new NotImplementedException("Invalid or unknown layout algorithm");					
			}
		}

		#endregion
	}
}