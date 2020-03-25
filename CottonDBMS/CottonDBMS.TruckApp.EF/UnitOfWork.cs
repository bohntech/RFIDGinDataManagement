//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.DataModels;
using CottonDBMS.Interfaces;
using CottonDBMS.EF.Repositories;
using CottonDBMS.EF;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Data;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace CottonDBMS.EF
{  
    public class UnitOfWork :  IUnitOfWork
    {
        AppDBContext context = null;
        private ISettingsRepository _settingsRepo = null;
        private IClientRepository _clientRepo = null;
        private IFarmRepository _farmRepo = null;
        private IFieldRepository _fieldRepo = null;
        private IModuleRepository _moduleRepo = null;
        private IPickupListRepository _pickupListRepo = null;
        private ITruckRepository _truckRepo = null;
        private IDriverRepository _driverRepo = null;
        private IDocumentsToProcessRepository _processRepo = null;
        private IEntityRepository<SyncedSettings> _syncedSettingsRepo = null;
        private IEntityRepository<TruckListsDownloaded> _truckListsDownloadedRepo = null;
        private IEntityRepository<TruckRegistrationEntity> _truckRegistrationsRepo = null;
        private ILoadScanRepository _loadScanRepo = null;
        private IFeederScanRepository _feederScanRepository = null;
        private IEntityRepository<ModuleOwnershipEntity> _moduleOwnershipRepo = null;
        private IAggregateEventRepository _aggregateEventRepo = null;
        private IGinLoadRepository _ginLoadRepo = null;
        private IBaleRepository _balesRepo = null;
        private IBaleScanRepository _baleScanRepo = null;


        public ISettingsRepository SettingsRepository
        {
            get
            {
                if (_settingsRepo == null)
                    _settingsRepo = (ISettingsRepository)new SettingsRepository(context);

                return _settingsRepo;
            }
        }

        public IClientRepository ClientRepository
        {
            get
            {
                if (_clientRepo == null)
                    _clientRepo = (IClientRepository) new ClientRepository(context); 
                return _clientRepo;
            }
        }

        public IFarmRepository FarmRepository
        {
            get
            {
                if (_farmRepo == null)
                    _farmRepo = (IFarmRepository)new FarmRepository(context);
                return _farmRepo;
            }
        }

        public IFieldRepository FieldRepository
        {
            get
            {
                if (_fieldRepo == null)
                    _fieldRepo = (IFieldRepository)new FieldRepository(context);
                return _fieldRepo;
            }
        }

        public IModuleRepository ModuleRepository
        {
            get
            {
                if (_moduleRepo == null)
                    _moduleRepo = (IModuleRepository)new ModuleRepository(context);
                return _moduleRepo;
            }
        }

        public IPickupListRepository PickupListRepository
        {
            get
            {
                if (_pickupListRepo == null)
                    _pickupListRepo = (IPickupListRepository)new PickupListRepository(context);
                return _pickupListRepo;
            }
        }

        public IDriverRepository DriverRepository
        {
            get
            {
                if (_driverRepo == null)
                    _driverRepo = (IDriverRepository)new DriverRepository(context);
                return _driverRepo;
            }
        }

        public ITruckRepository TruckRepository
        {
            get
            {
                if (_truckRepo == null)
                    _truckRepo = (ITruckRepository)new TruckRepository(context);
                return _truckRepo;
            }
        }

        public IEntityRepository<SyncedSettings> SyncedSettingsRepo
        {
            get
            {
                if (_syncedSettingsRepo == null)
                    _syncedSettingsRepo = (IEntityRepository<SyncedSettings>)new SyncedSettingRepository(context);
                return _syncedSettingsRepo;
            }
        }

        public IEntityRepository<TruckListsDownloaded> TruckListsDownloadedRepo
        {
            get
            {
                if (_truckListsDownloadedRepo == null)
                    _truckListsDownloadedRepo = (IEntityRepository<TruckListsDownloaded>)new  EntityRepository<TruckListsDownloaded>(context);
                return _truckListsDownloadedRepo;
            }
        }

        public IEntityRepository<TruckRegistrationEntity> TruckRegistrationsRepository
        {
            get
            {
                if (_truckRegistrationsRepo == null)
                    _truckRegistrationsRepo = (IEntityRepository<TruckRegistrationEntity>)new EntityRepository<TruckRegistrationEntity>(context);
                return _truckRegistrationsRepo;
            }
        }

        public ILoadScanRepository LoadScanRepository
        {
            get
            {
                if (_loadScanRepo == null)
                    _loadScanRepo = (ILoadScanRepository)new LoadScanRepository(context);
                return _loadScanRepo;
            }
        }

        public IFeederScanRepository FeederScanRepository
        {
            get
            {
                if (_feederScanRepository == null)
                    _feederScanRepository = (IFeederScanRepository)new FeederScanRepository(context);
                return _feederScanRepository;
            }
        }

        public IEntityRepository<ModuleOwnershipEntity> ModuleOwnershipRepository
        {
            get
            {
                if (_moduleOwnershipRepo == null)
                    _moduleOwnershipRepo = (IEntityRepository<ModuleOwnershipEntity>)new EntityRepository<ModuleOwnershipEntity>(context);
                return _moduleOwnershipRepo;
            }
        }

        public IAggregateEventRepository AggregateEventRepository
        {
            get
            {
                if (_aggregateEventRepo == null)
                    _aggregateEventRepo = (IAggregateEventRepository)new AggregateEventRepository(context);
                return _aggregateEventRepo;
            }
        }

        public IGinLoadRepository GinLoadRepository
        {
            get
            {
                if (_ginLoadRepo == null)
                    _ginLoadRepo = (IGinLoadRepository)new GinLoadRepository(context);
                return _ginLoadRepo;
            }
        }

        public IBaleRepository BalesRepository
        {
            get
            {
                if (_balesRepo == null)
                    _balesRepo = (IBaleRepository)new BaleRepository(context);
                return _balesRepo;
            }
        }

        public IBaleScanRepository BaleScanRepository
        {
            get
            {
                if (_baleScanRepo == null)
                    _baleScanRepo = (IBaleScanRepository)new BaleScanRepository(context);
                return _baleScanRepo;
            }
        }

        public IGenericRepository<TObject> GetGenericRepository<TObject>() where TObject: class
        {
            return new GenericRepository<TObject>(context);
        }


        public IDocumentsToProcessRepository DocumentsToProcessRepository
        {
            get
            {
                if (_processRepo == null)
                    _processRepo = (IDocumentsToProcessRepository) new DocumentsToProcessRepository(context);
                return _processRepo;
            }
        }

        public UnitOfWork()
        {
            context = new AppDBContext();            
        }

        public void DisableChangeTracking()
        {
            context.Configuration.AutoDetectChangesEnabled = false;
        }

        public void EnableChangeTracking()
        {
            context.Configuration.AutoDetectChangesEnabled = true;
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public DataTable GetDataTable(string query)
        {
            using (SqlConnection conn = new SqlConnection(context.Database.Connection.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Close();
                return dt;
            }
        }

        public string DBFileName
        {
            get
            {
                string dbFilename = new SqlConnectionStringBuilder(context.Database.Connection.ConnectionString).AttachDBFilename;
                return dbFilename;
            }
        }

        public void DeleteDb()
        {
            try
            {
                if (context != null)
                {
                    string dbFilename = new SqlConnectionStringBuilder(context.Database.Connection.ConnectionString).AttachDBFilename;

                    if (context.Database.Exists())
                    {
                        context.Database.Delete();
                        context.Dispose();
                    }                    
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                Console.WriteLine(exc.StackTrace);
            }
        }

        private void GrantAllAccess(string path)
        {
            bool exists = System.IO.Directory.Exists(path);
            if (!exists)
            {
                DirectoryInfo di = System.IO.Directory.CreateDirectory(path);               
            }

            DirectoryInfo dInfo = new DirectoryInfo(path);
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            dInfo.SetAccessControl(dSecurity);

        }

        public void BackupDB(string ginAppRoot)
        {
            string sql = @"
BACKUP DATABASE {DbName} TO  DISK = N'{toFileName}' 
WITH NOFORMAT, INIT,  NAME = N'CottonDBMSGINDB_{timestamp}', SKIP, REWIND, UNLOAD,  STATS = 10
";
            if (context.Database.Connection.State != ConnectionState.Open)
                context.Database.Connection.Open();

            string backupFolder = ginAppRoot + "\\Backup";

            GrantAllAccess(backupFolder);

            string toFileName = backupFolder + "\\" + "GIN_DB_BACKUP_" + DateTime.Now.ToString("yyyyMMdd_hh_mm_ss") + ".bak";

            sql = sql.Replace("{DbName}", context.Database.Connection.Database);
            sql = sql.Replace("{toFileName}", toFileName);
            sql = sql.Replace("{timestamp}", DateTime.Now.ToString("yyyyMMdd_HH_mm_ss"));
            context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, sql);
            
        }
    }
}