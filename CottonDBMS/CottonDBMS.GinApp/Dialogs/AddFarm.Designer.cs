namespace CottonDBMS.GinApp.Dialogs
{
    partial class AddFarm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.formErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.clientSelector1 = new CottonDBMS.GinApp.UserControls.ClientSelector();
            this.flowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.tbFarm = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formErrorProvider)).BeginInit();
            this.flowPanel.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.tbFarm, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 1);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 77);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(484, 81);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // formErrorProvider
            // 
            this.formErrorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.formErrorProvider.ContainerControl = this;
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
            this.clientSelector1.TabIndex = 6;
            this.clientSelector1.SelectionChanged += new System.EventHandler(this.clientSelector1_SelectionChanged);
            // 
            // flowPanel
            // 
            this.flowPanel.AutoSize = true;
            this.flowPanel.Controls.Add(this.clientSelector1);
            this.flowPanel.Controls.Add(this.tableLayoutPanel1);
            this.flowPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowPanel.Location = new System.Drawing.Point(1, 12);
            this.flowPanel.Name = "flowPanel";
            this.flowPanel.Size = new System.Drawing.Size(491, 175);
            this.flowPanel.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.label2.Size = new System.Drawing.Size(100, 22);
            this.label2.TabIndex = 14;
            this.label2.Text = "Farm:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbFarm
            // 
            this.tbFarm.Location = new System.Drawing.Point(109, 3);
            this.tbFarm.Margin = new System.Windows.Forms.Padding(3, 3, 25, 3);
            this.tbFarm.Name = "tbFarm";
            this.tbFarm.Size = new System.Drawing.Size(350, 22);
            this.tbFarm.TabIndex = 3;
            this.tbFarm.Validating += new System.ComponentModel.CancelEventHandler(this.tbFarm_Validating);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(124, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(115, 37);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(3, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(115, 37);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.btnSave);
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(106, 38);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(242, 43);
            this.flowLayoutPanel1.TabIndex = 15;
            // 
            // AddFarm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 196);
            this.Controls.Add(this.flowPanel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddFarm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AddFarm";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.AddFarm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.formErrorProvider)).EndInit();
            this.flowPanel.ResumeLayout(false);
            this.flowPanel.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ErrorProvider formErrorProvider;
        private System.Windows.Forms.FlowLayoutPanel flowPanel;
        private UserControls.ClientSelector clientSelector1;
        private System.Windows.Forms.TextBox tbFarm;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}