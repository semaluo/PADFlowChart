using System;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using Netron.GraphLib.UI;
using Netron.GraphLib.Interfaces;
namespace Netron.GraphLib
{
	/// <summary>
	/// Organizes the diagram in a random way
	/// </summary>
	public class RandomizerLayout: GraphLayout
	{
		#region Fields
		private Random rnd = new Random();
		#endregion

		#region Properties

		#endregion

		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="site"></param>
		public RandomizerLayout(IGraphSite site):base(site)
		{
			this.mSite = site;
		}
		#endregion

		#region Methods

		/// <summary>
		/// Starts the randomizer layout
		/// </summary>
		public override void StartLayout()
		{
			if(nodes.Count==0) return; //noting to do
			
			PointF[] e = new PointF[this.nodes.Count];
			PointF[] s = new PointF[this.nodes.Count];
			Point p;
			
			int steps = 50;
			//collect the final destination
			for(int k=0; k<this.nodes.Count; k++)
			{
				p = new Point(rnd.Next(10,(int)(mSite.Width-nodes[k].Width-10)),rnd.Next(10,(int)(mSite.Height-nodes[k].Height- 10)));
				e[k] = p;				
				s[k] = nodes[k].Rectangle.Location;
			}

			//animate the change
			for(int j=1; j<steps+1; j++)
			{
				for(int k=0; k<nodes.Count;k++)
				{
					nodes[k].X = s[k].X+ j*(e[k].X-s[k].X)/steps;
					nodes[k].Y= s[k].Y+ j*(e[k].Y-s[k].Y)/steps;
				}
				mSite.Invalidate();
			}


		}


		#endregion

	}
}
