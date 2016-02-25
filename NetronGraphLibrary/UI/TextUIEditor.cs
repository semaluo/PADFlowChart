using System;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Netron.GraphLib.UI
{
	/// <summary>
	/// UI type-editor for big (amounts of) text
	/// </summary>
	[ComVisible(false)]
	public class TextUIEditor : UITypeEditor 
	{ 
		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		public TextUIEditor() : base()
		{}

		#endregion

		#region Methods
		/// <summary>
		/// returns UITypeEditorEditStyle.Modal
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		[EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted=true)]
		public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context) 
		{ 
			// We will use a window for property editing. 
			return UITypeEditorEditStyle.Modal; 
		} 

		

		/// <summary>
		/// The actual editing
		/// </summary>
		/// <param name="context"></param>
		/// <param name="provider"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		[EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted=true)]
		public override object EditValue( System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value) 
		{ 
			
			GenericTextEditor editor = new GenericTextEditor();
			string sval = (string) value;
			editor.TextToEdit = sval;
			DialogResult res=editor.ShowDialog();

			// Return the new value. 
			if(res==DialogResult.OK)			
				return editor.TextToEdit;
			else
				return sval;
		} 

		/// <summary>
		/// Returns false
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		[EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted=true)]
		public override bool GetPaintValueSupported(	System.ComponentModel.ITypeDescriptorContext context) 
		{ 
			// No special thumbnail will be shown for the grid. 
			return false; 
		} 

		#endregion
	} 

}
