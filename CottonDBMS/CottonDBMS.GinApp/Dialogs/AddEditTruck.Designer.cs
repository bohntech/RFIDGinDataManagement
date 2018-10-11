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
            this.flowLayoutPanel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 30);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Truck ID:";
            // 
            // tbTruckID
            // 
            this.tbTruckID.Location = new System.Drawing.Point(101, 27);
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
            this.flowLayoutPanel9.Location = new System.Drawing.Point(98, 103);
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
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(3, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(115, 37);
            this.btnSave.TabIndex = 3;
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
            this.tbLoadPrefix.Location = new System.Drawing.Point(101, 61);
            this.tbLoadPrefix.Margin = new System.Windows.Forms.Padding(4);
            this.tbLoadPrefix.Name = "tbLoadPrefix";
            this.tbLoadPrefix.Size = new System.Drawing.Size(239, 22);
            this.tbLoadPrefix.TabIndex = 2;
            this.tbLoadPrefix.Validating += new System.ComponentModel.CancelEventHandler(this.tbLoadPrefix_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 64);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 16);
            this.label2.TabIndex = 15;
            this.label2.Text = "Load prefix:";
            // 
            // AddEditTruck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 165);
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
    }
}