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
using CottonDBMS.Interfaces;
using CottonDBMS.GinApp.Helpers;
using CottonDBMS.GinApp.Classes;
using CottonDBMS.DataModels;

namespace CottonDBMS.GinApp.Dialogs
{
    public partial class AddEditModule : Form
    {
        bool hasError = false;

        ModuleEntity dataObj = null;
        
        public AddEditModule()
        {
            InitializeComponent();

            this.AcceptButton = btnSave;

            BusyMessage.OnBusyMessageShown += BusyMessage_OnBusyMessageShown;
            BusyMessage.OnBusyMessageClosed += BusyMessage_OnBusyMessageClosed;
        }

        private void BusyMessage_OnBusyMessageClosed(object sender, EventArgs e)
        {
            this.Enabled = true;
        }

        private void BusyMessage_OnBusyMessageShown(object sender, EventArgs e)
        {
            this.Enabled = false;
        }

        public async Task LoadForm()
        {
            //initialize client drop down
            try
            {
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    ModuleStatus? status = null;
                    if (dataObj != null)
                    {
                        dataObj = uow.ModuleRepository.GetById(dataObj.Id, "Field.Farm.Client");
                        status = dataObj.ModuleStatus;
                    }

                    clientSelector1.Initialize(errorProvider, true, false, (dataObj != null) ? dataObj.Field.Farm.ClientId : "");

                    farmSelector1.FormErrorProvider = errorProvider;
                    fieldSelector1.FormErrorProvider = errorProvider;
                                      

                    BindingHelper.BindModuleStatusCombo(tbStatus, "-- Select One --", status);
                    BindingHelper.BindDriverComboByName(cboDriver, "-- Select One --", (dataObj != null) ? dataObj.Driver : string.Empty);
                    BindingHelper.BindTruckComboByName(cboTruck, "-- Select One --", (dataObj != null) ? dataObj.TruckID : string.Empty);

                    if (dataObj != null)
                    {
                        tbSerialNumber.Text = dataObj.Name;
                        if (!string.IsNullOrEmpty(dataObj.LoadNumber))  lblLoad.Text = dataObj.LoadNumber;
                        else lblLoad.Text = "Not assigned";

                        tbGinTicketLoadNumber.Text = dataObj.GinTagLoadNumber;
                        tbLatitude.Text = dataObj.Latitude.ToString();
                        tbLongitude.Text = dataObj.Longitude.ToString();
                        lblImportedLoad.Text = dataObj.ImportedLoadNumber;

                        lblDiameter.Text = dataObj.HIDDiameterInches.NullableToStringHighPrecision();
                        lblFieldArea.Text = dataObj.HIDFieldAreaAcres.NullableToStringHighPrecision();
                        lblIncrementalArea.Text = dataObj.HIDIncrementalAreaAcres.NullableToStringHighPrecision();
                        lblModuleWeight.Text = dataObj.HIDModuleWeightLBS.NullableToStringHighPrecision();
                        lblMoisturePercent.Text = dataObj.HIDMoisture.NullableToString();
                        lblOperator.Text = dataObj.HIDOperator;
                        lblProducerID.Text = dataObj.HIDProducerID;
                        lblSeasonTotal.Text = dataObj.HIDSeasonTotal.NullableToString();                        
                        lblVariety.Text = dataObj.HIDVariety;

                        lblHIDFieldTotal.Text = dataObj.HIDFieldTotal.NullableToString();
                        lblGinID.Text = dataObj.HIDGinID;
                        lblMachinePIN.Text = dataObj.HIDMachinePIN;
                        lblHIDDropLat.Text = dataObj.HIDDropLat.ToString();
                        lblHIDDropLong.Text = dataObj.HIDDropLong.ToString();
                        lblHIDLat.Text = dataObj.HIDLat.ToString();
                        lblHIDLong.Text = dataObj.HIDLong.ToString();
                        //lblHIDWrapLat.Text = dataObj.HIDWrapLat.ToString();
                        //lblHIDWrapLong.Text = dataObj.HIDWrapLong.ToString();
                        lblHIDTimestamp.Text = dataObj.HIDTimestamp.HasValue ? dataObj.HIDTimestamp.Value.ToLocalTime().ToString("MM/dd/yyyy hh:mm:ss tt") : "";


                        tbNotes.Text = (dataObj.Notes ?? "");
                        tbModuleID.Text = (dataObj.ModuleId ?? "");
                        //tbStatus
                    }
                    else
                    {
                        tbSerialNumber.Text = string.Empty;
                        lblLoad.Text = "Not assigned";
                        tbLatitude.Text = string.Empty;
                        tbLongitude.Text = string.Empty;
                        lblImportedLoad.Text = string.Empty;

                        lblDiameter.Text = string.Empty;
                        lblFieldArea.Text = string.Empty;
                        lblIncrementalArea.Text = string.Empty;
                        lblModuleWeight.Text = string.Empty;
                        lblMoisturePercent.Text = string.Empty;
                        lblOperator.Text = string.Empty;
                        lblProducerID.Text = string.Empty;
                        lblSeasonTotal.Text = string.Empty;
                        lblVariety.Text = string.Empty;


                        tbModuleID.Text = "--";
                        tbNotes.Text = "";
                    }
                    clearErrors();
                }
            }
            catch(Exception exc)
            {
                Logging.Logger.Log(exc);
                throw new Exception("Error occurred loading module form.", exc);
            }
        }

        private void clearErrors()
        {
            hasError = false;
            clientSelector1.ClearErrors();
            farmSelector1.ClearErrors();
            fieldSelector1.ClearErrors();

            errorProvider.SetError(tbSerialNumber, "");            
            errorProvider.SetError(tbLongitude, "");
            errorProvider.SetError(tbLatitude, "");
            errorProvider.SetError(tbStatus, "");
        }        

        private void AddEditModule_Load(object sender, EventArgs e)
        {

        }

        private bool ValidateForm()
        {
            clearErrors();
            hasError = clientSelector1.ValidateForm();

            bool farmError = farmSelector1.ValidateForm();
            bool fieldError = fieldSelector1.ValidateForm();

            if (farmError || fieldError) hasError = true;

            ValidateSerialNumber();
                       
            tbStatus_Validating(this, new CancelEventArgs());
            tbLatitude_Validating(this, new CancelEventArgs());
            tbLongitude_Validating(this, new CancelEventArgs());
            tbModuleID_Validating(this, new CancelEventArgs());

            return !hasError;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {   
            try
            {
                ValidateForm();
                if (!hasError)
                {

                    if (dataObj != null && !string.IsNullOrWhiteSpace(dataObj.GinTagLoadNumber) && string.IsNullOrWhiteSpace(tbGinTicketLoadNumber.Text))
                    {
                        if (MessageBox.Show("You have removed the gin ticket load number from this module.  Are you sure you want to save?  This will remove the module from gin ticket load " + dataObj.GinTagLoadNumber + ".", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes)
                            return;
                    }

                   
                    using (var uow = UnitOfWorkFactory.CreateUnitOfWork()) {

                        var ginLoadId = uow.GinLoadRepository.GetLoadIdForGinTicketNumber(tbGinTicketLoadNumber.Text.Trim());

                        if (string.IsNullOrEmpty(ginLoadId) && !string.IsNullOrWhiteSpace(tbGinTicketLoadNumber.Text))
                        {
                            if (MessageBox.Show("You have entered a gin ticket load number that does not yet have a matching load record from the scale bridge.  Are you sure you want to save? ", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                return;
                        }


                        BusyMessage.Show("Saving...", this.FindForm());
                        BaseEntity selectedStatus = (BaseEntity)tbStatus.SelectedItem;
                        ModuleStatus selectedModuleStatus = (ModuleStatus)int.Parse(selectedStatus.Id);
                        ModuleEntity existingModule = null;

                        if (dataObj == null)
                        {
                            existingModule = new ModuleEntity();
                        }
                        else
                        {
                            existingModule = uow.ModuleRepository.GetById(dataObj.Id, "Field.Farm.Client");
                        }

                        //ensure client, farm, and field are created
                        var client = clientSelector1.GetOrCreateClientEntity();
                        var farm = farmSelector1.GetOrCreateFarmEntity(client);
                        var field = fieldSelector1.GetOrCreateFieldEntity(farm);
                        
                        
                        string truck = (cboTruck.SelectedIndex > 0) ? ((BaseEntity)cboTruck.SelectedItem).Name : "";
                        string driver = (cboDriver.SelectedIndex > 0) ? ((BaseEntity)cboDriver.SelectedItem).Name : "";
                        
                        existingModule.Name = tbSerialNumber.Text.Trim();
                        existingModule.ModuleId = tbModuleID.Text.Trim();                    
                        existingModule.FieldId = field.Id;                        
                        existingModule.Name = tbSerialNumber.Text.Trim();
                        existingModule.Driver = driver;
                        existingModule.TruckID = truck;
                        existingModule.Latitude = double.Parse(tbLatitude.Text);
                        existingModule.Longitude = double.Parse(tbLongitude.Text);
                        existingModule.ModuleStatus = selectedModuleStatus;
                        existingModule.Notes = tbNotes.Text.Trim();
                        existingModule.GinLoadId = ginLoadId;
                        existingModule.GinTagLoadNumber = tbGinTicketLoadNumber.Text.Trim();

                        ModuleHistoryEntity historyItem = new ModuleHistoryEntity
                        {
                            Id = Guid.NewGuid().ToString(),
                            Created = DateTime.UtcNow,
                            Driver = existingModule.Driver,
                            TruckID = existingModule.TruckID,
                            Latitude = existingModule.Latitude,
                            Longitude = existingModule.Longitude,
                            ModuleEventType = ModuleEventType.MANUAL_EDIT
                        };
                        historyItem.ModuleStatus = existingModule.ModuleStatus;
                        existingModule.ModuleHistory.Add(historyItem);
                        existingModule.SyncedToCloud = false;
                        uow.ModuleRepository.Save(existingModule);
                        uow.SaveChanges();

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else
                {
                    this.Activate();
                }
            }
            catch(Exception exc)
            {
                Logging.Logger.Log(exc);
                MessageBox.Show("An error occurred trying to save module.");
            }
            finally
            {
                BusyMessage.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public DialogResult ShowAdd()
        {
            //await LoadForm();

            this.Text = "Add Module";

            return this.ShowDialog();
        }

        public DialogResult ShowEdit(ModuleEntity doc)
        {
            dataObj = doc;            
            //await LoadForm();
            this.Text = "Update Module";
            return this.ShowDialog();
        }

        private void ValidateSerialNumber()
        {
            if (string.IsNullOrWhiteSpace(tbSerialNumber.Text))
            {
                errorProvider.SetError(tbSerialNumber, "Serial number is requred.");
                hasError = true;
            }
            else
            {
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    bool result = uow.ModuleRepository.CanSaveModule((dataObj != null) ? dataObj.Id : string.Empty, tbSerialNumber.Text.Trim());
                    if (!result)
                    {
                        hasError = true;
                        errorProvider.SetError(tbSerialNumber, "Serial number already exists.");
                    }
                }
            }
        }

        private void tbSerialNumber_Validating(object sender, CancelEventArgs e)
        {
            ValidateSerialNumber();
        }       

        private void tbLatitude_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidationHelper.ValidateLatLong(tbLatitude.Text))
            {
                errorProvider.SetError(tbLatitude, "Latitude must be a decimal between -180 and 180.");
                hasError = true;
            }
        }

        private void tbLongitude_Validating(object sender, CancelEventArgs e)
        {
            if (!ValidationHelper.ValidateLatLong(tbLongitude.Text))
            {
                errorProvider.SetError(tbLongitude, "Longitude must be a decimal between -180 and 180.");
                hasError = true;
            }
        }

        private void tbStatus_Validating(object sender, CancelEventArgs e)
        {
            if (tbStatus.SelectedIndex == 0)
            {
                errorProvider.SetError(tbStatus, "Status is required.");
                hasError = true;
            }
        }

        private async void clientSelector1_SelectionChanged(object sender, EventArgs e)
        {
            if (clientSelector1.ExistingSelected || clientSelector1.IsNew)
            {
                farmSelector1.Visible = true;
                await farmSelector1.Initialize(errorProvider, true, clientSelector1.IsNew, clientSelector1.SelectedClientId, (dataObj != null) ? dataObj.Field.Farm.Id : "");             
            }
            else
            {
                farmSelector1.Visible = false;
                fieldSelector1.Visible = false;             
            }
        }

        private async void farmSelector1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (farmSelector1.ExistingSelected || farmSelector1.IsNew)
                {
                    fieldSelector1.Visible = true;
                    await fieldSelector1.Initialize(errorProvider, true, farmSelector1.IsNew, farmSelector1.SelectedFarmId, (dataObj != null) ? dataObj.FieldId : "");
                }
                else
                {
                    fieldSelector1.Visible = false;
                }
            }
            catch(Exception exc)
            {
                Logging.Logger.Log(exc);
                throw new Exception("Error initalizing field drop down.", exc);
            }
        }

        private void fieldSelector1_SelectionChanged(object sender, EventArgs e)
        {
           
        }

        private void cboTruck_Validating(object sender, CancelEventArgs e)
        {
           
        }

        private void cboDriver_Validating(object sender, CancelEventArgs e)
        {

        }

        private async void AddEditModule_Shown(object sender, EventArgs e)
        {
            //BusyMessage.Show("Loading...", this.FindForm());
            this.Enabled = false;
            await LoadForm();
            this.Enabled = true;
            //BusyMessage.Close();
            clientSelector1.Focus();
        }

        private void tbModuleID_Validating(object sender, CancelEventArgs e)
        {
            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                if (string.IsNullOrWhiteSpace(tbModuleID.Text))
                {
                    errorProvider.SetError(tbModuleID, "Module ID is required.");
                    hasError = true;
                }
                else if (uow.ModuleRepository.ModuleIDExists((dataObj != null) ? dataObj.Id : string.Empty, tbModuleID.Text.Trim())) {
                    errorProvider.SetError(tbModuleID, "Module ID already exists.");
                    hasError = true;
                }
            }
        }

        private void tbGinTicketLoadNumber_Validating(object sender, CancelEventArgs e)
        {
            
        }
    }
}
