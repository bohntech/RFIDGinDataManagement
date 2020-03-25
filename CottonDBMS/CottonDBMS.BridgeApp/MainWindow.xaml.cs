using CottonDBMS.BridgeApp.Navigation;
using CottonDBMS.BridgeApp.ViewModels;
using CottonDBMS.Bridges.Shared;
using CottonDBMS.Bridges.Shared.Helpers;
using CottonDBMS.Bridges.Shared.Messages;
using CottonDBMS.Bridges.Shared.Navigation;
using CottonDBMS.Bridges.Shared.Tasks;
using CottonDBMS.DataModels;
using CottonDBMS.Interfaces;
using CottonDBMS.RFID;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.IO.Ports;
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


namespace CottonDBMS.BridgeApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ShellViewModel shellVM = null;
        string inputBuffer = "";
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                CottonDBMS.Logging.Logger.Log("INFO", "APPLICATION LAUNCH");
           

                Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

                Messenger.Default.Register<ContentPageChangedMessage>(this, handleContentPageChanged);
                TagDataProvider.SetSettingsPath(FolderConstants.SCALE_BRIDGE_APP_DATA_FOLDER);

                INavigationService navService = SimpleIoc.Default.GetInstance<INavigationService>();
                shellVM = new ShellViewModel(navService);
                shellVM.AllowWeighInTimeOut = true;
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
                if (shellVM != null)
                    shellVM.Cleanup();

                BridgeSyncProcessHelper.Cancel();
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

            bool CtrlDown = System.Windows.Input.Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            bool TDown = Keyboard.IsKeyDown(Key.T);
            bool MDown = Keyboard.IsKeyDown(Key.M);

            if (CtrlDown && TDown)
            {
                using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                {
                    var truck = dp.TruckRepository.GetAll().ToList().First();
                    var message = new TagItem { AntennaePort = 1, Epc = truck.RFIDTagId, Firstseen = DateTime.UtcNow, Lastseen = DateTime.UtcNow };
                    var listtags = new List<TagItem>();
                    listtags.Add(message);
                    Messenger.Default.Send<List<TagItem>>(listtags);
                }
            }
            else if (CtrlDown && MDown)
            {
                Task.Run(() =>
                {
                    TagDataProvider.SpoofRandomTag();
                    System.Threading.Thread.Sleep(200);
                    TagDataProvider.SpoofRandomTag();
                    System.Threading.Thread.Sleep(200);
                    TagDataProvider.SpoofRandomTag();
                    System.Threading.Thread.Sleep(200);
                    TagDataProvider.SpoofRandomTag();
                    System.Threading.Thread.Sleep(200);
                });
            }

        }

        private void Window_TextInput(object sender, TextCompositionEventArgs e)
        {

        }

        //TODO ADD A SHELL VIEW MODEL
    }
}
