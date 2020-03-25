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
using CottonDBMS.GinApp.Classes;
using System.IO;
using CottonDBMS.GinApp.Helpers;
using CottonDBMS.GinApp.Dialogs;

namespace CottonDBMS.GinApp.UserControls
{
    public partial class BalesPage : UserControl
    {
        bool initialized = false;
        PagedResult<BaleEntity> lastResult = new PagedResult<BaleEntity> { LastPageNo = 1, TotalPages = 2 };

        System.Windows.Forms.BindingSource bindingSource = new System.Windows.Forms.BindingSource();
        int pageSize = 100;
        bool loading = false;
        CheckBox checkbox = new CheckBox();

        public BalesPage()
        {
            InitializeComponent();

            dataGridBales.AutoGenerateColumns = false;

            // Creating checkbox without panel
            checkbox.Size = new System.Drawing.Size(15, 15);
            checkbox.BackColor = Color.Transparent;

            // Reset properties
            checkbox.Padding = new Padding(0);
            checkbox.Margin = new Padding(0);
            checkbox.Text = "";

            // Add checkbox to datagrid cell
            dataGridBales.Controls.Add(checkbox);
            checkbox.CheckedChanged += Checkbox_CheckedChanged;
            DataGridViewHeaderCell header = dataGridBales.Columns[0].HeaderCell;
            checkbox.Location = new Point(
               header.ContentBounds.Left + (header.ContentBounds.Right - header.ContentBounds.Left + checkbox.Width) / 2 + 53,
               header.ContentBounds.Top + (header.ContentBounds.Bottom - header.ContentBounds.Top + checkbox.Size.Height) / 2 + 15
           );
        }

        private void Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridBales.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                chk.Value = checkbox.Checked;
            }

            if (dataGridBales.Rows.Count > 0)
                dataGridBales.CurrentCell = dataGridBales.Rows[0].Cells[7];
        }

        public void LoadData()
        {
            try
            {
                baleFilterBar.ShowApplyButton = true;
                baleFilterBar.ShowSort1 = true;
                baleFilterBar.ShowSort2 = false;
                baleFilterBar.ShowSort3 = false;

                if (!initialized)
                {
                    initialized = true;
                    baleFilterBar.Enabled = false;
                    BusyMessage.Show("Loading...", this.FindForm());
                    baleFilterBar.Initialize();
                    BusyMessage.Close();
                    refresh();
                    baleFilterBar.Enabled = true;
                }                
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error occurred trying to load bales.");
                Logging.Logger.Log(exc);
            }
        }

        private void refresh()
        {

            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    refresh();
                });
            }
            else {                    //load filters      

                var filter = baleFilterBar.Filter;
                baleFilterBar.Enabled = false;
                BusyMessage.Show("Loading...", this.FindForm());
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    lastResult = uow.BalesRepository.GetBales(filter, pageSize, 1);
                    bindingSource.DataSource = lastResult.ResultData;
                    dataGridBales.DataSource = bindingSource;
                    dataGridBales.AutoGenerateColumns = false;
                    dataGridBales.Columns[0].ReadOnly = false;
                    checkbox.Checked = false;
                    baleFilterBar.Enabled = true;
                    BusyMessage.Close();
                }
            }
        }

        private void loadMore()
        {
            if (lastResult.TotalPages > lastResult.LastPageNo)
            {
                loading = true;
                BusyMessage.Show("Loading...", this.FindForm());
                var filter = baleFilterBar.Filter;
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    lastResult = uow.BalesRepository.GetBales(filter, pageSize, lastResult.LastPageNo + 1);
                    foreach (var item in lastResult.ResultData)
                    {
                        bindingSource.Add(item);
                    }
                    BusyMessage.Close();

                }
                loading = false;
            }
        }

        private void dataGridBales_Scroll(object sender, ScrollEventArgs e)
        {
            if (!loading)
            {
                int totalHeight = 0;
                foreach (DataGridViewRow row in dataGridBales.Rows)
                    totalHeight += row.Height;

                if (totalHeight - dataGridBales.Height < dataGridBales.VerticalScrollingOffset)
                {
                    //Last row visible
                    loadMore();
                }
            }
        }                     
               
        private void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            try
            {
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    List<BaleEntity> itemsToDelete = new List<BaleEntity>();
                    //BusyMessage.Show("Verifying loads can be deleted.", this.FindForm());                   
                    foreach (DataGridViewRow row in this.dataGridBales.Rows)
                    {
                        var item = (BaleEntity)row.DataBoundItem;
                        if (Convert.ToBoolean(row.Cells[0].Value))
                        {
                            itemsToDelete.Add(item);
                        }
                    }
                 
                    if (itemsToDelete.Count() > 0)
                    {
                        if (itemsToDelete.Count() > 0 && MessageBox.Show("Are you sure you want to delete the " + itemsToDelete.Count.ToString() + " selected bale(s)?", "Delete?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            BusyMessage.Show("Deleting...", this.FindForm());
                            uow.BalesRepository.BulkDelete(itemsToDelete);
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
                MessageBox.Show("An error occurred deleting bales.");
            }
        }

        private void baleFilterBar_ApplyClicked(object sender, EventArgs e)
        {
            try
            {
                refresh();                
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                fileDialog.Title = "Import File";
                fileDialog.Multiselect = false;
                fileDialog.CheckFileExists = true;
                fileDialog.CheckPathExists = true;
                
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    
                    FileInfo info = new FileInfo(fileDialog.FileName);

                    Task.Run(() =>
                    {
                        BusyMessage.Show("Importing file...", this.FindForm());
                        ClassingFileImportTask import = new ClassingFileImportTask();
                        import.processFile(info);
                        BusyMessage.Close();

                       
                            refresh();
                        
                    });
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                MessageBox.Show("An error occurred importing file. " + exc.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog();
            
        }

        private void saveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    //var dt = uow.GetDataTable("SELECT c.Name as [Client], farm.Name as [Farm], f.Name as [Field] FROM dbo.BaleEntities b LEFT JOIN dbo.ModuleEntities m ON m.Id = b.ModuleId LEFT JOIN dbo.FieldEntities f on f.Id = m.FieldId LEFT JOIN dbo.FarmEntities farm on farm.Id = f.FarmId LEFT JOIN dbo.ClientEntities c on farm.ClientId = c.Id  ORDER BY [PbiNumber]");

                    var bales = uow.BalesRepository.GetAll(new string[] { "Module.Field.Farm.Client", "GinLoad.Modules", "Module.ModuleHistory", "Module.GinLoad" }).OrderBy(b => b.Created);

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Client,Farm,Field,FeederScanTime,GinLoadNo,RoundModuleId,AppScanLat,AppScanLon,HIDLat,HIDLon,HIDDropLat,HIDDropLon,LoadNetSeedCottonWeight,LoadLintEstimate," +
                                  "TotalRMInLoad,ModuleDiameter(in),EstWeightFromDiameter,DiameterLoadWeightMultiplier,HIDModuleWeight(Lbs)," +
                                  "LoadAvgModuleWeight,ModuleWeightForCalc,RoundModuleLintEst,PBI,PBIScanTime,TareWeight,NetBaleWeight,ScanNumber,LintTurnOut,PercentThreshold," +
                                  "AccumWeight,OverrageAdjustment,OutOfSequence," +
                                  "Classing_NetWeight,Classing_Pk,Classing_Gr,Classing_Lf,Classing_St,Classing_Mic,Classing_Ex,Classing_Rm," +
                                  "Classing_Str,Classing_Cgr,Classing_Rd," +
                                  "Classing_Plusb,Classing_Tr,Classing_Unif,Classing_Len,Classing_Value,Classing_TareWeight,Classing_EstimatedSeedWeight");

                    foreach (var b in bales)
                    {
                        sb.Append(Helpers.FileHelper.EscapeForCSV(b.ClientName) + ",");
                        sb.Append(Helpers.FileHelper.EscapeForCSV(b.FarmName) + ",");
                        sb.Append(Helpers.FileHelper.EscapeForCSV(b.FieldName) + ",");

                        if (b.Module != null)
                        {
                            sb.Append(FileHelper.EscapeForCSV(b.Module.LastFeederScanTimeToLocalString) + ",");
                        }
                        else
                            sb.Append(",");

                        sb.Append(FileHelper.EscapeForCSV(b.GinTicketLoadNumber) + ",");
                        
                        sb.Append(FileHelper.EscapeForCSV(b.ModuleSerialNumber) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.Module?.AppScanLat?.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.Module?.AppScanLong?.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.Module?.HIDLat.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.Module?.HIDLong.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.Module?.HIDDropLat.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.Module?.HIDDropLong.ToString()) + ",");

                        sb.Append(FileHelper.EscapeForCSV(b.GinLoad?.NetWeight.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.Module?.LoadLintWeight?.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.GinLoad?.Modules?.Count().ToString()) + ",");

                        sb.Append(FileHelper.EscapeForCSV(b.Module?.HIDDiameterInches?.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.Module?.DiameterApproximatedWeight?.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.Module?.LoadWeightMultiplier?.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.Module?.HIDModuleWeightLBS?.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.Module?.LoadAvgModuleWeight?.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.Module?.EstimatedNetSeedCottonWeight?.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.Module?.LintWeight?.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.PbiNumber) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.LocalCreatedTimestamp) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.TareWeight.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.NetWeight.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.ScanNumber.ToString()) + ",");

                        sb.Append(FileHelper.EscapeForCSV(b.LintTurnout?.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.OverrageThreshold?.ToString()) + ",");

                        sb.Append(FileHelper.EscapeForCSV(b.AccumWeight.ToString()) + ",");

                        sb.Append(FileHelper.EscapeForCSV(b.OverrageAdjustment?.ToString()) + ",");

                        sb.Append(FileHelper.EscapeForCSV(b.OutOfSequence ? "YES" : "NO") + ",");                        

                        sb.Append(FileHelper.EscapeForCSV(b.Classing_NetWeight?.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.Classing_Pk?.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.Classing_Gr?.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.Classing_Lf?.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.Classing_St?.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.Classing_Mic?.ToString()) + ",");

                        sb.Append(FileHelper.EscapeForCSV(b.Classing_Ex?.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.Classing_Rm?.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.Classing_Str?.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.Classing_CGr?.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.Classing_Rd?.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.Classing_Plusb?.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.Classing_Tr?.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.Classing_Unif?.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.Classing_Len?.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.Classing_Value?.ToString()) + ",");
                        sb.Append(FileHelper.EscapeForCSV(b.Classing_TareWeight?.ToString()) + ",");
                        sb.AppendLine(FileHelper.EscapeForCSV(b.Classing_EstimatedSeedWeight?.ToString()) + ",");
                    }

                    File.WriteAllText(saveFileDialog.FileName, sb.ToString());
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                MessageBox.Show("An error occurred exporting file. " + exc.Message);
            }
        }

        private void btnAssignChecked_Click(object sender, EventArgs e)
        {

            List<BaleEntity> itemsSelected = new List<BaleEntity>();
            //BusyMessage.Show("Verifying loads can be deleted.", this.FindForm());                   
            foreach (DataGridViewRow row in this.dataGridBales.Rows)
            {
                var item = (BaleEntity)row.DataBoundItem;
                if (Convert.ToBoolean(row.Cells[0].Value))
                {
                    itemsSelected.Add(item);
                }
            }

            AssignPBIsToNewModule dialog = new AssignPBIsToNewModule(itemsSelected);
            dialog.ShowDialog();
            refresh();
        }
    }
}
