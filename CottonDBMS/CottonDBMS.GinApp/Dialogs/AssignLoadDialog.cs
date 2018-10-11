//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CottonDBMS.GinApp.Dialogs
{
    public partial class AssignLoadDialog : Form
    {
        public AssignLoadDialog()
        {
            InitializeComponent();
        }

        public string SelectedModules
        {
            get
            {
                return lblSelectedSns.Text;
            }
            set
            {
                lblSelectedSns.Text = value;
            }
        }

        public string LoadNumber
        {
            get
            {
                return lblLoadNumber.Text;
            }
            set
            {
                lblLoadNumber.Text = value;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {

        }
    }
}
