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
    public partial class FarmSelector : UserControl
    {
        private ErrorProvider _errorProvider = null;
        public event EventHandler SelectionChanged = null;
        private string _selectedClientId = string.Empty;        
        private bool _allowAdd = false;
        private bool _forceAddMode = false;

        public string SelectedFarmId
        {
            get {
                if (SelectedItem != null)
                {
                    return SelectedItem.Id;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string SelectedFarmName
        {
            get
            {
                if (SelectedItem != null)
                {
                    return SelectedItem.Name;
                }
                else
                {
                    return string.Empty;
                }
            }
        }


        public string ExistingOrNewFarmName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(SelectedFarmId)) return SelectedItem.Name;
                else return tbNewFarm.Text.Trim();
            }
        }

        public FarmEntity SelectedItem
        {
            get
            {
                if (cboFarm.SelectedIndex > 1)
                {                    
                    return (FarmEntity)cboFarm.SelectedItem;
                }
                else
                {
                    return null;
                }
            }
        }

        public FarmSelector()
        {
            InitializeComponent();

            lblNewFarm.Width = 100;
            lblFarmSelect.Width = 100;
        }

        public async Task Initialize(ErrorProvider errorProvider, bool allowAdd, bool forceAddMode, string selectedClientId, string selectedFarmId)
        {

            await Task.Run(() => { });

            _errorProvider = errorProvider;
            cboFarm.Enabled = false;
            _selectedClientId = selectedClientId;            

            _allowAdd = allowAdd;
            _forceAddMode = forceAddMode;

            tbNewFarm.Visible = false;
            lblNewFarm.Visible = false;

            //initialize client drop down
            
            cboFarm.Items.Clear();
            cboFarm.Items.Add(new FarmEntity { Id = "-1", Name = "-- Select One --" });
            if (allowAdd)
            {
                cboFarm.Items.Add(new FarmEntity { Id = "0", Name = "-- Add New --" });
            }
            cboFarm.ValueMember = "Id";
            cboFarm.DisplayMember = "Name";

            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {

                if (!_forceAddMode)
                {
                    var client = uow.ClientRepository.GetById(selectedClientId, "Farms");                    
                    FarmEntity selectedItem = null;
                    foreach (var item in client.Farms)
                    {
                        var itemToAdd = new FarmEntity { Id = item.Id, Name = item.Name };
                        if (item.Id == selectedFarmId)
                        {
                            selectedItem = itemToAdd;
                        }
                        cboFarm.Items.Add(itemToAdd);
                    }

                    if (selectedItem != null)
                    {
                        cboFarm.SelectedItem = selectedItem;
                    }
                    else
                    {
                        cboFarm.SelectedIndex = 0;
                    }

                    cboFarm.Visible = true;
                    lblFarmSelect.Visible = true;
                    lblNewFarm.Visible = false;
                    tbNewFarm.Visible = false;
                }
                else
                {
                    cboFarm.SelectedIndex = 1;
                    cboFarm.Visible = !_forceAddMode;
                    lblFarmSelect.Visible = !_forceAddMode;
                    lblNewFarm.Visible = _forceAddMode;
                    tbNewFarm.Visible = _forceAddMode;
                }
                cboFarm.Enabled = true;
            }
        }

        public FarmEntity GetOrCreateFarmEntity(ClientEntity clientDoc)
        {
            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                if (this.IsNew)
                {
                    var farm = uow.FarmRepository.EnsureFarmCreated(clientDoc, tbNewFarm.Text, InputSource.GIN);
                    uow.SaveChanges();
                    return farm;
                }
                else if (ExistingSelected)
                {
                    return uow.FarmRepository.GetById(SelectedFarmId, "Client", "Fields");
                }
                else
                {
                    return null;
                }
            }
        }

        public ErrorProvider FormErrorProvider
        {
            get
            {
                return _errorProvider;
            }
            set
            {
                _errorProvider = value;
            }
        }

        public bool ValidateForm()
        {
            ValidateFarmCombo();
            ValidateFarmTextBox();

            return HasError;
        }

        public bool HasError
        {
            get;
            set;
        }

        public bool IsNew
        {
            get
            {
                FarmEntity selected = (FarmEntity)cboFarm.SelectedItem;
                return (selected != null && selected.Id == "0");
            }
        }

        public bool ExistingSelected
        {
            get
            {
                return (SelectedItem != null && SelectedItem.Id != "0");
            }
        }

        public int LabelColumnWidth
        {
            get
            {
                return lblFarmSelect.Width;
            }
            set
            {
                lblFarmSelect.Width = value;
                lblNewFarm.Width = value;
            }
        }

        public int InputColumnWidth
        {
            get
            {
                return tbNewFarm.Width;
            }
            set
            {
                tbNewFarm.Width = value;
                cboFarm.Width = value;
            }
        }

        private void cboFarm_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbNewFarm.Visible = IsNew;
            lblNewFarm.Visible = IsNew;

            if (SelectionChanged != null)
            {
                SelectionChanged(sender, e);
            }
        }

        public void ClearErrors()
        {
            HasError = false;
            if (_errorProvider != null)
            {
                _errorProvider.SetError(cboFarm, "");
                _errorProvider.SetError(tbNewFarm, "");
            }
        }

        private void ValidateFarmCombo()
        {
            if (cboFarm.SelectedIndex == 0)
            {
                _errorProvider.SetError(cboFarm, "Please select one.");
                HasError = true;
            }
        }

        private void ValidateFarmTextBox()
        {
            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                if (IsNew || string.IsNullOrWhiteSpace(_selectedClientId))
                {
                    if (string.IsNullOrWhiteSpace(tbNewFarm.Text))
                    {
                        _errorProvider.SetError(tbNewFarm, "Please enter new farm name.");
                        HasError = true;
                    }
                    else
                    {
                        bool canSave = uow.FarmRepository.CanSaveFarm(_selectedClientId, "", tbNewFarm.Text.Trim(), false);

                        if (!canSave)
                        {
                            _errorProvider.SetError(tbNewFarm, "Farm already exists.");
                            HasError = true;
                        }
                    }
                }
            }
        }

        private void cboFarm_Validating(object sender, CancelEventArgs e)
        {
            ValidateFarmCombo();
        }

        private void tbNewFarm_Validating(object sender, CancelEventArgs e)
        {
            ValidateFarmTextBox();
        } 
    }
}
