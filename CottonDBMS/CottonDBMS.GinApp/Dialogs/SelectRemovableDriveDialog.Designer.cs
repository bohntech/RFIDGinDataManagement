namespace CottonDBMS.GinApp.Dialogs
{
    partial class SelectRemovableDriveDialog
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSavePackage = new System.Windows.Forms.Button();
            this.cbTargetDrive = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(244, 69);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(130, 34);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSavePackage
            // 
            this.btnSavePackage.Location = new System.Drawing.Point(104, 69);
            this.btnSavePackage.Margin = new System.Windows.Forms.Padding(5);
            this.btnSavePackage.Name = "btnSavePackage";
            this.btnSavePackage.Size = new System.Drawing.Size(130, 34);
            this.btnSavePackage.TabIndex = 12;
            this.btnSavePackage.Text = "Create Package";
            this.btnSavePackage.UseVisualStyleBackColor = true;
            this.btnSavePackage.Click += new System.EventHandler(this.btnSavePackage_Click);
            // 
            // cbTargetDrive
            // 
            this.cbTargetDrive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTargetDrive.FormattingEnabled = true;
            this.cbTargetDrive.Location = new System.Drawing.Point(106, 21);
            this.cbTargetDrive.Name = "cbTargetDrive";
            this.cbTargetDrive.Size = new System.Drawing.Size(268, 21);
            this.cbTargetDrive.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 24);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "USB Drive:";
            // 
            // SelectRemovableDriveDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 119);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSavePackage);
            this.Controls.Add(this.cbTargetDrive);
            this.Controls.Add(this.label4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectRemovableDriveDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Select Drive";
            this.Load += new System.EventHandler(this.SelectRemovableDriveDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSavePackage;
        private System.Windows.Forms.ComboBox cbTargetDrive;
        private System.Windows.Forms.Label label4;
    }
}