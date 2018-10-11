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
using CottonDBMS.GinApp.Classes;
using CottonDBMS.DataModels;

namespace CottonDBMS.GinApp.Dialogs
{
    public partial class AddFarm : Form
    {
        bool hasError = false;

        FarmEntity farmObj = null;

        public AddFarm()
        {
            InitializeComponent();

            this.AcceptButton = btnSave;
        }

        public async Task LoadForm()
        {
            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                this.Enabled = true;
                //initialize client drop down
                
                if (farmObj != null)
                {
                    farmObj = uow.FarmRepository.GetById(farmObj.Id);
                }

                clientSelector1.Initialize(formErrorProvider, true, false, (farmObj != null) ? farmObj.ClientId : string.Empty);

                if (farmObj != null)
                {
                    tbFarm.Text = farmObj.Name;
                }

            }
            clearErrors();
        }       

        public async Task<DialogResult> ShowForm(FarmEntity entity)
        {
            if (entity == null)
            {
                await LoadForm();
                this.Text = "Add Farm";
                return this.ShowDialog();
            }
            else
            {
                farmObj = entity;
                await LoadForm();
                //this.Text = "Update Farm";
                return this.ShowDialog();
            }
        }       

        private void clearErrors()
        {
            clientSelector1.ClearErrors();            
            formErrorProvider.SetError(tbFarm, "");
            hasError = false;
        }

        private bool ValidateForm()
        {
            clearErrors();
            clientSelector1.ValidateForm();

            hasError = clientSelector1.HasError;

            ValidateFarm();
            return !hasError;
        }

        private void ValidateFarm()
        {
            try
            {
                if (string.IsNullOrEmpty(tbFarm.Text))
                {
                    formErrorProvider.SetError(tbFarm, "Farm is required.");
                    hasError = true;
                }
                else
                {

                    string farmId = "";
                    if (farmObj != null) farmId = farmObj.Id;

                    using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                    {
                        bool canSave = uow.FarmRepository.CanSaveFarm(clientSelector1.SelectedClientId, farmId, tbFarm.Text.Trim(), false);

                        if (!canSave)
                        {
                            formErrorProvider.SetError(tbFarm, "Farm already exists.");
                            hasError = true;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                formErrorProvider.SetError(tbFarm, "Couldn't validate farm");
                hasError = true;
            }
        }

        private void tbFarm_Validating(object sender, CancelEventArgs e)
        {
            ValidateFarm();
        }

        private void AddFarm_Load(object sender, EventArgs e)
        {
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ValidateForm();
                if (!hasError)
                {
                    this.Enabled = false;

                    using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                    {
                        //ensure client exists
                        var client = clientSelector1.GetOrCreateClientEntity();

                        if (farmObj == null)
                        {
                            farmObj = new FarmEntity();
                        }
                        else
                        {
                            farmObj = uow.FarmRepository.GetById(farmObj.Id);
                        }
                        farmObj.ClientId = client.Id;
                        farmObj.Name = tbFarm.Text.Trim();
                        uow.FarmRepository.Save(farmObj);
                        uow.SaveChanges();
                    }
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch(Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void clientSelector1_SelectionChanged(object sender, EventArgs e)
        {
            /*if (clientSelector1.IsNew)
            {
                tbClient.Text = "";
                tbClient.Visible = true;
                lblNewClient.Visible = true;
            }
            else
            {
                tbClient.Visible = false;
                lblNewClient.Visible = false;
            }*/
        }
    }
}