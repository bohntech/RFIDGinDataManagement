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
//using CottonDBMS.Cloud;
using CottonDBMS.GinApp.Classes;

namespace CottonDBMS.GinApp.UserControls
{
    public partial class ClientsPage : UserControl
    {
        bool initialized = false;
        CheckBox checkbox = new CheckBox();

        public ClientsPage()
        {
            InitializeComponent();
            checkbox.Size = new System.Drawing.Size(15, 15);
            checkbox.BackColor = Color.Transparent;

            // Reset properties
            checkbox.Padding = new Padding(0);
            checkbox.Margin = new Padding(0);
            checkbox.Text = "";

            // Add checkbox to datagrid cell
            dataGridClients.Controls.Add(checkbox);
            checkbox.CheckedChanged += Checkbox_CheckedChanged;
            DataGridViewHeaderCell header = dataGridClients.Columns[0].HeaderCell;
            checkbox.Location = new Point(
                header.ContentBounds.Left + (header.ContentBounds.Right - header.ContentBounds.Left + checkbox.Width) / 2 + 53,
                header.ContentBounds.Top + (header.ContentBounds.Bottom - header.ContentBounds.Top + checkbox.Size.Height) / 2
            );
        }

        private void Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridClients.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                chk.Value = checkbox.Checked;
            }

            if (dataGridClients.Rows.Count > 0)
                dataGridClients.CurrentCell = dataGridClients.Rows[0].Cells[2];
        }

        public void LoadData()
        {
            if (!initialized)
            {
                initialized = true;
                refresh();
            }
        }

        private void refresh()
        {
            BusyMessage.Show("Loading...", this.FindForm());
            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                IEnumerable<ClientEntity> clients = uow.ClientRepository.GetAll();
                var sorted = clients.OrderBy(t => t.Name).ToList();
                dataGridClients.DataSource = null;
                dataGridClients.AutoGenerateColumns = false;
                dataGridClients.DataSource = sorted;
                dataGridClients.Columns[0].ReadOnly = false;
                BusyMessage.Close();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddEditClient dialog = new AddEditClient();
            if (dialog.ShowForm(null) != DialogResult.Cancel)
            {
                refresh();
            }
        }

        private void btnEditSelected_Click(object sender, EventArgs e)
        {
            AddEditClient dialog = new AddEditClient();

            DataGridViewRow row = null;

            if (dataGridClients.SelectedRows.Count > 0)
            {
                row = dataGridClients.SelectedRows[0];
                ClientEntity doc = (ClientEntity)row.DataBoundItem;
                if (dialog.ShowForm(doc) != DialogResult.Cancel)
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
                List<ClientEntity> itemsToDelete = new List<ClientEntity>();
                var undeletableIds = uow.ClientRepository.GetClientIdsLinkedToFarms();
                int undeletableCount = 0;
                foreach (DataGridViewRow row in dataGridClients.Rows)
                {
                    ClientEntity doc = (ClientEntity)row.DataBoundItem;
                    if (Convert.ToBoolean(row.Cells[0].Value))
                    {
                        itemsToDelete.Add(doc);

                        if (undeletableIds.Contains(doc.Id))
                        {
                            undeletableCount++;
                        }
                    }
                }

                if (itemsToDelete.Count() > 0)
                {
                    if (undeletableCount > 0)
                    {
                        if (MessageBox.Show(string.Format("{0} client(s) cannot be deleted because they are linked to one or more farms. Would you like to continue?", undeletableCount), "Info", MessageBoxButtons.YesNo) == DialogResult.No)
                            return;
                        itemsToDelete.RemoveAll(doc => undeletableIds.Contains(doc.Id));
                    }

                    if (itemsToDelete.Count() > 0 && MessageBox.Show("Are you sure you want to delete the " + itemsToDelete.Count.ToString() + " selected clients(s)?", "Delete?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        BusyDialog dialog = new BusyDialog();
                        dialog.ShowMessage("Deleting...", this.FindForm());
                        //await CottonDBMS.Cloud.Caching.ClientCache.InvalidateAsync();
                        uow.ClientRepository.BulkDelete(itemsToDelete);
                        uow.SaveChanges();
                        dialog.Close();
                        refresh();
                    }
                }
                else
                {
                    MessageBox.Show("No records were selected to delete.");
                }               
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            refresh();
        }
    }
}
