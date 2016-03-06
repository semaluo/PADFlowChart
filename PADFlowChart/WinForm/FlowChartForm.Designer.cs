namespace PADFlowChart
{
    partial class FlowChartForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FlowChartForm));
            this.flowchart_menu = new System.Windows.Forms.MenuStrip();
            this.flowchart_menu_edit = new System.Windows.Forms.ToolStripMenuItem();
            this.flowchart_menu_edit_copy = new System.Windows.Forms.ToolStripMenuItem();
            this.flowchart_menu_edit_paste = new System.Windows.Forms.ToolStripMenuItem();
            this.flowchart_menu_edit_delete = new System.Windows.Forms.ToolStripMenuItem();
            this.flowchart_menu_edit_selectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.flowchart_menu_layer = new System.Windows.Forms.ToolStripMenuItem();
            this.flowchart_menu_layer_go_upper = new System.Windows.Forms.ToolStripMenuItem();
            this.flowchart_toolbar_left = new System.Windows.Forms.ToolStrip();
            this.flowchart_toolbar_arrow = new System.Windows.Forms.ToolStripButton();
            this.flowchart_toolbar_start_shape = new System.Windows.Forms.ToolStripButton();
            this.flowchart_toolbar_sequence_shape = new System.Windows.Forms.ToolStripButton();
            this.flowchart_toolbar_loop_shape = new System.Windows.Forms.ToolStripButton();
            this.flowchart_toolbar_if_shape = new System.Windows.Forms.ToolStripButton();
            this.flowchart_toolbar_switch_shape = new System.Windows.Forms.ToolStripButton();
            this.flowchart_toolbar_block_shape = new System.Windows.Forms.ToolStripButton();
            this.flowchart_toolbar_end_shape = new System.Windows.Forms.ToolStripButton();
            this.flowchart_toolbar_labe_shape = new System.Windows.Forms.ToolStripButton();
            this.toolbar_imageList = new System.Windows.Forms.ImageList(this.components);
            this.status_panel = new System.Windows.Forms.Panel();
            this.flowchart_status_info = new System.Windows.Forms.Label();
            this.flowchart_status_coordinate = new System.Windows.Forms.Label();
            this.flowchart_toolbar_top = new System.Windows.Forms.ToolStrip();
            this.flowchart_toolbar_copy = new System.Windows.Forms.ToolStripButton();
            this.flowchart_toolbar_paste = new System.Windows.Forms.ToolStripButton();
            this.flowchart_toolbar_back_to_upperLayer = new System.Windows.Forms.ToolStripButton();
            this.flowchart_toolbar_zoomout = new System.Windows.Forms.ToolStripButton();
            this.flowchart_toolbar_zoomin = new System.Windows.Forms.ToolStripButton();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenu_go_upper_layer = new System.Windows.Forms.ToolStripMenuItem();
            this.graphControl = new Netron.GraphLib.UI.GraphControl();
            this.flowchart_menu.SuspendLayout();
            this.flowchart_toolbar_left.SuspendLayout();
            this.status_panel.SuspendLayout();
            this.flowchart_toolbar_top.SuspendLayout();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowchart_menu
            // 
            this.flowchart_menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.flowchart_menu_edit,
            this.flowchart_menu_layer});
            this.flowchart_menu.Location = new System.Drawing.Point(0, 0);
            this.flowchart_menu.Name = "flowchart_menu";
            this.flowchart_menu.Size = new System.Drawing.Size(502, 25);
            this.flowchart_menu.TabIndex = 0;
            this.flowchart_menu.Text = "menuStrip1";
            this.flowchart_menu.Visible = false;
            // 
            // flowchart_menu_edit
            // 
            this.flowchart_menu_edit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.flowchart_menu_edit_copy,
            this.flowchart_menu_edit_paste,
            this.flowchart_menu_edit_delete,
            this.flowchart_menu_edit_selectAll});
            this.flowchart_menu_edit.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.flowchart_menu_edit.MergeIndex = 2;
            this.flowchart_menu_edit.Name = "flowchart_menu_edit";
            this.flowchart_menu_edit.Size = new System.Drawing.Size(44, 21);
            this.flowchart_menu_edit.Text = "编辑";
            // 
            // flowchart_menu_edit_copy
            // 
            this.flowchart_menu_edit_copy.Name = "flowchart_menu_edit_copy";
            this.flowchart_menu_edit_copy.Size = new System.Drawing.Size(100, 22);
            this.flowchart_menu_edit_copy.Text = "复制";
            this.flowchart_menu_edit_copy.Click += new System.EventHandler(this.flowchart_menu_edit_copy_Click);
            // 
            // flowchart_menu_edit_paste
            // 
            this.flowchart_menu_edit_paste.Name = "flowchart_menu_edit_paste";
            this.flowchart_menu_edit_paste.Size = new System.Drawing.Size(100, 22);
            this.flowchart_menu_edit_paste.Text = "粘贴";
            this.flowchart_menu_edit_paste.Click += new System.EventHandler(this.flowchart_menu_edit_paste_Click);
            // 
            // flowchart_menu_edit_delete
            // 
            this.flowchart_menu_edit_delete.Name = "flowchart_menu_edit_delete";
            this.flowchart_menu_edit_delete.Size = new System.Drawing.Size(100, 22);
            this.flowchart_menu_edit_delete.Text = "删除";
            this.flowchart_menu_edit_delete.Click += new System.EventHandler(this.flowchart_menu_edit_delete_Click);
            // 
            // flowchart_menu_edit_selectAll
            // 
            this.flowchart_menu_edit_selectAll.Name = "flowchart_menu_edit_selectAll";
            this.flowchart_menu_edit_selectAll.Size = new System.Drawing.Size(100, 22);
            this.flowchart_menu_edit_selectAll.Text = "全选";
            this.flowchart_menu_edit_selectAll.Click += new System.EventHandler(this.flowchart_menu_edit_selectAll_Click);
            // 
            // flowchart_menu_layer
            // 
            this.flowchart_menu_layer.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.flowchart_menu_layer_go_upper});
            this.flowchart_menu_layer.Name = "flowchart_menu_layer";
            this.flowchart_menu_layer.Size = new System.Drawing.Size(44, 21);
            this.flowchart_menu_layer.Text = "图层";
            // 
            // flowchart_menu_layer_go_upper
            // 
            this.flowchart_menu_layer_go_upper.Name = "flowchart_menu_layer_go_upper";
            this.flowchart_menu_layer_go_upper.Size = new System.Drawing.Size(148, 22);
            this.flowchart_menu_layer_go_upper.Text = "回到上一图层";
            this.flowchart_menu_layer_go_upper.Click += new System.EventHandler(this.flowchart_menu_layer_go_upper_Click);
            // 
            // flowchart_toolbar_left
            // 
            this.flowchart_toolbar_left.Dock = System.Windows.Forms.DockStyle.Left;
            this.flowchart_toolbar_left.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.flowchart_toolbar_left.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.flowchart_toolbar_arrow,
            this.flowchart_toolbar_start_shape,
            this.flowchart_toolbar_sequence_shape,
            this.flowchart_toolbar_loop_shape,
            this.flowchart_toolbar_if_shape,
            this.flowchart_toolbar_switch_shape,
            this.flowchart_toolbar_block_shape,
            this.flowchart_toolbar_end_shape,
            this.flowchart_toolbar_labe_shape});
            this.flowchart_toolbar_left.Location = new System.Drawing.Point(0, 0);
            this.flowchart_toolbar_left.Name = "flowchart_toolbar_left";
            this.flowchart_toolbar_left.Size = new System.Drawing.Size(37, 378);
            this.flowchart_toolbar_left.TabIndex = 1;
            this.flowchart_toolbar_left.Text = "toolbar";
            // 
            // flowchart_toolbar_arrow
            // 
            this.flowchart_toolbar_arrow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.flowchart_toolbar_arrow.Image = ((System.Drawing.Image)(resources.GetObject("flowchart_toolbar_arrow.Image")));
            this.flowchart_toolbar_arrow.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.flowchart_toolbar_arrow.Name = "flowchart_toolbar_arrow";
            this.flowchart_toolbar_arrow.Size = new System.Drawing.Size(34, 36);
            this.flowchart_toolbar_arrow.Text = "arrow";
            this.flowchart_toolbar_arrow.Click += new System.EventHandler(this.flowchart_toolbar_arrow_Click);
            // 
            // flowchart_toolbar_start_shape
            // 
            this.flowchart_toolbar_start_shape.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.flowchart_toolbar_start_shape.Image = ((System.Drawing.Image)(resources.GetObject("flowchart_toolbar_start_shape.Image")));
            this.flowchart_toolbar_start_shape.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.flowchart_toolbar_start_shape.Name = "flowchart_toolbar_start_shape";
            this.flowchart_toolbar_start_shape.Size = new System.Drawing.Size(34, 36);
            this.flowchart_toolbar_start_shape.Text = "开始结构";
            this.flowchart_toolbar_start_shape.ToolTipText = "开始结构";
            this.flowchart_toolbar_start_shape.Click += new System.EventHandler(this.flowchart_toolbar_start_shape_Click);
            // 
            // flowchart_toolbar_sequence_shape
            // 
            this.flowchart_toolbar_sequence_shape.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.flowchart_toolbar_sequence_shape.Image = ((System.Drawing.Image)(resources.GetObject("flowchart_toolbar_sequence_shape.Image")));
            this.flowchart_toolbar_sequence_shape.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.flowchart_toolbar_sequence_shape.Name = "flowchart_toolbar_sequence_shape";
            this.flowchart_toolbar_sequence_shape.Size = new System.Drawing.Size(34, 36);
            this.flowchart_toolbar_sequence_shape.Text = "顺序结构";
            this.flowchart_toolbar_sequence_shape.Click += new System.EventHandler(this.flowchart_toolbar_sequence_shape_Click);
            // 
            // flowchart_toolbar_loop_shape
            // 
            this.flowchart_toolbar_loop_shape.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.flowchart_toolbar_loop_shape.Image = ((System.Drawing.Image)(resources.GetObject("flowchart_toolbar_loop_shape.Image")));
            this.flowchart_toolbar_loop_shape.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.flowchart_toolbar_loop_shape.Name = "flowchart_toolbar_loop_shape";
            this.flowchart_toolbar_loop_shape.Size = new System.Drawing.Size(34, 36);
            this.flowchart_toolbar_loop_shape.Text = "循环结构";
            this.flowchart_toolbar_loop_shape.Click += new System.EventHandler(this.flowchart_toolbar_loop_shape_Click);
            // 
            // flowchart_toolbar_if_shape
            // 
            this.flowchart_toolbar_if_shape.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.flowchart_toolbar_if_shape.Image = ((System.Drawing.Image)(resources.GetObject("flowchart_toolbar_if_shape.Image")));
            this.flowchart_toolbar_if_shape.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.flowchart_toolbar_if_shape.Name = "flowchart_toolbar_if_shape";
            this.flowchart_toolbar_if_shape.Size = new System.Drawing.Size(34, 36);
            this.flowchart_toolbar_if_shape.Text = "分支结构";
            this.flowchart_toolbar_if_shape.Click += new System.EventHandler(this.flowchart_toolbar_if_shape_Click);
            // 
            // flowchart_toolbar_switch_shape
            // 
            this.flowchart_toolbar_switch_shape.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.flowchart_toolbar_switch_shape.Image = ((System.Drawing.Image)(resources.GetObject("flowchart_toolbar_switch_shape.Image")));
            this.flowchart_toolbar_switch_shape.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.flowchart_toolbar_switch_shape.Name = "flowchart_toolbar_switch_shape";
            this.flowchart_toolbar_switch_shape.Size = new System.Drawing.Size(34, 36);
            this.flowchart_toolbar_switch_shape.Text = "多分支结构";
            this.flowchart_toolbar_switch_shape.Click += new System.EventHandler(this.flowchart_toolbar_switch_shape_Click);
            // 
            // flowchart_toolbar_block_shape
            // 
            this.flowchart_toolbar_block_shape.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.flowchart_toolbar_block_shape.Image = ((System.Drawing.Image)(resources.GetObject("flowchart_toolbar_block_shape.Image")));
            this.flowchart_toolbar_block_shape.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.flowchart_toolbar_block_shape.Name = "flowchart_toolbar_block_shape";
            this.flowchart_toolbar_block_shape.Size = new System.Drawing.Size(34, 36);
            this.flowchart_toolbar_block_shape.Text = "块结构";
            this.flowchart_toolbar_block_shape.Click += new System.EventHandler(this.flowchart_toolbar_block_shape_Click);
            // 
            // flowchart_toolbar_end_shape
            // 
            this.flowchart_toolbar_end_shape.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.flowchart_toolbar_end_shape.Image = ((System.Drawing.Image)(resources.GetObject("flowchart_toolbar_end_shape.Image")));
            this.flowchart_toolbar_end_shape.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.flowchart_toolbar_end_shape.Name = "flowchart_toolbar_end_shape";
            this.flowchart_toolbar_end_shape.Size = new System.Drawing.Size(34, 36);
            this.flowchart_toolbar_end_shape.Text = "结束结构";
            this.flowchart_toolbar_end_shape.Click += new System.EventHandler(this.flowchart_toolbar_end_shape_Click);
            // 
            // flowchart_toolbar_labe_shape
            // 
            this.flowchart_toolbar_labe_shape.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.flowchart_toolbar_labe_shape.Image = ((System.Drawing.Image)(resources.GetObject("flowchart_toolbar_labe_shape.Image")));
            this.flowchart_toolbar_labe_shape.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.flowchart_toolbar_labe_shape.Name = "flowchart_toolbar_labe_shape";
            this.flowchart_toolbar_labe_shape.Size = new System.Drawing.Size(34, 36);
            this.flowchart_toolbar_labe_shape.Text = "标签结构";
            this.flowchart_toolbar_labe_shape.Click += new System.EventHandler(this.flowchart_toolbar_labe_shape_Click);
            // 
            // toolbar_imageList
            // 
            this.toolbar_imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("toolbar_imageList.ImageStream")));
            this.toolbar_imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.toolbar_imageList.Images.SetKeyName(0, "cursor_arrow.png");
            this.toolbar_imageList.Images.SetKeyName(1, "btn_sequence.bmp");
            this.toolbar_imageList.Images.SetKeyName(2, "btn_loop.bmp");
            this.toolbar_imageList.Images.SetKeyName(3, "btn_if.bmp");
            this.toolbar_imageList.Images.SetKeyName(4, "btn_switch.bmp");
            // 
            // status_panel
            // 
            this.status_panel.Controls.Add(this.flowchart_status_info);
            this.status_panel.Controls.Add(this.flowchart_status_coordinate);
            this.status_panel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.status_panel.Location = new System.Drawing.Point(37, 348);
            this.status_panel.Name = "status_panel";
            this.status_panel.Size = new System.Drawing.Size(465, 30);
            this.status_panel.TabIndex = 2;
            // 
            // flowchart_status_info
            // 
            this.flowchart_status_info.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowchart_status_info.Location = new System.Drawing.Point(0, 0);
            this.flowchart_status_info.Name = "flowchart_status_info";
            this.flowchart_status_info.Size = new System.Drawing.Size(327, 30);
            this.flowchart_status_info.TabIndex = 1;
            this.flowchart_status_info.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // flowchart_status_coordinate
            // 
            this.flowchart_status_coordinate.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowchart_status_coordinate.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.flowchart_status_coordinate.Location = new System.Drawing.Point(327, 0);
            this.flowchart_status_coordinate.Name = "flowchart_status_coordinate";
            this.flowchart_status_coordinate.Size = new System.Drawing.Size(138, 30);
            this.flowchart_status_coordinate.TabIndex = 0;
            this.flowchart_status_coordinate.Text = "X : 1000  Y : 1000";
            this.flowchart_status_coordinate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // flowchart_toolbar_top
            // 
            this.flowchart_toolbar_top.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.flowchart_toolbar_copy,
            this.flowchart_toolbar_paste,
            this.flowchart_toolbar_back_to_upperLayer,
            this.flowchart_toolbar_zoomout,
            this.flowchart_toolbar_zoomin});
            this.flowchart_toolbar_top.Location = new System.Drawing.Point(37, 0);
            this.flowchart_toolbar_top.Name = "flowchart_toolbar_top";
            this.flowchart_toolbar_top.Size = new System.Drawing.Size(465, 25);
            this.flowchart_toolbar_top.TabIndex = 4;
            this.flowchart_toolbar_top.Text = "toolStrip1";
            this.flowchart_toolbar_top.Visible = false;
            // 
            // flowchart_toolbar_copy
            // 
            this.flowchart_toolbar_copy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.flowchart_toolbar_copy.Image = ((System.Drawing.Image)(resources.GetObject("flowchart_toolbar_copy.Image")));
            this.flowchart_toolbar_copy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.flowchart_toolbar_copy.Name = "flowchart_toolbar_copy";
            this.flowchart_toolbar_copy.Size = new System.Drawing.Size(23, 22);
            this.flowchart_toolbar_copy.Text = "复制";
            this.flowchart_toolbar_copy.Click += new System.EventHandler(this.flowchart_menu_edit_copy_Click);
            // 
            // flowchart_toolbar_paste
            // 
            this.flowchart_toolbar_paste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.flowchart_toolbar_paste.Image = ((System.Drawing.Image)(resources.GetObject("flowchart_toolbar_paste.Image")));
            this.flowchart_toolbar_paste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.flowchart_toolbar_paste.Name = "flowchart_toolbar_paste";
            this.flowchart_toolbar_paste.Size = new System.Drawing.Size(23, 22);
            this.flowchart_toolbar_paste.Text = "粘贴";
            this.flowchart_toolbar_paste.Click += new System.EventHandler(this.flowchart_menu_edit_paste_Click);
            // 
            // flowchart_toolbar_back_to_upperLayer
            // 
            this.flowchart_toolbar_back_to_upperLayer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.flowchart_toolbar_back_to_upperLayer.Image = ((System.Drawing.Image)(resources.GetObject("flowchart_toolbar_back_to_upperLayer.Image")));
            this.flowchart_toolbar_back_to_upperLayer.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.flowchart_toolbar_back_to_upperLayer.Name = "flowchart_toolbar_back_to_upperLayer";
            this.flowchart_toolbar_back_to_upperLayer.Size = new System.Drawing.Size(23, 22);
            this.flowchart_toolbar_back_to_upperLayer.Text = "回到上一图层";
            this.flowchart_toolbar_back_to_upperLayer.Click += new System.EventHandler(this.flowchart_menu_layer_go_upper_Click);
            // 
            // flowchart_toolbar_zoomout
            // 
            this.flowchart_toolbar_zoomout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.flowchart_toolbar_zoomout.Image = ((System.Drawing.Image)(resources.GetObject("flowchart_toolbar_zoomout.Image")));
            this.flowchart_toolbar_zoomout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.flowchart_toolbar_zoomout.Name = "flowchart_toolbar_zoomout";
            this.flowchart_toolbar_zoomout.Size = new System.Drawing.Size(23, 22);
            this.flowchart_toolbar_zoomout.Text = "放大";
            this.flowchart_toolbar_zoomout.Click += new System.EventHandler(this.flowchart_toolbar_zoomout_Click);
            // 
            // flowchart_toolbar_zoomin
            // 
            this.flowchart_toolbar_zoomin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.flowchart_toolbar_zoomin.Image = ((System.Drawing.Image)(resources.GetObject("flowchart_toolbar_zoomin.Image")));
            this.flowchart_toolbar_zoomin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.flowchart_toolbar_zoomin.Name = "flowchart_toolbar_zoomin";
            this.flowchart_toolbar_zoomin.Size = new System.Drawing.Size(23, 22);
            this.flowchart_toolbar_zoomin.Text = "缩小";
            this.flowchart_toolbar_zoomin.Click += new System.EventHandler(this.flowchart_toolbar_zoomin_Click);
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextMenu_go_upper_layer});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(161, 26);
            // 
            // contextMenu_go_upper_layer
            // 
            this.contextMenu_go_upper_layer.Name = "contextMenu_go_upper_layer";
            this.contextMenu_go_upper_layer.Size = new System.Drawing.Size(160, 22);
            this.contextMenu_go_upper_layer.Text = "返回上一层图层";
            this.contextMenu_go_upper_layer.Click += new System.EventHandler(this.flowchart_menu_layer_go_upper_Click);
            // 
            // graphControl
            // 
            this.graphControl.AllowAddConnection = true;
            this.graphControl.AllowAddShape = true;
            this.graphControl.AllowDeleteShape = true;
            this.graphControl.AllowDrop = true;
            this.graphControl.AllowMoveShape = true;
            this.graphControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.graphControl.AutomataPulse = 10;
            this.graphControl.AutoScroll = true;
            this.graphControl.BackgroundColor = System.Drawing.Color.White;
            this.graphControl.BackgroundImagePath = null;
            this.graphControl.BackgroundType = Netron.GraphLib.CanvasBackgroundType.FlatColor;
            this.graphControl.ContextMenuStrip = this.contextMenu;
            this.graphControl.DefaultConnectionEnd = Netron.GraphLib.ConnectionEnd.NoEnds;
            this.graphControl.DefaultConnectionPath = "Default";
            this.graphControl.DefaultShapeHeight = 50F;
            this.graphControl.DefaultShapeWidth = 150F;
            this.graphControl.DoTrack = false;
            this.graphControl.EnableContextMenu = true;
            this.graphControl.EnableLayout = false;
            this.graphControl.EnableToolTip = false;
            this.graphControl.FileName = null;
            this.graphControl.GradientBottom = System.Drawing.Color.White;
            this.graphControl.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.graphControl.GradientTop = System.Drawing.Color.LightSteelBlue;
            this.graphControl.GraphLayoutAlgorithm = Netron.GraphLib.GraphLayoutAlgorithms.SpringEmbedder;
            this.graphControl.GridSize = 20;
            this.graphControl.IsDirty = false;
            this.graphControl.LastShape = null;
            this.graphControl.Location = new System.Drawing.Point(37, 0);
            this.graphControl.Name = "graphControl";
            this.graphControl.RestrictToCanvas = false;
            this.graphControl.ShowAutomataController = false;
            this.graphControl.ShowGrid = false;
            this.graphControl.Size = new System.Drawing.Size(465, 345);
            this.graphControl.Snap = false;
            this.graphControl.TabIndex = 3;
            this.graphControl.Text = "graphControl";
            this.graphControl.Zoom = 1F;
            this.graphControl.OnDirtyChanged += new Netron.GraphLib.DirtyChanged(this.graphControl_OnDirtyChanged);
            // 
            // FlowChartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 378);
            this.Controls.Add(this.flowchart_toolbar_top);
            this.Controls.Add(this.graphControl);
            this.Controls.Add(this.status_panel);
            this.Controls.Add(this.flowchart_toolbar_left);
            this.Controls.Add(this.flowchart_menu);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MainMenuStrip = this.flowchart_menu;
            this.Name = "FlowChartForm";
            this.Text = "FlowChart";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FlowChartForm_FormClosing);
            this.Load += new System.EventHandler(this.FlowChartForm_Load);
            this.flowchart_menu.ResumeLayout(false);
            this.flowchart_menu.PerformLayout();
            this.flowchart_toolbar_left.ResumeLayout(false);
            this.flowchart_toolbar_left.PerformLayout();
            this.status_panel.ResumeLayout(false);
            this.flowchart_toolbar_top.ResumeLayout(false);
            this.flowchart_toolbar_top.PerformLayout();
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip flowchart_menu;
        private System.Windows.Forms.ToolStripMenuItem flowchart_menu_edit;
        private System.Windows.Forms.ToolStripMenuItem flowchart_menu_edit_copy;
        private System.Windows.Forms.ToolStripMenuItem flowchart_menu_edit_paste;
        private System.Windows.Forms.ToolStripMenuItem flowchart_menu_edit_delete;
        private System.Windows.Forms.ToolStripMenuItem flowchart_menu_edit_selectAll;
        private System.Windows.Forms.ToolStrip flowchart_toolbar_left;
        private System.Windows.Forms.ToolStripButton flowchart_toolbar_arrow;
        private System.Windows.Forms.ToolStripButton flowchart_toolbar_start_shape;
        private System.Windows.Forms.ToolStripButton flowchart_toolbar_sequence_shape;
        private System.Windows.Forms.ToolStripButton flowchart_toolbar_if_shape;
        private System.Windows.Forms.ToolStripButton flowchart_toolbar_loop_shape;
        private System.Windows.Forms.ImageList toolbar_imageList;
        private System.Windows.Forms.ToolStripButton flowchart_toolbar_switch_shape;
        private System.Windows.Forms.ToolStripButton flowchart_toolbar_end_shape;
        private System.Windows.Forms.Panel status_panel;
        private System.Windows.Forms.Label flowchart_status_info;
        private System.Windows.Forms.Label flowchart_status_coordinate;
        private Netron.GraphLib.UI.GraphControl graphControl;
        private System.Windows.Forms.ToolStrip flowchart_toolbar_top;
        private System.Windows.Forms.ToolStripButton flowchart_toolbar_copy;
        private System.Windows.Forms.ToolStripButton flowchart_toolbar_paste;
        private System.Windows.Forms.ToolStripButton flowchart_toolbar_block_shape;
        private System.Windows.Forms.ToolStripButton flowchart_toolbar_back_to_upperLayer;
        private System.Windows.Forms.ToolStripMenuItem flowchart_menu_layer;
        private System.Windows.Forms.ToolStripMenuItem flowchart_menu_layer_go_upper;
        private System.Windows.Forms.ToolStripButton flowchart_toolbar_zoomout;
        private System.Windows.Forms.ToolStripButton flowchart_toolbar_zoomin;
        private System.Windows.Forms.ToolStripButton flowchart_toolbar_labe_shape;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem contextMenu_go_upper_layer;
    }
}