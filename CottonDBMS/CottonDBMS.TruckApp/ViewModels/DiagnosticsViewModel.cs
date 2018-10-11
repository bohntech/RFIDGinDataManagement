//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using CottonDBMS.TruckApp.DataProviders;
using Impinj.OctaneSdk;
using CottonDBMS.Interfaces;
using CottonDBMS.DataModels;
using CottonDBMS.TruckApp.Messages;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Win32;
using CottonDBMS.TruckApp.Enums;
using GalaSoft.MvvmLight.Ioc;

namespace CottonDBMS.TruckApp.ViewModels
{
    public class DiagnosticsViewModel : ViewModelBase
    {
        private ObservableCollection<QuadratureEvent> _quadratureEvents;

        public ObservableCollection<QuadratureEvent> QuadratureEvents
        {
            get { return _quadratureEvents; }
            private set { Set(() => QuadratureEvents, ref _quadratureEvents, value); }
        }

        private ObservableCollection<GPSEvent> _gpsEvents;
        public ObservableCollection<GPSEvent> GPSEvents
        {
            get { return this._gpsEvents; }
            private set { Set(() => GPSEvents, ref _gpsEvents, value); }
        }

        private ObservableCollection<RFIDEvent> _rfidEvents;
        public ObservableCollection<RFIDEvent> RFIDEvents
        {
            get { return this._rfidEvents; }
            private set { Set(() => RFIDEvents, ref _rfidEvents, value); }
        }

        private ObservableCollection<AggregateEvent> _aggregateEvents;
        public ObservableCollection<AggregateEvent> AggregateEvents
        {
            get { return this._aggregateEvents; }
            private set { Set(() => AggregateEvents, ref _aggregateEvents, value); }
        }

        private DateTime rfidStartTime = new DateTime(2010, 1, 1);


        public RelayCommand ExportGpsCommand { get; private set; }
        private void ExecuteExportGpsCommand()
        {
            StringBuilder sb = new StringBuilder();
            //lock (gpsLocker)
            //{
            sb.AppendLine("TIMESTAMP, MESSAGE TYPE, LATITUDE, LONGITUDE");
            foreach (var item in _gpsEvents)
            {
                sb.AppendLine(string.Format("{0},{1},{2},{3}", item.Timestamp.ToLongTimeString(), item.MessageType, item.Latitude.ToString(), item.Longitude.ToString()));
            }
            //}

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                System.IO.File.WriteAllText(saveFileDialog.FileName, sb.ToString());
        }

        public RelayCommand ClearGpsCommand { get; private set; }
        private void ExecuteClearGpsCommand()
        {
            GPSEvents.Clear();
            GPSDataProvider.ClearBuffer();
        }

        public RelayCommand ExportQuadratureCommand { get; private set; }
        private void ExecuteExportQuadratureCommand()
        {
            StringBuilder sb = new StringBuilder();
            //lock (quadLocker)
            // {
            sb.AppendLine("TIMESTAMP, STATUS");
            foreach (var item in _quadratureEvents)
            {
                sb.AppendLine(string.Format("{0},{1}", item.Timestamp.ToLongTimeString(), item.Status));
            }
            //}

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                System.IO.File.WriteAllText(saveFileDialog.FileName, sb.ToString());
        }

        public RelayCommand ClearQuadratureCommand { get; private set; }
        private void ExecuteClearQuadratureCommand()
        {
            //lock (quadLocker)
            //{
            QuadratureEvents.Clear();
            //}
        }

        public RelayCommand ClearRFIDCommand { get; private set; }
        private void ExecuteClearRFIDCommand()
        {            
            RFIDEvents.Clear();
            TagDataProvider.ClearBuffer();         
        }

        public RelayCommand ExportRFIDCommand { get; private set; }
        private void ExecuteExportRFIDCommand()
        {
            StringBuilder sb = new StringBuilder();            
            sb.AppendLine("TIMESTAMP, SERIAL");
            foreach (var item in _rfidEvents)
            {
                sb.AppendLine(string.Format("{0},{1}", item.Timestamp.ToLongTimeString(), item.SerialNumber));
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                System.IO.File.WriteAllText(saveFileDialog.FileName, sb.ToString());
        }

        public RelayCommand ClearAggregateCommand { get; private set; }
        private void ExecuteClearAggregateCommand()
        {            
            AggregateEvents.Clear();                
        }

        public RelayCommand ExportAggregateCommand { get; private set; }
        private void ExecuteExportAggregateCommand()
        {
            StringBuilder sb = new StringBuilder();           
            sb.AppendLine("Timestamp, Serial, EventType, AverageLat, AverageLong, MedianLat, MedianLong, FirstLat, FirstLong, LastLat, LastLong");
            foreach (var item in _aggregateEvents)
            {
                sb.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}",
                    item.Timestamp.ToLongTimeString(),
                    item.SerialNumber,
                    item.EventType,
                    item.AverageLat,
                    item.AverageLong,
                    item.MedianLat,
                    item.MedianLong,
                    item.FirstLat,
                    item.FirstLong,
                    item.LastLat,
                    item.LastLong
                    ));
            }          
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                System.IO.File.WriteAllText(saveFileDialog.FileName, sb.ToString());
        }

        public DiagnosticsViewModel()
        {
            QuadratureEvents = new ObservableCollection<QuadratureEvent>();
            GPSEvents = new ObservableCollection<GPSEvent>();
            RFIDEvents = new ObservableCollection<RFIDEvent>();
            AggregateEvents = new ObservableCollection<AggregateEvent>();
            
            Messenger.Default.Register<GPSEventMessage>(this, (action) => ProcessGPSMessage(action));
            Messenger.Default.Register<List<TagItem>>(this, (action) => ProcessTags(action));
            Messenger.Default.Register<AggregateEvent>(this, (action) => NewAggregateEvent(action));
            Messenger.Default.Register<QuadratureStateChangeMessage>(this, (action) => ProcessQuadratureStateChange(action));

            ExportGpsCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteExportGpsCommand);
            ClearGpsCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteClearGpsCommand);

            ExportRFIDCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteExportRFIDCommand);
            ClearRFIDCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteClearRFIDCommand);

            ExportQuadratureCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteExportQuadratureCommand);
            ClearQuadratureCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteClearQuadratureCommand);

            ExportAggregateCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteExportAggregateCommand);
            ClearAggregateCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteClearAggregateCommand);
        }


        private void ProcessQuadratureStateChange(QuadratureStateChangeMessage eventData)
        {
            int delay = 1;
            using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
            {
                var readDelay = dp.SettingsRepository.FindSingle(x => x.Key == TruckClientSettingKeys.RFID_READ_DELAY);

                if (readDelay != null)
                {
                    delay = int.Parse(readDelay.Value);
                }
            }

            QuadratureStateChangeMessage lastEvent = QuadratureEncoderDataProvider.LastEvent;
            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            if (eventData.DirectionOfRotation == DirectionOfRotation.RotatingClockwise)
                            {
                                QuadratureEvents.Add(new QuadratureEvent { Status = "Load started", Timestamp = DateTime.Now.ToUniversalTime() });
                                rfidStartTime = DateTime.Now.AddSeconds(delay).ToUniversalTime();
                            }
                            else if (eventData.DirectionOfRotation == DirectionOfRotation.RotatingCounterClockwise)
                            {
                                QuadratureEvents.Add(new QuadratureEvent { Status = "Unload started", Timestamp = DateTime.Now.ToUniversalTime() });
                                rfidStartTime = DateTime.Now.AddSeconds(delay).ToUniversalTime();
                            }
                            else
                            {
                                QuadratureEvents.Add(new QuadratureEvent { Status = "Stopped", Timestamp = DateTime.Now.ToUniversalTime() });
                                rfidStartTime = DateTime.Now.AddDays(100).ToUniversalTime();
                            }
                        }));
        }

        private void ProcessGPSMessage(GPSEventMessage e)
        {
            try
            {
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    GPSEvents.Add(new GPSEvent { Latitude = e.Latitude, Longitude = e.Longitude, MessageType = e.MessageType, Timestamp = e.Timestamp });

                    while(GPSEvents.Count() > 15000)
                    {
                        GPSEvents.RemoveAt(0);
                    }

                }));
            }
            catch (System.Threading.ThreadInterruptedException intExc)
            {
                System.Diagnostics.Trace.Write(intExc.Message);
            }
        }
        
        private void ProcessTags(List<TagItem> tags)
        {
            try
            {
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    foreach (var t in tags.Where(t => t.Firstseen >= rfidStartTime))
                    {
                        RFIDEvents.Add(new RFIDEvent { SerialNumber = t.SerialNumber, Timestamp = t.Firstseen });
                    }
                }));
            }
            catch(System.Threading.ThreadInterruptedException intExc)
            {
                System.Diagnostics.Trace.Write(intExc.Message);
            }
        }

        private void NewAggregateEvent(AggregateEvent aggEvent)
        {
            try
            {
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    AggregateEvents.Add(aggEvent);
                }));
            }
            catch (System.Threading.ThreadInterruptedException intExc)
            {
                System.Diagnostics.Trace.Write(intExc.Message);
            }
        }

        public override void Cleanup()
        {
            base.Cleanup();
            Messenger.Default.Unregister<QuadratureStateChangeMessage>(this);
            Messenger.Default.Unregister<GPSEventMessage>(this);
        }
    }    
}
