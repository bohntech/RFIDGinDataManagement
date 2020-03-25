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
using CottonDBMS.DataModels;
using CottonDBMS.GinApp.Dialogs;
using CottonDBMS.GinApp.Classes;

namespace CottonDBMS.GinApp.UserControls
{
    public partial class ModulesPage : UserControl
    {
        bool initialized = false;
        PagedResult<ModuleEntity> lastResult = new PagedResult<ModuleEntity> { LastPageNo = 1, TotalPages = 2 };

        System.Windows.Forms.BindingSource moduleBindingSource = new System.Windows.Forms.BindingSource();
        int pageSize = 100;
        bool loading = false;
        CheckBox checkbox = new CheckBox();

        public ModulesPage()
        {
            InitializeComponent();

            modulesGridView.AutoGenerateColumns = false;

            // Creating checkbox without panel
            
            checkbox.Size = new System.Drawing.Size(15, 15);
            checkbox.BackColor = Color.Transparent;

            // Reset properties
            checkbox.Padding = new Padding(0);
            checkbox.Margin = new Padding(0);
            checkbox.Text = "";

            // Add checkbox to datagrid cell
            modulesGridView.Controls.Add(checkbox);
            checkbox.CheckedChanged += Checkbox_CheckedChanged;
            DataGridViewHeaderCell header = modulesGridView.Columns[0].HeaderCell;
            checkbox.Location = new Point(
               header.ContentBounds.Left + (header.ContentBounds.Right - header.ContentBounds.Left + checkbox.Width) / 2 + 53,
               header.ContentBounds.Top + (header.ContentBounds.Bottom - header.ContentBounds.Top + checkbox.Size.Height) / 2
           );
        }

        private void Checkbox_CheckedChanged(object sender, EventArgs e)
        {            
            foreach (DataGridViewRow row in modulesGridView.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                chk.Value = checkbox.Checked; //because chk.Value is initialy null  
            }

            if (modulesGridView.Rows.Count > 0)
                modulesGridView.CurrentCell = modulesGridView.Rows[0].Cells[5];
        }

        public void LoadData()
        {
            try
            {
                moduleFilterBar.ShowApplyButton = true;
                moduleFilterBar.ShowLocationOptions = false;
                moduleFilterBar.ShowSort1 = true;
                moduleFilterBar.ShowSort2 = false;
                moduleFilterBar.ShowSort3 = false;
                
                if (!initialized)
                {
                    initialized = true;
                    moduleFilterBar.Enabled = false;                    
                    BusyMessage.Show("Loading...", this.FindForm());
                    moduleFilterBar.Initialize();
                    BusyMessage.Close();
                    refresh();
                    moduleFilterBar.Enabled = true;
                }
                else
                {
                    moduleFilterBar.RefreshAutocompletes();
                }
            }
            catch(Exception exc)
            {
                MessageBox.Show("An error occurred trying to load modules.");
                Logging.Logger.Log(exc);
            }
        }

        private void refresh()
        {
            //load filters                     
            var filter = moduleFilterBar.Filter;
            moduleFilterBar.Enabled = false;
            BusyMessage.Show("Loading...", this.FindForm());
            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                lastResult = uow.ModuleRepository.GetModules(filter, pageSize, 1);
                moduleBindingSource.DataSource = lastResult.ResultData;                
                modulesGridView.DataSource = moduleBindingSource;
                modulesGridView.AutoGenerateColumns = false;
                modulesGridView.Columns[0].ReadOnly = false;
                checkbox.Checked = false;
                moduleFilterBar.Enabled = true;
                BusyMessage.Close();
            }

        }

        private void loadMore()
        {
            if (lastResult.TotalPages > lastResult.LastPageNo)
            {
                loading = true;                
                BusyMessage.Show("Loading...", this.FindForm());
                var filter = moduleFilterBar.Filter;
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    lastResult = uow.ModuleRepository.GetModules(filter, pageSize, lastResult.LastPageNo+1);

                    foreach (var module in lastResult.ResultData)
                    {
                        moduleBindingSource.Add(module);
                    }
                    BusyMessage.Close();

                }               
                loading = false;
            }
        }

        private void btnApplyModuleFilters_Click(object sender, EventArgs e)
        {
            try
            {
                refresh();
                moduleFilterBar.RefreshAutocompletes();
            }
            catch( Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }

        private void btnAddModule_Click(object sender, EventArgs e)
        {
            AddEditModule dialog = new AddEditModule();
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
                    List<ModuleEntity> itemsToDelete = new List<ModuleEntity>();
                    BusyMessage.Show("Looking for modules on pickup lists.", this.FindForm());
                    IEnumerable<string> moduleIdsOnPickupList = uow.PickupListRepository.GetModuleIdsOnPickupList();
                    int undeletableCount = 0;
                    foreach (DataGridViewRow row in modulesGridView.Rows)
                    {
                        ModuleEntity module = (ModuleEntity)row.DataBoundItem;
                        if (Convert.ToBoolean(row.Cells[0].Value))
                        {
                            itemsToDelete.Add(module);
                            if (moduleIdsOnPickupList.Contains(module.Id))
                                undeletableCount++;
                        }
                    }
                    BusyMessage.Close();

                    if (itemsToDelete.Count() > 0)
                    {
                        if (undeletableCount > 0)
                        {
                            if (MessageBox.Show(string.Format("{0} module(s) cannot be deleted because they have been added to a pickup list. Would you like to continue and delete modules not linked?", undeletableCount), "Info", MessageBoxButtons.YesNo) == DialogResult.No)
                                return;
                            itemsToDelete.RemoveAll(doc => moduleIdsOnPickupList.Contains(doc.Id));
                        }

                        if (itemsToDelete.Count() > 0 && MessageBox.Show("Modules will be unlinked from associated gin loads and bales.  Are you sure you want to delete the " + itemsToDelete.Count.ToString() + " selected module(s)? ", "Delete?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            BusyMessage.Show("Deleting...", this.FindForm());

                            uow.ModuleRepository.BulkDeleteAndClearLinkedLoadsAndBales(itemsToDelete);
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
                MessageBox.Show("An error occurred deleting module(s).");
            }
        }

        private void btnEditSelectedModule_Click(object sender, EventArgs e)
        {
            AddEditModule dialog = new AddEditModule();
            DataGridViewRow row = null;

            if (modulesGridView.SelectedRows.Count > 0)
            {
                row = modulesGridView.SelectedRows[0];
                ModuleEntity doc = (ModuleEntity)row.DataBoundItem;
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

        private void btnViewHistory_Click(object sender, EventArgs e)
        {
            ModuleHistoryDialog moduleHistoryForm = new ModuleHistoryDialog();
            DataGridViewRow row = null;

            if (modulesGridView.SelectedRows.Count > 0)
            {
                row = modulesGridView.SelectedRows[0];
                ModuleEntity doc = (ModuleEntity)row.DataBoundItem;
                var result = moduleHistoryForm.Show(doc);               
            }
            else
            {
                MessageBox.Show("Please select a row to view history.");
            }            
        }

        private void moduleFilterBar_ApplyClicked(object sender, EventArgs e)
        {
            refresh();
        }

        private void modulesGridView_Scroll(object sender, ScrollEventArgs e)
        {
            if (!loading)
            {
                int totalHeight = 0;
                foreach (DataGridViewRow row in modulesGridView.Rows)
                    totalHeight += row.Height;

                if (totalHeight - modulesGridView.Height < modulesGridView.VerticalScrollingOffset)
                {
                    //Last row visible
                    loadMore();
                }
            }
        }

        private void btnCheckAll_Click(object sender, EventArgs e)
        {
            
        }

        private void btnGenLoad_Click(object sender, EventArgs e)
        {
            try
            {
                var checkedItems = new List<ModuleEntity>();
                string sns = "";
                foreach (DataGridViewRow row in modulesGridView.Rows)
                {
                    ModuleEntity module = (ModuleEntity)row.DataBoundItem;
                    if (Convert.ToBoolean(row.Cells[0].Value))
                    {
                        checkedItems.Add(module);
                        sns += module.Name + ",";
                    }
                }
                sns = sns.TrimEnd(',');
                AssignLoadDialog dialog = new AssignLoadDialog();
                dialog.SelectedModules = sns;
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    dialog.LoadNumber = uow.ModuleRepository
                        .GetNextManualLoadNumber(CottonDBMS.GinApp.Helpers.ConfigHelper.LoadPrefix + "M", 7);

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        var checkedIds = checkedItems.Select(m => m.Id).ToList();
                        var modules = uow.ModuleRepository.FindMatching(m => checkedIds.Contains(m.Id));
                        foreach (var m in modules)
                        {
                            m.LoadNumber = dialog.LoadNumber;
                            m.SyncedToCloud = false;
                            uow.ModuleRepository.Save(m);
                        }
                        uow.SaveChanges();

                        refresh();
                    }
                }                
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                MessageBox.Show(exc.Message);
            }       
        }

        private void btnChangeStatus_Click(object sender, EventArgs e)
        {
            try
            {
                var checkedItems = new List<ModuleEntity>();
                //List<string> sns = new List<string>();
                foreach (DataGridViewRow row in modulesGridView.Rows)
                {
                    ModuleEntity module = (ModuleEntity)row.DataBoundItem;
                    if (Convert.ToBoolean(row.Cells[0].Value))
                    {
                        checkedItems.Add(module);
                        //sns += module.Name + ",";
                    }
                }

                if (checkedItems.Count() == 0)
                {
                    MessageBox.Show("Please check at least one module.");
                    return;
                }

                var serialNumbers = checkedItems.Select(x => x.Name).ToArray();
                ChangeStatusDialog dialog = new ChangeStatusDialog();
                if (dialog.Show(serialNumbers) == DialogResult.OK)
                {
                    refresh();
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                MessageBox.Show(exc.Message);
            }
        }
    }
}
