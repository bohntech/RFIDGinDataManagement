namespace CottonDBMS.GinApp.UserControls
{
    partial class BalesPage
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.dataGridBales = new System.Windows.Forms.DataGridView();
            this.btnDeleteSelected = new System.Windows.Forms.Button();
            this.label23 = new System.Windows.Forms.Label();
            this.pnlBottomButtons = new System.Windows.Forms.Panel();
            this.btnAssignChecked = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.fileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.baleFilterBar = new CottonDBMS.GinApp.UserControls.BaleFilterBar();
            this.Selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.PbiNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModuleSerialNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GinTicketLoadNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WeightFromScale = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TareWeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NetWeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ScanNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocalCreatedTimestamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Updated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Classing_EstimatedSeedWeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Classing_TareWeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Classing_NetWeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Classing_Pk = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Classing_Gr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Classing_Lf = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Classing_St = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Classing_Mic = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Classing_Ex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Classing_Rm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Classing_Str = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Classing_CGr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Classing_Rd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Classing_Plusb = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Classing_Tr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Classing_Unif = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Classing_Len = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Classing_Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridBales)).BeginInit();
            this.pnlBottomButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlGrid
            // 
            this.pnlGrid.Controls.Add(this.dataGridBales);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Location = new System.Drawing.Point(0, 119);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Padding = new System.Windows.Forms.Padding(10, 5, 10, 10);
            this.pnlGrid.Size = new System.Drawing.Size(1093, 530);
            this.pnlGrid.TabIndex = 20;
            // 
            // dataGridBales
            // 
            this.dataGridBales.AllowUserToAddRows = false;
            this.dataGridBales.AllowUserToDeleteRows = false;
            this.dataGridBales.AllowUserToResizeRows = false;
            this.dataGridBales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridBales.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Selected,
            this.PbiNumber,
            this.ModuleSerialNumber,
            this.GinTicketLoadNumber,
            this.WeightFromScale,
            this.TareWeight,
            this.NetWeight,
            this.ScanNumber,
            this.LocalCreatedTimestamp,
            this.Updated,
            this.Classing_EstimatedSeedWeight,
            this.Classing_TareWeight,
            this.Classing_NetWeight,
            this.Classing_Pk,
            this.Classing_Gr,
            this.Classing_Lf,
            this.Classing_St,
            this.Classing_Mic,
            this.Classing_Ex,
            this.Classing_Rm,
            this.Classing_Str,
            this.Classing_CGr,
            this.Classing_Rd,
            this.Classing_Plusb,
            this.Classing_Tr,
            this.Classing_Unif,
            this.Classing_Len,
            this.Classing_Value});
            this.dataGridBales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridBales.Location = new System.Drawing.Point(10, 5);
            this.dataGridBales.MultiSelect = false;
            this.dataGridBales.Name = "dataGridBales";
            this.dataGridBales.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridBales.Size = new System.Drawing.Size(1073, 515);
            this.dataGridBales.TabIndex = 2;
            this.dataGridBales.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGridBales_Scroll);
            // 
            // btnDeleteSelected
            // 
            this.btnDeleteSelected.Location = new System.Drawing.Point(10, 3);
            this.btnDeleteSelected.Name = "btnDeleteSelected";
            this.btnDeleteSelected.Size = new System.Drawing.Size(134, 35);
            this.btnDeleteSelected.TabIndex = 5;
            this.btnDeleteSelected.Text = "Delete Checked";
            this.btnDeleteSelected.UseVisualStyleBackColor = true;
            this.btnDeleteSelected.Click += new System.EventHandler(this.btnDeleteSelected_Click);
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
            this.label23.TabIndex = 21;
            this.label23.Text = "Search Filters";
            // 
            // pnlBottomButtons
            // 
            this.pnlBottomButtons.Controls.Add(this.btnAssignChecked);
            this.pnlBottomButtons.Controls.Add(this.button1);
            this.pnlBottomButtons.Controls.Add(this.btnImport);
            this.pnlBottomButtons.Controls.Add(this.btnDeleteSelected);
            this.pnlBottomButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottomButtons.Location = new System.Drawing.Point(0, 649);
            this.pnlBottomButtons.Name = "pnlBottomButtons";
            this.pnlBottomButtons.Size = new System.Drawing.Size(1093, 45);
            this.pnlBottomButtons.TabIndex = 18;
            // 
            // btnAssignChecked
            // 
            this.btnAssignChecked.Location = new System.Drawing.Point(540, 3);
            this.btnAssignChecked.Name = "btnAssignChecked";
            this.btnAssignChecked.Size = new System.Drawing.Size(189, 35);
            this.btnAssignChecked.TabIndex = 8;
            this.btnAssignChecked.Text = "Assign Checked to Module";
            this.btnAssignChecked.UseVisualStyleBackColor = true;
            this.btnAssignChecked.Click += new System.EventHandler(this.btnAssignChecked_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(345, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(189, 35);
            this.button1.TabIndex = 7;
            this.button1.Text = "Export ALL Bale Data";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(150, 3);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(189, 35);
            this.btnImport.TabIndex = 6;
            this.btnImport.Text = "Import Classing Data";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // fileDialog
            // 
            this.fileDialog.FileName = "openFileDialog1";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.CreatePrompt = true;
            this.saveFileDialog.DefaultExt = "csv";
            this.saveFileDialog.Filter = "CSV Files|*.csv|All files|*.*";
            this.saveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog_FileOk);
            // 
            // baleFilterBar
            // 
            this.baleFilterBar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.baleFilterBar.BackColor = System.Drawing.SystemColors.Window;
            this.baleFilterBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.baleFilterBar.Location = new System.Drawing.Point(0, 22);
            this.baleFilterBar.Margin = new System.Windows.Forms.Padding(0);
            this.baleFilterBar.Name = "baleFilterBar";
            this.baleFilterBar.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.baleFilterBar.ShowApplyButton = true;
            this.baleFilterBar.ShowSort1 = true;
            this.baleFilterBar.ShowSort2 = true;
            this.baleFilterBar.ShowSort3 = true;
            this.baleFilterBar.Size = new System.Drawing.Size(1093, 97);
            this.baleFilterBar.TabIndex = 23;
            this.baleFilterBar.ApplyClicked += new System.EventHandler(this.baleFilterBar_ApplyClicked);
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
            // PbiNumber
            // 
            this.PbiNumber.DataPropertyName = "PbiNumber";
            this.PbiNumber.HeaderText = "Pbi";
            this.PbiNumber.Name = "PbiNumber";
            this.PbiNumber.ReadOnly = true;
            // 
            // ModuleSerialNumber
            // 
            this.ModuleSerialNumber.DataPropertyName = "ModuleSerialNumber";
            this.ModuleSerialNumber.HeaderText = "Module Serial#";
            this.ModuleSerialNumber.Name = "ModuleSerialNumber";
            this.ModuleSerialNumber.ReadOnly = true;
            // 
            // GinTicketLoadNumber
            // 
            this.GinTicketLoadNumber.DataPropertyName = "GinTicketLoadNumber";
            this.GinTicketLoadNumber.HeaderText = "Gin Tkt Load #";
            this.GinTicketLoadNumber.Name = "GinTicketLoadNumber";
            this.GinTicketLoadNumber.ReadOnly = true;
            // 
            // WeightFromScale
            // 
            this.WeightFromScale.DataPropertyName = "WeightFromScale";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.WeightFromScale.DefaultCellStyle = dataGridViewCellStyle1;
            this.WeightFromScale.HeaderText = "Weight from Scale (Gross)";
            this.WeightFromScale.Name = "WeightFromScale";
            this.WeightFromScale.ReadOnly = true;
            // 
            // TareWeight
            // 
            this.TareWeight.DataPropertyName = "TareWeight";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.TareWeight.DefaultCellStyle = dataGridViewCellStyle2;
            this.TareWeight.HeaderText = "Tare Weight";
            this.TareWeight.Name = "TareWeight";
            this.TareWeight.ReadOnly = true;
            // 
            // NetWeight
            // 
            this.NetWeight.DataPropertyName = "NetWeight";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.NetWeight.DefaultCellStyle = dataGridViewCellStyle3;
            this.NetWeight.HeaderText = "Net Weight";
            this.NetWeight.Name = "NetWeight";
            this.NetWeight.ReadOnly = true;
            // 
            // ScanNumber
            // 
            this.ScanNumber.DataPropertyName = "ScanNumber";
            this.ScanNumber.HeaderText = "Scan #";
            this.ScanNumber.Name = "ScanNumber";
            this.ScanNumber.ReadOnly = true;
            this.ScanNumber.Width = 50;
            // 
            // LocalCreatedTimestamp
            // 
            this.LocalCreatedTimestamp.DataPropertyName = "LocalCreatedTimestamp";
            this.LocalCreatedTimestamp.HeaderText = "Created";
            this.LocalCreatedTimestamp.Name = "LocalCreatedTimestamp";
            this.LocalCreatedTimestamp.ReadOnly = true;
            // 
            // Updated
            // 
            this.Updated.DataPropertyName = "LocalUpdatedTimestamp";
            this.Updated.HeaderText = "Updated";
            this.Updated.Name = "Updated";
            this.Updated.ReadOnly = true;
            // 
            // Classing_EstimatedSeedWeight
            // 
            this.Classing_EstimatedSeedWeight.DataPropertyName = "Classing_EstimatedSeedWeight";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Classing_EstimatedSeedWeight.DefaultCellStyle = dataGridViewCellStyle4;
            this.Classing_EstimatedSeedWeight.HeaderText = "Classing Est. Seed Weight";
            this.Classing_EstimatedSeedWeight.Name = "Classing_EstimatedSeedWeight";
            this.Classing_EstimatedSeedWeight.ReadOnly = true;
            this.Classing_EstimatedSeedWeight.Visible = false;
            // 
            // Classing_TareWeight
            // 
            this.Classing_TareWeight.DataPropertyName = "Classing_TareWeight";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Classing_TareWeight.DefaultCellStyle = dataGridViewCellStyle5;
            this.Classing_TareWeight.HeaderText = "Classing Tare Weight";
            this.Classing_TareWeight.Name = "Classing_TareWeight";
            this.Classing_TareWeight.ReadOnly = true;
            this.Classing_TareWeight.Visible = false;
            // 
            // Classing_NetWeight
            // 
            this.Classing_NetWeight.DataPropertyName = "Classing_NetWeight";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Classing_NetWeight.DefaultCellStyle = dataGridViewCellStyle6;
            this.Classing_NetWeight.HeaderText = "Classing Net Weight";
            this.Classing_NetWeight.Name = "Classing_NetWeight";
            this.Classing_NetWeight.ReadOnly = true;
            // 
            // Classing_Pk
            // 
            this.Classing_Pk.DataPropertyName = "Classing_Pk";
            this.Classing_Pk.HeaderText = "Pk";
            this.Classing_Pk.Name = "Classing_Pk";
            this.Classing_Pk.ReadOnly = true;
            // 
            // Classing_Gr
            // 
            this.Classing_Gr.DataPropertyName = "Classing_Gr";
            this.Classing_Gr.HeaderText = "Gr";
            this.Classing_Gr.Name = "Classing_Gr";
            this.Classing_Gr.ReadOnly = true;
            // 
            // Classing_Lf
            // 
            this.Classing_Lf.DataPropertyName = "Classing_Lf";
            this.Classing_Lf.HeaderText = "Lf";
            this.Classing_Lf.Name = "Classing_Lf";
            this.Classing_Lf.ReadOnly = true;
            // 
            // Classing_St
            // 
            this.Classing_St.DataPropertyName = "Classing_St";
            this.Classing_St.HeaderText = "St";
            this.Classing_St.Name = "Classing_St";
            this.Classing_St.ReadOnly = true;
            // 
            // Classing_Mic
            // 
            this.Classing_Mic.DataPropertyName = "Classing_Mic";
            this.Classing_Mic.HeaderText = "Mic";
            this.Classing_Mic.Name = "Classing_Mic";
            this.Classing_Mic.ReadOnly = true;
            // 
            // Classing_Ex
            // 
            this.Classing_Ex.DataPropertyName = "Classing_Ex";
            this.Classing_Ex.HeaderText = "Ex";
            this.Classing_Ex.Name = "Classing_Ex";
            this.Classing_Ex.ReadOnly = true;
            // 
            // Classing_Rm
            // 
            this.Classing_Rm.DataPropertyName = "Classing_Rm";
            this.Classing_Rm.HeaderText = "Rm";
            this.Classing_Rm.Name = "Classing_Rm";
            this.Classing_Rm.ReadOnly = true;
            // 
            // Classing_Str
            // 
            this.Classing_Str.DataPropertyName = "Classing_Str";
            this.Classing_Str.HeaderText = "Str";
            this.Classing_Str.Name = "Classing_Str";
            this.Classing_Str.ReadOnly = true;
            // 
            // Classing_CGr
            // 
            this.Classing_CGr.DataPropertyName = "Classing_CGr";
            this.Classing_CGr.HeaderText = "CGr";
            this.Classing_CGr.Name = "Classing_CGr";
            this.Classing_CGr.ReadOnly = true;
            // 
            // Classing_Rd
            // 
            this.Classing_Rd.DataPropertyName = "Classing_Rd";
            this.Classing_Rd.HeaderText = "Rd";
            this.Classing_Rd.Name = "Classing_Rd";
            this.Classing_Rd.ReadOnly = true;
            // 
            // Classing_Plusb
            // 
            this.Classing_Plusb.DataPropertyName = "Classing_Plusb";
            this.Classing_Plusb.HeaderText = "PlusB";
            this.Classing_Plusb.Name = "Classing_Plusb";
            this.Classing_Plusb.ReadOnly = true;
            // 
            // Classing_Tr
            // 
            this.Classing_Tr.DataPropertyName = "Classing_Tr";
            this.Classing_Tr.HeaderText = "Tr";
            this.Classing_Tr.Name = "Classing_Tr";
            this.Classing_Tr.ReadOnly = true;
            // 
            // Classing_Unif
            // 
            this.Classing_Unif.DataPropertyName = "Classing_Unif";
            this.Classing_Unif.HeaderText = "Unif";
            this.Classing_Unif.Name = "Classing_Unif";
            this.Classing_Unif.ReadOnly = true;
            // 
            // Classing_Len
            // 
            this.Classing_Len.DataPropertyName = "Classing_Len";
            this.Classing_Len.HeaderText = "Len";
            this.Classing_Len.Name = "Classing_Len";
            this.Classing_Len.ReadOnly = true;
            // 
            // Classing_Value
            // 
            this.Classing_Value.DataPropertyName = "Classing_Value";
            this.Classing_Value.HeaderText = "Value";
            this.Classing_Value.Name = "Classing_Value";
            this.Classing_Value.ReadOnly = true;
            // 
            // BalesPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.baleFilterBar);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.pnlBottomButtons);
            this.Name = "BalesPage";
            this.Size = new System.Drawing.Size(1093, 694);
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridBales)).EndInit();
            this.pnlBottomButtons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.DataGridView dataGridBales;
        private System.Windows.Forms.Button btnDeleteSelected;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Panel pnlBottomButtons;
        private BaleFilterBar baleFilterBar;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.OpenFileDialog fileDialog;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Button btnAssignChecked;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Selected;
        private System.Windows.Forms.DataGridViewTextBoxColumn PbiNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModuleSerialNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn GinTicketLoadNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn WeightFromScale;
        private System.Windows.Forms.DataGridViewTextBoxColumn TareWeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn NetWeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScanNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocalCreatedTimestamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn Updated;
        private System.Windows.Forms.DataGridViewTextBoxColumn Classing_EstimatedSeedWeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn Classing_TareWeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn Classing_NetWeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn Classing_Pk;
        private System.Windows.Forms.DataGridViewTextBoxColumn Classing_Gr;
        private System.Windows.Forms.DataGridViewTextBoxColumn Classing_Lf;
        private System.Windows.Forms.DataGridViewTextBoxColumn Classing_St;
        private System.Windows.Forms.DataGridViewTextBoxColumn Classing_Mic;
        private System.Windows.Forms.DataGridViewTextBoxColumn Classing_Ex;
        private System.Windows.Forms.DataGridViewTextBoxColumn Classing_Rm;
        private System.Windows.Forms.DataGridViewTextBoxColumn Classing_Str;
        private System.Windows.Forms.DataGridViewTextBoxColumn Classing_CGr;
        private System.Windows.Forms.DataGridViewTextBoxColumn Classing_Rd;
        private System.Windows.Forms.DataGridViewTextBoxColumn Classing_Plusb;
        private System.Windows.Forms.DataGridViewTextBoxColumn Classing_Tr;
        private System.Windows.Forms.DataGridViewTextBoxColumn Classing_Unif;
        private System.Windows.Forms.DataGridViewTextBoxColumn Classing_Len;
        private System.Windows.Forms.DataGridViewTextBoxColumn Classing_Value;
    }
}
