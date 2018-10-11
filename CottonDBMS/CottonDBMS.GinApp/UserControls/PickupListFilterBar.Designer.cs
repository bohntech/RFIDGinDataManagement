namespace CottonDBMS.GinApp.UserControls
{
    partial class PickupListFilterBar
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
            this.components = new System.ComponentModel.Container();
            this.flowLayoutPanel11 = new System.Windows.Forms.FlowLayoutPanel();
            this.label33 = new System.Windows.Forms.Label();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel12 = new System.Windows.Forms.FlowLayoutPanel();
            this.label34 = new System.Windows.Forms.Label();
            this.dpStartDate = new System.Windows.Forms.DateTimePicker();
            this.flowLayoutPanel13 = new System.Windows.Forms.FlowLayoutPanel();
            this.label35 = new System.Windows.Forms.Label();
            this.dpEndDate = new System.Windows.Forms.DateTimePicker();
            this.panelApply = new System.Windows.Forms.Panel();
            this.btnApply = new System.Windows.Forms.Button();
            this.outerFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.label24 = new System.Windows.Forms.Label();
            this.tbClient = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.label25 = new System.Windows.Forms.Label();
            this.tbFarm = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
            this.label26 = new System.Windows.Forms.Label();
            this.tbField = new System.Windows.Forms.TextBox();
            this.panelSort1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label63 = new System.Windows.Forms.Label();
            this.flowLayoutPanel48 = new System.Windows.Forms.FlowLayoutPanel();
            this.cboSortBy1 = new System.Windows.Forms.ComboBox();
            this.cboSortDirection1 = new System.Windows.Forms.ComboBox();
            this.lblDummy = new System.Windows.Forms.Label();
            this.layoutTimer = new System.Windows.Forms.Timer(this.components);
            this.flowLayoutPanel11.SuspendLayout();
            this.flowLayoutPanel12.SuspendLayout();
            this.flowLayoutPanel13.SuspendLayout();
            this.panelApply.SuspendLayout();
            this.outerFlowLayout.SuspendLayout();
            this.flowLayoutPanel.SuspendLayout();
            this.flowLayoutPanel4.SuspendLayout();
            this.flowLayoutPanel5.SuspendLayout();
            this.panelSort1.SuspendLayout();
            this.flowLayoutPanel48.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel11
            // 
            this.flowLayoutPanel11.AutoSize = true;
            this.flowLayoutPanel11.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel11.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel11.Controls.Add(this.label33);
            this.flowLayoutPanel11.Controls.Add(this.cboStatus);
            this.flowLayoutPanel11.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel11.Location = new System.Drawing.Point(618, 0);
            this.flowLayoutPanel11.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.flowLayoutPanel11.Name = "flowLayoutPanel11";
            this.flowLayoutPanel11.Size = new System.Drawing.Size(132, 47);
            this.flowLayoutPanel11.TabIndex = 7;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(3, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(48, 17);
            this.label33.TabIndex = 0;
            this.label33.Text = "Status";
            // 
            // cboStatus
            // 
            this.cboStatus.DisplayMember = "Name";
            this.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Items.AddRange(new object[] {
            "Any",
            "Open",
            "Completed"});
            this.cboStatus.Location = new System.Drawing.Point(3, 20);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(126, 24);
            this.cboStatus.TabIndex = 1;
            // 
            // flowLayoutPanel12
            // 
            this.flowLayoutPanel12.AutoSize = true;
            this.flowLayoutPanel12.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel12.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel12.Controls.Add(this.label34);
            this.flowLayoutPanel12.Controls.Add(this.dpStartDate);
            this.flowLayoutPanel12.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel12.Location = new System.Drawing.Point(753, 0);
            this.flowLayoutPanel12.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.flowLayoutPanel12.Name = "flowLayoutPanel12";
            this.flowLayoutPanel12.Size = new System.Drawing.Size(126, 46);
            this.flowLayoutPanel12.TabIndex = 8;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(3, 0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(115, 17);
            this.label34.TabIndex = 0;
            this.label34.Text = "Date added after";
            // 
            // dpStartDate
            // 
            this.dpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dpStartDate.Location = new System.Drawing.Point(3, 20);
            this.dpStartDate.Name = "dpStartDate";
            this.dpStartDate.Size = new System.Drawing.Size(120, 23);
            this.dpStartDate.TabIndex = 1;
            // 
            // flowLayoutPanel13
            // 
            this.flowLayoutPanel13.AutoSize = true;
            this.flowLayoutPanel13.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel13.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel13.Controls.Add(this.label35);
            this.flowLayoutPanel13.Controls.Add(this.dpEndDate);
            this.flowLayoutPanel13.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel13.Location = new System.Drawing.Point(882, 0);
            this.flowLayoutPanel13.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.flowLayoutPanel13.Name = "flowLayoutPanel13";
            this.flowLayoutPanel13.Size = new System.Drawing.Size(133, 46);
            this.flowLayoutPanel13.TabIndex = 9;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(3, 0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(127, 17);
            this.label35.TabIndex = 0;
            this.label35.Text = "Date added before";
            // 
            // dpEndDate
            // 
            this.dpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dpEndDate.Location = new System.Drawing.Point(3, 20);
            this.dpEndDate.Name = "dpEndDate";
            this.dpEndDate.Size = new System.Drawing.Size(120, 23);
            this.dpEndDate.TabIndex = 2;
            // 
            // panelApply
            // 
            this.panelApply.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelApply.BackColor = System.Drawing.Color.Transparent;
            this.panelApply.Controls.Add(this.btnApply);
            this.panelApply.Location = new System.Drawing.Point(1288, 3);
            this.panelApply.Name = "panelApply";
            this.panelApply.Padding = new System.Windows.Forms.Padding(0, 15, 0, 0);
            this.panelApply.Size = new System.Drawing.Size(94, 44);
            this.panelApply.TabIndex = 10;
            // 
            // btnApply
            // 
            this.btnApply.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnApply.Location = new System.Drawing.Point(0, 14);
            this.btnApply.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(94, 30);
            this.btnApply.TabIndex = 1;
            this.btnApply.Text = "Refresh";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // outerFlowLayout
            // 
            this.outerFlowLayout.AutoSize = true;
            this.outerFlowLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.outerFlowLayout.BackColor = System.Drawing.SystemColors.Window;
            this.outerFlowLayout.Controls.Add(this.flowLayoutPanel);
            this.outerFlowLayout.Controls.Add(this.flowLayoutPanel4);
            this.outerFlowLayout.Controls.Add(this.flowLayoutPanel5);
            this.outerFlowLayout.Controls.Add(this.flowLayoutPanel11);
            this.outerFlowLayout.Controls.Add(this.flowLayoutPanel12);
            this.outerFlowLayout.Controls.Add(this.flowLayoutPanel13);
            this.outerFlowLayout.Controls.Add(this.panelSort1);
            this.outerFlowLayout.Controls.Add(this.panelApply);
            this.outerFlowLayout.Controls.Add(this.lblDummy);
            this.outerFlowLayout.Dock = System.Windows.Forms.DockStyle.Top;
            this.outerFlowLayout.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outerFlowLayout.Location = new System.Drawing.Point(0, 0);
            this.outerFlowLayout.Margin = new System.Windows.Forms.Padding(0);
            this.outerFlowLayout.Name = "outerFlowLayout";
            this.outerFlowLayout.Size = new System.Drawing.Size(1737, 50);
            this.outerFlowLayout.TabIndex = 10;                        
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.AutoSize = true;
            this.flowLayoutPanel.Controls.Add(this.label24);
            this.flowLayoutPanel.Controls.Add(this.tbClient);
            this.flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(203, 46);
            this.flowLayoutPanel.TabIndex = 0;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(3, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(43, 17);
            this.label24.TabIndex = 0;
            this.label24.Text = "Client";
            // 
            // tbClient
            // 
            this.tbClient.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tbClient.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbClient.Location = new System.Drawing.Point(3, 20);
            this.tbClient.Name = "tbClient";
            this.tbClient.Size = new System.Drawing.Size(197, 23);
            this.tbClient.TabIndex = 1;
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.AutoSize = true;
            this.flowLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel4.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel4.Controls.Add(this.label25);
            this.flowLayoutPanel4.Controls.Add(this.tbFarm);
            this.flowLayoutPanel4.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel4.Location = new System.Drawing.Point(206, 0);
            this.flowLayoutPanel4.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(203, 46);
            this.flowLayoutPanel4.TabIndex = 1;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(3, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(40, 17);
            this.label25.TabIndex = 0;
            this.label25.Text = "Farm";
            // 
            // tbFarm
            // 
            this.tbFarm.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tbFarm.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbFarm.Location = new System.Drawing.Point(3, 20);
            this.tbFarm.Name = "tbFarm";
            this.tbFarm.Size = new System.Drawing.Size(197, 23);
            this.tbFarm.TabIndex = 1;
            // 
            // flowLayoutPanel5
            // 
            this.flowLayoutPanel5.AutoSize = true;
            this.flowLayoutPanel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel5.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel5.Controls.Add(this.label26);
            this.flowLayoutPanel5.Controls.Add(this.tbField);
            this.flowLayoutPanel5.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel5.Location = new System.Drawing.Point(412, 0);
            this.flowLayoutPanel5.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.flowLayoutPanel5.Name = "flowLayoutPanel5";
            this.flowLayoutPanel5.Size = new System.Drawing.Size(203, 46);
            this.flowLayoutPanel5.TabIndex = 2;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(3, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(38, 17);
            this.label26.TabIndex = 0;
            this.label26.Text = "Field";
            // 
            // tbField
            // 
            this.tbField.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tbField.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbField.Location = new System.Drawing.Point(3, 20);
            this.tbField.Name = "tbField";
            this.tbField.Size = new System.Drawing.Size(197, 23);
            this.tbField.TabIndex = 1;
            // 
            // panelSort1
            // 
            this.panelSort1.AutoSize = true;
            this.panelSort1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelSort1.BackColor = System.Drawing.Color.Transparent;
            this.panelSort1.Controls.Add(this.label63);
            this.panelSort1.Controls.Add(this.flowLayoutPanel48);
            this.panelSort1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.panelSort1.Location = new System.Drawing.Point(1018, 0);
            this.panelSort1.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.panelSort1.Name = "panelSort1";
            this.panelSort1.Size = new System.Drawing.Size(264, 47);
            this.panelSort1.TabIndex = 13;
            this.panelSort1.WrapContents = false;
            // 
            // label63
            // 
            this.label63.AutoSize = true;
            this.label63.Location = new System.Drawing.Point(3, 0);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(53, 17);
            this.label63.TabIndex = 0;
            this.label63.Text = "Sort by";
            // 
            // flowLayoutPanel48
            // 
            this.flowLayoutPanel48.AutoSize = true;
            this.flowLayoutPanel48.Controls.Add(this.cboSortBy1);
            this.flowLayoutPanel48.Controls.Add(this.cboSortDirection1);
            this.flowLayoutPanel48.Location = new System.Drawing.Point(0, 17);
            this.flowLayoutPanel48.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel48.Name = "flowLayoutPanel48";
            this.flowLayoutPanel48.Size = new System.Drawing.Size(264, 30);
            this.flowLayoutPanel48.TabIndex = 10;
            this.flowLayoutPanel48.WrapContents = false;
            // 
            // cboSortBy1
            // 
            this.cboSortBy1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSortBy1.FormattingEnabled = true;
            this.cboSortBy1.Items.AddRange(new object[] {
            "Name",
            "Client",
            "Farm",
            "Field",
            "Status",
            "Timestamp"});
            this.cboSortBy1.Location = new System.Drawing.Point(3, 3);
            this.cboSortBy1.Name = "cboSortBy1";
            this.cboSortBy1.Size = new System.Drawing.Size(126, 24);
            this.cboSortBy1.TabIndex = 2;
            // 
            // cboSortDirection1
            // 
            this.cboSortDirection1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSortDirection1.FormattingEnabled = true;
            this.cboSortDirection1.Items.AddRange(new object[] {
            "ascending order",
            "descending order"});
            this.cboSortDirection1.Location = new System.Drawing.Point(135, 3);
            this.cboSortDirection1.Name = "cboSortDirection1";
            this.cboSortDirection1.Size = new System.Drawing.Size(126, 24);
            this.cboSortDirection1.TabIndex = 3;
            // 
            // lblDummy
            // 
            this.outerFlowLayout.SetFlowBreak(this.lblDummy, true);
            this.lblDummy.Location = new System.Drawing.Point(1385, 0);
            this.lblDummy.Margin = new System.Windows.Forms.Padding(0);
            this.lblDummy.Name = "lblDummy";
            this.lblDummy.Size = new System.Drawing.Size(0, 0);
            this.lblDummy.TabIndex = 14;
            // 
            // layoutTimer
            // 
            this.layoutTimer.Enabled = true;
            this.layoutTimer.Interval = 500;
            this.layoutTimer.Tick += new System.EventHandler(this.layoutTimer_Tick);
            // 
            // PickupListFilterBar
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.outerFlowLayout);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "PickupListFilterBar";
            this.Size = new System.Drawing.Size(1737, 56);            
            this.flowLayoutPanel11.ResumeLayout(false);
            this.flowLayoutPanel11.PerformLayout();
            this.flowLayoutPanel12.ResumeLayout(false);
            this.flowLayoutPanel12.PerformLayout();
            this.flowLayoutPanel13.ResumeLayout(false);
            this.flowLayoutPanel13.PerformLayout();
            this.panelApply.ResumeLayout(false);
            this.outerFlowLayout.ResumeLayout(false);
            this.outerFlowLayout.PerformLayout();
            this.flowLayoutPanel.ResumeLayout(false);
            this.flowLayoutPanel.PerformLayout();
            this.flowLayoutPanel4.ResumeLayout(false);
            this.flowLayoutPanel4.PerformLayout();
            this.flowLayoutPanel5.ResumeLayout(false);
            this.flowLayoutPanel5.PerformLayout();
            this.panelSort1.ResumeLayout(false);
            this.panelSort1.PerformLayout();
            this.flowLayoutPanel48.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel11;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel12;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.DateTimePicker dpStartDate;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel13;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.DateTimePicker dpEndDate;
        private System.Windows.Forms.Panel panelApply;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.FlowLayoutPanel outerFlowLayout;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox tbClient;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox tbFarm;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox tbField;
        private System.Windows.Forms.FlowLayoutPanel panelSort1;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel48;
        private System.Windows.Forms.ComboBox cboSortBy1;
        private System.Windows.Forms.ComboBox cboSortDirection1;
        private System.Windows.Forms.Label lblDummy;
        private System.Windows.Forms.Timer layoutTimer;
    }
}
