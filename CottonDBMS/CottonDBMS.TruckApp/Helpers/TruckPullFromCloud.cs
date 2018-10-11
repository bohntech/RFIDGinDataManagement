//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using CottonDBMS.Cloud;
using CottonDBMS.DataModels;
using CottonDBMS.Interfaces;
using System.Diagnostics;
using CottonDBMS.EF;
using CottonDBMS.DataModels.Helpers;
using GalaSoft.MvvmLight.Messaging;
using CottonDBMS.TruckApp.Messages;

namespace CottonDBMS.TruckApp.Tasks
{
    public static class TruckPullFromCloudTask
    {
        public static System.Timers.Timer timer = null;

        public static void Init()
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                var interval = uow.SettingsRepository.GetSettingDoubleValue(TruckClientSettingKeys.DATA_SYNC_INTERVAL);
                timer = new System.Timers.Timer(interval * 1000 * 60);
                timer.AutoReset = false;
                timer.Start();
                timer.Elapsed += Timer_Elapsed;
            }
                        
        }

        private static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (!NetworkHelper.SyncProcessRunning())
                {
                    NetworkHelper.RunSync(System.Reflection.Assembly.GetExecutingAssembly().Location, false);
                    NetworkHelper.WaitForSyncToStop();
                    Messenger.Default.Send<DataRefreshedMessage>(new DataRefreshedMessage());
                }

                using (IUnitOfWork uow = new UnitOfWork())
                {
                    var interval = uow.SettingsRepository.GetSettingDoubleValue(TruckClientSettingKeys.DATA_SYNC_INTERVAL);
                    timer.Interval = interval * 1000 * 60;
                    timer.Start();
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }

        public static void Cancel()
        {
            timer.Stop();
        }
    }
}
