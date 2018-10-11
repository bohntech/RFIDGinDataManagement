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
    public partial class ChangeStatusDialog : Form
    {
        bool hasError = false;
        string[] _serialNumbers = null;
        public ChangeStatusDialog()
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

        public void LoadForm()
        {                        
            try
            {
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {                   
                    clientSelector1.Initialize(errorProvider, true, false, "");
                    farmSelector1.FormErrorProvider = errorProvider;
                    fieldSelector1.FormErrorProvider = errorProvider;

                    lblSerialNumbers.Text = "";
                    foreach (var sn in _serialNumbers)
                    {
                        lblSerialNumbers.Text += sn + ",";
                    }
                    lblSerialNumbers.Text = lblSerialNumbers.Text.TrimEnd(',');
                    BindingHelper.BindModuleStatusCombo(tbStatus, "-- Select One --", null);    
                    clearErrors();
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                throw new Exception("Error occurred loading module form.", exc);
            }
        }

        private void clearErrors()
        {
            clientSelector1.ClearErrors();
            farmSelector1.ClearErrors();
            fieldSelector1.ClearErrors();

            hasError = false;           
            errorProvider.SetError(tbStatus, "");
        }

        private void ChangeStatusDialog_Load(object sender, EventArgs e)
        {

        }

        private bool ValidateForm()
        {
            clearErrors();
            hasError = clientSelector1.ValidateForm();

            bool farmError = farmSelector1.ValidateForm();
            bool fieldError = fieldSelector1.ValidateForm();

            if (farmError || fieldError) hasError = true;

            tbStatus_Validating(this, new CancelEventArgs());           
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
                        BaseEntity selectedStatus = (BaseEntity)tbStatus.SelectedItem;
                        ModuleStatus selectedModuleStatus = (ModuleStatus)int.Parse(selectedStatus.Id);

                        //ensure client, farm, and field are created
                        var client = clientSelector1.GetOrCreateClientEntity();
                        var farm = farmSelector1.GetOrCreateFarmEntity(client);
                        var field = fieldSelector1.GetOrCreateFieldEntity(farm);

                        foreach (var sn in _serialNumbers)
                        {
                            ModuleEntity existingModule = uow.ModuleRepository.FindSingle(x => x.Name == sn, "ModuleHistory");                            
                            existingModule.ModuleStatus = selectedModuleStatus;
                            existingModule.FieldId = field.Id;
                            ModuleHistoryEntity historyItem = new ModuleHistoryEntity
                            {
                                Id = Guid.NewGuid().ToString(),
                                Created = DateTime.UtcNow,
                                Driver = existingModule.Driver,
                                TruckID = existingModule.TruckID,
                                Latitude = existingModule.Latitude,
                                Longitude = existingModule.Longitude,
                                ModuleStatus = existingModule.ModuleStatus,                                
                                ModuleEventType = ModuleEventType.MANUAL_EDIT
                            };                            
                            existingModule.ModuleHistory.Add(historyItem);
                            existingModule.SyncedToCloud = false;
                            uow.ModuleRepository.Save(existingModule);
                        }
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

        public DialogResult Show(string[] serialNumbers)
        {
            //await LoadForm();
            _serialNumbers = serialNumbers;
            this.Text = "Change Module Status";

            return this.ShowDialog();
        }

        private void ChangeStatusDialog_Shown(object sender, EventArgs e)
        {
            //BusyMessage.Show("Loading...", this.FindForm());
            this.Enabled = false;
            LoadForm();
            this.Enabled = true;
            //BusyMessage.Close();
            tbStatus.Focus();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private async void clientSelector1_SelectionChanged(object sender, EventArgs e)
        {
            if (clientSelector1.ExistingSelected || clientSelector1.IsNew)
            {
                farmSelector1.Visible = true;
                await farmSelector1.Initialize(errorProvider, true, clientSelector1.IsNew, clientSelector1.SelectedClientId, "");
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
                    await fieldSelector1.Initialize(errorProvider, true, farmSelector1.IsNew, farmSelector1.SelectedFarmId, "");
                }
                else
                {
                    fieldSelector1.Visible = false;
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                throw new Exception("Error initalizing field drop down.", exc);
            }
        }

        private void fieldSelector1_SelectionChanged(object sender, EventArgs e)
        {

        }      

        private void tbStatus_Validating(object sender, CancelEventArgs e)
        {
            if (tbStatus.SelectedIndex == 0)
            {
                errorProvider.SetError(tbStatus, "Status is required.");
                hasError = true;
            }
        }
    }
}
