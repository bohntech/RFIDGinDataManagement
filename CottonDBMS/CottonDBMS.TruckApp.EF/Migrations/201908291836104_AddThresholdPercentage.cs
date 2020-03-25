namespace CottonDBMS.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddThresholdPercentage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BaleEntities", "LintTurnout", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.BaleEntities", "OverrageThreshold", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BaleEntities", "OverrageThreshold");
            DropColumn("dbo.BaleEntities", "LintTurnout");
        }
    }
}
