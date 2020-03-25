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
using CottonDBMS.EF;
using System.Diagnostics;
using CottonDBMS.Bridges.Shared.Helpers;
using CottonDBMS.Bridges.Shared.Tasks;

namespace CottonDBMS.BridgeApp.Sync
{
    class Program
    {
        static CancellationTokenSource tokenSource = null;
        static CancellationToken token;
        
        static void Main(string[] args)
        {
            if (StartupHelper.alreadyRunningProcess()) Environment.Exit(0);

            SyncHelper.SetEnvironmentPaths(FolderConstants.SCALE_BRIDGE_APP_DATA_FOLDER, FolderConstants.SCALE_BRIDGE_SYNC_APP_DATA_FOLDER);

            Logging.Logger.Log("INFO", "BRIDGE SYNC LAUNCHED");
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;            
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;

            Task callTask = Task.Run(async () =>
            {
                System.Threading.Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                var task = new ScaleBridgeSyncTask(token);
                await task.DoSync();
            });

            callTask.Wait();
            Environment.Exit(0);
        }                    

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            try
            {
                if (tokenSource != null) tokenSource.Cancel();

                Logging.Logger.CleanUp();
            }
            catch (Exception exc)
            {
                string s = exc.Message;
            }
        }
    }
}
