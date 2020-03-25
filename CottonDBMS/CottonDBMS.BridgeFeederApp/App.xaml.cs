using CottonDBMS.BridgeFeederApp.Navigation;
using CottonDBMS.Bridges.Shared;
using CottonDBMS.Bridges.Shared.Helpers;
using CottonDBMS.Bridges.Shared.Navigation;
using CottonDBMS.DataModels;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CottonDBMS.BridgeFeederApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                StartupHelper.InitializeAppDomain(FolderConstants.FEEDER_BRIDGE_APP_DATA_FOLDER);
                SimpleIoc.Default.Register<INavigationService, NavigationService>();
                SimpleIoc.Default.Register<IUnitOfWorkFactory, UnitOfWorkFactory>();
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                Logging.Logger.CleanUp();
            }
        }
    }
}
