using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CottonDBMS.DataModels;
using System.Threading;
using System.Diagnostics;
using CottonDBMS.BridgeApp.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using CottonDBMS.EF;
using CottonDBMS.Interfaces;
using CottonDBMS.BridgeApp.Navigation;
using CottonDBMS.Bridges.Shared;
using CottonDBMS.Bridges.Shared.Navigation;
using CottonDBMS.Bridges.Shared.Helpers;

namespace CottonDBMS.BridgeApp
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
                StartupHelper.InitializeAppDomain(FolderConstants.SCALE_BRIDGE_APP_DATA_FOLDER);                            
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
