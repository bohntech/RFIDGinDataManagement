//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using CottonDBMS.DataModels;
using CottonDBMS.Interfaces;
using System.Diagnostics;
using CottonDBMS.EF;
using CottonDBMS.DataModels.Helpers;

namespace CottonDBMS.Bridges.Shared.Tasks
{
    public static class BridgeSyncProcessHelper
    {
        public static System.Timers.Timer timer = null;

        private static string _appName = "";

        public static bool Initialized
        {
            get
            {
                return timer != null;
            }
        }

        public static void SetAppName(string appName)
        {
            _appName = appName;
        }

        public static void Init()
        {
            Init(_appName);
        }

        public static void Init(string appName)
        {
            _appName = appName;
            using (IUnitOfWork uow = new UnitOfWork())
            {
                var interval = uow.SettingsRepository.GetSettingDoubleValue(BridgeSettingKeys.DATA_SYNC_INTERVAL);
                timer = new System.Timers.Timer(interval * 1000 * 60);
                timer.AutoReset = false;
                timer.Start();
                timer.Elapsed += Timer_Elapsed;
            }
        }

        public static bool SyncProcessRunning()
        {
            Process thisProcess = Process.GetCurrentProcess();
            Process[] matchingProcs = Process.GetProcesses();
            bool processRunning = false;
            foreach (Process p in matchingProcs.Where(p => p.ProcessName.IndexOf(_appName + ".Sync") >= 0))
            {
                processRunning = true;
            }
            return processRunning;
        }

        public static void RunSync(string executionLocation, bool showWindow)
        {
            string location = executionLocation.Replace("CottonDBMS.Bridges.Shared.dll", "CottonDBMS." + _appName + ".exe");            
            if (location.IndexOf(".Uninstall") > 0)
            {
                location = location.Replace(_appName + ".Uninstall", _appName + ".Sync");
            }
            else
            {
                location = location.Replace(_appName, _appName + ".Sync");
            }

            Process syncProcess = new Process();
            syncProcess.StartInfo.FileName = location;
            if (showWindow)
                syncProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            else
                syncProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            syncProcess.Start();
            syncProcess.WaitForExit();
        }

        public static void WaitForSyncToStop()
        {
            int waitCount = 0;

            while (SyncProcessRunning())
            {
                System.Threading.Thread.Sleep(5000);
                waitCount++;
            }
        }

        private static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (!SyncProcessRunning())
                {
                    RunSync(System.Reflection.Assembly.GetExecutingAssembly().Location, false);
                    WaitForSyncToStop();
                    //Messenger.Default.Send<DataRefreshedMessage>(new DataRefreshedMessage());
                }                
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }

            using (IUnitOfWork uow = new UnitOfWork())
            {
                var interval = uow.SettingsRepository.GetSettingDoubleValue(BridgeSettingKeys.DATA_SYNC_INTERVAL);
                timer.Interval = interval * 1000 * 60;
                timer.Start();
            }
        }

        public static void Cancel()
        {
            if (timer != null)
                timer.Stop();
        }
    }
}
