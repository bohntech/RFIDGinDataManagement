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
            return this.ShowDialog();
        }

        public DialogResult ShowEdit(TruckEntity doc)
        {
            clearErrors();
            tbTruckID.Text = doc.Name;
            tbLoadPrefix.Text = doc.LoadPrefix;
            truckObj = doc;
            this.Text = "Update Truck";            
            return this.ShowDialog();
        }

        private void clearErrors()
        {
            formErrorProvider.SetError(tbTruckID, "");          
            hasError = false;
        }

        private bool ValidateForm()
        {
            clearErrors();
            tbTruckID_Validating(this, new CancelEventArgs());
            tbLoadPrefix_Validating(this, new CancelEventArgs());
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
    }
}
