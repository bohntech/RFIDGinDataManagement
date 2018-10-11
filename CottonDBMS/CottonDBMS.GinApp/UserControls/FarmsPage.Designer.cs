namespace CottonDBMS.GinApp.UserControls
{
    partial class FarmsPage
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
            this.dataGridFarms = new System.Windows.Forms.DataGridView();
            this.Selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FarmId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClientName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FarmName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.label24 = new System.Windows.Forms.Label();
            this.tbClientFilter = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnApply = new System.Windows.Forms.Button();
            this.label23 = new System.Windows.Forms.Label();
            this.pnlBottomButtons = new System.Windows.Forms.Panel();
            this.btnDeleteSelected = new System.Windows.Forms.Button();
            this.btnEditSelected = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.pnlGrid = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridFarms)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.pnlBottomButtons.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridFarms
            // 
            this.dataGridFarms.AllowUserToAddRows = false;
            this.dataGridFarms.AllowUserToDeleteRows = false;
            this.dataGridFarms.AllowUserToResizeRows = false;
            this.dataGridFarms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridFarms.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Selected,
            this.FarmId,
            this.ClientName,
            this.FarmName});
            this.dataGridFarms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridFarms.Location = new System.Drawing.Point(10, 5);
            this.dataGridFarms.MultiSelect = false;
            this.dataGridFarms.Name = "dataGridFarms";
            this.dataGridFarms.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridFarms.Size = new System.Drawing.Size(1048, 499);
            this.dataGridFarms.TabIndex = 2;
            this.dataGridFarms.DoubleClick += new System.EventHandler(this.btnEditSelected_Click);
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
            // FarmId
            // 
            this.FarmId.DataPropertyName = "Id";
            this.FarmId.HeaderText = "FarmId";
            this.FarmId.Name = "FarmId";
            this.FarmId.ReadOnly = true;
            this.FarmId.Visible = false;
            // 
            // ClientName
            // 
            this.ClientName.DataPropertyName = "Client";
            this.ClientName.HeaderText = "Client";
            this.ClientName.MinimumWidth = 250;
            this.ClientName.Name = "ClientName";
            this.ClientName.ReadOnly = true;
            this.ClientName.Width = 250;
            // 
            // FarmName
            // 
            this.FarmName.DataPropertyName = "Name";
            this.FarmName.HeaderText = "Farm";
            this.FarmName.MinimumWidth = 250;
            this.FarmName.Name = "FarmName";
            this.FarmName.ReadOnly = true;
            this.FarmName.Width = 250;
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.flowLayoutPanel2);
            this.pnlTop.Controls.Add(this.label23);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1068, 76);
            this.pnlTop.TabIndex = 11;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.Controls.Add(this.flowLayoutPanel3);
            this.flowLayoutPanel2.Controls.Add(this.panel4);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 27);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.flowLayoutPanel2.Size = new System.Drawing.Size(1068, 55);
            this.flowLayoutPanel2.TabIndex = 8;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.AutoSize = true;
            this.flowLayoutPanel3.Controls.Add(this.label24);
            this.flowLayoutPanel3.Controls.Add(this.tbClientFilter);
            this.flowLayoutPanel3.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Padding = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.flowLayoutPanel3.Size = new System.Drawing.Size(244, 46);
            this.flowLayoutPanel3.TabIndex = 0;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(10, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(43, 17);
            this.label24.TabIndex = 0;
            this.label24.Text = "Client";
            // 
            // tbClientFilter
            // 
            this.tbClientFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tbClientFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbClientFilter.Location = new System.Drawing.Point(10, 20);
            this.tbClientFilter.Name = "tbClientFilter";
            this.tbClientFilter.Size = new System.Drawing.Size(231, 23);
            this.tbClientFilter.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnApply);
            this.panel4.Location = new System.Drawing.Point(250, 3);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.panel4.Size = new System.Drawing.Size(102, 44);
            this.panel4.TabIndex = 10;
            // 
            // btnApply
            // 
            this.btnApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApply.Location = new System.Drawing.Point(0, 14);
            this.btnApply.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(91, 30);
            this.btnApply.TabIndex = 1;
            this.btnApply.Text = "Refresh";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
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
            this.label23.TabIndex = 1;
            this.label23.Text = "Search Filters";
            // 
            // pnlBottomButtons
            // 
            this.pnlBottomButtons.Controls.Add(this.btnDeleteSelected);
            this.pnlBottomButtons.Controls.Add(this.btnEditSelected);
            this.pnlBottomButtons.Controls.Add(this.btnAdd);
            this.pnlBottomButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottomButtons.Location = new System.Drawing.Point(0, 590);
            this.pnlBottomButtons.Name = "pnlBottomButtons";
            this.pnlBottomButtons.Size = new System.Drawing.Size(1068, 45);
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
            this.btnAdd.Text = "Add Farm";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // pnlGrid
            // 
            this.pnlGrid.Controls.Add(this.dataGridFarms);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Location = new System.Drawing.Point(0, 76);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Padding = new System.Windows.Forms.Padding(10, 5, 10, 10);
            this.pnlGrid.Size = new System.Drawing.Size(1068, 514);
            this.pnlGrid.TabIndex = 12;
            // 
            // FarmsPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.pnlBottomButtons);
            this.Name = "FarmsPage";
            this.Size = new System.Drawing.Size(1068, 635);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridFarms)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.pnlBottomButtons.ResumeLayout(false);
            this.pnlGrid.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridFarms;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlBottomButtons;
        private System.Windows.Forms.Button btnDeleteSelected;
        private System.Windows.Forms.Button btnEditSelected;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox tbClientFilter;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Selected;
        private System.Windows.Forms.DataGridViewTextBoxColumn FarmId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClientName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FarmName;
    }
}
