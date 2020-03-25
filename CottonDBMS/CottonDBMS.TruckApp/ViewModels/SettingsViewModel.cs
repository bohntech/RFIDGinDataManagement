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
using GalaSoft.MvvmLight.Ioc;

namespace CottonDBMS.TruckApp.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {

        private string _portName;
        public string PortName
        {
            get
            {
                return _portName;
            }
            set
            {
                Set<string>(() => PortName, ref _portName, value);
            }
        }

        public ObservableCollection<string> AvailablePorts { get; private set; }

        private int _rfidReadDelay;
        public int RFIDReadDelay
        {
            get
            {
                return _rfidReadDelay;
            }
            set
            {
                Set<int>(() => RFIDReadDelay, ref _rfidReadDelay, value);
            }
        }

        private int _antenna1TransmitPower;
        public int Antenna1TransmitPower
        {
            get
            {
                return _antenna1TransmitPower;
            }
            set
            {
                Set<int>(() => Antenna1TransmitPower, ref _antenna1TransmitPower, value);
            }
        }

        private int _antenna1ReceivePower;
        public int Antenna1ReceivePower
        {
            get
            {
                return _antenna1ReceivePower;
            }
            set
            {
                Set<int>(() => Antenna1ReceivePower, ref _antenna1ReceivePower, value);
            }
        }

        private int _antenna2TransmitPower;
        public int Antenna2TransmitPower
        {
            get
            {
                return _antenna2TransmitPower;
            }
            set
            {
                Set<int>(() => Antenna2TransmitPower, ref _antenna2TransmitPower, value);
            }
        }

        private int _antenna2ReceivePower;
        public int Antenna2ReceivePower
        {
            get
            {
                return _antenna2ReceivePower;
            }
            set
            {
                Set<int>(() => Antenna2ReceivePower, ref _antenna2ReceivePower, value);
            }
        }

        private int _antenna3TransmitPower;
        public int Antenna3TransmitPower
        {
            get
            {
                return _antenna3TransmitPower;
            }
            set
            {
                Set<int>(() => Antenna3TransmitPower, ref _antenna3TransmitPower, value);
            }
        }

        private int _antenna3ReceivePower;
        public int Antenna3ReceivePower
        {
            get
            {
                return _antenna3ReceivePower;
            }
            set
            {
                Set<int>(() => Antenna3ReceivePower, ref _antenna3ReceivePower, value);
            }
        }

        private int _antenna4TransmitPower;
        public int Antenna4TransmitPower
        {
            get
            {
                return _antenna4TransmitPower;
            }
            set
            {
                Set<int>(() => Antenna4TransmitPower, ref _antenna4TransmitPower, value);
            }
        }

        private int _antenna4ReceivePower;
        public int Antenna4ReceivePower
        {
            get
            {
                return _antenna4ReceivePower;
            }
            set
            {
                Set<int>(() => Antenna4ReceivePower, ref _antenna4ReceivePower, value);
            }
        }

        public RelayCommand OverrideGPSCommand { get; private set; }

        private void ExecuteOverrideGPSCommand()
        {
            var overrideWindow = new CottonDBMS.TruckApp.Windows.OverrideGPS();
            overrideWindow.ShowDialog();
        }

        public RelayCommand SaveCommand { get; private set; }

        private void ExecuteSave()
        {
            Task.Run(() =>
            {
                Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "Saving..." });

                try
                {
                    Settings settings = TagDataProvider.GetDefaultSettings();

                    if (settings != null)
                    {
                        settings.Antennas.TxPowerMax = false;
                        settings.Antennas.RxSensitivityMax = false;
                        settings.HoldReportsOnDisconnect = false;
                        settings.Report.Mode = ReportMode.Individual;
                        //settings.AutoStart.Mode = AutoStartMode.None;
                        settings.SearchMode = SearchMode.DualTarget;
                        settings.ReaderMode = ReaderMode.AutoSetStaticDRM;
                        settings.Report.IncludeFirstSeenTime = true;
                        settings.Report.IncludeLastSeenTime = true;
                        settings.Report.IncludeSeenCount = true;
                        settings.Keepalives.Enabled = true;
                        settings.Keepalives.EnableLinkMonitorMode = true;
                        settings.Keepalives.LinkDownThreshold = 5;
                        settings.Keepalives.PeriodInMs = 3000;

                        settings.Antennas.GetAntenna(1).RxSensitivityInDbm = (double)_antenna1ReceivePower;
                        settings.Antennas.GetAntenna(1).TxPowerInDbm = (double)_antenna1TransmitPower;

                        settings.Antennas.GetAntenna(2).RxSensitivityInDbm = (double)_antenna2ReceivePower;
                        settings.Antennas.GetAntenna(2).TxPowerInDbm = (double)_antenna2TransmitPower;

                        settings.Antennas.GetAntenna(3).RxSensitivityInDbm = (double)_antenna3ReceivePower;
                        settings.Antennas.GetAntenna(3).TxPowerInDbm = (double)_antenna3TransmitPower;

                        settings.Antennas.GetAntenna(4).RxSensitivityInDbm = (double)_antenna4ReceivePower;
                        settings.Antennas.GetAntenna(4).TxPowerInDbm = (double)_antenna4TransmitPower;

                        TagDataProvider.ApplySettings(settings);

                        Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "Settings saved." });
                        System.Threading.Thread.Sleep(3000);
                    }
                    else
                    {
                        Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "Unable to save reader settings. Reader disconnected?" });
                        System.Threading.Thread.Sleep(3000);
                    }

                    bool portChanged = false;
                    using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                    {
                        var repo = dp.SettingsRepository;
                        var setting = repo.FindSingle(x => x.Key == TruckClientSettingKeys.RFID_READ_DELAY);
                        if (setting != null)
                        {
                            setting.Value = RFIDReadDelay.ToString();
                            AggregateDataProvider.UpdateReadDelay(RFIDReadDelay);
                        }

                        var portSetting = repo.FindSingle(x => x.Key == TruckClientSettingKeys.GPS_COM_PORT);
                        if (portSetting != null)
                        {
                            if (PortName != "-- Select One --")
                            {
                                if (portSetting.Value != PortName) portChanged = true;
                                portSetting.Value = PortName;
                            }
                        }
                        else //Add Port setting
                        {
                            portSetting = new Setting();
                            portSetting.Value = PortName;
                            portSetting.Key = TruckClientSettingKeys.GPS_COM_PORT;
                            dp.SettingsRepository.Add(portSetting);
                            portChanged = true;
                        }
                        dp.SaveChanges();
                    }

                    if (portChanged)
                    {                        
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                        {
                            System.Windows.MessageBox.Show("Please close and restart the application for new GPS settings to take effect");
                        }));                        
                    }
                }
                catch (Exception exc)
                {
                    
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        System.Windows.MessageBox.Show("An error occurred saving settings to reader. " + exc.Message);
                    }));                    
                    Logging.Logger.Log(exc);                    
                }
                finally
                {
                    Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = false, Message = "" });
                }
            });
        }

        public SettingsViewModel()
        {
            SaveCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteSave);
            OverrideGPSCommand = new RelayCommand(this.ExecuteOverrideGPSCommand);
            AvailablePorts = new ObservableCollection<string>();
            
        }

        public void Initialize()
        {
            Settings settings = TagDataProvider.GetSettings();

            if (settings != null)
            {
                Antenna1ReceivePower = Convert.ToInt32(settings.Antennas.GetAntenna(1).RxSensitivityInDbm);
                Antenna1TransmitPower = Convert.ToInt32(settings.Antennas.GetAntenna(1).TxPowerInDbm);

                Antenna2ReceivePower = Convert.ToInt32(settings.Antennas.GetAntenna(2).RxSensitivityInDbm);
                Antenna2TransmitPower = Convert.ToInt32(settings.Antennas.GetAntenna(2).TxPowerInDbm);

                Antenna3ReceivePower = Convert.ToInt32(settings.Antennas.GetAntenna(3).RxSensitivityInDbm);
                Antenna3TransmitPower = Convert.ToInt32(settings.Antennas.GetAntenna(3).TxPowerInDbm);

                Antenna4ReceivePower = Convert.ToInt32(settings.Antennas.GetAntenna(4).RxSensitivityInDbm);
                Antenna4TransmitPower = Convert.ToInt32(settings.Antennas.GetAntenna(4).TxPowerInDbm);
            }

            if (Antenna1ReceivePower == 0) Antenna1ReceivePower = -80;
            if (Antenna2ReceivePower == 0) Antenna2ReceivePower = -80;
            if (Antenna3ReceivePower == 0) Antenna3ReceivePower = -80;
            if (Antenna4ReceivePower == 0) Antenna4ReceivePower = -80;

            if (Antenna1TransmitPower == 0) Antenna1TransmitPower = 30;
            if (Antenna2TransmitPower == 0) Antenna2TransmitPower = 30;
            if (Antenna3TransmitPower == 0) Antenna3TransmitPower = 30;
            if (Antenna4TransmitPower == 0) Antenna4TransmitPower = 30;
                        

            foreach (var p in System.IO.Ports.SerialPort.GetPortNames().OrderBy(s => s))
            {
                AvailablePorts.Add(p);
            }

            using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
            {
                var repo = dp.SettingsRepository;

                var setting = repo.FindSingle(x => x.Key == TruckClientSettingKeys.RFID_READ_DELAY);
                if (setting != null && !string.IsNullOrWhiteSpace(setting.Value))
                {
                    RFIDReadDelay = int.Parse(setting.Value);
                }

                var portSetting = repo.FindSingle(x => x.Key == TruckClientSettingKeys.GPS_COM_PORT);

                if (portSetting != null && !string.IsNullOrWhiteSpace(portSetting.Value))
                {
                    PortName = portSetting.Value;
                }
                else
                {
                    if (AvailablePorts.Count > 0)
                    {
                        PortName = AvailablePorts.First();
                    }
                    else
                    {
                        PortName = "";
                    }
                }
            }
        }
    }
}
