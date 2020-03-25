namespace CottonDBMS.GinApp.Dialogs
{
    partial class ChangeStatusDialog
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
            this.lblSerialNumbers = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.clientSelector1 = new CottonDBMS.GinApp.UserControls.ClientSelector();
            this.farmSelector1 = new CottonDBMS.GinApp.UserControls.FarmSelector();
            this.fieldSelector1 = new CottonDBMS.GinApp.UserControls.FieldSelector();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label11 = new System.Windows.Forms.Label();
            this.tbStatus = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel9 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel9.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSerialNumbers
            // 
            this.lblSerialNumbers.AutoEllipsis = true;
            this.lblSerialNumbers.Location = new System.Drawing.Point(119, 0);
            this.lblSerialNumbers.Name = "lblSerialNumbers";
            this.lblSerialNumbers.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblSerialNumbers.Size = new System.Drawing.Size(350, 63);
            this.lblSerialNumbers.TabIndex = 1;
            this.lblSerialNumbers.Text = "--";
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
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
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(499, 369);
            this.flowLayoutPanel1.TabIndex = 3;
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
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.label11, 0, 8);
            this.tableLayoutPanel2.Controls.Add(this.lblSerialNumbers, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.tbStatus, 1, 8);
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel9, 1, 10);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 0);
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
            this.tableLayoutPanel2.Size = new System.Drawing.Size(496, 142);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(3, 66);
            this.label11.Margin = new System.Windows.Forms.Padding(3);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(110, 24);
            this.label11.TabIndex = 12;
            this.label11.Text = "Status:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            "On feeder",
            "Ginned"});
            this.tbStatus.Location = new System.Drawing.Point(119, 66);
            this.tbStatus.Name = "tbStatus";
            this.tbStatus.Size = new System.Drawing.Size(350, 24);
            this.tbStatus.TabIndex = 9;
            this.tbStatus.Validating += new System.ComponentModel.CancelEventHandler(this.tbStatus_Validating);
            // 
            // flowLayoutPanel9
            // 
            this.flowLayoutPanel9.AutoSize = true;
            this.flowLayoutPanel9.Controls.Add(this.btnCancel);
            this.flowLayoutPanel9.Controls.Add(this.btnSave);
            this.flowLayoutPanel9.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel9.Location = new System.Drawing.Point(119, 96);
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
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(3, 3);
            this.label4.Margin = new System.Windows.Forms.Padding(3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 22);
            this.label4.TabIndex = 4;
            this.label4.Text = "Serial #s:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ChangeStatusDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 395);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChangeStatusDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Update Modules";
            this.Load += new System.EventHandler(this.ChangeStatusDialog_Load);
            this.Shown += new System.EventHandler(this.ChangeStatusDialog_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.flowLayoutPanel9.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblSerialNumbers;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private UserControls.ClientSelector clientSelector1;
        private UserControls.FarmSelector farmSelector1;
        private UserControls.FieldSelector fieldSelector1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox tbStatus;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel9;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label4;
    }
}