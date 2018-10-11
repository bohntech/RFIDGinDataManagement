//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.Interfaces;
using CottonDBMS.EF;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;

namespace CottonDBMS.GinApp
{
    public class UnitOfWorkFactory
    {
        static UnitOfWorkFactory()
        {
            try
            {
                var context = new AppDBContext();

                bool dbExists = context.Database.Exists();                

                if (!dbExists || !context.Database.CompatibleWithModel(false))
                {
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
            }
            catch (Exception exc)
            {
                var msg = exc.Message;
            }

            CottonDBMS.EF.Tasks.GinSyncWithCloudTask.Init();
        }

        public static IUnitOfWork CreateUnitOfWork()
        {
            return (IUnitOfWork)new UnitOfWork();
        }
    }
}
