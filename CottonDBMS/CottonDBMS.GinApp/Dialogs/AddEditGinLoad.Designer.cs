namespace CottonDBMS.GinApp.Dialogs
{
    partial class AddEditGinLoad
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
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblImportedLoadNumber = new System.Windows.Forms.Label();
            this.tbVariety = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label13 = new System.Windows.Forms.Label();
            this.tbSplitWeight1 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tbNetWeight = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblModulesInLoad = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbYardLocation = new System.Windows.Forms.TextBox();
            this.cboTruck = new System.Windows.Forms.ComboBox();
            this.tbGinTagLoadNumber = new System.Windows.Forms.TextBox();
            this.tbPickedBy = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel9 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.tbBridgeId = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbScaleBridgeLoadNumber = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbGrossWeight = new System.Windows.Forms.TextBox();
            this.tbSplitWeight2 = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.rdoAttendant = new System.Windows.Forms.RadioButton();
            this.rdoDriver = new System.Windows.Forms.RadioButton();
            this.tbTrailerNumber = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.clientSelector = new CottonDBMS.GinApp.UserControls.ClientSelector();
            this.farmSelector = new CottonDBMS.GinApp.UserControls.FarmSelector();
            this.fieldSelector = new CottonDBMS.GinApp.UserControls.FieldSelector();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel9.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(124, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(115, 37);
            this.btnCancel.TabIndex = 19;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.Location = new System.Drawing.Point(3, 345);
            this.label11.Margin = new System.Windows.Forms.Padding(3);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(133, 24);
            this.label11.TabIndex = 116;
            this.label11.Text = "Trailer/Module#";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.Location = new System.Drawing.Point(3, 317);
            this.label10.Margin = new System.Windows.Forms.Padding(3);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(133, 22);
            this.label10.TabIndex = 115;
            this.label10.Text = "Variety:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.Location = new System.Drawing.Point(3, 289);
            this.label9.Margin = new System.Windows.Forms.Padding(3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(133, 22);
            this.label9.TabIndex = 110;
            this.label9.Text = "Picked by:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblImportedLoadNumber
            // 
            this.lblImportedLoadNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblImportedLoadNumber.Location = new System.Drawing.Point(3, 261);
            this.lblImportedLoadNumber.Margin = new System.Windows.Forms.Padding(3);
            this.lblImportedLoadNumber.Name = "lblImportedLoadNumber";
            this.lblImportedLoadNumber.Size = new System.Drawing.Size(133, 22);
            this.lblImportedLoadNumber.TabIndex = 109;
            this.lblImportedLoadNumber.Text = "Yard Location:";
            this.lblImportedLoadNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbVariety
            // 
            this.tbVariety.Location = new System.Drawing.Point(142, 317);
            this.tbVariety.MaxLength = 50;
            this.tbVariety.Name = "tbVariety";
            this.tbVariety.Size = new System.Drawing.Size(350, 22);
            this.tbVariety.TabIndex = 15;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.clientSelector);
            this.flowLayoutPanel1.Controls.Add(this.farmSelector);
            this.flowLayoutPanel1.Controls.Add(this.fieldSelector);
            this.flowLayoutPanel1.Controls.Add(this.tableLayoutPanel2);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(523, 729);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.label13, 0, 8);
            this.tableLayoutPanel2.Controls.Add(this.tbSplitWeight1, 1, 6);
            this.tableLayoutPanel2.Controls.Add(this.label12, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.tbNetWeight, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.label8, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 7);
            this.tableLayoutPanel2.Controls.Add(this.lblModulesInLoad, 1, 14);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 14);
            this.tableLayoutPanel2.Controls.Add(this.tbYardLocation, 1, 10);
            this.tableLayoutPanel2.Controls.Add(this.tbGinTagLoadNumber, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.label11, 0, 13);
            this.tableLayoutPanel2.Controls.Add(this.label10, 0, 12);
            this.tableLayoutPanel2.Controls.Add(this.label9, 0, 11);
            this.tableLayoutPanel2.Controls.Add(this.lblImportedLoadNumber, 0, 10);
            this.tableLayoutPanel2.Controls.Add(this.tbVariety, 1, 12);
            this.tableLayoutPanel2.Controls.Add(this.tbPickedBy, 1, 11);
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel9, 1, 15);
            this.tableLayoutPanel2.Controls.Add(this.tbBridgeId, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tbScaleBridgeLoadNumber, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.tbGrossWeight, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.tbSplitWeight2, 1, 7);
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel2, 1, 8);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.cboTruck, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.tbTrailerNumber, 1, 13);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 210);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 16;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(517, 493);
            this.tableLayoutPanel2.TabIndex = 16;
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 226);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(133, 32);
            this.label13.TabIndex = 108;
            this.label13.Text = "Submitted by:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbSplitWeight1
            // 
            this.tbSplitWeight1.Location = new System.Drawing.Point(142, 173);
            this.tbSplitWeight1.MaxLength = 50;
            this.tbSplitWeight1.Name = "tbSplitWeight1";
            this.tbSplitWeight1.Size = new System.Drawing.Size(350, 22);
            this.tbSplitWeight1.TabIndex = 8;
            this.tbSplitWeight1.TextChanged += new System.EventHandler(this.tbSplitWeight1_TextChanged);
            this.tbSplitWeight1.Validating += new System.ComponentModel.CancelEventHandler(this.tbSplitWeight1_Validating);
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 170);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(133, 28);
            this.label12.TabIndex = 106;
            this.label12.Text = "Split Weight 1:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbNetWeight
            // 
            this.tbNetWeight.Location = new System.Drawing.Point(142, 145);
            this.tbNetWeight.MaxLength = 50;
            this.tbNetWeight.Name = "tbNetWeight";
            this.tbNetWeight.ReadOnly = true;
            this.tbNetWeight.Size = new System.Drawing.Size(350, 22);
            this.tbNetWeight.TabIndex = 8;
            this.tbNetWeight.TabStop = false;
            this.tbNetWeight.Validating += new System.ComponentModel.CancelEventHandler(this.tbNetWeight_Validating);
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 142);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(133, 28);
            this.label8.TabIndex = 105;
            this.label8.Text = "Net Weight:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 198);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(133, 28);
            this.label7.TabIndex = 107;
            this.label7.Text = "Split Weight 2:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblModulesInLoad
            // 
            this.lblModulesInLoad.Location = new System.Drawing.Point(142, 375);
            this.lblModulesInLoad.Margin = new System.Windows.Forms.Padding(3);
            this.lblModulesInLoad.Name = "lblModulesInLoad";
            this.lblModulesInLoad.Size = new System.Drawing.Size(350, 66);
            this.lblModulesInLoad.TabIndex = 120;
            this.lblModulesInLoad.Text = "--";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(3, 375);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 14);
            this.label3.TabIndex = 117;
            this.label3.Text = "Modules in load:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbYardLocation
            // 
            this.tbYardLocation.Location = new System.Drawing.Point(142, 261);
            this.tbYardLocation.Margin = new System.Windows.Forms.Padding(3, 3, 25, 3);
            this.tbYardLocation.MaxLength = 50;
            this.tbYardLocation.Name = "tbYardLocation";
            this.tbYardLocation.Size = new System.Drawing.Size(350, 22);
            this.tbYardLocation.TabIndex = 13;
            // 
            // cboTruck
            // 
            this.cboTruck.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTruck.FormattingEnabled = true;
            this.cboTruck.Items.AddRange(new object[] {
            "-- Select One --",
            "In field",
            "Picked up",
            "At gin",
            "On feeder",
            "Ginned"});
            this.cboTruck.Location = new System.Drawing.Point(142, 59);
            this.cboTruck.Name = "cboTruck";
            this.cboTruck.Size = new System.Drawing.Size(350, 24);
            this.cboTruck.TabIndex = 6;
            this.cboTruck.SelectedIndexChanged += new System.EventHandler(this.cboTruck_SelectedIndexChanged);
            this.cboTruck.Validating += new System.ComponentModel.CancelEventHandler(this.cboTruck_Validating);
            // 
            // tbGinTagLoadNumber
            // 
            this.tbGinTagLoadNumber.Location = new System.Drawing.Point(142, 89);
            this.tbGinTagLoadNumber.MaxLength = 50;
            this.tbGinTagLoadNumber.Name = "tbGinTagLoadNumber";
            this.tbGinTagLoadNumber.Size = new System.Drawing.Size(350, 22);
            this.tbGinTagLoadNumber.TabIndex = 7;
            this.tbGinTagLoadNumber.Validating += new System.ComponentModel.CancelEventHandler(this.tbGinTagLoadNumber_Validating);
            // 
            // tbPickedBy
            // 
            this.tbPickedBy.Location = new System.Drawing.Point(142, 289);
            this.tbPickedBy.Margin = new System.Windows.Forms.Padding(3, 3, 25, 3);
            this.tbPickedBy.MaxLength = 50;
            this.tbPickedBy.Name = "tbPickedBy";
            this.tbPickedBy.Size = new System.Drawing.Size(350, 22);
            this.tbPickedBy.TabIndex = 14;
            // 
            // flowLayoutPanel9
            // 
            this.flowLayoutPanel9.AutoSize = true;
            this.flowLayoutPanel9.Controls.Add(this.btnCancel);
            this.flowLayoutPanel9.Controls.Add(this.btnSave);
            this.flowLayoutPanel9.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel9.Location = new System.Drawing.Point(142, 447);
            this.flowLayoutPanel9.Name = "flowLayoutPanel9";
            this.flowLayoutPanel9.Size = new System.Drawing.Size(242, 43);
            this.flowLayoutPanel9.TabIndex = 18;
            this.flowLayoutPanel9.WrapContents = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(3, 59);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 24);
            this.label1.TabIndex = 102;
            this.label1.Text = "Truck ID:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbBridgeId
            // 
            this.tbBridgeId.Location = new System.Drawing.Point(142, 3);
            this.tbBridgeId.MaxLength = 50;
            this.tbBridgeId.Name = "tbBridgeId";
            this.tbBridgeId.Size = new System.Drawing.Size(350, 22);
            this.tbBridgeId.TabIndex = 4;
            this.tbBridgeId.Validating += new System.ComponentModel.CancelEventHandler(this.tbBridgeId_Validating);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Location = new System.Drawing.Point(3, 3);
            this.label4.Margin = new System.Windows.Forms.Padding(3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 22);
            this.label4.TabIndex = 100;
            this.label4.Text = "Scale Bridge ID:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 86);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(133, 28);
            this.label5.TabIndex = 103;
            this.label5.Text = "Gin Ticket Load#:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 28);
            this.label2.TabIndex = 101;
            this.label2.Text = "Scale Bridge Load #:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbScaleBridgeLoadNumber
            // 
            this.tbScaleBridgeLoadNumber.Location = new System.Drawing.Point(142, 31);
            this.tbScaleBridgeLoadNumber.MaxLength = 50;
            this.tbScaleBridgeLoadNumber.Name = "tbScaleBridgeLoadNumber";
            this.tbScaleBridgeLoadNumber.Size = new System.Drawing.Size(350, 22);
            this.tbScaleBridgeLoadNumber.TabIndex = 5;
            this.tbScaleBridgeLoadNumber.Validating += new System.ComponentModel.CancelEventHandler(this.tbScaleBridgeLoadNumber_Validating);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 114);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(133, 28);
            this.label6.TabIndex = 104;
            this.label6.Text = "Gross Weight:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tbGrossWeight
            // 
            this.tbGrossWeight.Location = new System.Drawing.Point(142, 117);
            this.tbGrossWeight.MaxLength = 50;
            this.tbGrossWeight.Name = "tbGrossWeight";
            this.tbGrossWeight.ReadOnly = true;
            this.tbGrossWeight.Size = new System.Drawing.Size(350, 22);
            this.tbGrossWeight.TabIndex = 7;
            this.tbGrossWeight.TabStop = false;
            this.tbGrossWeight.Validating += new System.ComponentModel.CancelEventHandler(this.tbGrossWeight_Validating);
            // 
            // tbSplitWeight2
            // 
            this.tbSplitWeight2.Location = new System.Drawing.Point(142, 201);
            this.tbSplitWeight2.MaxLength = 50;
            this.tbSplitWeight2.Name = "tbSplitWeight2";
            this.tbSplitWeight2.Size = new System.Drawing.Size(350, 22);
            this.tbSplitWeight2.TabIndex = 9;
            this.tbSplitWeight2.TextChanged += new System.EventHandler(this.tbSplitWeight2_TextChanged);
            this.tbSplitWeight2.Validating += new System.ComponentModel.CancelEventHandler(this.tbSplitWeight2_Validating);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.Controls.Add(this.rdoAttendant);
            this.flowLayoutPanel2.Controls.Add(this.rdoDriver);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(142, 229);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(156, 26);
            this.flowLayoutPanel2.TabIndex = 10;
            // 
            // rdoAttendant
            // 
            this.rdoAttendant.AutoSize = true;
            this.rdoAttendant.Checked = true;
            this.rdoAttendant.Location = new System.Drawing.Point(3, 3);
            this.rdoAttendant.Name = "rdoAttendant";
            this.rdoAttendant.Size = new System.Drawing.Size(82, 20);
            this.rdoAttendant.TabIndex = 11;
            this.rdoAttendant.TabStop = true;
            this.rdoAttendant.Text = "Attendant";
            this.rdoAttendant.UseVisualStyleBackColor = true;
            // 
            // rdoDriver
            // 
            this.rdoDriver.AutoSize = true;
            this.rdoDriver.Location = new System.Drawing.Point(91, 3);
            this.rdoDriver.Name = "rdoDriver";
            this.rdoDriver.Size = new System.Drawing.Size(62, 20);
            this.rdoDriver.TabIndex = 12;
            this.rdoDriver.Text = "Driver";
            this.rdoDriver.UseVisualStyleBackColor = true;
            // 
            // tbTrailerNumber
            // 
            this.tbTrailerNumber.Location = new System.Drawing.Point(142, 345);
            this.tbTrailerNumber.Margin = new System.Windows.Forms.Padding(3, 3, 25, 3);
            this.tbTrailerNumber.MaxLength = 50;
            this.tbTrailerNumber.Name = "tbTrailerNumber";
            this.tbTrailerNumber.Size = new System.Drawing.Size(350, 22);
            this.tbTrailerNumber.TabIndex = 16;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(3, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(115, 37);
            this.btnSave.TabIndex = 18;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // clientSelector
            // 
            this.clientSelector.AutoSize = true;
            this.clientSelector.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.clientSelector.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clientSelector.HasError = false;
            this.clientSelector.InputColumnWidth = 350;
            this.clientSelector.LabelColumnWidth = 133;
            this.clientSelector.Location = new System.Drawing.Point(4, 4);
            this.clientSelector.Margin = new System.Windows.Forms.Padding(4);
            this.clientSelector.Name = "clientSelector";
            this.clientSelector.Size = new System.Drawing.Size(516, 68);
            this.clientSelector.TabIndex = 1;
            this.clientSelector.SelectionChanged += new System.EventHandler(this.clientSelector_SelectionChanged);
            // 
            // farmSelector
            // 
            this.farmSelector.AutoSize = true;
            this.farmSelector.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.farmSelector.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.farmSelector.FormErrorProvider = null;
            this.farmSelector.HasError = false;
            this.farmSelector.InputColumnWidth = 350;
            this.farmSelector.LabelColumnWidth = 133;
            this.farmSelector.Location = new System.Drawing.Point(4, 80);
            this.farmSelector.Margin = new System.Windows.Forms.Padding(4);
            this.farmSelector.Name = "farmSelector";
            this.farmSelector.Size = new System.Drawing.Size(512, 58);
            this.farmSelector.TabIndex = 2;
            this.farmSelector.Visible = false;
            this.farmSelector.SelectionChanged += new System.EventHandler(this.farmSelector_SelectionChanged);
            // 
            // fieldSelector
            // 
            this.fieldSelector.AutoSize = true;
            this.fieldSelector.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.fieldSelector.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fieldSelector.FormErrorProvider = null;
            this.fieldSelector.HasError = false;
            this.fieldSelector.InputColumnWidth = 350;
            this.fieldSelector.LabelColumnWidth = 133;
            this.fieldSelector.Location = new System.Drawing.Point(3, 145);
            this.fieldSelector.Name = "fieldSelector";
            this.fieldSelector.Size = new System.Drawing.Size(515, 59);
            this.fieldSelector.TabIndex = 3;
            this.fieldSelector.Visible = false;
            // 
            // AddEditGinLoad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 732);
            this.Controls.Add(this.flowLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddEditGinLoad";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Gin Load";
            this.Shown += new System.EventHandler(this.AddEditGinLoad_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.flowLayoutPanel9.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private UserControls.ClientSelector clientSelector;
        private UserControls.FarmSelector farmSelector;
        private UserControls.FieldSelector fieldSelector;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox tbGinTagLoadNumber;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblImportedLoadNumber;
        private System.Windows.Forms.TextBox tbVariety;
        private System.Windows.Forms.TextBox tbPickedBy;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel9;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbBridgeId;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbScaleBridgeLoadNumber;
        private System.Windows.Forms.TextBox tbGrossWeight;
        private System.Windows.Forms.TextBox tbYardLocation;
        private System.Windows.Forms.ComboBox cboTruck;
        private System.Windows.Forms.Label lblModulesInLoad;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox tbSplitWeight1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbNetWeight;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbSplitWeight2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.RadioButton rdoAttendant;
        private System.Windows.Forms.RadioButton rdoDriver;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox tbTrailerNumber;
    }
}