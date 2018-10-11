//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using CottonDBMS.DataModels;
using CottonDBMS.GinApp.Helpers;
using CottonDBMS.GinApp.Classes;
using CottonDBMS.GinApp.Dialogs;
using CottonDBMS.Interfaces;
using CottonDBMS.Cloud;
using System.Threading.Tasks;

namespace CottonDBMS.GinApp.UserControls
{
    public partial class SettingsPage : UserControl
    {
        private bool hasError = false;

        public SettingsPage()
        {
            InitializeComponent();
        }

        private void SettingsPage_Load(object sender, EventArgs e)
        {
            
        }

        private void clearErrors()
        {
            formErrorProvider.SetError(tbIMAPPort, "");
            formErrorProvider.SetError(tbYardNWLatitude, "");
            formErrorProvider.SetError(tbYardNWLongitude, "");
            formErrorProvider.SetError(tbYardSELatitude, "");
            formErrorProvider.SetError(tbYardSELongitude, "");
            formErrorProvider.SetError(tbFeederLatitude, "");
            formErrorProvider.SetError(tbFeederLongitude, "");
            formErrorProvider.SetError(tbFeederDetectionRadius, "");
            formErrorProvider.SetError(tbIMAPHostname, "");
            formErrorProvider.SetError(tbIMAPPassword, "");
            formErrorProvider.SetError(tbIMAPUsername, "");
            formErrorProvider.SetError(tbImportInterval, "");
            formErrorProvider.SetError(tbMapsAPIKey, "");
            formErrorProvider.SetError(tbAzureCosmosEndpoint, "");
            formErrorProvider.SetError(tbAzureKey, "");
            formErrorProvider.SetError(tbStartingLoadNumber, "");
            formErrorProvider.SetError(tbModulesPerLoad, "");
            formErrorProvider.SetError(tbGinName, "");
            hasError = false;
        }

        public void LoadData()
        {
            clearErrors();
            using (IUnitOfWork uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                var allSettings = uow.SettingsRepository.GetAll();

                Setting importFolderSetting = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.IMPORT_FOLDER);
                Setting archiveFolderSetting = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.ARCHIVE_FOLDER);

                Setting hostnameSetting = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.IMAP_HOSTNAME);
                Setting portSetting = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.IMAP_PORT);
                Setting usernameSetting = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.IMAP_USERNAME);
                Setting passwordSetting = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.IMAP_PASSWORD);

                Setting ginYardNWCornerNorthSetting = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.GIN_YARD_NW_CORNER_NORTH);
                Setting ginYardNWCornerWestSetting = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.GIN_YARD_NW_CORNER_WEST);

                Setting ginYardSECornerNorthSetting = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.GIN_YARD_SE_CORNER_NORTH);
                Setting ginYardSECornerWestSetting = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.GIN_YARD_SE_CORNER_WEST);

                Setting ginFeederNorthSetting = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.GIN_FEEDER_NORTH);
                Setting ginFeederWestSetting = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.GIN_FEEDER_WEST);

                Setting ginFeederDetectionRadiusSetting = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.GIN_FEEDER_DETECTION_RADIUS);
                Setting googleMapsApiKeySetting = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.GOOGLE_MAPS_API_KEY);
                Setting importScheduleSettingKey = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.IMPORT_INTERVAL);

                Setting azureEndpointSetting = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.AZURE_DOCUMENTDB_ENDPOINT);
                Setting azureDocumentDBKeySetting = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.AZURE_DOCUMENTDB_KEY);

                Setting azureEndpointReadOnlySetting = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.AZURE_DOCUMENTDB_READONLY_ENDPOINT);
                Setting azureDocumentDBReadOnlyKeySetting = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.AZURE_DOCUMENTDB_READONLY_KEY);

                Setting loadPrefixSetting = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.LOAD_PREFIX);
                Setting loadNumberSetting = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.STARTING_LOAD_NUMBER);
                Setting modulesPerLoadSetting = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.MODULES_PER_LOAD);
                Setting ginNameSetting = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.GIN_NAME);

                lblImportFilesFolderValue.Text = (importFolderSetting != null) ? importFolderSetting.Value : "";
                lblArchiveFolderValue.Text = (archiveFolderSetting != null) ? archiveFolderSetting.Value : "";
                tbIMAPHostname.Text = (hostnameSetting != null) ? hostnameSetting.Value : "";
                tbIMAPPort.Text = (portSetting != null) ? portSetting.Value : "";
                tbIMAPUsername.Text = (usernameSetting != null) ? usernameSetting.Value : "";
                tbIMAPPassword.Text = (passwordSetting != null) ? passwordSetting.Value : "";
                tbYardNWLatitude.Text = (ginYardNWCornerNorthSetting != null) ? ginYardNWCornerNorthSetting.Value : "";
                tbYardNWLongitude.Text = (ginYardNWCornerWestSetting != null) ? ginYardNWCornerWestSetting.Value : "";
                tbYardSELatitude.Text = (ginYardSECornerNorthSetting != null) ? ginYardSECornerNorthSetting.Value : "";
                tbYardSELongitude.Text = (ginYardSECornerWestSetting != null) ? ginYardSECornerWestSetting.Value : "";
                tbFeederLatitude.Text = (ginFeederNorthSetting != null) ? ginFeederNorthSetting.Value : "";
                tbFeederLongitude.Text = (ginFeederWestSetting != null) ? ginFeederWestSetting.Value : "";
                tbFeederDetectionRadius.Text = (ginFeederDetectionRadiusSetting != null) ? ginFeederDetectionRadiusSetting.Value : "";
                tbMapsAPIKey.Text = (googleMapsApiKeySetting != null) ? googleMapsApiKeySetting.Value : "";
                tbImportInterval.Value = (importScheduleSettingKey != null) ? decimal.Parse(importScheduleSettingKey.Value) : 5.0M;
                tbAzureCosmosEndpoint.Text = (azureEndpointSetting != null) ? azureEndpointSetting.Value : "";
                tbAzureKey.Text = (azureDocumentDBKeySetting != null) ? azureDocumentDBKeySetting.Value : "";

                tbAzureCosmosReadOnlyEndPoint.Text = (azureEndpointReadOnlySetting != null) ? azureEndpointReadOnlySetting.Value : "";
                tbAzureReadOnlyKey.Text = (azureDocumentDBReadOnlyKeySetting != null) ? azureDocumentDBReadOnlyKeySetting.Value : "";

                tbLoadPrefix.Text = (loadPrefixSetting != null) ? loadPrefixSetting.Value : "";
                tbStartingLoadNumber.Text = (loadNumberSetting != null) ? loadNumberSetting.Value : "";
                tbModulesPerLoad.Text = (modulesPerLoadSetting != null) ? modulesPerLoadSetting.Value : "";
                tbGinName.Text = (ginNameSetting != null) ? ginNameSetting.Value : "";

                btnShowQRCode.Visible = !string.IsNullOrEmpty(tbAzureCosmosEndpoint.Text);

                Task.Run(async () =>
                {
                    //try to write - this also ensure the write test document is in the cloud for the read test
                    if (azureEndpointSetting != null && azureDocumentDBKeySetting != null)
                    {
                        await DocumentDBContext.CanWrite(azureEndpointSetting.Value, azureDocumentDBKeySetting.Value);
                    }
                });
            }
        }

        private void tbIMAPHostname_Validating(object sender, CancelEventArgs e)
        {

        }

        private void tbIMAPPort_Validating(object sender, CancelEventArgs e)
        {   
            if (!ValidationHelper.ValidPort(tbIMAPPort.Text))
            {
                formErrorProvider.SetError(tbIMAPPort, "Port is invalid.");
                hasError = true;
            }
        }

        private void tbYardNWLatitude_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidationHelper.ValidateLatLong(tbYardNWLatitude.Text))
            {
                formErrorProvider.SetError(tbYardNWLatitude, "Latitude must be decimal number between -180 and 180.");
                hasError = true;
            }
        }

        private void tbYardNWLongitude_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidationHelper.ValidateLatLong(tbYardNWLongitude.Text))
            {
                formErrorProvider.SetError(tbYardNWLongitude, "Longitude must be decimal number between -180 and 180.");
                hasError = true;
            }
        }

        private void tbYardSELatitude_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidationHelper.ValidateLatLong(tbYardSELatitude.Text))
            {
                formErrorProvider.SetError(tbYardSELatitude, "Latitude must be decimal number between -180 and 180.");
                hasError = true;
            }
        }

        private void tbYardSELongitude_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidationHelper.ValidateLatLong(tbYardSELongitude.Text))
            {
                formErrorProvider.SetError(tbYardSELongitude, "Longitude must be decimal number between -180 and 180..");
                hasError = true;
            }
        }

        private void tbFeederLatitude_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidationHelper.ValidateLatLong(tbFeederLatitude.Text))
            {
                formErrorProvider.SetError(tbFeederLatitude, "Latitude must be decimal number between -180 and 180..");
                hasError = true;
            }
        }

        private void tbFeederLongitude_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidationHelper.ValidateLatLong(tbFeederLongitude.Text))
            {
                formErrorProvider.SetError(tbFeederLongitude, "Longitude must be decimal number between -180 and 180..");
                hasError = true;
            }
        }

        private void tbMapsAPIKey_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbMapsAPIKey.Text))
            {
                formErrorProvider.SetError(tbMapsAPIKey, "Maps API key is required.");
                hasError = true;
            }
        }

        private void tbAzureCosmosEndpoint_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbAzureCosmosEndpoint.Text))
            {
                formErrorProvider.SetError(tbAzureCosmosEndpoint, "Azure Document DB endpoint is required.");
                hasError = true;
            }
            else if (!ValidationHelper.ValidUrl(tbAzureCosmosEndpoint.Text))
            {
                formErrorProvider.SetError(tbAzureCosmosEndpoint, "Endpoint is not a valid url.");
                hasError = true;
            }
        }

        private void tbAzureKey_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbAzureKey.Text))
            {
                formErrorProvider.SetError(tbAzureKey, "Azure Document DB endpoint is required.");
                hasError = true;
            }
        }

        private void tbGinName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbGinName.Text))
            {
                formErrorProvider.SetError(tbGinName, "Gin name is required.  This name will be displayed in RFID Module Scan when connected.");
                hasError = true;
            }
        }

        private void blLoadPrefix_Validating(object sender, CancelEventArgs e)
        {

        }

        private void tbStartingLoadNumber_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidationHelper.ValidInt(tbStartingLoadNumber.Text, 1, int.MaxValue)) {
                formErrorProvider.SetError(tbStartingLoadNumber, "Starting load number must be a number greater than 0.");
                hasError = true;
            }
        }

        private void tbModulesPerLoad_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidationHelper.ValidInt(tbModulesPerLoad.Text, 1, int.MaxValue))
            {
                formErrorProvider.SetError(tbModulesPerLoad, "Modules per load must be a number greater than 0.");
                hasError = true;
            }
        }

        private bool validateForm()
        {
            clearErrors();
            this.tbAzureCosmosEndpoint_Validating(this, new CancelEventArgs());
            this.tbAzureKey_Validating(this, new CancelEventArgs());
            this.tbFeederLatitude_Validating(this, new CancelEventArgs());
            this.tbFeederLongitude_Validating(this, new CancelEventArgs());
            this.tbIMAPHostname_Validating(this, new CancelEventArgs());
            this.tbIMAPPort_Validating(this, new CancelEventArgs());
            this.tbMapsAPIKey_Validating(this, new CancelEventArgs());
            this.tbYardNWLatitude_Validating(this, new CancelEventArgs());
            this.tbYardNWLongitude_Validating(this, new CancelEventArgs());
            this.tbYardSELatitude_Validating(this, new CancelEventArgs());
            this.tbYardSELongitude_Validating(this, new CancelEventArgs());
            this.tbStartingLoadNumber_Validating(this, new CancelEventArgs());
            this.tbModulesPerLoad_Validating(this, new CancelEventArgs());
            this.tbGinName_Validating(this, new CancelEventArgs());
            return !hasError;                    
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            if (validateForm())
            {                
                BusyMessage.Show("Saving.", this.FindForm());

                ConfigHelper.SaveSettingsFromUIControls(lblImportFilesFolderValue, lblArchiveFolderValue, tbIMAPHostname, tbIMAPPort, tbIMAPUsername, tbIMAPPassword,
                    tbYardNWLatitude, tbYardNWLongitude, tbYardSELatitude, tbYardSELongitude, tbFeederLatitude, tbFeederLongitude, tbFeederDetectionRadius,
                    tbMapsAPIKey, tbImportInterval, tbAzureCosmosEndpoint, tbAzureKey, tbAzureCosmosReadOnlyEndPoint, tbAzureReadOnlyKey, tbLoadPrefix, tbStartingLoadNumber, tbModulesPerLoad, tbGinName);
                
               System.Threading.Thread.Sleep(1000);
               BusyMessage.Close();              
            }
        }

        private void btnChangeImportFolder_Click(object sender, EventArgs e)
        {
            folderDialog.SelectedPath = lblImportFilesFolderValue.Text;
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                lblImportFilesFolderValue.Text = folderDialog.SelectedPath;
            }
        }

        private void btnChangeArchiveFolder_Click(object sender, EventArgs e)
        {
            folderDialog.SelectedPath = lblArchiveFolderValue.Text;
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                lblArchiveFolderValue.Text = folderDialog.SelectedPath;
            }
        }

        private void btnClearAllData_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear all data?  This will delete all data in the system.  Data stored on truck computers should be cleared on each truck.", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    uow.TruckRepository.ClearTruckData();
                }
            }
        }

        private async void btnClearModuleData_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete ALL pickup list, modules, and all module location history?", "Warning!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                    {
                        BusyMessage.Show("Suspending background sync", this.FindForm());
                        CottonDBMS.EF.Tasks.GinSyncWithCloudTask.Cancel();
                        CottonDBMS.EF.Tasks.GinSyncWithCloudTask.WaitForSyncToFinish();

                        BusyMessage.UpdateMessage("Clearing module data");
                        uow.ModuleRepository.ClearGinModuleData();

                        await CottonDBMS.Cloud.DocumentDBContext.DeleteCollectionAsync();
                        await CottonDBMS.Cloud.DocumentDBContext.CreateCollectionAsync();
                        await CottonDBMS.Cloud.DocumentDBContext.CreateStoredProceduresAsync();

                        var docsToProcess = uow.DocumentsToProcessRepository.GetAll().ToList();
                        uow.DocumentsToProcessRepository.BulkDelete(docsToProcess);

                        uow.ClientRepository.MarkAllDirty();
                        uow.FarmRepository.MarkAllDirty();
                        uow.FieldRepository.MarkAllDirty();
                        uow.TruckRepository.MarkAllDirty();
                        uow.DriverRepository.MarkAllDirty();

                        uow.SaveChanges();

                        BusyMessage.UpdateMessage("Restarting background sync");
                        CottonDBMS.EF.Tasks.GinSyncWithCloudTask.Reset();

                        BusyMessage.Close();
                    }
                }
                catch(Exception exc)
                {
                    Logging.Logger.Log(exc);
                    BusyMessage.Close();
                    MessageBox.Show("An error occurred clearing data: " + exc.Message);
                }
            }
        }

        private async void btnShowQRCode_Click(object sender, EventArgs e)
        {
            QRCodeDialog dialog = new QRCodeDialog();
            BusyMessage.Show("Please wait...", this.FindForm());
            await dialog.LoadDialogAsync();
            BusyMessage.Close();
            var result = dialog.ShowDialog();            
        }

       
    }
}
