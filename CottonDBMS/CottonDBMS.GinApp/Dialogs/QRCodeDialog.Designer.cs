namespace CottonDBMS.GinApp.Dialogs
{
    partial class QRCodeDialog
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
            this.lblInfoText = new System.Windows.Forms.Label();
            this.pictureBoxQRCode = new System.Windows.Forms.PictureBox();
            this.btnDownloadQRCode = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblError = new System.Windows.Forms.Label();
            this.fileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQRCode)).BeginInit();
            this.SuspendLayout();
            // 
            // lblInfoText
            // 
            this.lblInfoText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfoText.Location = new System.Drawing.Point(12, 9);
            this.lblInfoText.Name = "lblInfoText";
            this.lblInfoText.Size = new System.Drawing.Size(477, 90);
            this.lblInfoText.TabIndex = 0;
            this.lblInfoText.Text = "Scan the QR code below from the settings screen in RFID Module Scan to share clie" +
    "nt, farm, and field lists with the app.  Sharing this code will give users read " +
    "access to your data.";
            // 
            // pictureBoxQRCode
            // 
            this.pictureBoxQRCode.Location = new System.Drawing.Point(-2, 76);
            this.pictureBoxQRCode.Name = "pictureBoxQRCode";
            this.pictureBoxQRCode.Size = new System.Drawing.Size(497, 304);
            this.pictureBoxQRCode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxQRCode.TabIndex = 1;
            this.pictureBoxQRCode.TabStop = false;
            // 
            // btnDownloadQRCode
            // 
            this.btnDownloadQRCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownloadQRCode.Location = new System.Drawing.Point(41, 386);
            this.btnDownloadQRCode.Name = "btnDownloadQRCode";
            this.btnDownloadQRCode.Size = new System.Drawing.Size(201, 35);
            this.btnDownloadQRCode.TabIndex = 2;
            this.btnDownloadQRCode.Text = "Save To Pdf";
            this.btnDownloadQRCode.UseVisualStyleBackColor = true;
            this.btnDownloadQRCode.Click += new System.EventHandler(this.btnDownloadQRCode_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(249, 386);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(201, 35);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblError
            // 
            this.lblError.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(12, 9);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(472, 161);
            this.lblError.TabIndex = 4;
            this.lblError.Text = "Unable to generate QR code.";
            this.lblError.Visible = false;
            // 
            // fileDialog
            // 
            this.fileDialog.CheckFileExists = false;
            this.fileDialog.DefaultExt = "pdf";
            this.fileDialog.Filter = "PDF Files|*.pdf|All files|*.*";
            this.fileDialog.Title = "Save File";
            // 
            // QRCodeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(496, 428);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDownloadQRCode);
            this.Controls.Add(this.pictureBoxQRCode);
            this.Controls.Add(this.lblInfoText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "QRCodeDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Connect RFID Module Scan";
            this.Load += new System.EventHandler(this.QRCodeDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQRCode)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblInfoText;
        private System.Windows.Forms.PictureBox pictureBoxQRCode;
        private System.Windows.Forms.Button btnDownloadQRCode;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.OpenFileDialog fileDialog;
    }
}