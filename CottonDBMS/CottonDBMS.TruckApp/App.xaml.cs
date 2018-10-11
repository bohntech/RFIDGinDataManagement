using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CottonDBMS.TruckApp.DataProviders;
using GalaSoft.MvvmLight.Ioc;
using CottonDBMS.TruckApp.Navigation;
using CottonDBMS.TruckApp.ViewModels;
using CottonDBMS.DataModels;
using CottonDBMS.Interfaces;
using CottonDBMS.EF;
using System.Diagnostics;
using IWshRuntimeLibrary;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;


namespace CottonDBMS.TruckApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public bool alreadyRunningProcess()
        {
            Process thisProcess = Process.GetCurrentProcess();
            Process[] matchingProcs = Process.GetProcessesByName(thisProcess.ProcessName);
            foreach (Process p in matchingProcs)
            {
                if ((p.Id != thisProcess.Id) &&
                    (p.MainModule.FileName == thisProcess.MainModule.FileName))
                    return true;
            }
            return false;
        }

        private void createStartupLink()
        {
            
            /*var wshCSClass = new WshShellClass();
            IWshRuntimeLibrary.IWshShortcut appShortcut;
            appShortcut = (IWshRuntimeLibrary.IWshShortcut)wshCSClass.CreateShortcut(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "CottonDBMSTruckApp.lnk"));
            appShortcut.TargetPath = System.Reflection.Assembly.GetExecutingAssembly().Location;            
            appShortcut.WorkingDirectory = System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("CottonDBMS.TruckApp.exe", "");
            appShortcut.Description = "Start RFID Cotton Truck App";
            //appShortcut.IconLocation = Application.StartupPath + @"\Resources\favicon.ico";
            //appShortcut.Arguments = " -maximized";
            appShortcut.WindowStyle = 7;
            appShortcut.Save();*/
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                if (alreadyRunningProcess()) Application.Current.Shutdown(-2);

                

                //Force database to be stored in same folder as program for easy location
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                dir = dir.TrimEnd('\\') + "\\" + FolderConstants.ROOT_APP_DATA_FOLDER;

                if (!System.IO.Directory.Exists(dir))
                {
                    System.IO.Directory.CreateDirectory(dir);
                }

                string truckLogDir = dir + "\\"+FolderConstants.TRUCK_APP_DATA_FOLDER+"\\";
                if (!System.IO.Directory.Exists(truckLogDir))
                {
                    System.IO.Directory.CreateDirectory(truckLogDir);
                }

                string currentDirectory = System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("CottonDBMS.TruckApp.exe", "");

                CottonDBMS.Logging.Logger.SetLogPath(truckLogDir);

                var shortcutFile = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "CottonDBMSTruckApp.lnk");

                try
                {
                    if (ConfigurationManager.AppSettings["AllowStartupAndShutdown"].ToLower() == "true")
                    {
                        if (!System.IO.File.Exists(shortcutFile))
                        {
                            createStartupLink();
                        }
                    }
                    else
                    {
                        if (System.IO.File.Exists(shortcutFile))
                        {
                            System.IO.File.Delete(shortcutFile);
                        }
                    }
                }
                catch (Exception exc)
                {
                    Logging.Logger.Log(exc);
                }

                Logging.Logger.Log("INFO", "Data dir: " + dir);
                AppDomain.CurrentDomain.SetData("DataDirectory", dir.TrimEnd('\\'));

                using (var context = new AppDBContext())
                {

                    bool dbExists = context.Database.Exists();

                    if (!dbExists || !context.Database.CompatibleWithModel(false))
                    {
                        Logging.Logger.Log("INFO", "Creating database");
                        var configuration = new CottonDBMS.Data.EF.Migrations.Configuration();

                        var migrator = new DbMigrator(configuration);
                        migrator.Configuration.TargetDatabase = new DbConnectionInfo(context.Database.Connection.ConnectionString, "System.Data.SqlClient");
                        var migrations = migrator.GetPendingMigrations();
                        if (migrations.Any())
                        {
                            migrator.Update();
                        }
                    }
                    if (!dbExists) context.ApplySeeds();
                    context.Dispose();
                }

                SimpleIoc.Default.Register<AddPickUpListViewModel>();
                SimpleIoc.Default.Register<DataSyncSettingsViewModel>();
                SimpleIoc.Default.Register<DiagnosticsViewModel>();
                SimpleIoc.Default.Register<HomeViewModel>();
                SimpleIoc.Default.Register<NavViewModel>();
                SimpleIoc.Default.Register<SettingsViewModel>();
                SimpleIoc.Default.Register<TruckSettingsViewModel>();
                SimpleIoc.Default.Register<PickUpListViewModel>();
                SimpleIoc.Default.Register<LoadingWindowViewModel>();
                SimpleIoc.Default.Register<LoadingIncorrectModuleViewModel>();
                SimpleIoc.Default.Register<UnloadingModuleViewModel>();
                SimpleIoc.Default.Register<ChangeListViewModel>();
                SimpleIoc.Default.Register<UnloadingAtGinViewModel>();
                SimpleIoc.Default.Register<LoadingAtGinViewModel>();
                SimpleIoc.Default.Register<UnloadCorrectionViewModel>();
                SimpleIoc.Default.Register<TruckSetupViewModel>();

                SimpleIoc.Default.Register<IWindowService, WindowService>();
                SimpleIoc.Default.Register<IUnitOfWork, UnitOfWork>();
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                Logging.Logger.CleanUp();
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
           
        }

        private void Application_SessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            try
            {
                //CottonDBMS.EF.Tasks.TruckPullFromCloudTask.Cancel();
                //TagDataProvider.Disconnect();
                //GPSDataProvider.Disconnect();                
                //QuadratureEncoderDataProvider.Dispose();                
            }
            catch(Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }
    }
}

