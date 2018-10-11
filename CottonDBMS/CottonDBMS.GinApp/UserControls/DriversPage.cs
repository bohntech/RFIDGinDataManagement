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
using CottonDBMS.GinApp.Dialogs;
using CottonDBMS.Interfaces;
using CottonDBMS.DataModels;
using CottonDBMS.GinApp.Classes;

namespace CottonDBMS.GinApp.UserControls
{
    public partial class DriversPage : UserControl
    {
        bool initialized = false;
        CheckBox checkbox = new CheckBox();

        public DriversPage()
        {
            InitializeComponent();
            checkbox.Size = new System.Drawing.Size(15, 15);
            checkbox.BackColor = Color.Transparent;

            // Reset properties
            checkbox.Padding = new Padding(0);
            checkbox.Margin = new Padding(0);
            checkbox.Text = "";

            // Add checkbox to datagrid cell
            dataGridDrivers.Controls.Add(checkbox);
            checkbox.CheckedChanged += Checkbox_CheckedChanged;
            DataGridViewHeaderCell header = dataGridDrivers.Columns[0].HeaderCell;
            checkbox.Location = new Point(
                header.ContentBounds.Left + (header.ContentBounds.Right - header.ContentBounds.Left + checkbox.Width) / 2 + 53,
                header.ContentBounds.Top + (header.ContentBounds.Bottom - header.ContentBounds.Top + checkbox.Size.Height) / 2
            );
        }

        private void Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridDrivers.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                chk.Value = checkbox.Checked;
            }

            if (dataGridDrivers.Rows.Count > 0)
                dataGridDrivers.CurrentCell = dataGridDrivers.Rows[0].Cells[2];
        }

        public void LoadData()
        {
            if (!initialized)
            {
                initialized = true;
                refresh();
            }
        }

        private void refresh()
        {
            BusyMessage.Show("Loading...", this.FindForm());

            using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                IEnumerable<DriverEntity> drivers = uow.DriverRepository.GetAll();
                var sorted = drivers.OrderBy(t => t.Firstname).ToList();
                dataGridDrivers.DataSource = null;
                dataGridDrivers.AutoGenerateColumns = false;
                dataGridDrivers.DataSource = sorted;
                dataGridDrivers.Columns[0].ReadOnly = false;
            }
            BusyMessage.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddEditDriver dialog = new AddEditDriver();
            if (dialog.ShowAdd() != DialogResult.Cancel)
            {
                refresh();
            }
        }

        private void btnEditSelected_Click(object sender, EventArgs e)
        {
            AddEditDriver dialog = new AddEditDriver();

            DataGridViewRow row = null;

            if (dataGridDrivers.SelectedRows.Count > 0)
            {
                row = dataGridDrivers.SelectedRows[0];
                DriverEntity doc = (DriverEntity)row.DataBoundItem;
                if (dialog.ShowEdit(doc) != DialogResult.Cancel)
                {
                    refresh();
                }
            }
            else
            {
                MessageBox.Show("Please select a row to edit.");
            }
        }

        private void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            List<DriverEntity> docsToDelete = new List<DriverEntity>();
            foreach (DataGridViewRow row in dataGridDrivers.Rows)
            {
                DriverEntity doc = (DriverEntity)row.DataBoundItem;
                if (Convert.ToBoolean(row.Cells[0].Value))
                {
                    docsToDelete.Add(doc);
                }
            }

            if (docsToDelete.Count() > 0)
            {
                if (MessageBox.Show("Are you sure you want to delete the " + docsToDelete.Count.ToString() + " selected driver(s)?", "Delete?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    BusyMessage.Show("Deleting...", this.FindForm());
                    using (var uow = UnitOfWorkFactory.CreateUnitOfWork())
                    {
                        uow.DriverRepository.BulkDelete(docsToDelete);
                        uow.SaveChanges();                     
                    }
                    BusyMessage.Close();
                    refresh();
                }
            }
            else
            {
                MessageBox.Show("No records were selected to delete.");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            refresh();
        }
    }
}
