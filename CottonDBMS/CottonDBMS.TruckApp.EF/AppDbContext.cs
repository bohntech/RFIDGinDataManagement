//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using CottonDBMS.Data;
using CottonDBMS.DataModels;
using System.ComponentModel.DataAnnotations.Schema;
using CottonDBMS.EF.Tasks;
using System.Data.Entity.Migrations;
using System.Data.Entity.Infrastructure;

namespace CottonDBMS.EF
{
    public class AppDBContext : DbContext
    {        
        public DbSet<Setting> Settings { get; set; }
        public DbSet<ClientEntity> Clients { get; set; }
        public DbSet<FarmEntity> Farms { get; set; }
        public DbSet<FieldEntity> Fields { get; set; }
        public DbSet<ModuleEntity> Modules { get; set; }
        public DbSet<ModuleHistoryEntity> ModuleHistory { get; set; }
        public DbSet<TruckEntity> Truck { get; set; }
        public DbSet<DriverEntity> Driver { get; set; }
        public DbSet<PickupListEntity> PickupLists { get; set; }
        public DbSet<DocumentToProcess> DocumentsToProcess { get; set; }
        public DbSet<SyncedSettings> SyncedSettings { get; set; }
        public DbSet<AggregateEvent> AggregateEvents { get; set; }
        public DbSet<TruckListsDownloaded> TruckListsDownloaded { get; set; }
        public DbSet<BaleEntity> Bales { get; set; }
        public DbSet<GinLoadEntity> GinLoads { get; set; }
        public DbSet<TruckRegistrationEntity> TruckRegistrations { get; set; }
        public DbSet<LoadScanEntity> LoadScans { get; set; }
        public DbSet<BaleScanEntity> BaleScans { get; set; }
        public DbSet<ModuleOwnershipEntity> ModuleOwnerships { get; set; }
        public DbSet<FeederScanEntity> FeederScans { get; set; }

        public AppDBContext() : base()
        {   
            InitializeDatabase();                        
        }

        /// <summary>
        /// Initialize entity keys and schema requirements
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Setting>()
                .HasKey<string>(s => s.Key)
                .Property(p => p.Key).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<AggregateEvent>()
                .HasKey<string>(s => s.Id)
                .Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<TruckListsDownloaded>()
                .HasKey<string>(s => s.Id)
                .Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<SyncedSettings>()
                .HasKey<string>(s => s.Id)
                .Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<DocumentToProcess>()
                .HasKey<string>(s => s.Id)
                .Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<ClientEntity>()
                .HasKey<string>(s => s.Id)
                .Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<ClientEntity>().Property(p => p.Name).HasMaxLength(100)
               .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<FarmEntity>()
                .HasKey<string>(s => s.Id)
                .Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<FarmEntity>().Property(p => p.Name).HasMaxLength(100)
              .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<FieldEntity>()
                .HasKey<string>(s => s.Id)
                .Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<FieldEntity>().Property(p => p.Name).HasMaxLength(100)
              .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<ModuleEntity>()
                .HasKey<string>(s => s.Id)
                .Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<ModuleEntity>().Property(p => p.Name).HasMaxLength(100)
              .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<ModuleEntity>().Property(p => p.ModuleStatus)
              .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<ModuleEntity>().Property(p => p.Created)
              .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<ModuleEntity>().Property(p => p.Updated)
              .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<ModuleHistoryEntity>()
                .HasKey<string>(s => s.Id)
                .Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<ModuleHistoryEntity>().Property(p => p.Name).HasMaxLength(100)
              .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<ModuleHistoryEntity>().Property(p => p.ModuleStatus)
              .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<ModuleHistoryEntity>().Property(p => p.Created)
              .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<ModuleHistoryEntity>().Property(p => p.Updated)
              .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<TruckEntity>()
                .HasKey<string>(s => s.Id)
                .Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<DriverEntity>()
                .HasKey<string>(s => s.Id)
                .Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<ModuleEntity>()
            .HasRequired<FieldEntity>(s => s.Field)
            .WithMany(g => g.Modules)
            .HasForeignKey<string>(s => s.FieldId);

            modelBuilder.Entity<FieldEntity>()
           .HasRequired<FarmEntity>(s => s.Farm)
           .WithMany(g => g.Fields)
           .HasForeignKey<string>(s => s.FarmId);

            modelBuilder.Entity<FarmEntity>()
          .HasRequired<ClientEntity>(s => s.Client)
          .WithMany(g => g.Farms)
          .HasForeignKey<string>(s => s.ClientId);

            modelBuilder.Entity<ModuleEntity>()
            .HasOptional<PickupListEntity>(s => s.PickupList)
            .WithMany(g => g.AssignedModules)
            .HasForeignKey<string>(s => s.PickupListId).WillCascadeOnDelete(false);

            modelBuilder.Entity<ModuleHistoryEntity>()
          .HasRequired<ModuleEntity>(s => s.Module)
          .WithMany(g => g.ModuleHistory)
          .HasForeignKey<string>(s => s.ModuleId).WillCascadeOnDelete(true);

            modelBuilder.Entity<PickupListEntity>()
               .HasMany<TruckEntity>(s => s.AssignedTrucks)
               .WithMany(c => c.AssignedToLists)
               .Map(cs =>
               {
                   cs.MapLeftKey("PickupListId");
                   cs.MapRightKey("TruckId");
                   cs.ToTable("PickupListAssignedTrucks");
               });

            modelBuilder.Entity<PickupListEntity>()
                .Ignore(p => p.SearchSetModulesRemaining)
                .Ignore(p => p.SearchSetTotalModules)
               .HasMany<TruckEntity>(s => s.DownloadedToTrucks)
               .WithMany(c => c.DownloadedLists)
               .Map(cs =>
               {
                   cs.MapLeftKey("PickupListId");
                   cs.MapRightKey("TruckId");
                   cs.ToTable("PickupListDownloadedToTrucks");
               });

            modelBuilder.Entity<GinLoadEntity>()
               .HasKey<string>(s => s.Id)
               .Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<GinLoadEntity>().Property(t => t.ScaleBridgeLoadNumber)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_ScaleBridgeLoadNumber"))); //TODO REMOVE UNIQUE CONSTRAINT ??

            modelBuilder.Entity<GinLoadEntity>().Property(t => t.GinTagLoadNumber).HasMaxLength(50)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_GinTagLoadNumber") { IsUnique = true }));

            modelBuilder.Entity<ModuleEntity>()
            .HasOptional<GinLoadEntity>(s => s.GinLoad)
            .WithMany(g => g.Modules)
            .HasForeignKey<string>(s => s.GinLoadId).WillCascadeOnDelete(false);

            modelBuilder.Entity<BaleEntity>()
              .HasKey<string>(s => s.Id)
              .Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<BaleEntity>().Property(t => t.PbiNumber).HasMaxLength(50)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_PBINumber") { IsUnique = true }));

            modelBuilder.Entity<BaleEntity>().Property(t => t.Name).HasMaxLength(50)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Name") { IsUnique = true }));

            modelBuilder.Entity<BaleEntity>().Property(p => p.Updated)
              .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<BaleEntity>().Property(p => p.Created)
              .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<BaleEntity>().Property(p => p.ScanNumber)
              .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<BaleEntity>().Property(p => p.GinTicketLoadNumber).HasMaxLength(50)
              .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<BaleEntity>().Property(p => p.ModuleSerialNumber).HasMaxLength(50)
              .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            modelBuilder.Entity<BaleEntity>()
                .HasOptional<ModuleEntity>(s => s.Module)
                .WithMany(g => g.Bales)
                .HasForeignKey<string>(s => s.ModuleId);

            modelBuilder.Entity<BaleEntity>()
            .HasOptional<GinLoadEntity>(s => s.GinLoad)
            .WithMany(g => g.Bales)
            .HasForeignKey<string>(s => s.GinLoadId);

            modelBuilder.Entity<TruckRegistrationEntity>()
               .HasKey<string>(s => s.Id)
               .Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<LoadScanEntity>()
              .HasKey<string>(s => s.Id)
              .Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<ModuleOwnershipEntity>()
             .HasKey<string>(s => s.Id)
             .Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<FeederScanEntity>()
             .HasKey<string>(s => s.Id)
             .Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            base.OnModelCreating(modelBuilder);
        }


        public void ApplySeeds()
        {
            this.Settings.Add(new Setting { Key = TruckClientSettingKeys.A1_XMIT_POWER, Value = "25" });
            this.Settings.Add(new Setting { Key = TruckClientSettingKeys.A2_XMIT_POWER, Value = "25" });
            this.Settings.Add(new Setting { Key = TruckClientSettingKeys.A3_XMIT_POWER, Value = "25" });
            this.Settings.Add(new Setting { Key = TruckClientSettingKeys.A4_XMIT_POWER, Value = "25" });
            this.Settings.Add(new Setting { Key = TruckClientSettingKeys.A1_RECV_SENSITIVITY, Value = "25" });
            this.Settings.Add(new Setting { Key = TruckClientSettingKeys.A2_RECV_SENSITIVITY, Value = "25" });
            this.Settings.Add(new Setting { Key = TruckClientSettingKeys.A3_RECV_SENSITIVITY, Value = "25" });
            this.Settings.Add(new Setting { Key = TruckClientSettingKeys.A4_RECV_SENSITIVITY, Value = "25" });
            this.Settings.Add(new Setting { Key = TruckClientSettingKeys.RFID_READ_DELAY, Value = "2" });
            this.Settings.Add(new Setting { Key = TruckClientSettingKeys.TRUCK_ID, Value = "" });
            this.Settings.Add(new Setting { Key = TruckClientSettingKeys.DRIVER_ID, Value = "" });
            this.Settings.Add(new Setting { Key = TruckClientSettingKeys.DOCUMENT_DB_KEY, Value = "" });
            this.Settings.Add(new Setting { Key = TruckClientSettingKeys.DOCUMENTDB_ENDPOINT, Value = "" });
            this.Settings.Add(new Setting { Key = TruckClientSettingKeys.DATA_SYNC_INTERVAL, Value = "5" });
            this.Settings.Add(new Setting { Key = TruckClientSettingKeys.ADMIN_PASSWORD, Value = "" });
            this.Settings.Add(new Setting { Key = TruckClientSettingKeys.GPS_COM_PORT, Value = "COM13" });
            this.Settings.Add(new Setting { Key = TruckClientSettingKeys.MODULES_ON_TRUCK, Value = "" });

            this.Settings.Add(new Setting { Key = GinAppSettingKeys.IMPORT_FOLDER, Value = Environment.CurrentDirectory });
            this.Settings.Add(new Setting { Key = GinAppSettingKeys.ARCHIVE_FOLDER, Value = Environment.CurrentDirectory });
            this.Settings.Add(new Setting { Key = GinAppSettingKeys.IMAP_HOSTNAME, Value = "" });
            this.Settings.Add(new Setting { Key = GinAppSettingKeys.IMAP_PORT, Value = "" });
            this.Settings.Add(new Setting { Key = GinAppSettingKeys.IMAP_USERNAME, Value = "" });
            this.Settings.Add(new Setting { Key = GinAppSettingKeys.IMAP_PASSWORD, Value = "" });

            this.Settings.Add(new Setting { Key = GinAppSettingKeys.GIN_YARD_NW_CORNER_NORTH, Value = "33.688782" });
            this.Settings.Add(new Setting { Key = GinAppSettingKeys.GIN_YARD_NW_CORNER_WEST, Value = "-102.10453" });
            this.Settings.Add(new Setting { Key = GinAppSettingKeys.GIN_YARD_SE_CORNER_NORTH, Value = "33.68189" });
            this.Settings.Add(new Setting { Key = GinAppSettingKeys.GIN_YARD_SE_CORNER_WEST, Value = "-102.096419" });
            this.Settings.Add(new Setting { Key = GinAppSettingKeys.GIN_FEEDER_NORTH, Value = "33.675073" });
            this.Settings.Add(new Setting { Key = GinAppSettingKeys.GIN_FEEDER_WEST, Value = "-102.103211" });
            this.Settings.Add(new Setting { Key = GinAppSettingKeys.GIN_FEEDER_DETECTION_RADIUS, Value = "10.0" });
            this.Settings.Add(new Setting { Key = GinAppSettingKeys.GOOGLE_MAPS_API_KEY, Value = "" });
            this.Settings.Add(new Setting { Key = GinAppSettingKeys.IMPORT_INTERVAL, Value = "5" });
            this.Settings.Add(new Setting { Key = GinAppSettingKeys.AZURE_DOCUMENTDB_ENDPOINT, Value = "" });
            this.Settings.Add(new Setting { Key = GinAppSettingKeys.AZURE_DOCUMENTDB_KEY, Value = "" });
            this.Settings.Add(new Setting { Key = GinAppSettingKeys.AZURE_DOCUMENTDB_READONLY_ENDPOINT, Value = "" });
            this.Settings.Add(new Setting { Key = GinAppSettingKeys.AZURE_DOCUMENTDB_READONLY_KEY, Value = "" });
            this.Settings.Add(new Setting { Key = GinAppSettingKeys.LOAD_PREFIX, Value = "2018" });
            this.Settings.Add(new Setting { Key = GinAppSettingKeys.STARTING_LOAD_NUMBER, Value = "1" });
            this.Settings.Add(new Setting { Key = GinAppSettingKeys.MODULES_PER_LOAD, Value = "4" });
            this.Settings.Add(new Setting { Key = GinAppSettingKeys.GIN_NAME, Value = "" });

            this.SaveChanges();
        }

        /// <summary>
        /// Creates database and initialized default settings if does not already exist
        /// No longer needed now that automatic migrations have been enabled
        /// </summary>
        protected virtual void InitializeDatabase()
        {            
            
        } 


    }
}


