namespace CottonDBMS.GinApp.UserControls
{
    partial class TrucksPage
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDeleteSelected = new System.Windows.Forms.Button();
            this.btnEditSelected = new System.Windows.Forms.Button();
            this.btnAddTruck = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.dataGridTrucks = new System.Windows.Forms.DataGridView();
            this.Selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TruckID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LoadPrefix = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnCreateInstallPackage = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridTrucks)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCreateInstallPackage);
            this.panel1.Controls.Add(this.btnDeleteSelected);
            this.panel1.Controls.Add(this.btnEditSelected);
            this.panel1.Controls.Add(this.btnAddTruck);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 520);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(946, 45);
            this.panel1.TabIndex = 0;
            // 
            // btnDeleteSelected
            // 
            this.btnDeleteSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteSelected.Location = new System.Drawing.Point(246, 3);
            this.btnDeleteSelected.Name = "btnDeleteSelected";
            this.btnDeleteSelected.Size = new System.Drawing.Size(134, 35);
            this.btnDeleteSelected.TabIndex = 5;
            this.btnDeleteSelected.Text = "Delete Checked";
            this.btnDeleteSelected.UseVisualStyleBackColor = true;
            this.btnDeleteSelected.Click += new System.EventHandler(this.btnDeleteSelected_Click);
            // 
            // btnEditSelected
            // 
            this.btnEditSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditSelected.Location = new System.Drawing.Point(128, 3);
            this.btnEditSelected.Name = "btnEditSelected";
            this.btnEditSelected.Size = new System.Drawing.Size(112, 35);
            this.btnEditSelected.TabIndex = 4;
            this.btnEditSelected.Text = "Edit Selected";
            this.btnEditSelected.UseVisualStyleBackColor = true;
            this.btnEditSelected.Click += new System.EventHandler(this.btnEditSelected_Click);
            // 
            // btnAddTruck
            // 
            this.btnAddTruck.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddTruck.Location = new System.Drawing.Point(9, 3);
            this.btnAddTruck.Name = "btnAddTruck";
            this.btnAddTruck.Size = new System.Drawing.Size(113, 35);
            this.btnAddTruck.TabIndex = 1;
            this.btnAddTruck.Text = "Add Truck";
            this.btnAddTruck.UseVisualStyleBackColor = true;
            this.btnAddTruck.Click += new System.EventHandler(this.btnAddTruck_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnRefresh);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(946, 45);
            this.panel2.TabIndex = 2;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(10, 7);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 30);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // pnlGrid
            // 
            this.pnlGrid.Controls.Add(this.dataGridTrucks);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Location = new System.Drawing.Point(0, 45);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Padding = new System.Windows.Forms.Padding(10, 0, 10, 10);
            this.pnlGrid.Size = new System.Drawing.Size(946, 475);
            this.pnlGrid.TabIndex = 3;
            // 
            // dataGridTrucks
            // 
            this.dataGridTrucks.AllowUserToAddRows = false;
            this.dataGridTrucks.AllowUserToDeleteRows = false;
            this.dataGridTrucks.AllowUserToResizeRows = false;
            this.dataGridTrucks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridTrucks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Selected,
            this.Id,
            this.TruckID,
            this.LoadPrefix});
            this.dataGridTrucks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridTrucks.Location = new System.Drawing.Point(10, 0);
            this.dataGridTrucks.MultiSelect = false;
            this.dataGridTrucks.Name = "dataGridTrucks";
            this.dataGridTrucks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridTrucks.Size = new System.Drawing.Size(926, 465);
            this.dataGridTrucks.TabIndex = 2;
            this.dataGridTrucks.DoubleClick += new System.EventHandler(this.dataGridTrucks_DoubleClick);
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
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Visible = false;
            // 
            // TruckID
            // 
            this.TruckID.DataPropertyName = "Name";
            this.TruckID.HeaderText = "Truck ID";
            this.TruckID.Name = "TruckID";
            this.TruckID.ReadOnly = true;
            this.TruckID.Width = 250;
            // 
            // LoadPrefix
            // 
            this.LoadPrefix.DataPropertyName = "LoadPrefix";
            this.LoadPrefix.HeaderText = "Load Prefix";
            this.LoadPrefix.MinimumWidth = 150;
            this.LoadPrefix.Name = "LoadPrefix";
            this.LoadPrefix.ReadOnly = true;
            this.LoadPrefix.Width = 200;
            // 
            // btnCreateInstallPackage
            // 
            this.btnCreateInstallPackage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateInstallPackage.Location = new System.Drawing.Point(386, 3);
            this.btnCreateInstallPackage.Name = "btnCreateInstallPackage";
            this.btnCreateInstallPackage.Size = new System.Drawing.Size(240, 35);
            this.btnCreateInstallPackage.TabIndex = 6;
            this.btnCreateInstallPackage.Text = "Create Truck Installer Package";
            this.btnCreateInstallPackage.UseVisualStyleBackColor = true;
            this.btnCreateInstallPackage.Click += new System.EventHandler(this.btnCreateInstallPackage_Click);
            // 
            // TrucksPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "TrucksPage";
            this.Size = new System.Drawing.Size(946, 565);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridTrucks)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnAddTruck;
        private System.Windows.Forms.Button btnDeleteSelected;
        private System.Windows.Forms.Button btnEditSelected;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.DataGridView dataGridTrucks;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Selected;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn TruckID;
        private System.Windows.Forms.DataGridViewTextBoxColumn LoadPrefix;
        private System.Windows.Forms.Button btnCreateInstallPackage;
    }
}
