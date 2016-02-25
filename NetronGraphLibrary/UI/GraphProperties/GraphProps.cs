using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Netron.GraphLib;
namespace Netron.GraphLib.UI
{
	/// <summary>
	/// Summary description for GraphProps.
	/// </summary>
	public class GraphProps : UserControl
	{
		#region Fields
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		/// <summary>
		/// the description textbox
		/// </summary>
		private System.Windows.Forms.TextBox description;
		/// <summary>
		/// the description label
		/// </summary>
		private System.Windows.Forms.Label DescriptionLabel;
		/// <summary>
		/// the creation date label
		/// </summary>
		private System.Windows.Forms.Label creationdate;
		/// <summary>
		/// the creation date value
		/// </summary>
		private System.Windows.Forms.Label CreationDateLabel;
		/// <summary>
		/// the subject textbox
		/// </summary>
		private System.Windows.Forms.TextBox subject;
		/// <summary>
		/// the subject label
		/// </summary>
		private System.Windows.Forms.Label SubjectLabel;
		/// <summary>
		/// the title textbox
		/// </summary>
		private System.Windows.Forms.TextBox title;
		/// <summary>
		/// the title lable
		/// </summary>
		private System.Windows.Forms.Label TitleLabel;
		/// <summary>
		/// the author textbox
		/// </summary>
		private System.Windows.Forms.TextBox author;
		/// <summary>
		/// the author label
		/// </summary>
		private System.Windows.Forms.Label AuthorLabel;
		/// <summary>
		/// the GraphInformation this control interfaces
		/// </summary>
		private GraphInformation info;

		#endregion 


		#region Properties

		/// <summary>
		/// Gets or sets the graph information displayed by this control
		/// </summary>
		public GraphInformation GraphInformation
		{
			get{return info;}
			set{
				info = value;
				LoadData(info);
				}
		}
		#endregion




		/// <summary>
		/// Constructor
		/// </summary>
		public GraphProps()
		{			
			InitializeComponent();
			info = new GraphInformation();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="info"></param>
		public GraphProps(GraphInformation info) : this()
		{
			this.info = info;			
			LoadData(info);
		}

		/// <summary>
		/// Loads the data of the graph information into the textboxes
		/// </summary>
		/// <param name="info"></param>
		private void LoadData(GraphInformation info)
		{
			author.Text = info.Author;
			description.Text = info.Description;
			title.Text = info.Title;
			subject.Text = info.Subject;
			creationdate.Text = info.CreationDate;
		}



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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(GraphProps));
			this.description = new System.Windows.Forms.TextBox();
			this.DescriptionLabel = new System.Windows.Forms.Label();
			this.creationdate = new System.Windows.Forms.Label();
			this.CreationDateLabel = new System.Windows.Forms.Label();
			this.subject = new System.Windows.Forms.TextBox();
			this.SubjectLabel = new System.Windows.Forms.Label();
			this.title = new System.Windows.Forms.TextBox();
			this.TitleLabel = new System.Windows.Forms.Label();
			this.author = new System.Windows.Forms.TextBox();
			this.AuthorLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// description
			// 
			this.description.AccessibleDescription = resources.GetString("description.AccessibleDescription");
			this.description.AccessibleName = resources.GetString("description.AccessibleName");
			this.description.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("description.Anchor")));
			this.description.AutoSize = ((bool)(resources.GetObject("description.AutoSize")));
			this.description.BackColor = System.Drawing.Color.WhiteSmoke;
			this.description.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("description.BackgroundImage")));
			this.description.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.description.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("description.Dock")));
			this.description.Enabled = ((bool)(resources.GetObject("description.Enabled")));
			this.description.Font = ((System.Drawing.Font)(resources.GetObject("description.Font")));
			this.description.ForeColor = System.Drawing.Color.DimGray;
			this.description.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("description.ImeMode")));
			this.description.Location = ((System.Drawing.Point)(resources.GetObject("description.Location")));
			this.description.MaxLength = ((int)(resources.GetObject("description.MaxLength")));
			this.description.Multiline = ((bool)(resources.GetObject("description.Multiline")));
			this.description.Name = "description";
			this.description.PasswordChar = ((char)(resources.GetObject("description.PasswordChar")));
			this.description.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("description.RightToLeft")));
			this.description.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("description.ScrollBars")));
			this.description.Size = ((System.Drawing.Size)(resources.GetObject("description.Size")));
			this.description.TabIndex = ((int)(resources.GetObject("description.TabIndex")));
			this.description.Text = resources.GetString("description.Text");
			this.description.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("description.TextAlign")));
			this.description.Visible = ((bool)(resources.GetObject("description.Visible")));
			this.description.WordWrap = ((bool)(resources.GetObject("description.WordWrap")));
			// 
			// DescriptionLabel
			// 
			this.DescriptionLabel.AccessibleDescription = resources.GetString("DescriptionLabel.AccessibleDescription");
			this.DescriptionLabel.AccessibleName = resources.GetString("DescriptionLabel.AccessibleName");
			this.DescriptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("DescriptionLabel.Anchor")));
			this.DescriptionLabel.AutoSize = ((bool)(resources.GetObject("DescriptionLabel.AutoSize")));
			this.DescriptionLabel.BackColor = System.Drawing.Color.Transparent;
			this.DescriptionLabel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("DescriptionLabel.Dock")));
			this.DescriptionLabel.Enabled = ((bool)(resources.GetObject("DescriptionLabel.Enabled")));
			this.DescriptionLabel.Font = ((System.Drawing.Font)(resources.GetObject("DescriptionLabel.Font")));
			this.DescriptionLabel.ForeColor = System.Drawing.Color.DimGray;
			this.DescriptionLabel.Image = ((System.Drawing.Image)(resources.GetObject("DescriptionLabel.Image")));
			this.DescriptionLabel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("DescriptionLabel.ImageAlign")));
			this.DescriptionLabel.ImageIndex = ((int)(resources.GetObject("DescriptionLabel.ImageIndex")));
			this.DescriptionLabel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("DescriptionLabel.ImeMode")));
			this.DescriptionLabel.Location = ((System.Drawing.Point)(resources.GetObject("DescriptionLabel.Location")));
			this.DescriptionLabel.Name = "DescriptionLabel";
			this.DescriptionLabel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("DescriptionLabel.RightToLeft")));
			this.DescriptionLabel.Size = ((System.Drawing.Size)(resources.GetObject("DescriptionLabel.Size")));
			this.DescriptionLabel.TabIndex = ((int)(resources.GetObject("DescriptionLabel.TabIndex")));
			this.DescriptionLabel.Text = resources.GetString("DescriptionLabel.Text");
			this.DescriptionLabel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("DescriptionLabel.TextAlign")));
			this.DescriptionLabel.Visible = ((bool)(resources.GetObject("DescriptionLabel.Visible")));
			// 
			// creationdate
			// 
			this.creationdate.AccessibleDescription = resources.GetString("creationdate.AccessibleDescription");
			this.creationdate.AccessibleName = resources.GetString("creationdate.AccessibleName");
			this.creationdate.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("creationdate.Anchor")));
			this.creationdate.AutoSize = ((bool)(resources.GetObject("creationdate.AutoSize")));
			this.creationdate.BackColor = System.Drawing.Color.WhiteSmoke;
			this.creationdate.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("creationdate.Dock")));
			this.creationdate.Enabled = ((bool)(resources.GetObject("creationdate.Enabled")));
			this.creationdate.Font = ((System.Drawing.Font)(resources.GetObject("creationdate.Font")));
			this.creationdate.ForeColor = System.Drawing.Color.DimGray;
			this.creationdate.Image = ((System.Drawing.Image)(resources.GetObject("creationdate.Image")));
			this.creationdate.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("creationdate.ImageAlign")));
			this.creationdate.ImageIndex = ((int)(resources.GetObject("creationdate.ImageIndex")));
			this.creationdate.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("creationdate.ImeMode")));
			this.creationdate.Location = ((System.Drawing.Point)(resources.GetObject("creationdate.Location")));
			this.creationdate.Name = "creationdate";
			this.creationdate.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("creationdate.RightToLeft")));
			this.creationdate.Size = ((System.Drawing.Size)(resources.GetObject("creationdate.Size")));
			this.creationdate.TabIndex = ((int)(resources.GetObject("creationdate.TabIndex")));
			this.creationdate.Text = resources.GetString("creationdate.Text");
			this.creationdate.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("creationdate.TextAlign")));
			this.creationdate.Visible = ((bool)(resources.GetObject("creationdate.Visible")));
			// 
			// CreationDateLabel
			// 
			this.CreationDateLabel.AccessibleDescription = resources.GetString("CreationDateLabel.AccessibleDescription");
			this.CreationDateLabel.AccessibleName = resources.GetString("CreationDateLabel.AccessibleName");
			this.CreationDateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("CreationDateLabel.Anchor")));
			this.CreationDateLabel.AutoSize = ((bool)(resources.GetObject("CreationDateLabel.AutoSize")));
			this.CreationDateLabel.BackColor = System.Drawing.Color.Transparent;
			this.CreationDateLabel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("CreationDateLabel.Dock")));
			this.CreationDateLabel.Enabled = ((bool)(resources.GetObject("CreationDateLabel.Enabled")));
			this.CreationDateLabel.Font = ((System.Drawing.Font)(resources.GetObject("CreationDateLabel.Font")));
			this.CreationDateLabel.ForeColor = System.Drawing.Color.DimGray;
			this.CreationDateLabel.Image = ((System.Drawing.Image)(resources.GetObject("CreationDateLabel.Image")));
			this.CreationDateLabel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("CreationDateLabel.ImageAlign")));
			this.CreationDateLabel.ImageIndex = ((int)(resources.GetObject("CreationDateLabel.ImageIndex")));
			this.CreationDateLabel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("CreationDateLabel.ImeMode")));
			this.CreationDateLabel.Location = ((System.Drawing.Point)(resources.GetObject("CreationDateLabel.Location")));
			this.CreationDateLabel.Name = "CreationDateLabel";
			this.CreationDateLabel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("CreationDateLabel.RightToLeft")));
			this.CreationDateLabel.Size = ((System.Drawing.Size)(resources.GetObject("CreationDateLabel.Size")));
			this.CreationDateLabel.TabIndex = ((int)(resources.GetObject("CreationDateLabel.TabIndex")));
			this.CreationDateLabel.Text = resources.GetString("CreationDateLabel.Text");
			this.CreationDateLabel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("CreationDateLabel.TextAlign")));
			this.CreationDateLabel.Visible = ((bool)(resources.GetObject("CreationDateLabel.Visible")));
			// 
			// subject
			// 
			this.subject.AccessibleDescription = resources.GetString("subject.AccessibleDescription");
			this.subject.AccessibleName = resources.GetString("subject.AccessibleName");
			this.subject.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("subject.Anchor")));
			this.subject.AutoSize = ((bool)(resources.GetObject("subject.AutoSize")));
			this.subject.BackColor = System.Drawing.Color.WhiteSmoke;
			this.subject.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("subject.BackgroundImage")));
			this.subject.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.subject.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("subject.Dock")));
			this.subject.Enabled = ((bool)(resources.GetObject("subject.Enabled")));
			this.subject.Font = ((System.Drawing.Font)(resources.GetObject("subject.Font")));
			this.subject.ForeColor = System.Drawing.Color.DimGray;
			this.subject.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("subject.ImeMode")));
			this.subject.Location = ((System.Drawing.Point)(resources.GetObject("subject.Location")));
			this.subject.MaxLength = ((int)(resources.GetObject("subject.MaxLength")));
			this.subject.Multiline = ((bool)(resources.GetObject("subject.Multiline")));
			this.subject.Name = "subject";
			this.subject.PasswordChar = ((char)(resources.GetObject("subject.PasswordChar")));
			this.subject.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("subject.RightToLeft")));
			this.subject.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("subject.ScrollBars")));
			this.subject.Size = ((System.Drawing.Size)(resources.GetObject("subject.Size")));
			this.subject.TabIndex = ((int)(resources.GetObject("subject.TabIndex")));
			this.subject.Text = resources.GetString("subject.Text");
			this.subject.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("subject.TextAlign")));
			this.subject.Visible = ((bool)(resources.GetObject("subject.Visible")));
			this.subject.WordWrap = ((bool)(resources.GetObject("subject.WordWrap")));
			// 
			// SubjectLabel
			// 
			this.SubjectLabel.AccessibleDescription = resources.GetString("SubjectLabel.AccessibleDescription");
			this.SubjectLabel.AccessibleName = resources.GetString("SubjectLabel.AccessibleName");
			this.SubjectLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("SubjectLabel.Anchor")));
			this.SubjectLabel.AutoSize = ((bool)(resources.GetObject("SubjectLabel.AutoSize")));
			this.SubjectLabel.BackColor = System.Drawing.Color.Transparent;
			this.SubjectLabel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("SubjectLabel.Dock")));
			this.SubjectLabel.Enabled = ((bool)(resources.GetObject("SubjectLabel.Enabled")));
			this.SubjectLabel.Font = ((System.Drawing.Font)(resources.GetObject("SubjectLabel.Font")));
			this.SubjectLabel.ForeColor = System.Drawing.Color.DimGray;
			this.SubjectLabel.Image = ((System.Drawing.Image)(resources.GetObject("SubjectLabel.Image")));
			this.SubjectLabel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("SubjectLabel.ImageAlign")));
			this.SubjectLabel.ImageIndex = ((int)(resources.GetObject("SubjectLabel.ImageIndex")));
			this.SubjectLabel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("SubjectLabel.ImeMode")));
			this.SubjectLabel.Location = ((System.Drawing.Point)(resources.GetObject("SubjectLabel.Location")));
			this.SubjectLabel.Name = "SubjectLabel";
			this.SubjectLabel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("SubjectLabel.RightToLeft")));
			this.SubjectLabel.Size = ((System.Drawing.Size)(resources.GetObject("SubjectLabel.Size")));
			this.SubjectLabel.TabIndex = ((int)(resources.GetObject("SubjectLabel.TabIndex")));
			this.SubjectLabel.Text = resources.GetString("SubjectLabel.Text");
			this.SubjectLabel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("SubjectLabel.TextAlign")));
			this.SubjectLabel.Visible = ((bool)(resources.GetObject("SubjectLabel.Visible")));
			// 
			// title
			// 
			this.title.AccessibleDescription = resources.GetString("title.AccessibleDescription");
			this.title.AccessibleName = resources.GetString("title.AccessibleName");
			this.title.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("title.Anchor")));
			this.title.AutoSize = ((bool)(resources.GetObject("title.AutoSize")));
			this.title.BackColor = System.Drawing.Color.WhiteSmoke;
			this.title.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("title.BackgroundImage")));
			this.title.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.title.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("title.Dock")));
			this.title.Enabled = ((bool)(resources.GetObject("title.Enabled")));
			this.title.Font = ((System.Drawing.Font)(resources.GetObject("title.Font")));
			this.title.ForeColor = System.Drawing.Color.DimGray;
			this.title.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("title.ImeMode")));
			this.title.Location = ((System.Drawing.Point)(resources.GetObject("title.Location")));
			this.title.MaxLength = ((int)(resources.GetObject("title.MaxLength")));
			this.title.Multiline = ((bool)(resources.GetObject("title.Multiline")));
			this.title.Name = "title";
			this.title.PasswordChar = ((char)(resources.GetObject("title.PasswordChar")));
			this.title.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("title.RightToLeft")));
			this.title.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("title.ScrollBars")));
			this.title.Size = ((System.Drawing.Size)(resources.GetObject("title.Size")));
			this.title.TabIndex = ((int)(resources.GetObject("title.TabIndex")));
			this.title.Text = resources.GetString("title.Text");
			this.title.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("title.TextAlign")));
			this.title.Visible = ((bool)(resources.GetObject("title.Visible")));
			this.title.WordWrap = ((bool)(resources.GetObject("title.WordWrap")));
			// 
			// TitleLabel
			// 
			this.TitleLabel.AccessibleDescription = resources.GetString("TitleLabel.AccessibleDescription");
			this.TitleLabel.AccessibleName = resources.GetString("TitleLabel.AccessibleName");
			this.TitleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("TitleLabel.Anchor")));
			this.TitleLabel.AutoSize = ((bool)(resources.GetObject("TitleLabel.AutoSize")));
			this.TitleLabel.BackColor = System.Drawing.Color.Transparent;
			this.TitleLabel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("TitleLabel.Dock")));
			this.TitleLabel.Enabled = ((bool)(resources.GetObject("TitleLabel.Enabled")));
			this.TitleLabel.Font = ((System.Drawing.Font)(resources.GetObject("TitleLabel.Font")));
			this.TitleLabel.ForeColor = System.Drawing.Color.DimGray;
			this.TitleLabel.Image = ((System.Drawing.Image)(resources.GetObject("TitleLabel.Image")));
			this.TitleLabel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("TitleLabel.ImageAlign")));
			this.TitleLabel.ImageIndex = ((int)(resources.GetObject("TitleLabel.ImageIndex")));
			this.TitleLabel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("TitleLabel.ImeMode")));
			this.TitleLabel.Location = ((System.Drawing.Point)(resources.GetObject("TitleLabel.Location")));
			this.TitleLabel.Name = "TitleLabel";
			this.TitleLabel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("TitleLabel.RightToLeft")));
			this.TitleLabel.Size = ((System.Drawing.Size)(resources.GetObject("TitleLabel.Size")));
			this.TitleLabel.TabIndex = ((int)(resources.GetObject("TitleLabel.TabIndex")));
			this.TitleLabel.Text = resources.GetString("TitleLabel.Text");
			this.TitleLabel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("TitleLabel.TextAlign")));
			this.TitleLabel.Visible = ((bool)(resources.GetObject("TitleLabel.Visible")));
			// 
			// author
			// 
			this.author.AccessibleDescription = resources.GetString("author.AccessibleDescription");
			this.author.AccessibleName = resources.GetString("author.AccessibleName");
			this.author.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("author.Anchor")));
			this.author.AutoSize = ((bool)(resources.GetObject("author.AutoSize")));
			this.author.BackColor = System.Drawing.Color.WhiteSmoke;
			this.author.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("author.BackgroundImage")));
			this.author.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.author.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("author.Dock")));
			this.author.Enabled = ((bool)(resources.GetObject("author.Enabled")));
			this.author.Font = ((System.Drawing.Font)(resources.GetObject("author.Font")));
			this.author.ForeColor = System.Drawing.Color.DimGray;
			this.author.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("author.ImeMode")));
			this.author.Location = ((System.Drawing.Point)(resources.GetObject("author.Location")));
			this.author.MaxLength = ((int)(resources.GetObject("author.MaxLength")));
			this.author.Multiline = ((bool)(resources.GetObject("author.Multiline")));
			this.author.Name = "author";
			this.author.PasswordChar = ((char)(resources.GetObject("author.PasswordChar")));
			this.author.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("author.RightToLeft")));
			this.author.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("author.ScrollBars")));
			this.author.Size = ((System.Drawing.Size)(resources.GetObject("author.Size")));
			this.author.TabIndex = ((int)(resources.GetObject("author.TabIndex")));
			this.author.Text = resources.GetString("author.Text");
			this.author.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("author.TextAlign")));
			this.author.Visible = ((bool)(resources.GetObject("author.Visible")));
			this.author.WordWrap = ((bool)(resources.GetObject("author.WordWrap")));
			// 
			// AuthorLabel
			// 
			this.AuthorLabel.AccessibleDescription = resources.GetString("AuthorLabel.AccessibleDescription");
			this.AuthorLabel.AccessibleName = resources.GetString("AuthorLabel.AccessibleName");
			this.AuthorLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("AuthorLabel.Anchor")));
			this.AuthorLabel.AutoSize = ((bool)(resources.GetObject("AuthorLabel.AutoSize")));
			this.AuthorLabel.BackColor = System.Drawing.Color.Transparent;
			this.AuthorLabel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("AuthorLabel.Dock")));
			this.AuthorLabel.Enabled = ((bool)(resources.GetObject("AuthorLabel.Enabled")));
			this.AuthorLabel.Font = ((System.Drawing.Font)(resources.GetObject("AuthorLabel.Font")));
			this.AuthorLabel.ForeColor = System.Drawing.Color.DimGray;
			this.AuthorLabel.Image = ((System.Drawing.Image)(resources.GetObject("AuthorLabel.Image")));
			this.AuthorLabel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("AuthorLabel.ImageAlign")));
			this.AuthorLabel.ImageIndex = ((int)(resources.GetObject("AuthorLabel.ImageIndex")));
			this.AuthorLabel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("AuthorLabel.ImeMode")));
			this.AuthorLabel.Location = ((System.Drawing.Point)(resources.GetObject("AuthorLabel.Location")));
			this.AuthorLabel.Name = "AuthorLabel";
			this.AuthorLabel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("AuthorLabel.RightToLeft")));
			this.AuthorLabel.Size = ((System.Drawing.Size)(resources.GetObject("AuthorLabel.Size")));
			this.AuthorLabel.TabIndex = ((int)(resources.GetObject("AuthorLabel.TabIndex")));
			this.AuthorLabel.Text = resources.GetString("AuthorLabel.Text");
			this.AuthorLabel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("AuthorLabel.TextAlign")));
			this.AuthorLabel.Visible = ((bool)(resources.GetObject("AuthorLabel.Visible")));
			// 
			// GraphProps
			// 
			this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
			this.AccessibleName = resources.GetString("$this.AccessibleName");
			this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
			this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
			this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
			this.BackColor = System.Drawing.SystemColors.Control;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.Controls.Add(this.description);
			this.Controls.Add(this.DescriptionLabel);
			this.Controls.Add(this.creationdate);
			this.Controls.Add(this.CreationDateLabel);
			this.Controls.Add(this.subject);
			this.Controls.Add(this.SubjectLabel);
			this.Controls.Add(this.title);
			this.Controls.Add(this.TitleLabel);
			this.Controls.Add(this.author);
			this.Controls.Add(this.AuthorLabel);
			this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
			this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
			this.ForeColor = System.Drawing.Color.Orange;
			this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
			this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
			this.Name = "GraphProps";
			this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
			this.Size = ((System.Drawing.Size)(resources.GetObject("$this.Size")));
			this.ResumeLayout(false);

		}
		#endregion

	

		/// <summary>
		/// Commits the changes to the control
		/// </summary>
		public void Commit()
		{
			info.Author = author.Text.Trim();
			info.Description = description.Text.Trim();
			info.CreationDate = creationdate.Text.Trim();
			info.Title = title.Text.Trim();
			info.Subject = subject.Text.Trim();
			
		}
	}
}
