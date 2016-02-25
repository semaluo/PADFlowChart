using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
namespace Netron.GraphLib.UI
{
	/// <summary>
	/// UITypeEditor to edit file names
	/// </summary>
	public class FilenameUIEditor: System.Drawing.Design.UITypeEditor
	{
		/// <summary>
		/// Gets the edit style
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			if (context != null)
				return UITypeEditorEditStyle.Modal;

			return UITypeEditorEditStyle.None;
		}

		/// <summary>
		/// Edits tyhe value
		/// </summary>
		/// <param name="context"></param>
		/// <param name="provider"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		[RefreshProperties(RefreshProperties.All)]    
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{   
			FileDialog fileDlg;
       
			if (context == null || provider == null || context.Instance == null)
				return base.EditValue(provider, value);

			fileDlg = new System.Windows.Forms.OpenFileDialog();
			fileDlg.Title = "Select Image Filename"; 
			fileDlg.FileName = (value as string);
			fileDlg.Filter = "All Image Files |*.jpg;*.bmp;*.gif;*.png";
          
			if (fileDlg.ShowDialog() == DialogResult.OK)
				value = fileDlg.FileName;
          
			return value;
		}
	}
}
