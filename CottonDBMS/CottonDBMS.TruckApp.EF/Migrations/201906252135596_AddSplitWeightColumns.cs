namespace CottonDBMS.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSplitWeightColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GinLoadEntities", "NetWeight", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.GinLoadEntities", "SplitWeight1", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.GinLoadEntities", "SplitWeight2", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.GinLoadEntities", "SubmittedBy", c => c.String());
            AddColumn("dbo.LoadScanEntities", "NetWeight", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.LoadScanEntities", "SplitWeight1", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.LoadScanEntities", "SplitWeight2", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.LoadScanEntities", "SubmittedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LoadScanEntities", "SubmittedBy");
            DropColumn("dbo.LoadScanEntities", "SplitWeight2");
            DropColumn("dbo.LoadScanEntities", "SplitWeight1");
            DropColumn("dbo.LoadScanEntities", "NetWeight");
            DropColumn("dbo.GinLoadEntities", "SubmittedBy");
            DropColumn("dbo.GinLoadEntities", "SplitWeight2");
            DropColumn("dbo.GinLoadEntities", "SplitWeight1");
            DropColumn("dbo.GinLoadEntities", "NetWeight");
        }
    }
}
