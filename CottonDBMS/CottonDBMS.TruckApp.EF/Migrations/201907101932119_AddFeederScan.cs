namespace CottonDBMS.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFeederScan : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FeederScanEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Processed = c.Boolean(nullable: false),
                        BridgeID = c.String(),
                        EPC = c.String(),
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FeederScanEntities");
        }
    }
}
