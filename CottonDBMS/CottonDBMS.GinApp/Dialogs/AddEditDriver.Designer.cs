namespace CottonDBMS.GinApp.Dialogs
{
    partial class AddEditDriver
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
            this.flowLayoutPanel9 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.tbFirstname = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbLastname = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.formErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.flowLayoutPanel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formErrorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutPanel9
            // 
            this.flowLayoutPanel9.AutoSize = true;
            this.flowLayoutPanel9.Controls.Add(this.btnCancel);
            this.flowLayoutPanel9.Controls.Add(this.btnSave);
            this.flowLayoutPanel9.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel9.Location = new System.Drawing.Point(105, 96);
            this.flowLayoutPanel9.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel9.Name = "flowLayoutPanel9";
            this.flowLayoutPanel9.Size = new System.Drawing.Size(246, 53);
            this.flowLayoutPanel9.TabIndex = 17;
            this.flowLayoutPanel9.WrapContents = false;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(127, 4);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(115, 37);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(4, 4);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(115, 37);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tbFirstname
            // 
            this.tbFirstname.Location = new System.Drawing.Point(108, 28);
            this.tbFirstname.Margin = new System.Windows.Forms.Padding(5);
            this.tbFirstname.Name = "tbFirstname";
            this.tbFirstname.Size = new System.Drawing.Size(239, 22);
            this.tbFirstname.TabIndex = 1;
            this.tbFirstname.Validating += new System.ComponentModel.CancelEventHandler(this.tbFirstname_Validating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 31);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 16);
            this.label1.TabIndex = 15;
            this.label1.Text = "First name:";
            // 
            // tbLastname
            // 
            this.tbLastname.Location = new System.Drawing.Point(108, 62);
            this.tbLastname.Margin = new System.Windows.Forms.Padding(5);
            this.tbLastname.Name = "tbLastname";
            this.tbLastname.Size = new System.Drawing.Size(239, 22);
            this.tbLastname.TabIndex = 2;
            this.tbLastname.Validating += new System.ComponentModel.CancelEventHandler(this.tbLastname_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 65);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 16);
            this.label2.TabIndex = 18;
            this.label2.Text = "Last name:";
            // 
            // formErrorProvider
            // 
            this.formErrorProvider.ContainerControl = this;
            // 
            // AddEditDriver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 154);
            this.Controls.Add(this.tbLastname);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.flowLayoutPanel9);
            this.Controls.Add(this.tbFirstname);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddEditDriver";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AddEditDriver";
            this.TopMost = true;
            this.flowLayoutPanel9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.formErrorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel9;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox tbFirstname;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbLastname;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ErrorProvider formErrorProvider;
    }
}