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
    public partial class PropertyForm : DockContent
    {
        public PropertyForm()
        {
            InitializeComponent();
        }

        public void ShowProperty(object obj)
        {
            propertyGrid.SelectedObject = obj;
        }

        public void ShowProperty(object sender, object[] objs)
        {
            propertyGrid.SelectedObjects = objs;
        }
    }
}
