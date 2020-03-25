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
using CottonDBMS.Interfaces;

namespace CottonDBMS.GinApp.Dialogs
{
    public partial class AssignPBIsToNewModule : Form
    {
        private List<BaleEntity> bales = new List<BaleEntity>();
        private bool hasError = false;

        public AssignPBIsToNewModule(List<BaleEntity> _bales)
        {
            InitializeComponent();
            bales = _bales;
        }

        private void clearErrors()
        {
            formErrorProvider.SetError(tbSerialNumber, "");            
            hasError = false;
        }

        private bool ValidateForm()
        {
            clearErrors();
            tbSerialNumber_Validating(this, new CancelEventArgs());
            return !hasError;
        }

        private void tbSerialNumber_Validating(object sender, CancelEventArgs e)
        {
            using (Interfaces.IUnitOfWork uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                var module = uow.ModuleRepository.FindSingle(x => x.Name == tbSerialNumber.Text);
                if (module == null)
                {
                    hasError = true;
                    formErrorProvider.SetError(tbSerialNumber, "Serial number not found.");
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            clearErrors();
            if (ValidateForm())
            {
                using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    var module = uow.ModuleRepository.FindSingle(x => x.Name == tbSerialNumber.Text, "GinLoad");
                    foreach (var b in bales)
                    {
                        b.ModuleId = module.Id;
                        b.ModuleSerialNumber = module.Name;
                        b.GinLoadId = module.GinLoadId;
                        b.GinTicketLoadNumber = (module.GinLoad != null) ? module.GinLoad.GinTagLoadNumber : "";
                        uow.BalesRepository.QuickUpdate(b, true);
                    }
                    uow.SaveChanges();
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
