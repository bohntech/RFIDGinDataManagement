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
using System.IO;
using CottonDBMS.DataModels;
using CottonDBMS.GinApp.Helpers;
using Newtonsoft.Json;
using Newtonsoft;
using CottonDBMS.Interfaces;

namespace CottonDBMS.GinApp.Dialogs
{
    public partial class CreateTruckInstallPackageDialog : Form
    {
        public CreateTruckInstallPackageDialog()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void CreateTruckInstallPackageDialog_Load(object sender, EventArgs e)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            cbTargetDrive.Items.Clear();
            foreach (DriveInfo d in allDrives)
            {
                if (d.DriveType == DriveType.Removable)
                {
                    cbTargetDrive.Items.Add(d.RootDirectory.Name);
                }
            }

            if (cbTargetDrive.Items.Count > 0)
            {
                cbTargetDrive.SelectedIndex = 0;
            }          
           
        }

        private void btnSavePackage_Click(object sender, EventArgs e)
        {
            try
            {
                //write contents of TruckInstallFolder to drive
                var truckInstallPackageDir = AppDomain.CurrentDomain.BaseDirectory + "\\TruckInstall";
                string targetDir = ((string)cbTargetDrive.SelectedItem).TrimEnd("\\".ToCharArray());

                var soureDI = new DirectoryInfo(truckInstallPackageDir);
                foreach (var f in soureDI.EnumerateFiles())
                {
                    File.Copy(f.FullName, targetDir + "\\" + f, true);
                }

                //write settings file
                using (IUnitOfWork uow = UnitOfWorkFactory.CreateUnitOfWork())
                {
                    var settingsRepo = uow.SettingsRepository;
                    string endpoint = settingsRepo.FindSingle(t => t.Key == GinAppSettingKeys.AZURE_DOCUMENTDB_ENDPOINT).Value;
                    string key = settingsRepo.FindSingle(t => t.Key == GinAppSettingKeys.AZURE_DOCUMENTDB_KEY).Value;
                    TruckAppInstallParams parms = new TruckAppInstallParams();
                    parms.EndPoint = endpoint;
                    parms.Key = key;
                    //parms.LockCode = tbLockCode.Text;
                    var dataString = CottonDBMS.Helpers.EncryptionHelper.Encrypt(Newtonsoft.Json.JsonConvert.SerializeObject(parms));
                    File.WriteAllText(targetDir + "\\settings.txt", dataString);
                }
                MessageBox.Show("Installer files have been successfully created.  To begin truck installation boot the Truck system, plug in the removable drive, and run the 'setup.exe' file that has been saved on the removable drive.");

                this.Close();
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                MessageBox.Show("An error occurred creating truck install package.");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
