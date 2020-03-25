using CottonDBMS.BridgeFeederApp.ViewModels;
using CottonDBMS.Bridges.Shared.Messages;
using CottonDBMS.Bridges.Shared.Navigation;
using CottonDBMS.Bridges.Shared;
using CottonDBMS.Bridges.Shared.Tasks;
using CottonDBMS.DataModels;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
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
using CottonDBMS.BridgePBIApp.ViewModels;
using CottonDBMS.Bridges.Shared.Helpers;
using CottonDBMS.BridgePBIApp.Helpers;

namespace CottonDBMS.BridgePBIApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ShellViewModel shellVM = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                CottonDBMS.Logging.Logger.Log("INFO", "PBI APPLICATION LAUNCH");
                Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                Messenger.Default.Register<ContentPageChangedMessage>(this, handleContentPageChanged);                
                INavigationService navService = SimpleIoc.Default.GetInstance<INavigationService>();
                shellVM = new ShellViewModel(navService);
                shellVM.AllowBarcoderConnect = true;
                shellVM.AllowRFIDReaderConnect = false;
                shellVM.AllowScaleConnect = true;
                shellVM.AllowWeighInTimeOut = false;
                this.DataContext = shellVM;
                shellVM.Initialize();
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                Logging.Logger.CleanUp();
            }
        }

        private void handleContentPageChanged(ContentPageChangedMessage msg)
        {
            mainFrame.Content = msg.Content;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                PBISyncHelper.Cancel();

                if (shellVM != null)
                    shellVM.Cleanup();
                                
                Messenger.Default.Unregister<ContentPageChangedMessage>(this);
                Logging.Logger.CleanUp();
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            ScaleSimHelper.RunSim();
        }
    }
}
