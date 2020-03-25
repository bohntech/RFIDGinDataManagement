//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CottonDBMS.GinApp.Helpers;
using CottonDBMS.DataModels;
using CottonDBMS.Interfaces;

namespace CottonDBMS.GinApp.UserControls
{
    public partial class GinLoadFilterBar : UserControl
    {
        public event EventHandler ApplyClicked = null;

        public GinLoadFilterBar()
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

        private void LoadAutocompletes()
        {
            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                IEnumerable<ClientEntity> clients = uow.ClientRepository.GetAll(new string[] { "Farms.Fields" }).OrderBy(c => c.Name);
                IEnumerable<TruckEntity> trucks = uow.TruckRepository.GetAll().OrderBy(c => c.Name);
                IEnumerable<DriverEntity> drivers = uow.DriverRepository.GetAll().OrderBy(d => d.Name);

                AutoCompleteStringCollection clientCollection = new AutoCompleteStringCollection();
                AutoCompleteStringCollection farmsCollection = new AutoCompleteStringCollection();
                AutoCompleteStringCollection fieldsCollection = new AutoCompleteStringCollection();

                foreach (var c in clients)
                {
                    clientCollection.Add(c.Name);
                    foreach (var farm in c.Farms)
                    {
                        farmsCollection.Add(farm.Name);
                        foreach (var field in farm.Fields)
                        {
                            fieldsCollection.Add(field.Name);
                        }
                    }
                }

                tbClient.AutoCompleteCustomSource = clientCollection;
                tbFarm.AutoCompleteCustomSource = farmsCollection;
                tbField.AutoCompleteCustomSource = fieldsCollection;             
            }
        }

        public void RefreshAutocompletes()
        {
            LoadAutocompletes();
        }

        public void Initialize()
        {
            cboSortBy1.SelectedItem = "Client";
            cboSortBy2.SelectedItem = "Farm";
            cboSortBy3.SelectedItem = "Field";

            cboSortDirection1.SelectedIndex = 0;
            cboSortDirection2.SelectedIndex = 0;
            cboSortDirection3.SelectedIndex = 0;            
            //cboRecordsToShow.SelectedItem = "Only most recent locations";
            LoadAutocompletes();           
            dpStartDate.Value = DateTime.Now.AddMonths(-3);
            dpEndDate.Value = DateTime.Now;            
        }

        public CottonDBMS.Interfaces.GinLoadsFilter Filter
        {
            get
            {             
                var filter = new CottonDBMS.Interfaces.GinLoadsFilter();
                filter.Client = tbClient.Text.Trim();
                filter.Farm = tbFarm.Text.Trim();
                filter.Field = tbField.Text.Trim();
                filter.BridgeId = tbBridgeId.Text.Trim();
                filter.GinTagLoadNumber = tbGinTagLoad.Text.Trim();
                filter.BridgeLoadNumber = tbScaleBridgeLoad.Text.Trim();
                                
                DateTime startDate = new DateTime(dpStartDate.Value.Year, dpStartDate.Value.Month, dpStartDate.Value.Day, 0, 0, 0);
                DateTime endDate = new DateTime(dpEndDate.Value.Year, dpEndDate.Value.Month, dpEndDate.Value.Day, 23, 59, 59);
                filter.StartDate = startDate.ToUniversalTime();
                filter.EndDate = endDate.ToUniversalTime();
                filter.Sort1Ascending = (cboSortDirection1.SelectedIndex == 0);
                filter.Sort2Ascending = (cboSortDirection2.SelectedIndex == 0);
                filter.Sort3Ascending = (cboSortDirection3.SelectedIndex == 0);
                filter.SortCol1 = cboSortBy1.SelectedItem.ToString();
                filter.SortCol2 = cboSortBy2.SelectedItem.ToString();
                filter.SortCol3 = cboSortBy3.SelectedItem.ToString();

                return filter;
            }
        }

        private void Form_SizeChanged(object sender, EventArgs e)
        {
            //this.Width = form.Width;
            //this.outerFlowLayout.Width = form.Width;                        
        }

        private void flowLayoutPanel_Resize(object sender, EventArgs e)
        {
            //this.Height = flowLayoutPanel.Height;
           // this.Width = flowLayoutPanel.Width;
        }

        private void ModuleFilterBar_Resize(object sender, EventArgs e)
        {
            //this.Height = outerFlowLayout.Height;
             //this.Width = outerFlowLayout.Width;
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
