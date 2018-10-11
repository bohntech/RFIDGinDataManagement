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
    public partial class AddEditDriver : Form
    {
        #region private variables
        bool hasError = false;
        DriverEntity driverObj = null;
        #endregion

        #region constructor
        public AddEditDriver()
        {
            InitializeComponent();
            this.AcceptButton = btnSave;
        }
        #endregion

        #region public methods
        public DialogResult ShowAdd()
        {
            clearErrors();
            tbFirstname.Text = "";
            tbLastname.Text = "";
            this.Text = "Add Driver";
            return this.ShowDialog();
        }

        public DialogResult ShowEdit(DriverEntity doc)
        {
            driverObj = doc;
            clearErrors();
            tbFirstname.Text = doc.Firstname;
            tbLastname.Text = doc.Lastname;
            this.Text = "Update Driver";
            return this.ShowDialog();
        }
        #endregion

        #region private methods
        private void clearErrors()
        {
            formErrorProvider.SetError(tbFirstname, "");
            formErrorProvider.SetError(tbLastname, "");
            hasError = false;
        }

        private bool ValidateForm()
        {
            clearErrors();
            tbFirstname_Validating(this, new CancelEventArgs());
            tbLastname_Validating(this, new CancelEventArgs());
            return !hasError;
        }
        #endregion

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                DriverEntity existingDriver = null;
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork()) {
                    if (driverObj == null)
                    {
                        existingDriver = new DriverEntity();
                    }
                    else
                    {
                        existingDriver = uow.DriverRepository.GetById(driverObj.Id);
                    }
                    existingDriver.Firstname = tbFirstname.Text.Trim();
                    existingDriver.Lastname = tbLastname.Text.Trim();
                    existingDriver.Name = string.Format("{0} {1}", existingDriver.Firstname, existingDriver.Lastname);
                    uow.DriverRepository.Save(existingDriver);
                    uow.SaveChanges();
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }       

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }        

        private void tbFirstname_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tbFirstname.Text))
            {
                hasError = true;
                formErrorProvider.SetError(tbFirstname, "First name is required.");
            }
        }

        private void tbLastname_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tbLastname.Text))
            {
                hasError = true;
                formErrorProvider.SetError(tbLastname, "Last name is required.");
            }
        }
    }
}
