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
using System.Windows.Shapes;
using System.Security.Permissions;
using CottonDBMS.TruckApp.ViewModels;
using System.IO;
using CottonDBMS.TruckApp.DataProviders;
using CottonDBMS.TruckApp.Messages;
using CottonDBMS.DataModels;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Ioc;
using CottonDBMS.TruckApp.Navigation;
using CottonDBMS.Interfaces;

namespace CottonDBMS.TruckApp
{
    /// <summary>
    /// Interaction logic for PickupWindow.xaml
    /// </summary>    
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class PickupWindow : Window
    {
        PickUpListViewModel vm = null;
        bool documentCompleted = false;
        bool headerIsChecked = false;

        public PickupWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            documentCompleted = false;
            vm = (PickUpListViewModel)DataContext;
            vm.Initialize();
            if (vm.HasNetwork)
            {
                loadBrowser();
            }
            Messenger.Default.Register<NetworkStatus>(this, (action) => ProcessNetworkStatusChange(action));
            Messenger.Default.Register<MapViewRequested>(this, (action) => ProcessMapViewRequested(action));
        }

        private void loadBrowser()
        {
            string currentDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            currentDir = currentDir.TrimEnd('\\');

            string appDataTruckDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).TrimEnd('\\') + "\\" + FolderConstants.ROOT_APP_DATA_FOLDER + "\\"
                + FolderConstants.TRUCK_APP_DATA_FOLDER + "\\Html";

            if (!Directory.Exists(appDataTruckDir)) Directory.CreateDirectory(appDataTruckDir);

            browser.ObjectForScripting = this;

            browser.LoadCompleted -= Browser_LoadCompleted;
            documentCompleted = false;
            browser.LoadCompleted += Browser_LoadCompleted;

            using (var uow = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
            {
                var syncedSettings = uow.SyncedSettingsRepo.GetAll().FirstOrDefault();

                if (syncedSettings != null)
                {
                    string html = File.ReadAllText(string.Format("{0}/Html/map.html", currentDir));
                    html = html.Replace("{MAPS_KEY}", syncedSettings.GoogleMapsKey);
                    html = html.Replace("{INIT_SCRIPT}", getInitScript());
                    File.WriteAllText(string.Format("{0}\\mapWithKey.html", appDataTruckDir), html);
                    System.Random rand = new Random();
                    browser.Source = new Uri(string.Format("file:///{0}/mapWithKey.html", appDataTruckDir));
                }
                else
                {
                    MessageBox.Show("Unable to show map.  Google maps key missing.");
                }
            }                   
        }

        private void ProcessNetworkStatusChange(NetworkStatus statusMessage)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (statusMessage.HasInternet)
                {
                    loadBrowser();
                }
                else
                {
                    browser.LoadCompleted -= Browser_LoadCompleted;                    
                    browser.Source = new Uri("about:blank");
                }
            }));
        }

        private void ProcessMapViewRequested(MapViewRequested msg)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
            {                
                loadBrowser();               
            }));
        }

        private string getInitScript()
        {
            StringBuilder sb = new StringBuilder();
            string parmLat;
            string parmLong;
            var coords = GPSDataProvider.GetLastCoords(DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow);

            if (vm.FieldLat != 0.000 && vm.FieldLong != 0.000)
            {
                parmLat = vm.FieldLat.ToString();
                parmLong = vm.FieldLong.ToString();
            }
            else if (coords != null)
            {
                parmLat = coords.Latitude.ToString();
                parmLong = coords.Longitude.ToString();
            }
            else
            {
                parmLat = "33.5749";
                parmLong = "-101.8572";
            }

            //browser.InvokeScript("setMyLocation", parms.ToArray());
            sb.AppendLine(string.Format("setMyLocation('{0}', '{1}');", parmLat, parmLong));
            foreach (var point in vm.Modules.Where(m => m.ShowOnMap && !m.Loaded))
            {   
                sb.AppendLine(string.Format("addModule('{0}', '{1}', '{2}');", point.Latitude.ToString(), point.Longitude.ToString(), point.SerialNumber));
            }

            if (vm.Modules.Count() == 0)
            {
                sb.AppendLine(string.Format("addModule('{0}', '{1}', '{2}');", parmLat.ToString(), parmLong.ToString(), vm.Client + " - " + vm.Farm + " - " + vm.Field));                
            }

            return sb.ToString();
        }

        private void Browser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (vm.HasNetwork && !documentCompleted)
            {                
                documentCompleted = true;
                browser.Source = browser.Source;
                /*try
                {
                    //browser.InvokeScript("initMap", new String[] { "" });
                }
                catch (Exception exc)
                {
                    Logging.Logger.Log(exc);
                    dgModules.Visibility = Visibility.Visible;
                    browser.Visibility = Visibility.Collapsed;
                }*/
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            browser.LoadCompleted -= Browser_LoadCompleted;
            Messenger.Default.Unregister<NetworkStatus>(this);
            Messenger.Default.Unregister<MapViewRequested>(this);
            vm.DoCleanUp();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            IWindowService windowService = SimpleIoc.Default.GetInstance<IWindowService>();
            windowService.FocusLast(WindowType.WaitingForUnloadWindow);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            int i = 0;
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (Keyboard.IsKeyDown(Key.G))
                {
                    CottonDBMS.TruckApp.Windows.OverrideGPS overrideWindow = new CottonDBMS.TruckApp.Windows.OverrideGPS();
                    overrideWindow.Show();
                }
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            headerIsChecked = !headerIsChecked;
            foreach (var item in dgModules.Items)
            {                
                ModuleViewModel m = item as ModuleViewModel;
                m.Selected = headerIsChecked;
            }
        }
    }  
}
