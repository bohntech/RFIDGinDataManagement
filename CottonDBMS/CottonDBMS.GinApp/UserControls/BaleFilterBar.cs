using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CottonDBMS.DataModels;

namespace CottonDBMS.GinApp.UserControls
{
    public partial class BaleFilterBar : UserControl
    {
        public event EventHandler ApplyClicked = null;

        public BaleFilterBar()
        {
            InitializeComponent();
        }

        public bool ShowSort1
        {
            get
            {
                return panelSort1.Visible;
            }
            set
            {
                panelSort1.Visible = value;
                panelSort1.AutoSize = false;
            }
        }

        public bool ShowSort2
        {
            get
            {
                return panelSortBy2.Visible;
            }
            set
            {
                panelSortBy2.Visible = value;

                if (value == false)
                {
                    panelSortBy2.Height = 0;
                    panelSortBy2.Width = 0;
                    panelSortBy2.AutoSize = false;
                }
            }
        }

        public bool ShowSort3
        {
            get
            {
                return panelSortBy3.Visible;
            }
            set
            {
                panelSortBy3.Visible = value;
                if (value == false)
                {
                    panelSortBy3.Height = 0;
                    panelSortBy3.Width = 0;
                    panelSortBy3.AutoSize = false;
                }
            }
        }

        public bool ShowApplyButton
        {
            get
            {
                return panelApply.Visible;
            }
            set
            {
                panelApply.Visible = value;

                if (value == false)
                {
                    panelApply.Height = 0;
                    panelApply.Width = 0;
                    panelApply.AutoSize = false;
                }
            }
        }

        
        public void Initialize()
        {
            cboSortBy1.SelectedItem = "PBI Number";
            cboSortBy2.SelectedItem = "Created";
            cboSortBy3.SelectedItem = "Created";

            cboSortDirection1.SelectedIndex = 1;
            cboSortDirection2.SelectedIndex = 1;
            cboSortDirection3.SelectedIndex = 1;
            
            dpStartDate.Value = DateTime.Now.AddMonths(-3);
            dpEndDate.Value = DateTime.Now;
        }

        public BalesFilter Filter
        {
            get
            {
                var filter = new BalesFilter();
               
                filter.PBINumber = tbPBINumber.Text.Trim();
                
                DateTime startDate = new DateTime(dpStartDate.Value.Year, dpStartDate.Value.Month, dpStartDate.Value.Day, 0, 0, 0);
                DateTime endDate = new DateTime(dpEndDate.Value.Year, dpEndDate.Value.Month, dpEndDate.Value.Day, 23, 59, 59);
                filter.StartDate = startDate.ToUniversalTime();
                filter.EndDate = endDate.ToUniversalTime();
                filter.Sort1Ascending = (cboSortDirection1.SelectedIndex == 0);
                filter.Sort2Ascending = (cboSortDirection2.SelectedIndex == 0);
                filter.Sort3Ascending = (cboSortDirection3.SelectedIndex == 0);
                filter.SortCol1 = cboSortBy1.SelectedItem.ToString().ToLower();
                filter.SortCol2 = cboSortBy2.SelectedItem.ToString().ToLower();
                filter.SortCol3 = cboSortBy3.SelectedItem.ToString().ToLower();

                return filter;
            }
        }       

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (ApplyClicked != null)
            {
                ApplyClicked(sender, e);
            }
        }

        private void layoutTimer_Tick(object sender, EventArgs e)
        {
            if (this.Height != outerFlowLayout.Height)
            {
                this.Height = outerFlowLayout.Height;
            }
        }
    }
}
