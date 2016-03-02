namespace PADFlowChart
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip_main = new System.Windows.Forms.MenuStrip();
            this.menu_file = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_file_new = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_file_open = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_file_close = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_file_save = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_file_saveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_file_saveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_edit = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_window = new System.Windows.Forms.ToolStripMenuItem();
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.toolbar_new = new System.Windows.Forms.ToolStripButton();
            this.toolbar_open = new System.Windows.Forms.ToolStripButton();
            this.toolbar_save = new System.Windows.Forms.ToolStripButton();
            this.toolbar_saveAll = new System.Windows.Forms.ToolStripButton();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.menuStrip_main.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip_main
            // 
            this.menuStrip_main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_file,
            this.menu_edit,
            this.menu_window});
            this.menuStrip_main.Location = new System.Drawing.Point(0, 0);
            this.menuStrip_main.Name = "menuStrip_main";
            this.menuStrip_main.Size = new System.Drawing.Size(584, 25);
            this.menuStrip_main.TabIndex = 0;
            // 
            // menu_file
            // 
            this.menu_file.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_file_new,
            this.menu_file_open,
            this.menu_file_close,
            this.menu_file_save,
            this.menu_file_saveAs,
            this.menu_file_saveAll});
            this.menu_file.MergeIndex = 1;
            this.menu_file.Name = "menu_file";
            this.menu_file.Size = new System.Drawing.Size(44, 21);
            this.menu_file.Text = "文件";
            // 
            // menu_file_new
            // 
            this.menu_file_new.Name = "menu_file_new";
            this.menu_file_new.Size = new System.Drawing.Size(124, 22);
            this.menu_file_new.Text = "新建";
            this.menu_file_new.Click += new System.EventHandler(this.menu_file_new_Click);
            // 
            // menu_file_open
            // 
            this.menu_file_open.Name = "menu_file_open";
            this.menu_file_open.Size = new System.Drawing.Size(124, 22);
            this.menu_file_open.Text = "打开";
            this.menu_file_open.Click += new System.EventHandler(this.menu_file_open_Click);
            // 
            // menu_file_close
            // 
            this.menu_file_close.Name = "menu_file_close";
            this.menu_file_close.Size = new System.Drawing.Size(124, 22);
            this.menu_file_close.Text = "关闭";
            // 
            // menu_file_save
            // 
            this.menu_file_save.Name = "menu_file_save";
            this.menu_file_save.Size = new System.Drawing.Size(124, 22);
            this.menu_file_save.Text = "保存";
            this.menu_file_save.Click += new System.EventHandler(this.menu_file_save_Click);
            // 
            // menu_file_saveAs
            // 
            this.menu_file_saveAs.Name = "menu_file_saveAs";
            this.menu_file_saveAs.Size = new System.Drawing.Size(124, 22);
            this.menu_file_saveAs.Text = "另存为";
            this.menu_file_saveAs.Click += new System.EventHandler(this.menu_file_saveAs_Click);
            // 
            // menu_file_saveAll
            // 
            this.menu_file_saveAll.Name = "menu_file_saveAll";
            this.menu_file_saveAll.Size = new System.Drawing.Size(124, 22);
            this.menu_file_saveAll.Text = "全部保存";
            this.menu_file_saveAll.Click += new System.EventHandler(this.menu_file_saveAll_Click);
            // 
            // menu_edit
            // 
            this.menu_edit.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.menu_edit.MergeIndex = 2;
            this.menu_edit.Name = "menu_edit";
            this.menu_edit.Size = new System.Drawing.Size(44, 21);
            this.menu_edit.Text = "编辑";
            // 
            // menu_window
            // 
            this.menu_window.MergeIndex = 3;
            this.menu_window.Name = "menu_window";
            this.menu_window.Size = new System.Drawing.Size(44, 21);
            this.menu_window.Text = "窗口";
            // 
            // dockPanel
            // 
            this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel.Location = new System.Drawing.Point(0, 51);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.ShowDocumentIcon = true;
            this.dockPanel.Size = new System.Drawing.Size(584, 363);
            this.dockPanel.TabIndex = 2;
            // 
            // toolbar_new
            // 
            this.toolbar_new.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbar_new.Image = ((System.Drawing.Image)(resources.GetObject("toolbar_new.Image")));
            this.toolbar_new.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.toolbar_new.Name = "toolbar_new";
            this.toolbar_new.Size = new System.Drawing.Size(23, 23);
            this.toolbar_new.Text = "新建";
            this.toolbar_new.Click += new System.EventHandler(this.menu_file_new_Click);
            // 
            // toolbar_open
            // 
            this.toolbar_open.AutoSize = false;
            this.toolbar_open.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbar_open.Image = ((System.Drawing.Image)(resources.GetObject("toolbar_open.Image")));
            this.toolbar_open.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbar_open.Name = "toolbar_open";
            this.toolbar_open.Size = new System.Drawing.Size(23, 23);
            this.toolbar_open.Text = "打开";
            this.toolbar_open.Click += new System.EventHandler(this.menu_file_open_Click);
            // 
            // toolbar_save
            // 
            this.toolbar_save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbar_save.Image = ((System.Drawing.Image)(resources.GetObject("toolbar_save.Image")));
            this.toolbar_save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbar_save.Name = "toolbar_save";
            this.toolbar_save.Size = new System.Drawing.Size(23, 23);
            this.toolbar_save.Text = "保存";
            this.toolbar_save.Click += new System.EventHandler(this.menu_file_save_Click);
            // 
            // toolbar_saveAll
            // 
            this.toolbar_saveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbar_saveAll.Image = ((System.Drawing.Image)(resources.GetObject("toolbar_saveAll.Image")));
            this.toolbar_saveAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbar_saveAll.Name = "toolbar_saveAll";
            this.toolbar_saveAll.Size = new System.Drawing.Size(23, 23);
            this.toolbar_saveAll.Text = "全部保存";
            this.toolbar_saveAll.Click += new System.EventHandler(this.menu_file_saveAll_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolbar_new,
            this.toolbar_open,
            this.toolbar_save,
            this.toolbar_saveAll});
            this.toolStrip.Location = new System.Drawing.Point(0, 25);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(584, 26);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 414);
            this.Controls.Add(this.dockPanel);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip_main);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip_main;
            this.Name = "MainForm";
            this.Text = "PAD流程图";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MdiChildActivate += new System.EventHandler(this.MainForm_MdiChildActivate);
            this.menuStrip_main.ResumeLayout(false);
            this.menuStrip_main.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip_main;
        private System.Windows.Forms.ToolStripMenuItem menu_file;
        private System.Windows.Forms.ToolStripMenuItem menu_file_new;
        private System.Windows.Forms.ToolStripMenuItem menu_file_open;
        private System.Windows.Forms.ToolStripMenuItem menu_file_close;
        private System.Windows.Forms.ToolStripMenuItem menu_file_saveAll;
        private System.Windows.Forms.ToolStripMenuItem menu_file_saveAs;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
        private System.Windows.Forms.ToolStripMenuItem menu_window;
        private System.Windows.Forms.ToolStripMenuItem menu_edit;
        private System.Windows.Forms.ToolStripButton toolbar_new;
        private System.Windows.Forms.ToolStripButton toolbar_open;
        private System.Windows.Forms.ToolStripButton toolbar_save;
        private System.Windows.Forms.ToolStripButton toolbar_saveAll;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripMenuItem menu_file_save;
    }
}

