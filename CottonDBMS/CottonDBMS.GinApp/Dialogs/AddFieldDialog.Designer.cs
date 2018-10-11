namespace CottonDBMS.GinApp.Dialogs
{
    partial class AddFieldDialog
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tbField = new System.Windows.Forms.TextBox();
            this.lblField = new System.Windows.Forms.Label();
            this.formErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.clientSelector1 = new CottonDBMS.GinApp.UserControls.ClientSelector();
            this.farmSelector1 = new CottonDBMS.GinApp.UserControls.FarmSelector();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formErrorProvider)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(124, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(115, 37);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(3, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(115, 37);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbField, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblField, 0, 0);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 143);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(484, 81);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // tbField
            // 
            this.tbField.BackColor = System.Drawing.SystemColors.Window;
            this.tbField.Location = new System.Drawing.Point(109, 3);
            this.tbField.Margin = new System.Windows.Forms.Padding(3, 3, 25, 3);
            this.tbField.Name = "tbField";
            this.tbField.Size = new System.Drawing.Size(350, 22);
            this.tbField.TabIndex = 5;
            this.tbField.Validating += new System.ComponentModel.CancelEventHandler(this.tbField_Validating);
            // 
            // lblField
            // 
            this.lblField.Location = new System.Drawing.Point(3, 0);
            this.lblField.Name = "lblField";
            this.lblField.Size = new System.Drawing.Size(100, 22);
            this.lblField.TabIndex = 17;
            this.lblField.Text = "Field:";
            this.lblField.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // formErrorProvider
            // 
            this.formErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.formErrorProvider.ContainerControl = this;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.clientSelector1);
            this.flowLayoutPanel1.Controls.Add(this.farmSelector1);
            this.flowLayoutPanel1.Controls.Add(this.tableLayoutPanel1);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(495, 227);
            this.flowLayoutPanel1.TabIndex = 8;
            // 
            // clientSelector1
            // 
            this.clientSelector1.AutoSize = true;
            this.clientSelector1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.clientSelector1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clientSelector1.HasError = false;
            this.clientSelector1.InputColumnWidth = 350;
            this.clientSelector1.LabelColumnWidth = 100;
            this.clientSelector1.Location = new System.Drawing.Point(3, 3);
            this.clientSelector1.Name = "clientSelector1";
            this.clientSelector1.Size = new System.Drawing.Size(483, 68);
            this.clientSelector1.TabIndex = 0;
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
            this.farmSelector1.LabelColumnWidth = 100;
            this.farmSelector1.Location = new System.Drawing.Point(4, 78);
            this.farmSelector1.Margin = new System.Windows.Forms.Padding(4);
            this.farmSelector1.Name = "farmSelector1";
            this.farmSelector1.Size = new System.Drawing.Size(479, 58);
            this.farmSelector1.TabIndex = 1;
            this.farmSelector1.SelectionChanged += new System.EventHandler(this.farmSelector1_SelectionChanged);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel2.Controls.Add(this.btnSave);
            this.flowLayoutPanel2.Controls.Add(this.btnCancel);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(106, 38);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(242, 43);
            this.flowLayoutPanel2.TabIndex = 9;
            // 
            // AddFieldDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 250);
            this.Controls.Add(this.flowLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddFieldDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AddFieldDialog";
            this.TopMost = true;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formErrorProvider)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox tbField;
        private System.Windows.Forms.ErrorProvider formErrorProvider;
        private System.Windows.Forms.Label lblField;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private UserControls.ClientSelector clientSelector1;
        private UserControls.FarmSelector farmSelector1;
    }
}