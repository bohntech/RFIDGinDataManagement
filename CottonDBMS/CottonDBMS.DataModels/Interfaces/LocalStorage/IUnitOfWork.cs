//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.DataModels;

namespace CottonDBMS.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
        ISettingsRepository SettingsRepository { get; }
        IClientRepository ClientRepository { get; }
        IDriverRepository DriverRepository { get; }
        IFarmRepository FarmRepository { get; }
        IFieldRepository FieldRepository { get; }
        IModuleRepository ModuleRepository { get; }
        IPickupListRepository PickupListRepository { get; }
        ITruckRepository TruckRepository { get; }
        IDocumentsToProcessRepository DocumentsToProcessRepository { get; }
        IEntityRepository<SyncedSettings> SyncedSettingsRepo { get; }
        IAggregateEventRepository AggregateEventRepository { get; }
        IEntityRepository<TruckListsDownloaded> TruckListsDownloadedRepo { get; }
        void BackupDB(string toFileName);
    }
}
