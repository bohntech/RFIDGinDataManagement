namespace CottonDBMS.GinApp.UserControls
{
    partial class ModulesPage
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
            this.pnlBottomButtons = new System.Windows.Forms.Panel();
            this.btnChangeStatus = new System.Windows.Forms.Button();
            this.btnGenLoad = new System.Windows.Forms.Button();
            this.btnDeleteSelected = new System.Windows.Forms.Button();
            this.btnViewHistory = new System.Windows.Forms.Button();
            this.btnEditSelectedModule = new System.Windows.Forms.Button();
            this.btnAddModule = new System.Windows.Forms.Button();
            this.label23 = new System.Windows.Forms.Label();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.modulesGridView = new System.Windows.Forms.DataGridView();
            this.Selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClientName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FarmName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FieldName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SerialNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LoadNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ImportedLoadNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GinTagLoadNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BridgeLoadNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastBridgeId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TruckID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Driver = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Latitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Longitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocalCreatedTimestamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Updated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.moduleFilterBar = new CottonDBMS.GinApp.UserControls.ModuleFilterBar();
            this.pnlBottomButtons.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.modulesGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBottomButtons
            // 
            this.pnlBottomButtons.Controls.Add(this.btnChangeStatus);
            this.pnlBottomButtons.Controls.Add(this.btnGenLoad);
            this.pnlBottomButtons.Controls.Add(this.btnDeleteSelected);
            this.pnlBottomButtons.Controls.Add(this.btnViewHistory);
            this.pnlBottomButtons.Controls.Add(this.btnEditSelectedModule);
            this.pnlBottomButtons.Controls.Add(this.btnAddModule);
            this.pnlBottomButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottomButtons.Location = new System.Drawing.Point(0, 649);
            this.pnlBottomButtons.Name = "pnlBottomButtons";
            this.pnlBottomButtons.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.pnlBottomButtons.Size = new System.Drawing.Size(1093, 45);
            this.pnlBottomButtons.TabIndex = 12;
            // 
            // btnChangeStatus
            // 
            this.btnChangeStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangeStatus.Location = new System.Drawing.Point(791, 7);
            this.btnChangeStatus.Name = "btnChangeStatus";
            this.btnChangeStatus.Size = new System.Drawing.Size(178, 35);
            this.btnChangeStatus.TabIndex = 5;
            this.btnChangeStatus.Text = "Update all checked";
            this.btnChangeStatus.UseVisualStyleBackColor = true;
            this.btnChangeStatus.Click += new System.EventHandler(this.btnChangeStatus_Click);
            // 
            // btnGenLoad
            // 
            this.btnGenLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenLoad.Location = new System.Drawing.Point(607, 7);
            this.btnGenLoad.Name = "btnGenLoad";
            this.btnGenLoad.Size = new System.Drawing.Size(178, 35);
            this.btnGenLoad.TabIndex = 4;
            this.btnGenLoad.Text = "Assign load to checked";
            this.btnGenLoad.UseVisualStyleBackColor = true;
            this.btnGenLoad.Click += new System.EventHandler(this.btnGenLoad_Click);
            // 
            // btnDeleteSelected
            // 
            this.btnDeleteSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteSelected.Location = new System.Drawing.Point(467, 7);
            this.btnDeleteSelected.Name = "btnDeleteSelected";
            this.btnDeleteSelected.Size = new System.Drawing.Size(134, 35);
            this.btnDeleteSelected.TabIndex = 3;
            this.btnDeleteSelected.Text = "Delete Checked";
            this.btnDeleteSelected.UseVisualStyleBackColor = true;
            this.btnDeleteSelected.Click += new System.EventHandler(this.btnDeleteSelected_Click);
            // 
            // btnViewHistory
            // 
            this.btnViewHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewHistory.Location = new System.Drawing.Point(242, 7);
            this.btnViewHistory.Name = "btnViewHistory";
            this.btnViewHistory.Size = new System.Drawing.Size(219, 35);
            this.btnViewHistory.TabIndex = 2;
            this.btnViewHistory.Text = "View Selected Module History";
            this.btnViewHistory.UseVisualStyleBackColor = true;
            this.btnViewHistory.Click += new System.EventHandler(this.btnViewHistory_Click);
            // 
            // btnEditSelectedModule
            // 
            this.btnEditSelectedModule.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditSelectedModule.Location = new System.Drawing.Point(124, 7);
            this.btnEditSelectedModule.Name = "btnEditSelectedModule";
            this.btnEditSelectedModule.Size = new System.Drawing.Size(112, 35);
            this.btnEditSelectedModule.TabIndex = 1;
            this.btnEditSelectedModule.Text = "Edit Selected";
            this.btnEditSelectedModule.UseVisualStyleBackColor = true;
            this.btnEditSelectedModule.Click += new System.EventHandler(this.btnEditSelectedModule_Click);
            // 
            // btnAddModule
            // 
            this.btnAddModule.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddModule.Location = new System.Drawing.Point(5, 7);
            this.btnAddModule.Name = "btnAddModule";
            this.btnAddModule.Size = new System.Drawing.Size(113, 35);
            this.btnAddModule.TabIndex = 0;
            this.btnAddModule.Text = "Add Module";
            this.btnAddModule.UseVisualStyleBackColor = true;
            this.btnAddModule.Click += new System.EventHandler(this.btnAddModule_Click);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Dock = System.Windows.Forms.DockStyle.Top;
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(0, 0);
            this.label23.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.label23.Name = "label23";
            this.label23.Padding = new System.Windows.Forms.Padding(2, 2, 3, 0);
            this.label23.Size = new System.Drawing.Size(126, 22);
            this.label23.TabIndex = 16;
            this.label23.Text = "Search Filters";
            // 
            // pnlGrid
            // 
            this.pnlGrid.AutoSize = true;
            this.pnlGrid.Controls.Add(this.modulesGridView);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Location = new System.Drawing.Point(0, 165);
            this.pnlGrid.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Padding = new System.Windows.Forms.Padding(5);
            this.pnlGrid.Size = new System.Drawing.Size(1093, 484);
            this.pnlGrid.TabIndex = 18;
            // 
            // modulesGridView
            // 
            this.modulesGridView.AllowUserToAddRows = false;
            this.modulesGridView.AllowUserToDeleteRows = false;
            this.modulesGridView.AllowUserToResizeRows = false;
            this.modulesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.modulesGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Selected,
            this.Id,
            this.ClientName,
            this.FarmName,
            this.FieldName,
            this.SerialNumber,
            this.LoadNumber,
            this.ImportedLoadNumber,
            this.GinTagLoadNumber,
            this.BridgeLoadNumber,
            this.LastBridgeId,
            this.TruckID,
            this.Driver,
            this.Latitude,
            this.Longitude,
            this.Status,
            this.LocalCreatedTimestamp,
            this.Updated});
            this.modulesGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modulesGridView.Location = new System.Drawing.Point(5, 5);
            this.modulesGridView.Margin = new System.Windows.Forms.Padding(0);
            this.modulesGridView.MultiSelect = false;
            this.modulesGridView.Name = "modulesGridView";
            this.modulesGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.modulesGridView.Size = new System.Drawing.Size(1083, 474);
            this.modulesGridView.TabIndex = 7;
            this.modulesGridView.Scroll += new System.Windows.Forms.ScrollEventHandler(this.modulesGridView_Scroll);
            this.modulesGridView.DoubleClick += new System.EventHandler(this.btnEditSelectedModule_Click);
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
            // Id
            // 
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Visible = false;
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
            this.FieldName.DataPropertyName = "Field";
            this.FieldName.HeaderText = "Field";
            this.FieldName.Name = "FieldName";
            this.FieldName.ReadOnly = true;
            this.FieldName.Width = 97;
            // 
            // SerialNumber
            // 
            this.SerialNumber.DataPropertyName = "Name";
            this.SerialNumber.HeaderText = "Serial #";
            this.SerialNumber.Name = "SerialNumber";
            this.SerialNumber.ReadOnly = true;
            this.SerialNumber.Width = 150;
            // 
            // LoadNumber
            // 
            this.LoadNumber.DataPropertyName = "LoadNumberString";
            this.LoadNumber.HeaderText = "Load #";
            this.LoadNumber.Name = "LoadNumber";
            this.LoadNumber.ReadOnly = true;
            this.LoadNumber.Width = 97;
            // 
            // ImportedLoadNumber
            // 
            this.ImportedLoadNumber.DataPropertyName = "ImportedLoadNumber";
            this.ImportedLoadNumber.HeaderText = "Imported Load #";
            this.ImportedLoadNumber.MinimumWidth = 150;
            this.ImportedLoadNumber.Name = "ImportedLoadNumber";
            this.ImportedLoadNumber.ReadOnly = true;
            this.ImportedLoadNumber.Width = 150;
            // 
            // GinTagLoadNumber
            // 
            this.GinTagLoadNumber.DataPropertyName = "GinTagLoadNumber";
            this.GinTagLoadNumber.HeaderText = "Gin Tkt Load #";
            this.GinTagLoadNumber.MinimumWidth = 150;
            this.GinTagLoadNumber.Name = "GinTagLoadNumber";
            this.GinTagLoadNumber.ReadOnly = true;
            this.GinTagLoadNumber.Width = 175;
            // 
            // BridgeLoadNumber
            // 
            this.BridgeLoadNumber.DataPropertyName = "BridgeLoadNumber";
            this.BridgeLoadNumber.HeaderText = "Bridge Load#";
            this.BridgeLoadNumber.MinimumWidth = 100;
            this.BridgeLoadNumber.Name = "BridgeLoadNumber";
            this.BridgeLoadNumber.Width = 125;
            // 
            // LastBridgeId
            // 
            this.LastBridgeId.DataPropertyName = "LastBridgeId";
            this.LastBridgeId.HeaderText = "Bridge Id";
            this.LastBridgeId.MinimumWidth = 75;
            this.LastBridgeId.Name = "LastBridgeId";
            this.LastBridgeId.ReadOnly = true;
            this.LastBridgeId.Width = 125;
            // 
            // TruckID
            // 
            this.TruckID.DataPropertyName = "TruckID";
            this.TruckID.HeaderText = "Truck";
            this.TruckID.Name = "TruckID";
            this.TruckID.ReadOnly = true;
            // 
            // Driver
            // 
            this.Driver.DataPropertyName = "Driver";
            this.Driver.HeaderText = "Driver";
            this.Driver.Name = "Driver";
            this.Driver.ReadOnly = true;
            // 
            // Latitude
            // 
            this.Latitude.DataPropertyName = "Latitude";
            this.Latitude.HeaderText = "Latitude";
            this.Latitude.Name = "Latitude";
            this.Latitude.ReadOnly = true;
            this.Latitude.Width = 150;
            // 
            // Longitude
            // 
            this.Longitude.DataPropertyName = "Longitude";
            this.Longitude.HeaderText = "Longitude";
            this.Longitude.Name = "Longitude";
            this.Longitude.ReadOnly = true;
            this.Longitude.Width = 150;
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
            this.LocalCreatedTimestamp.Width = 150;
            // 
            // Updated
            // 
            this.Updated.DataPropertyName = "LocaleUpdatedTimestamp";
            this.Updated.HeaderText = "Updated";
            this.Updated.Name = "Updated";
            this.Updated.ReadOnly = true;
            this.Updated.Width = 150;
            // 
            // moduleFilterBar
            // 
            this.moduleFilterBar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.moduleFilterBar.BackColor = System.Drawing.SystemColors.Window;
            this.moduleFilterBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.moduleFilterBar.Location = new System.Drawing.Point(0, 22);
            this.moduleFilterBar.Margin = new System.Windows.Forms.Padding(0);
            this.moduleFilterBar.Name = "moduleFilterBar";
            this.moduleFilterBar.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.moduleFilterBar.ShowApplyButton = true;
            this.moduleFilterBar.ShowLocationOptions = false;
            this.moduleFilterBar.ShowSort1 = true;
            this.moduleFilterBar.ShowSort2 = true;
            this.moduleFilterBar.ShowSort3 = true;
            this.moduleFilterBar.Size = new System.Drawing.Size(1093, 143);
            this.moduleFilterBar.TabIndex = 17;
            this.moduleFilterBar.ApplyClicked += new System.EventHandler(this.btnApplyModuleFilters_Click);
            // 
            // ModulesPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.moduleFilterBar);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.pnlBottomButtons);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ModulesPage";
            this.Size = new System.Drawing.Size(1093, 694);
            this.pnlBottomButtons.ResumeLayout(false);
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.modulesGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel pnlBottomButtons;
        private System.Windows.Forms.Button btnDeleteSelected;
        private System.Windows.Forms.Button btnViewHistory;
        private System.Windows.Forms.Button btnEditSelectedModule;
        private System.Windows.Forms.Button btnAddModule;
        private System.Windows.Forms.Label label23;
        private ModuleFilterBar moduleFilterBar;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.DataGridView modulesGridView;
        private System.Windows.Forms.Button btnGenLoad;
        private System.Windows.Forms.Button btnChangeStatus;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Selected;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClientName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FarmName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FieldName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SerialNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn LoadNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn ImportedLoadNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn GinTagLoadNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn BridgeLoadNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastBridgeId;
        private System.Windows.Forms.DataGridViewTextBoxColumn TruckID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Driver;
        private System.Windows.Forms.DataGridViewTextBoxColumn Latitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn Longitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocalCreatedTimestamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn Updated;
    }
}
