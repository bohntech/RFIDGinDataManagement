namespace CottonDBMS.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AggregateEvents",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Timestamp = c.DateTime(nullable: false),
                        EventType = c.Int(nullable: false),
                        SerialNumber = c.String(),
                        Epc = c.String(),
                        FirstLat = c.Double(nullable: false),
                        FirstLong = c.Double(nullable: false),
                        LastLat = c.Double(nullable: false),
                        LastLong = c.Double(nullable: false),
                        AverageLat = c.Double(nullable: false),
                        AverageLong = c.Double(nullable: false),
                        MedianLat = c.Double(nullable: false),
                        MedianLong = c.Double(nullable: false),
                        TruckID = c.String(),
                        DriverID = c.String(),
                        LoadNumber = c.String(),
                        ClientId = c.String(),
                        ClientName = c.String(),
                        FarmId = c.String(),
                        FarmName = c.String(),
                        FieldId = c.String(),
                        ModuleId = c.String(),
                        FieldName = c.String(),
                        PickupListId = c.String(),
                        PickupListName = c.String(),
                        Processed = c.Boolean(nullable: false),
                        Source = c.Int(nullable: false),
                        SelfLink = c.String(),
                        EntityType = c.Int(nullable: false),
                        Name = c.String(),
                        Created = c.DateTime(nullable: false),
                        SyncedToCloud = c.Boolean(nullable: false),
                        Updated = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClientEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Source = c.Int(nullable: false),
                        SelfLink = c.String(),
                        EntityType = c.Int(nullable: false),
                        Name = c.String(maxLength: 100),
                        Created = c.DateTime(nullable: false),
                        SyncedToCloud = c.Boolean(nullable: false),
                        Updated = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name);
            
            CreateTable(
                "dbo.FarmEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ClientId = c.String(nullable: false, maxLength: 128),
                        Source = c.Int(nullable: false),
                        SelfLink = c.String(),
                        EntityType = c.Int(nullable: false),
                        Name = c.String(maxLength: 100),
                        Created = c.DateTime(nullable: false),
                        SyncedToCloud = c.Boolean(nullable: false),
                        Updated = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClientEntities", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId)
                .Index(t => t.Name);
            
            CreateTable(
                "dbo.FieldEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FarmId = c.String(nullable: false, maxLength: 128),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Source = c.Int(nullable: false),
                        SelfLink = c.String(),
                        EntityType = c.Int(nullable: false),
                        Name = c.String(maxLength: 100),
                        Created = c.DateTime(nullable: false),
                        SyncedToCloud = c.Boolean(nullable: false),
                        Updated = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FarmEntities", t => t.FarmId, cascadeDelete: true)
                .Index(t => t.FarmId)
                .Index(t => t.Name);
            
            CreateTable(
                "dbo.ModuleEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FieldId = c.String(nullable: false, maxLength: 128),
                        PickupListId = c.String(maxLength: 128),
                        IsConventional = c.Boolean(nullable: false),
                        ModuleId = c.String(),
                        TruckID = c.String(),
                        Driver = c.String(),
                        LoadNumber = c.String(),
                        ImportedLoadNumber = c.String(),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        ModuleStatus = c.Int(nullable: false),
                        Notes = c.String(),
                        Source = c.Int(nullable: false),
                        SelfLink = c.String(),
                        EntityType = c.Int(nullable: false),
                        Name = c.String(maxLength: 100),
                        Created = c.DateTime(nullable: false),
                        SyncedToCloud = c.Boolean(nullable: false),
                        Updated = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FieldEntities", t => t.FieldId, cascadeDelete: true)
                .ForeignKey("dbo.PickupListEntities", t => t.PickupListId)
                .Index(t => t.FieldId)
                .Index(t => t.PickupListId)
                .Index(t => t.ModuleStatus)
                .Index(t => t.Name)
                .Index(t => t.Created)
                .Index(t => t.Updated);
            
            CreateTable(
                "dbo.ModuleHistoryEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ModuleId = c.String(nullable: false, maxLength: 128),
                        Driver = c.String(),
                        TruckID = c.String(),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        ModuleEventType = c.Int(nullable: false),
                        ModuleStatus = c.Int(nullable: false),
                        Source = c.Int(nullable: false),
                        SelfLink = c.String(),
                        EntityType = c.Int(nullable: false),
                        Name = c.String(maxLength: 100),
                        Created = c.DateTime(nullable: false),
                        SyncedToCloud = c.Boolean(nullable: false),
                        Updated = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ModuleEntities", t => t.ModuleId, cascadeDelete: true)
                .Index(t => t.ModuleId)
                .Index(t => t.ModuleStatus)
                .Index(t => t.Name)
                .Index(t => t.Created)
                .Index(t => t.Updated);
            
            CreateTable(
                "dbo.PickupListEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FieldId = c.String(maxLength: 128),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        PickupListStatus = c.Int(nullable: false),
                        Destination = c.Int(nullable: false),
                        ModulesPerLoad = c.Int(nullable: false),
                        Source = c.Int(nullable: false),
                        SelfLink = c.String(),
                        EntityType = c.Int(nullable: false),
                        Name = c.String(),
                        Created = c.DateTime(nullable: false),
                        SyncedToCloud = c.Boolean(nullable: false),
                        Updated = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FieldEntities", t => t.FieldId)
                .Index(t => t.FieldId);
            
            CreateTable(
                "dbo.TruckEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        LoadPrefix = c.String(),
                        Source = c.Int(nullable: false),
                        SelfLink = c.String(),
                        EntityType = c.Int(nullable: false),
                        Name = c.String(),
                        Created = c.DateTime(nullable: false),
                        SyncedToCloud = c.Boolean(nullable: false),
                        Updated = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DocumentToProcesses",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Action = c.Int(nullable: false),
                        Source = c.Int(nullable: false),
                        SelfLink = c.String(),
                        EntityType = c.Int(nullable: false),
                        Name = c.String(),
                        Created = c.DateTime(nullable: false),
                        SyncedToCloud = c.Boolean(nullable: false),
                        Updated = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DriverEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Firstname = c.String(),
                        Lastname = c.String(),
                        Source = c.Int(nullable: false),
                        SelfLink = c.String(),
                        EntityType = c.Int(nullable: false),
                        Name = c.String(),
                        Created = c.DateTime(nullable: false),
                        SyncedToCloud = c.Boolean(nullable: false),
                        Updated = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        Key = c.String(nullable: false, maxLength: 128),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Key);
            
            CreateTable(
                "dbo.SyncedSettings",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ModulesPerLoad = c.Int(nullable: false),
                        StartingLoadNumber = c.Int(nullable: false),
                        LoadPrefix = c.String(),
                        GoogleMapsKey = c.String(),
                        FeederLatitude = c.Double(nullable: false),
                        FeederLongitude = c.Double(nullable: false),
                        FeederDetectionRadius = c.Double(nullable: false),
                        GinYardNWLat = c.Double(nullable: false),
                        GinYardNWLong = c.Double(nullable: false),
                        GinYardSELat = c.Double(nullable: false),
                        GinYardSELong = c.Double(nullable: false),
                        Source = c.Int(nullable: false),
                        SelfLink = c.String(),
                        EntityType = c.Int(nullable: false),
                        Name = c.String(),
                        Created = c.DateTime(nullable: false),
                        SyncedToCloud = c.Boolean(nullable: false),
                        Updated = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TruckListsDownloadeds",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Source = c.Int(nullable: false),
                        SelfLink = c.String(),
                        EntityType = c.Int(nullable: false),
                        Name = c.String(),
                        Created = c.DateTime(nullable: false),
                        SyncedToCloud = c.Boolean(nullable: false),
                        Updated = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PickupListAssignedTrucks",
                c => new
                    {
                        PickupListId = c.String(nullable: false, maxLength: 128),
                        TruckId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.PickupListId, t.TruckId })
                .ForeignKey("dbo.PickupListEntities", t => t.PickupListId, cascadeDelete: true)
                .ForeignKey("dbo.TruckEntities", t => t.TruckId, cascadeDelete: true)
                .Index(t => t.PickupListId)
                .Index(t => t.TruckId);
            
            CreateTable(
                "dbo.PickupListDownloadedToTrucks",
                c => new
                    {
                        PickupListId = c.String(nullable: false, maxLength: 128),
                        TruckId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.PickupListId, t.TruckId })
                .ForeignKey("dbo.PickupListEntities", t => t.PickupListId, cascadeDelete: true)
                .ForeignKey("dbo.TruckEntities", t => t.TruckId, cascadeDelete: true)
                .Index(t => t.PickupListId)
                .Index(t => t.TruckId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ModuleEntities", "PickupListId", "dbo.PickupListEntities");
            DropForeignKey("dbo.PickupListEntities", "FieldId", "dbo.FieldEntities");
            DropForeignKey("dbo.PickupListDownloadedToTrucks", "TruckId", "dbo.TruckEntities");
            DropForeignKey("dbo.PickupListDownloadedToTrucks", "PickupListId", "dbo.PickupListEntities");
            DropForeignKey("dbo.PickupListAssignedTrucks", "TruckId", "dbo.TruckEntities");
            DropForeignKey("dbo.PickupListAssignedTrucks", "PickupListId", "dbo.PickupListEntities");
            DropForeignKey("dbo.ModuleHistoryEntities", "ModuleId", "dbo.ModuleEntities");
            DropForeignKey("dbo.ModuleEntities", "FieldId", "dbo.FieldEntities");
            DropForeignKey("dbo.FieldEntities", "FarmId", "dbo.FarmEntities");
            DropForeignKey("dbo.FarmEntities", "ClientId", "dbo.ClientEntities");
            DropIndex("dbo.PickupListDownloadedToTrucks", new[] { "TruckId" });
            DropIndex("dbo.PickupListDownloadedToTrucks", new[] { "PickupListId" });
            DropIndex("dbo.PickupListAssignedTrucks", new[] { "TruckId" });
            DropIndex("dbo.PickupListAssignedTrucks", new[] { "PickupListId" });
            DropIndex("dbo.PickupListEntities", new[] { "FieldId" });
            DropIndex("dbo.ModuleHistoryEntities", new[] { "Updated" });
            DropIndex("dbo.ModuleHistoryEntities", new[] { "Created" });
            DropIndex("dbo.ModuleHistoryEntities", new[] { "Name" });
            DropIndex("dbo.ModuleHistoryEntities", new[] { "ModuleStatus" });
            DropIndex("dbo.ModuleHistoryEntities", new[] { "ModuleId" });
            DropIndex("dbo.ModuleEntities", new[] { "Updated" });
            DropIndex("dbo.ModuleEntities", new[] { "Created" });
            DropIndex("dbo.ModuleEntities", new[] { "Name" });
            DropIndex("dbo.ModuleEntities", new[] { "ModuleStatus" });
            DropIndex("dbo.ModuleEntities", new[] { "PickupListId" });
            DropIndex("dbo.ModuleEntities", new[] { "FieldId" });
            DropIndex("dbo.FieldEntities", new[] { "Name" });
            DropIndex("dbo.FieldEntities", new[] { "FarmId" });
            DropIndex("dbo.FarmEntities", new[] { "Name" });
            DropIndex("dbo.FarmEntities", new[] { "ClientId" });
            DropIndex("dbo.ClientEntities", new[] { "Name" });
            DropTable("dbo.PickupListDownloadedToTrucks");
            DropTable("dbo.PickupListAssignedTrucks");
            DropTable("dbo.TruckListsDownloadeds");
            DropTable("dbo.SyncedSettings");
            DropTable("dbo.Settings");
            DropTable("dbo.DriverEntities");
            DropTable("dbo.DocumentToProcesses");
            DropTable("dbo.TruckEntities");
            DropTable("dbo.PickupListEntities");
            DropTable("dbo.ModuleHistoryEntities");
            DropTable("dbo.ModuleEntities");
            DropTable("dbo.FieldEntities");
            DropTable("dbo.FarmEntities");
            DropTable("dbo.ClientEntities");
            DropTable("dbo.AggregateEvents");
        }
    }
}
