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
using CottonDBMS.DataModels;
using CottonDBMS.GinApp.Helpers;
using CottonDBMS.GinApp.Classes;

namespace CottonDBMS.GinApp.UserControls
{
    public partial class PickupListsPage : UserControl
    {
        bool initialized = false;
        PagedResult<PickupListEntity> lastResult = new PagedResult<PickupListEntity> { LastPageNo = 1, TotalPages = 2 };
        System.Windows.Forms.BindingSource listBindingSource = new System.Windows.Forms.BindingSource();
        int pageSize = 100;
        bool loading = false;
        CheckBox checkbox = new CheckBox();

        public PickupListsPage()
        {
            InitializeComponent();

            gridView.AutoGenerateColumns = false;
                       
            checkbox.Size = new System.Drawing.Size(15, 15);
            checkbox.BackColor = Color.Transparent;

            // Reset properties
            checkbox.Padding = new Padding(0);
            checkbox.Margin = new Padding(0);
            checkbox.Text = "";

            // Add checkbox to datagrid cell
            gridView.Controls.Add(checkbox);
            checkbox.CheckedChanged += Checkbox_CheckedChanged;
            DataGridViewHeaderCell header = gridView.Columns[0].HeaderCell;
            checkbox.Location = new Point(
               header.ContentBounds.Left + (header.ContentBounds.Right - header.ContentBounds.Left + checkbox.Width) / 2 + 53,
               header.ContentBounds.Top + (header.ContentBounds.Bottom - header.ContentBounds.Top + checkbox.Size.Height) / 2
            );
        }

        private void Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in gridView.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                chk.Value = checkbox.Checked; //because chk.Value is initialy null  
            }

            if (gridView.Rows.Count > 0)
                gridView.CurrentCell = gridView.Rows[0].Cells[5];
        }

        public void LoadData()
        {
            try
            {
                pickupListFilterBar.ShowApplyButton = true;                
                pickupListFilterBar.ShowSort1 = true;                

                if (!initialized)
                {
                    initialized = true;
                    pickupListFilterBar.Enabled = false;
                    
                    BusyMessage.Show("Loading...", this.FindForm());
                    pickupListFilterBar.Initialize();
                    BusyMessage.Close();
                    refresh();
                    pickupListFilterBar.Enabled = true;
                }
                else
                {
                    pickupListFilterBar.RefreshAutocompletes();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error occurred trying to load lists.");
                Logging.Logger.Log(exc);
            }
        }

        private void refresh()
        {
            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                //load filters             
                var filter = pickupListFilterBar.Filter;
                pickupListFilterBar.Enabled = false;
                BusyMessage.Show("Loading...", this.FindForm());
                PagedResult<PickupListEntity> result = uow.PickupListRepository.GetLists(filter, pageSize, 1, ConfigHelper.ModulesPerLoad);
                listBindingSource.DataSource = result.ResultData;
                lastResult = result;
                gridView.DataSource = listBindingSource;
                gridView.AutoGenerateColumns = false;
                gridView.Columns[0].ReadOnly = false;
                checkbox.Checked = false;
                pickupListFilterBar.Enabled = true;
                BusyMessage.Close();
            }
        }

        private void loadMore()
        {
            if (gridView.RowCount < lastResult.Total)
            {
                loading = true;                
                BusyMessage.Show("Loading...", this.FindForm());
                var filter = pickupListFilterBar.Filter;
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    PagedResult<PickupListEntity> result = uow.PickupListRepository.GetLists(filter, pageSize, lastResult.LastPageNo+1, ConfigHelper.ModulesPerLoad);
                    foreach (var list in lastResult.ResultData)
                    {
                        listBindingSource.Add(list);
                    }
                    BusyMessage.Close();
                }
                loading = false;
            }
        }
        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddEditPickupList dialog = new AddEditPickupList();
            var result = dialog.ShowAdd();
            if (result != DialogResult.Cancel)
            {
                refresh();
            }
        }

        private void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            try
            {
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    var idsDownloaded = uow.PickupListRepository.GetDownloadedPickupListIds();

                    List<PickupListEntity> itemsToDelete = new List<PickupListEntity>();
                    int undeletableCount = 0;
                    foreach (DataGridViewRow row in gridView.Rows)
                    {
                        var list = (PickupListEntity)row.DataBoundItem;
                        if (Convert.ToBoolean(row.Cells[0].Value))
                        {
                            itemsToDelete.Add(list);
                            if (idsDownloaded.Contains(list.Id)) undeletableCount++;
                        }
                    }

                    if (itemsToDelete.Count() > 0)
                    {
                        if (undeletableCount > 0)
                        {
                            if (MessageBox.Show(string.Format("{0} list(s) cannot be deleted because they have been downloaded to one or more trucks. Would you like to continue and delete the checked lists that have not been downloaded?", undeletableCount), "Info", MessageBoxButtons.YesNo) == DialogResult.No)
                                return;
                            itemsToDelete.RemoveAll(p => idsDownloaded.Contains(p.Id));
                        }

                        if (itemsToDelete.Count() > 0 && MessageBox.Show("Are you sure you want to delete " + itemsToDelete.Count.ToString() + " lists(s)?", "Delete?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            BusyMessage.Show("Deleting...", this.FindForm());                            
                            uow.PickupListRepository.BulkDelete(itemsToDelete);
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
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                MessageBox.Show("An error occurred deleting list(s).");
            }
        }

        private async void btnEditSelect_Click(object sender, EventArgs e)
        {
            var dialog = new AddEditPickupList();
            DataGridViewRow row = null;

            if (gridView.SelectedRows.Count > 0)
            {
                row = gridView.SelectedRows[0];
                var doc = (PickupListEntity)row.DataBoundItem;
                var result = await dialog.ShowEdit(doc);
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

        private async void filterBar_ApplyClicked(object sender, EventArgs e)
        {
            refresh();
        }

        private void gridView_Scroll(object sender, ScrollEventArgs e)
        {
            if (!loading)
            {
                int totalHeight = 0;
                foreach (DataGridViewRow row in gridView.Rows)
                    totalHeight += row.Height;

                if (totalHeight - gridView.Height < gridView.VerticalScrollingOffset)
                {
                    //Last row visible
                    loadMore();
                }
            }
        }

        private async void pickupListFilterBar_ApplyClicked(object sender, EventArgs e)
        {
            try
            {
                refresh();
                pickupListFilterBar.RefreshAutocompletes();
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }       
    }
}
