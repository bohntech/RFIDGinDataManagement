//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace CottonDBMS.DataModels.Helpers
{
    public static class GPSHelper
    {
        public static double SafeLong(GpsCoords coords)
        {
            return (coords != null) ? coords.NonNullLongitude : 0.000;
        }

        public static double SafeLat(GpsCoords coords)
        {
            return (coords != null) ? coords.NonNullLatitude : 0.000;
        }
    }

    public static class NetworkHelper
    {
        public static bool HasNetwork()
        {
            bool _hasNetwork = false;           
            try
            {
                using (var webClient = new System.Net.WebClient())
                {
                    using (webClient.OpenRead("http://google.com"))
                    {
                        _hasNetwork = true;
                    }
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                _hasNetwork = false;
            }

            return _hasNetwork;           
        }

        public static bool SyncProcessRunning()
        {
            Process thisProcess = Process.GetCurrentProcess();
            Process[] matchingProcs = Process.GetProcesses();
            bool processRunning = false;
            foreach (Process p in matchingProcs.Where(p => p.ProcessName.IndexOf("TruckApp.Sync") >= 0))
            {               
              processRunning = true;
            }

            return processRunning;
        }

        public static void RunSync(string executionLocation, bool showWindow)
        {
            string location = executionLocation;

            if (location.IndexOf(".Uninstall") > 0)
            {
                location = location.Replace("TruckApp.Uninstall", "TruckApp.Sync");
            }
            else
            {
                location = location.Replace("TruckApp", "TruckApp.Sync");
            }

            Console.WriteLine(location);
            
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
                //Logging.Logger.Log("INFO", "Waiting for sync to stop.");
                System.Threading.Thread.Sleep(5000);
                waitCount++;
            }
        }
    }
}
