//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.DataModels;
using CottonDBMS.Interfaces;
using System.Windows.Forms;
using System.Windows;

namespace CottonDBMS.GinApp.Helpers
{
    public static class ConfigHelper
    {
        private static string GetSettingWithDefault(string key, string defaultValue)
        {
            using (IUnitOfWork uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                return uow.SettingsRepository.GetSettingWithDefault(key, defaultValue);
            }
        }

        public static DateTime? LastImportDateTime
        {
            get
            {
                string val = GetSettingWithDefault(GinAppSettingKeys.LAST_LOCAL_IMPORT_TIMESTAMP, "");
                if (!string.IsNullOrEmpty(val)) return DateTime.Parse(val);
                else return null;
            }
        }

        public static int ModulesPerLoad
        {
            get
            {
                string val = GetSettingWithDefault(GinAppSettingKeys.MODULES_PER_LOAD, "4");
                return int.Parse(val);
            }
        }

        public static string LoadPrefix
        {
            get
            {
                return GetSettingWithDefault(GinAppSettingKeys.LOAD_PREFIX, "");
            }
        }

        public static int ImportInterval
        {
            get
            {
                string val = GetSettingWithDefault(GinAppSettingKeys.IMPORT_INTERVAL, "5");
                return int.Parse(val);
            }
        }

        public static string LastImportStatus
        {
            get
            {
                return GetSettingWithDefault(GinAppSettingKeys.LAST_IMPORT_STATUS, "");
            }
        }

        public static string ImapHostName
        {
            get
            {
                return GetSettingWithDefault(GinAppSettingKeys.IMAP_HOSTNAME, "");
            }
        }

        public static string ImapPort
        {
            get
            {
                return GetSettingWithDefault(GinAppSettingKeys.IMAP_PORT, "");
            }
        }

        public static string ImapUsername
        {
            get
            {
                return GetSettingWithDefault(GinAppSettingKeys.IMAP_USERNAME, "");
            }
        }

        public static string ImapPassword
        {
            get
            {
                return GetSettingWithDefault(GinAppSettingKeys.IMAP_PASSWORD, "");
            }
        }

        public static string ImportFolder
        {
            get
            {
                return GetSettingWithDefault(GinAppSettingKeys.IMPORT_FOLDER, "");
            }
        }

        public static string GoogleMapsKey
        {
            get
            {
                return GetSettingWithDefault(GinAppSettingKeys.GOOGLE_MAPS_API_KEY, "");
            }
        }

        public static double GinFeederLat
        {
            get
            {
                return double.Parse(GetSettingWithDefault(GinAppSettingKeys.GIN_FEEDER_NORTH, "0"));
            }
        }

        public static double GinFeederLong
        {
            get
            {
                return double.Parse(GetSettingWithDefault(GinAppSettingKeys.GIN_FEEDER_WEST, "0"));
            }
        }

        public static double GinYardNWCornerLat
        {
            get
            {
                return double.Parse(GetSettingWithDefault(GinAppSettingKeys.GIN_YARD_NW_CORNER_NORTH, "0"));
            }
        }

        public static double GinYardNWCornerLong
        {
            get
            {
                return double.Parse(GetSettingWithDefault(GinAppSettingKeys.GIN_YARD_NW_CORNER_WEST, "0"));
            }
        }

        public static double GinYardSECornerLat
        {
            get
            {
                return double.Parse(GetSettingWithDefault(GinAppSettingKeys.GIN_YARD_SE_CORNER_NORTH, "0"));
            }
        }

        public static double GinYardSECornerLong
        {
            get
            {
                return double.Parse(GetSettingWithDefault(GinAppSettingKeys.GIN_YARD_SE_CORNER_WEST, "0"));
            }
        }

        public static string ArchivePath
        {
            get
            {
                return GetSettingWithDefault(GinAppSettingKeys.ARCHIVE_FOLDER, System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments).Trim('\\') + "\\PickupLists").TrimEnd('\\') + "\\";
            }
        }


        public static void SaveSettingsFromUIControls(Label lblImportFilesFolderValue, Label lblArchiveFolderValue,
            TextBox tbIMAPHostname, TextBox tbIMAPPort, TextBox tbIMAPUsername, TextBox tbIMAPPassword, TextBox tbYardNWLatitude,
            TextBox tbYardNWLongitude, TextBox tbYardSELatitude, TextBox tbYardSELongitude, TextBox tbFeederLatitude, TextBox tbFeederLongitude,
            NumericUpDown tbFeederDetectionRadius, TextBox tbMapsAPIKey, NumericUpDown tbImportInterval, TextBox tbAzureCosmosEndpoint,
            TextBox tbAzureKey, TextBox tbAzureCosmosReadOnlyEndPoint, TextBox tbAzureReadOnlyKey, TextBox tbLoadPrefix,
            TextBox tbStartingLoadNumber, NumericUpDown tbModulesPerLoad, TextBox tbGinName)
        {
            using (IUnitOfWork uow = UnitOfWorkFactory.CreateUnitOfWork())
            {
                var repo = uow.SettingsRepository;
                var allSettings = repo.GetAll();

                Setting importFolderSetting = allSettings.Single(s => s.Key == GinAppSettingKeys.IMPORT_FOLDER);
                importFolderSetting.Value = lblImportFilesFolderValue.Text.Trim();

                Setting archiveFolderSetting = allSettings.Single(s => s.Key == GinAppSettingKeys.ARCHIVE_FOLDER);
                archiveFolderSetting.Value = lblArchiveFolderValue.Text.Trim();

                Setting hostnameSetting = allSettings.Single(s => s.Key == GinAppSettingKeys.IMAP_HOSTNAME);
                hostnameSetting.Value = tbIMAPHostname.Text.Trim();

                Setting portSetting = allSettings.Single(s => s.Key == GinAppSettingKeys.IMAP_PORT);
                portSetting.Value = tbIMAPPort.Text.Trim();

                Setting usernameSetting = allSettings.Single(s => s.Key == GinAppSettingKeys.IMAP_USERNAME);
                usernameSetting.Value = tbIMAPUsername.Text.Trim();

                Setting passwordSetting = allSettings.Single(s => s.Key == GinAppSettingKeys.IMAP_PASSWORD);
                passwordSetting.Value = tbIMAPPassword.Text.Trim();

                Setting ginYardNWCornerNorthSetting = allSettings.Single(s => s.Key == GinAppSettingKeys.GIN_YARD_NW_CORNER_NORTH);
                ginYardNWCornerNorthSetting.Value = tbYardNWLatitude.Text.Trim();

                Setting ginYardNWCornerWestSetting = allSettings.Single(s => s.Key == GinAppSettingKeys.GIN_YARD_NW_CORNER_WEST);
                ginYardNWCornerWestSetting.Value = tbYardNWLongitude.Text.Trim();

                Setting ginYardSECornerNorthSetting = allSettings.Single(s => s.Key == GinAppSettingKeys.GIN_YARD_SE_CORNER_NORTH);
                ginYardSECornerNorthSetting.Value = tbYardSELatitude.Text.Trim();

                Setting ginYardSECornerWestSetting = allSettings.Single(s => s.Key == GinAppSettingKeys.GIN_YARD_SE_CORNER_WEST);
                ginYardSECornerWestSetting.Value = tbYardSELongitude.Text.Trim();

                Setting ginFeederNorthSetting = allSettings.Single(s => s.Key == GinAppSettingKeys.GIN_FEEDER_NORTH);
                ginFeederNorthSetting.Value = tbFeederLatitude.Text.Trim();

                Setting ginFeederWestSetting = allSettings.Single(s => s.Key == GinAppSettingKeys.GIN_FEEDER_WEST);
                ginFeederWestSetting.Value = tbFeederLongitude.Text.Trim();

                Setting ginFeederDetectionRadiusSetting = allSettings.Single(s => s.Key == GinAppSettingKeys.GIN_FEEDER_DETECTION_RADIUS);
                ginFeederDetectionRadiusSetting.Value = tbFeederDetectionRadius.Text.Trim();

                Setting googleMapsApiKeySetting = allSettings.Single(s => s.Key == GinAppSettingKeys.GOOGLE_MAPS_API_KEY);
                googleMapsApiKeySetting.Value = tbMapsAPIKey.Text.Trim();

                Setting importScheduleSettingKey = allSettings.Single(s => s.Key == GinAppSettingKeys.IMPORT_INTERVAL);
                importScheduleSettingKey.Value = tbImportInterval.Text.Trim();

                Setting azureEndpointSetting = allSettings.Single(s => s.Key == GinAppSettingKeys.AZURE_DOCUMENTDB_ENDPOINT);
                azureEndpointSetting.Value = tbAzureCosmosEndpoint.Text.Trim();

                Setting azureDocumentDBKeySetting = allSettings.Single(s => s.Key == GinAppSettingKeys.AZURE_DOCUMENTDB_KEY);
                azureDocumentDBKeySetting.Value = tbAzureKey.Text.Trim();

                Setting azureDocumentDBReadOnlyEndpointSetting = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.AZURE_DOCUMENTDB_READONLY_ENDPOINT);
                Setting azureDocumentDBReadOnlyKeySetting = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.AZURE_DOCUMENTDB_READONLY_KEY);

                if (azureDocumentDBReadOnlyEndpointSetting != null)
                {
                    azureDocumentDBReadOnlyEndpointSetting.Value = tbAzureCosmosReadOnlyEndPoint.Text.Trim();
                }
                else
                {
                    azureDocumentDBReadOnlyEndpointSetting = new Setting { Key = GinAppSettingKeys.AZURE_DOCUMENTDB_READONLY_ENDPOINT };
                    azureDocumentDBReadOnlyEndpointSetting.Value = tbAzureCosmosReadOnlyEndPoint.Text.Trim();
                    uow.SettingsRepository.Add(azureDocumentDBReadOnlyEndpointSetting);
                }

                if (azureDocumentDBReadOnlyKeySetting != null)
                {
                    azureDocumentDBReadOnlyKeySetting.Value = tbAzureReadOnlyKey.Text.Trim();
                }
                else
                {
                    azureDocumentDBReadOnlyKeySetting = new Setting { Key = GinAppSettingKeys.AZURE_DOCUMENTDB_READONLY_KEY };
                    azureDocumentDBReadOnlyKeySetting.Value = tbAzureReadOnlyKey.Text.Trim();
                    uow.SettingsRepository.Add(azureDocumentDBReadOnlyKeySetting);
                }

                Setting loadPrefixSetting = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.LOAD_PREFIX);

                if (loadPrefixSetting == null)
                {
                    loadPrefixSetting = new Setting { Key = GinAppSettingKeys.LOAD_PREFIX };
                    uow.SettingsRepository.Add(loadPrefixSetting);
                }
                loadPrefixSetting.Value = tbLoadPrefix.Text.Trim();

                Setting loadNumberSetting = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.STARTING_LOAD_NUMBER);
                if (loadNumberSetting == null)
                {
                    loadNumberSetting = new Setting { Key = GinAppSettingKeys.STARTING_LOAD_NUMBER };
                    uow.SettingsRepository.Add(loadNumberSetting);
                }
                loadNumberSetting.Value = tbStartingLoadNumber.Text.Trim();

                Setting modulesPerLoadSetting = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.MODULES_PER_LOAD);
                if (modulesPerLoadSetting == null)
                {
                    modulesPerLoadSetting = new Setting { Key = GinAppSettingKeys.MODULES_PER_LOAD };
                    uow.SettingsRepository.Add(modulesPerLoadSetting);
                }
                modulesPerLoadSetting.Value = tbModulesPerLoad.Text.Trim();

                Setting ginNameSetting = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.GIN_NAME);
                if (ginNameSetting == null)
                {
                    ginNameSetting = new Setting { Key = GinAppSettingKeys.GIN_NAME };
                    uow.SettingsRepository.Add(ginNameSetting);
                }
                ginNameSetting.Value = tbGinName.Text.Trim();

                Setting firstRunSetting = allSettings.SingleOrDefault(s => s.Key == GinAppSettingKeys.SETUP_WIZARD_COMPLETE);
                if (firstRunSetting == null)
                {
                    firstRunSetting = new Setting { Key = GinAppSettingKeys.SETUP_WIZARD_COMPLETE };
                    uow.SettingsRepository.Add(firstRunSetting);
                }
                firstRunSetting.Value = "1";

                uow.SaveChanges();                
            }

            PersistSettingsToAppData();
        }




        public static void PersistSettingsToAppData()
        {
            using (var dp = UnitOfWorkFactory.CreateUnitOfWork())
            {
                PersistSettingsToAppData(dp);
            }
        }

        public static void PersistSettingsToAppData(IUnitOfWork dp)
        {
            string endpoint = dp.SettingsRepository.GetSettingWithDefault(GinAppSettingKeys.AZURE_DOCUMENTDB_ENDPOINT, "");
            string key = dp.SettingsRepository.GetSettingWithDefault(GinAppSettingKeys.AZURE_DOCUMENTDB_KEY, "");
          

            //write settings file to app data this way we have
            //access to keys to delete truckdownloaded pickup lists from the cloud
            TruckAppInstallParams parms = new TruckAppInstallParams();
            parms.EndPoint = endpoint;
            parms.Key = key;
          
            var dataString = CottonDBMS.Helpers.EncryptionHelper.Encrypt(Newtonsoft.Json.JsonConvert.SerializeObject(parms));
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            dir = dir.TrimEnd('\\') + "\\"+FolderConstants.ROOT_APP_DATA_FOLDER+"\\" + FolderConstants.GIN_APP_DATA_FOLDER;

            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }

            System.IO.File.WriteAllText(dir + "\\settings.txt", dataString);
        }


    }
}
