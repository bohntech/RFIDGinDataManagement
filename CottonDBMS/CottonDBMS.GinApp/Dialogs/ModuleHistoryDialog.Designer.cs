namespace CottonDBMS.GinApp.Dialogs
{
    partial class ModuleHistoryDialog
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.lblClient = new System.Windows.Forms.Label();
            this.lblFarm = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblField = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblSerialNo = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblLoad = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblLocationValue = new System.Windows.Forms.Label();
            this.lblLocation = new System.Windows.Forms.Label();
            this.historyDataGrid = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            this.TruckID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Driver = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BridgeId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BridgeLoadNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GinTagLoadNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Event = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Latitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Longitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Timestamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.historyDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(24, 16);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Client:";
            // 
            // lblClient
            // 
            this.lblClient.Location = new System.Drawing.Point(76, 16);
            this.lblClient.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblClient.Name = "lblClient";
            this.lblClient.Size = new System.Drawing.Size(302, 16);
            this.lblClient.TabIndex = 1;
            this.lblClient.Text = "Grower 1";
            // 
            // lblFarm
            // 
            this.lblFarm.Location = new System.Drawing.Point(466, 16);
            this.lblFarm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFarm.Name = "lblFarm";
            this.lblFarm.Size = new System.Drawing.Size(302, 16);
            this.lblFarm.TabIndex = 3;
            this.lblFarm.Text = "Farm 1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(420, 16);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 16);
            this.label4.TabIndex = 2;
            this.label4.Text = "Farm:";
            // 
            // lblField
            // 
            this.lblField.Location = new System.Drawing.Point(76, 42);
            this.lblField.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblField.Name = "lblField";
            this.lblField.Size = new System.Drawing.Size(302, 16);
            this.lblField.TabIndex = 5;
            this.lblField.Text = "Field 1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(30, 42);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 16);
            this.label6.TabIndex = 4;
            this.label6.Text = "Field:";
            // 
            // lblSerialNo
            // 
            this.lblSerialNo.Location = new System.Drawing.Point(466, 42);
            this.lblSerialNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSerialNo.Name = "lblSerialNo";
            this.lblSerialNo.Size = new System.Drawing.Size(302, 16);
            this.lblSerialNo.TabIndex = 7;
            this.lblSerialNo.Text = "000000012";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(402, 42);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 16);
            this.label8.TabIndex = 6;
            this.label8.Text = "Serial #:";
            // 
            // lblLoad
            // 
            this.lblLoad.Location = new System.Drawing.Point(76, 68);
            this.lblLoad.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLoad.Name = "lblLoad";
            this.lblLoad.Size = new System.Drawing.Size(302, 16);
            this.lblLoad.TabIndex = 9;
            this.lblLoad.Text = "000123";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(18, 68);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 16);
            this.label10.TabIndex = 8;
            this.label10.Text = "Load #:";
            // 
            // lblLocationValue
            // 
            this.lblLocationValue.Location = new System.Drawing.Point(468, 68);
            this.lblLocationValue.Margin = new System.Windows.Forms.Padding(0);
            this.lblLocationValue.Name = "lblLocationValue";
            this.lblLocationValue.Size = new System.Drawing.Size(302, 25);
            this.lblLocationValue.TabIndex = 11;
            this.lblLocationValue.Text = "33.5779, 101.8552";
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocation.Location = new System.Drawing.Point(396, 68);
            this.lblLocation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(71, 16);
            this.lblLocation.TabIndex = 10;
            this.lblLocation.Text = "Location:";
            // 
            // historyDataGrid
            // 
            this.historyDataGrid.AllowUserToAddRows = false;
            this.historyDataGrid.AllowUserToDeleteRows = false;
            this.historyDataGrid.AllowUserToResizeRows = false;
            this.historyDataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.historyDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.historyDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TruckID,
            this.Driver,
            this.BridgeId,
            this.BridgeLoadNumber,
            this.GinTagLoadNumber,
            this.Event,
            this.Status,
            this.Latitude,
            this.Longitude,
            this.Timestamp});
            this.historyDataGrid.Location = new System.Drawing.Point(21, 108);
            this.historyDataGrid.MultiSelect = false;
            this.historyDataGrid.Name = "historyDataGrid";
            this.historyDataGrid.ReadOnly = true;
            this.historyDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.historyDataGrid.Size = new System.Drawing.Size(959, 398);
            this.historyDataGrid.TabIndex = 12;            
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(21, 519);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 36);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // TruckID
            // 
            this.TruckID.DataPropertyName = "TruckID";
            this.TruckID.HeaderText = "Truck ID";
            this.TruckID.Name = "TruckID";
            this.TruckID.ReadOnly = true;
            this.TruckID.Width = 83;
            // 
            // Driver
            // 
            this.Driver.DataPropertyName = "Driver";
            this.Driver.HeaderText = "Driver";
            this.Driver.Name = "Driver";
            this.Driver.ReadOnly = true;
            this.Driver.Width = 69;
            // 
            // BridgeId
            // 
            this.BridgeId.DataPropertyName = "BridgeId";
            this.BridgeId.HeaderText = "Bridge ID";
            this.BridgeId.Name = "BridgeId";
            this.BridgeId.ReadOnly = true;
            this.BridgeId.Width = 89;
            // 
            // BridgeLoadNumber
            // 
            this.BridgeLoadNumber.DataPropertyName = "BridgeLoadNumber";
            dataGridViewCellStyle3.NullValue = null;
            this.BridgeLoadNumber.DefaultCellStyle = dataGridViewCellStyle3;
            this.BridgeLoadNumber.HeaderText = "Bridge Load#";
            this.BridgeLoadNumber.Name = "BridgeLoadNumber";
            this.BridgeLoadNumber.ReadOnly = true;
            this.BridgeLoadNumber.Width = 114;
            // 
            // GinTagLoadNumber
            // 
            this.GinTagLoadNumber.DataPropertyName = "GinTagLoadNumber";
            dataGridViewCellStyle4.NullValue = " ";
            this.GinTagLoadNumber.DefaultCellStyle = dataGridViewCellStyle4;
            this.GinTagLoadNumber.HeaderText = "Gin Tkt Load#";
            this.GinTagLoadNumber.Name = "GinTagLoadNumber";
            this.GinTagLoadNumber.ReadOnly = true;
            this.GinTagLoadNumber.Width = 116;
            // 
            // Event
            // 
            this.Event.DataPropertyName = "EventName";
            this.Event.HeaderText = "Event";
            this.Event.Name = "Event";
            this.Event.ReadOnly = true;
            this.Event.Width = 67;
            // 
            // Status
            // 
            this.Status.DataPropertyName = "StatusName";
            this.Status.HeaderText = "Status Note";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Width = 102;
            // 
            // Latitude
            // 
            this.Latitude.DataPropertyName = "Latitude";
            this.Latitude.HeaderText = "Latitude";
            this.Latitude.Name = "Latitude";
            this.Latitude.ReadOnly = true;
            this.Latitude.Width = 80;
            // 
            // Longitude
            // 
            this.Longitude.DataPropertyName = "Longitude";
            this.Longitude.HeaderText = "Longitude";
            this.Longitude.Name = "Longitude";
            this.Longitude.ReadOnly = true;
            this.Longitude.Width = 92;
            // 
            // Timestamp
            // 
            this.Timestamp.DataPropertyName = "LocalCreatedTimestamp";
            this.Timestamp.HeaderText = "Timestamp";
            this.Timestamp.Name = "Timestamp";
            this.Timestamp.ReadOnly = true;
            this.Timestamp.Width = 101;
            // 
            // ModuleHistoryDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1013, 567);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.historyDataGrid);
            this.Controls.Add(this.lblLocationValue);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.lblLoad);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.lblSerialNo);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblField);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblFarm);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblClient);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModuleHistoryDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Module History";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ModuleHistory_Load);
            ((System.ComponentModel.ISupportInitialize)(this.historyDataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblClient;
        private System.Windows.Forms.Label lblFarm;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblField;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblSerialNo;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblLoad;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblLocationValue;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.DataGridView historyDataGrid;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn TruckID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Driver;
        private System.Windows.Forms.DataGridViewTextBoxColumn BridgeId;
        private System.Windows.Forms.DataGridViewTextBoxColumn BridgeLoadNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn GinTagLoadNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn Event;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Latitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn Longitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn Timestamp;
    }
}