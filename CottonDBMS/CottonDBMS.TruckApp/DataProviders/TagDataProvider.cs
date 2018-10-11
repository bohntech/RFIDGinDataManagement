//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Impinj.OctaneSdk;
using System.Net.NetworkInformation;
using GalaSoft.MvvmLight.Messaging;
using System.Collections;
using CottonDBMS.DataModels;

namespace CottonDBMS.TruckApp.DataProviders
{
    public class TagItem
    {
        private string ExtractJohnDeereSN(string hex)
        {
            //extract serial number
            string serialHex = hex.Substring(15);
            return Convert.ToInt64(serialHex, 16).ToString();
        }

        private bool IsJohnDeereEncoding(string hex)
        {
            return hex.StartsWith("3500B9880611");
        }

        public string Epc { get; set; }

        public uint AntennaePort { get; set; }

        public string SerialNumber
        {
            get
            {
                if (IsJohnDeereEncoding(Epc))
                {
                    return ExtractJohnDeereSN(Epc);
                }
                else
                {
                    return Epc;
                }
            }
        }
        
        public DateTime Firstseen { get; set; }

        public DateTime Lastseen { get; set; }
    }
        

    public class TagDataProvider
    {
        private static TagDataProvider instance = new TagDataProvider();        
        private const string Hostname = "169.254.1.1";
        private ImpinjReader reader = new ImpinjReader();
        
        private Dictionary<string, TagItem> tagsRead = new Dictionary<string, TagItem>();
        private bool readCycleStarted = false;
        private DateTime sendEventsAfter = DateTime.Now;
        //private System.Timers.Timer queryTimer;

        private string settingsFileName = "";
                        
        static TagDataProvider() {            
            instance.settingsFileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\"+FolderConstants.ROOT_APP_DATA_FOLDER+"\\"+FolderConstants.TRUCK_APP_DATA_FOLDER+"\\readersettings.xml";
        }

        public static void Initialize()
        {
            // instance.queryTimer = new System.Timers.Timer(250);
            //  instance.queryTimer.Stop();
            // instance.queryTimer.Elapsed += instance.QueryTimer_Elapsed;
            //instance.reader.TagsReported += instance.Reader_TagsReported;
        }

        /*private void processPendingTags()
        {
            instance.reader.QueryTags();
        }*/

        /* private void QueryTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
         {
             lock (instance)
             {
                 instance.queryTimer.Stop();
                 instance.processPendingTags();
                 instance.queryTimer.Start();
             }
         }*/

        private void Reader_TagsReported(ImpinjReader reader, TagReport report)
        {
            if (DateTime.UtcNow < instance.sendEventsAfter) return; //discard events if before time delay has elapsed

            Logging.Logger.Log("RFID", "Tags reported.");
            foreach (var t in report.Tags.OrderBy(t => t.FirstSeenTime))
            {                
                
                                
                TagItem item = new TagItem { Epc = t.Epc.ToHexString(), AntennaePort=t.AntennaPortNumber, Firstseen = t.FirstSeenTime.LocalDateTime.ToUniversalTime(), Lastseen = t.LastSeenTime.LocalDateTime.ToUniversalTime() };
                if (!tagsRead.ContainsKey(item.SerialNumber))
                {
                    Logging.Logger.Log("RFID", "Tag found in dictionary " + item.SerialNumber + " " + item.Epc + " " + item.Firstseen.ToLocalTime().ToString() + " last seen " + item.Lastseen.ToLocalTime().ToString() + " Port: " + item.AntennaePort.ToString());
                    lock (tagsRead)
                    {
                        tagsRead.Add(item.SerialNumber, item);
                    }
                    List<TagItem> list = new List<TagItem>();
                    list.Add(item);

                    Task.Run(() =>
                    {
                        Messenger.Default.Send<List<TagItem>>(list);
                    });
                }
                else
                {
                    Logging.Logger.Log("RFID", "Tag first report " + item.SerialNumber + " " + item.Epc + " " + item.Firstseen.ToLocalTime().ToString() + " last seen " + item.Lastseen.ToLocalTime().ToString() + " Port: " + item.AntennaePort.ToString());
                    lock (tagsRead)
                    {
                        var temp = tagsRead[item.SerialNumber];
                        temp.Lastseen = item.Lastseen;
                    }
                }
            }
        }

        private static void ConnectNoLoc()
        {
            if (!instance.reader.IsConnected)
            {
                instance.reader.Connect(Hostname);
                instance.reader.KeepaliveReceived -= Reader_KeepaliveReceived1;
                instance.reader.KeepaliveReceived += Reader_KeepaliveReceived1;               
            }
        }

        public static void SyncReaderTime()
        {
            try
            {
                lock (instance)
                {
                    string reply;

                    // Open up an RShell connection on the reader.
                    // Specify the reader address, user name, password and connection timeout.
                    instance.reader.RShell.OpenSecureSession(Hostname, "root", "impinj", 5000);

                    DateTime currentTime = DateTime.UtcNow;
                    while (currentTime.Millisecond > 5)
                    {
                        System.Threading.Thread.Sleep(1000 - currentTime.Millisecond);
                        currentTime = DateTime.UtcNow;
                    }

                    string time = currentTime.ToString("MM.dd-HH:mm:ss");

                    // Send an RShell command
                    RShellCmdStatus statusNTP = instance.reader.RShell.Send("config network ntp disable", out reply);

                    RShellCmdStatus status = instance.reader.RShell.Send("config system time " + time, out reply);

                    // Check the status
                    if (status == RShellCmdStatus.Success)
                    {
                        Logging.Logger.Log("RFID", "RShell command executed successfully.\n");
                    }
                    else
                    {
                        Logging.Logger.Log("RFID", "RShell command failed to execute.\n");
                    }

                    // Print out the entire reply.
                    Logging.Logger.Log("RFID", "RShell command reply : \n\n" + reply + "\n");

                    // Close the RShell connection.
                    instance.reader.RShell.Close();
                }
            }
            catch (OctaneSdkException e)
            {
                // Handle Octane SDK errors.
                Logging.Logger.Log(e);
            }
        }

        public static void Connect()
        {
            try
            {
                lock (instance)
                {
                    ConnectNoLoc();
                }                
            }
            catch (OctaneSdkException sdkExc)
            {
                System.Diagnostics.Trace.Write(sdkExc.Message);
            }
        }

        public static void ClearBuffer()
        {
            lock (instance.tagsRead)
            {
                Logging.Logger.Log("RFID", "Clearing tag provider buffer ending");
                instance.tagsRead.Clear();
            }
        }

        private static void Reader_KeepaliveReceived1(ImpinjReader reader)
        {
            try
            {
                //reader.QueryStatus();
            }
            catch(Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }

        private static void Reader_ConnectionLost(ImpinjReader reader)
        {
            Logging.Logger.Log("WARNING", "Reader connection lost.");
        }

        private static void Reader_KeepaliveReceived(ImpinjReader reader)
        {          
            System.Diagnostics.Trace.WriteLine("Reader keep alive received");
        }

        public static List<TagItem> GetTagsFirstSeenInTimeRange(DateTime start, DateTime end)
        {
            lock (instance.tagsRead)
            {
                return instance.tagsRead.OrderBy(t => t.Value.Firstseen).Where(t => t.Value.Firstseen >= start && t.Value.Firstseen <= end).Select(t => t.Value).ToList();
            }
        }

        public static List<TagItem> GetTagsLastSeenInTimeRange(DateTime start, DateTime end)
        {
            lock (instance.tagsRead)
            {
                return instance.tagsRead.OrderBy(t => t.Value.Firstseen).Where(t => t.Value.Lastseen >= start && t.Value.Lastseen <= end).Select(t => t.Value).ToList();
            }
        }

        /*public static List<TagItem> GetTagsInTimeRange(string SerialNumber, DateTime start, DateTime end)
        {
            lock (instance.tagQueue)
            {
                return instance.tagQueue.OrderBy(t => t.TimeStamp).Where(t => t.SerialNumber == SerialNumber && t.TimeStamp >= start && t.TimeStamp <= end).ToList();
            }
        }*/

        public static async Task<Settings> GetSettingsAsync()
        {
            Settings settings = null;

            await Task.Run(() => { 
                 settings = GetSettings();

                 
            });

            return settings;
        }

        public static Settings GetSettings()
        {
            lock (instance)
            {
                Settings settings = null;
                try
                {
                    if (!instance.reader.IsConnected)
                    {
                        ConnectNoLoc();
                    }

                    if (System.IO.File.Exists(instance.settingsFileName))
                    {
                        settings = Settings.Load(instance.settingsFileName);
                        instance.reader.ApplySettings(settings);
                    }
                    else
                    {
                        settings = instance.reader.QuerySettings();
                    }
                }
                catch (OctaneSdkException exc)
                {
                    Logging.Logger.Log("ERROR", "Error retrieving settings: " + exc.Message);
                }
                catch (NullReferenceException exc)
                {
                    Logging.Logger.Log("ERROR", "Error retrieving settings: " + exc.Message);
                    try
                    {
                        settings = instance.reader.QueryDefaultSettings();
                        settings.Antennas.TxPowerMax = false;
                        settings.Antennas.RxSensitivityMax = false;
                        settings.HoldReportsOnDisconnect = false;
                        settings.Report.Mode = ReportMode.Individual;                    
                        settings.SearchMode = SearchMode.DualTarget;
                        settings.ReaderMode = ReaderMode.AutoSetStaticDRM;
                        settings.Report.IncludeFirstSeenTime = true;
                        settings.Report.IncludeLastSeenTime = true;
                        settings.Report.IncludeSeenCount = true;
                        settings.Keepalives.Enabled = true;
                        settings.Keepalives.EnableLinkMonitorMode = true;
                        settings.Keepalives.LinkDownThreshold = 5;
                        settings.Keepalives.PeriodInMs = 3000;

                        foreach(AntennaConfig a in settings.Antennas)
                        {
                            a.RxSensitivityInDbm = -50;
                            a.TxPowerInDbm = 23.5; 
                        }

                        instance.reader.ApplySettings(settings);
                        settings.Save(instance.settingsFileName);
                    }
                    catch (Exception exc2)
                    {
                        Logging.Logger.Log(exc2);
                    }
                }

                return settings;
            }
        }

        public static Settings GetDefaultSettings()
        {
            lock (instance)
            {
                Settings settings = null;
                try
                {
                    if (!IsConnected)
                    {
                        ConnectNoLoc();
                    }
                    settings = instance.reader.QueryDefaultSettings();
                }
                catch (OctaneSdkException exc)
                {
                    System.Diagnostics.Trace.WriteLine("Error retrieving settings: " + exc.Message);
                }
                /*catch (NullReferenceException exc)
                {
                    System.Diagnostics.Trace.WriteLine("Error retrieving settings: " + exc.Message);
                    try
                    {
                        settings = instance.reader.QueryDefaultSettings();
                        settings.Antennas.TxPowerMax = false;
                        settings.Antennas.RxSensitivityMax = false;
                        settings.HoldReportsOnDisconnect = false;
                        settings.Keepalives.Enabled = true;
                        settings.Keepalives.EnableLinkMonitorMode = true;
                        settings.Keepalives.LinkDownThreshold = 10;
                        settings.Keepalives.PeriodInMs = 1000;
                        settings.SearchMode = SearchMode.DualTarget;
                        settings.Report.Mode = ReportMode.WaitForQuery;
                        //settings.Session = 2;
                        settings.LowDutyCycle.IsEnabled = false;
                        instance.reader.ApplySettings(settings);
                    }
                    catch (Exception exc2)
                    {
                        Logging.Logger.Log(exc2);
                    }
                }*/

                return settings;
            }
        }

        public static void Start(int delaySeconds)
        {
            
            lock (instance.tagsRead) {
                instance.tagsRead.Clear();
            }

            lock (instance)
            {
                instance.readCycleStarted = true;
                instance.sendEventsAfter = DateTime.UtcNow.AddSeconds(delaySeconds);
                try
                {

                    instance.reader.TagsReported -= instance.Reader_TagsReported;
                    instance.reader.TagsReported += instance.Reader_TagsReported;

                    ConnectNoLoc();
                                        
                    if (!instance.reader.QueryStatus().IsSingulating)
                    {                     
                        instance.reader.Start();
                        Logging.Logger.Log("RFID", "Starting reader");
                    }
                }
                catch (Exception ex)
                {
                    GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<TruckApp.Messages.ReaderExceptionMessage>(new Messages.ReaderExceptionMessage { Message = ex.Message });

                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        System.Windows.MessageBox.Show("Error starting reader: " + ex.Message);
                    }));

                    Logging.Logger.Log(ex);
                }
            }
        }

        /*public static void EnableMessages()
        {
            instance.tagsRead.Clear();
            instance.sendmessages = true;
        }

        public static void DisableMessages()
        {
            instance.sendmessages = false;
        }*/

        /*public static void EnableEvents()
        {
            lock (instance)
            {
                instance.queryTimer.Start();
            }
        }*/

        /*public static void DisableEvents()
        {
            lock (instance)
            {
                instance.queryTimer.Stop();
                instance.processPendingTags();
            }
        }*/

        public static bool IsInReadCycle()
        {
            lock (instance)
            {
                return instance.readCycleStarted;
            }
        }

        public static void Stop()
        {
            lock (instance)
            {
                
                try
                {
                    if (instance.reader.QueryStatus().IsSingulating)
                    {
                        instance.reader.Stop();
                        Logging.Logger.Log("RFID", "Read cycle ending");
                    }
                    instance.reader.TagsReported -= instance.Reader_TagsReported;
                    instance.readCycleStarted = false;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine("An exception occurred stopping read operations : {0}", ex.Message);
                }
            }
        }

        public static async Task ApplySettingsAsync(Settings newSettings)
        {
            await Task.Run(() =>
            {
                ApplySettings(newSettings);
            });
        }

        public static void ApplySettings(Settings newSettings)
        {
            lock (instance)
            {
                try
                {
                    if (!IsConnected)
                    {
                        ConnectNoLoc();
                    }

                    newSettings.Save(instance.settingsFileName);
                    instance.reader.ApplySettings(newSettings);
                    instance.reader.SaveSettings();                    
                }
                catch (OctaneSdkException sdkExc)
                {
                    System.Diagnostics.Trace.WriteLine("An error occurred appying new settings. " + sdkExc.Message);
                }
            }
        }

        public static bool IsConnected
        {
            get
            {
                return instance.reader.IsConnected;
            }
        }
        
        public static void Disconnect()
        {
            lock (instance)
            {
                try
                {
                    try
                    {
                        if (instance.reader.QueryStatus().IsSingulating)
                        {
                            instance.reader.Stop();
                            System.Threading.Thread.Sleep(500);
                        }
                    }
                    catch (OctaneSdkException sdkExc)
                    {
                        Logging.Logger.Log("ERROR", "An error occurred querying reader status. " + sdkExc.Message);
                    }                                                   
                    instance.reader.KeepaliveReceived -= Reader_KeepaliveReceived1;                    
                    instance.reader.TagsReported -= instance.Reader_TagsReported;
                    instance.reader.Disconnect();                                        
                }
                catch (OctaneSdkException exc)
                {
                    Logging.Logger.Log("ERROR", "An error occurred disconnecting from reader. " + exc.Message);
                }
            }
        }               
        
    }   
}
