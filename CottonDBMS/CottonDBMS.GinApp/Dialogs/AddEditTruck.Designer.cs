namespace CottonDBMS.GinApp.Dialogs
{
    partial class AddEditTruck
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbTruckID = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel9 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.formErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.tbLoadPrefix = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbTareWeight = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbRFIDTag = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbxIsSemi = new System.Windows.Forms.CheckBox();
            this.tbPhone = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbLicensePlate = new System.Windows.Forms.TextBox();
            this.tbOwner = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.flowLayoutPanel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 30);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Truck ID:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbTruckID
            // 
            this.tbTruckID.Location = new System.Drawing.Point(114, 27);
            this.tbTruckID.Margin = new System.Windows.Forms.Padding(4);
            this.tbTruckID.Name = "tbTruckID";
            this.tbTruckID.Size = new System.Drawing.Size(239, 22);
            this.tbTruckID.TabIndex = 1;
            this.tbTruckID.Validating += new System.ComponentModel.CancelEventHandler(this.tbTruckID_Validating);
            // 
            // flowLayoutPanel9
            // 
            this.flowLayoutPanel9.AutoSize = true;
            this.flowLayoutPanel9.Controls.Add(this.btnCancel);
            this.flowLayoutPanel9.Controls.Add(this.btnSave);
            this.flowLayoutPanel9.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel9.Location = new System.Drawing.Point(111, 274);
            this.flowLayoutPanel9.Name = "flowLayoutPanel9";
            this.flowLayoutPanel9.Size = new System.Drawing.Size(242, 43);
            this.flowLayoutPanel9.TabIndex = 14;
            this.flowLayoutPanel9.WrapContents = false;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(124, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(115, 37);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(3, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(115, 37);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // formErrorProvider
            // 
            this.formErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.formErrorProvider.ContainerControl = this;
            // 
            // tbLoadPrefix
            // 
            this.tbLoadPrefix.Location = new System.Drawing.Point(114, 57);
            this.tbLoadPrefix.Margin = new System.Windows.Forms.Padding(4);
            this.tbLoadPrefix.Name = "tbLoadPrefix";
            this.tbLoadPrefix.Size = new System.Drawing.Size(239, 22);
            this.tbLoadPrefix.TabIndex = 2;
            this.tbLoadPrefix.Validating += new System.ComponentModel.CancelEventHandler(this.tbLoadPrefix_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 60);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 16);
            this.label2.TabIndex = 15;
            this.label2.Text = "Load prefix:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbTareWeight
            // 
            this.tbTareWeight.Location = new System.Drawing.Point(114, 87);
            this.tbTareWeight.Margin = new System.Windows.Forms.Padding(4);
            this.tbTareWeight.MaxLength = 15;
            this.tbTareWeight.Name = "tbTareWeight";
            this.tbTareWeight.Size = new System.Drawing.Size(239, 22);
            this.tbTareWeight.TabIndex = 3;
            this.tbTareWeight.Validating += new System.ComponentModel.CancelEventHandler(this.tbTareWeight_Validating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 87);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 16);
            this.label3.TabIndex = 17;
            this.label3.Text = "Tare Weight:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // tbRFIDTag
            // 
            this.tbRFIDTag.Location = new System.Drawing.Point(114, 143);
            this.tbRFIDTag.Margin = new System.Windows.Forms.Padding(4);
            this.tbRFIDTag.MaxLength = 15;
            this.tbRFIDTag.Name = "tbRFIDTag";
            this.tbRFIDTag.Size = new System.Drawing.Size(239, 22);
            this.tbRFIDTag.TabIndex = 5;
            this.tbRFIDTag.Validating += new System.ComponentModel.CancelEventHandler(this.tbRFIDTag_Validating);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 146);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 16);
            this.label4.TabIndex = 19;
            this.label4.Text = "RFID Tag:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cbxIsSemi
            // 
            this.cbxIsSemi.AutoSize = true;
            this.cbxIsSemi.Location = new System.Drawing.Point(114, 116);
            this.cbxIsSemi.Name = "cbxIsSemi";
            this.cbxIsSemi.Size = new System.Drawing.Size(98, 20);
            this.cbxIsSemi.TabIndex = 4;
            this.cbxIsSemi.Text = "Split weigh?";
            this.cbxIsSemi.UseVisualStyleBackColor = true;
            // 
            // tbPhone
            // 
            this.tbPhone.Location = new System.Drawing.Point(114, 233);
            this.tbPhone.Margin = new System.Windows.Forms.Padding(4);
            this.tbPhone.MaxLength = 15;
            this.tbPhone.Name = "tbPhone";
            this.tbPhone.Size = new System.Drawing.Size(239, 22);
            this.tbPhone.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 174);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 16);
            this.label5.TabIndex = 28;
            this.label5.Text = "License Plate:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(57, 235);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 16);
            this.label7.TabIndex = 32;
            this.label7.Text = "Phone:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbLicensePlate
            // 
            this.tbLicensePlate.Location = new System.Drawing.Point(114, 173);
            this.tbLicensePlate.Margin = new System.Windows.Forms.Padding(4);
            this.tbLicensePlate.MaxLength = 15;
            this.tbLicensePlate.Name = "tbLicensePlate";
            this.tbLicensePlate.Size = new System.Drawing.Size(239, 22);
            this.tbLicensePlate.TabIndex = 6;
            // 
            // tbOwner
            // 
            this.tbOwner.Location = new System.Drawing.Point(114, 203);
            this.tbOwner.Margin = new System.Windows.Forms.Padding(4);
            this.tbOwner.MaxLength = 30;
            this.tbOwner.Name = "tbOwner";
            this.tbOwner.Size = new System.Drawing.Size(239, 22);
            this.tbOwner.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(57, 205);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 16);
            this.label6.TabIndex = 30;
            this.label6.Text = "Owner:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // AddEditTruck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 335);
            this.Controls.Add(this.tbPhone);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbLicensePlate);
            this.Controls.Add(this.tbOwner);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbxIsSemi);
            this.Controls.Add(this.tbRFIDTag);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbTareWeight);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbLoadPrefix);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.flowLayoutPanel9);
            this.Controls.Add(this.tbTruckID);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddEditTruck";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Truck";
            this.TopMost = true;
            this.flowLayoutPanel9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.formErrorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbTruckID;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel9;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ErrorProvider formErrorProvider;
        private System.Windows.Forms.TextBox tbLoadPrefix;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbTareWeight;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbxIsSemi;
        private System.Windows.Forms.TextBox tbRFIDTag;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbPhone;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbLicensePlate;
        private System.Windows.Forms.TextBox tbOwner;
        private System.Windows.Forms.Label label6;
    }
}