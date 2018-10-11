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
using CottonDBMS.GinApp.Classes;
using CottonDBMS.Interfaces;
using CottonDBMS.DataModels;
using CottonDBMS.GinApp.Helpers;
using CottonDBMS.Cloud;
using CottonDBMS.EF.Tasks;

namespace CottonDBMS.GinApp.UserControls
{
    public partial class HomePage : UserControl
    {
        private bool importRunning = false;
        private DateTime? lastImport = null;
        private bool initialized = false;
        private int importInterval = 5;
        private string importPath = "";

        public HomePage()
        {
            InitializeComponent();
        }       



        public void LoadData()
        {
            if (!syncStatusUpdateTimer.Enabled) syncStatusUpdateTimer.Start();


            if (!importRunning)
            {                            
                importTimer.Enabled = false;
                
                Task.Run(() =>
                {
                    importInterval = ConfigHelper.ImportInterval;
                    lastImport = ConfigHelper.LastImportDateTime;
                    importPath = ConfigHelper.ImportFolder;             

                    string statusText = ConfigHelper.LastImportStatus;
                    int modulesPerLoad = ConfigHelper.ModulesPerLoad;

                    int loadsInField = 0;
                    int loadsPickedUp = 0;
                    int loadsAtGin = 0;
                    int loadsGinned = 0;
                    int totalLoads = 0;
                    int totalModules = 0;
                    int modulesInField = 0;
                    int modulesOnYard = 0;
                    int modulesGinned = 0;

                    using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                    {
                        loadsInField = uow.ModuleRepository.GetLoadCountByStatus(ModuleStatus.IN_FIELD, modulesPerLoad);
                        loadsPickedUp = uow.ModuleRepository.GetLoadCountByStatus(ModuleStatus.PICKED_UP, modulesPerLoad);
                        loadsAtGin = uow.ModuleRepository.GetLoadCountByStatus(ModuleStatus.AT_GIN, modulesPerLoad);
                        loadsGinned = uow.ModuleRepository.GetLoadCountByStatus(ModuleStatus.GINNED, modulesPerLoad);
                        totalLoads = loadsInField + loadsPickedUp + loadsAtGin + loadsGinned;
                        totalModules = uow.ModuleRepository.GetTotalModuleCount();
                        modulesInField = uow.ModuleRepository.GetModuleCountByStatus(ModuleStatus.IN_FIELD);
                        modulesOnYard = uow.ModuleRepository.GetModuleCountByStatus(ModuleStatus.AT_GIN);
                        modulesGinned = uow.ModuleRepository.GetModuleCountByStatus(ModuleStatus.GINNED);
                    }

                    this.Invoke((MethodInvoker)delegate
                    {
                        lblTotalModulesValue.Text = totalModules.ToString();
                        lblTotalLoadsValue.Text = totalLoads.ToString();
                        lblTotalModulesInFieldValue.Text = modulesInField.ToString();
                        lblTotalLoadsInFieldValue.Text = loadsInField.ToString();
                        lblModulesOnYardValue.Text = modulesOnYard.ToString();
                        lblModulesGinnedValue.Text = modulesGinned.ToString();

                        if (lastImport.HasValue)
                        {
                            lastImport = ConfigHelper.LastImportDateTime.Value;
                            lblLastImportTimeValue.Text = lastImport.ToString();
                        }
                        else
                        {
                            lblLastImportTimeValue.Text = "--";
                        }

                        if (!string.IsNullOrWhiteSpace(statusText))
                        {
                            lblStatus.Text = statusText;
                            if (lblStatus.Text.ToLower().IndexOf("error") >= 0) lblStatus.ForeColor = Color.Red;
                            else lblStatus.ForeColor = Color.Green;
                        }
                        else
                        {
                            lblStatus.Text = "Waiting for next import.";
                        }

                        initialized = true;
                        importTimer.Enabled = true;
                    });
                    
                });
            }

            backupTimer.Enabled = true;
        }

        private void runImport()
        {
            if (!importRunning)
            {
                lblStatus.Visible = true;
                btnImport.Enabled = false;
                btnRefresh.Enabled = false;
                lblStatus.ForeColor = Color.Green;
                importRunning = true;
                

                Task.Run(async () =>
                {

                    bool emailImportSuccess = runEmailImport();
                    DateTime startTime = DateTime.Now;
                    int errorCount = await runFileImport();
                    string statusText = "Import finished.";
                    if (errorCount > 0 || !emailImportSuccess)
                    {
                        statusText = "Import finished with errors.  See application log for more details.";
                    }
                    lastImport = startTime;
                    using (IUnitOfWork uow = UnitOfWorkFactory.CreateUnitOfWork())
                    {
                        var settingsRepo = uow.SettingsRepository;
                        settingsRepo.UpsertSetting(GinAppSettingKeys.LAST_LOCAL_IMPORT_TIMESTAMP, startTime.ToString());
                        settingsRepo.UpsertSetting(GinAppSettingKeys.LAST_IMPORT_STATUS, statusText);
                        uow.SaveChanges();
                    }  
                    importRunning = false;
                    this.Invoke((MethodInvoker)delegate
                    {
                        btnImport.Enabled = true;
                        btnRefresh.Enabled = true;
                    });

                    LoadData();
                });
            }
        }

        private bool runEmailImport()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(importPath))
                {
                    string emailFolder = importPath.TrimEnd('\\') + "\\emailimports\\";
                    System.IO.Directory.CreateDirectory(emailFolder);
                    MailImportTask importTask = new MailImportTask(ConfigHelper.ImapHostName, ConfigHelper.ImapPassword, ConfigHelper.ImapPort, 
                        ConfigHelper.ImapUsername, emailFolder);
                    importTask.OnProgressUpdate += LocalImportTask_OnProgressUpdate;

                    if (lastImport.HasValue)
                    {
                        importTask.Run(lastImport.Value);
                    }
                    else
                    {
                        importTask.Run(DateTime.Now.AddHours(-1));
                    }

                    importTask.OnProgressUpdate -= LocalImportTask_OnProgressUpdate;
                }

                return true;
            }
            catch(Exception exc)
            {
                Logging.Logger.Log("ERROR", "An error occurred running email import task.");
                Logging.Logger.Log(exc);

                return false;
            }
        }      

        private async Task<int> runFileImport()
        {
            DateTime startFiles = (lastImport.HasValue) ? lastImport.Value : new DateTime(1980, 1, 1);
            List<string> importFolders = new List<string>();
            importFolders.Add(importPath.TrimEnd('\\') + "\\emailimports\\");
            importFolders.Add(importPath.TrimEnd('\\') + "\\");            
            LocalFileImportTask localImportTask = new LocalFileImportTask(startFiles, importFolders.ToArray());                                    
            localImportTask.OnProgressUpdate += LocalImportTask_OnProgressUpdate;            
            int errors = await localImportTask.Run();                                
            localImportTask.OnProgressUpdate -= LocalImportTask_OnProgressUpdate;
            return errors;
        }
        
        private void btnImport_Click(object sender, EventArgs e)
        {
            runImport();
        }

        private void LocalImportTask_OnProgressUpdate(string statusMessage)
        {
            this.Invoke((MethodInvoker)delegate
            {
                lblStatus.ForeColor = Color.Green;
                lblStatus.Text = statusMessage;
            });
        }

        private void importTimer_Tick(object sender, EventArgs e)
        {
            if (initialized)
            {
                if (!lastImport.HasValue) runImport();
                else
                {
                    if (lastImport.Value.AddMinutes(importInterval) < DateTime.Now)
                    {
                        runImport();
                    }
                }
            }
        }

        private void btnOpenLog_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Logging.Logger.CurrentLogFile);
            }
            catch (Exception exc)
            {                
                MessageBox.Show("Could not open log file. " + exc.Message);
            }
        }

        private void tbnRefresh_Click(object sender, EventArgs e)
        {
            if (!importRunning) {
                BusyMessage.Show("Updating module counts.", this.FindForm());
                btnRefresh.Enabled = false;
                btnImport.Enabled = false;
                importRunning = true;
                Task.Run(() =>
                {                    
                    this.Invoke((MethodInvoker)delegate
                    {
                        LoadData();
                        btnRefresh.Enabled = true;
                        btnImport.Enabled = true;
                        importRunning = false;
                        BusyMessage.Close();
                    });
                });
            }
        }

        private void btnDataSync_Click(object sender, EventArgs e)
        {
            GinSyncWithCloudTask.ForceSyncRun();
        }

        private void syncStatusUpdateTimer_Tick(object sender, EventArgs e)
        {
            btnDataSync.Enabled = !GinSyncWithCloudTask.IsProcessing;
            lblLastRunTime.Text = GinSyncWithCloudTask.LastRun.HasValue ? GinSyncWithCloudTask.LastRun.Value.ToString("MM/dd/yyyy hh:mm tt") : "--";
            lblSyncStatus.Text = GinSyncWithCloudTask.StatusMessage;
        }

        private void backupTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                using (IUnitOfWork uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    uow.BackupDB(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).TrimEnd('\\') + "\\" + FolderConstants.ROOT_APP_DATA_FOLDER + "\\" + FolderConstants.GIN_APP_DATA_FOLDER);
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }
    }
}
