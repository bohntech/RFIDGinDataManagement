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

namespace CottonDBMS.GinApp.Dialogs
{
    public partial class SelectRemovableDriveDialog : Form
    {
        public SelectRemovableDriveDialog()
        {
            InitializeComponent();
        }

        public string RootDirectory { get; set; }

        private void SelectRemovableDriveDialog_Load(object sender, EventArgs e)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            cbTargetDrive.Items.Clear();
            foreach (DriveInfo d in allDrives)
            {
                if (d.DriveType == DriveType.Removable)
                {
                    cbTargetDrive.Items.Add(d.RootDirectory.Name);
                }
            }

            if (cbTargetDrive.Items.Count > 0)
            {
                cbTargetDrive.SelectedIndex = 0;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnSavePackage_Click(object sender, EventArgs e)
        {
            RootDirectory = (string) cbTargetDrive.SelectedItem;
            this.DialogResult = DialogResult.OK;
        }
    }
}
