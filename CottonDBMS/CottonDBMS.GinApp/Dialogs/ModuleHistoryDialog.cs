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
    public partial class ModuleHistoryDialog : Form
    {
        private ModuleEntity _module = null;

        public ModuleHistoryDialog()
        {
            InitializeComponent();
        }

        private void ModuleHistory_Load(object sender, EventArgs e)
        {           
                    
        }

        public DialogResult Show(ModuleEntity doc)
        {
            historyDataGrid.AutoGenerateColumns = false;

            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                //reload document to ensure we have the freshest history
                _module = uow.ModuleRepository.GetById(doc.Id, new string[] { "ModuleHistory", "Field.Farm.Client" });
                lblClient.Text = _module.Field.Farm.Client.Name;
                lblFarm.Text = _module.Field.Farm.Name;
                lblField.Text = _module.Field.Name;
                lblSerialNo.Text = _module.Name;
                lblLoad.Text = _module.LoadNumber;
                lblLocationValue.Text = string.Format("{0}, {1}", _module.Latitude, _module.Longitude);
                historyDataGrid.DataSource = _module.ModuleHistory.OrderBy(t => t.Created).ToList();
            }
            return this.ShowDialog();            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       
    }
}
