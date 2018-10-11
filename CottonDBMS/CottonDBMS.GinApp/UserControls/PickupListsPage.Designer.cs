namespace CottonDBMS.GinApp.UserControls
{
    partial class PickupListsPage
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnDeleteSelected = new System.Windows.Forms.Button();
            this.btnEditSelected = new System.Windows.Forms.Button();
            this.btnAddModule = new System.Windows.Forms.Button();
            this.pnlBottomButtons = new System.Windows.Forms.Panel();
            this.label23 = new System.Windows.Forms.Label();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.gridView = new System.Windows.Forms.DataGridView();
            this.pickupListFilterBar = new CottonDBMS.GinApp.UserControls.PickupListFilterBar();
            this.Selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FieldId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ListName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClientName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FarmName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FieldName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TransmittedTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReceivedByTrucks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocalCreatedTimestamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalModules = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModulesInField = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalLoads = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LoadsRemaining = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Updated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlBottomButtons.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDeleteSelected
            // 
            this.btnDeleteSelected.Location = new System.Drawing.Point(246, 3);
            this.btnDeleteSelected.Name = "btnDeleteSelected";
            this.btnDeleteSelected.Size = new System.Drawing.Size(134, 35);
            this.btnDeleteSelected.TabIndex = 3;
            this.btnDeleteSelected.Text = "Delete Checked";
            this.btnDeleteSelected.UseVisualStyleBackColor = true;
            this.btnDeleteSelected.Click += new System.EventHandler(this.btnDeleteSelected_Click);
            // 
            // btnEditSelected
            // 
            this.btnEditSelected.Location = new System.Drawing.Point(128, 3);
            this.btnEditSelected.Name = "btnEditSelected";
            this.btnEditSelected.Size = new System.Drawing.Size(112, 35);
            this.btnEditSelected.TabIndex = 1;
            this.btnEditSelected.Text = "Edit Selected";
            this.btnEditSelected.UseVisualStyleBackColor = true;
            this.btnEditSelected.Click += new System.EventHandler(this.btnEditSelect_Click);
            // 
            // btnAddModule
            // 
            this.btnAddModule.Location = new System.Drawing.Point(9, 3);
            this.btnAddModule.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.btnAddModule.Name = "btnAddModule";
            this.btnAddModule.Size = new System.Drawing.Size(113, 35);
            this.btnAddModule.TabIndex = 0;
            this.btnAddModule.Text = "Add List";
            this.btnAddModule.UseVisualStyleBackColor = true;
            this.btnAddModule.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // pnlBottomButtons
            // 
            this.pnlBottomButtons.Controls.Add(this.btnDeleteSelected);
            this.pnlBottomButtons.Controls.Add(this.btnEditSelected);
            this.pnlBottomButtons.Controls.Add(this.btnAddModule);
            this.pnlBottomButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottomButtons.Location = new System.Drawing.Point(0, 577);
            this.pnlBottomButtons.Name = "pnlBottomButtons";
            this.pnlBottomButtons.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.pnlBottomButtons.Size = new System.Drawing.Size(1093, 45);
            this.pnlBottomButtons.TabIndex = 17;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Dock = System.Windows.Forms.DockStyle.Top;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(0, 0);
            this.label23.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.label23.Name = "label23";
            this.label23.Padding = new System.Windows.Forms.Padding(7, 7, 0, 0);
            this.label23.Size = new System.Drawing.Size(128, 27);
            this.label23.TabIndex = 0;
            this.label23.Text = "Search Filters";
            // 
            // pnlGrid
            // 
            this.pnlGrid.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlGrid.Controls.Add(this.gridView);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Location = new System.Drawing.Point(0, 124);
            this.pnlGrid.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Padding = new System.Windows.Forms.Padding(10);
            this.pnlGrid.Size = new System.Drawing.Size(1093, 453);
            this.pnlGrid.TabIndex = 27;
            // 
            // gridView
            // 
            this.gridView.AllowUserToAddRows = false;
            this.gridView.AllowUserToDeleteRows = false;
            this.gridView.AllowUserToResizeRows = false;
            this.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Selected,
            this.FieldId,
            this.Id,
            this.ListName,
            this.ClientName,
            this.FarmName,
            this.FieldName,
            this.TransmittedTo,
            this.ReceivedByTrucks,
            this.Status,
            this.LocalCreatedTimestamp,
            this.TotalModules,
            this.ModulesInField,
            this.TotalLoads,
            this.LoadsRemaining,
            this.Updated});
            this.gridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridView.Location = new System.Drawing.Point(10, 10);
            this.gridView.Margin = new System.Windows.Forms.Padding(0);
            this.gridView.MultiSelect = false;
            this.gridView.Name = "gridView";
            this.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridView.Size = new System.Drawing.Size(1073, 433);
            this.gridView.TabIndex = 8;
            this.gridView.Scroll += new System.Windows.Forms.ScrollEventHandler(this.gridView_Scroll);
            this.gridView.DoubleClick += new System.EventHandler(this.btnEditSelect_Click);
            // 
            // pickupListFilterBar
            // 
            this.pickupListFilterBar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pickupListFilterBar.BackColor = System.Drawing.SystemColors.Window;
            this.pickupListFilterBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pickupListFilterBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pickupListFilterBar.Location = new System.Drawing.Point(0, 27);
            this.pickupListFilterBar.Margin = new System.Windows.Forms.Padding(0);
            this.pickupListFilterBar.Name = "pickupListFilterBar";
            this.pickupListFilterBar.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.pickupListFilterBar.ShowApplyButton = true;
            this.pickupListFilterBar.ShowSort1 = true;
            this.pickupListFilterBar.Size = new System.Drawing.Size(1093, 97);
            this.pickupListFilterBar.TabIndex = 26;
            this.pickupListFilterBar.ApplyClicked += new System.EventHandler(this.filterBar_ApplyClicked);
            // 
            // Selected
            // 
            this.Selected.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Selected.Frozen = true;
            this.Selected.HeaderText = "";
            this.Selected.MinimumWidth = 50;
            this.Selected.Name = "Selected";
            this.Selected.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Selected.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Selected.Width = 50;
            // 
            // FieldId
            // 
            this.FieldId.HeaderText = "FieldId";
            this.FieldId.Name = "FieldId";
            this.FieldId.ReadOnly = true;
            this.FieldId.Visible = false;
            // 
            // Id
            // 
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Visible = false;
            // 
            // ListName
            // 
            this.ListName.DataPropertyName = "Name";
            this.ListName.HeaderText = "List Name";
            this.ListName.Name = "ListName";
            this.ListName.ReadOnly = true;
            // 
            // ClientName
            // 
            this.ClientName.DataPropertyName = "ClientName";
            this.ClientName.HeaderText = "Client";
            this.ClientName.Name = "ClientName";
            this.ClientName.ReadOnly = true;
            this.ClientName.Width = 96;
            // 
            // FarmName
            // 
            this.FarmName.DataPropertyName = "FarmName";
            this.FarmName.HeaderText = "Farm";
            this.FarmName.Name = "FarmName";
            this.FarmName.ReadOnly = true;
            this.FarmName.Width = 97;
            // 
            // FieldName
            // 
            this.FieldName.DataPropertyName = "FieldName";
            this.FieldName.HeaderText = "Field";
            this.FieldName.Name = "FieldName";
            this.FieldName.ReadOnly = true;
            this.FieldName.Width = 97;
            // 
            // TransmittedTo
            // 
            this.TransmittedTo.DataPropertyName = "AssignedTruckNames";
            this.TransmittedTo.HeaderText = "Assigned To";
            this.TransmittedTo.MinimumWidth = 150;
            this.TransmittedTo.Name = "TransmittedTo";
            this.TransmittedTo.ReadOnly = true;
            this.TransmittedTo.Width = 150;
            // 
            // ReceivedByTrucks
            // 
            this.ReceivedByTrucks.DataPropertyName = "DownloadedByTruckNames";
            this.ReceivedByTrucks.HeaderText = "Received By";
            this.ReceivedByTrucks.MinimumWidth = 150;
            this.ReceivedByTrucks.Name = "ReceivedByTrucks";
            this.ReceivedByTrucks.ReadOnly = true;
            this.ReceivedByTrucks.Width = 150;
            // 
            // Status
            // 
            this.Status.DataPropertyName = "StatusName";
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Width = 97;
            // 
            // LocalCreatedTimestamp
            // 
            this.LocalCreatedTimestamp.DataPropertyName = "LocaleCreatedTimestamp";
            this.LocalCreatedTimestamp.HeaderText = "Added";
            this.LocalCreatedTimestamp.Name = "LocalCreatedTimestamp";
            this.LocalCreatedTimestamp.ReadOnly = true;
            // 
            // TotalModules
            // 
            this.TotalModules.DataPropertyName = "OriginalModuleCount";
            this.TotalModules.HeaderText = "Total Modules";
            this.TotalModules.MinimumWidth = 150;
            this.TotalModules.Name = "TotalModules";
            this.TotalModules.ReadOnly = true;
            this.TotalModules.Width = 150;
            // 
            // ModulesInField
            // 
            this.ModulesInField.DataPropertyName = "SearchSetModulesRemaining";
            this.ModulesInField.HeaderText = "Remaining";
            this.ModulesInField.Name = "ModulesInField";
            this.ModulesInField.ReadOnly = true;
            // 
            // TotalLoads
            // 
            this.TotalLoads.DataPropertyName = "TotalLoads";
            this.TotalLoads.HeaderText = "Total Loads";
            this.TotalLoads.MinimumWidth = 150;
            this.TotalLoads.Name = "TotalLoads";
            this.TotalLoads.ReadOnly = true;
            this.TotalLoads.Width = 150;
            // 
            // LoadsRemaining
            // 
            this.LoadsRemaining.DataPropertyName = "LoadsRemaining";
            this.LoadsRemaining.HeaderText = "Loads Remaining";
            this.LoadsRemaining.MinimumWidth = 150;
            this.LoadsRemaining.Name = "LoadsRemaining";
            this.LoadsRemaining.ReadOnly = true;
            this.LoadsRemaining.Width = 150;
            // 
            // Updated
            // 
            this.Updated.DataPropertyName = "LocaleUpdatedTimestamp";
            this.Updated.HeaderText = "Updated";
            this.Updated.Name = "Updated";
            this.Updated.ReadOnly = true;
            // 
            // PickupListsPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pickupListFilterBar);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.pnlBottomButtons);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PickupListsPage";
            this.Size = new System.Drawing.Size(1093, 622);
            this.pnlBottomButtons.ResumeLayout(false);
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnDeleteSelected;
        private System.Windows.Forms.Button btnEditSelected;
        private System.Windows.Forms.Button btnAddModule;
        private System.Windows.Forms.Panel pnlBottomButtons;
        private System.Windows.Forms.Label label23;
        private PickupListFilterBar pickupListFilterBar;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.DataGridView gridView;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Selected;
        private System.Windows.Forms.DataGridViewTextBoxColumn FieldId;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn ListName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClientName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FarmName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FieldName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TransmittedTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReceivedByTrucks;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocalCreatedTimestamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalModules;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModulesInField;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalLoads;
        private System.Windows.Forms.DataGridViewTextBoxColumn LoadsRemaining;
        private System.Windows.Forms.DataGridViewTextBoxColumn Updated;
    }
}
