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
	/// Implements the wonderful spring embedder layout algorithm, for more information see
	/// the hyperlinks and information pages on the Netron Project site.
	/// </summary>
	public class SpringEmbedder:GraphLayout
	{
		#region Fields
		/// <summary>
		/// Allows to the shake the graph layout a bit
		/// </summary>
		public bool random=false;

		/// <summary>
		/// algorithm cutoff
		/// </summary>
		protected int cutOff = 15;
		
		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the cutoff of the layout, i.e. the time in seconds the layout algorithm is working
		/// </summary>
		public int LayoutCutOff
		{
			get{return cutOff;}
			set{cutOff = value;}
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Class Constructor 
		/// </summary>
		public SpringEmbedder(IGraphSite site ) : base(site)
		{			
		
		}
		#endregion

		#region Methods
		/// <summary>
		/// Starts the layout process
		/// </summary>
		public override void StartLayout()
		{

			DateTime start = DateTime.Now;
			TimeSpan span = TimeSpan.FromSeconds(cutOff);
			
			
			Random rnd = new Random();
			while (true) 
			{
				//try some relaxation
				try
				{
					if(DateTime.Now.Subtract(start)>span) break;//check cutoff
					relax();
					
				}
				catch (System.OverflowException )
				{
					//Trace.WriteLine(exc.Message);
				}
				//if random set then move a randomly chosen node a bit to stabilize the layout
				if (random && (rnd.NextDouble() < 0.03)) 
				{
					Shape n = (Shape) nodes[(int)(rnd.NextDouble() * nnodes)];
					if (!n.IsFixed) 
					{
						n.X +=Convert.ToSingle(100*rnd.NextDouble() - 50);
						n.Y += Convert.ToSingle(100*rnd.NextDouble() - 50);
					}
																   
				}
				try 
				{
					Thread.Sleep(100);//wait a while
				} 
				catch (System.Threading.ThreadInterruptedException exc) 
				{
					Trace.WriteLine(exc.Message, "SpringEmbedder.StartLayout");
					break;
				}
			}
		}

		/// <summary>
		/// the relaxation or layout algorithm<br>
		/// the MethodImpl sets a lock on the class, the 'lock(this)' could be used inside the code as well </br>
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]void relax() 
		{
			Random rnd = new Random(); //init randomizer
			for (int i = 0 ; i < nedges ; i++)  //loop over all edges
			{
				Connection e =(Connection)  edges[i]; //get an edge
				SizeF siz=e.ConnectionSize;
				double vx = e.To.BelongsTo.X-e.From.BelongsTo.X;
				double vy = e.To.BelongsTo.Y-e.From.BelongsTo.Y;
				double len = e.Length;
				double f = (e.RestLength - len) / (len * 3) ;
				double dx = f * vx;
				double dy = f * vy;

				e.To.BelongsTo.dx += dx;
				e.To.BelongsTo.dy += dy;
				e.From.BelongsTo.dx += -dx;
				e.From.BelongsTo.dy += -dy;
				
			}

			for (int i = 0 ; i < nnodes ; i++) 
			{
				try
				{
					Shape n1 =(Shape) nodes[i];
					double dx = 0;
					double dy = 0;

					for (int j = 0 ; j < nnodes ; j++) 
					{
						if (i == j) 
						{
							continue;
						}
						Shape n2 =(Shape) nodes[j];
						double vx = n1.X - n2.X;
						double vy = n1.Y - n2.Y;
						double len = vx * vx + vy * vy;
						if (len == 0) 
						{
							dx += rnd.NextDouble();
							dy += rnd.NextDouble();
						} 
						else if (len < 100*100) 
						{
							dx += vx / len;
							dy += vy / len;
						}
					}
					double dlen = dx * dx + dy * dy;
					if (dlen > 0) 
					{
						dlen = Math.Sqrt(dlen) / 2;
						n1.dx += dx / dlen;
						n1.dy += dy / dlen;
					}
				}
				catch
				{
					continue;
				}
			}

			
			for (int i = 0 ; i < nnodes ; i++) 
			{
				try
				{
					Shape n = (Shape) nodes[i];
					if (!n.IsFixed) 
					{
						n.X += Convert.ToSingle(Math.Max(-5, Math.Min(5, n.dx)));
						n.Y +=Convert.ToSingle( Math.Max(-5, Math.Min(5, n.dy)));					
					
						if (n.X < 0) 
						{
							n.X = 0;
						} 
						else if (n.X > CanvasSize.Width) 
						{
							n.X = CanvasSize.Width;
						}
						if (n.Y < 0) 
						{
							n.Y = 0;
						} 
						else if (n.Y > CanvasSize.Height) 
						{
							n.Y = CanvasSize.Height;
						}
					}
					n.dx /= 2;
					n.dy /= 2;
				}
				catch
				{
					continue;
				}
			}
			mSite.Invalidate();
		}

		#endregion		
	}
}
