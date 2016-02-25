using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Drawing.Design;

namespace Netron.GraphLib.UI
{
	/// <summary>
	/// Control designer of the graph-control
	/// </summary>
	internal class GraphControlDesigner : ControlDesigner 
	{
				private Bitmap bmp;

		#region Properties
		/// <summary>
		/// Gets the verbs of the control
		/// </summary>
		public override System.ComponentModel.Design.DesignerVerbCollection Verbs
		{
			get
			{		
				DesignerVerbCollection col=new DesignerVerbCollection();
				col.Add(new DesignerVerb("About",new EventHandler(About)));
				col.Add(new DesignerVerb("Help",new EventHandler(NetronSite)));
				return col;
			}
		}
		#endregion

		#region Constructor
		public GraphControlDesigner()
		{		
			Stream stream=Assembly.GetExecutingAssembly().GetManifestResourceStream("Netron.GraphLib.Info.AboutSplash.jpg");
					
			bmp= Bitmap.FromStream(stream) as Bitmap;
			stream.Close();
			stream=null;

		}
		#endregion
		
		public override void Initialize(System.ComponentModel.IComponent component)
		{
			base.Initialize (component);
			(component as Control).AllowDrop = false;
			(component as Control).BackColor = Color.White;
			(component as GraphControl).EnableContextMenu = true;
		}
		

		protected override void OnPaintAdornments(PaintEventArgs pe)
		{
			base.OnPaintAdornments (pe);
			System.Reflection.Assembly ass = System.Reflection.Assembly.GetExecutingAssembly();
			pe.Graphics.DrawString("Version " + ass.GetName().Version.ToString(),new Font("Verdana",10), Brushes.DimGray,new PointF(10,10));

			pe.Graphics.DrawImage(bmp,10,100,530,228);

			pe.Graphics.DrawString("The graph library comes with some default shapes, if you want additional shapes you need to import them via the app.config. See the tutorials on the Netron site for more information on this." + Environment.NewLine + "The properties of the diagram and diagram entities are accessible via the PropertyGrid, you need to connect the graph control to the PropertyGrid via the OnShowProperties event.",new Font("Verdana",10), Brushes.DimGray,new Rectangle(10,400, 500,300));
			
		}


		

		
		private void About(object sender, EventArgs e)
		{
			Form frm = new Netron.GraphLib.AboutForm(true);
			frm.ShowDialog();
		}

		private void NetronSite(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("http://netron.sf.net");
		}
	
	}
}
