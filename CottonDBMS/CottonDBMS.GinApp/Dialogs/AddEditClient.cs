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
using CottonDBMS.Cloud;
using CottonDBMS.DataModels;
using CottonDBMS.GinApp.Classes;
using CottonDBMS.Interfaces;

namespace CottonDBMS.GinApp.Dialogs
{
    public partial class AddEditClient : Form
    {
        bool hasError = false;
        ClientEntity clientObj = null;

        public AddEditClient()
        {
            InitializeComponent();

            this.AcceptButton = btnSave;
        }

        public DialogResult ShowForm(ClientEntity doc)
        {
            if (doc == null)
            {
                clearErrors();
                tbClientName.Text = "";
                this.Text = "Add Client";
                return this.ShowDialog();
            }
            else
            {
                clearErrors();
                tbClientName.Text = doc.Name;
                clientObj = doc;
                this.Text = "Update Client";
                return this.ShowDialog();
            }
        }        

        private void clearErrors()
        {
            formErrorProvider.SetError(tbClientName, "");
            hasError = false;
        }

        private bool ValidateForm()
        {
            clearErrors();
            tbName_Validating(this, new CancelEventArgs());
            return !hasError;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                using (IUnitOfWork uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    this.Enabled = false;
                    if (clientObj == null)
                    {
                        clientObj = new ClientEntity();
                        clientObj.Created = DateTime.Now.ToUniversalTime();
                        clientObj.Farms = new List<FarmEntity>();
                    }
                    else
                    {
                        //if updating need to retreive original saved document because we need to preserve the child documents
                        clientObj = uow.ClientRepository.GetById(clientObj.Id);                       
                        clientObj.Updated = DateTime.Now.ToUniversalTime();
                    }
                    clientObj.Name = tbClientName.Text.Trim();

                    //TODO Save to clould
                    uow.ClientRepository.Save(clientObj);

                    //await CloudDataProvider.ClientDocumentRepository.

                    uow.SaveChanges();
                                        
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void tbName_Validating(object sender, CancelEventArgs e)
        {
            using (IUnitOfWork uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                if (String.IsNullOrWhiteSpace(tbClientName.Text))
                {
                    hasError = true;
                    formErrorProvider.SetError(tbClientName, "Client name is required.");
                }
                else if (!uow.ClientRepository.CanSaveClient((clientObj != null) ? clientObj.Id : "", tbClientName.Text.Trim()))
                {
                    hasError = true;
                    formErrorProvider.SetError(tbClientName, "Client name already exists.");
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
