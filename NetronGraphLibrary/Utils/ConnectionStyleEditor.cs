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
using System.Security.Permissions;
namespace Netron.GraphLib
{
	/// <summary>
	/// Allows to select a connection path from the propertygrid, the collection is expandable 
	/// by means of the app.config
	/// </summary>
	public class ConnectionStyleEditor : UITypeEditor 
	{
		#region Fields
		/// <summary>
		/// the editor service
		/// </summary>
		IWindowsFormsEditorService edSvc;
		/// <summary>
		/// the inner listbox
		/// </summary>
		ListBox listbox;
		#endregion
		
		#region Methods
		/// <summary>
		/// Returns false, no icon support
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		[SecurityPermissionAttribute(SecurityAction.Demand,SerializationFormatter=true)]
		public override bool GetPaintValueSupported(ITypeDescriptorContext context) 
		{
			return false; //set true if you want an iconic representation next to the value
		}
		/// <summary>
		/// The actual editing
		/// </summary>
		/// <param name="args"></param>
		[SecurityPermissionAttribute(SecurityAction.Demand,SerializationFormatter=true)]
		public override void PaintValue(PaintValueEventArgs args) 
		{
			
			if (args.Value == null)
				return;

			string cb = (string)args.Value;
			
			Rectangle r = args.Bounds;

			
			try 
			{
				GraphicsState s = args.Graphics.Save();
				args.Graphics.RenderingOrigin = new Point(r.X, r.Y);
				switch(cb)
				{
					case "Bezier": 
						args.Graphics.FillRectangle(Brushes.BlueViolet, r);break;
					case "Rectangular":
						args.Graphics.FillRectangle(Brushes.Coral, r);break;
					case "Default":
						args.Graphics.FillRectangle(Brushes.YellowGreen, r);break;
					default:
						args.Graphics.FillRectangle(Brushes.WhiteSmoke, r);break;

				}
				
				args.Graphics.Restore(s);

			} 
			finally 
			{
				
			}
		}

		/// <summary>
		/// Returns UITypeEditorEditStyle.DropDown
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		[SecurityPermissionAttribute(SecurityAction.Demand,SerializationFormatter=true)]
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) 
		{
			return UITypeEditorEditStyle.DropDown;
		}
		/// <summary>
		/// The actual editing
		/// </summary>
		/// <param name="context"></param>
		/// <param name="provider"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		[SecurityPermissionAttribute(SecurityAction.Demand,SerializationFormatter=true)]
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
				ConnectionStyleAttribute attr =  context.PropertyDescriptor.Attributes[typeof(ConnectionStyleAttribute)] as ConnectionStyleAttribute;
			
				ArrayList values =new ArrayList();
				values.Add("Default");
				values.Add("Rectangular");
				values.Add("Bezier");
					
				//attributes are the only way to pass info from the context to the property grid
				//A UITypeDescriptor is supposed to be independent of the context/application
				//There is no way you can get access to the canvas starting from the grid or from this class

				if (attr != null && !attr.IsDefaultAttribute()) 
				{	
					values.AddRange(attr.ExtraStyles);
				}			
				
				//this is only for design-time support:
				//ISelectionService serv = (ISelectionService )provider.GetService(typeof(ISelectionService ));
				listbox.Items.Clear();

				int width = 0;
				Font font = listbox.Font;

				// Add the standard values in the list box and
				// measure the text at the same time.

				using (Graphics g = listbox.CreateGraphics()) 
				{
					foreach (object item in values) 
					{
						if (!listbox.Items.Contains(item)) 
						{						
							//							if (!editor.ShowPreviewOnly)
							width = (int)Math.Max(width, g.MeasureString(item.ToString(), font).Width);
							listbox.Items.Add(item);
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
		/// <summary>
		/// Closes the dropdown when something got selected
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnListBoxChanged(object sender, EventArgs e)
		{
			edSvc.CloseDropDown();
		}

		#endregion
	}

}
