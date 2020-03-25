using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CottonDBMS.BridgePBIApp.Navigation;
using CottonDBMS.DataModels;
using CottonDBMS.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System.Timers;
using CottonDBMS.Bridges.Shared.Messages;
using CottonDBMS.Bridges.Shared;
using CottonDBMS.Bridges.Shared.Navigation;
using CottonDBMS.Bridges.Shared.ViewModels;
using CottonDBMS.BridgePBIApp.ViewModels;
using System.Collections.ObjectModel;
using CottonDBMS.Bridges.Shared.Tasks;
using CottonDBMS.BridgePBIApp.Helpers;

namespace CottonDBMS.BridgeFeederApp.ViewModels
{

    public class ScanItemViewModel : ViewModelBase
    {
        private DateTime _timestamp;
        public DateTime TimeStamp
        {
            get
            {
                return _timestamp;
            }
            set
            {
                Set<DateTime>(() => TimeStamp, ref _timestamp, value);
                LocalTimestamp = value.ToLocalTime().ToString("MM/dd/yyyy hh:mm:ss tt");
            }
        }

        private string _localTimestamp;
        public string LocalTimestamp
        {
            get
            {
                return _localTimestamp;
            }
            set
            {
                Set<string>(() => LocalTimestamp, ref _localTimestamp, value);
            }
        }
                

        private string _barCode;
        public string Barcode
        {
            get
            {
                return _barCode;
            }
            set
            {
                Set<string>(() => Barcode, ref _barCode, value);
            }
        }

        private bool _outOfSequence;
        public bool OutOfSequence
        {
            get
            {
                return _outOfSequence;
            }
            set
            {
                Set<bool>(() => OutOfSequence, ref _outOfSequence, value);
            }
        }

        private decimal _weight;
        public decimal Weight
        {
            get
            {
                return _weight;
            }
            set
            {
                Set<decimal>(() => Weight, ref _weight, value);
            }
        }

        /*public override string ToString()
        {
            string sequenceStr = "";
            if (OutOfSequence) sequenceStr = " - OUT OF SEQUENCE ";
            return PbiNumber.ToString().Trim().PadLeft(25, ' ') + (ScaleWeight.ToString("0.00") + "LBS").PadLeft(25, ' ') + Created.ToLocalTime().ToString("MM/dd/yyyy hh:mm:ss tt").PadLeft(25, ' ') + sequenceStr;
        }*/
    }

    public class IdlePageViewModel : BasePageViewModel
    {

        private decimal _weight = 0.00M;
        private long lastPBINumber = 0;
        private bool waitingForWeight = false;
        
        private ObservableCollection<ScanItemViewModel> _PBIScans;
        public ObservableCollection<ScanItemViewModel> PBIScans
        {
            get
            {
                return _PBIScans;
            }
            set
            {
                Set<ObservableCollection<ScanItemViewModel>>(() => PBIScans, ref _PBIScans, value);
            }
        }

        public IdlePageViewModel(INavigationService navService) : base(navService)
        {
            Messenger.Default.Register<BarcodeScannedMessage>(this, handleBarCodeScanned);
            Messenger.Default.Register<ScaleWeightReportMessage>(this, handleWeightReportMessage);
            Messenger.Default.Register<WeightAcquiredMessage>(this, handleWeightAcquired);
        }        

        public void Initialize()
        {
            using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
            {
                var lastScanNumber = dp.BaleScanRepository.LastScanNumber();
                
                var bales = dp.BaleScanRepository.Last200Scans();

                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                {

                    PBIScans = new ObservableCollection<ScanItemViewModel>();
                    var lastBale = bales.OrderBy(b => b.ScanNumber).LastOrDefault();
                    long temp = 0;
                    if (lastBale != null && long.TryParse(lastBale.PbiNumber, out temp))
                        lastPBINumber = temp;
                    else
                        lastPBINumber = 0L;

                    foreach (var b in bales)
                        PBIScans.Add(new ScanItemViewModel { Barcode = b.Name, OutOfSequence = b.OutOfSequence, TimeStamp = b.LastCreatedOrUpdated, Weight = b.ScaleWeight - b.TareWeight });
                }));
            }
        }

        public override void Cleanup()
        {           
            Messenger.Default.Unregister<BarcodeScannedMessage>(this);
            Messenger.Default.Unregister<ScaleWeightReportMessage>(this);
            Messenger.Default.Unregister<WeightAcquiredMessage>(this);
            base.Cleanup();
        }

        private void handleWeightReportMessage(ScaleWeightReportMessage msg)
        {
            _weight = msg.Weight;
        }

        private void handleWeightAcquired(WeightAcquiredMessage msg)
        {
            if (waitingForWeight)
            {
                try
                {
                    using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                    {
                        var scanNumber = dp.BaleScanRepository.LastScanNumber();
                        var scan = dp.BaleScanRepository.FindSingle(s => s.ScanNumber == scanNumber);
                        if (scan != null && scan.Created >= DateTime.Now.AddMinutes(-5))
                        {
                            scan.ScaleWeight = msg.Weight;
                            scan.SyncedToCloud = false;
                            scan.Source = InputSource.TRUCK;
                            dp.BaleScanRepository.Save(scan);
                            dp.SaveChanges();

                            var displayedScan = PBIScans.FirstOrDefault(s => s.Barcode == scan.PbiNumber);

                            //update weight on displayed scan
                            if (displayedScan != null)
                                displayedScan.Weight = scan.NetLintBaleWeight;

                            Task.Run(() =>
                            {
                                if (PBISyncHelper.IsRunning)
                                {
                                    PBISyncHelper.WaitForCompletion();
                                }
                                PBISyncHelper.RunSync();
                            });
                        }
                    }
                }
                catch(Exception exc)
                {
                    Logging.Logger.Log(exc);
                }
                waitingForWeight = false;
            }
        }

        private void handleBarCodeScanned(BarcodeScannedMessage msg)
        {
            if (msg.Data.Length != 12) return;

            using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
            {
                BaleScanEntity scan = new BaleScanEntity();
                
                long newPBINumber = 0;

                if (!long.TryParse(msg.Data, out newPBINumber))
                    newPBINumber = 0;

                if (lastPBINumber == newPBINumber)
                    return;

                scan.OutOfSequence = (lastPBINumber != 0) && newPBINumber != (lastPBINumber + 1);

                lastPBINumber = newPBINumber;
                scan.ScanNumber = dp.BaleScanRepository.LastScanNumber() + 1;
                scan.PbiNumber = msg.Data;
                scan.Name = msg.Data;
                scan.Processed = false;
                scan.SyncedToCloud = false;
                scan.ScaleWeight = _weight;
                scan.TareWeight = decimal.Parse(dp.SettingsRepository.GetSettingWithDefault(BridgeSettingKeys.TARE_WEIGHT, "0"));                
                dp.BaleScanRepository.Save(scan);
                dp.SaveChanges();
                waitingForWeight = true;
                Logging.Logger.Log("SCAN", scan.ToString());

                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    if (PBIScans.Count() >= 200)
                    {
                        PBIScans.RemoveAt(PBIScans.Count() - 1);
                    }

                    PBIScans.Insert(0, new ScanItemViewModel { Barcode = scan.Name, OutOfSequence = scan.OutOfSequence, TimeStamp = scan.LastCreatedOrUpdated, Weight = scan.NetLintBaleWeight });
                }));


                Task.Run(() =>
                {
                    if (PBISyncHelper.IsRunning)
                    {
                        PBISyncHelper.WaitForCompletion();
                    }
                    PBISyncHelper.RunSync();
                });
            }
        }
    }
}

