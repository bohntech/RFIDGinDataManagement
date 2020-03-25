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
    public partial class GinLoadsPage : UserControl
    {
        bool initialized = false;
        PagedResult<GinLoadEntity> lastResult = new PagedResult<GinLoadEntity> { LastPageNo = 1, TotalPages = 2 };

        System.Windows.Forms.BindingSource loadBindingSource = new System.Windows.Forms.BindingSource();
        int pageSize = 100;
        bool loading = false;
        CheckBox checkbox = new CheckBox();

        public GinLoadsPage()
        {
            InitializeComponent();

            dataGridLoads.AutoGenerateColumns = false;

            // Creating checkbox without panel

            checkbox.Size = new System.Drawing.Size(15, 15);
            checkbox.BackColor = Color.Transparent;

            // Reset properties
            checkbox.Padding = new Padding(0);
            checkbox.Margin = new Padding(0);
            checkbox.Text = "";

            // Add checkbox to datagrid cell
            dataGridLoads.Controls.Add(checkbox);
            checkbox.CheckedChanged += Checkbox_CheckedChanged;
            DataGridViewHeaderCell header = dataGridLoads.Columns[0].HeaderCell;
            checkbox.Location = new Point(
               header.ContentBounds.Left + (header.ContentBounds.Right - header.ContentBounds.Left + checkbox.Width) / 2 + 53,
               header.ContentBounds.Top + (header.ContentBounds.Bottom - header.ContentBounds.Top + checkbox.Size.Height) / 2
           );
        }

        private void Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridLoads.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                chk.Value = checkbox.Checked; 
            }

            if (dataGridLoads.Rows.Count > 0)
                dataGridLoads.CurrentCell = dataGridLoads.Rows[0].Cells[7];
        }
        
        public void LoadData()
        {
            try
            {
                loadFilterBar.ShowApplyButton = true;                
                loadFilterBar.ShowSort1 = true;
                loadFilterBar.ShowSort2 = false;
                loadFilterBar.ShowSort3 = false;

                if (!initialized)
                {
                    initialized = true;
                    loadFilterBar.Enabled = false;
                    BusyMessage.Show("Loading...", this.FindForm());
                    loadFilterBar.Initialize();
                    BusyMessage.Close();
                    refresh();
                    loadFilterBar.Enabled = true;
                }
                else
                {
                    loadFilterBar.RefreshAutocompletes();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error occurred trying to load gin loads.");
                Logging.Logger.Log(exc);
            }
        }

        private void refresh()
        {
            //load filters                     
            var filter = loadFilterBar.Filter;
            loadFilterBar.Enabled = false;
            BusyMessage.Show("Loading...", this.FindForm());
            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                lastResult = uow.GinLoadRepository.GetLoads(filter, pageSize, 1);
                loadBindingSource.DataSource = lastResult.ResultData;
                dataGridLoads.DataSource = loadBindingSource;
                dataGridLoads.AutoGenerateColumns = false;
                dataGridLoads.Columns[0].ReadOnly = false;
                checkbox.Checked = false;
                loadFilterBar.Enabled = true;
                BusyMessage.Close();
            }

        }

        private void loadMore()
        {
            if (lastResult.TotalPages > lastResult.LastPageNo)
            {
                loading = true;
                BusyMessage.Show("Loading...", this.FindForm());
                var filter = loadFilterBar.Filter;
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    lastResult = uow.GinLoadRepository.GetLoads(filter, pageSize, lastResult.LastPageNo + 1);
                    foreach (var item in lastResult.ResultData)
                    {
                        loadBindingSource.Add(item);
                    }
                    BusyMessage.Close();

                }
                loading = false;
            }
        }

        private void dataGridLoads_Scroll(object sender, ScrollEventArgs e)
        {
            if (!loading)
            {
                int totalHeight = 0;
                foreach (DataGridViewRow row in dataGridLoads.Rows)
                    totalHeight += row.Height;

                if (totalHeight - dataGridLoads.Height < dataGridLoads.VerticalScrollingOffset)
                {
                    //Last row visible
                    loadMore();
                }
            }
        }
              
       
        private void btnEditSelected_Click(object sender, EventArgs e)
        {
            var dialog = new AddEditGinLoad();
            DataGridViewRow row = null;

            if (dataGridLoads.SelectedRows.Count > 0)
            {
                row = dataGridLoads.SelectedRows[0];
                GinLoadEntity doc = (GinLoadEntity)row.DataBoundItem;
                var result = dialog.ShowEdit(doc);
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var dialog = new AddEditGinLoad();
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
                    List<GinLoadEntity> itemsToDelete = new List<GinLoadEntity>();
                    BusyMessage.Show("Verifying loads can be deleted.", this.FindForm());
                    IEnumerable<string> undeletableIds = uow.GinLoadRepository.GetUndeletableIds();
                    int undeletableCount = undeletableIds.Count();
                    foreach (DataGridViewRow row in this.dataGridLoads.Rows)
                    {
                        GinLoadEntity item = (GinLoadEntity) row.DataBoundItem;
                        if (Convert.ToBoolean(row.Cells[0].Value))
                        {
                            itemsToDelete.Add(item);                            
                        }
                    }
                    BusyMessage.Close();

                    if (itemsToDelete.Count() > 0)
                    {
                        if (undeletableCount > 0)
                        {
                            if (MessageBox.Show(string.Format("{0} load(s) cannot be deleted because they are linked to other records. Would you like to continue and delete loads not linked?", undeletableCount), "Info", MessageBoxButtons.YesNo) == DialogResult.No)
                                return;
                            itemsToDelete.RemoveAll(doc => undeletableIds.Contains(doc.Id));
                        }

                        if (itemsToDelete.Count() > 0 && MessageBox.Show("Are you sure you want to delete the " + itemsToDelete.Count.ToString() + " selected load(s)? All modules and bales from this load will have load information removed and will no longer be linked to a load.", "Delete?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            BusyMessage.Show("Deleting...", this.FindForm());

                            uow.GinLoadRepository.BulkDelete(itemsToDelete);
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
                MessageBox.Show("An error occurred deleting gin load(s).");
            }
        }

        private void loadFilterBar_ApplyClicked(object sender, EventArgs e)
        {
            try
            {
                refresh();
                loadFilterBar.RefreshAutocompletes();
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }
    }
}
