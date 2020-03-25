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

namespace CottonDBMS.RFID
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

        public string HexToAscii(String hexString)
        {
            try
            {
                string ascii = string.Empty;

                for (int i = 0; i < hexString.Length; i += 2)
                {
                    String hs = string.Empty;

                    hs = hexString.Substring(i, 2);
                    uint decval = System.Convert.ToUInt32(hs, 16);
                    char character = System.Convert.ToChar(decval);
                    ascii += character;

                }

                return ascii;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return string.Empty;
        }

        public string HexToIntegerString(string hexString)
        {
            try
            {
                int intValue = int.Parse(hexString, System.Globalization.NumberStyles.HexNumber);
                return intValue.ToString();
            }
            catch (Exception exc)
            {
                Logging.Logger.Log("WARNING", "Hex string " + hexString + " not John Deere format and could not decode as integer will fallback to string.");
                Logging.Logger.Log(exc);
                return "";
            }
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
                else if (Epc.Length % 4 == 0)
                {
                    try
                    {
                        var result = HexToIntegerString(Epc);
                        if (result == string.Empty) return Epc;
                        else return result.Trim('\0');
                    }
                    catch (Exception exc)
                    {
                        Logging.Logger.Log(exc);
                        Console.WriteLine(exc.Message);
                        return Epc;
                    }
                }
                else return Epc;
            }
        }
        
        public DateTime Firstseen { get; set; }

        public DateTime Lastseen { get; set; }

        public double PeakRSSI { get; set; }

        public double PhaseAngle { get; set; }
    }
   
    public class TagDataProvider
    {
        private static TagDataProvider instance = new TagDataProvider();        
        private string Hostname = "169.254.1.1";
        private ImpinjReader reader = new ImpinjReader();
        
        private Dictionary<string, TagItem> tagsRead = new Dictionary<string, TagItem>();
        private bool readCycleStarted = false;
        private DateTime sendEventsAfter = DateTime.Now;
        private DateTime? connectedAt = null;
        //private System.Timers.Timer queryTimer;

        private string settingsFileName = "";
        private bool[] gpoStates = { false, false, false, false };

        private static Random rand = new Random(DateTime.Now.Millisecond);

        static TagDataProvider() {            
            
        }

        public static void SetHostName(string host)
        {
            instance.Hostname = host;
        }

        public static void SetSettingsPath(string folder)
        {
            instance.settingsFileName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + FolderConstants.ROOT_APP_DATA_FOLDER + "\\" + folder + "\\readersettings.xml";
        }

        /*public static void DisconnectIfUptimeLimitReached(int maxUpTimeMinutes)
        {
            lock (instance)
            {
                if (instance.connectedAt.HasValue && instance.reader.IsConnected)
                {
                    if (instance.connectedAt.Value.AddMinutes(maxUpTimeMinutes) < DateTime.Now)
                    {
                        try
                        {
                            try
                            {
                                if (instance.reader.QueryStatus().IsSingulating)
                                {
                                    instance.reader.Stop();
                                    System.Threading.Thread.Sleep(100);
                                }
                            }
                            catch (OctaneSdkException sdkExc)
                            {
                                Logging.Logger.Log("ERROR", "An error occurred querying reader status when recycling. " + sdkExc.Message);
                            }
                            instance.reader.KeepaliveReceived -= Reader_KeepaliveReceived1;
                            instance.reader.TagsReported -= instance.Reader_TagsReported;
                            instance.reader.Disconnect();
                            instance.connectedAt = null;
                            Logging.Logger.Log("INFO", "RECYCLING READER CONNECTION");
                        }
                        catch (OctaneSdkException exc)
                        {
                            Logging.Logger.Log("ERROR", "An error occurred disconnecting from reader during recycle. " + exc.Message);
                        }
                    }
                }
            }
        }*/

        public static void SpoofRandomTag()
        {           

            string serialNumber = rand.Next(8000000, 11000000).ToString();
            TagItem item = new TagItem { PeakRSSI = 0.0, PhaseAngle = 0.0, Epc = serialNumber, AntennaePort = 1, Firstseen = DateTime.Now.ToUniversalTime(), Lastseen = DateTime.Now.ToUniversalTime() };
            
            bool containsKey = false;

            lock (instance.tagsRead)
            {
                containsKey = instance.tagsRead.ContainsKey(item.SerialNumber);
                if (!containsKey) instance.tagsRead.Add(item.SerialNumber, item);
                else
                {
                    var temp = instance.tagsRead[item.SerialNumber];
                    temp.Lastseen = item.Lastseen;
                }
            }

            if (!containsKey)
            {
                List<TagItem> list = new List<TagItem>();
                list.Add(item);
                Task.Run(() =>
                {
                    Messenger.Default.Send<List<TagItem>>(list);
                });
            }
        }
               
        private void Reader_TagsReported(ImpinjReader reader, TagReport report)
        {
            if (DateTime.UtcNow < instance.sendEventsAfter) return; //discard events if before time delay has elapsed

            
            foreach (var t in report.Tags.OrderBy(t => t.FirstSeenTime))
            {                    
                TagItem item = new TagItem { PeakRSSI = t.PeakRssiInDbm, PhaseAngle = t.PhaseAngleInRadians, Epc = t.Epc.ToHexString(), AntennaePort=t.AntennaPortNumber, Firstseen = t.FirstSeenTime.LocalDateTime.ToUniversalTime(), Lastseen = t.LastSeenTime.LocalDateTime.ToUniversalTime() };
                bool containsKey = false;

                lock (tagsRead)
                {
                    containsKey = tagsRead.ContainsKey(item.SerialNumber);
                    if (!containsKey) tagsRead.Add(item.SerialNumber, item);
                    else
                    {
                        var temp = tagsRead[item.SerialNumber];
                        temp.Lastseen = item.Lastseen;
                    }
                }

                if (!containsKey)
                {
                    Logging.Logger.Log("RFID", "New Tags reported.");
                    List<TagItem> list = new List<TagItem>();
                    list.Add(item);
                    Task.Run(() =>
                    {
                        Messenger.Default.Send<List<TagItem>>(list);
                    });
                }
                
            }
        }

        private static void ConnectNoLoc()
        {
            if (!instance.reader.IsConnected)
            {
                instance.reader.Connect(instance.Hostname);
                instance.connectedAt = DateTime.Now;

                instance.reader.SetGpo(1, false);
                instance.reader.SetGpo(2, false);
                instance.reader.SetGpo(3, false);
                instance.reader.KeepaliveReceived -= Reader_KeepaliveReceived1;
                instance.reader.KeepaliveReceived += Reader_KeepaliveReceived1;               
            }
        }

        public static void SetGPOState(ushort port, bool state)
        {
            try
            {
                lock (instance)
                {
                    if (instance.reader.IsConnected)
                    {
                        if (instance.gpoStates[port] != state)
                        {
                            instance.gpoStates[port] = state;
                            instance.reader.SetGpo(port, state);
                        }
                    }
                }
            }
            catch(Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }

        public static bool GetGPOState(ushort port)
        {
            lock (instance)
            {
                return instance.gpoStates[port];
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
                    instance.reader.RShell.OpenSecureSession(instance.Hostname, "root", "impinj", 5000);

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

        public static int TagsInBuffer()
        {
            lock (instance.tagsRead)
            {
                return instance.tagsRead.Count();
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
                        Logging.Logger.Log("INFO", "Loading reader settings from file.");
                        settings = Settings.Load(instance.settingsFileName);
                        Logging.Logger.Log("INFO", "Apply reader settings from file.");
                        instance.reader.ApplySettings(settings);
                    }
                    else
                    {
                        Logging.Logger.Log("INFO", "Query reader settings from reader.");
                        settings = instance.reader.QuerySettings();
                    }
                }
                catch (OctaneSdkException exc)
                {
                    Logging.Logger.Log("ERROR", "Octane SDK error retrieving settings: " + exc.Message);
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
                        settings.Report.IncludePeakRssi = true;
                        settings.Report.IncludePhaseAngle = true;                        
                        //settings.Report.IncludeCrc = true;  
                        //settings.Report.IncludePcBits = true;
                        settings.Report.IncludeAntennaPortNumber = true;
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
                        Logging.Logger.Log("INFO", "Error trying to re-initialize settings.");
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
                    if (ex.Message.Contains("ROSPEC"))
                    {
                        //try to save configuration
                        GetSettings();
                        Logging.Logger.Log("RFID", "Settings resaved");
                    }
                    Messenger.Default.Send<ReaderExceptionMessage>(new ReaderExceptionMessage { Message = ex.Message });
                    //Logging.Logger.Log(ex);
                }
            }
        }

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

        public static bool IsSingulating()
        {
            lock (instance)
            {

                try
                {
                    return instance.reader.QueryStatus().IsSingulating;
                }
                catch (Exception ex)
                {
                    Logging.Logger.Log(ex);
                    return false;                    
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
                    instance.connectedAt = null;
                }
                catch (OctaneSdkException exc)
                {
                    Logging.Logger.Log("ERROR", "An error occurred disconnecting from reader. " + exc.Message);
                }
            }
        }               
        
    }   
}
