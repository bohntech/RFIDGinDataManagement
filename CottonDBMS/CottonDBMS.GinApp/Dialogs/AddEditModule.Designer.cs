namespace CottonDBMS.GinApp.Dialogs
{
    partial class AddEditModule
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tbSerialNumber = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.clientSelector1 = new CottonDBMS.GinApp.UserControls.ClientSelector();
            this.farmSelector1 = new CottonDBMS.GinApp.UserControls.FarmSelector();
            this.fieldSelector1 = new CottonDBMS.GinApp.UserControls.FieldSelector();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.cboDriver = new System.Windows.Forms.ComboBox();
            this.cboTruck = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblImportedLoadNumber = new System.Windows.Forms.Label();
            this.tbStatus = new System.Windows.Forms.ComboBox();
            this.lblImportedLoad = new System.Windows.Forms.Label();
            this.tbLongitude = new System.Windows.Forms.TextBox();
            this.tbLatitude = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel9 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblLoad = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbNotes = new System.Windows.Forms.TextBox();
            this.tbModuleID = new System.Windows.Forms.TextBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // tbSerialNumber
            // 
            this.tbSerialNumber.Location = new System.Drawing.Point(119, 3);
            this.tbSerialNumber.Name = "tbSerialNumber";
            this.tbSerialNumber.Size = new System.Drawing.Size(350, 22);
            this.tbSerialNumber.TabIndex = 4;
            this.tbSerialNumber.Validating += new System.ComponentModel.CancelEventHandler(this.tbSerialNumber_Validating);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.clientSelector1);
            this.flowLayoutPanel1.Controls.Add(this.farmSelector1);
            this.flowLayoutPanel1.Controls.Add(this.fieldSelector1);
            this.flowLayoutPanel1.Controls.Add(this.tableLayoutPanel2);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flowLayoutPanel1.Location = new System.Drawing.Point(5, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(507, 551);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // clientSelector1
            // 
            this.clientSelector1.AutoSize = true;
            this.clientSelector1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.clientSelector1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clientSelector1.HasError = false;
            this.clientSelector1.InputColumnWidth = 350;
            this.clientSelector1.LabelColumnWidth = 110;
            this.clientSelector1.Location = new System.Drawing.Point(4, 4);
            this.clientSelector1.Margin = new System.Windows.Forms.Padding(4);
            this.clientSelector1.Name = "clientSelector1";
            this.clientSelector1.Size = new System.Drawing.Size(493, 68);
            this.clientSelector1.TabIndex = 1;
            this.clientSelector1.SelectionChanged += new System.EventHandler(this.clientSelector1_SelectionChanged);
            // 
            // farmSelector1
            // 
            this.farmSelector1.AutoSize = true;
            this.farmSelector1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.farmSelector1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.farmSelector1.FormErrorProvider = null;
            this.farmSelector1.HasError = false;
            this.farmSelector1.InputColumnWidth = 350;
            this.farmSelector1.LabelColumnWidth = 110;
            this.farmSelector1.Location = new System.Drawing.Point(4, 80);
            this.farmSelector1.Margin = new System.Windows.Forms.Padding(4);
            this.farmSelector1.Name = "farmSelector1";
            this.farmSelector1.Size = new System.Drawing.Size(489, 58);
            this.farmSelector1.TabIndex = 2;
            this.farmSelector1.Visible = false;
            this.farmSelector1.SelectionChanged += new System.EventHandler(this.farmSelector1_SelectionChanged);
            // 
            // fieldSelector1
            // 
            this.fieldSelector1.AutoSize = true;
            this.fieldSelector1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.fieldSelector1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fieldSelector1.FormErrorProvider = null;
            this.fieldSelector1.HasError = false;
            this.fieldSelector1.InputColumnWidth = 350;
            this.fieldSelector1.LabelColumnWidth = 110;
            this.fieldSelector1.Location = new System.Drawing.Point(3, 145);
            this.fieldSelector1.Name = "fieldSelector1";
            this.fieldSelector1.Size = new System.Drawing.Size(492, 59);
            this.fieldSelector1.TabIndex = 3;
            this.fieldSelector1.Visible = false;
            this.fieldSelector1.SelectionChanged += new System.EventHandler(this.fieldSelector1_SelectionChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.cboDriver, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.cboTruck, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label11, 0, 8);
            this.tableLayoutPanel2.Controls.Add(this.label10, 0, 7);
            this.tableLayoutPanel2.Controls.Add(this.label9, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.lblImportedLoadNumber, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.tbStatus, 1, 8);
            this.tableLayoutPanel2.Controls.Add(this.lblImportedLoad, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.tbLongitude, 1, 7);
            this.tableLayoutPanel2.Controls.Add(this.tbLatitude, 1, 6);
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel9, 1, 10);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.tbSerialNumber, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.lblLoad, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 9);
            this.tableLayoutPanel2.Controls.Add(this.tbNotes, 1, 9);
            this.tableLayoutPanel2.Controls.Add(this.tbModuleID, 1, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 210);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 11;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(494, 335);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // cboDriver
            // 
            this.cboDriver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDriver.FormattingEnabled = true;
            this.cboDriver.Items.AddRange(new object[] {
            "-- Select One --",
            "In field",
            "Picked up",
            "At gin",
            "Ginned"});
            this.cboDriver.Location = new System.Drawing.Point(119, 89);
            this.cboDriver.Name = "cboDriver";
            this.cboDriver.Size = new System.Drawing.Size(350, 24);
            this.cboDriver.TabIndex = 6;
            this.cboDriver.Validating += new System.ComponentModel.CancelEventHandler(this.cboDriver_Validating);
            // 
            // cboTruck
            // 
            this.cboTruck.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTruck.FormattingEnabled = true;
            this.cboTruck.Items.AddRange(new object[] {
            "-- Select One --",
            "In field",
            "Picked up",
            "At gin",
            "Ginned"});
            this.cboTruck.Location = new System.Drawing.Point(119, 59);
            this.cboTruck.Name = "cboTruck";
            this.cboTruck.Size = new System.Drawing.Size(350, 24);
            this.cboTruck.TabIndex = 5;
            this.cboTruck.Validating += new System.ComponentModel.CancelEventHandler(this.cboTruck_Validating);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(3, 231);
            this.label11.Margin = new System.Windows.Forms.Padding(3);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(110, 24);
            this.label11.TabIndex = 12;
            this.label11.Text = "Status:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(3, 203);
            this.label10.Margin = new System.Windows.Forms.Padding(3);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(110, 22);
            this.label10.TabIndex = 10;
            this.label10.Text = "Longitude:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(3, 175);
            this.label9.Margin = new System.Windows.Forms.Padding(3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(110, 22);
            this.label9.TabIndex = 9;
            this.label9.Text = "Latitude:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblImportedLoadNumber
            // 
            this.lblImportedLoadNumber.Location = new System.Drawing.Point(3, 147);
            this.lblImportedLoadNumber.Margin = new System.Windows.Forms.Padding(3);
            this.lblImportedLoadNumber.Name = "lblImportedLoadNumber";
            this.lblImportedLoadNumber.Size = new System.Drawing.Size(110, 22);
            this.lblImportedLoadNumber.TabIndex = 8;
            this.lblImportedLoadNumber.Text = "Imported Load#:";
            this.lblImportedLoadNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbStatus
            // 
            this.tbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tbStatus.FormattingEnabled = true;
            this.tbStatus.Items.AddRange(new object[] {
            "-- Select One --",
            "In field",
            "Picked up",
            "At gin",
            "Ginned"});
            this.tbStatus.Location = new System.Drawing.Point(119, 231);
            this.tbStatus.Name = "tbStatus";
            this.tbStatus.Size = new System.Drawing.Size(350, 24);
            this.tbStatus.TabIndex = 9;
            this.tbStatus.Validating += new System.ComponentModel.CancelEventHandler(this.tbStatus_Validating);
            // 
            // lblImportedLoad
            // 
            this.lblImportedLoad.Location = new System.Drawing.Point(119, 147);
            this.lblImportedLoad.Margin = new System.Windows.Forms.Padding(3);
            this.lblImportedLoad.Name = "lblImportedLoad";
            this.lblImportedLoad.Size = new System.Drawing.Size(110, 22);
            this.lblImportedLoad.TabIndex = 22;
            this.lblImportedLoad.Text = "--";
            this.lblImportedLoad.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbLongitude
            // 
            this.tbLongitude.Location = new System.Drawing.Point(119, 203);
            this.tbLongitude.Name = "tbLongitude";
            this.tbLongitude.Size = new System.Drawing.Size(350, 22);
            this.tbLongitude.TabIndex = 8;
            this.tbLongitude.Validating += new System.ComponentModel.CancelEventHandler(this.tbLongitude_Validating);
            // 
            // tbLatitude
            // 
            this.tbLatitude.Location = new System.Drawing.Point(119, 175);
            this.tbLatitude.Margin = new System.Windows.Forms.Padding(3, 3, 25, 3);
            this.tbLatitude.Name = "tbLatitude";
            this.tbLatitude.Size = new System.Drawing.Size(350, 22);
            this.tbLatitude.TabIndex = 7;
            this.tbLatitude.Validating += new System.ComponentModel.CancelEventHandler(this.tbLatitude_Validating);
            // 
            // flowLayoutPanel9
            // 
            this.flowLayoutPanel9.AutoSize = true;
            this.flowLayoutPanel9.Controls.Add(this.btnCancel);
            this.flowLayoutPanel9.Controls.Add(this.btnSave);
            this.flowLayoutPanel9.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel9.Location = new System.Drawing.Point(119, 289);
            this.flowLayoutPanel9.Name = "flowLayoutPanel9";
            this.flowLayoutPanel9.Size = new System.Drawing.Size(242, 43);
            this.flowLayoutPanel9.TabIndex = 13;
            this.flowLayoutPanel9.WrapContents = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(124, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(115, 37);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(3, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(115, 37);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 119);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 22);
            this.label1.TabIndex = 5;
            this.label1.Text = "Load#:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 3);
            this.label4.Margin = new System.Windows.Forms.Padding(3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 22);
            this.label4.TabIndex = 4;
            this.label4.Text = "Serial #:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(3, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(110, 22);
            this.label5.TabIndex = 24;
            this.label5.Text = "Truck:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(3, 86);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(110, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "Driver";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLoad
            // 
            this.lblLoad.Location = new System.Drawing.Point(119, 119);
            this.lblLoad.Margin = new System.Windows.Forms.Padding(3);
            this.lblLoad.Name = "lblLoad";
            this.lblLoad.Size = new System.Drawing.Size(110, 22);
            this.lblLoad.TabIndex = 26;
            this.lblLoad.Text = "--";
            this.lblLoad.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 28);
            this.label2.TabIndex = 27;
            this.label2.Text = "Module ID:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 258);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 28);
            this.label3.TabIndex = 29;
            this.label3.Text = "Notes:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbNotes
            // 
            this.tbNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbNotes.Location = new System.Drawing.Point(119, 261);
            this.tbNotes.Margin = new System.Windows.Forms.Padding(3, 3, 25, 3);
            this.tbNotes.Name = "tbNotes";
            this.tbNotes.Size = new System.Drawing.Size(350, 22);
            this.tbNotes.TabIndex = 30;
            // 
            // tbModuleID
            // 
            this.tbModuleID.Location = new System.Drawing.Point(119, 31);
            this.tbModuleID.Name = "tbModuleID";
            this.tbModuleID.Size = new System.Drawing.Size(350, 22);
            this.tbModuleID.TabIndex = 31;
            this.tbModuleID.Validating += new System.ComponentModel.CancelEventHandler(this.tbModuleID_Validating);
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // AddEditModule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 562);
            this.Controls.Add(this.flowLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddEditModule";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Module";
            this.Load += new System.EventHandler(this.AddEditModule_Load);
            this.Shown += new System.EventHandler(this.AddEditModule_Shown);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.flowLayoutPanel9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox tbSerialNumber;
        private UserControls.ClientSelector clientSelector1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private UserControls.FarmSelector farmSelector1;
        private UserControls.FieldSelector fieldSelector1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblImportedLoadNumber;
        private System.Windows.Forms.ComboBox tbStatus;
        private System.Windows.Forms.Label lblImportedLoad;
        private System.Windows.Forms.TextBox tbLongitude;
        private System.Windows.Forms.TextBox tbLatitude;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.ComboBox cboDriver;
        private System.Windows.Forms.ComboBox cboTruck;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel9;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblLoad;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbNotes;
        private System.Windows.Forms.TextBox tbModuleID;
    }
}