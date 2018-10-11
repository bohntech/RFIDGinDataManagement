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
using CottonDBMS.GinApp.Dialogs;
using CottonDBMS.Interfaces;
using CottonDBMS.DataModels;
using CottonDBMS.GinApp.Classes;

namespace CottonDBMS.GinApp.UserControls
{
    public partial class FarmsPage : UserControl
    {
        bool initialized = false;
        CheckBox checkbox = new CheckBox();

        public FarmsPage()
        {
            InitializeComponent();
            checkbox.Size = new System.Drawing.Size(15, 15);
            checkbox.BackColor = Color.Transparent;

            // Reset properties
            checkbox.Padding = new Padding(0);
            checkbox.Margin = new Padding(0);
            checkbox.Text = "";

            // Add checkbox to datagrid cell
            dataGridFarms.Controls.Add(checkbox);
            checkbox.CheckedChanged += Checkbox_CheckedChanged;
            DataGridViewHeaderCell header = dataGridFarms.Columns[0].HeaderCell;
            checkbox.Location = new Point(
                header.ContentBounds.Left + (header.ContentBounds.Right - header.ContentBounds.Left + checkbox.Width) / 2 + 53,
                header.ContentBounds.Top + (header.ContentBounds.Bottom - header.ContentBounds.Top + checkbox.Size.Height) / 2
            );
        }

        private void Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridFarms.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                chk.Value = checkbox.Checked; 
            }

            if (dataGridFarms.Rows.Count > 0)
                dataGridFarms.CurrentCell = dataGridFarms.Rows[0].Cells[3];
        }

        public void LoadData()
        {
            if (!initialized)
            {
                initialized = true;
                refresh();
            }
            else
            {
                refreshFilter();
            }
        }

        private void refreshFilter()
        {
            
            BusyMessage.Show("Loading ...", this.FindForm());
            //IEnumerable<ClientDocument> clients = await ClientCache.GetAllClientsAsync();

            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                AutoCompleteStringCollection clientCollection = new AutoCompleteStringCollection();
                foreach (var c in uow.ClientRepository.GetAll().OrderBy(c => c.Name))
                {
                    clientCollection.Add(c.Name);
                }
                tbClientFilter.AutoCompleteCustomSource = clientCollection;
            }
            BusyMessage.Close();
        }

        private void refresh()
        {
            refreshFilter();

            BusyMessage.Show("Loading...", this.FindForm());

            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {                
                IEnumerable<FarmEntity> farms = uow.FarmRepository.FindMatching(c => tbClientFilter.Text.Trim() == "" || c.Client.Name == tbClientFilter.Text.Trim(), new string[] { "Client" });
                var sorted = farms.OrderBy(t => t.Client.Name).ThenBy(t => t.Name).ToList();
                dataGridFarms.DataSource = null;
                dataGridFarms.AutoGenerateColumns = false;
                dataGridFarms.DataSource = sorted;
                dataGridFarms.Columns[0].ReadOnly = false;
            }
            BusyMessage.Close();

        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            AddFarm dialog = new AddFarm();
            var result = await dialog.ShowForm(null);
            if (result != DialogResult.Cancel)
            {
                refresh();
            }          
        }

        private async void btnEditSelected_Click(object sender, EventArgs e)
        {
            AddFarm dialog = new AddFarm();
            DataGridViewRow row = null;

            if (dataGridFarms.SelectedRows.Count > 0)
            {
                row = dataGridFarms.SelectedRows[0];
                FarmEntity doc = (FarmEntity)row.DataBoundItem;
                var result = await dialog.ShowForm(doc);
                if (result != DialogResult.Cancel)
                {
                    refresh();
                }
            }
            else
            {
                MessageBox.Show("Please select a row to edit.");
            }
        }

        private void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                List<FarmEntity> farmsToDelete = new List<FarmEntity>();                
                var idsLinkedToFields = uow.FarmRepository.GetFarmIdsLinkedToFields();
                int undeletableCount = 0;

                foreach (DataGridViewRow row in dataGridFarms.Rows)
                {
                    FarmEntity entity = (FarmEntity)row.DataBoundItem;
                    if (Convert.ToBoolean(row.Cells[0].Value))
                    {
                        farmsToDelete.Add(entity);
                        if (idsLinkedToFields.Contains(entity.Id))
                        {
                            undeletableCount++;
                        }                       
                    }
                }

                if (farmsToDelete.Count() > 0)
                {
                    if (undeletableCount > 0)
                    {
                        if (MessageBox.Show(string.Format("{0} farms(s) cannot be deleted because they are linked to one or more fields. Would you like to continue?", undeletableCount), "Info", MessageBoxButtons.YesNo) == DialogResult.No)
                            return;

                        farmsToDelete.RemoveAll(f => idsLinkedToFields.Contains(f.Id));
                    }

                    if (farmsToDelete.Count() > 0 && MessageBox.Show("Are you sure you want to delete the " + farmsToDelete.Count.ToString() + " selected farms(s)?", "Delete?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        BusyMessage.Show("Deleting...", this.FindForm());
                        uow.FarmRepository.BulkDelete(farmsToDelete);
                        uow.SaveChanges();
                        BusyMessage.Close();
                        refresh();
                    }
                }
                else
                {
                    MessageBox.Show("No records were selected to delete.");
                }
            }
        }

        private async void btnApply_Click(object sender, EventArgs e)
        {
            refresh();
        }      
    }
}
