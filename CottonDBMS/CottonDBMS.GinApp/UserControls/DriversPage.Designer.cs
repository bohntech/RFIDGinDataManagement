namespace CottonDBMS.GinApp.UserControls
{
    partial class DriversPage
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDeleteSelected = new System.Windows.Forms.Button();
            this.btnEditSelected = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.dataGridDrivers = new System.Windows.Forms.DataGridView();
            this.Selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Firstname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Lastname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDrivers)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnRefresh);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1047, 45);
            this.panel2.TabIndex = 5;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(10, 7);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 30);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnDeleteSelected);
            this.panel1.Controls.Add(this.btnEditSelected);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 609);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1047, 45);
            this.panel1.TabIndex = 4;
            // 
            // btnDeleteSelected
            // 
            this.btnDeleteSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.btnEditSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(9, 2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(113, 35);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add Driver";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // pnlGrid
            // 
            this.pnlGrid.Controls.Add(this.dataGridDrivers);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Location = new System.Drawing.Point(0, 45);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Padding = new System.Windows.Forms.Padding(10, 0, 10, 10);
            this.pnlGrid.Size = new System.Drawing.Size(1047, 564);
            this.pnlGrid.TabIndex = 6;
            // 
            // dataGridDrivers
            // 
            this.dataGridDrivers.AllowUserToAddRows = false;
            this.dataGridDrivers.AllowUserToDeleteRows = false;
            this.dataGridDrivers.AllowUserToResizeRows = false;
            this.dataGridDrivers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridDrivers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Selected,
            this.Id,
            this.Firstname,
            this.Lastname});
            this.dataGridDrivers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridDrivers.Location = new System.Drawing.Point(10, 0);
            this.dataGridDrivers.MultiSelect = false;
            this.dataGridDrivers.Name = "dataGridDrivers";
            this.dataGridDrivers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridDrivers.Size = new System.Drawing.Size(1027, 554);
            this.dataGridDrivers.TabIndex = 2;
            this.dataGridDrivers.DoubleClick += new System.EventHandler(this.btnEditSelected_Click);
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
            // Firstname
            // 
            this.Firstname.DataPropertyName = "Firstname";
            this.Firstname.HeaderText = "First name";
            this.Firstname.Name = "Firstname";
            this.Firstname.ReadOnly = true;
            // 
            // Lastname
            // 
            this.Lastname.DataPropertyName = "Lastname";
            this.Lastname.HeaderText = "Last name";
            this.Lastname.Name = "Lastname";
            this.Lastname.ReadOnly = true;
            // 
            // DriversPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "DriversPage";
            this.Size = new System.Drawing.Size(1047, 654);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDrivers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnDeleteSelected;
        private System.Windows.Forms.Button btnEditSelected;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.DataGridView dataGridDrivers;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Selected;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Firstname;
        private System.Windows.Forms.DataGridViewTextBoxColumn Lastname;
    }
}
