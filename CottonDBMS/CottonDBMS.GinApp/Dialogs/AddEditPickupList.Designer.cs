namespace CottonDBMS.GinApp.Dialogs
{
    partial class AddEditPickupList
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
            this.pnlModuleStep = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tbListname = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblCheckInfo = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.availableModulesGridView = new System.Windows.Forms.DataGridView();
            this.Selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SerialNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Latitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Longitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mapBrowser = new System.Windows.Forms.WebBrowser();
            this.pnlTruckStep = new System.Windows.Forms.Panel();
            this.gridViewTrucks = new System.Windows.Forms.DataGridView();
            this.Select = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.TruckName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTruckInstructions = new System.Windows.Forms.Label();
            this.btnSaveAndClose = new System.Windows.Forms.Button();
            this.btnCancelClose = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.fieldSelector = new CottonDBMS.GinApp.UserControls.FieldSelector();
            this.clientSelector = new CottonDBMS.GinApp.UserControls.ClientSelector();
            this.farmSelector = new CottonDBMS.GinApp.UserControls.FarmSelector();
            this.label1 = new System.Windows.Forms.Label();
            this.cboDestination = new System.Windows.Forms.ComboBox();
            this.pnlModuleStep.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.availableModulesGridView)).BeginInit();
            this.pnlTruckStep.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTrucks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlModuleStep
            // 
            this.pnlModuleStep.Controls.Add(this.fieldSelector);
            this.pnlModuleStep.Controls.Add(this.clientSelector);
            this.pnlModuleStep.Controls.Add(this.farmSelector);
            this.pnlModuleStep.Controls.Add(this.tableLayoutPanel2);
            this.pnlModuleStep.Controls.Add(this.lblCheckInfo);
            this.pnlModuleStep.Controls.Add(this.btnCancel);
            this.pnlModuleStep.Controls.Add(this.btnNext);
            this.pnlModuleStep.Controls.Add(this.availableModulesGridView);
            this.pnlModuleStep.Controls.Add(this.mapBrowser);
            this.pnlModuleStep.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlModuleStep.Location = new System.Drawing.Point(0, 0);
            this.pnlModuleStep.Name = "pnlModuleStep";
            this.pnlModuleStep.Size = new System.Drawing.Size(1070, 694);
            this.pnlModuleStep.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.tbListname, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label1, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.cboDestination, 3, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(24, 10);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(655, 28);
            this.tableLayoutPanel2.TabIndex = 16;
            // 
            // tbListname
            // 
            this.tbListname.Location = new System.Drawing.Point(89, 3);
            this.tbListname.Margin = new System.Windows.Forms.Padding(3, 3, 25, 3);
            this.tbListname.Name = "tbListname";
            this.tbListname.Size = new System.Drawing.Size(225, 22);
            this.tbListname.TabIndex = 1;
            this.tbListname.Validating += new System.ComponentModel.CancelEventHandler(this.tbListname_Validating);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(3, 3);
            this.label6.Margin = new System.Windows.Forms.Padding(3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 22);
            this.label6.TabIndex = 4;
            this.label6.Text = "List name:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCheckInfo
            // 
            this.lblCheckInfo.AutoSize = true;
            this.lblCheckInfo.Location = new System.Drawing.Point(21, 111);
            this.lblCheckInfo.Name = "lblCheckInfo";
            this.lblCheckInfo.Size = new System.Drawing.Size(498, 16);
            this.lblCheckInfo.TabIndex = 15;
            this.lblCheckInfo.Text = "Check off the modules from the list below you would like to include in this pick " +
    "up list.";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(104, 650);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 36);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(23, 650);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 36);
            this.btnNext.TabIndex = 7;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // availableModulesGridView
            // 
            this.availableModulesGridView.AllowUserToAddRows = false;
            this.availableModulesGridView.AllowUserToDeleteRows = false;
            this.availableModulesGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.availableModulesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.availableModulesGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Selected,
            this.Id,
            this.SerialNumber,
            this.StatusName,
            this.Latitude,
            this.Longitude});
            this.availableModulesGridView.Location = new System.Drawing.Point(23, 130);
            this.availableModulesGridView.MultiSelect = false;
            this.availableModulesGridView.Name = "availableModulesGridView";
            this.availableModulesGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.availableModulesGridView.Size = new System.Drawing.Size(1034, 514);
            this.availableModulesGridView.TabIndex = 5;
            this.availableModulesGridView.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.availableModulesGridView_CellBeginEdit);
            this.availableModulesGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.availableModulesGridView_DataBindingComplete);
            // 
            // Selected
            // 
            this.Selected.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Selected.HeaderText = " ";
            this.Selected.MinimumWidth = 50;
            this.Selected.Name = "Selected";
            this.Selected.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Selected.Width = 50;
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Visible = false;
            // 
            // SerialNumber
            // 
            this.SerialNumber.DataPropertyName = "Name";
            this.SerialNumber.HeaderText = "Serial #";
            this.SerialNumber.Name = "SerialNumber";
            this.SerialNumber.ReadOnly = true;
            // 
            // StatusName
            // 
            this.StatusName.DataPropertyName = "StatusName";
            this.StatusName.HeaderText = "Status";
            this.StatusName.Name = "StatusName";
            // 
            // Latitude
            // 
            this.Latitude.DataPropertyName = "Latitude";
            this.Latitude.HeaderText = "Latitude";
            this.Latitude.Name = "Latitude";
            this.Latitude.ReadOnly = true;
            // 
            // Longitude
            // 
            this.Longitude.DataPropertyName = "Longitude";
            this.Longitude.HeaderText = "Longitude";
            this.Longitude.Name = "Longitude";
            this.Longitude.ReadOnly = true;
            // 
            // mapBrowser
            // 
            this.mapBrowser.IsWebBrowserContextMenuEnabled = false;
            this.mapBrowser.Location = new System.Drawing.Point(26, 130);
            this.mapBrowser.Margin = new System.Windows.Forms.Padding(0);
            this.mapBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.mapBrowser.Name = "mapBrowser";
            this.mapBrowser.ScrollBarsEnabled = false;
            this.mapBrowser.Size = new System.Drawing.Size(1032, 514);
            this.mapBrowser.TabIndex = 13;
            this.mapBrowser.Url = new System.Uri("http://EmbeddedMap.html", System.UriKind.Absolute);
            this.mapBrowser.Visible = false;
            this.mapBrowser.WebBrowserShortcutsEnabled = false;
            // 
            // pnlTruckStep
            // 
            this.pnlTruckStep.Controls.Add(this.gridViewTrucks);
            this.pnlTruckStep.Controls.Add(this.lblTruckInstructions);
            this.pnlTruckStep.Controls.Add(this.btnSaveAndClose);
            this.pnlTruckStep.Controls.Add(this.btnCancelClose);
            this.pnlTruckStep.Controls.Add(this.btnBack);
            this.pnlTruckStep.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlTruckStep.Location = new System.Drawing.Point(1070, 0);
            this.pnlTruckStep.Name = "pnlTruckStep";
            this.pnlTruckStep.Size = new System.Drawing.Size(1061, 694);
            this.pnlTruckStep.TabIndex = 2;
            this.pnlTruckStep.Visible = false;
            // 
            // gridViewTrucks
            // 
            this.gridViewTrucks.AllowUserToAddRows = false;
            this.gridViewTrucks.AllowUserToDeleteRows = false;
            this.gridViewTrucks.AllowUserToResizeRows = false;
            this.gridViewTrucks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridViewTrucks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Select,
            this.TruckName});
            this.gridViewTrucks.Location = new System.Drawing.Point(18, 53);
            this.gridViewTrucks.MultiSelect = false;
            this.gridViewTrucks.Name = "gridViewTrucks";
            this.gridViewTrucks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridViewTrucks.Size = new System.Drawing.Size(1034, 583);
            this.gridViewTrucks.TabIndex = 9;
            this.gridViewTrucks.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.gridViewTrucks_CellBeginEdit);
            this.gridViewTrucks.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.gridViewTrucks_DataBindingComplete);
            // 
            // Select
            // 
            this.Select.HeaderText = "";
            this.Select.MinimumWidth = 50;
            this.Select.Name = "Select";
            this.Select.Width = 50;
            // 
            // TruckName
            // 
            this.TruckName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TruckName.DataPropertyName = "Name";
            this.TruckName.HeaderText = "Truck";
            this.TruckName.Name = "TruckName";
            this.TruckName.ReadOnly = true;
            // 
            // lblTruckInstructions
            // 
            this.lblTruckInstructions.AutoSize = true;
            this.lblTruckInstructions.Location = new System.Drawing.Point(17, 18);
            this.lblTruckInstructions.Name = "lblTruckInstructions";
            this.lblTruckInstructions.Size = new System.Drawing.Size(317, 16);
            this.lblTruckInstructions.TabIndex = 8;
            this.lblTruckInstructions.Text = "Select the trucks below that this list should be sent to:";
            // 
            // btnSaveAndClose
            // 
            this.btnSaveAndClose.Location = new System.Drawing.Point(98, 642);
            this.btnSaveAndClose.Name = "btnSaveAndClose";
            this.btnSaveAndClose.Padding = new System.Windows.Forms.Padding(3);
            this.btnSaveAndClose.Size = new System.Drawing.Size(75, 36);
            this.btnSaveAndClose.TabIndex = 6;
            this.btnSaveAndClose.Text = "Save";
            this.btnSaveAndClose.UseVisualStyleBackColor = true;
            this.btnSaveAndClose.Click += new System.EventHandler(this.btnSaveAndClose_Click);
            // 
            // btnCancelClose
            // 
            this.btnCancelClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelClose.Location = new System.Drawing.Point(179, 642);
            this.btnCancelClose.Name = "btnCancelClose";
            this.btnCancelClose.Padding = new System.Windows.Forms.Padding(3);
            this.btnCancelClose.Size = new System.Drawing.Size(75, 36);
            this.btnCancelClose.TabIndex = 7;
            this.btnCancelClose.Text = "Cancel";
            this.btnCancelClose.UseVisualStyleBackColor = true;
            this.btnCancelClose.Click += new System.EventHandler(this.btnCancelClose_Click);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(17, 642);
            this.btnBack.Name = "btnBack";
            this.btnBack.Padding = new System.Windows.Forms.Padding(3);
            this.btnBack.Size = new System.Drawing.Size(75, 36);
            this.btnBack.TabIndex = 5;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // fieldSelector
            // 
            this.fieldSelector.AutoSize = true;
            this.fieldSelector.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.fieldSelector.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fieldSelector.FormErrorProvider = null;
            this.fieldSelector.HasError = false;
            this.fieldSelector.InputColumnWidth = 225;
            this.fieldSelector.LabelColumnWidth = 75;
            this.fieldSelector.Location = new System.Drawing.Point(696, 48);
            this.fieldSelector.Name = "fieldSelector";
            this.fieldSelector.Size = new System.Drawing.Size(329, 57);
            this.fieldSelector.TabIndex = 4;
            this.fieldSelector.Visible = false;
            this.fieldSelector.SelectionChanged += new System.EventHandler(this.fieldSelector_SelectionChanged);
            // 
            // clientSelector
            // 
            this.clientSelector.AutoSize = true;
            this.clientSelector.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.clientSelector.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clientSelector.HasError = false;
            this.clientSelector.InputColumnWidth = 225;
            this.clientSelector.LabelColumnWidth = 80;
            this.clientSelector.Location = new System.Drawing.Point(24, 42);
            this.clientSelector.Name = "clientSelector";
            this.clientSelector.Size = new System.Drawing.Size(338, 68);
            this.clientSelector.TabIndex = 2;
            this.clientSelector.SelectionChanged += new System.EventHandler(this.clientSelector_SelectionChanged);
            // 
            // farmSelector
            // 
            this.farmSelector.AutoSize = true;
            this.farmSelector.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.farmSelector.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.farmSelector.FormErrorProvider = null;
            this.farmSelector.HasError = false;
            this.farmSelector.InputColumnWidth = 225;
            this.farmSelector.LabelColumnWidth = 75;
            this.farmSelector.Location = new System.Drawing.Point(363, 46);
            this.farmSelector.Margin = new System.Windows.Forms.Padding(4);
            this.farmSelector.Name = "farmSelector";
            this.farmSelector.Size = new System.Drawing.Size(329, 58);
            this.farmSelector.TabIndex = 3;
            this.farmSelector.Visible = false;
            this.farmSelector.SelectionChanged += new System.EventHandler(this.farmSelector_SelectionChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(342, 3);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 22);
            this.label1.TabIndex = 5;
            this.label1.Text = "Destination:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboDestination
            // 
            this.cboDestination.FormattingEnabled = true;
            this.cboDestination.Location = new System.Drawing.Point(428, 3);
            this.cboDestination.Name = "cboDestination";
            this.cboDestination.Size = new System.Drawing.Size(224, 24);
            this.cboDestination.TabIndex = 6;
            this.cboDestination.SelectedIndexChanged += new System.EventHandler(this.cboDestination_SelectedIndexChanged);
            // 
            // AddEditPickupList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1083, 694);
            this.Controls.Add(this.pnlTruckStep);
            this.Controls.Add(this.pnlModuleStep);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddEditPickupList";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AddEditPickupList";
            this.Shown += new System.EventHandler(this.AddEditPickupList_Shown);
            this.pnlModuleStep.ResumeLayout(false);
            this.pnlModuleStep.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.availableModulesGridView)).EndInit();
            this.pnlTruckStep.ResumeLayout(false);
            this.pnlTruckStep.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTrucks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlModuleStep;
        private System.Windows.Forms.Label lblCheckInfo;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.DataGridView availableModulesGridView;
        private System.Windows.Forms.WebBrowser mapBrowser;
        private System.Windows.Forms.Panel pnlTruckStep;
        private System.Windows.Forms.DataGridView gridViewTrucks;
        private System.Windows.Forms.Label lblTruckInstructions;
        private System.Windows.Forms.Button btnCancelClose;
        private System.Windows.Forms.Button btnSaveAndClose;
        private System.Windows.Forms.Button btnBack;
        private UserControls.FarmSelector farmSelector;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox tbListname;
        private System.Windows.Forms.Label label6;
        private UserControls.FieldSelector fieldSelector;
        private UserControls.ClientSelector clientSelector;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Select;
        private System.Windows.Forms.DataGridViewTextBoxColumn TruckName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Selected;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn SerialNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatusName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Latitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn Longitude;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboDestination;
    }
}