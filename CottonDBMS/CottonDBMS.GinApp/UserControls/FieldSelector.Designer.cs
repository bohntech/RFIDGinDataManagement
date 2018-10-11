namespace CottonDBMS.GinApp.UserControls
{
    partial class FieldSelector
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
            this.tbNewField = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblFieldSelect = new System.Windows.Forms.Label();
            this.cboField = new System.Windows.Forms.ComboBox();
            this.lblNewField = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbNewField
            // 
            this.tbNewField.Location = new System.Drawing.Point(109, 32);
            this.tbNewField.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.tbNewField.Name = "tbNewField";
            this.tbNewField.Size = new System.Drawing.Size(375, 21);
            this.tbNewField.TabIndex = 3;
            this.tbNewField.Validating += new System.ComponentModel.CancelEventHandler(this.tbNewField_Validating);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.lblFieldSelect, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cboField, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblNewField, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbNewField, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(504, 56);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // lblFieldSelect
            // 
            this.lblFieldSelect.Location = new System.Drawing.Point(3, 3);
            this.lblFieldSelect.Margin = new System.Windows.Forms.Padding(3);
            this.lblFieldSelect.Name = "lblFieldSelect";
            this.lblFieldSelect.Size = new System.Drawing.Size(100, 23);
            this.lblFieldSelect.TabIndex = 0;
            this.lblFieldSelect.Text = "Field:";
            this.lblFieldSelect.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboField
            // 
            this.cboField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboField.FormattingEnabled = true;
            this.cboField.Location = new System.Drawing.Point(109, 3);
            this.cboField.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.cboField.Name = "cboField";
            this.cboField.Size = new System.Drawing.Size(375, 23);
            this.cboField.TabIndex = 1;
            this.cboField.SelectedIndexChanged += new System.EventHandler(this.cboField_SelectedIndexChanged);
            this.cboField.Validating += new System.ComponentModel.CancelEventHandler(this.cboField_Validating);
            // 
            // lblNewField
            // 
            this.lblNewField.Location = new System.Drawing.Point(3, 32);
            this.lblNewField.Margin = new System.Windows.Forms.Padding(3);
            this.lblNewField.Name = "lblNewField";
            this.lblNewField.Size = new System.Drawing.Size(100, 21);
            this.lblNewField.TabIndex = 2;
            this.lblNewField.Text = "New Field:";
            this.lblNewField.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FieldSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FieldSelector";
            this.Size = new System.Drawing.Size(504, 56);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbNewField;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblFieldSelect;
        private System.Windows.Forms.ComboBox cboField;
        private System.Windows.Forms.Label lblNewField;
    }
}
