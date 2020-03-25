namespace CottonDBMS.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDecimalColumnsOnLoadScan : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LoadScanEntities", "GrossWeight", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LoadScanEntities", "NetWeight", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LoadScanEntities", "SplitWeight1", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.LoadScanEntities", "SplitWeight2", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LoadScanEntities", "SplitWeight2", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LoadScanEntities", "SplitWeight1", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LoadScanEntities", "NetWeight", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.LoadScanEntities", "GrossWeight", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
