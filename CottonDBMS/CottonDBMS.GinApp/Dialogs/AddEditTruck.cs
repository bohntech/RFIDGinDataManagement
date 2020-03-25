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
using CottonDBMS.DataModels;
using CottonDBMS.GinApp.Helpers;

namespace CottonDBMS.GinApp.Dialogs
{
    public partial class AddEditTruck : Form
    {
        bool hasError = false;
        TruckEntity  truckObj =  null;

        public AddEditTruck()
        {
            InitializeComponent();

            this.AcceptButton = btnSave;
        }

        public DialogResult ShowAdd()
        {
            clearErrors();
            tbTruckID.Text = "";            
            this.Text = "Add Truck";
            tbPhone.Text = "";
            tbOwner.Text = "";
            tbRFIDTag.Text = "";
            tbLicensePlate.Text = "";
            cbxIsSemi.Checked = false;
            tbTareWeight.Text = "";
            return this.ShowDialog();
        }

        public DialogResult ShowEdit(TruckEntity doc)
        {
            clearErrors();
            tbTruckID.Text = doc.Name;
            tbLoadPrefix.Text = doc.LoadPrefix;
            tbTareWeight.Text = doc.TareWeight.ToString();
            cbxIsSemi.Checked = doc.IsSemi;
            tbRFIDTag.Text = doc.RFIDTagId;
            tbLicensePlate.Text = doc.LicensePlate;
            tbOwner.Text = doc.OwnerName;
            tbPhone.Text = doc.OwnerPhone;
            truckObj = doc;
            this.Text = "Update Truck";            
            return this.ShowDialog();
        }

        private void clearErrors()
        {
            formErrorProvider.SetError(tbTruckID, "");
            formErrorProvider.SetError(tbTareWeight, "");
            formErrorProvider.SetError(tbRFIDTag, "");
            formErrorProvider.SetError(tbPhone, "");
            formErrorProvider.SetError(tbLicensePlate, "");

            hasError = false;
        }

        private bool ValidateForm()
        {
            clearErrors();
            tbTruckID_Validating(this, new CancelEventArgs());
            tbLoadPrefix_Validating(this, new CancelEventArgs());
            tbTareWeight_Validating(this, new CancelEventArgs());
            tbRFIDTag_Validating(this, new CancelEventArgs());
            tbPhone_Validating(this, new CancelEventArgs());
            tbLicensePlate_Validating(this, new CancelEventArgs());
            return !hasError;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork()) {
                    TruckEntity existingObj = null;
                    if (truckObj == null)
                    {
                        existingObj = new TruckEntity();
                    }
                    else
                    {
                        existingObj = uow.TruckRepository.GetById(truckObj.Id);
                    }
                    existingObj.Name = tbTruckID.Text.Trim();
                    existingObj.LoadPrefix = tbLoadPrefix.Text.Trim();
                    existingObj.TareWeight = decimal.Parse(tbTareWeight.Text);
                    existingObj.RFIDTagId = tbRFIDTag.Text.Trim();
                    existingObj.LicensePlate = tbLicensePlate.Text.Trim();
                    existingObj.IsSemi = cbxIsSemi.Checked;
                    existingObj.OwnerPhone = tbPhone.Text.Trim();
                    existingObj.OwnerName = tbOwner.Text.Trim();
                    uow.TruckRepository.Save(existingObj);
                    uow.SaveChanges();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void tbTruckID_Validating(object sender, CancelEventArgs e)
        {
            string id = (truckObj != null) ? truckObj.Id : "";
            if (String.IsNullOrWhiteSpace(tbTruckID.Text))
            {
                hasError = true;
                formErrorProvider.SetError(tbTruckID, "Truck ID is required.");
            }
            else
            {
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    if (!uow.TruckRepository.CanSaveTruckId(id, tbTruckID.Text.Trim()))
                    {
                        hasError = true;
                        formErrorProvider.SetError(tbTruckID, "Truck ID is in use by another truck.");
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tbLoadPrefix_Validating(object sender, CancelEventArgs e)
        {
            string id = (truckObj != null) ? truckObj.Id : "";

            if (String.IsNullOrWhiteSpace(tbLoadPrefix.Text))
            {
                hasError = true;
                formErrorProvider.SetError(tbLoadPrefix, "Load prefix is required.");
            }
            else
            {
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    if (!uow.TruckRepository.CanSaveTruckPrefix(id, tbLoadPrefix.Text.Trim()))
                    {
                        hasError = true;
                        formErrorProvider.SetError(tbLoadPrefix, "Load prefix is in use by another truck.");
                    }
                }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }      

        private void tbLicensePlate_Validating(object sender, CancelEventArgs e)
        {
            string id = (truckObj != null) ? truckObj.Id : "";

            if (String.IsNullOrWhiteSpace(tbLicensePlate.Text))
            {
                hasError = true;
                formErrorProvider.SetError(tbLicensePlate, "License plate is required.");
            }
            else
            {
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    if (!uow.TruckRepository.CanSaveLicensePlate(id, tbLoadPrefix.Text.Trim()))
                    {
                        hasError = true;
                        formErrorProvider.SetError(tbLicensePlate, "License plate is in use by another truck.");
                    }
                }
            }
        }

        private void tbOwner_Validating(object sender, CancelEventArgs e)
        {

        }

        private void tbPhone_Validating(object sender, CancelEventArgs e)
        {
            System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(@"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}");
            if (!string.IsNullOrEmpty(tbPhone.Text) && !regEx.IsMatch(tbPhone.Text))
            {
                hasError = true;
                formErrorProvider.SetError(tbPhone, "Invalid phone number.");
            }
        }

        private void tbRFIDTag_Validating(object sender, CancelEventArgs e)
        {
            string id = (truckObj != null) ? truckObj.Id : "";

            if (String.IsNullOrWhiteSpace(tbRFIDTag.Text) && !cbxIsSemi.Checked)
            {
                hasError = true;
                formErrorProvider.SetError(tbRFIDTag, "RFID tag is required for gin trucks.");
            }
            else if (!String.IsNullOrWhiteSpace(tbRFIDTag.Text))
            {
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    if (!uow.TruckRepository.CanSaveRFIDTag(id, tbRFIDTag.Text.Trim()))
                    {
                        hasError = true;
                        formErrorProvider.SetError(tbRFIDTag, "RFID tag is in use by another truck.");
                    }
                }
            }
        }

        private void tbTareWeight_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(tbTareWeight.Text))
            {
                hasError = true;
                formErrorProvider.SetError(tbTareWeight, "Tare Weight is required.");
            }
            else if (!ValidationHelper.ValidDecimal(tbTareWeight.Text))
            {
                hasError = true;
                formErrorProvider.SetError(tbTareWeight, "Invalid decimal number.");
            }
        }
    }
}
