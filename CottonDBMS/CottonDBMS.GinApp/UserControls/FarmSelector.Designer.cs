namespace CottonDBMS.GinApp.UserControls
{
    partial class FarmSelector
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblFarmSelect = new System.Windows.Forms.Label();
            this.cboFarm = new System.Windows.Forms.ComboBox();
            this.lblNewFarm = new System.Windows.Forms.Label();
            this.tbNewFarm = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.lblFarmSelect, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cboFarm, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblNewFarm, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbNewFarm, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(505, 58);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // lblFarmSelect
            // 
            this.lblFarmSelect.Location = new System.Drawing.Point(3, 3);
            this.lblFarmSelect.Margin = new System.Windows.Forms.Padding(3);
            this.lblFarmSelect.Name = "lblFarmSelect";
            this.lblFarmSelect.Size = new System.Drawing.Size(100, 24);
            this.lblFarmSelect.TabIndex = 0;
            this.lblFarmSelect.Text = "Farm:";
            this.lblFarmSelect.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboFarm
            // 
            this.cboFarm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFarm.FormattingEnabled = true;
            this.cboFarm.Location = new System.Drawing.Point(109, 3);
            this.cboFarm.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.cboFarm.Name = "cboFarm";
            this.cboFarm.Size = new System.Drawing.Size(376, 24);
            this.cboFarm.TabIndex = 1;
            this.cboFarm.SelectedIndexChanged += new System.EventHandler(this.cboFarm_SelectedIndexChanged);
            this.cboFarm.Validating += new System.ComponentModel.CancelEventHandler(this.cboFarm_Validating);
            // 
            // lblNewFarm
            // 
            this.lblNewFarm.Location = new System.Drawing.Point(3, 33);
            this.lblNewFarm.Margin = new System.Windows.Forms.Padding(3);
            this.lblNewFarm.Name = "lblNewFarm";
            this.lblNewFarm.Size = new System.Drawing.Size(100, 22);
            this.lblNewFarm.TabIndex = 2;
            this.lblNewFarm.Text = "New Farm:";
            this.lblNewFarm.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbNewFarm
            // 
            this.tbNewFarm.Location = new System.Drawing.Point(109, 33);
            this.tbNewFarm.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.tbNewFarm.Name = "tbNewFarm";
            this.tbNewFarm.Size = new System.Drawing.Size(376, 22);
            this.tbNewFarm.TabIndex = 3;
            this.tbNewFarm.Validating += new System.ComponentModel.CancelEventHandler(this.tbNewFarm_Validating);
            // 
            // FarmSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FarmSelector";
            this.Size = new System.Drawing.Size(505, 58);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblFarmSelect;
        private System.Windows.Forms.ComboBox cboFarm;
        private System.Windows.Forms.Label lblNewFarm;
        private System.Windows.Forms.TextBox tbNewFarm;
    }
}
