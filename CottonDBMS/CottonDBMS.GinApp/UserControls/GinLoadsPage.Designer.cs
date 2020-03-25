namespace CottonDBMS.GinApp.UserControls
{
    partial class GinLoadsPage
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
            this.dataGridLoads = new System.Windows.Forms.DataGridView();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.label23 = new System.Windows.Forms.Label();
            this.pnlBottomButtons = new System.Windows.Forms.Panel();
            this.btnDeleteSelected = new System.Windows.Forms.Button();
            this.btnEditSelected = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.loadFilterBar = new CottonDBMS.GinApp.UserControls.GinLoadFilterBar();
            this.Selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.GinLoadId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClientName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FarmName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FieldName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScaleBridgeID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GinTagLoadNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScaleBridgeLoadNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GrossWeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TruckID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.YardLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PickedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Variety = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TrailerNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocalCreatedTimestamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Updated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridLoads)).BeginInit();
            this.pnlBottomButtons.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridLoads
            // 
            this.dataGridLoads.AllowUserToAddRows = false;
            this.dataGridLoads.AllowUserToDeleteRows = false;
            this.dataGridLoads.AllowUserToResizeRows = false;
            this.dataGridLoads.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridLoads.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Selected,
            this.GinLoadId,
            this.ClientName,
            this.FarmName,
            this.FieldName,
            this.ScaleBridgeID,
            this.GinTagLoadNumber,
            this.ScaleBridgeLoadNumber,
            this.GrossWeight,
            this.TruckID,
            this.YardLocation,
            this.PickedBy,
            this.Variety,
            this.TrailerNumber,
            this.LocalCreatedTimestamp,
            this.Updated});
            this.dataGridLoads.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridLoads.Location = new System.Drawing.Point(10, 5);
            this.dataGridLoads.MultiSelect = false;
            this.dataGridLoads.Name = "dataGridLoads";
            this.dataGridLoads.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridLoads.Size = new System.Drawing.Size(1073, 469);
            this.dataGridLoads.TabIndex = 2;
            this.dataGridLoads.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGridLoads_Scroll);
            this.dataGridLoads.DoubleClick += new System.EventHandler(this.btnEditSelected_Click);
            // 
            // pnlTop
            // 
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1068, 38);
            this.pnlTop.TabIndex = 11;
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
            // pnlBottomButtons
            // 
            this.pnlBottomButtons.Controls.Add(this.btnDeleteSelected);
            this.pnlBottomButtons.Controls.Add(this.btnEditSelected);
            this.pnlBottomButtons.Controls.Add(this.btnAdd);
            this.pnlBottomButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottomButtons.Location = new System.Drawing.Point(0, 649);
            this.pnlBottomButtons.Name = "pnlBottomButtons";
            this.pnlBottomButtons.Size = new System.Drawing.Size(1093, 45);
            this.pnlBottomButtons.TabIndex = 10;
            // 
            // btnDeleteSelected
            // 
            this.btnDeleteSelected.Location = new System.Drawing.Point(246, 2);
            this.btnDeleteSelected.Name = "btnDeleteSelected";
            this.btnDeleteSelected.Size = new System.Drawing.Size(134, 35);
            this.btnDeleteSelected.TabIndex = 5;
            this.btnDeleteSelected.Text = "Delete Checked";
            this.btnDeleteSelected.UseVisualStyleBackColor = true;
            this.btnDeleteSelected.Click += new System.EventHandler(this.btnDeleteSelected_Click);
            // 
            // btnEditSelected
            // 
            this.btnEditSelected.Location = new System.Drawing.Point(128, 2);
            this.btnEditSelected.Name = "btnEditSelected";
            this.btnEditSelected.Size = new System.Drawing.Size(112, 35);
            this.btnEditSelected.TabIndex = 4;
            this.btnEditSelected.Text = "Edit Selected";
            this.btnEditSelected.UseVisualStyleBackColor = true;
            this.btnEditSelected.Click += new System.EventHandler(this.btnEditSelected_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(9, 2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(113, 35);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add Gin Load";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // pnlGrid
            // 
            this.pnlGrid.Controls.Add(this.dataGridLoads);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Location = new System.Drawing.Point(0, 165);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Padding = new System.Windows.Forms.Padding(10, 5, 10, 10);
            this.pnlGrid.Size = new System.Drawing.Size(1093, 484);
            this.pnlGrid.TabIndex = 12;
            // 
            // loadFilterBar
            // 
            this.loadFilterBar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.loadFilterBar.BackColor = System.Drawing.SystemColors.Window;
            this.loadFilterBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.loadFilterBar.Location = new System.Drawing.Point(0, 22);
            this.loadFilterBar.Margin = new System.Windows.Forms.Padding(0);
            this.loadFilterBar.Name = "loadFilterBar";
            this.loadFilterBar.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.loadFilterBar.ShowApplyButton = true;
            this.loadFilterBar.ShowSort1 = true;
            this.loadFilterBar.ShowSort2 = true;
            this.loadFilterBar.ShowSort3 = true;
            this.loadFilterBar.Size = new System.Drawing.Size(1093, 143);
            this.loadFilterBar.TabIndex = 17;
            this.loadFilterBar.ApplyClicked += new System.EventHandler(this.loadFilterBar_ApplyClicked);
            // 
            // Selected
            // 
            this.Selected.Frozen = true;
            this.Selected.HeaderText = "";
            this.Selected.MinimumWidth = 50;
            this.Selected.Name = "Selected";
            this.Selected.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Selected.Width = 50;
            // 
            // GinLoadId
            // 
            this.GinLoadId.DataPropertyName = "Id";
            this.GinLoadId.HeaderText = "GinLoadId";
            this.GinLoadId.Name = "GinLoadId";
            this.GinLoadId.ReadOnly = true;
            this.GinLoadId.Visible = false;
            // 
            // ClientName
            // 
            this.ClientName.DataPropertyName = "ClientName";
            this.ClientName.HeaderText = "Client";
            this.ClientName.MinimumWidth = 150;
            this.ClientName.Name = "ClientName";
            this.ClientName.ReadOnly = true;
            this.ClientName.Width = 150;
            // 
            // FarmName
            // 
            this.FarmName.DataPropertyName = "FarmName";
            this.FarmName.HeaderText = "Farm";
            this.FarmName.MinimumWidth = 150;
            this.FarmName.Name = "FarmName";
            this.FarmName.ReadOnly = true;
            this.FarmName.Width = 150;
            // 
            // FieldName
            // 
            this.FieldName.DataPropertyName = "FieldName";
            this.FieldName.HeaderText = "Field";
            this.FieldName.MinimumWidth = 150;
            this.FieldName.Name = "FieldName";
            this.FieldName.ReadOnly = true;
            this.FieldName.Width = 150;
            // 
            // ScaleBridgeID
            // 
            this.ScaleBridgeID.DataPropertyName = "ScaleBridgeId";
            this.ScaleBridgeID.HeaderText = "Bridge ID";
            this.ScaleBridgeID.MinimumWidth = 90;
            this.ScaleBridgeID.Name = "ScaleBridgeID";
            this.ScaleBridgeID.ReadOnly = true;
            this.ScaleBridgeID.Width = 90;
            // 
            // GinTagLoadNumber
            // 
            this.GinTagLoadNumber.DataPropertyName = "GinTagLoadNumber";
            this.GinTagLoadNumber.HeaderText = "Gin Tkt Load #";
            this.GinTagLoadNumber.MinimumWidth = 140;
            this.GinTagLoadNumber.Name = "GinTagLoadNumber";
            this.GinTagLoadNumber.ReadOnly = true;
            this.GinTagLoadNumber.Width = 150;
            // 
            // ScaleBridgeLoadNumber
            // 
            this.ScaleBridgeLoadNumber.DataPropertyName = "ScaleBridgeLoadNumber";
            this.ScaleBridgeLoadNumber.HeaderText = "Bridge Load #";
            this.ScaleBridgeLoadNumber.MinimumWidth = 140;
            this.ScaleBridgeLoadNumber.Name = "ScaleBridgeLoadNumber";
            this.ScaleBridgeLoadNumber.ReadOnly = true;
            this.ScaleBridgeLoadNumber.Width = 140;
            // 
            // GrossWeight
            // 
            this.GrossWeight.DataPropertyName = "GrossWeight";
            this.GrossWeight.HeaderText = "Gross Weight";
            this.GrossWeight.MinimumWidth = 100;
            this.GrossWeight.Name = "GrossWeight";
            this.GrossWeight.ReadOnly = true;
            this.GrossWeight.Width = 125;
            // 
            // TruckID
            // 
            this.TruckID.DataPropertyName = "TruckID";
            this.TruckID.HeaderText = "Truck ID";
            this.TruckID.MinimumWidth = 100;
            this.TruckID.Name = "TruckID";
            this.TruckID.ReadOnly = true;
            // 
            // YardLocation
            // 
            this.YardLocation.DataPropertyName = "YardLocation";
            this.YardLocation.HeaderText = "Yard Location/Row#";
            this.YardLocation.MinimumWidth = 100;
            this.YardLocation.Name = "YardLocation";
            this.YardLocation.ReadOnly = true;
            this.YardLocation.Width = 160;
            // 
            // PickedBy
            // 
            this.PickedBy.DataPropertyName = "PickedBy";
            this.PickedBy.HeaderText = "Picked By";
            this.PickedBy.Name = "PickedBy";
            this.PickedBy.ReadOnly = true;
            // 
            // Variety
            // 
            this.Variety.DataPropertyName = "Variety";
            this.Variety.HeaderText = "Variety";
            this.Variety.MinimumWidth = 50;
            this.Variety.Name = "Variety";
            this.Variety.ReadOnly = true;
            this.Variety.Width = 75;
            // 
            // TrailerNumber
            // 
            this.TrailerNumber.DataPropertyName = "TrailerNumber";
            this.TrailerNumber.HeaderText = "Trailer/Module #";
            this.TrailerNumber.Name = "TrailerNumber";
            this.TrailerNumber.ReadOnly = true;
            this.TrailerNumber.Width = 140;
            // 
            // LocalCreatedTimestamp
            // 
            this.LocalCreatedTimestamp.DataPropertyName = "LocalCreatedTimestamp";
            this.LocalCreatedTimestamp.HeaderText = "Created";
            this.LocalCreatedTimestamp.Name = "LocalCreatedTimestamp";
            this.LocalCreatedTimestamp.ReadOnly = true;
            this.LocalCreatedTimestamp.Width = 130;
            // 
            // Updated
            // 
            this.Updated.DataPropertyName = "LocalUpdatedTimestamp";
            this.Updated.HeaderText = "Updated";
            this.Updated.Name = "Updated";
            this.Updated.ReadOnly = true;
            // 
            // GinLoadsPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.loadFilterBar);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.pnlBottomButtons);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "GinLoadsPage";
            this.Size = new System.Drawing.Size(1093, 694);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridLoads)).EndInit();
            this.pnlBottomButtons.ResumeLayout(false);
            this.pnlGrid.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridLoads;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlBottomButtons;
        private System.Windows.Forms.Button btnDeleteSelected;
        private System.Windows.Forms.Button btnEditSelected;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label23;
        private GinLoadFilterBar loadFilterBar;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Selected;
        private System.Windows.Forms.DataGridViewTextBoxColumn GinLoadId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClientName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FarmName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FieldName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScaleBridgeID;
        private System.Windows.Forms.DataGridViewTextBoxColumn GinTagLoadNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScaleBridgeLoadNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn GrossWeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn TruckID;
        private System.Windows.Forms.DataGridViewTextBoxColumn YardLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn PickedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn Variety;
        private System.Windows.Forms.DataGridViewTextBoxColumn TrailerNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocalCreatedTimestamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn Updated;
    }
}
