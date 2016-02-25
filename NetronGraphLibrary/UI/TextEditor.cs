using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Netron.GraphLib.UI
{
	/// <summary>
	/// Allows text editing, for shape labels in particular
	/// </summary>
	public class GenericTextEditor : System.Windows.Forms.Form
	{
		#region Fields
		private System.Windows.Forms.TextBox textBox;
		private System.Windows.Forms.Button OKButton;
		private System.Windows.Forms.Button mCancelButton;
		

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the text to edit
		/// </summary>
		public string TextToEdit
		{
			get
			{
				return this.textBox.Text;
			}
			set
			{				
				this.textBox.Text = value;
			}
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		public GenericTextEditor()
		{
		
					InitializeComponent();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.textBox = new System.Windows.Forms.TextBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.mCancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.AcceptsReturn = true;
            this.textBox.AcceptsTab = true;
            this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox.Location = new System.Drawing.Point(4, 4);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox.Size = new System.Drawing.Size(439, 214);
            this.textBox.TabIndex = 0;
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OKButton.Location = new System.Drawing.Point(342, 229);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(90, 25);
            this.OKButton.TabIndex = 1;
            this.OKButton.Text = "OK";
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // mCancelButton
            // 
            this.mCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.mCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.mCancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mCancelButton.Location = new System.Drawing.Point(246, 229);
            this.mCancelButton.Name = "mCancelButton";
            this.mCancelButton.Size = new System.Drawing.Size(90, 25);
            this.mCancelButton.TabIndex = 2;
            this.mCancelButton.Text = "Cancel";
            this.mCancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // GenericTextEditor
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(448, 262);
            this.Controls.Add(this.mCancelButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.textBox);
            this.Name = "GenericTextEditor";
            this.Text = "GenericTextEditor";
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void CancelButton_Click(object sender, System.EventArgs e)
		{
			this.Close();
			this.Dispose();
		}

		private void OKButton_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		#endregion
	}

	

}
