namespace CottonDBMS.GinApp.UserControls
{
    partial class ReportPage
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
            this.label36 = new System.Windows.Forms.Label();
            this.lblFilterTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblPdfTitle = new System.Windows.Forms.Label();
            this.tbReportTitle = new System.Windows.Forms.TextBox();
            this.lblOutputTemplate = new System.Windows.Forms.Label();
            this.cboOutputType = new System.Windows.Forms.ComboBox();
            this.label50 = new System.Windows.Forms.Label();
            this.cboExportType = new System.Windows.Forms.ComboBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.label49 = new System.Windows.Forms.Label();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.savePdfDialog = new System.Windows.Forms.SaveFileDialog();
            this.filterBar = new CottonDBMS.GinApp.UserControls.ModuleFilterBar();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label36.Location = new System.Drawing.Point(-1025, 215);
            this.label36.Margin = new System.Windows.Forms.Padding(5, 10, 3, 0);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(117, 20);
            this.label36.TabIndex = 15;
            this.label36.Text = "Filter Options";
            // 
            // lblFilterTitle
            // 
            this.lblFilterTitle.AutoSize = true;
            this.lblFilterTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFilterTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFilterTitle.Location = new System.Drawing.Point(10, 10);
            this.lblFilterTitle.Margin = new System.Windows.Forms.Padding(5, 10, 3, 0);
            this.lblFilterTitle.Name = "lblFilterTitle";
            this.lblFilterTitle.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.lblFilterTitle.Size = new System.Drawing.Size(117, 30);
            this.lblFilterTitle.TabIndex = 1;
            this.lblFilterTitle.Text = "Filter Options";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblPdfTitle);
            this.panel1.Controls.Add(this.tbReportTitle);
            this.panel1.Controls.Add(this.lblOutputTemplate);
            this.panel1.Controls.Add(this.cboOutputType);
            this.panel1.Controls.Add(this.label50);
            this.panel1.Controls.Add(this.cboExportType);
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(10, 306);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(664, 125);
            this.panel1.TabIndex = 21;
            // 
            // lblPdfTitle
            // 
            this.lblPdfTitle.AutoSize = true;
            this.lblPdfTitle.Location = new System.Drawing.Point(149, 11);
            this.lblPdfTitle.Name = "lblPdfTitle";
            this.lblPdfTitle.Size = new System.Drawing.Size(78, 16);
            this.lblPdfTitle.TabIndex = 26;
            this.lblPdfTitle.Text = "Report Title";
            // 
            // tbReportTitle
            // 
            this.tbReportTitle.Location = new System.Drawing.Point(149, 32);
            this.tbReportTitle.Name = "tbReportTitle";
            this.tbReportTitle.Size = new System.Drawing.Size(381, 22);
            this.tbReportTitle.TabIndex = 25;
            // 
            // lblOutputTemplate
            // 
            this.lblOutputTemplate.AutoSize = true;
            this.lblOutputTemplate.Location = new System.Drawing.Point(146, 11);
            this.lblOutputTemplate.Name = "lblOutputTemplate";
            this.lblOutputTemplate.Size = new System.Drawing.Size(101, 16);
            this.lblOutputTemplate.TabIndex = 24;
            this.lblOutputTemplate.Text = "Output template";
            // 
            // cboOutputType
            // 
            this.cboOutputType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOutputType.FormattingEnabled = true;
            this.cboOutputType.Items.AddRange(new object[] {
            "CSV",
            "PDF List",
            "PDF Map"});
            this.cboOutputType.Location = new System.Drawing.Point(149, 30);
            this.cboOutputType.Name = "cboOutputType";
            this.cboOutputType.Size = new System.Drawing.Size(126, 24);
            this.cboOutputType.TabIndex = 23;
            this.cboOutputType.Visible = false;
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(1, 9);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(81, 16);
            this.label50.TabIndex = 22;
            this.label50.Text = "Export Type";
            // 
            // cboExportType
            // 
            this.cboExportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboExportType.FormattingEnabled = true;
            this.cboExportType.Items.AddRange(new object[] {
            "CSV",
            "PDF List",
            "PDF Map"});
            this.cboExportType.Location = new System.Drawing.Point(4, 30);
            this.cboExportType.Name = "cboExportType";
            this.cboExportType.Size = new System.Drawing.Size(126, 24);
            this.cboExportType.TabIndex = 20;
            this.cboExportType.SelectedIndexChanged += new System.EventHandler(this.cboExportType_SelectedIndexChanged);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(4, 61);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 36);
            this.btnExport.TabIndex = 21;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click_1);
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Dock = System.Windows.Forms.DockStyle.Top;
            this.label49.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label49.Location = new System.Drawing.Point(10, 276);
            this.label49.Margin = new System.Windows.Forms.Padding(5, 10, 3, 0);
            this.label49.Name = "label49";
            this.label49.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.label49.Size = new System.Drawing.Size(128, 30);
            this.label49.TabIndex = 20;
            this.label49.Text = "Export Options";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "csv";
            this.saveFileDialog.Filter = "CSV files|*.csv|All files|*.*";
            this.saveFileDialog.Title = "Save File";
            // 
            // savePdfDialog
            // 
            this.savePdfDialog.DefaultExt = "pdf";
            this.savePdfDialog.Filter = "PDF Files|*.pdf";
            // 
            // filterBar
            // 
            this.filterBar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.filterBar.BackColor = System.Drawing.SystemColors.Window;
            this.filterBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.filterBar.Location = new System.Drawing.Point(10, 40);
            this.filterBar.Margin = new System.Windows.Forms.Padding(0);
            this.filterBar.Name = "filterBar";
            this.filterBar.ShowApplyButton = true;
            this.filterBar.ShowLocationOptions = false;
            this.filterBar.ShowSort1 = true;
            this.filterBar.ShowSort2 = true;
            this.filterBar.ShowSort3 = true;
            this.filterBar.Size = new System.Drawing.Size(664, 236);
            this.filterBar.TabIndex = 19;
            // 
            // ReportPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label49);
            this.Controls.Add(this.filterBar);
            this.Controls.Add(this.lblFilterTitle);
            this.Controls.Add(this.label36);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ReportPage";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(684, 480);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFilterTitle;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblOutputTemplate;
        private System.Windows.Forms.ComboBox cboOutputType;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.ComboBox cboExportType;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label label49;
        private ModuleFilterBar filterBar;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.SaveFileDialog savePdfDialog;
        private System.Windows.Forms.Label lblPdfTitle;
        private System.Windows.Forms.TextBox tbReportTitle;
    }
}
