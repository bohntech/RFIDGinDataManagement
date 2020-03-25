//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using CottonDBMS.Interfaces;
using CottonDBMS.DataModels;
using GalaSoft.MvvmLight.Messaging;
using CottonDBMS.TruckApp.Messages;
using CottonDBMS.Logging;

namespace CottonDBMS.TruckApp.DataProviders
{
    public class GPSDataProvider
    {
        private static GPSDataProvider instance = new GPSDataProvider();
        private static bool intialized = false;

        private System.IO.Ports.SerialPort port = null;
        private NmeaParser.SerialPortDevice device = null;
        private int maxMessages = 3000;
        private Queue<GpsCoords> coordinatesQueue = new Queue<GpsCoords>();

        public static double? OverrideLat { get; set; }
        public static double? OverrideLong { get; set; }

        private static double lastBearing { get; set; }

        private static double gpsOffsetFeet = 0.00;

        private static bool inReverse = false;
  

        public static bool IsConnected
        {
            get
            {
                return intialized;
            }
        }

        public static void SetGpsOffsetFeet(double feet)
        {
            gpsOffsetFeet = feet;
        }

        public static void SetReverse(bool _inReverse)
        {
            inReverse = _inReverse;
        }

        private static System.IO.Ports.SerialPort GetPortIfValid(string portName)
        {
            using (var port = new System.IO.Ports.SerialPort(portName))
            {
                List<int> baudRatesToTest = new List<int>(new[] { 9600, 4800, 115200, 19200, 57600, 38400, 2400 });
                foreach (var baud in baudRatesToTest)
                {
                    port.BaudRate = baud;
                    port.ReadTimeout = 2000; //this might not be long enough
                    bool success = false;
                    try
                    {
                        port.Open();
                        if (!port.IsOpen)
                            continue; //couldn't open port
                        try
                        {
                            port.ReadTo("$GP");
                            success = true;
                        }
                        catch (TimeoutException)
                        {
                            continue;
                        }
                    }
                    catch
                    {
                        //Error reading
                    }
                    finally
                    {
                        port.Close();
                    }

                    if (success)
                    {
                        return new System.IO.Ports.SerialPort(portName, baud);
                    }
                }

                return null;
            }
        }

        private static System.IO.Ports.SerialPort DetectGPSPort()
        {
            var ports = System.IO.Ports.SerialPort.GetPortNames().OrderBy(s => s);
            foreach (var portName in ports)
            {
                var port = GetPortIfValid(portName);

                if (port != null)
                {
                    return port;
                }
            }
            return null;
        }

        private static System.IO.Ports.SerialPort GetSavedPort()
        {

            string portName = GetSavedPortName();

                if (!string.IsNullOrWhiteSpace(portName))
                {
                    return GetPortIfValid(portName);
                }
                else
                {
                    return null;
                }            
        }

        private static string GetSavedPortName()
        {
            using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
            {
                var setting = dp.SettingsRepository.FindSingle(x => x.Key == TruckClientSettingKeys.GPS_COM_PORT);

                if (setting != null && !string.IsNullOrWhiteSpace(setting.Value))
                {
                    return setting.Value;
                }
                else
                {
                    return null;
                }
            }
        }

        private static void SaveDefaultPort(string portname)
        {
            using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
            {
                var setting = dp.SettingsRepository.FindSingle(x => x.Key == TruckClientSettingKeys.GPS_COM_PORT);

                if (setting != null && !string.IsNullOrWhiteSpace(setting.Value))
                {
                    setting.Value = portname;
                    dp.SaveChanges();
                }
            }
        }

        public string AutoDiscoverPort()
        {
            var ports = System.IO.Ports.SerialPort.GetPortNames().OrderBy(s => s);
            string portStr = "";
            bool success = false;

            foreach (var portName in ports)
            {
                using (var port = new System.IO.Ports.SerialPort(portName))
                {
                    List<int> baudRatesToTest = new List<int>(new[] { 9600, 4800, 2400 });
                    foreach (var baud in baudRatesToTest)
                    {
                        port.BaudRate = baud;
                        port.ReadTimeout = 2000;
                        
                        try
                        {
                            port.Open();
                            if (!port.IsOpen)
                            {
                                continue; //couldn't open port 
                            }
                            try
                            {
                                port.ReadTo("$GP");
                                portStr = portName;
                                success = true;
                                break;
                            }
                            catch (TimeoutException)
                            {
                                continue;
                            }
                        }

                        catch (Exception exc)
                        {
                            continue;
                        }
                    }
                }

                if (success) break;
            }

            return portStr;
        }

        public static void Connect()
        {
            try
            {
                intialized = false;
                Logger.Log("INFO", "Fetch saved port.");
                string savedPortname = GetSavedPortName();
                              
                ////
                instance.port = GetPortIfValid(savedPortname);

                if (instance.port == null)
                {
                    instance.port = DetectGPSPort();
                }

                if (instance.port != null)
                {
                    if (instance.device != null)
                    {
                        instance.device.MessageReceived -= Device_MessageReceived;
                        instance.device.Dispose();
                    }

                    SaveDefaultPort(instance.port.PortName); //save detected port so it is tried first next time.

                    lastBearing = 0.00;
                    Logger.Log("INFO", "Connecting to port " + instance.port.PortName);
                    instance.device = new NmeaParser.SerialPortDevice(instance.port);
                    instance.device.MessageReceived += Device_MessageReceived;
                    instance.device.OpenAsync();
                }
                else
                {
                    intialized = false;
                }
                ////
                           
            }
            catch (Exception exc)
            {
                intialized = false;
                Logger.Log(exc);
            }
        }

        public static void ClearBuffer()
        {
            lock (instance.coordinatesQueue)
            {
                instance.coordinatesQueue.Clear();
            }
        }
        
        private static void Device_MessageReceived(object sender, NmeaParser.NmeaMessageReceivedEventArgs e)
        {
            DateTime nowUTC = DateTime.Now.ToUniversalTime();
            double lat = 0.00;
            double longitude = 0.00;
            bool hasValidMessage = false;
            string msgType = "";

            DateTime fixtime = DateTime.Now.ToUniversalTime();
            
            if (e.Message is NmeaParser.Nmea.Gps.Gprmc)
            {
                var message = e.Message as NmeaParser.Nmea.Gps.Gprmc;
                lat = message.Latitude;
                fixtime = message.FixTime.ToUniversalTime();
                longitude = message.Longitude;
                hasValidMessage = true;
                msgType = "GPRMC";

                if (!double.IsNaN(message.Course))
                {
                    lastBearing = message.Course;
                }              
               
            }
            else if (e.Message is NmeaParser.Nmea.Gps.Gpgga)
            {
                var message = e.Message as NmeaParser.Nmea.Gps.Gpgga;
                fixtime = new DateTime(nowUTC.Year, nowUTC.Month, nowUTC.Day, message.FixTime.Hours, message.FixTime.Minutes, message.FixTime.Seconds, message.FixTime.Milliseconds);
                lat = message.Latitude;
                longitude = message.Longitude;
                hasValidMessage = true;
                msgType = "GPGGA";
            }
            else if (e.Message is NmeaParser.Nmea.Gps.Gpgll)
            {
                var message = e.Message as NmeaParser.Nmea.Gps.Gpgll;
                
                lat = message.Latitude;
                longitude = message.Longitude;
                hasValidMessage = true;
                fixtime = new DateTime(nowUTC.Year, nowUTC.Month, nowUTC.Day, message.FixTime.Hours, message.FixTime.Minutes, message.FixTime.Seconds, message.FixTime.Milliseconds);
                msgType = "GPGLL";
            }
            else if (e.Message is NmeaParser.Nmea.Gps.Gpbod)
            {
                var message = e.Message as NmeaParser.Nmea.Gps.Gpbod;
                lastBearing = message.TrueBearing;
            }

            if (hasValidMessage)
            {
                intialized = true;
                lock (instance.coordinatesQueue)
                {
                    if (!OverrideLat.HasValue || !OverrideLong.HasValue)
                    {
                        var coords = new GpsCoords { Altitude = null, Latitude = lat, Longitude = longitude, Timestamp = nowUTC };
                        coords.TranslateByHeading(lastBearing, gpsOffsetFeet, inReverse);
                        instance.coordinatesQueue.Enqueue(coords);
                    }
                    else
                    {
                        instance.coordinatesQueue.Enqueue(new GpsCoords { Altitude = null, Latitude = OverrideLat, Longitude = OverrideLong, Timestamp = nowUTC });
                    }
                }

                try
                {
                    //Logger.Log("GPS", "Sending GPS Message. " + lat.ToString() + ", " + longitude.ToString());
                    if (!OverrideLat.HasValue || !OverrideLong.HasValue)
                        Messenger.Default.Send<GPSEventMessage>(new GPSEventMessage { Timestamp = fixtime, Latitude = lat, Longitude = longitude, MessageType = msgType });
                    else
                        Messenger.Default.Send<GPSEventMessage>(new GPSEventMessage { Timestamp = fixtime, Latitude = OverrideLat.Value, Longitude = OverrideLong.Value, MessageType = msgType });
                }
                catch (Exception gExc)
                {
                    Logging.Logger.Log(gExc);
                }
                
            }

            lock (instance.coordinatesQueue)
            {
                while (instance.coordinatesQueue.Count > instance.maxMessages)
                {
                    instance.coordinatesQueue.Dequeue();
                }
            }
        }

        private static GpsCoords GetLastValidCoord()
        {
            lock (instance.coordinatesQueue)
            {
                var orderedList = instance.coordinatesQueue.ToList().Where(x => x.IsValid).OrderBy(x => x.Timestamp);
                return orderedList.LastOrDefault();
            }
        }

        private static double GetLastValidLat()
        {
            var lastReading = GetLastValidCoord();
            if (lastReading != null && lastReading.Latitude.HasValue) return lastReading.Latitude.Value;
            else return 0.00;
        }

        private static double GetLastNonZeroLong()
        {
            var lastReading = GetLastValidCoord();
            if (lastReading != null && lastReading.Longitude.HasValue) return lastReading.Longitude.Value;
            else return 0.00;
        }

        public static List<GpsCoords> GetValidCoordinatesInRange(DateTime startTime, DateTime endTime)
        {
            lock (instance.coordinatesQueue)
            {
                return instance.coordinatesQueue.ToList().Where(x => x.Timestamp >= startTime && x.Timestamp <= endTime && x.IsValid).OrderBy(x => x.Timestamp).ToList();
            }
        }

        public static double GetMedianLatitude(DateTime startTime, DateTime endTime)
        {
            var orderedList = GetValidCoordinatesInRange(startTime, endTime).OrderBy(x => x.Latitude);
            var count = orderedList.Count();

            if (count == 1)
            {
                var coords = orderedList.Single();
                return CottonDBMS.DataModels.Helpers.GPSHelper.SafeLat(coords);
            }
            else if (count > 1)
            {
                var e1 = orderedList.ElementAt(count / 2);
                var e2 = orderedList.ElementAt((count - 1) / 2);
                double median = (e1.Latitude.Value + e2.Latitude.Value) / 2.0;
                return median;
            }
            else
            {
                return GetLastValidLat();
            }
        }

        public static double GetMedianLongitude(DateTime startTime, DateTime endTime)
        {
            var orderedList = GetValidCoordinatesInRange(startTime, endTime).OrderBy(x => x.Longitude);
            var count = orderedList.Count();

            if (count == 1)
            {
                var coords = orderedList.Single();
                return CottonDBMS.DataModels.Helpers.GPSHelper.SafeLong(coords);
            }
            else if (count > 1)
            {
                var e1 = orderedList.ElementAt(count / 2);
                var e2 = orderedList.ElementAt((count - 1) / 2);
                double median = (e1.Longitude.Value + e2.Longitude.Value) / 2.0;
                return median;
            }
            else
            {
                return GetLastNonZeroLong();
            }
        }

        public static double GetAverageLatitude(DateTime startTime, DateTime endTime)
        {
            var coordinates = GetValidCoordinatesInRange(startTime, endTime);
            if (coordinates.Count() > 0)
                return coordinates.Average(x => x.Latitude.Value);
            else
            {
                return GetLastValidLat();
            }
        }

        public static double GetAverageLongitude(DateTime startTime, DateTime endTime)
        {
            var coordinates = GetValidCoordinatesInRange(startTime, endTime);
            if (coordinates.Count() > 0)
                return coordinates.Average(x => x.Longitude.Value);
            else
            {
                return GetLastNonZeroLong();
            }
        }

        public static GpsCoords GetFirstCoords(DateTime startTime, DateTime endTime)
        {
            var coordinates = GetValidCoordinatesInRange(startTime, endTime);
            return coordinates.OrderBy(x => x.Timestamp).FirstOrDefault();            
        }

        public static GpsCoords GetLastCoords(DateTime startTime, DateTime endTime)
        {
            var coordinates = GetValidCoordinatesInRange(startTime, endTime);
            return coordinates.OrderBy(x => x.Timestamp).LastOrDefault();
        }

        public static GpsCoords GetLastCoords()
        {
            var coordinates = GetValidCoordinatesInRange(DateTime.Now.AddDays(-30), DateTime.Now.AddDays(30));            
            return coordinates.OrderBy(x => x.Timestamp).LastOrDefault();            
        }

        public static async void Disconnect()
        {
            if (instance.device != null)
            {
                instance.device.MessageReceived -= Device_MessageReceived;



                if (instance.device.IsOpen)
                {
                    await instance.device.CloseAsync();
                }
                if (instance.device.Port.IsOpen)
                {
                    System.Threading.Thread.Sleep(250);
                    instance.device.Port.Close();
                }

                System.Threading.Thread.Sleep(500);
                instance.device.Dispose();
                intialized = false;
            }
        }
    }

   
}
