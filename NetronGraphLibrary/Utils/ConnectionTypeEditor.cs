using System;
using System.Drawing.Design;
using System.Windows.Forms;
namespace Netron.GraphLib.UI
{
	/// <summary>
	/// The UI-editor for the ConnectionType of the Connection object in the propertygrid
	/// </summary>
	public class ConnectionTypeEditor : UITypeEditor 
	{ 
		#region Methods
		/// <summary>
		/// Returns UITypeEditorEditStyle.DropDown
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context) 
		{ 
			// We will use a window for property editing. 
			return UITypeEditorEditStyle.DropDown; 
		} 
		/// <summary>
		/// Edits the values
		/// </summary>
		/// <param name="context"></param>
		/// <param name="provider"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public override object EditValue( System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value) 
		{ 
			
			GenericTextEditor editor = new GenericTextEditor();
			editor.TextToEdit = (string) value;
			DialogResult res=editor.ShowDialog();

			// Return the new value. 
			if(res==DialogResult.OK)			
				return editor.TextToEdit;
			else
				return (string) value;
		} 
		/// <summary>
		/// Returns false, we don't use icons here
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public override bool GetPaintValueSupported(	System.ComponentModel.ITypeDescriptorContext context) 
		{ 
			// No special thumbnail will be shown for the grid. 
			return false; 
		} 

		#endregion
	} 

}
