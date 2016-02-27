using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace PADFlowChart
{


    public partial class MainForm : Form
    {
        //private FlowChartForm[] chartForms 
        private int m_fileCount = 1;

        //private FlowChartForm chartForm = new FlowChartForm();
        private PropertyForm m_propertyForm = new PropertyForm();
        
          
        public MainForm()
        {
            InitializeComponent();
            m_propertyForm.Show(dockPanel, DockState.DockRight);
            NewFlowChartDiagram();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //toolStrip1.Visible = false;
        }

        private void MainForm_MdiChildActivate(object sender, EventArgs e)
        {
//            return;
            if (menu_edit.DropDownItems.Count == 0) menu_edit.Visible = false;
            ToolStripManager.RevertMerge(toolStrip);

            if (this.ActiveMdiChild == null) return;
            if ((ActiveMdiChild as IMergeToolStrip).MergeToolStrip == null) return;

            ToolStripManager.Merge((ActiveMdiChild as IMergeToolStrip).MergeToolStrip, toolStrip);

            if (toolStrip.Items.Count > 0)
                toolStrip.Visible = true;
            else
                toolStrip.Visible = false;


        }

        private void menu_file_open_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    FlowChartForm form = new FlowChartForm();
                    form.OpenFile(ofd.FileName);
                    form.Show(dockPanel);
                    form.OnShowProperties += m_propertyForm.ShowProperty;

                }
            }
        }

        public string GenerateFileName()
        {
            string t_fileName = "FlowChart" + m_fileCount.ToString() + ".pfc";
            m_fileCount++;
            return t_fileName;
        }

        public void NewFlowChartDiagram()
        {
            FlowChartForm form = new FlowChartForm();
            form.Text = GenerateFileName();
            form.Show(dockPanel);
            form.OnShowProperties += m_propertyForm.ShowProperty;
        }

        private void menu_file_new_Click(object sender, EventArgs e)
        {
            NewFlowChartDiagram();
        }
    }

    public interface IMergeToolStrip
    {
        ToolStrip MergeToolStrip
        {
            get;
        }
    }

    
}
