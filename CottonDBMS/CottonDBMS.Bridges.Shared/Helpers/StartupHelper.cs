using CottonDBMS.DataModels;
using CottonDBMS.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CottonDBMS.Bridges.Shared.Helpers
{
    public class StartupHelper
    {
        public static bool alreadyRunningProcess()
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

        public static void InitializeAppDomain(string appDataFolder)
        {
            if (alreadyRunningProcess()) Application.Current.Shutdown(-2);

            string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            dir = dir.TrimEnd('\\') + "\\" + FolderConstants.ROOT_APP_DATA_FOLDER;

            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }

            dir = dir + "\\" + appDataFolder + "\\";
            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }

            CottonDBMS.Logging.Logger.SetLogPath(dir);

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
        }
    }
}
