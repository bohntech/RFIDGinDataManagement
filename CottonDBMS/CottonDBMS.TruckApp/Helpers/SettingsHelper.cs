//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using CottonDBMS.Interfaces;
using CottonDBMS.DataModels;
using Newtonsoft.Json;

namespace CottonDBMS.TruckApp.Helpers
{
    public static class SettingsHelper
    {
        public static void PersistSettingsToAppData()
        {

            using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
            {
                PersistSettingsToAppData(dp);
            }                
        }

        public static void PersistSettingsToAppData(IUnitOfWork dp)
        {
            string endpoint = dp.SettingsRepository.GetSettingWithDefault(TruckClientSettingKeys.DOCUMENTDB_ENDPOINT, "");
            string key = dp.SettingsRepository.GetSettingWithDefault(TruckClientSettingKeys.DOCUMENT_DB_KEY, "");
            string truckid = dp.SettingsRepository.GetSettingWithDefault(TruckClientSettingKeys.TRUCK_ID, "");

            //write settings file to app data this way we have
            //access to keys to delete truckdownloaded pickup lists from the cloud
            TruckAppInstallParams parms = new TruckAppInstallParams();
            parms.EndPoint = endpoint;
            parms.Key = key;
            parms.TruckID = truckid;
            var dataString = CottonDBMS.Helpers.EncryptionHelper.Encrypt(Newtonsoft.Json.JsonConvert.SerializeObject(parms));
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            dir = dir.TrimEnd('\\') + "\\"+FolderConstants.ROOT_APP_DATA_FOLDER+"\\" + FolderConstants.TRUCK_APP_DATA_FOLDER;

            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }

            System.IO.File.WriteAllText(dir + "\\settings.txt", dataString);
        }
    }
}
