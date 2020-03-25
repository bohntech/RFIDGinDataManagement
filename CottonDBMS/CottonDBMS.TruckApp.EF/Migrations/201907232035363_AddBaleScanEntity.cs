namespace CottonDBMS.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBaleScanEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BaleScanEntities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        PbiNumber = c.Int(nullable: false),
                        ScanNumber = c.Int(nullable: false),
                        ScaleWeight = c.Decimal(nullable: false, precision: 18, scale: 2),
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BaleScanEntities");
        }
    }
}
