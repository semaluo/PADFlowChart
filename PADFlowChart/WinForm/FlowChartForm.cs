using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Netron.GraphLib;
using WeifenLuo.WinFormsUI.Docking;

namespace PADFlowChart
{
    public partial class FlowChartForm : DockContent, IMergeToolStrip
    {
        private string m_fullFileName;

        public event PropertiesInfo OnShowProperties
        {
            add
            {
                graphControl.OnShowProperties += value;
            }
            remove
            {
                graphControl.OnShowProperties -= value;
            }
        }

        public ToolStrip MergeToolStrip
        {
            get
            {
                return flowchart_toolbar_top;
            }
        }

        public string FullFileName
        {
            get
            {
                if (string.IsNullOrEmpty(m_fullFileName))
                {
                    m_fullFileName = string.Empty;
                    using (SaveFileDialog sfd = new SaveFileDialog())
                    {
                        sfd.DefaultExt = ".pfc";
                        sfd.Filter = "PAD Flow Chart file(*.pfc)|*.pfc|jpg file(*.jpg)|*.jpg";

                        Text = Text.Trim();
                        if (Text.Length > 0)
                        {
                            if (Text.EndsWith("*"))
                            {
                                sfd.FileName = Text.Substring(0, Text.Length - 1);
                            }
                        }

                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            m_fullFileName = sfd.FileName;
                            if (Path.GetExtension(m_fullFileName).Trim().ToUpper() == ".PFC")
                            {
                                Text = Path.GetFileName(m_fullFileName);
                            }
                        }
                    }
                }

                return m_fullFileName;
            }
            set
            {
                m_fullFileName = value;
            }
        }

        public FlowChartForm()
        {
            InitializeComponent();
        }




        private void UncheckToolbar()
        {
            foreach (ToolStripButton button in flowchart_toolbar_left.Items)
            {
                button.Checked = false;
            }
        }


        private void flowchart_toolbar_arrow_Click(object sender, EventArgs e)
        {
            UncheckToolbar();
            graphControl.EndDrawShapeWithMouse();
        }

        private void flowchart_toolbar_start_shape_Click(object sender, EventArgs e)
        {
            UncheckToolbar();
            flowchart_toolbar_start_shape.Checked = true;
            StartShape shape = new StartShape();
            graphControl.StartDrawShapeWithMouse(shape);
        }

        private void flowchart_toolbar_sequence_shape_Click(object sender, EventArgs e)
        {
            UncheckToolbar();
            flowchart_toolbar_sequence_shape.Checked = true;
            SequenceShape shape = new SequenceShape();
            graphControl.StartDrawShapeWithMouse(shape);
            //ShapeDrawTool.DrawShape(typeof(SequenceShape), graphControl);

        }

        private void flowchart_toolbar_loop_shape_Click(object sender, EventArgs e)
        {
            UncheckToolbar();
            flowchart_toolbar_loop_shape.Checked = true;
            LoopShape shape = new LoopShape();
            graphControl.StartDrawShapeWithMouse(shape);
        }

        private void flowchart_toolbar_if_shape_Click(object sender, EventArgs e)
        {
            UncheckToolbar();
            flowchart_toolbar_if_shape.Checked = true;
            IfShape shape = new IfShape();
            graphControl.StartDrawShapeWithMouse(shape);

        }

        private void flowchart_toolbar_switch_shape_Click(object sender, EventArgs e)
        {
            UncheckToolbar();
            flowchart_toolbar_switch_shape.Checked = true;
            using (InputDialog idlg = new InputDialog())
            {
                idlg.Value = 3.ToString();
                idlg.Message = "请输入分支结构的分支数：";
                if (idlg.ShowDialog() == DialogResult.OK)
                {
                    int n;
                    if (int.TryParse(idlg.Value, out n))
                    {
                        SwitchShape shape = new SwitchShape(n);
                        graphControl.StartDrawShapeWithMouse(shape);

                    }
                    else
                    {
                        graphControl.EndDrawShapeWithMouse();
                    }
                }
                else
                {
                    graphControl.EndDrawShapeWithMouse();
                }
            }
            
        }

        private void flowchart_toolbar_block_shape_Click(object sender, EventArgs e)
        {
            UncheckToolbar();
            flowchart_toolbar_block_shape.Checked = true;
            BlockShape shape = new BlockShape();
            graphControl.StartDrawShapeWithMouse(shape);
        }

        private void flowchart_toolbar_end_shape_Click(object sender, EventArgs e)
        {
            UncheckToolbar();
            flowchart_toolbar_end_shape.Checked = true;
            EndShape shape = new EndShape();
            graphControl.StartDrawShapeWithMouse(shape);

        }

        private void flowchart_toolbar_labe_shape_Click(object sender, EventArgs e)
        {
            UncheckToolbar();
            flowchart_toolbar_end_shape.Checked = true;
            LabelShape shape = new LabelShape();
            graphControl.StartDrawShapeWithMouse(shape);

        }

        private void flowchart_menu_edit_selectAll_Click(object sender, EventArgs e)
        {
            graphControl.SelectAll(true);
        }

        private void flowchart_menu_edit_delete_Click(object sender, EventArgs e)
        {
            graphControl.Delete();
        }

        private void flowchart_menu_edit_paste_Click(object sender, EventArgs e)
        {
            graphControl.Paste();
        }

        private void flowchart_menu_edit_copy_Click(object sender, EventArgs e)
        {
            graphControl.Copy();
        }

        public void OpenFile(string fileName)
        {
            if (!graphControl.IsDirty)
            {
                graphControl.Open(fileName);
                m_fullFileName = fileName;
                Text = Path.GetFileName(fileName);
                ToolTipText = Text;
                graphControl.OnDirtyChanged += graphControl_OnDirtyChanged;
            }

        }

        private void flowchart_menu_layer_go_upper_Click(object sender, EventArgs e)
        {
            if (graphControl.Abstract.CurrentLayer != graphControl.Abstract.DefaultLayer)
            {
                foreach (Shape shape in graphControl.Shapes)
                {
                    if (shape is BlockShape)
                    {
                        GraphLayer layer = (shape as BlockShape).LinkedLayer;
                        if (layer != null && layer == graphControl.Abstract.CurrentLayer)
                        {
                            graphControl.Abstract.ActiveLayer(shape.Layer.Name);
                            return;
                        }
                    }
                }
            }

        }

        private void graphControl_OnDirtyChanged(object sender, bool isDirty)
        {
            if (isDirty)
            {
                if (!Text.Trim().EndsWith("*"))
                {
                    Text = Text.Trim() + "*";
                }
            }
            else
            {
                if (Text.Trim().EndsWith("*"))
                {
                    Text = Text.Trim();
                    Text = Text.Substring(0, Text.Length - 1);
                }
            }

            ToolTipText = Text;

        }

        public bool Save()
        {
            if (FullFileName.Trim().Length == 0)
            {
                return false;
            }

            if (Path.GetExtension(FullFileName).ToUpper() != ".PFC")
            {
                graphControl.SaveImage3(FullFileName);
            }
            else
            {
                graphControl.SaveAs(FullFileName);
            }

            return true;
        }


        public void SaveIfDirty()
        {
            if (graphControl.IsDirty)
            {
                Save();
            }
        }

        //private void Save()

        private void FlowChartForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (graphControl.IsDirty)
            {
                string t_fileName = Text.Trim();
                if (t_fileName.EndsWith("*"))
                {
                    t_fileName = t_fileName.Substring(0, t_fileName.Length - 1);
                }

                string t_msg = string.Format("文件 {0} 未保存，是否保存？", t_fileName);
                DialogResult dr = MessageBox.Show(t_msg, "保存文件", MessageBoxButtons.YesNoCancel);

                switch (dr)
                {
                    case DialogResult.Yes:
                        bool t_isSucceed = Save();
                        e.Cancel = !t_isSucceed;
                        break;

                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }

        private void flowchart_toolbar_zoomout_Click(object sender, EventArgs e)
        {
            graphControl.Zoom += 0.1F;
        }

        private void flowchart_toolbar_zoomin_Click(object sender, EventArgs e)
        {
            if (graphControl.Zoom >= 0.11F)
            {
                graphControl.Zoom -= 0.1F;
                
            }
        }

        private void FlowChartForm_Load(object sender, EventArgs e)
        {
            graphControl.MouseMove += GraphControl_MouseMove;
        }

        private void GraphControl_MouseMove(object sender, MouseEventArgs e)
        {
            string t_msg = string.Format("X : {0} Y : {1}", e.X, e.Y);
            flowchart_status_coordinate.Text = t_msg;
        }
    }
}
