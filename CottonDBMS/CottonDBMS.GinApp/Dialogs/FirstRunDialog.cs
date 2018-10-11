//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using CottonDBMS.GinApp.Helpers;
using CottonDBMS.GinApp.Classes;
using CottonDBMS.GinApp.Dialogs;
using CottonDBMS.Interfaces;
using CottonDBMS.Cloud;

namespace CottonDBMS.GinApp.Dialogs
{
    public partial class FirstRunDialog : Form
    {
        bool hasError = false;

        public FirstRunDialog()
        {
            InitializeComponent();
        }

        private void hideAllSteps()
        {
            pnlIntro.Visible = false;
            pnlStep1ImportExport.Visible = false;
            pnlStep2EmailImport.Visible = false;
            pnlStep3GeoCoding.Visible = false;
            pnlStep4CosmosSettings.Visible = false;
            pnlStep6LoadNumbering.Visible = false;
            pnlStep5GoogleMaps.Visible = false;        
        }

        private void clearErrors()
        {
            errorProvider.SetError(tbIMAPPort, "");
            errorProvider.SetError(tbYardNWLatitude, "");
            errorProvider.SetError(tbYardNWLongitude, "");
            errorProvider.SetError(tbYardSELatitude, "");
            errorProvider.SetError(tbYardSELongitude, "");
            errorProvider.SetError(tbFeederLatitude, "");
            errorProvider.SetError(tbFeederLongitude, "");
            errorProvider.SetError(tbFeederDetectionRadius, "");
            errorProvider.SetError(tbIMAPHostname, "");
            errorProvider.SetError(tbIMAPPassword, "");
            errorProvider.SetError(tbIMAPUsername, "");
            errorProvider.SetError(tbImportInterval, "");
            errorProvider.SetError(tbMapsAPIKey, "");
            errorProvider.SetError(tbAzureCosmosEndpoint, "");
            errorProvider.SetError(tbAzureKey, "");
            errorProvider.SetError(tbAzureCosmosReadOnlyEndPoint, "");
            errorProvider.SetError(tbAzureReadOnlyKey, "");
            errorProvider.SetError(tbStartingLoadNumber, "");
            errorProvider.SetError(tbModulesPerLoad, "");
            errorProvider.SetError(tbGinName, "");
            hasError = false;
        }

        #region Import Settings Events
        private void btnIntroNext_Click(object sender, EventArgs e)
        {
            hideAllSteps();
            pnlStep1ImportExport.Visible = true;
        }        

        private void btnBackStep1_Click(object sender, EventArgs e)
        {
            hideAllSteps();
            pnlIntro.Visible = true;
        }

        private void btnNextStep1_Click(object sender, EventArgs e)
        {
            hideAllSteps();
            pnlStep2EmailImport.Visible = true;

            if (lblImportFilesFolderValue.Text == "--")
            {
                
            }
        }

        private void btnExitIntro_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
            folderDialog.SelectedPath = lblImportFilesFolderValue.Text;
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                lblArchiveFolderValue.Text = folderDialog.SelectedPath;
            }
        }

        #endregion

        private async Task<bool> CreateDatabase()
        {
            DocumentDBContext.Initialize(tbAzureCosmosEndpoint.Text.Trim(), tbAzureKey.Text.Trim());
            bool exists = await DocumentDBContext.DatabaseExistsAsync();
            bool createSucceded = true;

            if (!exists)
            {                
                try
                {
                    await DocumentDBContext.CreateDatabaseAsync();
                }
                catch (Exception exc)
                {
                    Logging.Logger.Log(exc);
                    createSucceded = false;                                        
                }
            }

            return createSucceded;
        }

        private async Task CreateCollection()
        {
            var exists = await DocumentDBContext.CollectionExistsAsync();

            if (!exists)
            {
                await DocumentDBContext.CreateCollectionAsync();
                await DocumentDBContext.CreateStoredProceduresAsync();
            }
        }


        private async Task<bool> validateForm()
        {
            clearErrors();


            if (pnlStep1ImportExport.Visible)
            {

            }
            else if (pnlStep2EmailImport.Visible)
            {
                this.tbIMAPHostname_Validating(this, new CancelEventArgs());
                this.tbIMAPPort_Validating(this, new CancelEventArgs());
            }
            else if (pnlStep3GeoCoding.Visible)
            {
                this.tbFeederLatitude_Validating(this, new CancelEventArgs());
                this.tbFeederLongitude_Validating(this, new CancelEventArgs());
                this.tbYardNWLatitude_Validating(this, new CancelEventArgs());
                this.tbYardNWLongitude_Validating(this, new CancelEventArgs());
                this.tbYardSELatitude_Validating(this, new CancelEventArgs());
                this.tbYardSELongitude_Validating(this, new CancelEventArgs());
            }
            else if (pnlStep4CosmosSettings.Visible)
            {
                this.tbAzureCosmosEndpoint_Validating(this, new CancelEventArgs());
                this.tbAzureKey_Validating(this, new CancelEventArgs());
                this.tbAzureCosmosReadOnlyEndPoint_Validating(this, new CancelEventArgs());
                this.tbAzureReadOnlyKey_Validating(this, new CancelEventArgs());

                if (!hasError)
                {
                    try
                    {

                        if (!string.IsNullOrEmpty(tbAzureCosmosEndpoint.Text) && !string.IsNullOrEmpty(tbAzureKey.Text))
                        {
                            await CreateDatabase();

                            await CreateCollection();

                            bool primaryEndpointWriteable = await CottonDBMS.Cloud.DocumentDBContext.CanWrite(tbAzureCosmosEndpoint.Text, tbAzureKey.Text);

                            if (!primaryEndpointWriteable)
                            {
                                hasError = true;
                                MessageBox.Show("Unable to write using Azure Read/Write endpoint and key.  Please check you entered the endpoint and key correctly.");                                
                            }
                        }

                        if (!hasError)
                        {
                            bool readonlyWriteable = await CottonDBMS.Cloud.DocumentDBContext.CanWrite(tbAzureCosmosReadOnlyEndPoint.Text, tbAzureReadOnlyKey.Text);
                            bool readOnyReadable = false;

                            int retries = 0;
                            while (!readOnyReadable && retries < 30)
                            {
                                readOnyReadable = await CottonDBMS.Cloud.DocumentDBContext.CanRead(tbAzureCosmosReadOnlyEndPoint.Text, tbAzureReadOnlyKey.Text);
                                retries++;
                                System.Threading.Thread.Sleep(1000);
                            }


                            if (readonlyWriteable)
                            {
                                hasError = true;
                                MessageBox.Show("Azure Cosmos Read Only settings are allowing write access.  Please ensure you have not entered the read/write key for the READ only key.");
                            }
                            else if (!readOnyReadable)
                            {
                                hasError = true;
                                MessageBox.Show("Unable to read from Azure Cosmos DB.  Please verify your READ ONLY Uri and Key is correct.");
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        hasError = true;
                        Logging.Logger.Log(exc);
                        Logging.Logger.CleanUp();
                        MessageBox.Show("Unable to verify Azure endpoints. Please verify that you have entered the endpoints and keys correctly.");
                    }
                }

            }
            else if (pnlStep5GoogleMaps.Visible)
            {
                this.tbMapsAPIKey_Validating(this, new CancelEventArgs());
            }
            else if (pnlStep6LoadNumbering.Visible)
            {
                this.tbStartingLoadNumber_Validating(this, new CancelEventArgs());
                this.tbModulesPerLoad_Validating(this, new CancelEventArgs());
                this.tbGinName_Validating(this, new CancelEventArgs());
            }            
           
            return !hasError;
        }

        #region Step 2 Email Import Events
        private void btnBackStep2EmailImport_Click(object sender, EventArgs e)
        {
            hideAllSteps();
            pnlStep1ImportExport.Visible = true;
        }

        private async void btnNextStep2EmailImport_Click(object sender, EventArgs e)
        {
            if (await validateForm())
            {
                hideAllSteps();
                pnlStep3GeoCoding.Visible = true;
            }
        }
        #endregion

        #region Step 3 - geo-coding step button events
        private void pnlBackStep3GeoCoding_Click(object sender, EventArgs e)
        {
            hideAllSteps();
            pnlStep2EmailImport.Visible = true;
        }

        private async void btnNextStep3GeoCoding_Click(object sender, EventArgs e)
        {
            if (await validateForm())
            {
                hideAllSteps();
                pnlStep4CosmosSettings.Visible = true;
            }
        }
        #endregion

        #region Validation Methods
        private void tbIMAPHostname_Validating(object sender, CancelEventArgs e)
        {

        }

        private void tbIMAPPort_Validating(object sender, CancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbIMAPPort.Text) && !ValidationHelper.ValidPort(tbIMAPPort.Text))
            {
                errorProvider.SetError(tbIMAPPort, "Port is invalid.");
                hasError = true;
            }
        }

        private void tbYardNWLatitude_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidationHelper.ValidateLatLong(tbYardNWLatitude.Text))
            {
                errorProvider.SetError(tbYardNWLatitude, "Latitude must be decimal number between -180 and 180.");
                hasError = true;
            }
        }

        private void tbYardNWLongitude_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidationHelper.ValidateLatLong(tbYardNWLongitude.Text))
            {
                errorProvider.SetError(tbYardNWLongitude, "Longitude must be decimal number between -180 and 180.");
                hasError = true;
            }
        }

        private void tbYardSELatitude_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidationHelper.ValidateLatLong(tbYardSELatitude.Text))
            {
                errorProvider.SetError(tbYardSELatitude, "Latitude must be decimal number between -180 and 180.");
                hasError = true;
            }
        }

        private void tbYardSELongitude_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidationHelper.ValidateLatLong(tbYardSELongitude.Text))
            {
                errorProvider.SetError(tbYardSELongitude, "Longitude must be decimal number between -180 and 180..");
                hasError = true;
            }
        }

        private void tbFeederLatitude_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidationHelper.ValidateLatLong(tbFeederLatitude.Text))
            {
                errorProvider.SetError(tbFeederLatitude, "Latitude must be decimal number between -180 and 180..");
                hasError = true;
            }
        }

        private void tbFeederLongitude_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidationHelper.ValidateLatLong(tbFeederLongitude.Text))
            {
                errorProvider.SetError(tbFeederLongitude, "Longitude must be decimal number between -180 and 180..");
                hasError = true;
            }
        }

        private void tbMapsAPIKey_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbMapsAPIKey.Text))
            {
                errorProvider.SetError(tbMapsAPIKey, "Maps API key is required.");
                hasError = true;
            }
        }

        private void tbAzureCosmosEndpoint_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbAzureCosmosEndpoint.Text))
            {
                errorProvider.SetError(tbAzureCosmosEndpoint, "Azure Cosmos DB endpoint is required.");
                hasError = true;
            }
            else if (!ValidationHelper.ValidUrl(tbAzureCosmosEndpoint.Text))
            {
                errorProvider.SetError(tbAzureCosmosEndpoint, "Endpoint is not a valid url.");
                hasError = true;
            }
        }

        private void tbAzureKey_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbAzureKey.Text))
            {
                errorProvider.SetError(tbAzureKey, "Azure Cosmos DB api key is required.");
                hasError = true;
            }
        }

        private void tbGinName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbGinName.Text))
            {
                errorProvider.SetError(tbGinName, "Gin name is required.  This name will be displayed in RFID Module Scan when connected.");
                hasError = true;
            }
        }

        private void blLoadPrefix_Validating(object sender, CancelEventArgs e)
        {

        }

        private void tbStartingLoadNumber_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidationHelper.ValidInt(tbStartingLoadNumber.Text, 1, int.MaxValue))
            {
                errorProvider.SetError(tbStartingLoadNumber, "Starting load number must be a number greater than 0.");
                hasError = true;
            }
        }

        private void tbModulesPerLoad_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidationHelper.ValidInt(tbModulesPerLoad.Text, 1, int.MaxValue))
            {
                errorProvider.SetError(tbModulesPerLoad, "Modules per load must be a number greater than 0.");
                hasError = true;
            }
        }

        private void tbAzureCosmosReadOnlyEndPoint_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbAzureCosmosReadOnlyEndPoint.Text))
            {
                errorProvider.SetError(tbAzureCosmosReadOnlyEndPoint, "Read only endpoint is required.");
                hasError = true;
            }
            else if (!ValidationHelper.ValidUrl(tbAzureCosmosReadOnlyEndPoint.Text))
            {
                errorProvider.SetError(tbAzureCosmosReadOnlyEndPoint, "Endpoint is not a valid url.");
                hasError = true;
            }
        }

        private void tbAzureReadOnlyKey_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbAzureReadOnlyKey.Text))
            {
                errorProvider.SetError(tbAzureReadOnlyKey, "Azure Cosmos DB read only api key is required.");
                hasError = true;
            }
        }
        #endregion


        #region Step 4 - Cosmos DB Settings
        private void btnStep4BackCosmosSettings_Click(object sender, EventArgs e)
        {
            hideAllSteps();
            pnlStep3GeoCoding.Visible = true;
        }

        private async void btnStep4NextCosmosSettings_Click(object sender, EventArgs e)
        {            
            if (await validateForm())
            {
                hideAllSteps();
                pnlStep5GoogleMaps.Visible = true;
            }
        }
        #endregion

        #region Step 5 - Google Maps API Key
        private void btnStep5BackMaps_Click(object sender, EventArgs e)
        {
            hideAllSteps();
            pnlStep4CosmosSettings.Visible = true;
        }

        private async void btnNextStep5GoogleMaps_Click(object sender, EventArgs e)
        {
            if (await validateForm())
            {
                hideAllSteps();
                pnlStep6LoadNumbering.Visible = true;
            }
        }
        #endregion

        #region Step 6 Load Numbering
        private void btnBackStep6LoadNumbering_Click(object sender, EventArgs e)
        {
            hideAllSteps();
            pnlStep5GoogleMaps.Visible = true;
        }

        private async void btnFinish_Click(object sender, EventArgs e)
        {            
            if (await validateForm())
            {
                //save settings
                ConfigHelper.SaveSettingsFromUIControls(lblImportFilesFolderValue, lblArchiveFolderValue, tbIMAPHostname, tbIMAPPort, tbIMAPUsername, tbIMAPPassword,
                   tbYardNWLatitude, tbYardNWLongitude, tbYardSELatitude, tbYardSELongitude, tbFeederLatitude, tbFeederLongitude, tbFeederDetectionRadius,
                   tbMapsAPIKey, tbImportInterval, tbAzureCosmosEndpoint, tbAzureKey, tbAzureCosmosReadOnlyEndPoint, tbAzureReadOnlyKey, tbLoadPrefix, tbStartingLoadNumber, tbModulesPerLoad, tbGinName);
                                
                //exit
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        #endregion
        
        private void FirstRunDialog_Load(object sender, EventArgs e)
        {
            hideAllSteps();
            pnlIntro.Visible = true;
        }

        private void FirstRunDialog_Shown(object sender, EventArgs e)
        {
            hideAllSteps();
            pnlIntro.Visible = true;

            lblImportFilesFolderValue.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\RFID Gin Data\\Imports\\";
            lblArchiveFolderValue.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\RFID Gin Data\\Pickup List Archive\\";
            tbIMAPPort.Text = "993";

            if (!Directory.Exists(lblImportFilesFolderValue.Text))
            {
                Directory.CreateDirectory(lblImportFilesFolderValue.Text);
            }
            
            if (!Directory.Exists(lblArchiveFolderValue.Text))
            {
                Directory.CreateDirectory(lblArchiveFolderValue.Text);
            }
        }  
        
    }
}
