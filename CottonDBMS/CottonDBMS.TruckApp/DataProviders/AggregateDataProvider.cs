//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.DataModels;
using GalaSoft.MvvmLight.Messaging;
using CottonDBMS.TruckApp.Enums;
using CottonDBMS.TruckApp.Messages;
using CottonDBMS.Interfaces;
using GalaSoft.MvvmLight.Ioc;
using System.Timers;
using CottonDBMS.DataModels.Helpers;

namespace CottonDBMS.TruckApp.DataProviders
{
    public class AggregateDataProvider
    {
        private static AggregateDataProvider instance = new AggregateDataProvider();


        private List<AggregateEvent> _aggregateEvents;

        private DateTime readStartTime;

        private List<TagItem> tagBuffer = new List<TagItem>();

        private bool isLoading = false;
        private bool isUnloading = false;
        private object locker = new object();
        private bool initialized = false;
        private int delay = 1;

        static AggregateDataProvider()
        {

        }

        public static void Initialize()
        {
            instance._aggregateEvents = new List<AggregateEvent>();

            //load aggregate events
            using (var uow = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
            {
                //only load events since last completed load
                instance._aggregateEvents.AddRange(uow.AggregateEventRepository.GetEventsSinceLastLoad());

                var readDelay = uow.SettingsRepository.FindSingle(x => x.Key == TruckClientSettingKeys.RFID_READ_DELAY);
                instance.delay = 1;
                if (readDelay != null)
                {
                    instance.delay = int.Parse(readDelay.Value);
                }
            }

            instance.isLoading = false;
            instance.isUnloading = false;

            Messenger.Default.Register<QuadratureStateChangeMessage>(instance, (action) => instance.ProcessQuadratureStateChange2(action));
            Messenger.Default.Register<List<TagItem>>(instance, (action) => instance.ProcessTags2(action));

            instance.initialized = true;
        }

        public static void UpdateReadDelay(int delay)
        {
            instance.delay = delay;
        }

        public static void ClearBuffer()
        {
            lock (instance._aggregateEvents)
            {
                instance._aggregateEvents.Clear();
            }
        }

        public static void Reset()
        {
            lock (instance._aggregateEvents)
            {
                instance._aggregateEvents.Clear();
                instance.isLoading = false;
                instance.isUnloading = false;
            }
        }

        public static List<string> SerialNumbersOnTruckNotThreadSafe
        {
            get
            {
                var allSNs = instance._aggregateEvents.OrderBy(t => t.Timestamp).Select(t => t.SerialNumber).ToList();
                List<string> loadedSNs = new List<string>();
                foreach (var sn in allSNs)
                {
                    var lastAggEvent = instance._aggregateEvents.Where(t => t.SerialNumber == sn).OrderBy(t => t.Timestamp).LastOrDefault();
                    if (lastAggEvent != null && lastAggEvent.EventType == EventType.LOADED)
                    {
                        if (!loadedSNs.Contains(sn))
                            loadedSNs.Add(sn);
                    }
                }
                return loadedSNs;
            }
        }

        public static List<string> SerialNumbersOnTruck
        {
            get
            {
                //loading          
                lock (instance._aggregateEvents)
                {
                    return SerialNumbersOnTruckNotThreadSafe;
                }
            }
        }

        public static List<string> SerialNumbersOnTruckIncludingBuffered
        {
            get
            {
                List<string> sns = new List<string>();
                sns.AddRange(SerialNumbersOnTruck);
                sns.AddRange(instance.tagBuffer.Select(t => t.SerialNumber).Distinct());
                return sns;
            }
        }

        public static List<string> SerialNumbersInBuffer
        {
            get
            {
                List<string> sns = new List<string>();
                sns.AddRange(instance.tagBuffer.Select(t => t.SerialNumber).Distinct());
                return sns;
            }
        }

        private void ProcessQuadratureStateChange2(QuadratureStateChangeMessage eventData)
        {
            try
            {
                if (!initialized) return;

                Logging.Logger.Log("AGG", "Processing quadrature state change.");
                lock (instance.locker)
                {
                    using (var uow = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
                    {
                        //if quadrature is turning then start the reader
                        if (eventData.DirectionOfRotation == DirectionOfRotation.RotatingClockwise || eventData.DirectionOfRotation == DirectionOfRotation.RotatingCounterClockwise)
                        {
                            if (eventData.DirectionOfRotation == DirectionOfRotation.RotatingClockwise)
                            {
                                GPSDataProvider.SetReverse(true);
                            }

                            readStartTime = DateTime.Now.AddSeconds(delay).ToUniversalTime();

                            Logging.Logger.Log("AGG", "Ignore tags before " + readStartTime.ToString());

                            isLoading = false;
                            isUnloading = false;

                            tagBuffer.Clear();
                            Logging.Logger.Log("AGG", "Starting reader for read cycle");                            
                            TagDataProvider.Start(delay);

                            if (eventData.DirectionOfRotation == DirectionOfRotation.RotatingClockwise)
                            {
                                isLoading = true;
                                GPSDataProvider.SetReverse(true);
                                Logging.Logger.Log("AGG", "Loading started");
                            }
                            else
                            {
                                isUnloading = true;
                                GPSDataProvider.SetReverse(false);
                                Logging.Logger.Log("AGG", "Unloading started");
                            }                            
                        }
                        else
                        {
                            if (DateTime.UtcNow < readStartTime)
                            {
                                Logging.Logger.Log("AGG", "Cancelling read cycle");                                
                                TagDataProvider.Stop();                                                                
                                return;
                            }

                            Logging.Logger.Log("AGG", "Processing stop");
                            GPSDataProvider.SetReverse(false);
                            var isLoadEvent = isLoading;
                            DateTime stopTime = DateTime.Now.ToUniversalTime();

                            isLoading = false;  //stops adding tags to the buffer of distinct items scanned
                            isUnloading = false;

                            Logging.Logger.Log("AGG", "Checking for modules still in view");

                            if (!isLoadEvent) System.Threading.Thread.Sleep(100); //changed from 200 to 100

                            DateTime correctionStart = DateTime.UtcNow;
                            if (!isLoadEvent) System.Threading.Thread.Sleep(200); //changed from 2000 to 500
                                                        
                            TagDataProvider.Stop();
                            DateTime correctionEnd = DateTime.UtcNow;

                            List<string> tagsStillOnTruck = new List<string>();
                            var tagsRead = TagDataProvider.GetTagsFirstSeenInTimeRange(readStartTime, stopTime);

                            if (!isLoadEvent) {
                                tagsStillOnTruck = TagDataProvider.GetTagsLastSeenInTimeRange(correctionStart, correctionEnd).Select(t => t.SerialNumber).Distinct().ToList();
                                StringBuilder sb = new StringBuilder();
                                foreach (var s in tagsStillOnTruck) {
                                    sb.Append(s + ", ");
                                }
                                Logging.Logger.Log("AGG", "Tags still on truck (detected in view after chains stopped): " + sb.ToString().TrimEnd(','));
                            }

                            

                            var truck = uow.SettingsRepository.GetCurrentTruck();
                            var driver = uow.SettingsRepository.GetCurrentDriver();
                            string[] snsOnTruck = SerialNumbersOnTruck.ToArray();

                            TagItem[] tagsInBuffer = null;

                            lock (tagBuffer)
                            {
                                tagsInBuffer = tagBuffer.ToArray();
                            }

                            Logging.Logger.Log("AGG", "Processing unique tags seen.");
                            int agEventCount = 0;
                            foreach (var tag in tagsInBuffer)
                            {
                                var tagRead = tagsRead.Where(r => r.SerialNumber == tag.SerialNumber).FirstOrDefault();
                                //var lastTagLastTagRead = tagsRead.Where(r => r.SerialNumber == tag.SerialNumber).LastOrDefault();

                                DateTime startTime = readStartTime;
                                DateTime endTime = stopTime;

                                var firstTag = tagsRead.Where(r => r.SerialNumber == tag.SerialNumber).FirstOrDefault();
                                var lastTag = tagsRead.Where(r => r.SerialNumber == tag.SerialNumber).LastOrDefault();

                                if (tagRead != null)
                                    startTime = tagRead.Firstseen;
                                if (tagRead != null)
                                    endTime = tagRead.Lastseen;

                                var firstCoords = GPSDataProvider.GetFirstCoords(startTime, endTime);
                                var lastCoords = GPSDataProvider.GetLastCoords(startTime, endTime);

                                AggregateEvent agEvent = new AggregateEvent();

                                if (isLoadEvent)
                                    agEvent.Timestamp = startTime;
                                else
                                    agEvent.Timestamp = endTime;

                                agEvent.SerialNumber = tag.SerialNumber;
                                agEvent.Epc = tag.Epc;
                                agEvent.AverageLat = GPSDataProvider.GetAverageLatitude(startTime, endTime);
                                agEvent.AverageLong = GPSDataProvider.GetAverageLongitude(startTime, endTime);
                                agEvent.MedianLat = GPSDataProvider.GetMedianLatitude(startTime, endTime);
                                agEvent.MedianLong = GPSDataProvider.GetMedianLongitude(startTime, endTime);
                                agEvent.FirstLat = GPSHelper.SafeLat(firstCoords);
                                agEvent.FirstLong = GPSHelper.SafeLong(firstCoords);
                                agEvent.LastLat = GPSHelper.SafeLat(lastCoords);
                                agEvent.LastLong = GPSHelper.SafeLong(lastCoords);
                                agEvent.TruckID = (truck != null) ? truck.Id : "";
                                agEvent.DriverID = (driver != null) ? driver.Id : "";
                                agEvent.EventType = (isLoadEvent) ? EventType.LOADED : EventType.UNLOADED;

                                lock (_aggregateEvents)
                                {
                                    //if we are on the gin yard leave load number empty
                                    if (!isLoadEvent) agEvent.LoadNumber = "";
                                    else
                                    {
                                        if (uow.SettingsRepository.EventOnGinYard(agEvent))
                                        {
                                            agEvent.LoadNumber = "";
                                        }
                                        else if (uow.SettingsRepository.EventAtFeeder(agEvent))
                                        {
                                            agEvent.LoadNumber = "";
                                        }
                                        else if (SerialNumbersOnTruck == null || SerialNumbersOnTruck.Count() == 0)
                                        {
                                            //NO MODULES ON TRUCK SO GENERATE NEW NUMBER
                                            agEvent.LoadNumber = uow.TruckRepository.GetNextLoadNumber();
                                        }
                                        else
                                        {
                                            //THERE ARE OTHER MODULES ON TRUCK SO GET LAST NUMBER USED                                           
                                            agEvent.LoadNumber = uow.TruckRepository.GetLastLoadNumber();
                                        }
                                    }

                                    //only log event if it is a load of a module not previously scanned in a load cycle
                                    //or if an unload for which the serial wasn't picked up still on the truck - added check to only record unload event for modules that were on the truck
                                    if ((isLoadEvent && !snsOnTruck.Contains(agEvent.SerialNumber)) || (!isLoadEvent && snsOnTruck.Contains(agEvent.SerialNumber)/*&& !tagsStillOnTruck.Contains(agEvent.SerialNumber)*/))
                                    {
                                        agEventCount++;
                                        _aggregateEvents.Add(agEvent);
                                        agEvent.Created = DateTime.UtcNow;
                                        uow.AggregateEventRepository.Add(agEvent);
                                        uow.SaveChanges();

                                        //if unloading in the field clear last load number on events with this serial number
                                        //that were generated when loading stopped - this prevents skipping
                                        //load numbers when all modules are unloaded in the field
                                        CorrectLoadNumber(uow, isLoadEvent, agEvent);

                                        UpdateModuleStatus(uow, isLoadEvent, agEvent);

                                        Messenger.Default.Send<AggregateEvent>(agEvent);                                        
                                    }
                                }
                            }
                            if (agEventCount > 0)
                            {
                                Messenger.Default.Send<AllAggEventsProcessComplete>(new AllAggEventsProcessComplete { IsLoading = isLoadEvent });
                            }
                            tagBuffer.Clear();
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }

        private void UpdateModuleStatus(IUnitOfWork uow, bool isLoadEvent, AggregateEvent agEvent)
        {
            var thisModule = uow.ModuleRepository.FindSingle(x => x.Name == agEvent.SerialNumber);

            if (thisModule != null)
            {
                bool atFeeder = uow.SettingsRepository.EventAtFeeder(agEvent);
                bool atGinYard = uow.SettingsRepository.EventOnGinYard(agEvent);
                if (!isLoadEvent && atFeeder)
                {
                    thisModule.ModuleStatus = ModuleStatus.GINNED;
                }
                else if (!isLoadEvent && atGinYard)
                {
                    thisModule.ModuleStatus = ModuleStatus.AT_GIN;
                }
                else if (isLoadEvent && !atFeeder && !atGinYard)
                {
                    thisModule.ModuleStatus = ModuleStatus.PICKED_UP;
                }
                else if (!isLoadEvent && !atFeeder && !atGinYard)
                {
                    thisModule.ModuleStatus = ModuleStatus.IN_FIELD;
                }
                uow.ModuleRepository.Save(thisModule);
                uow.SaveChanges();
            }
        }

        private void CorrectLoadNumber(IUnitOfWork uow, bool isLoadEvent, AggregateEvent agEvent)
        {
            if (!isLoadEvent && !uow.SettingsRepository.CoordsOnGinYard(agEvent.AverageLat, agEvent.AverageLong) && !uow.SettingsRepository.CoordsAtFeeder(agEvent.AverageLat, agEvent.AverageLong))
            {
                //string lastLoadNumber = uow.TruckRepository.GetLastLoadNumber();
                uow.TruckRepository.ClearLoadNumber(agEvent.SerialNumber);
                foreach (var evt in _aggregateEvents.Where(a => a.SerialNumber == agEvent.SerialNumber))
                {
                    evt.LoadNumber = "";
                }
                uow.SaveChanges();
            }
        }

        private void ProcessTags2(List<TagItem> tags)
        {
            if (DateTime.UtcNow < readStartTime)
            {
                Logging.Logger.Log("AGG", "Cancelling read cycle");                                
                return;
            }

            Logging.Logger.Log("AGG", "ProcessingTags2");
            if (!initialized) return;

            if (isLoading)
            {
                Logging.Logger.Log("AGG", "Processing tags seen while loading.");
                foreach (var t in tags)
                {
                    lock (tagBuffer)
                    {
                        if (!tagBuffer.Any(i => i.SerialNumber == t.SerialNumber) && !SerialNumbersOnTruck.Any(i => i == t.SerialNumber))
                        {
                            Messenger.Default.Send<TagLoadingMessage>(new TagLoadingMessage { SerialNumber = t.SerialNumber });
                            tagBuffer.Add(t);
                        }
                    }
                }
            }
            else if (isUnloading)
            {
                Logging.Logger.Log("AGG", "Processing tags seen while unloading.");
                foreach (var t in tags)
                {
                    lock (tagBuffer)
                    {
                        if (!tagBuffer.Any(i => i.SerialNumber == t.SerialNumber))
                        {
                            Messenger.Default.Send<TagUnloadingMessage>(new TagUnloadingMessage { SerialNumber = t.SerialNumber });
                            tagBuffer.Add(t);
                        }
                    }
                }
            }
            
            Logging.Logger.Log("AGG", "End ProcessingTags2");
        }

        /// <summary>
        /// Inserts event for a manual unload override
        /// </summary>
        /// <param name="SerialNumber"></param>
        public static void ForceUnload(string SerialNumber)
        {
            using (var uow = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
            {
                var truck = uow.SettingsRepository.GetCurrentTruck();
                var driver = uow.SettingsRepository.GetCurrentDriver();

                var aggEvt = instance._aggregateEvents.FirstOrDefault(evt => evt.SerialNumber == SerialNumber && !string.IsNullOrEmpty(evt.Epc));
                string epc = (aggEvt != null) ? aggEvt.Epc : "";

                AggregateEvent agEvent = new AggregateEvent();
                agEvent.Timestamp = DateTime.Now.ToUniversalTime();
               
                var gpsCoords = GPSDataProvider.GetLastCoords();

                double lat = GPSHelper.SafeLat(gpsCoords); 
                double lng = GPSHelper.SafeLong(gpsCoords);

                agEvent.SerialNumber = SerialNumber;
                agEvent.Epc = epc;
                agEvent.AverageLat = lat;
                agEvent.AverageLong = lng;
                agEvent.MedianLat = lat;
                agEvent.MedianLong = lng;
                agEvent.FirstLat = lat;
                agEvent.FirstLong = lng;
                agEvent.LastLat = lat;
                agEvent.LastLong = lng;
                agEvent.TruckID = (truck != null) ? truck.Id : "";
                agEvent.DriverID = (driver != null) ? driver.Id : "";
                agEvent.EventType = EventType.UNLOADED;
                agEvent.Created = DateTime.Now.ToUniversalTime();

                instance._aggregateEvents.Add(agEvent);
                uow.AggregateEventRepository.Add(agEvent);
                uow.SaveChanges();

                instance.CorrectLoadNumber(uow, false, agEvent);
                instance.UpdateModuleStatus(uow, false, agEvent);

                //Messenger.Default.Send<AggregateEvent>(agEvent);
            }
        }

        /// <summary>
        /// Inserts event for a manual unload override
        /// </summary>
        /// <param name="SerialNumber"></param>
        public static void ForceLoad(string SerialNumber)
        {
            if (!SerialNumbersOnTruckNotThreadSafe.Any(x => x == SerialNumber))
            {
                using (var uow = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
                {
                    var truck = uow.SettingsRepository.GetCurrentTruck();
                    var driver = uow.SettingsRepository.GetCurrentDriver();

                    var aggEvt = instance._aggregateEvents.FirstOrDefault(evt => evt.SerialNumber == SerialNumber && !string.IsNullOrEmpty(evt.Epc));
                    string epc = (aggEvt != null) ? aggEvt.Epc : "";
                    AggregateEvent agEvent = new AggregateEvent();
                    agEvent.Timestamp = DateTime.Now.ToUniversalTime();

                    var gpsCoords = GPSDataProvider.GetLastCoords();

                    double lat = GPSHelper.SafeLat(gpsCoords);
                    double lng = GPSHelper.SafeLong(gpsCoords);

                    agEvent.SerialNumber = SerialNumber;
                    agEvent.Epc = epc;
                    agEvent.AverageLat = lat;
                    agEvent.AverageLong = lng;
                    agEvent.MedianLat = lat;
                    agEvent.MedianLong = lng;
                    agEvent.FirstLat = lat;
                    agEvent.FirstLong = lng;
                    agEvent.LastLat = lat;
                    agEvent.LastLong = lng;
                    agEvent.TruckID = (truck != null) ? truck.Id : "";
                    agEvent.DriverID = (driver != null) ? driver.Id : "";
                    agEvent.EventType = EventType.LOADED;
                    agEvent.Created = DateTime.Now.ToUniversalTime();

                    if (uow.SettingsRepository.EventOnGinYard(agEvent))
                    {
                        agEvent.LoadNumber = "";
                    }
                    else if (uow.SettingsRepository.EventAtFeeder(agEvent))
                    {
                        agEvent.LoadNumber = "";
                    }
                    else if (SerialNumbersOnTruck == null || SerialNumbersOnTruck.Count() == 0)
                    {
                        //NO MODULES ON TRUCK SO GENERATE NEW NUMBER
                        agEvent.LoadNumber = uow.TruckRepository.GetNextLoadNumber();
                    }
                    else
                    {
                        //THERE ARE OTHER MODULES ON TRUCK SO GET LAST NUMBER USED                                           
                        agEvent.LoadNumber = uow.TruckRepository.GetLastLoadNumber();
                    }

                    instance._aggregateEvents.Add(agEvent);
                    uow.AggregateEventRepository.Add(agEvent);
                    uow.SaveChanges();

                    instance.CorrectLoadNumber(uow, true, agEvent);
                    instance.UpdateModuleStatus(uow, true, agEvent);
                    //Messenger.Default.Send<AggregateEvent>(agEvent);
                }
            }
        }
        
        public static void Cleanup()
        {
            Messenger.Default.Unregister<QuadratureStateChangeMessage>(instance);
            Messenger.Default.Unregister<List<TagItem>>(instance);
        }
    }

    public class QuadratureEvent
    {
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }
    }

    public class GPSEvent
    {
        public DateTime Timestamp { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string MessageType { get; set; }
    }

    public class RFIDEvent
    {
        public DateTime Timestamp { get; set; }
        public string SerialNumber { get; set; }
    }    
}

