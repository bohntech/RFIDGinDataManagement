namespace CottonDBMS.GinApp.UserControls
{
    partial class ClientSelector
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
            this.lblClientSelect = new System.Windows.Forms.Label();
            this.cboClient = new System.Windows.Forms.ComboBox();
            this.lblNewClient = new System.Windows.Forms.Label();
            this.tbNewClient = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.lblClientSelect, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.cboClient, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblNewClient, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbNewClient, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 4);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(379, 60);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblClientSelect
            // 
            this.lblClientSelect.Location = new System.Drawing.Point(3, 3);
            this.lblClientSelect.Margin = new System.Windows.Forms.Padding(3);
            this.lblClientSelect.Name = "lblClientSelect";
            this.lblClientSelect.Size = new System.Drawing.Size(100, 24);
            this.lblClientSelect.TabIndex = 0;
            this.lblClientSelect.Text = "Client:";
            this.lblClientSelect.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboClient
            // 
            this.cboClient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboClient.FormattingEnabled = true;
            this.cboClient.Location = new System.Drawing.Point(109, 3);
            this.cboClient.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.cboClient.Name = "cboClient";
            this.cboClient.Size = new System.Drawing.Size(250, 24);
            this.cboClient.TabIndex = 1;
            this.cboClient.SelectedIndexChanged += new System.EventHandler(this.cboClient_SelectedIndexChanged);
            this.cboClient.Validating += new System.ComponentModel.CancelEventHandler(this.cboClient_Validating);
            // 
            // lblNewClient
            // 
            this.lblNewClient.Location = new System.Drawing.Point(3, 33);
            this.lblNewClient.Margin = new System.Windows.Forms.Padding(3);
            this.lblNewClient.Name = "lblNewClient";
            this.lblNewClient.Size = new System.Drawing.Size(100, 24);
            this.lblNewClient.TabIndex = 2;
            this.lblNewClient.Text = "New Client:";
            this.lblNewClient.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbNewClient
            // 
            this.tbNewClient.Location = new System.Drawing.Point(109, 33);
            this.tbNewClient.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.tbNewClient.Name = "tbNewClient";
            this.tbNewClient.Size = new System.Drawing.Size(250, 22);
            this.tbNewClient.TabIndex = 3;
            this.tbNewClient.Validating += new System.ComponentModel.CancelEventHandler(this.tbNewClient_Validating);
            // 
            // ClientSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ClientSelector";
            this.Size = new System.Drawing.Size(383, 68);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblClientSelect;
        private System.Windows.Forms.ComboBox cboClient;
        private System.Windows.Forms.Label lblNewClient;
        private System.Windows.Forms.TextBox tbNewClient;
    }
}
