using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PADFlowChart
{


    public partial class MainForm : Form
    {
        //private FlowChartForm[] chartForms 
        private FlowChartForm chartForm = new FlowChartForm();    
        public MainForm()
        {
            InitializeComponent();
            chartForm.Show(dockPanel);
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
