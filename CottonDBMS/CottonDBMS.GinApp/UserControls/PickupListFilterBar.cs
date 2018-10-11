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

namespace CottonDBMS.GinApp.UserControls
{
    public partial class PickupListFilterBar : UserControl
    {

        Form form = null;

        public event EventHandler ApplyClicked = null;

        public PickupListFilterBar()
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
            }
        }       

        private void LoadAutocompletes()
        {
            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                IEnumerable<ClientEntity> clients = uow.ClientRepository.GetAll(new string[] {"Farms.Fields"}).OrderBy(c => c.Name);
                IEnumerable<TruckEntity> trucks =  uow.TruckRepository.GetAll();
                IEnumerable<DriverEntity> drivers = uow.DriverRepository.GetAll();

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
            cboSortDirection1.SelectedIndex = 0;   

            LoadAutocompletes();

            BindingHelper.BindPickupListStatusCombo(cboStatus, " Any ");
            cboStatus.SelectedIndex = 0;
            dpStartDate.Value = DateTime.Now.AddMonths(-1);
            dpEndDate.Value = DateTime.Now;
        }

        public CottonDBMS.Interfaces.PickupListFilter Filter
        {
            get
            {
                var filter = new CottonDBMS.Interfaces.PickupListFilter();
                filter.Client = tbClient.Text.Trim();
                filter.Farm = tbFarm.Text.Trim();
                filter.Field = tbField.Text.Trim();             
                //filter.TruckID = tbTruck.Text.Trim();
                
                if (cboStatus.SelectedIndex == 0) filter.Status = null;
                else
                {
                    BaseEntity item = (BaseEntity) cboStatus.SelectedItem;
                    filter.Status = (PickupListStatus)Convert.ToInt32(item.Id);
                }
                DateTime startDate = new DateTime(dpStartDate.Value.Year, dpStartDate.Value.Month, dpStartDate.Value.Day, 0, 0, 0);
                DateTime endDate = new DateTime(dpEndDate.Value.Year, dpEndDate.Value.Month, dpEndDate.Value.Day, 23, 59, 59);
                filter.StartDate = startDate.ToUniversalTime();
                filter.EndDate = endDate.ToUniversalTime();
                filter.Sort1Ascending = (cboSortDirection1.SelectedIndex == 0);               
                filter.SortCol1 = cboSortBy1.SelectedItem.ToString();               

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
