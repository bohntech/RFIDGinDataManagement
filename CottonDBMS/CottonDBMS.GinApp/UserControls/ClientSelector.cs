//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CottonDBMS.Cloud;
using CottonDBMS.DataModels;

namespace CottonDBMS.GinApp.UserControls
{
    public partial class ClientSelector : UserControl
    {
        private ErrorProvider _errorProvider = null;

        public event EventHandler SelectionChanged = null;

        private bool _allowAdd = true;
        private bool _forceAddMode = false;

        public ClientSelector()
        {
            InitializeComponent();

            lblNewClient.Width = 100;
            lblClientSelect.Width = 100;
        }

        public void Initialize(ErrorProvider errorProvider, bool allowAdd, bool forceAddMode, string selectedClientId)
        {
                cboClient.Enabled = false;

                _errorProvider = errorProvider;
                _allowAdd = allowAdd;
                _forceAddMode = forceAddMode;

                //initialize client drop down            
                cboClient.Items.Clear();
                cboClient.Items.Add(new { Id = -1, Name = "-- Select One --" });

                if (_allowAdd) cboClient.Items.Add(new { Id = 0, Name = "-- Add New --" });

                cboClient.ValueMember = "Id";
                cboClient.DisplayMember = "Name";

                if (!_forceAddMode)
                {
                    using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                    {
                        var clients = uow.ClientRepository.GetAll();
                        ClientEntity selectedItem = null;
                        foreach (var item in clients.OrderBy(c => c.Name.ToLower()))
                        {
                            if (item.Id == selectedClientId) selectedItem = item;
                            cboClient.Items.Add(item);
                        }

                        if (selectedItem != null) cboClient.SelectedItem = selectedItem;
                        else cboClient.SelectedIndex = 0;
                    }
                }
                else
                {
                    cboClient.SelectedIndex = 1;
                    lblClientSelect.Visible = false;
                    cboClient.Visible = false;
                    lblNewClient.Visible = true;
                    tbNewClient.Visible = true;
                }
                cboClient.Enabled = true;
         
        }
    
        public void ClearErrors()
        {
            HasError = false;
            _errorProvider.SetError(cboClient, "");
            _errorProvider.SetError(tbNewClient, "");
        }        

        public ClientEntity GetOrCreateClientEntity()
        {
            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                if (this.IsNew)
                {
                    return uow.ClientRepository.EnsureClientCreated(tbNewClient.Text, InputSource.GIN);
                }
                else if (ExistingSelected)
                {
                    return uow.ClientRepository.GetById(SelectedClientId, "Farms.Fields");
                }
                else
                {
                    return null;
                }
            }
        }

        public bool ValidateForm()
        {            
            ValidateClientCombo();
            ValidateClientTextBox();
            return HasError;
        }

        public string SelectedClientId
        {
            get
            {
                if (cboClient.SelectedIndex > 1)
                {
                    ClientEntity item = (ClientEntity)cboClient.SelectedItem;
                    return item.Id;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string SelectedClientName
        {
            get
            {
                if (cboClient.SelectedIndex > 1)
                {
                    ClientEntity item = (ClientEntity)cboClient.SelectedItem;
                    return item.Name;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string ExistingOrNewClientName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(SelectedClientName)) return SelectedClientName;
                else return tbNewClient.Text.Trim();
            }
        }

        /*public string NewClientText
        {
            get
            {
                return tbNewClient.Text.Trim();
            }
        }*/

        public bool HasError
        {
            get;
            set;
        }

        public bool IsNew
        {
            get
            {
                return cboClient.SelectedIndex == 1;
            }
        }

        public bool ExistingSelected
        {
            get
            {
                return cboClient.SelectedIndex > 1;
            }
        }

        public int LabelColumnWidth
        {
            get
            {
                return lblClientSelect.Width;
            }
            set
            {
                lblClientSelect.Width = value;
                lblNewClient.Width = value;
            }
        }

        public int InputColumnWidth
        {
            get
            {
                return tbNewClient.Width;
            }
            set
            {
                tbNewClient.Width = value;
                cboClient.Width = value;
            }
        }

        private void cboClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbNewClient.Visible = IsNew;
            lblNewClient.Visible = IsNew;

            if (SelectionChanged != null)
            {
                SelectionChanged(sender, e);
            }
        }

        private void ValidateClientCombo()
        {
            if (cboClient.SelectedIndex == 0)
            {
                _errorProvider.SetError(cboClient, "Please select one.");
                HasError = true;
            }
        }

        private void ValidateClientTextBox()
        {
            if (cboClient.SelectedIndex == 1)
            {
                if (string.IsNullOrWhiteSpace(tbNewClient.Text))
                {
                    _errorProvider.SetError(tbNewClient, "Please enter new client name.");
                    HasError = true;
                }
                else
                {
                    using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                    {
                        bool canSave = uow.ClientRepository.CanSaveClient(string.Empty, tbNewClient.Text.Trim());

                        if (!canSave)
                        {
                            _errorProvider.SetError(tbNewClient, "Client already exists.");
                            HasError = true;
                        }
                    }
                }
            }
        }

        private void cboClient_Validating(object sender, CancelEventArgs e)
        {
            ValidateClientCombo();
        }

        private void tbNewClient_Validating(object sender, CancelEventArgs e)
        {
            ValidateClientTextBox();
        }
    }
}
