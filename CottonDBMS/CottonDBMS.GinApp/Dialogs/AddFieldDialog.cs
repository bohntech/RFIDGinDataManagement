using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
//Licensed under MIT License see LICENSE.TXT in project root folder
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CottonDBMS.DataModels;
using CottonDBMS.GinApp.Classes;

namespace CottonDBMS.GinApp.Dialogs
{
    public partial class AddFieldDialog : Form
    {
        bool hasError = false;

        FieldEntity dataObj = null;

        bool initializing = true;

        public AddFieldDialog()
        {
            InitializeComponent();

            this.AcceptButton = btnSave;
        }

        public void LoadForm()
        {
            //refresh field document to ensure latest updates are loaded
            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                if (dataObj != null)
                {
                    dataObj = uow.FieldRepository.GetById(dataObj.Id, "Farm.Client");
                }

                tbField.Text = "";
                clientSelector1.Initialize(formErrorProvider, true, false, (dataObj != null) ? dataObj.Farm.Client.Id : string.Empty);
                farmSelector1.FormErrorProvider = formErrorProvider;
                if (dataObj != null)
                {
                    tbField.Text = dataObj.Name;
                }
                clearErrors();
            }
        }   

        public DialogResult ShowForm(FieldEntity field)
        {
            if (field == null)
            {
                LoadForm();
                this.Text = "Add Field";
                return this.ShowDialog();
            }
            else
            {
                dataObj = field;
                initializing = true;
                LoadForm();
                this.Text = "Update Field";
                return this.ShowDialog();
            }
        }

        

        private void clearErrors()
        {
            clientSelector1.ClearErrors();
            farmSelector1.ClearErrors();
            formErrorProvider.SetError(tbField, "");
            hasError = false;
        }

        private bool ValidateForm()
        {
            clearErrors();

            clientSelector1.ValidateForm();
            farmSelector1.ValidateForm();

            if (clientSelector1.HasError) hasError = true;
            if (farmSelector1.HasError) hasError = true;

            ValidateField();
            return !hasError;
        }

        private void ValidateField()
        {
            try
            {
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    if (string.IsNullOrEmpty(tbField.Text))
                    {
                        formErrorProvider.SetError(tbField, "Field is required.");
                        hasError = true;
                    }
                    else
                    {
                        string fieldId = "";
                        if (dataObj != null) fieldId = dataObj.Id;
                        bool canSave = uow.FieldRepository.CanSaveField(clientSelector1.SelectedClientId, farmSelector1.SelectedFarmId, fieldId, tbField.Text.Trim(), false);
                        if (!canSave)
                        {
                            formErrorProvider.SetError(tbField, "Field already exists.");
                            hasError = true;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                formErrorProvider.SetError(tbField, "Couldn't validate field");
                hasError = true;
            }
        }

        private void tbField_Validating(object sender, CancelEventArgs e)
        {
            ValidateField();
        }       

        private void btnSave_Click(object sender, EventArgs e)
        {
            ValidateForm();
            if (!hasError)
            {
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    FieldEntity existingField = null;

                    if (dataObj == null)
                    {
                        existingField = new FieldEntity();
                    }
                    else
                    {
                        existingField = uow.FieldRepository.GetById(dataObj.Id);
                    }

                    var client = clientSelector1.GetOrCreateClientEntity();
                    var farm = farmSelector1.GetOrCreateFarmEntity(client);

                    existingField.FarmId = farm.Id;
                    existingField.Name = tbField.Text.Trim();

                    uow.FieldRepository.Save(existingField);
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

        private async void clientSelector1_SelectionChanged(object sender, EventArgs e)
        {
            if (clientSelector1.ExistingSelected || clientSelector1.IsNew)
            {
                farmSelector1.Visible = true;
                await farmSelector1.Initialize(formErrorProvider, true, clientSelector1.IsNew, clientSelector1.SelectedClientId, (dataObj != null) ? dataObj.FarmId : "");
            }
            else
            {
                farmSelector1.Visible = false;
            }
        }

        private void farmSelector1_SelectionChanged(object sender, EventArgs e)
        {

        }
    }
}