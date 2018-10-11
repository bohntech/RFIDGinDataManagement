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
    public partial class BusyDialog : Form
    {
        private Form _parent = null;

        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x20000;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        public BusyDialog()
        {
            InitializeComponent();
            this.TopMost = false;
        }

        public void ShowMessage(string msg, Form parent)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    ShowMessage(msg, parent);
                });
            }
            else
            {
                _parent = parent;
                //parent.Opacity = 0.85;            
                this.Left = parent.Location.X + ((parent.Width - this.Width) / 2);
                this.Top = parent.Location.Y + ((parent.Height - this.Height) / 2);
                lblMessage.Text = msg;
                this.Show();
                this.UpdateZOrder();
                this.Update();
            }
        }

        public void UpdateMessage(string msg)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    UpdateMessage(msg);
                });
            }
            else
            {
                lblMessage.Text = msg;
                this.UpdateZOrder();
                this.Update();
            }           
        }

        public void CloseOnUIThread()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    this.Close();
                });
            }
            else
            {
                this.Close();
            }
        }
            

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void BusyDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            _parent.Opacity = 1.0;
        }
    }
}
