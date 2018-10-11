namespace CottonDBMS.GinApp.Dialogs
{
    partial class MapReportDialog
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSaveToPdf = new System.Windows.Forms.Button();
            this.btnCloseMap = new System.Windows.Forms.Button();
            this.mapBrowser = new System.Windows.Forms.WebBrowser();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCloseMap);
            this.panel1.Controls.Add(this.btnSaveToPdf);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1100, 44);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(435, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Scale and position then map as you would like it to appear in your report.";
            // 
            // btnSaveToPdf
            // 
            this.btnSaveToPdf.Location = new System.Drawing.Point(876, 7);
            this.btnSaveToPdf.Name = "btnSaveToPdf";
            this.btnSaveToPdf.Size = new System.Drawing.Size(105, 30);
            this.btnSaveToPdf.TabIndex = 1;
            this.btnSaveToPdf.Text = "Save to PDF";
            this.btnSaveToPdf.UseVisualStyleBackColor = true;
            this.btnSaveToPdf.Click += new System.EventHandler(this.btnSaveToPdf_Click);
            // 
            // btnCloseMap
            // 
            this.btnCloseMap.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCloseMap.Location = new System.Drawing.Point(987, 7);
            this.btnCloseMap.Name = "btnCloseMap";
            this.btnCloseMap.Size = new System.Drawing.Size(105, 30);
            this.btnCloseMap.TabIndex = 2;
            this.btnCloseMap.Text = "Cancel";
            this.btnCloseMap.UseVisualStyleBackColor = true;
            this.btnCloseMap.Click += new System.EventHandler(this.btnCloseMap_Click);
            // 
            // mapBrowser
            // 
            this.mapBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapBrowser.Location = new System.Drawing.Point(0, 44);
            this.mapBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.mapBrowser.Name = "mapBrowser";
            this.mapBrowser.Size = new System.Drawing.Size(1100, 637);
            this.mapBrowser.TabIndex = 1;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "pdf";
            this.saveFileDialog.Filter = "PDF Files|*.pdf";
            // 
            // MapReportDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1100, 681);
            this.Controls.Add(this.mapBrowser);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MapReportDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Map Boundaries";
            this.Load += new System.EventHandler(this.MapReportDialog_Load);
            this.Shown += new System.EventHandler(this.MapReportDialog_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCloseMap;
        private System.Windows.Forms.Button btnSaveToPdf;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.WebBrowser mapBrowser;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}