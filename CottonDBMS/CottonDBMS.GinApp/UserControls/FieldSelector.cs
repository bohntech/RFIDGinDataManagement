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
using CottonDBMS.DataModels;

namespace CottonDBMS.GinApp.UserControls
{
    public partial class FieldSelector : UserControl
    {
        private ErrorProvider _errorProvider = null;

        public event EventHandler SelectionChanged = null;

        private FarmEntity selectedFarm = null;

        private bool _forceAddMode = false;
        private bool _allowAdd = false;
        
        public string SelectedFieldId
        {
            get
            {
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

        public string ExistingOrNewFieldName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(SelectedFieldId))
                {
                    return SelectedItem.Name;
                }
                else
                {
                    return tbNewField.Text.Trim();
                }
            }
        }

        public FieldEntity SelectedItem
        {
            get
            {
                if (cboField.SelectedIndex > 1)
                {
                    return (FieldEntity)cboField.SelectedItem;
                }
                else
                {
                    return null;
                }
            }
        }

        public FieldSelector()
        {
            InitializeComponent();

            lblNewField.Width = 100;
            lblFieldSelect.Width = 100;
        }

        public async Task Initialize(ErrorProvider errorProvider, bool allowAdd, bool forceAddMode, string selectedFarmId, string selectedFieldId)
        {
            _errorProvider = errorProvider;
            _forceAddMode = forceAddMode;
            
            
            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                selectedFarm = uow.FarmRepository.GetById(selectedFarmId, "Fields", "Client");
            }

                _allowAdd = allowAdd;
            cboField.Enabled = false;
            tbNewField.Visible = false;
            lblNewField.Visible = false;

            //initialize client drop down
            
                
                cboField.Items.Clear();
                cboField.Items.Add(new FieldEntity { Id = "-1", Name = "-- Select One --" });

                if (_allowAdd)
                {
                    cboField.Items.Add(new FieldEntity { Id = "0", Name = "-- Add New --" });
                }

                cboField.ValueMember = "Id";
                cboField.DisplayMember = "Name";

            if (!_forceAddMode)
            {
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {                    
                    FieldEntity selectedItem = null;
                    foreach (var item in selectedFarm.Fields.OrderBy(c => c.Name))
                    {
                        var itemToAdd = new FieldEntity { Id = item.Id, Name = item.Name };
                        if (item.Id == selectedFieldId)
                        {
                            selectedItem = itemToAdd;
                        }
                        cboField.Items.Add(itemToAdd);
                    }

                    if (selectedItem != null)
                    {
                        cboField.SelectedItem = selectedItem;
                    }
                    else
                    {
                        cboField.SelectedIndex = 0;
                    }

                    cboField.Visible = true;
                    lblFieldSelect.Visible = true;
                    lblNewField.Visible = false;
                    tbNewField.Visible = false;
                }
            }
            else
            {
                cboField.SelectedIndex = 1;
                cboField.Visible = !_forceAddMode;
                lblFieldSelect.Visible = !_forceAddMode;
                lblNewField.Visible = _forceAddMode;
                tbNewField.Visible = _forceAddMode;
            }
            cboField.Enabled = true;
        }        

        public FieldEntity GetOrCreateFieldEntity(FarmEntity farmDoc)
        {
            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                if (this.IsNew)
                {
                    var field = uow.FieldRepository.EnsureFieldCreated(farmDoc, tbNewField.Text, InputSource.GIN);
                    uow.SaveChanges();
                    return field;
                }
                else if (ExistingSelected)
                {
                    return uow.FieldRepository.GetById(SelectedFieldId);
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

        public void ClearErrors()
        {
            HasError = false;

            if (_errorProvider != null)
            {
                _errorProvider.SetError(cboField, "");
                _errorProvider.SetError(tbNewField, "");
            }
        }

        public bool ValidateForm()
        {
            ValidateCombo();
            ValidateTextBox();
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
                FieldEntity selected = (FieldEntity)cboField.SelectedItem;
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
                return lblFieldSelect.Width;
            }
            set
            {
                lblFieldSelect.Width = value;
                lblNewField.Width = value;
            }
        }

        public int InputColumnWidth
        {
            get
            {
                return tbNewField.Width;
            }
            set
            {
                tbNewField.Width = value;
                cboField.Width = value;
            }
        }

      

        private void ValidateCombo()
        {
            if (cboField.SelectedIndex == 0)
            {
                _errorProvider.SetError(cboField, "Please select one.");
                HasError = true;
            }
        }

        private void ValidateTextBox()
        {
            string _selectedClientId = (selectedFarm != null) ? selectedFarm.ClientId : "";
            string _selectedFarmId = (selectedFarm != null) ? selectedFarm.Id : "";

            if (IsNew || string.IsNullOrWhiteSpace(_selectedClientId) || string.IsNullOrWhiteSpace(_selectedFarmId))
            {
                if (string.IsNullOrWhiteSpace(tbNewField.Text))
                {
                    _errorProvider.SetError(tbNewField, "Please enter new field name.");
                    HasError = true;
                }
                else
                {
                    using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                    {
                        bool canSave = uow.FieldRepository.CanSaveField(_selectedClientId, _selectedFarmId, SelectedFieldId, tbNewField.Text.Trim(), false);

                        if (!canSave)
                        {
                            _errorProvider.SetError(tbNewField, "Field already exists.");
                            HasError = true;
                        }
                    }                    
                }
            }
        }      
        
        private void cboField_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbNewField.Visible = IsNew;
            lblNewField.Visible = IsNew;

            if (SelectionChanged != null)
            {
                SelectionChanged(sender, e);
            }
        }

        private void cboField_Validating(object sender, CancelEventArgs e)
        {
            ValidateCombo();
        }

        private void tbNewField_Validating(object sender, CancelEventArgs e)
        {
            ValidateTextBox();
        }
    }
}
