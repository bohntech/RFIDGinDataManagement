//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CottonDBMS.TruckApp.ViewModels;
using CottonDBMS.TruckApp.DataProviders;
using CottonDBMS.Interfaces;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Ioc;
using CottonDBMS.TruckApp.Messages;
using CottonDBMS.TruckApp.Navigation;
using CottonDBMS.Cloud;
using CottonDBMS.DataModels;
using CottonDBMS.EF.Tasks;
using System.Management;
using CottonDBMS.TruckApp.Windows;
using System.Diagnostics;
using System.Reflection;
using System.Configuration;
using IWshRuntimeLibrary;
using System.IO;



namespace CottonDBMS.TruckApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NavViewModel vm = null;        

        public MainWindow()
        {
            InitializeComponent();

            disableStylus();
        }

        private void handleShutDownMessage(ShutdownMessage msg)
        {
            Application.Current.Shutdown();
        }

        private void handleFirstRunWizardComplete(FirstSetupWizardComplete msg)
        {
            initialize();
        }

        private async Task checkForSettings()
        {
            using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
            {
                var endpoint = dp.SettingsRepository.GetSettingWithDefault(TruckClientSettingKeys.DOCUMENTDB_ENDPOINT, "");
                var key = dp.SettingsRepository.GetSettingWithDefault(TruckClientSettingKeys.DOCUMENT_DB_KEY, "");                              

                if (string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(key))
                {
                    //settings have not been saved so let's open the first run wizard
                    IWindowService windowService = SimpleIoc.Default.GetInstance<IWindowService>();
                    TruckSetupViewModel vm = new TruckSetupViewModel(windowService);
                    await vm.Initialize();
                    windowService.ShowModalWindow(WindowType.TruckSetupWindow, vm);
                }
                else
                {
                    initialize();  //settings have already been saved so continue initialiazation
                }
            }
        }

        private void initialize()
        {
            if (vm == null)
            {
                vm = SimpleIoc.Default.GetInstance<NavViewModel>();
                DataContext = vm;
                unlockSettingsControl.DataContext = vm;
                passwordModalControl.DataContext = vm;
                busyModal.DataContext = vm;
            }

            this.Visibility = Visibility.Visible;

            //check for gps offsett setting
            using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
            {
                var repo = dp.SettingsRepository;
                var offsetSetting = repo.FindSingle(s => s.Key == TruckClientSettingKeys.GPS_OFFSET_FEET);

                if (offsetSetting == null)
                {
                    repo.UpsertSetting(TruckClientSettingKeys.GPS_OFFSET_FEET, "45");
                    GPSDataProvider.SetGpsOffsetFeet(45.0);
                }
                else
                {
                    GPSDataProvider.SetGpsOffsetFeet(double.Parse(offsetSetting.Value));
                }
            }

            if (!vm.HasCloudSettings)
            {
                MessageBox.Show("Data sync settings have not been configured.  Please configure then restart the application.");
                navTabControl.SelectedIndex = 2;
            }
            else
            {
                Task.Run(() =>
                {
                    try
                    {

                        try
                        {
                            if (!TagDataProvider.IsConnected)
                            {
                                Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "Looking for RFID reader..." });
                                //System.Threading.Thread.Sleep(35000);
                                int tryNumber = 1;

                                do
                                {

                                    //todo fix error hangs when no reader on rugged pc
                                    TagDataProvider.Connect();
                                    tryNumber++;
                                    System.Threading.Thread.Sleep(3000);

                                } while (tryNumber < 10 && !TagDataProvider.IsConnected);

                                //Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "Connecting to RFID reader..." });                            
                                //TagDataProvider.Connect();
                                //System.Threading.Thread.Sleep(1400);
                            }

                            if (!TagDataProvider.IsConnected)
                            {
                                Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "Not connected to RFID Reader." });
                                System.Threading.Thread.Sleep(1200);
                            }
                            else
                            {
                                TagDataProvider.Stop();
                                Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "RFID Reader found." });
                                TagDataProvider.SyncReaderTime();
                                System.Threading.Thread.Sleep(1200);
                            }
                        }
                        catch (Exception readerExc)
                        {
                            Logging.Logger.Log(readerExc);
                        }

                        Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "Initializing shaft sensor..." });
                        QuadratureEncoderDataProvider.StartEvents();
                        System.Threading.Thread.Sleep(1000);

                        if (!QuadratureEncoderDataProvider.IsStarted)
                        {
                            Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "Shaft Sensor not found." });
                            System.Threading.Thread.Sleep(1200);
                        }

                        if (!GPSDataProvider.IsConnected)
                        {
                            Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "Connecting to GPS receiver..." });

                            Task.Run(() =>
                            {
                                GPSDataProvider.Connect();
                            });

                            int waitCount = 0;
                            while (waitCount < 60 && !GPSDataProvider.IsConnected)
                            {
                                System.Threading.Thread.Sleep(500);
                                waitCount++;
                            }

                            if (GPSDataProvider.IsConnected)
                            {
                                Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "GPS connected." });
                                System.Threading.Thread.Sleep(1000);
                            }
                            else
                            {
                                Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "GPS not connected." });
                                System.Threading.Thread.Sleep(2000);
                            }
                        }

                        Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = false, Message = "" });

                        TagDataProvider.Initialize();
                        AggregateDataProvider.Initialize();
                        CottonDBMS.TruckApp.Tasks.TruckPullFromCloudTask.Init();
                        readerSettingsControl.Initialize();
                    }
                    catch (Exception exc)
                    {
                        CottonDBMS.Logging.Logger.Log(exc);
                    }
                });
            }

            navTabControl.Height = canvasLayout.ActualHeight;
            navTabControl.Width = canvasLayout.ActualWidth;
            modalBG.Height = canvasLayout.ActualHeight;
            modalBG.Width = canvasLayout.ActualWidth;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                CottonDBMS.Logging.Logger.Log("INFO", "APPLICATION LAUNCH");
                this.Visibility = Visibility.Hidden;

                Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

                Messenger.Default.Register<ShutdownMessage>(this, (action) => handleShutDownMessage(action));
                Messenger.Default.Register<FirstSetupWizardComplete>(this, (action) => handleFirstRunWizardComplete(action));

                await checkForSettings();
            }
            catch(Exception exc)
            {
                Logging.Logger.Log(exc);
                Logging.Logger.CleanUp();
            }                     
        }
        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                Messenger.Default.Unregister<FirstSetupWizardComplete>(this);

                CottonDBMS.TruckApp.Tasks.TruckPullFromCloudTask.Cancel();
                AggregateDataProvider.Cleanup();
                TagDataProvider.Disconnect();
                GPSDataProvider.Disconnect();
                QuadratureEncoderDataProvider.Dispose();                
                Logging.Logger.CleanUp();
                base.OnClosed(e);
            }
            catch (Exception exc)
            {
                System.Diagnostics.Trace.Write(exc.Message);
            }

            //if (ConfigurationManager.AppSettings["AllowStartupAndShutdown"].ToLower() == "true")
                //System.Diagnostics.Process.Start("Shutdown", "-s -t 10");
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            navTabControl.Height = canvasLayout.ActualHeight;
            navTabControl.Width = canvasLayout.ActualWidth;
            passwordModalControl.Width = canvasLayout.ActualWidth;
            passwordModalControl.Height = canvasLayout.ActualHeight;
            unlockSettingsControl.Width = canvasLayout.ActualWidth;
            unlockSettingsControl.Height = canvasLayout.ActualHeight;
            busyModal.Width = canvasLayout.ActualWidth;
            busyModal.Height = canvasLayout.ActualHeight;
            modalBG.Height = canvasLayout.ActualHeight;
            modalBG.Width = canvasLayout.ActualWidth;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            IWindowService windowService = SimpleIoc.Default.GetInstance<IWindowService>();
            windowService.FocusLast(WindowType.MainWindow);
        }

        public static void disableStylus()
        {             
            TabletDeviceCollection devices = System.Windows.Input.Tablet.TabletDevices;
            if (devices.Count > 0)
            {                
                Type inputManagerType = typeof(System.Windows.Input.InputManager);
             
                object stylusLogic = inputManagerType.InvokeMember("StylusLogic",
                            BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                            null, InputManager.Current, null);

                if (stylusLogic != null)
                {                  
                    Type devicesType = devices.GetType();                    
                    int count = devices.Count + 1;

                    while (devices.Count > 0)
                    {                        
                        devicesType.InvokeMember("HandleTabletRemoved", BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic, null, devices, new object[] { (uint)0 });
                        count--;
                        if (devices.Count != count)
                        {
                            throw new Exception("Unable to remove real-time stylus support.");
                        }
                    }
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            int i = 0;
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (Keyboard.IsKeyDown(Key.G))
                {
                    OverrideGPS overrideWindow = new OverrideGPS();
                    overrideWindow.Show();
                }
            }
        }
    }
}
