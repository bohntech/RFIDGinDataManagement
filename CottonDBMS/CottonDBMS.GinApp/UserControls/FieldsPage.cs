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
    public partial class FieldsPage : UserControl
    {
        bool initialized = false;
        CheckBox checkbox = new CheckBox();

        public FieldsPage()
        {
            InitializeComponent();
            // Creating checkbox without panel

            checkbox.Size = new System.Drawing.Size(15, 15);
            checkbox.BackColor = Color.Transparent;

            // Reset properties
            checkbox.Padding = new Padding(0);
            checkbox.Margin = new Padding(0);
            checkbox.Text = "";

            // Add checkbox to datagrid cell
            dataGridFields.Controls.Add(checkbox);
            checkbox.CheckedChanged += Checkbox_CheckedChanged;
            DataGridViewHeaderCell header = dataGridFields.Columns[0].HeaderCell;
            checkbox.Location = new Point(
                header.ContentBounds.Left + (header.ContentBounds.Right - header.ContentBounds.Left + checkbox.Width) / 2 + 53,
                header.ContentBounds.Top + (header.ContentBounds.Bottom - header.ContentBounds.Top + checkbox.Size.Height) / 2
            );
        }

        private void Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridFields.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                chk.Value = checkbox.Checked; //because chk.Value is initialy null  
            }

            if (dataGridFields.Rows.Count > 0)
                dataGridFields.CurrentCell = dataGridFields.Rows[0].Cells[4];
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
            BusyMessage.Show("Loading...", this.FindForm());
            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                IEnumerable<ClientEntity> clients = uow.ClientRepository.GetAll(new string[] { "Farms" }).OrderBy(c => c.Name).ToList();
                AutoCompleteStringCollection clientCollection = new AutoCompleteStringCollection();
                AutoCompleteStringCollection farmsCollection = new AutoCompleteStringCollection();

                foreach (var c in clients)
                {
                    clientCollection.Add(c.Name);
                    foreach (var farm in c.Farms)
                    {
                        farmsCollection.Add(farm.Name);
                    }
                }
                tbClientFilter.AutoCompleteCustomSource = clientCollection;
                tbFarm.AutoCompleteCustomSource = farmsCollection;
            }
            BusyMessage.Close();
        }

        private void refresh()
        {
            refreshFilter();
            BusyMessage.Show("Loading...", this.FindForm());
            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                IEnumerable<FieldEntity> fields = uow.FieldRepository.GetAllMatchingFields(tbClientFilter.Text.Trim(), tbFarm.Text.Trim(), "");
                dataGridFields.DataSource = null;
                dataGridFields.AutoGenerateColumns = false;
                dataGridFields.DataSource = fields;
                dataGridFields.Columns[0].ReadOnly = false;
            }
            BusyMessage.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddFieldDialog dialog = new AddFieldDialog();
            var result = dialog.ShowForm(null);
            if (result != DialogResult.Cancel)
            {
                refresh();
            }
        }

        private void btnEditSelected_Click(object sender, EventArgs e)
        {
            AddFieldDialog dialog = new AddFieldDialog();
            DataGridViewRow row = null;

            if (dataGridFields.SelectedRows.Count > 0)
            {
                row = dataGridFields.SelectedRows[0];
                FieldEntity doc = (FieldEntity) row.DataBoundItem;
                var result = dialog.ShowForm(doc);
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

        private async void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            
            List<FieldEntity>itemsToDelete = new List<FieldEntity>();
            
            foreach (DataGridViewRow row in dataGridFields.Rows)
            {
                FieldEntity doc = (FieldEntity) row.DataBoundItem;
                if (Convert.ToBoolean(row.Cells[0].Value))
                {
                    itemsToDelete.Add(doc);
                }
            }
            
            if (itemsToDelete.Count() > 0)
            {
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    BusyMessage.Show("Checking fields for assigned modules...", this.FindForm());
                    var undeletableFieldIds = uow.FieldRepository.GetUndeletableFieldIds(itemsToDelete);
                    int undeletableCount = 0;
                    BusyMessage.Close();
                    foreach (DataGridViewRow row in dataGridFields.Rows)
                    {
                        FieldEntity doc = (FieldEntity)row.DataBoundItem;
                        if (Convert.ToBoolean(row.Cells[0].Value))
                        {
                            if (undeletableFieldIds.Contains(doc.Id))
                            {
                                undeletableCount++;
                            }                           
                        }
                    }


                    if (undeletableCount > 0)
                    {
                        if (MessageBox.Show(string.Format("{0} field(s) cannot be deleted because they are linked to one or modules, gin loads, or pickup lists. Would you like to continue?", undeletableCount), "Info", MessageBoxButtons.YesNo) == DialogResult.No)
                            return;
                        itemsToDelete.RemoveAll(item => undeletableFieldIds.Contains(item.Id));
                    }

                    if (itemsToDelete.Count() > 0 && MessageBox.Show("Are you sure you want to delete the " + itemsToDelete.Count.ToString() + " selected fields(s)?", "Delete?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        BusyMessage.Show("Deleting...", this.FindForm());

                        uow.FieldRepository.BulkDelete(itemsToDelete);
                        uow.SaveChanges();

                        BusyMessage.Close();
                        refresh();
                    }
                }
            }
            else
            {
                MessageBox.Show("No records were selected to delete.");
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            refresh();
        }
    }
}
