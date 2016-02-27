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
            WeifenLuo.WinFormsUI.Docking.DockPanelSkin dockPanelSkin2 = new WeifenLuo.WinFormsUI.Docking.DockPanelSkin();
            WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin autoHideStripSkin2 = new WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient4 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient8 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin dockPaneStripSkin2 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient dockPaneStripGradient2 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient9 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient5 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient10 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient2 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient11 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient12 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient6 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient13 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient14 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip_main = new System.Windows.Forms.MenuStrip();
            this.menu_file = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_file_new = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_file_open = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_file_close = new System.Windows.Forms.ToolStripMenuItem();
            this.全部保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_file_saveAsPicture = new System.Windows.Forms.ToolStripMenuItem();
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
            this.全部保存ToolStripMenuItem,
            this.menu_file_saveAsPicture});
            this.menu_file.MergeIndex = 1;
            this.menu_file.Name = "menu_file";
            this.menu_file.Size = new System.Drawing.Size(44, 21);
            this.menu_file.Text = "文件";
            // 
            // menu_file_new
            // 
            this.menu_file_new.Name = "menu_file_new";
            this.menu_file_new.Size = new System.Drawing.Size(152, 22);
            this.menu_file_new.Text = "新建";
            this.menu_file_new.Click += new System.EventHandler(this.menu_file_new_Click);
            // 
            // menu_file_open
            // 
            this.menu_file_open.Name = "menu_file_open";
            this.menu_file_open.Size = new System.Drawing.Size(152, 22);
            this.menu_file_open.Text = "打开";
            this.menu_file_open.Click += new System.EventHandler(this.menu_file_open_Click);
            // 
            // menu_file_close
            // 
            this.menu_file_close.Name = "menu_file_close";
            this.menu_file_close.Size = new System.Drawing.Size(152, 22);
            this.menu_file_close.Text = "关闭";
            // 
            // 全部保存ToolStripMenuItem
            // 
            this.全部保存ToolStripMenuItem.Name = "全部保存ToolStripMenuItem";
            this.全部保存ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.全部保存ToolStripMenuItem.Text = "全部保存";
            // 
            // menu_file_saveAsPicture
            // 
            this.menu_file_saveAsPicture.Name = "menu_file_saveAsPicture";
            this.menu_file_saveAsPicture.Size = new System.Drawing.Size(152, 22);
            this.menu_file_saveAsPicture.Text = "另存为图片";
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
            this.dockPanel.Size = new System.Drawing.Size(584, 363);
            dockPanelGradient4.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient4.StartColor = System.Drawing.SystemColors.ControlLight;
            autoHideStripSkin2.DockStripGradient = dockPanelGradient4;
            tabGradient8.EndColor = System.Drawing.SystemColors.Control;
            tabGradient8.StartColor = System.Drawing.SystemColors.Control;
            tabGradient8.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            autoHideStripSkin2.TabGradient = tabGradient8;
            autoHideStripSkin2.TextFont = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            dockPanelSkin2.AutoHideStripSkin = autoHideStripSkin2;
            tabGradient9.EndColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient9.StartColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient9.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient2.ActiveTabGradient = tabGradient9;
            dockPanelGradient5.EndColor = System.Drawing.SystemColors.Control;
            dockPanelGradient5.StartColor = System.Drawing.SystemColors.Control;
            dockPaneStripGradient2.DockStripGradient = dockPanelGradient5;
            tabGradient10.EndColor = System.Drawing.SystemColors.ControlLight;
            tabGradient10.StartColor = System.Drawing.SystemColors.ControlLight;
            tabGradient10.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient2.InactiveTabGradient = tabGradient10;
            dockPaneStripSkin2.DocumentGradient = dockPaneStripGradient2;
            dockPaneStripSkin2.TextFont = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            tabGradient11.EndColor = System.Drawing.SystemColors.ActiveCaption;
            tabGradient11.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient11.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
            tabGradient11.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
            dockPaneStripToolWindowGradient2.ActiveCaptionGradient = tabGradient11;
            tabGradient12.EndColor = System.Drawing.SystemColors.Control;
            tabGradient12.StartColor = System.Drawing.SystemColors.Control;
            tabGradient12.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient2.ActiveTabGradient = tabGradient12;
            dockPanelGradient6.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient6.StartColor = System.Drawing.SystemColors.ControlLight;
            dockPaneStripToolWindowGradient2.DockStripGradient = dockPanelGradient6;
            tabGradient13.EndColor = System.Drawing.SystemColors.InactiveCaption;
            tabGradient13.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient13.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient13.TextColor = System.Drawing.SystemColors.InactiveCaptionText;
            dockPaneStripToolWindowGradient2.InactiveCaptionGradient = tabGradient13;
            tabGradient14.EndColor = System.Drawing.Color.Transparent;
            tabGradient14.StartColor = System.Drawing.Color.Transparent;
            tabGradient14.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            dockPaneStripToolWindowGradient2.InactiveTabGradient = tabGradient14;
            dockPaneStripSkin2.ToolWindowGradient = dockPaneStripToolWindowGradient2;
            dockPanelSkin2.DockPaneStripSkin = dockPaneStripSkin2;
            this.dockPanel.Skin = dockPanelSkin2;
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
            this.toolbar_open.Text = "Open";
            this.toolbar_open.Click += new System.EventHandler(this.menu_file_open_Click);
            // 
            // toolbar_save
            // 
            this.toolbar_save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbar_save.Image = ((System.Drawing.Image)(resources.GetObject("toolbar_save.Image")));
            this.toolbar_save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbar_save.Name = "toolbar_save";
            this.toolbar_save.Size = new System.Drawing.Size(23, 23);
            this.toolbar_save.Text = "Save";
            // 
            // toolbar_saveAll
            // 
            this.toolbar_saveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbar_saveAll.Image = ((System.Drawing.Image)(resources.GetObject("toolbar_saveAll.Image")));
            this.toolbar_saveAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbar_saveAll.Name = "toolbar_saveAll";
            this.toolbar_saveAll.Size = new System.Drawing.Size(23, 23);
            this.toolbar_saveAll.Text = "SaveAll";
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
        private System.Windows.Forms.ToolStripMenuItem 全部保存ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menu_file_saveAsPicture;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
        private System.Windows.Forms.ToolStripMenuItem menu_window;
        private System.Windows.Forms.ToolStripMenuItem menu_edit;
        private System.Windows.Forms.ToolStripButton toolbar_new;
        private System.Windows.Forms.ToolStripButton toolbar_open;
        private System.Windows.Forms.ToolStripButton toolbar_save;
        private System.Windows.Forms.ToolStripButton toolbar_saveAll;
        private System.Windows.Forms.ToolStrip toolStrip;
    }
}

