//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CottonDBMS.EF;
using CottonDBMS.Interfaces;
using CottonDBMS.DataModels;
using CottonDBMS.Cloud;
using CottonDBMS.GinApp.Classes;
using GalaSoft.MvvmLight.Messaging;
using CottonDBMS.GinApp.Dialogs;
using System.Runtime.InteropServices;

namespace CottonDBMS.GinApp
{
    public partial class MainForm : Form
    {        
        bool DbIntitialized = false;
        FirstRunDialog setupForm = null;

        public MainForm()
        {
            InitializeComponent();

            BusyMessage.OnBusyMessageClosed += BusyMessage_OnBusyMessageClosed;
            BusyMessage.OnBusyMessageShown += BusyMessage_OnBusyMessageShown;
        }

        private void BusyMessage_OnBusyMessageShown(object sender, EventArgs e)
        {
            appTabs.Enabled = false;
        }

        private void BusyMessage_OnBusyMessageClosed(object sender, EventArgs e)
        {
            appTabs.Enabled = true;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// This method brings the appropriate window to the foreground
        /// </summary>
        public void focusApplication()
        {
            if (setupForm != null && setupForm.Visible)
            {
                this.WindowState = FormWindowState.Normal;
                SetForegroundWindow(this.Handle);
                setupForm.WindowState = FormWindowState.Normal;
                SetForegroundWindow(setupForm.Handle);
            }
            else
            {
                Show();
                this.WindowState = FormWindowState.Minimized;
                SetForegroundWindow(this.Handle);
            }
        }

        private void runSetup()
        {
            try
            {
                using (IUnitOfWork uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    var setupWizardSetting = uow.SettingsRepository.GetSettingWithDefault(GinAppSettingKeys.SETUP_WIZARD_COMPLETE, "");

                    if (string.IsNullOrEmpty(setupWizardSetting))
                    {
                        //wizard hasn't been completed so let's run it
                        Logging.Logger.Log("INFO", "Launching setup form");                      
                        var setupForm = new FirstRunDialog();
                        focusApplication();
                        if (setupForm.ShowDialog() == DialogResult.OK)
                        {
                            Show();
                            this.WindowState = FormWindowState.Maximized;
                            SetForegroundWindow(this.Handle);
                        }
                        else {
                            Application.Exit();
                        }
                    }
                    else
                    {
                        Show();
                        this.WindowState = FormWindowState.Maximized;
                        SetForegroundWindow(this.Handle);
                    }
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                MessageBox.Show("An error occured in setup wizard");
                Application.Exit();
            }         
        }

        private async Task InitializeDataSourcesAsync()
        {           

            using (IUnitOfWork uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                BusyMessage.Show("Checking for Azure database.", this);
                var settingsRepo = uow.SettingsRepository;
                string endpoint = settingsRepo.FindSingle(t => t.Key == GinAppSettingKeys.AZURE_DOCUMENTDB_ENDPOINT).Value;
                string key = settingsRepo.FindSingle(t => t.Key == GinAppSettingKeys.AZURE_DOCUMENTDB_KEY).Value;

                if (endpoint.ToLower().IndexOf("azure.com") >= 0 && !string.IsNullOrEmpty(key))
                {
                    DocumentDBContext.Initialize(endpoint, key);
                    bool exists = await DocumentDBContext.DatabaseExistsAsync();
                    bool createSucceded = true;
                    if (!exists)
                    {
                        BusyMessage.UpdateMessage("Creating Azure document database..");
                        try
                        {
                            await DocumentDBContext.CreateDatabaseAsync();
                        }
                        catch (Exception exc)
                        {
                            Logging.Logger.Log(exc);
                            createSucceded = false;
                            BusyMessage.UpdateMessage("Creating Azure document database failed.");
                            System.Threading.Thread.Sleep(2000);
                        }
                    }

                    if (createSucceded)
                    {
                        exists = await DocumentDBContext.CollectionExistsAsync();
                        if (!exists)
                        {
                            try
                            {
                                BusyMessage.UpdateMessage("Creating Azure document database collections..");
                                await DocumentDBContext.CreateCollectionAsync();
                                createSucceded = true;

                            }
                            catch (Exception exc)
                            {
                                BusyMessage.UpdateMessage("Creating Azure document database collections failed.");
                                System.Threading.Thread.Sleep(2000);
                                Logging.Logger.Log(exc);
                                createSucceded = false;
                            }
                        }
                    }

                    if (createSucceded)
                    {
                        try
                        {
                            await DocumentDBContext.CreateStoredProceduresAsync();
                        }
                        catch (Exception exc)
                        {
                            BusyMessage.UpdateMessage("Creating database stored procedures failed.");
                            System.Threading.Thread.Sleep(2000);
                            Logging.Logger.Log(exc);
                            createSucceded = false;
                        }
                    }

                    DbIntitialized = true;
                    BusyMessage.Close();
                }
                else
                {
                    BusyMessage.Close();
                    DbIntitialized = false;
                    appTabs.TabPages.Remove(tabHome);
                    appTabs.TabPages.Remove(tabFields);
                    appTabs.TabPages.Remove(tabModules);
                    appTabs.TabPages.Remove(tabPickupLists);
                    appTabs.TabPages.Remove(tabClients);
                    appTabs.TabPages.Remove(tabFarms);
                    appTabs.TabPages.Remove(tabTrucks);
                    appTabs.TabPages.Remove(tabDrivers);
                    appTabs.TabPages.Remove(tabReports);
                    appTabs.TabIndex = 0;
                    MessageBox.Show("Azure Document DB connection settings are missing or incorrect.  Please update Azure information in the Settings tab then restart the application.");
                }
            }
            
        }
        
        private async void MainForm_Load(object sender, EventArgs e)
        {

            if (!Helpers.NetworkHelper.HasNetwork())
            {
                MessageBox.Show("An internet connection is needed to use this application.");

                Application.Exit();
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
                this.Visible = false;
                runSetup();
                this.Visible = true;

                try
                {
                    await InitializeDataSourcesAsync();
                    if (DbIntitialized)
                        homePage1.LoadData();
                }
                catch (Exception exc)
                {
                    MessageBox.Show("An error occurred trying to initialize application. See error log for details.");
                    Logging.Logger.Log(exc);
                }
            }

            string license =
@"MIT License

Copyright (c) 2018  United States Department of Agriculture,  Agricultural Research Service

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the ""Software""), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/ or sell
 copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.";

            tbLicense.Text = license;
            
        }

        private void appTabs_Selected(object sender, TabControlEventArgs e)
        {
            if (appTabs.TabPages.IndexOf(tabSettings) == appTabs.SelectedIndex)
            {
                 settingsPage.LoadData();
            }
            else if (appTabs.TabPages.IndexOf(tabTrucks) == appTabs.SelectedIndex)
            {
                trucksPage.LoadData();
            }
            else if (appTabs.TabPages.IndexOf(tabDrivers) == appTabs.SelectedIndex)
            {
                driversPage.LoadData();
            }
            else if (appTabs.TabPages.IndexOf(tabClients) == appTabs.SelectedIndex)
            {
                clientsPage.LoadData();
            }
            else if (appTabs.TabPages.IndexOf(tabFarms) == appTabs.SelectedIndex)
            {
                farmsPage.LoadData();
            }
            else if (appTabs.TabPages.IndexOf(tabFields) == appTabs.SelectedIndex)
            {
                fieldsPage.LoadData();
            }
            else if (appTabs.TabPages.IndexOf(tabModules) == appTabs.SelectedIndex)
            {
                modulesPage1.LoadData();
            }
            else if (appTabs.TabPages.IndexOf(tabHome) == appTabs.SelectedIndex)
            {
                 homePage1.LoadData();
            }
            else if (appTabs.TabPages.IndexOf(tabPickupLists) == appTabs.SelectedIndex)
            {
                 pickupListsPage1.LoadData();
            }
            else if (appTabs.TabPages.IndexOf(tabReports) == appTabs.SelectedIndex)
            {
                 reportPage1.LoadData();
            }
            else if (appTabs.TabPages.IndexOf(tabGinLoads) == appTabs.SelectedIndex)
            {
                ginLoadsPage1.LoadData();
            }
            else if (appTabs.TabPages.IndexOf(tabBales) == appTabs.SelectedIndex)
            {
                balesPage.LoadData();
            }
            else if (appTabs.TabPages.IndexOf(tabPBIMapping) == appTabs.SelectedIndex)
            {
                balesPage.LoadData();
            }

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CottonDBMS.EF.Tasks.GinSyncWithCloudTask.Cancel();
            Logging.Logger.CleanUp();

            try
            {
                using (IUnitOfWork uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    uow.BackupDB(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).TrimEnd('\\') + "\\" + FolderConstants.ROOT_APP_DATA_FOLDER + "\\" + FolderConstants.GIN_APP_DATA_FOLDER);
                }
            }
            catch (Exception exc)
            {
                //do nothing logger is cleaned up and disposed
                string s = exc.Message;
            }
        }
    }
}
