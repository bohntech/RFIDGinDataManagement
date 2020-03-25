using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CottonDBMS.GinApp.Classes;
using CottonDBMS.DataModels;
using CottonDBMS.Interfaces;

namespace CottonDBMS.GinApp.UserControls
{
    public partial class PBIMappingPage : UserControl
    {
        
        bool initialized = false;
        bool modulesBound = false;
        bool pbisBound = false;

        public PBIMappingPage()
        {
            InitializeComponent();
            splitContainer1.Visible = false;
            pnlBottomButtons.Visible = false;
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
            splitContainer1.Visible = true;
            BusyMessage.Show("Loading modules scanned in time range...", this.FindForm());
            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                List<ModuleEntity> modules = uow.ModuleRepository.GetModulesScannedAtFeederInRange(dpStartTime.Value.ToUniversalTime(), dpEndTime.Value.ToUniversalTime());
                var sorted = modules.OrderBy(t => t.LastFeederScanTime).ToList();

                modulesBound = false;
                gridViewModules.DataSource = null;
                gridViewModules.AutoGenerateColumns = false;                
                gridViewModules.DataSource = sorted;                
                gridViewModules.Visible = true;
                gridViewPBIScans.Visible = false;
                pnlBottomButtons.Visible = false;
                modulesBound = true;
                gridViewModules.ClearSelection();                
                gridViewModules.CurrentCell = null;
                pbisBound = false;
                gridViewPBIScans.DataSource = null;                
                lblPBIMessage.Visible = true;

            }
            BusyMessage.Close();

        }

        private void loadPBIScans()
        {

            if (gridViewModules.SelectedRows.Count == 0) return;

            DateTime start = DateTime.Now.AddYears(100);
            DateTime end = DateTime.Now.AddYears(-100);
            foreach(DataGridViewRow item in gridViewModules.SelectedRows)
            {
                var thisModule = (ModuleEntity)item.DataBoundItem;
                if (thisModule.LastFeederScanTime.HasValue)
                {
                    if (thisModule.LastFeederScanTime < start) start = thisModule.LastFeederScanTime.Value;
                    if (thisModule.LastFeederScanTime > end) end = thisModule.LastFeederScanTime.Value;
                }
            }


            BusyMessage.Show("Loading bales scanned in time range...", this.FindForm());
            Task.Run(() =>
            {
                
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {  
                    var startTime = start.AddMinutes(-30);
                    var endTime = end.AddMinutes(90);

                    List<BaleEntity> bales = uow.BalesRepository.FindMatching(x => x.Created >= startTime && x.Created <= endTime).OrderBy(x => x.Created).ThenBy(x => x.PbiNumber).ToList();
                   
                    this.Invoke((MethodInvoker)delegate
                    {
                        lblPBIScanHeader.Text = "Bales";
                        lblFeederScanHeader.Text = string.Format("Modules - {0} Selected - Time Range {1} to {2}", 
                            gridViewModules.SelectedRows.Count, 
                            startTime.ToLocalTime().ToString("MM/dd/yyyy hh:mm:ss tt"),
                            endTime.ToLocalTime().ToString("MM/dd/yyyy hh:mm:ss tt"));

                        pbisBound = false;
                        gridViewPBIScans.DataSource = null;
                        gridViewPBIScans.AutoGenerateColumns = false;                        
                        gridViewPBIScans.DataSource = bales;
                        gridViewPBIScans.Visible = true;
                        pbisBound = true;
                        gridViewPBIScans.ClearSelection();                        
                        gridViewPBIScans.CurrentCell = null;
                        pnlBottomButtons.Visible = false;
                        lblPBIMessage.Visible = false;
                        BusyMessage.Close();
                    });                    
                }                
            });

        }

        private void btnFetch_Click(object sender, EventArgs e)
        {
            refresh();
        }

        private void gridViewModules_MultiSelectChanged(object sender, EventArgs e)
        {
            
        }

        private void gridViewPBIScans_MultiSelectChanged(object sender, EventArgs e)
        {
           
        }

        private void gridViewModules_SelectionChanged(object sender, EventArgs e)
        {
            if (!modulesBound)
            {
                return;
            }

            if (gridViewModules.SelectedRows.Count > 0)
            {
                loadPBIScans();
            }
            else
            {
                lblFeederScanHeader.Text = "Modules - 0 selected";
                gridViewPBIScans.Visible = false;
                pnlBottomButtons.Visible = false;
                gridViewModules.ClearSelection();
                gridViewPBIScans.DataSource = null;
                pbisBound = false;
                lblPBIMessage.Visible = true;
            }
        }

        private void gridViewPBIScans_SelectionChanged(object sender, EventArgs e)
        {
            if (pbisBound == false)
            {            
                return;
            }

            if (gridViewPBIScans.SelectedRows.Count > 0)
            {
                lblPBIScanHeader.Text = "Bales - " + gridViewPBIScans.SelectedRows.Count.ToString() + " selected";
                pnlBottomButtons.Visible = true;
                lblPBIMessage.Visible = false;
            }
            else
            {
                lblPBIScanHeader.Text = "Bales";
                pnlBottomButtons.Visible = false;
            }
                        
            List<ModuleEntity> selectedModules = new List<ModuleEntity>();
            foreach(DataGridViewRow row in gridViewModules.SelectedRows)
            {
                selectedModules.Add((ModuleEntity)row.DataBoundItem);
            }

            List<BaleEntity> selectedBales = new List<BaleEntity>();
            foreach(DataGridViewRow row in gridViewPBIScans.SelectedRows)
            {
                selectedBales.Add((BaleEntity)row.DataBoundItem);
            }

            lblSelectedSeedCottonWeight.Text = "Calculating...";
            lblSelectedBaleWeight.Text = "Calculating...";
            Task.Run(() =>
            {
                decimal totalModuleWeight = 0.00M;
                foreach(var m in selectedModules)
                {
                    if (m.HIDModuleWeightLBS.HasValue) totalModuleWeight += m.HIDModuleWeightLBS.Value;
                    else if (m.DiameterApproximatedWeight.HasValue) totalModuleWeight += m.DiameterApproximatedWeight.Value;
                    else if (m.LoadAvgModuleWeight.HasValue) totalModuleWeight += m.LoadAvgModuleWeight.Value;
                }

                decimal totalBaleWeight = selectedBales.Sum(b => b.NetWeight);

                this.Invoke((MethodInvoker)delegate
                {
                    lblSelectedSeedCottonWeight.Text = totalModuleWeight.ToString("N2");
                    lblSelectedBaleWeight.Text = totalBaleWeight.ToString("N2");

                    if (totalModuleWeight == 0.00M)
                    {
                        lblLintTurnout.Text = "--";                        
                    }
                    else
                    {
                        lblLintTurnout.Text = (totalBaleWeight / totalModuleWeight).ToString("N2");
                    }
                });
            });
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            decimal turnOutRatio = 0.00M;
            decimal overragePercent = 0.00M;

            if (string.IsNullOrWhiteSpace(txtTurnOutMultiplier.Text) || !decimal.TryParse(txtTurnOutMultiplier.Text, out turnOutRatio) || turnOutRatio < 0.00M || turnOutRatio > 1.00M)
            {
                MessageBox.Show("Please enter a valid turn out as a decimal between 0 and 1.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtOverrageThreshold.Text) || !decimal.TryParse(txtOverrageThreshold.Text, out overragePercent) || overragePercent < 0.00M || overragePercent > 100.00M)
            {
                MessageBox.Show("Please enter a valid overrage percentage as a decimal between 0 and 100.");
                return;
            }

            List<ModuleEntity> modules = new List<ModuleEntity>();
            List<BaleEntity> bales = new List<BaleEntity>();

            int startModuleIndex = int.MaxValue;
            int endModuleIndex = -1;
            int startBaleIndex = int.MaxValue;

            //find index of first and last selected module rows
            foreach (DataGridViewRow row in gridViewModules.SelectedRows)
            {
                if (row.Index < startModuleIndex)
                    startModuleIndex = row.Index;

                if (row.Index > endModuleIndex)
                    endModuleIndex = row.Index;
            }

            //get list of modules we are going to accumulate bale weights for
            for (int i = startModuleIndex; i <= endModuleIndex; i++)
            {
                ModuleEntity m = (ModuleEntity)gridViewModules.Rows[i].DataBoundItem;
                m.Bales.Clear(); // need to clear bales because we will be re-assigning
                modules.Add(m);
            }

            //get index of first bale to use as starting point for accumulation
            foreach (DataGridViewRow row in gridViewPBIScans.SelectedRows)
            {
                if (row.Index < startBaleIndex)
                    startBaleIndex = row.Index;
            }

            var firstModule = modules.First();
            var lastModule = modules.Last();

            //get list of bales we are working with
            for (var i = startBaleIndex; i < gridViewPBIScans.Rows.Count; i++)
            {
                BaleEntity b = (BaleEntity)gridViewPBIScans.Rows[i].DataBoundItem;
                bales.Add(b);
            }

            var firstBale = bales.First();

            if (MessageBox.Show("Accumulation will start with module " + firstModule.Name
                + " and end with module " + lastModule.Name
                + ". Lint weight accumulation will begin with bale "
                + firstBale.PbiNumber + " using a turnout ratio of " + turnOutRatio.ToString() + ". Would you like to continue?  Any previous mappings for this data set will be overwritten.",
                "Continue?", MessageBoxButtons.YesNo)
                == DialogResult.Yes)
            {

                BusyMessage.Show("Step 1 - Calculating estimated load and module lint turn out.", this.FindForm());

                Task.Run(() =>
                {
                    try
                    {
                        using (IUnitOfWork uow = UnitOfWorkFactory.CreateUnitOfWork())
                        {
                            uow.ModuleRepository.DisableChangeTracking();                            
                            int moduleCount = 1;
                            foreach (var m in modules)
                            {
                                m.GinLoad.LintWeight = m.GinLoad.NetWeight * turnOutRatio;

                                if (m.HIDModuleWeightLBS.HasValue)
                                    m.NetSeedCottonWeight = m.HIDModuleWeightLBS.Value;
                                else if (m.DiameterApproximatedWeight.HasValue)
                                    m.NetSeedCottonWeight = m.DiameterApproximatedWeight.Value;
                                else
                                    m.NetSeedCottonWeight = m.LoadAvgModuleWeight.Value;
                                m.LintWeight = m.NetSeedCottonWeight * turnOutRatio;
                                uow.GinLoadRepository.QuickUpdate(m.GinLoad, false);
                                uow.ModuleRepository.QuickUpdate(m, false);

                                if (moduleCount % 25 == 0)
                                {
                                    BusyMessage.UpdateMessage("Step 1 - Calculating lint turn out for module " + moduleCount.ToString() + " of " + modules.Count().ToString());
                                }
                                moduleCount++;
                            }
                            BusyMessage.UpdateMessage("Step 1 - Saving lint weight estimates.");
                            uow.SaveChanges();
                        }

                            BusyMessage.UpdateMessage("Step 2 - Accumulating bale lint weights");

                        using (IUnitOfWork uow = UnitOfWorkFactory.CreateUnitOfWork())
                        {
                            uow.ModuleRepository.DisableChangeTracking();
                            uow.BalesRepository.DisableChangeTracking();
                            decimal overrage = 0.00M;
                            decimal carryOver = 0.00M;
                            int currentBaleIndex = 0;
                            decimal accumulatedLintWeight = 0.00M;
                            bool startNewAccumulation = true;

                            for (int i = 0; i < modules.Count(); i++)
                            {                                
                                if (i % 25 == 0)
                                {
                                    BusyMessage.UpdateMessage("Step 2 - Accumulating for module " + (i + 1).ToString() + " of " + modules.Count().ToString());
                                }
                                var m = modules[i];
                                startNewAccumulation = false;
                                while (!startNewAccumulation && currentBaleIndex < bales.Count())
                                {
                                    var b = bales[currentBaleIndex];                                    
                                    Logging.Logger.Log("INFO", "Accumulating weight for bale: " + b.PbiNumber);
                                    b.OverrageThreshold = overragePercent;
                                    b.LintTurnout = turnOutRatio;
                                    if (accumulatedLintWeight + bales[currentBaleIndex].NetWeight + carryOver <= m.LintWeight)
                                    {
                                        Logging.Logger.Log("INFO", "Bale weight under module weight");
                                        //map bale to current module/gin load
                                        accumulatedLintWeight += b.NetWeight + carryOver;
                                        b.ModuleId = m.Id;
                                        b.Module = null;
                                        b.ModuleSerialNumber = m.Name;
                                        b.GinLoadId = m.GinLoadId;
                                        b.GinTicketLoadNumber = m.GinLoad.GinTagLoadNumber;
                                        b.AccumWeight = accumulatedLintWeight;
                                        b.OverrageAdjustment = carryOver;
                                        b.LintTurnout = turnOutRatio;
                                        carryOver = 0.00M;
                                        uow.BalesRepository.QuickUpdate(b, true);
                                    }
                                    else
                                    {
                                        startNewAccumulation = true;
                                        overrage = accumulatedLintWeight + b.NetWeight - m.LintWeight.Value;
                                        accumulatedLintWeight += b.NetWeight + carryOver;
                                        //if > overrage % of weight belongs to next module map to next and subtract off portion belonging to current
                                        if ((overrage / b.NetWeight) > (1.00M - (overragePercent / 100.00M)))
                                        {
                                            Logging.Logger.Log("INFO", "Bale will be assigned to next module");
                                            int nextModuleIndex = i + 1;
                                            if (nextModuleIndex < modules.Count())
                                            {
                                                Logging.Logger.Log("INFO", "Bale ModuleID: " + b.ModuleId);
                                                b.ModuleId = modules[nextModuleIndex].Id;
                                                b.Module = null;
                                                b.ModuleSerialNumber = modules[nextModuleIndex].Name;
                                                b.GinLoadId = modules[nextModuleIndex].GinLoadId;
                                                b.GinTicketLoadNumber = modules[nextModuleIndex].GinLoad.GinTagLoadNumber;

                                                //adjust accumulation for next module by subtracting off weight from bale 
                                                //applied to current module
                                                carryOver = 0.00M - (b.NetWeight - overrage);
                                                accumulatedLintWeight = b.NetWeight + carryOver;
                                                b.AccumWeight = accumulatedLintWeight;
                                                b.OverrageAdjustment = carryOver;
                                                uow.BalesRepository.QuickUpdate(b, true);
                                                carryOver = 0;
                                            }
                                        }
                                        else //bale belongs to current module
                                        {
                                            Logging.Logger.Log("INFO", "Bale assigned to current module");
                                            b.ModuleId = m.Id;
                                            b.Module = null;
                                            b.ModuleSerialNumber = m.Name;
                                            b.GinLoadId = m.GinLoadId;
                                            b.GinTicketLoadNumber = m.GinLoad.GinTagLoadNumber;
                                            b.AccumWeight = accumulatedLintWeight;
                                            b.OverrageAdjustment = carryOver;
                                            uow.BalesRepository.QuickUpdate(b, true);
                                            carryOver = overrage;
                                            accumulatedLintWeight = 0.00M;
                                        }
                                    }
                                    if (b.Module != null && b.ModuleId != b.Module.Id)
                                    {
                                        Logging.Logger.Log("INFO", "MISMATCH IDS");
                                    }
                                    currentBaleIndex++;
                                }

                                if (i % 25 == 0) //save every 25 modules
                                    {
                                    uow.SaveChanges();
                                }
                            }
                            uow.SaveChanges();
                        }

                        BusyMessage.Close();
                    }
                    catch (Exception exc)
                    {
                        BusyMessage.Close();
                        this.Invoke((MethodInvoker)delegate
                        {
                            MessageBox.Show("An exception occurred: " + exc.Message);
                        });

                        Logging.Logger.Log(exc);
                    }
                });
            }

        }

        private void gridViewPBIScans_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for(var i = e.RowIndex; i < e.RowCount; i++)
            {
                BaleEntity b = (BaleEntity)gridViewPBIScans.Rows[i].DataBoundItem;
                if (b.OutOfSequence)
                {
                    foreach(DataGridViewCell c in gridViewPBIScans.Rows[i].Cells)
                    {
                        c.Style.BackColor = Color.Pink;
                    }
                }
            }
        }

        private void txtOverrageThreshold_Validating(object sender, CancelEventArgs e)
        {

        }
    }
}
