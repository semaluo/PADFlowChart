using System;
using System.Diagnostics;
using System.Windows.Forms;
using Netron.GraphLib.Configuration;
namespace Netron.GraphLib.UI
{
	/// <mSummary>
	/// Represent a menu item collecting shape types belonging to the same category
	/// </mSummary>
	public class CategoryMenuItem : MenuItem
	{
		

		#region Constructors
		/// <summary>
		/// Constructor
		/// </summary>
		public CategoryMenuItem()
		{
					
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="text"></param>
		public CategoryMenuItem(string text) : base(text)
		{}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="text"></param>
		/// <param name="handler"></param>
		public CategoryMenuItem(string text, EventHandler handler) : base(text,handler)
		{
			this.Text = text;
			
		}
		#endregion

		
		
		/// <summary>
		/// Overrides the default base implementation since the cloning
		/// does not return the correct GraphMenuItem type but simply
		/// a MenuItem, which gives a 
		/// </summary>
		/// <returns></returns>
		public override MenuItem CloneMenu()
		{			
			CategoryMenuItem item = new CategoryMenuItem(this.Text); //has no Click handler
			GraphMenuItem gmi;
			for(int k=0; k<this.MenuItems.Count; k++)
			{
				gmi = this.MenuItems[k] as GraphMenuItem;
				if(gmi!=null)
				{
					item.MenuItems.Add(new GraphMenuItem(gmi.Summary,gmi.ClickHandler));
				}
			}
			//note that we don't need to recurse since a shape can only be in one category
			return item;

		}



	}
}
