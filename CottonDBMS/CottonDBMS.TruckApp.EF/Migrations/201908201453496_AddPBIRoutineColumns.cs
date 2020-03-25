namespace CottonDBMS.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPBIRoutineColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BaleEntities", "OutOfSequence", c => c.Boolean(nullable: false));
            AddColumn("dbo.BaleEntities", "AccumWeight", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.BaleEntities", "OverrageAdjustment", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.GinLoadEntities", "LintWeight", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.ModuleEntities", "NetSeedCottonWeight", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.ModuleEntities", "LintWeight", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ModuleEntities", "LintWeight");
            DropColumn("dbo.ModuleEntities", "NetSeedCottonWeight");
            DropColumn("dbo.GinLoadEntities", "LintWeight");
            DropColumn("dbo.BaleEntities", "OverrageAdjustment");
            DropColumn("dbo.BaleEntities", "AccumWeight");
            DropColumn("dbo.BaleEntities", "OutOfSequence");
        }
    }
}
