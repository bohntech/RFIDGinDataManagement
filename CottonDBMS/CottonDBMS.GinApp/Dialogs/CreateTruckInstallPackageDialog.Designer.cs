namespace CottonDBMS.GinApp.Dialogs
{
    partial class CreateTruckInstallPackageDialog
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
            this.label4 = new System.Windows.Forms.Label();
            this.cbTargetDrive = new System.Windows.Forms.ComboBox();
            this.btnSavePackage = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(54, 34);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "USB Drive:";
            // 
            // cbTargetDrive
            // 
            this.cbTargetDrive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTargetDrive.FormattingEnabled = true;
            this.cbTargetDrive.Location = new System.Drawing.Point(144, 31);
            this.cbTargetDrive.Name = "cbTargetDrive";
            this.cbTargetDrive.Size = new System.Drawing.Size(268, 24);
            this.cbTargetDrive.TabIndex = 7;
            // 
            // btnSavePackage
            // 
            this.btnSavePackage.Location = new System.Drawing.Point(142, 79);
            this.btnSavePackage.Margin = new System.Windows.Forms.Padding(5);
            this.btnSavePackage.Name = "btnSavePackage";
            this.btnSavePackage.Size = new System.Drawing.Size(130, 34);
            this.btnSavePackage.TabIndex = 8;
            this.btnSavePackage.Text = "Create Package";
            this.btnSavePackage.UseVisualStyleBackColor = true;
            this.btnSavePackage.Click += new System.EventHandler(this.btnSavePackage_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(282, 79);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(130, 34);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // CreateTruckInstallPackageDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 143);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSavePackage);
            this.Controls.Add(this.cbTargetDrive);
            this.Controls.Add(this.label4);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateTruckInstallPackageDialog";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create Truck Install Package";
            this.Load += new System.EventHandler(this.CreateTruckInstallPackageDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbTargetDrive;
        private System.Windows.Forms.Button btnSavePackage;
        private System.Windows.Forms.Button btnCancel;
    }
}