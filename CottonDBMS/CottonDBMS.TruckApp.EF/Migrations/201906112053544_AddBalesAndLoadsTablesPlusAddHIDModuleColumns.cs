namespace CottonDBMS.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBalesAndLoadsTablesPlusAddHIDModuleColumns : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BaleEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        PbiNumber = c.String(maxLength: 50),
                        TareWeight = c.Decimal(nullable: false, precision: 18, scale: 5),
                        GrossWeight = c.Decimal(nullable: false, precision: 18, scale: 5),
                        NetWeight = c.Decimal(nullable: false, precision: 18, scale: 5),
                        ScanNumber = c.Int(nullable: false),
                        Source = c.Int(nullable: false),
                        SelfLink = c.String(),
                        EntityType = c.Int(nullable: false),
                        Name = c.String(maxLength: 50),
                        Created = c.DateTime(nullable: false),
                        SyncedToCloud = c.Boolean(nullable: false),
                        Updated = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.PbiNumber, unique: true, name: "IX_PBINumber")
                .Index(t => t.Name, unique: true)
                .Index(t => t.Created)
                .Index(t => t.Updated);
            
            CreateTable(
                "dbo.GinLoadEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        GinTagLoadNumber = c.String(maxLength: 50),
                        ScaleBridgeLoadNumber = c.Int(nullable: false),
                        ScaleBridgeId = c.String(),
                        GrossWeight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TruckID = c.String(),
                        YardLocation = c.String(),
                        PickedBy = c.String(),
                        Variety = c.String(),
                        TrailerNumber = c.String(),
                        ClientId = c.String(),
                        FarmId = c.String(),
                        FieldId = c.String(),
                        Source = c.Int(nullable: false),
                        SelfLink = c.String(),
                        EntityType = c.Int(nullable: false),
                        Name = c.String(),
                        Created = c.DateTime(nullable: false),
                        SyncedToCloud = c.Boolean(nullable: false),
                        Updated = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.GinTagLoadNumber, unique: true)
                .Index(t => t.ScaleBridgeLoadNumber, unique: true);
            
            AddColumn("dbo.ModuleEntities", "ClassingModuleId", c => c.String());
            AddColumn("dbo.ModuleEntities", "GinTagLoadNumber", c => c.String());
            AddColumn("dbo.ModuleEntities", "LoadId", c => c.String());
            AddColumn("dbo.ModuleEntities", "FirstBridgeId", c => c.String());
            AddColumn("dbo.ModuleEntities", "LastBridgeId", c => c.String());
            AddColumn("dbo.ModuleEntities", "HIDModuleWeight", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ModuleEntities", "HIDMoisture", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ModuleEntities", "HIDSeasonTotal", c => c.Int(nullable: false));
            AddColumn("dbo.ModuleEntities", "HIDVariety", c => c.String());
            AddColumn("dbo.ModuleEntities", "GinLoadId", c => c.String(maxLength: 128));
            AddColumn("dbo.ModuleHistoryEntities", "BridgeId", c => c.String());
            AddColumn("dbo.ModuleHistoryEntities", "BridgeLoadNumber", c => c.Int(nullable: false));
            AddColumn("dbo.ModuleHistoryEntities", "GinTagLoadNumber", c => c.Int(nullable: false));
            AddColumn("dbo.TruckEntities", "TareWeight", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.TruckEntities", "LicensePlate", c => c.String());
            AddColumn("dbo.TruckEntities", "RFIDTagId", c => c.String());
            AddColumn("dbo.TruckEntities", "OwnerName", c => c.String());
            AddColumn("dbo.TruckEntities", "OwnerPhone", c => c.String());
            AddColumn("dbo.TruckEntities", "CustomerHauler", c => c.Boolean(nullable: false));
            CreateIndex("dbo.ModuleEntities", "GinLoadId");
            AddForeignKey("dbo.ModuleEntities", "GinLoadId", "dbo.GinLoadEntities", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ModuleEntities", "GinLoadId", "dbo.GinLoadEntities");
            DropIndex("dbo.GinLoadEntities", new[] { "ScaleBridgeLoadNumber" });
            DropIndex("dbo.GinLoadEntities", new[] { "GinTagLoadNumber" });
            DropIndex("dbo.ModuleEntities", new[] { "GinLoadId" });
            DropIndex("dbo.BaleEntities", new[] { "Updated" });
            DropIndex("dbo.BaleEntities", new[] { "Created" });
            DropIndex("dbo.BaleEntities", new[] { "Name" });
            DropIndex("dbo.BaleEntities", "IX_PBINumber");
            DropColumn("dbo.TruckEntities", "CustomerHauler");
            DropColumn("dbo.TruckEntities", "OwnerPhone");
            DropColumn("dbo.TruckEntities", "OwnerName");
            DropColumn("dbo.TruckEntities", "RFIDTagId");
            DropColumn("dbo.TruckEntities", "LicensePlate");
            DropColumn("dbo.TruckEntities", "TareWeight");
            DropColumn("dbo.ModuleHistoryEntities", "GinTagLoadNumber");
            DropColumn("dbo.ModuleHistoryEntities", "BridgeLoadNumber");
            DropColumn("dbo.ModuleHistoryEntities", "BridgeId");
            DropColumn("dbo.ModuleEntities", "GinLoadId");
            DropColumn("dbo.ModuleEntities", "HIDVariety");
            DropColumn("dbo.ModuleEntities", "HIDSeasonTotal");
            DropColumn("dbo.ModuleEntities", "HIDMoisture");
            DropColumn("dbo.ModuleEntities", "HIDModuleWeight");
            DropColumn("dbo.ModuleEntities", "LastBridgeId");
            DropColumn("dbo.ModuleEntities", "FirstBridgeId");
            DropColumn("dbo.ModuleEntities", "LoadId");
            DropColumn("dbo.ModuleEntities", "GinTagLoadNumber");
            DropColumn("dbo.ModuleEntities", "ClassingModuleId");
            DropTable("dbo.GinLoadEntities");
            DropTable("dbo.BaleEntities");
        }
    }
}
