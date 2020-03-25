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
    public partial class AddEditGinLoad : Form
    {
        bool hasError = false;

        GinLoadEntity dataObj = null;

        public AddEditGinLoad()
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
                    if (dataObj != null)
                    {
                        dataObj = uow.GinLoadRepository.GetById(dataObj.Id, "Field.Farm.Client", "Modules");                        
                    }

                    clientSelector.Initialize(errorProvider, true, false, (dataObj != null) ? dataObj.Field.Farm.ClientId : "");
                    farmSelector.FormErrorProvider = errorProvider;
                    fieldSelector.FormErrorProvider = errorProvider;

                    BindingHelper.BindTruckComboByName(cboTruck, "-- Select One --", (dataObj != null) ? dataObj.TruckID : string.Empty);

                    if (dataObj != null)
                    {
                        tbBridgeId.Text = dataObj.ScaleBridgeId;
                        tbGinTagLoadNumber.Text = dataObj.GinTagLoadNumber;
                        tbGrossWeight.Text = dataObj.GrossWeight.ToString("0.00");
                        tbNetWeight.Text = dataObj.NetWeight.ToString("0.00");
                        tbSplitWeight1.Text = (dataObj.SplitWeight1.HasValue) ? dataObj.SplitWeight1.Value.ToString("0.00") : "";
                        tbSplitWeight2.Text = (dataObj.SplitWeight2.HasValue) ? dataObj.SplitWeight2.Value.ToString("0.00") : "";

                        if (!string.IsNullOrEmpty(dataObj.SubmittedBy))
                        {
                            rdoAttendant.Checked = dataObj.SubmittedBy.ToLower() == "attendant";
                            rdoDriver.Checked = dataObj.SubmittedBy.ToLower() == "driver";
                        }

                        tbScaleBridgeLoadNumber.Text = dataObj.ScaleBridgeLoadNumber.ToString();
                        tbPickedBy.Text = dataObj.PickedBy;
                        tbVariety.Text = dataObj.Variety;
                        tbTrailerNumber.Text = dataObj.TrailerNumber;
                        tbYardLocation.Text = dataObj.YardLocation;
                        lblModulesInLoad.Text = "";
                        foreach(var module in dataObj.Modules)
                        {
                            lblModulesInLoad.Text += module.Name + ", ";
                        }
                        lblModulesInLoad.Text = lblModulesInLoad.Text.Trim(", ".ToCharArray());
                    }
                    else
                    {
                        tbBridgeId.Text = string.Empty;
                        tbGinTagLoadNumber.Text = string.Empty;
                        tbGrossWeight.Text = string.Empty;
                        tbNetWeight.Text = string.Empty;
                        tbSplitWeight1.Text = string.Empty;
                        tbSplitWeight2.Text = string.Empty;
                        rdoAttendant.Checked = true;
                        rdoDriver.Checked = false;
                        tbScaleBridgeLoadNumber.Text = string.Empty;
                        tbPickedBy.Text = string.Empty;
                        tbVariety.Text = string.Empty;
                        tbTrailerNumber.Text = string.Empty;
                        tbYardLocation.Text = string.Empty;
                        lblModulesInLoad.Text = "--";
                        lblModulesInLoad.Text = string.Empty;
                    }
                    clearErrors();
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                throw new Exception("Error occurred loading gin load form.", exc);
            }
        }

        private void clearErrors()
        {
            hasError = false;
            clientSelector.ClearErrors();
            farmSelector.ClearErrors();
            fieldSelector.ClearErrors();

            errorProvider.SetError(cboTruck, "");
            errorProvider.SetError(tbScaleBridgeLoadNumber, "");
            errorProvider.SetError(tbPickedBy, "");
            errorProvider.SetError(tbTrailerNumber, "");
            errorProvider.SetError(tbVariety, "");
            errorProvider.SetError(tbYardLocation, "");
            errorProvider.SetError(tbGrossWeight, "");
            errorProvider.SetError(tbNetWeight, "");
            errorProvider.SetError(tbSplitWeight1, "");
            errorProvider.SetError(tbSplitWeight2, "");
            errorProvider.SetError(tbGinTagLoadNumber, "");
            errorProvider.SetError(tbBridgeId, "");            
        }


        private bool ValidateForm()
        {
            clearErrors();
            hasError = clientSelector.ValidateForm();

            bool farmError = farmSelector.ValidateForm();
            bool fieldError = fieldSelector.ValidateForm();

            if (farmError || fieldError) hasError = true;

            ValidateBridgeLoadNumber();
            cboTruck_Validating(this, new CancelEventArgs());
            tbBridgeId_Validating(this, new CancelEventArgs());
            tbGinTagLoadNumber_Validating(this, new CancelEventArgs());
            tbGrossWeight_Validating(this, new CancelEventArgs());
            tbNetWeight_Validating(this, new CancelEventArgs());
            tbSplitWeight1_Validating(this, new CancelEventArgs());
            tbSplitWeight2_Validating(this, new CancelEventArgs());

            return !hasError;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            BusyMessage.Show("Saving...", this.FindForm());
            try
            {
                ValidateForm();
                if (!hasError)
                {
                    using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                    {                        
                        GinLoadEntity existingLoad = null;

                        if (dataObj == null)
                        {
                            existingLoad = new GinLoadEntity();
                        }
                        else
                        {
                            existingLoad = uow.GinLoadRepository.GetById(dataObj.Id, "Field.Farm.Client");
                        }

                        //ensure client, farm, and field are created
                        var client = clientSelector.GetOrCreateClientEntity();
                        var farm = farmSelector.GetOrCreateFarmEntity(client);
                        var field = fieldSelector.GetOrCreateFieldEntity(farm);

                        string truck = (cboTruck.SelectedIndex > 0) ? ((BaseEntity)cboTruck.SelectedItem).Name : "";

                        existingLoad.Name = tbScaleBridgeLoadNumber.Text.Trim();
                        existingLoad.ScaleBridgeLoadNumber = int.Parse(tbScaleBridgeLoadNumber.Text.Trim());
                        existingLoad.ScaleBridgeId = tbBridgeId.Text.Trim();
                        existingLoad.GinTagLoadNumber = tbGinTagLoadNumber.Text.Trim();
                        existingLoad.GrossWeight = decimal.Parse(tbGrossWeight.Text.Trim());
                        existingLoad.NetWeight = decimal.Parse(tbNetWeight.Text.Trim());

                        if (!string.IsNullOrWhiteSpace(tbSplitWeight1.Text))
                        {
                            existingLoad.SplitWeight1 = decimal.Parse(tbSplitWeight1.Text.Trim());
                        }
                        else
                        {
                            existingLoad.SplitWeight1 = null;
                        }

                        if (!string.IsNullOrWhiteSpace(tbSplitWeight2.Text))
                        {
                            existingLoad.SplitWeight2 = decimal.Parse(tbSplitWeight2.Text.Trim());
                        }
                        else
                            existingLoad.SplitWeight2 = null;

                        if (rdoAttendant.Checked) existingLoad.SubmittedBy = "attendant";
                        else existingLoad.SubmittedBy = "driver";

                        existingLoad.TruckID = truck;
                        existingLoad.YardLocation = tbYardLocation.Text;
                        existingLoad.PickedBy = tbPickedBy.Text;
                        existingLoad.Variety = tbVariety.Text;
                        existingLoad.TrailerNumber = tbTrailerNumber.Text;
                        existingLoad.ScaleBridgeId = tbBridgeId.Text;
                        existingLoad.FieldId = field.Id;
                        existingLoad.SyncedToCloud = false;
                        uow.GinLoadRepository.Save(existingLoad);
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
            catch (Exception exc)
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

            this.Text = "Add Gin Load";

            return this.ShowDialog();
        }

        public DialogResult ShowEdit(GinLoadEntity doc)
        {
            dataObj = doc;
            //await LoadForm();
            this.Text = "Update Gin Load";
            return this.ShowDialog();
        }

        private void ValidateBridgeLoadNumber()
        {
            if (string.IsNullOrWhiteSpace(tbScaleBridgeLoadNumber.Text))
            {
                errorProvider.SetError(tbScaleBridgeLoadNumber, "Load# number is requred.");
                hasError = true;
            }
            else if (!Helpers.ValidationHelper.ValidInt(tbScaleBridgeLoadNumber.Text, 1, int.MaxValue))
            {
                errorProvider.SetError(tbScaleBridgeLoadNumber, "Invalid number.");
                hasError = true;
            }
            else
            {
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    var nextNumber = uow.GinLoadRepository.LastLoadNumber() + 1;
                    bool result = uow.GinLoadRepository.CanSaveLoadNumber((dataObj != null) ? dataObj.Id : string.Empty, tbScaleBridgeLoadNumber.Text.Trim());
                    if (!result)
                    {
                        hasError = true;
                        errorProvider.SetError(tbScaleBridgeLoadNumber, "Load number should be " + nextNumber.ToString());
                    }
                }
            }
        }        

        private async void clientSelector_SelectionChanged(object sender, EventArgs e)
        {
            if (clientSelector.ExistingSelected || clientSelector.IsNew)
            {
                farmSelector.Visible = true;
                await farmSelector.Initialize(errorProvider, true, clientSelector.IsNew, clientSelector.SelectedClientId, (dataObj != null) ? dataObj.Field.Farm.Id : "");
            }
            else
            {
                farmSelector.Visible = false;
                fieldSelector.Visible = false;
            }
        }

        private async void farmSelector_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (farmSelector.ExistingSelected || farmSelector.IsNew)
                {
                    fieldSelector.Visible = true;
                    await fieldSelector.Initialize(errorProvider, true, farmSelector.IsNew, farmSelector.SelectedFarmId, (dataObj != null) ? dataObj.FieldId : "");
                }
                else
                {
                    fieldSelector.Visible = false;
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                throw new Exception("Error initalizing field drop down.", exc);
            }
        }

        private void fieldSelector_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void cboTruck_Validating(object sender, CancelEventArgs e)
        {
            if (cboTruck.SelectedIndex == 0)
            {
                errorProvider.SetError(cboTruck, "Truck is requred.");
                hasError = true;
            }
        }     

        private async void AddEditGinLoad_Shown(object sender, EventArgs e)
        {            
            this.Enabled = false;
            await LoadForm();
            this.Enabled = true;            
            clientSelector.Focus();
        }                

        private void tbBridgeId_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbBridgeId.Text))
            {
                errorProvider.SetError(tbBridgeId, "Bridge ID is required.");
                hasError = true;
            }
        }

        private void tbScaleBridgeLoadNumber_Validating(object sender, CancelEventArgs e)
        {
            ValidateBridgeLoadNumber();
        }

        private void tbGinTagLoadNumber_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbGinTagLoadNumber.Text))
            {
                errorProvider.SetError(tbGinTagLoadNumber, "Gin tag load # is required.");
                hasError = true;
            }
            else
            {
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {                    
                    bool result = uow.GinLoadRepository.CanGinTagLoadNumber((dataObj != null) ? dataObj.Id : string.Empty, tbGinTagLoadNumber.Text.Trim());
                    if (!result)
                    {
                        hasError = true;
                        errorProvider.SetError(tbGinTagLoadNumber, "Gin ticket load number already in use.");
                    }
                }
            }
        }

        private void tbGrossWeight_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbGrossWeight.Text))
            {
                errorProvider.SetError(tbGrossWeight, "Gross weight is required.");
                hasError = true;
            }
            else if (!Helpers.ValidationHelper.ValidDecimal(tbGrossWeight.Text))
            {
                errorProvider.SetError(tbGrossWeight, "Invalid decimal number.");
                hasError = true;
            }
        }

        private void tbNetWeight_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbNetWeight.Text))
            {
                errorProvider.SetError(tbNetWeight, "Net weight is required.");
                hasError = true;
            }
            else if (!Helpers.ValidationHelper.ValidDecimal(tbNetWeight.Text))
            {
                errorProvider.SetError(tbNetWeight, "Invalid decimal number.");
                hasError = true;
            }
        }

        private void tbSplitWeight1_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbSplitWeight1.Text.Trim()) || !Helpers.ValidationHelper.ValidDecimal(tbSplitWeight1.Text.Trim()))
            {
                errorProvider.SetError(tbSplitWeight1, "Invalid decimal number.");
                hasError = true;
            }
        }

        private void tbSplitWeight2_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbSplitWeight2.Text.Trim()) || !Helpers.ValidationHelper.ValidDecimal(tbSplitWeight2.Text.Trim()))
            {
                errorProvider.SetError(tbSplitWeight2, "Invalid decimal number.");
                hasError = true;
            }
        }

        private void CalcWeights()
        {
            string truck = (cboTruck.SelectedIndex > 0) ? ((BaseEntity)cboTruck.SelectedItem).Name : "";
            Task.Run(() =>
            {
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {                    
                    var truckEntity = uow.TruckRepository.FindSingle(t => t.Name == truck);
                    decimal truckTareWeight = 0.00M;
                    if (truckEntity != null)
                    {
                        truckTareWeight = truckEntity.TareWeight;
                    }
                    SetWeights(truckTareWeight);                    
                }
            });
        }

        private void SetWeights(decimal tareWeight)
        {
            decimal w1 = 0.00M;
            decimal w2 = 0.00M;
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    SetWeights(tareWeight);
                });
            }
            else
            {

                if (!decimal.TryParse(tbSplitWeight1.Text, out w1)) w1 = 0.00M;
                if (!decimal.TryParse(tbSplitWeight2.Text, out w2)) w2 = 0.00M;

                tbGrossWeight.Text = (w1 + w2).ToString("N2");
                tbNetWeight.Text = (w1 + w2 - tareWeight).ToString("N2");
            }
        }

        private void tbSplitWeight1_TextChanged(object sender, EventArgs e)
        {
            CalcWeights();
        }

        private void tbSplitWeight2_TextChanged(object sender, EventArgs e)
        {
            CalcWeights();
        }

        private void cboTruck_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcWeights();
        }
    }
}
