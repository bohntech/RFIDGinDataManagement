namespace CottonDBMS.GinApp
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.appTabs = new System.Windows.Forms.TabControl();
            this.tabHome = new System.Windows.Forms.TabPage();
            this.homePage1 = new CottonDBMS.GinApp.UserControls.HomePage();
            this.tabPickupLists = new System.Windows.Forms.TabPage();
            this.pickupListsPage1 = new CottonDBMS.GinApp.UserControls.PickupListsPage();
            this.tabGinLoads = new System.Windows.Forms.TabPage();
            this.ginLoadsPage1 = new CottonDBMS.GinApp.UserControls.GinLoadsPage();
            this.tabModules = new System.Windows.Forms.TabPage();
            this.modulesPage1 = new CottonDBMS.GinApp.UserControls.ModulesPage();
            this.tabBales = new System.Windows.Forms.TabPage();
            this.balesPage = new CottonDBMS.GinApp.UserControls.BalesPage();
            this.tabClients = new System.Windows.Forms.TabPage();
            this.clientsPage = new CottonDBMS.GinApp.UserControls.ClientsPage();
            this.tabFarms = new System.Windows.Forms.TabPage();
            this.farmsPage = new CottonDBMS.GinApp.UserControls.FarmsPage();
            this.tabFields = new System.Windows.Forms.TabPage();
            this.fieldsPage = new CottonDBMS.GinApp.UserControls.FieldsPage();
            this.tabDrivers = new System.Windows.Forms.TabPage();
            this.driversPage = new CottonDBMS.GinApp.UserControls.DriversPage();
            this.tabTrucks = new System.Windows.Forms.TabPage();
            this.trucksPage = new CottonDBMS.GinApp.UserControls.TrucksPage();
            this.tabReports = new System.Windows.Forms.TabPage();
            this.reportPage1 = new CottonDBMS.GinApp.UserControls.ReportPage();
            this.flowLayoutPanel33 = new System.Windows.Forms.FlowLayoutPanel();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.settingsPage = new CottonDBMS.GinApp.UserControls.SettingsPage();
            this.tabAbout = new System.Windows.Forms.TabPage();
            this.tbLicense = new System.Windows.Forms.TextBox();
            this.tabPBIMapping = new System.Windows.Forms.TabPage();
            this.pbiMappingPage1 = new CottonDBMS.GinApp.UserControls.PBIMappingPage();
            this.appTabs.SuspendLayout();
            this.tabHome.SuspendLayout();
            this.tabPickupLists.SuspendLayout();
            this.tabGinLoads.SuspendLayout();
            this.tabModules.SuspendLayout();
            this.tabBales.SuspendLayout();
            this.tabClients.SuspendLayout();
            this.tabFarms.SuspendLayout();
            this.tabFields.SuspendLayout();
            this.tabDrivers.SuspendLayout();
            this.tabTrucks.SuspendLayout();
            this.tabReports.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.tabAbout.SuspendLayout();
            this.tabPBIMapping.SuspendLayout();
            this.SuspendLayout();
            // 
            // appTabs
            // 
            this.appTabs.Controls.Add(this.tabHome);
            this.appTabs.Controls.Add(this.tabPickupLists);
            this.appTabs.Controls.Add(this.tabGinLoads);
            this.appTabs.Controls.Add(this.tabModules);
            this.appTabs.Controls.Add(this.tabBales);
            this.appTabs.Controls.Add(this.tabPBIMapping);
            this.appTabs.Controls.Add(this.tabClients);
            this.appTabs.Controls.Add(this.tabFarms);
            this.appTabs.Controls.Add(this.tabFields);
            this.appTabs.Controls.Add(this.tabDrivers);
            this.appTabs.Controls.Add(this.tabTrucks);
            this.appTabs.Controls.Add(this.tabReports);
            this.appTabs.Controls.Add(this.tabSettings);
            this.appTabs.Controls.Add(this.tabAbout);
            this.appTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appTabs.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.appTabs.ItemSize = new System.Drawing.Size(58, 40);
            this.appTabs.Location = new System.Drawing.Point(0, 0);
            this.appTabs.Name = "appTabs";
            this.appTabs.Padding = new System.Drawing.Point(15, 3);
            this.appTabs.SelectedIndex = 0;
            this.appTabs.Size = new System.Drawing.Size(1092, 795);
            this.appTabs.TabIndex = 0;
            this.appTabs.Selected += new System.Windows.Forms.TabControlEventHandler(this.appTabs_Selected);
            // 
            // tabHome
            // 
            this.tabHome.Controls.Add(this.homePage1);
            this.tabHome.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabHome.Location = new System.Drawing.Point(4, 44);
            this.tabHome.Name = "tabHome";
            this.tabHome.Padding = new System.Windows.Forms.Padding(3);
            this.tabHome.Size = new System.Drawing.Size(1084, 747);
            this.tabHome.TabIndex = 2;
            this.tabHome.Text = "Home";
            this.tabHome.UseVisualStyleBackColor = true;
            // 
            // homePage1
            // 
            this.homePage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.homePage1.Location = new System.Drawing.Point(3, 3);
            this.homePage1.Margin = new System.Windows.Forms.Padding(4);
            this.homePage1.Name = "homePage1";
            this.homePage1.Size = new System.Drawing.Size(1078, 741);
            this.homePage1.TabIndex = 0;
            // 
            // tabPickupLists
            // 
            this.tabPickupLists.Controls.Add(this.pickupListsPage1);
            this.tabPickupLists.Location = new System.Drawing.Point(4, 44);
            this.tabPickupLists.Name = "tabPickupLists";
            this.tabPickupLists.Size = new System.Drawing.Size(1084, 747);
            this.tabPickupLists.TabIndex = 3;
            this.tabPickupLists.Text = "Pickup Lists";
            this.tabPickupLists.UseVisualStyleBackColor = true;
            // 
            // pickupListsPage1
            // 
            this.pickupListsPage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pickupListsPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pickupListsPage1.Location = new System.Drawing.Point(0, 0);
            this.pickupListsPage1.Margin = new System.Windows.Forms.Padding(4);
            this.pickupListsPage1.Name = "pickupListsPage1";
            this.pickupListsPage1.Size = new System.Drawing.Size(1084, 747);
            this.pickupListsPage1.TabIndex = 0;
            // 
            // tabGinLoads
            // 
            this.tabGinLoads.Controls.Add(this.ginLoadsPage1);
            this.tabGinLoads.Location = new System.Drawing.Point(4, 44);
            this.tabGinLoads.Name = "tabGinLoads";
            this.tabGinLoads.Size = new System.Drawing.Size(1084, 747);
            this.tabGinLoads.TabIndex = 13;
            this.tabGinLoads.Text = "Gin Loads";
            this.tabGinLoads.UseVisualStyleBackColor = true;
            // 
            // ginLoadsPage1
            // 
            this.ginLoadsPage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ginLoadsPage1.Location = new System.Drawing.Point(0, 0);
            this.ginLoadsPage1.Margin = new System.Windows.Forms.Padding(0);
            this.ginLoadsPage1.Name = "ginLoadsPage1";
            this.ginLoadsPage1.Size = new System.Drawing.Size(1084, 747);
            this.ginLoadsPage1.TabIndex = 0;
            // 
            // tabModules
            // 
            this.tabModules.Controls.Add(this.modulesPage1);
            this.tabModules.Location = new System.Drawing.Point(4, 44);
            this.tabModules.Name = "tabModules";
            this.tabModules.Padding = new System.Windows.Forms.Padding(5);
            this.tabModules.Size = new System.Drawing.Size(1084, 747);
            this.tabModules.TabIndex = 4;
            this.tabModules.Text = "Modules";
            this.tabModules.UseVisualStyleBackColor = true;
            // 
            // modulesPage1
            // 
            this.modulesPage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modulesPage1.Location = new System.Drawing.Point(5, 5);
            this.modulesPage1.Margin = new System.Windows.Forms.Padding(0);
            this.modulesPage1.Name = "modulesPage1";
            this.modulesPage1.Size = new System.Drawing.Size(1074, 737);
            this.modulesPage1.TabIndex = 0;
            // 
            // tabBales
            // 
            this.tabBales.Controls.Add(this.balesPage);
            this.tabBales.Location = new System.Drawing.Point(4, 44);
            this.tabBales.Name = "tabBales";
            this.tabBales.Padding = new System.Windows.Forms.Padding(3);
            this.tabBales.Size = new System.Drawing.Size(1084, 747);
            this.tabBales.TabIndex = 14;
            this.tabBales.Text = "Bales";
            this.tabBales.UseVisualStyleBackColor = true;
            // 
            // balesPage
            // 
            this.balesPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.balesPage.Location = new System.Drawing.Point(3, 3);
            this.balesPage.Name = "balesPage";
            this.balesPage.Size = new System.Drawing.Size(1078, 741);
            this.balesPage.TabIndex = 0;
            // 
            // tabClients
            // 
            this.tabClients.Controls.Add(this.clientsPage);
            this.tabClients.Location = new System.Drawing.Point(4, 44);
            this.tabClients.Name = "tabClients";
            this.tabClients.Size = new System.Drawing.Size(1084, 747);
            this.tabClients.TabIndex = 10;
            this.tabClients.Text = "Clients";
            this.tabClients.UseVisualStyleBackColor = true;
            // 
            // clientsPage
            // 
            this.clientsPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clientsPage.Location = new System.Drawing.Point(0, 0);
            this.clientsPage.Margin = new System.Windows.Forms.Padding(4);
            this.clientsPage.Name = "clientsPage";
            this.clientsPage.Size = new System.Drawing.Size(1084, 747);
            this.clientsPage.TabIndex = 0;
            // 
            // tabFarms
            // 
            this.tabFarms.Controls.Add(this.farmsPage);
            this.tabFarms.Location = new System.Drawing.Point(4, 44);
            this.tabFarms.Name = "tabFarms";
            this.tabFarms.Size = new System.Drawing.Size(1084, 747);
            this.tabFarms.TabIndex = 11;
            this.tabFarms.Text = "Farms";
            this.tabFarms.UseVisualStyleBackColor = true;
            // 
            // farmsPage
            // 
            this.farmsPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.farmsPage.Location = new System.Drawing.Point(0, 0);
            this.farmsPage.Margin = new System.Windows.Forms.Padding(4);
            this.farmsPage.Name = "farmsPage";
            this.farmsPage.Size = new System.Drawing.Size(1084, 747);
            this.farmsPage.TabIndex = 0;
            // 
            // tabFields
            // 
            this.tabFields.Controls.Add(this.fieldsPage);
            this.tabFields.Location = new System.Drawing.Point(4, 44);
            this.tabFields.Name = "tabFields";
            this.tabFields.Size = new System.Drawing.Size(1084, 747);
            this.tabFields.TabIndex = 12;
            this.tabFields.Text = "Fields";
            this.tabFields.UseVisualStyleBackColor = true;
            // 
            // fieldsPage
            // 
            this.fieldsPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fieldsPage.Location = new System.Drawing.Point(0, 0);
            this.fieldsPage.Margin = new System.Windows.Forms.Padding(4);
            this.fieldsPage.Name = "fieldsPage";
            this.fieldsPage.Size = new System.Drawing.Size(1084, 747);
            this.fieldsPage.TabIndex = 0;
            // 
            // tabDrivers
            // 
            this.tabDrivers.Controls.Add(this.driversPage);
            this.tabDrivers.Location = new System.Drawing.Point(4, 44);
            this.tabDrivers.Name = "tabDrivers";
            this.tabDrivers.Size = new System.Drawing.Size(1084, 747);
            this.tabDrivers.TabIndex = 9;
            this.tabDrivers.Text = "Drivers";
            this.tabDrivers.UseVisualStyleBackColor = true;
            // 
            // driversPage
            // 
            this.driversPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.driversPage.Location = new System.Drawing.Point(0, 0);
            this.driversPage.Margin = new System.Windows.Forms.Padding(4);
            this.driversPage.Name = "driversPage";
            this.driversPage.Size = new System.Drawing.Size(1084, 747);
            this.driversPage.TabIndex = 0;
            // 
            // tabTrucks
            // 
            this.tabTrucks.Controls.Add(this.trucksPage);
            this.tabTrucks.Location = new System.Drawing.Point(4, 44);
            this.tabTrucks.Name = "tabTrucks";
            this.tabTrucks.Size = new System.Drawing.Size(1084, 747);
            this.tabTrucks.TabIndex = 8;
            this.tabTrucks.Text = "Trucks";
            this.tabTrucks.UseVisualStyleBackColor = true;
            // 
            // trucksPage
            // 
            this.trucksPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trucksPage.Location = new System.Drawing.Point(0, 0);
            this.trucksPage.Margin = new System.Windows.Forms.Padding(4);
            this.trucksPage.Name = "trucksPage";
            this.trucksPage.Size = new System.Drawing.Size(1084, 747);
            this.trucksPage.TabIndex = 0;
            // 
            // tabReports
            // 
            this.tabReports.Controls.Add(this.reportPage1);
            this.tabReports.Controls.Add(this.flowLayoutPanel33);
            this.tabReports.Location = new System.Drawing.Point(4, 44);
            this.tabReports.Name = "tabReports";
            this.tabReports.Size = new System.Drawing.Size(1084, 747);
            this.tabReports.TabIndex = 5;
            this.tabReports.Text = "Reports";
            this.tabReports.UseVisualStyleBackColor = true;
            // 
            // reportPage1
            // 
            this.reportPage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportPage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reportPage1.Location = new System.Drawing.Point(0, 0);
            this.reportPage1.Margin = new System.Windows.Forms.Padding(4);
            this.reportPage1.Name = "reportPage1";
            this.reportPage1.Padding = new System.Windows.Forms.Padding(10);
            this.reportPage1.Size = new System.Drawing.Size(1084, 747);
            this.reportPage1.TabIndex = 11;
            // 
            // flowLayoutPanel33
            // 
            this.flowLayoutPanel33.AutoSize = true;
            this.flowLayoutPanel33.BackColor = System.Drawing.Color.Transparent;
            this.flowLayoutPanel33.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel33.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel33.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel33.Name = "flowLayoutPanel33";
            this.flowLayoutPanel33.Size = new System.Drawing.Size(1084, 0);
            this.flowLayoutPanel33.TabIndex = 10;
            this.flowLayoutPanel33.WrapContents = false;
            // 
            // tabSettings
            // 
            this.tabSettings.AutoScroll = true;
            this.tabSettings.Controls.Add(this.settingsPage);
            this.tabSettings.Location = new System.Drawing.Point(4, 44);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Size = new System.Drawing.Size(1084, 747);
            this.tabSettings.TabIndex = 6;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // settingsPage
            // 
            this.settingsPage.AutoSize = true;
            this.settingsPage.Location = new System.Drawing.Point(4, 5);
            this.settingsPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 30);
            this.settingsPage.Name = "settingsPage";
            this.settingsPage.Size = new System.Drawing.Size(1027, 1180);
            this.settingsPage.TabIndex = 0;
            // 
            // tabAbout
            // 
            this.tabAbout.Controls.Add(this.tbLicense);
            this.tabAbout.Location = new System.Drawing.Point(4, 44);
            this.tabAbout.Name = "tabAbout";
            this.tabAbout.Padding = new System.Windows.Forms.Padding(3);
            this.tabAbout.Size = new System.Drawing.Size(1084, 747);
            this.tabAbout.TabIndex = 7;
            this.tabAbout.Text = "About";
            this.tabAbout.UseVisualStyleBackColor = true;
            // 
            // tbLicense
            // 
            this.tbLicense.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLicense.Location = new System.Drawing.Point(3, 3);
            this.tbLicense.Margin = new System.Windows.Forms.Padding(6);
            this.tbLicense.Multiline = true;
            this.tbLicense.Name = "tbLicense";
            this.tbLicense.ReadOnly = true;
            this.tbLicense.Size = new System.Drawing.Size(1078, 741);
            this.tbLicense.TabIndex = 0;
            // 
            // tabPBIMapping
            // 
            this.tabPBIMapping.Controls.Add(this.pbiMappingPage1);
            this.tabPBIMapping.Location = new System.Drawing.Point(4, 44);
            this.tabPBIMapping.Name = "tabPBIMapping";
            this.tabPBIMapping.Padding = new System.Windows.Forms.Padding(3);
            this.tabPBIMapping.Size = new System.Drawing.Size(1084, 747);
            this.tabPBIMapping.TabIndex = 15;
            this.tabPBIMapping.Text = "PBI to Module Mapping";
            this.tabPBIMapping.UseVisualStyleBackColor = true;
            // 
            // pbiMappingPage1
            // 
            this.pbiMappingPage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbiMappingPage1.Location = new System.Drawing.Point(3, 3);
            this.pbiMappingPage1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pbiMappingPage1.Name = "pbiMappingPage1";
            this.pbiMappingPage1.Size = new System.Drawing.Size(1078, 741);
            this.pbiMappingPage1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1092, 795);
            this.Controls.Add(this.appTabs);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RFID Gin Data Management";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.appTabs.ResumeLayout(false);
            this.tabHome.ResumeLayout(false);
            this.tabPickupLists.ResumeLayout(false);
            this.tabGinLoads.ResumeLayout(false);
            this.tabModules.ResumeLayout(false);
            this.tabBales.ResumeLayout(false);
            this.tabClients.ResumeLayout(false);
            this.tabFarms.ResumeLayout(false);
            this.tabFields.ResumeLayout(false);
            this.tabDrivers.ResumeLayout(false);
            this.tabTrucks.ResumeLayout(false);
            this.tabReports.ResumeLayout(false);
            this.tabReports.PerformLayout();
            this.tabSettings.ResumeLayout(false);
            this.tabSettings.PerformLayout();
            this.tabAbout.ResumeLayout(false);
            this.tabAbout.PerformLayout();
            this.tabPBIMapping.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl appTabs;
        private System.Windows.Forms.TabPage tabHome;
        private System.Windows.Forms.TabPage tabPickupLists;
        private System.Windows.Forms.TabPage tabReports;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.TabPage tbReports;
        private System.Windows.Forms.TabPage tabSetting1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel33;
        private UserControls.ReportPage reportPage1;
        private System.Windows.Forms.TabPage tabModules;
        private UserControls.ModulesPage modulesPage1;
        private System.Windows.Forms.TabPage tabAbout;
        private System.Windows.Forms.TextBox tbLicense;
        private UserControls.PickupListsPage pickupListsPage1;
        private UserControls.SettingsPage settingsPage;
        private System.Windows.Forms.TabPage tabClients;
        private System.Windows.Forms.TabPage tabFarms;
        private System.Windows.Forms.TabPage tabFields;
        private System.Windows.Forms.TabPage tabDrivers;
        private System.Windows.Forms.TabPage tabTrucks;
        private UserControls.TrucksPage trucksPage;
        private UserControls.DriversPage driversPage;
        private UserControls.ClientsPage clientsPage;
        private UserControls.FarmsPage farmsPage;
        private UserControls.FieldsPage fieldsPage;
        private UserControls.HomePage homePage1;
        private System.Windows.Forms.TabPage tabGinLoads;
        private UserControls.GinLoadsPage ginLoadsPage1;
        private System.Windows.Forms.TabPage tabBales;
        private UserControls.BalesPage balesPage;
        private System.Windows.Forms.TabPage tabPBIMapping;
        private UserControls.PBIMappingPage pbiMappingPage1;
    }
}

