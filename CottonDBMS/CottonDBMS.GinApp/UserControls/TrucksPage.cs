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
using CottonDBMS.GinApp.Classes;
using CottonDBMS.DataModels;
using System.IO;

namespace CottonDBMS.GinApp.UserControls
{
    public partial class TrucksPage : UserControl
    {
        bool initialized = false;
        CheckBox checkbox = new CheckBox();

        public TrucksPage()
        {
            InitializeComponent();
            checkbox.Size = new System.Drawing.Size(15, 15);
            checkbox.BackColor = Color.Transparent;

            // Reset properties
            checkbox.Padding = new Padding(0);
            checkbox.Margin = new Padding(0);
            checkbox.Text = "";

            // Add checkbox to datagrid cell
            dataGridTrucks.Controls.Add(checkbox);
            checkbox.CheckedChanged += Checkbox_CheckedChanged;
            DataGridViewHeaderCell header = dataGridTrucks.Columns[0].HeaderCell;
            checkbox.Location = new Point(
                header.ContentBounds.Left + (header.ContentBounds.Right - header.ContentBounds.Left + checkbox.Width) / 2 + 53,
                header.ContentBounds.Top + (header.ContentBounds.Bottom - header.ContentBounds.Top + checkbox.Size.Height) / 2
            );
        }

        private void Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridTrucks.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                chk.Value = checkbox.Checked;
            }

            if (dataGridTrucks.Rows.Count > 0)
                dataGridTrucks.CurrentCell = dataGridTrucks.Rows[0].Cells[2];
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
            using (var uow = UnitOfWorkFactory.CreateUnitOfWork()) { 
                          
                IEnumerable<TruckEntity> trucks = uow.TruckRepository.GetAll();
                var sorted = trucks.OrderBy(t => t.Name).ToList();                
                    dataGridTrucks.DataSource = null;
                    dataGridTrucks.AutoGenerateColumns = false;                    
                    dataGridTrucks.DataSource = sorted;                    
                    dataGridTrucks.Columns[0].ReadOnly = false;
                    BusyMessage.Close();               
            }
        }

        private void btnAddTruck_Click(object sender, EventArgs e)
        {
            AddEditTruck dialog = new AddEditTruck();
            if (dialog.ShowAdd() != DialogResult.Cancel)
            {
                refresh();
            }
        }

        private void btnEditSelected_Click(object sender, EventArgs e)
        {
            AddEditTruck dialog = new AddEditTruck();

            DataGridViewRow row = null;

            if (dataGridTrucks.SelectedRows.Count > 0) {
                row = dataGridTrucks.SelectedRows[0];
                TruckEntity doc = (TruckEntity)row.DataBoundItem;
                if (dialog.ShowEdit(doc) != DialogResult.Cancel)
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
            List<TruckEntity> itemsToDelete = new List<TruckEntity>();

            foreach (DataGridViewRow row in dataGridTrucks.Rows)
            {
                TruckEntity doc = (TruckEntity)row.DataBoundItem;
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
                    var undeletableFieldIds = uow.TruckRepository.GetUndeletableIds(itemsToDelete);
                    int undeletableCount = 0;
                    BusyMessage.Close();
                    foreach (DataGridViewRow row in dataGridTrucks.Rows)
                    {
                        var doc = (TruckEntity)row.DataBoundItem;
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
                        if (MessageBox.Show(string.Format("{0} trucks(s) cannot be deleted because they are linked to one or more pickup lists. Would you like to continue and delete trucks that are not linked?", undeletableCount), "Info", MessageBoxButtons.YesNo) == DialogResult.No)
                            return;
                        itemsToDelete.RemoveAll(item => undeletableFieldIds.Contains(item.Id));
                    }

                    if (itemsToDelete.Count() > 0 && MessageBox.Show("Are you sure you want to delete the " + itemsToDelete.Count.ToString() + " selected truck(s)?", "Delete?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        BusyMessage.Show("Deleting...", this.FindForm());
                        uow.TruckRepository.BulkDelete(itemsToDelete);
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

        private void dataGridTrucks_DoubleClick(object sender, EventArgs e)
        {
            btnEditSelected_Click(sender, e);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            refresh();
        }

        private void btnCreateInstallPackage_Click(object sender, EventArgs e)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            if (allDrives.Count(d => d.DriveType == DriveType.Removable) == 0)
            {
                MessageBox.Show("No removable drives detected.  Please insert a removable USB drive, and then try again.");
            }
            else
            {

                using (IUnitOfWork uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    var trucks = uow.TruckRepository.GetAll();
                    var drivers = uow.DriverRepository.GetAll();

                    if (trucks.Count() <= 0)
                    {
                        MessageBox.Show("No trucks have been entered.  Please enter one or more trucks before attempting to install the truck software.");                        
                    }
                    else if (drivers.Count() == 0)
                    {
                        MessageBox.Show("No drivers have been entered.  Please enter one or more drivers before attempting to install the truck software.");                        
                    }
                    else
                    {
                        CreateTruckInstallPackageDialog dialog = new CreateTruckInstallPackageDialog();
                        dialog.ShowDialog();
                    }
                }               
            }
        }
    }
}
