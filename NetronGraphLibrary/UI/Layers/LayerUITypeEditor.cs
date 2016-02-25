using System;
using System.Runtime.Remoting;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.Drawing.Drawing2D;
using Netron.GraphLib.Attributes;
namespace Netron.GraphLib
{
	/// <summary>
	/// Allows to select a layer from the propertygrid, the collection depends on the layers added to the control
	/// </summary>
	public class LayerUITypeEditor : UITypeEditor 
	{

		#region Fields
		IWindowsFormsEditorService edSvc;
		ListBox listbox;
		#endregion

		#region Properties
		#endregion

		#region Constructor
		/// <summary>
		/// Default constructor
		/// </summary>
		public LayerUITypeEditor(){}

		#endregion

		#region Methods
		/// <summary>
		/// Overrides the methods to set that iconic reps are supported
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public override bool GetPaintValueSupported(ITypeDescriptorContext context) 
		{
			return true; //set true if you want an iconic representation next to the value
		}
		/// <summary>
		/// Paints the selected value; iconic representation
		/// </summary>
		/// <param name="args"></param>
		public override void PaintValue(PaintValueEventArgs args) 
		{
			
			if (args.Value == null)
				return;

			string cb = args.Value.ToString();
			
			Rectangle r = args.Bounds;

			
			try 
			{
				GraphicsState s = args.Graphics.Save();
				args.Graphics.RenderingOrigin = new Point(r.X, r.Y);				
				//fetching the color of the layer requires a long deep access...
				args.Graphics.FillRectangle(new SolidBrush(((Shape)((PropertyBag) args.Context.Instance).Owner).Layer.LayerColor), r);			
				args.Graphics.Restore(s);

			} 
			finally 
			{
				
			}
		}
		/// <summary>
		/// Returns the basic UITypeEditorEditStyle.DropDown style
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) 
		{
			return UITypeEditorEditStyle.DropDown;
		}
		/// <summary>
		/// Implements the dropdown style for selecting layers
		/// </summary>
		/// <param name="context"></param>
		/// <param name="provider"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			// Uses the IWindowsFormsEditorService to display a  drop-down UI
			if (edSvc == null)
				edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
			if( edSvc != null ) 
			{
				if (listbox == null) 
				{
					listbox = new ListBox();
					listbox.BorderStyle = BorderStyle.None;
					listbox.SelectedIndexChanged += new EventHandler(OnListBoxChanged);
				}
				GraphLayerAttribute attr =  context.PropertyDescriptor.Attributes[typeof(GraphLayerAttribute)] as GraphLayerAttribute;
			
				GraphLayerCollection values =new GraphLayerCollection();
				
				
					
				//attributes are the only way to pass info from the context to the property grid
				//A UITypeDescriptor is supposed to be independent of the context/application
				//There is no way you can get access to the canvas starting from the grid or from this class

				if (attr != null && !attr.IsDefaultAttribute()) 
				{	
					values.AddRange(attr.Layers);
				}			
				
				//GraphLayer dl = new GraphLayer("Default");
				//values.Add(GraphAbstract.DefaultLayer);
				//this is only for design-time support:
				//ISelectionService serv = (ISelectionService )provider.GetService(typeof(ISelectionService ));
				listbox.Items.Clear();

				int width = 0;
				Font font = listbox.Font;

				// Add the standard values in the list box and
				// measure the text at the same time.

				using (Graphics g = listbox.CreateGraphics()) 
				{
					foreach (GraphLayer layer in values) 
					{
						if (!listbox.Items.Contains(layer)) 
						{						
							//							if (!editor.ShowPreviewOnly)
							width = (int)Math.Max(width, g.MeasureString(layer.ToString(), font).Width);
							listbox.Items.Add(layer);
						}
					}
				}

				
				listbox.SelectedItem = value;
				listbox.Height = 
					Math.Max(font.Height + 2, Math.Min(200, listbox.PreferredHeight));
				listbox.Width = Math.Max(width, 100);
                
				edSvc.DropDownControl( listbox );

				if (listbox.SelectedItem != null)
					return listbox.SelectedItem;
				else return value;
               
			}
			return value;

		}
		private void OnListBoxChanged(object sender, EventArgs e)
		{
			edSvc.CloseDropDown();
		}

		#endregion
		
	}

}
