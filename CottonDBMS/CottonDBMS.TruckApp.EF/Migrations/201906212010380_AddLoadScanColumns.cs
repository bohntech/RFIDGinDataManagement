namespace CottonDBMS.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLoadScanColumns : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.GinLoadEntities", new[] { "ScaleBridgeLoadNumber" });
            CreateTable(
                "dbo.LoadScanEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        BridgeLoadNumber = c.Int(nullable: false),
                        GinTagLoadNumber = c.String(),
                        GrossWeight = c.Decimal(precision: 18, scale: 2),
                        YardRow = c.String(),
                        PickedBy = c.String(),
                        Variety = c.String(),
                        TrailerNumber = c.String(),
                        Client = c.String(),
                        Farm = c.String(),
                        Field = c.String(),
                        Processed = c.Boolean(nullable: false),
                        SerializedModuleScanData = c.String(),
                        BridgeID = c.String(),
                        TruckID = c.String(),
                        TargetStatus = c.Int(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
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
                "dbo.ModuleOwnershipEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        GinTagLoadNumber = c.String(),
                        BridgeLoadNumber = c.Int(nullable: false),
                        ImportedLoadNumber = c.String(),
                        TruckLoadNumber = c.String(),
                        Status = c.String(),
                        Client = c.String(),
                        Farm = c.String(),
                        Field = c.String(),
                        Deleted = c.Boolean(nullable: false),
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
                "dbo.TruckRegistrationEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        LicensePlate = c.String(),
                        OwnerPhone = c.String(),
                        Owner = c.String(),
                        Weight = c.Decimal(precision: 18, scale: 2),
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
            
            CreateIndex("dbo.GinLoadEntities", "ScaleBridgeLoadNumber");
        }
        
        public override void Down()
        {
            DropIndex("dbo.GinLoadEntities", new[] { "ScaleBridgeLoadNumber" });
            DropTable("dbo.TruckRegistrationEntities");
            DropTable("dbo.ModuleOwnershipEntities");
            DropTable("dbo.LoadScanEntities");
            CreateIndex("dbo.GinLoadEntities", "ScaleBridgeLoadNumber", unique: true);
        }
    }
}
