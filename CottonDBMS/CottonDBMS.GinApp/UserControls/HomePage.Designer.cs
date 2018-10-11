namespace CottonDBMS.GinApp.UserControls
{
    partial class HomePage
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
            this.groupBoxImportSummary = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnImport = new System.Windows.Forms.Button();
            this.lblLastImportTimeValue = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblLastStatus = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.groupBoxSummary = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblTotalLoadsValue = new System.Windows.Forms.Label();
            this.lblTotalLoads = new System.Windows.Forms.Label();
            this.lblTotalModulesValue = new System.Windows.Forms.Label();
            this.lblTotalModules = new System.Windows.Forms.Label();
            this.lblTotalModulesInField = new System.Windows.Forms.Label();
            this.lblTotalLoadsInField = new System.Windows.Forms.Label();
            this.lblModulesOnYard = new System.Windows.Forms.Label();
            this.lblModulesGinned = new System.Windows.Forms.Label();
            this.lblTotalModulesInFieldValue = new System.Windows.Forms.Label();
            this.lblTotalLoadsInFieldValue = new System.Windows.Forms.Label();
            this.lblModulesOnYardValue = new System.Windows.Forms.Label();
            this.lblModulesGinnedValue = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.importTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelSync = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnDataSync = new System.Windows.Forms.Button();
            this.btnViewLog = new System.Windows.Forms.Button();
            this.lblLastRunTime = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblSyncStatus = new System.Windows.Forms.Label();
            this.syncStatusUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.backupTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBoxImportSummary.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.btnFlowPanel.SuspendLayout();
            this.groupBoxSummary.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanelSync.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxImportSummary
            // 
            this.groupBoxImportSummary.Controls.Add(this.tableLayoutPanel2);
            this.groupBoxImportSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxImportSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxImportSummary.Location = new System.Drawing.Point(0, 232);
            this.groupBoxImportSummary.Name = "groupBoxImportSummary";
            this.groupBoxImportSummary.Size = new System.Drawing.Size(1109, 121);
            this.groupBoxImportSummary.TabIndex = 3;
            this.groupBoxImportSummary.TabStop = false;
            this.groupBoxImportSummary.Text = "Import Summary";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.84768F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 87.15232F));
            this.tableLayoutPanel2.Controls.Add(this.btnFlowPanel, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.lblLastImportTimeValue, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblLastStatus, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lblStatus, 1, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 25);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1103, 95);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // btnFlowPanel
            // 
            this.btnFlowPanel.AutoSize = true;
            this.btnFlowPanel.Controls.Add(this.btnImport);
            this.btnFlowPanel.Location = new System.Drawing.Point(141, 50);
            this.btnFlowPanel.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.btnFlowPanel.Name = "btnFlowPanel";
            this.btnFlowPanel.Size = new System.Drawing.Size(164, 42);
            this.btnFlowPanel.TabIndex = 4;
            // 
            // btnImport
            // 
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImport.Location = new System.Drawing.Point(3, 3);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(158, 36);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "Run File Imports";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // lblLastImportTimeValue
            // 
            this.lblLastImportTimeValue.AutoSize = true;
            this.lblLastImportTimeValue.Location = new System.Drawing.Point(144, 0);
            this.lblLastImportTimeValue.Name = "lblLastImportTimeValue";
            this.lblLastImportTimeValue.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.lblLastImportTimeValue.Size = new System.Drawing.Size(23, 27);
            this.lblLastImportTimeValue.TabIndex = 1;
            this.lblLastImportTimeValue.Text = "--";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.label4.Size = new System.Drawing.Size(135, 27);
            this.label4.TabIndex = 0;
            this.label4.Text = "Last import:";
            // 
            // lblLastStatus
            // 
            this.lblLastStatus.AutoSize = true;
            this.lblLastStatus.Location = new System.Drawing.Point(3, 27);
            this.lblLastStatus.Name = "lblLastStatus";
            this.lblLastStatus.Size = new System.Drawing.Size(95, 17);
            this.lblLastStatus.TabIndex = 4;
            this.lblLastStatus.Text = "Import Status:";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(144, 27);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(18, 17);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "--";
            // 
            // groupBoxSummary
            // 
            this.groupBoxSummary.AutoSize = true;
            this.groupBoxSummary.Controls.Add(this.tableLayoutPanel1);
            this.groupBoxSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxSummary.Location = new System.Drawing.Point(0, 0);
            this.groupBoxSummary.Name = "groupBoxSummary";
            this.groupBoxSummary.Size = new System.Drawing.Size(1109, 232);
            this.groupBoxSummary.TabIndex = 2;
            this.groupBoxSummary.TabStop = false;
            this.groupBoxSummary.Text = "System Summary";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 181F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lblTotalLoadsValue, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblTotalLoads, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblTotalModulesValue, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblTotalModules, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblTotalModulesInField, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblTotalLoadsInField, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblModulesOnYard, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblModulesGinned, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblTotalModulesInFieldValue, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblTotalLoadsInFieldValue, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblModulesOnYardValue, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblModulesGinnedValue, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnRefresh, 1, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 25);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1103, 204);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblTotalLoadsValue
            // 
            this.lblTotalLoadsValue.AutoSize = true;
            this.lblTotalLoadsValue.Location = new System.Drawing.Point(184, 27);
            this.lblTotalLoadsValue.Name = "lblTotalLoadsValue";
            this.lblTotalLoadsValue.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.lblTotalLoadsValue.Size = new System.Drawing.Size(23, 27);
            this.lblTotalLoadsValue.TabIndex = 3;
            this.lblTotalLoadsValue.Text = "--";
            // 
            // lblTotalLoads
            // 
            this.lblTotalLoads.AutoSize = true;
            this.lblTotalLoads.Location = new System.Drawing.Point(3, 27);
            this.lblTotalLoads.Name = "lblTotalLoads";
            this.lblTotalLoads.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.lblTotalLoads.Size = new System.Drawing.Size(150, 27);
            this.lblTotalLoads.TabIndex = 2;
            this.lblTotalLoads.Text = "Total loads in system:";
            // 
            // lblTotalModulesValue
            // 
            this.lblTotalModulesValue.AutoSize = true;
            this.lblTotalModulesValue.Location = new System.Drawing.Point(184, 0);
            this.lblTotalModulesValue.Name = "lblTotalModulesValue";
            this.lblTotalModulesValue.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.lblTotalModulesValue.Size = new System.Drawing.Size(23, 27);
            this.lblTotalModulesValue.TabIndex = 1;
            this.lblTotalModulesValue.Text = "--";
            // 
            // lblTotalModules
            // 
            this.lblTotalModules.AutoSize = true;
            this.lblTotalModules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotalModules.Location = new System.Drawing.Point(3, 0);
            this.lblTotalModules.Name = "lblTotalModules";
            this.lblTotalModules.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.lblTotalModules.Size = new System.Drawing.Size(175, 27);
            this.lblTotalModules.TabIndex = 0;
            this.lblTotalModules.Text = "Total modules in system:";
            // 
            // lblTotalModulesInField
            // 
            this.lblTotalModulesInField.AutoSize = true;
            this.lblTotalModulesInField.Location = new System.Drawing.Point(3, 54);
            this.lblTotalModulesInField.Name = "lblTotalModulesInField";
            this.lblTotalModulesInField.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.lblTotalModulesInField.Size = new System.Drawing.Size(139, 27);
            this.lblTotalModulesInField.TabIndex = 4;
            this.lblTotalModulesInField.Text = "Modules in the field:";
            // 
            // lblTotalLoadsInField
            // 
            this.lblTotalLoadsInField.AutoSize = true;
            this.lblTotalLoadsInField.Location = new System.Drawing.Point(3, 81);
            this.lblTotalLoadsInField.Name = "lblTotalLoadsInField";
            this.lblTotalLoadsInField.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.lblTotalLoadsInField.Size = new System.Drawing.Size(125, 27);
            this.lblTotalLoadsInField.TabIndex = 5;
            this.lblTotalLoadsInField.Text = "Loads in the field:";
            // 
            // lblModulesOnYard
            // 
            this.lblModulesOnYard.AutoSize = true;
            this.lblModulesOnYard.Location = new System.Drawing.Point(3, 108);
            this.lblModulesOnYard.Name = "lblModulesOnYard";
            this.lblModulesOnYard.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.lblModulesOnYard.Size = new System.Drawing.Size(146, 27);
            this.lblModulesOnYard.TabIndex = 7;
            this.lblModulesOnYard.Text = "Modules on the yard:";
            // 
            // lblModulesGinned
            // 
            this.lblModulesGinned.AutoSize = true;
            this.lblModulesGinned.Location = new System.Drawing.Point(3, 135);
            this.lblModulesGinned.Name = "lblModulesGinned";
            this.lblModulesGinned.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.lblModulesGinned.Size = new System.Drawing.Size(117, 27);
            this.lblModulesGinned.TabIndex = 6;
            this.lblModulesGinned.Text = "Modules ginned:";
            // 
            // lblTotalModulesInFieldValue
            // 
            this.lblTotalModulesInFieldValue.AutoSize = true;
            this.lblTotalModulesInFieldValue.Location = new System.Drawing.Point(184, 54);
            this.lblTotalModulesInFieldValue.Name = "lblTotalModulesInFieldValue";
            this.lblTotalModulesInFieldValue.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.lblTotalModulesInFieldValue.Size = new System.Drawing.Size(23, 27);
            this.lblTotalModulesInFieldValue.TabIndex = 8;
            this.lblTotalModulesInFieldValue.Text = "--";
            // 
            // lblTotalLoadsInFieldValue
            // 
            this.lblTotalLoadsInFieldValue.AutoSize = true;
            this.lblTotalLoadsInFieldValue.Location = new System.Drawing.Point(184, 81);
            this.lblTotalLoadsInFieldValue.Name = "lblTotalLoadsInFieldValue";
            this.lblTotalLoadsInFieldValue.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.lblTotalLoadsInFieldValue.Size = new System.Drawing.Size(23, 27);
            this.lblTotalLoadsInFieldValue.TabIndex = 9;
            this.lblTotalLoadsInFieldValue.Text = "--";
            // 
            // lblModulesOnYardValue
            // 
            this.lblModulesOnYardValue.AutoSize = true;
            this.lblModulesOnYardValue.Location = new System.Drawing.Point(184, 108);
            this.lblModulesOnYardValue.Name = "lblModulesOnYardValue";
            this.lblModulesOnYardValue.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.lblModulesOnYardValue.Size = new System.Drawing.Size(23, 27);
            this.lblModulesOnYardValue.TabIndex = 10;
            this.lblModulesOnYardValue.Text = "--";
            // 
            // lblModulesGinnedValue
            // 
            this.lblModulesGinnedValue.AutoSize = true;
            this.lblModulesGinnedValue.Location = new System.Drawing.Point(184, 135);
            this.lblModulesGinnedValue.Name = "lblModulesGinnedValue";
            this.lblModulesGinnedValue.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.lblModulesGinnedValue.Size = new System.Drawing.Size(23, 27);
            this.lblModulesGinnedValue.TabIndex = 11;
            this.lblModulesGinnedValue.Text = "--";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(184, 165);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(146, 36);
            this.btnRefresh.TabIndex = 13;
            this.btnRefresh.Text = "Refresh Counts";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.tbnRefresh_Click);
            // 
            // importTimer
            // 
            this.importTimer.Interval = 20000;
            this.importTimer.Tick += new System.EventHandler(this.importTimer_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanelSync);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 353);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1109, 121);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data Sync";
            // 
            // tableLayoutPanelSync
            // 
            this.tableLayoutPanelSync.AutoSize = true;
            this.tableLayoutPanelSync.ColumnCount = 2;
            this.tableLayoutPanelSync.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.84768F));
            this.tableLayoutPanelSync.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 87.15232F));
            this.tableLayoutPanelSync.Controls.Add(this.flowLayoutPanel1, 1, 2);
            this.tableLayoutPanelSync.Controls.Add(this.lblLastRunTime, 1, 0);
            this.tableLayoutPanelSync.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanelSync.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanelSync.Controls.Add(this.lblSyncStatus, 1, 1);
            this.tableLayoutPanelSync.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelSync.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelSync.Location = new System.Drawing.Point(3, 25);
            this.tableLayoutPanelSync.Name = "tableLayoutPanelSync";
            this.tableLayoutPanelSync.RowCount = 3;
            this.tableLayoutPanelSync.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSync.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelSync.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSync.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelSync.Size = new System.Drawing.Size(1103, 95);
            this.tableLayoutPanelSync.TabIndex = 1;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.btnDataSync);
            this.flowLayoutPanel1.Controls.Add(this.btnViewLog);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(141, 50);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(345, 42);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // btnDataSync
            // 
            this.btnDataSync.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDataSync.Location = new System.Drawing.Point(3, 3);
            this.btnDataSync.Name = "btnDataSync";
            this.btnDataSync.Size = new System.Drawing.Size(158, 36);
            this.btnDataSync.TabIndex = 2;
            this.btnDataSync.Text = "Run Data Sync";
            this.btnDataSync.UseVisualStyleBackColor = true;
            this.btnDataSync.Click += new System.EventHandler(this.btnDataSync_Click);
            // 
            // btnViewLog
            // 
            this.btnViewLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewLog.Location = new System.Drawing.Point(167, 3);
            this.btnViewLog.Name = "btnViewLog";
            this.btnViewLog.Size = new System.Drawing.Size(175, 36);
            this.btnViewLog.TabIndex = 3;
            this.btnViewLog.Text = "View Application Log";
            this.btnViewLog.UseVisualStyleBackColor = true;
            this.btnViewLog.Click += new System.EventHandler(this.btnOpenLog_Click);
            // 
            // lblLastRunTime
            // 
            this.lblLastRunTime.AutoSize = true;
            this.lblLastRunTime.Location = new System.Drawing.Point(144, 0);
            this.lblLastRunTime.Name = "lblLastRunTime";
            this.lblLastRunTime.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.lblLastRunTime.Size = new System.Drawing.Size(23, 27);
            this.lblLastRunTime.TabIndex = 1;
            this.lblLastRunTime.Text = "--";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.label2.Size = new System.Drawing.Size(135, 27);
            this.label2.TabIndex = 0;
            this.label2.Text = "Last run time:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Sync Status:";
            // 
            // lblSyncStatus
            // 
            this.lblSyncStatus.AutoSize = true;
            this.lblSyncStatus.Location = new System.Drawing.Point(144, 27);
            this.lblSyncStatus.Name = "lblSyncStatus";
            this.lblSyncStatus.Size = new System.Drawing.Size(18, 17);
            this.lblSyncStatus.TabIndex = 5;
            this.lblSyncStatus.Text = "--";
            // 
            // syncStatusUpdateTimer
            // 
            this.syncStatusUpdateTimer.Interval = 500;
            this.syncStatusUpdateTimer.Tick += new System.EventHandler(this.syncStatusUpdateTimer_Tick);
            // 
            // backupTimer
            // 
            this.backupTimer.Interval = 7200000;
            this.backupTimer.Tick += new System.EventHandler(this.backupTimer_Tick);
            // 
            // HomePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxImportSummary);
            this.Controls.Add(this.groupBoxSummary);
            this.Name = "HomePage";
            this.Size = new System.Drawing.Size(1109, 608);
            this.groupBoxImportSummary.ResumeLayout(false);
            this.groupBoxImportSummary.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.btnFlowPanel.ResumeLayout(false);
            this.groupBoxSummary.ResumeLayout(false);
            this.groupBoxSummary.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanelSync.ResumeLayout(false);
            this.tableLayoutPanelSync.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxImportSummary;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel btnFlowPanel;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Label lblLastImportTimeValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblLastStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.GroupBox groupBoxSummary;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblTotalLoadsValue;
        private System.Windows.Forms.Label lblTotalLoads;
        private System.Windows.Forms.Label lblTotalModulesValue;
        private System.Windows.Forms.Label lblTotalModules;
        private System.Windows.Forms.Label lblTotalModulesInField;
        private System.Windows.Forms.Label lblTotalLoadsInField;
        private System.Windows.Forms.Label lblModulesOnYard;
        private System.Windows.Forms.Label lblModulesGinned;
        private System.Windows.Forms.Label lblTotalModulesInFieldValue;
        private System.Windows.Forms.Label lblTotalLoadsInFieldValue;
        private System.Windows.Forms.Label lblModulesOnYardValue;
        private System.Windows.Forms.Label lblModulesGinnedValue;
        private System.Windows.Forms.Timer importTimer;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSync;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnDataSync;
        private System.Windows.Forms.Label lblLastRunTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblSyncStatus;
        private System.Windows.Forms.Button btnViewLog;
        private System.Windows.Forms.Timer syncStatusUpdateTimer;
        private System.Windows.Forms.Timer backupTimer;
    }
}
