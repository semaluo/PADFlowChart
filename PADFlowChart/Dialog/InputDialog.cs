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
    public partial class InputDialog : Form
    {
        public string Caption
        {
            get { return Text; }
            set { Text = value; }
        }

        public string Message
        {
            set { lb_msg.Text = value; }
        }

        public string Value
        {
            get { return tb_input.Text; }
            set { tb_input.Text = value; }
        }



        public InputDialog()
        {
            InitializeComponent();
            
        }

        private void InputDialog_Load(object sender, EventArgs e)
        {
            tb_input.Focus();
            tb_input.SelectAll();
            tb_input.ScrollToCaret();
        }
    }
}
