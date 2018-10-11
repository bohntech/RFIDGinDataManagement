namespace CottonDBMS.GinApp.UserControls
{
    partial class ClientsPage
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.pnlBottomButtons = new System.Windows.Forms.Panel();
            this.btnDeleteSelected = new System.Windows.Forms.Button();
            this.btnEditSelected = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.dataGridClients = new System.Windows.Forms.DataGridView();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.Selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClientName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlTop.SuspendLayout();
            this.pnlBottomButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridClients)).BeginInit();
            this.pnlGrid.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.btnRefresh);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1124, 45);
            this.pnlTop.TabIndex = 8;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(9, 7);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 30);
            this.btnRefresh.TabIndex = 0;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // pnlBottomButtons
            // 
            this.pnlBottomButtons.Controls.Add(this.btnDeleteSelected);
            this.pnlBottomButtons.Controls.Add(this.btnEditSelected);
            this.pnlBottomButtons.Controls.Add(this.btnAdd);
            this.pnlBottomButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottomButtons.Location = new System.Drawing.Point(0, 654);
            this.pnlBottomButtons.Name = "pnlBottomButtons";
            this.pnlBottomButtons.Size = new System.Drawing.Size(1124, 45);
            this.pnlBottomButtons.TabIndex = 7;
            // 
            // btnDeleteSelected
            // 
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
            this.btnEditSelected.Location = new System.Drawing.Point(128, 3);
            this.btnEditSelected.Name = "btnEditSelected";
            this.btnEditSelected.Size = new System.Drawing.Size(112, 35);
            this.btnEditSelected.TabIndex = 4;
            this.btnEditSelected.Text = "Edit Selected";
            this.btnEditSelected.UseVisualStyleBackColor = true;
            this.btnEditSelected.Click += new System.EventHandler(this.btnEditSelected_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(9, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(113, 35);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add Client";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dataGridClients
            // 
            this.dataGridClients.AllowUserToAddRows = false;
            this.dataGridClients.AllowUserToDeleteRows = false;
            this.dataGridClients.AllowUserToResizeRows = false;
            this.dataGridClients.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridClients.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Selected,
            this.Id,
            this.ClientName});
            this.dataGridClients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridClients.Location = new System.Drawing.Point(10, 0);
            this.dataGridClients.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.dataGridClients.MultiSelect = false;
            this.dataGridClients.Name = "dataGridClients";
            this.dataGridClients.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridClients.Size = new System.Drawing.Size(1104, 599);
            this.dataGridClients.TabIndex = 9;
            this.dataGridClients.DoubleClick += new System.EventHandler(this.btnEditSelected_Click);
            // 
            // pnlGrid
            // 
            this.pnlGrid.Controls.Add(this.dataGridClients);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Location = new System.Drawing.Point(0, 45);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Padding = new System.Windows.Forms.Padding(10, 0, 10, 10);
            this.pnlGrid.Size = new System.Drawing.Size(1124, 609);
            this.pnlGrid.TabIndex = 10;
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
            // ClientName
            // 
            this.ClientName.DataPropertyName = "Name";
            this.ClientName.HeaderText = "Client Name";
            this.ClientName.Name = "ClientName";
            this.ClientName.ReadOnly = true;
            // 
            // ClientsPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.pnlBottomButtons);
            this.Name = "ClientsPage";
            this.Size = new System.Drawing.Size(1124, 699);
            this.pnlTop.ResumeLayout(false);
            this.pnlBottomButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridClients)).EndInit();
            this.pnlGrid.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlBottomButtons;
        private System.Windows.Forms.Button btnDeleteSelected;
        private System.Windows.Forms.Button btnEditSelected;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView dataGridClients;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Selected;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClientName;
    }
}
