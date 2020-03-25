namespace CottonDBMS.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHIDFieldsToModuleOperatorAndProducerID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ModuleEntities", "HIDFieldArea", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("dbo.ModuleEntities", "HIDIncrementalArea", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("dbo.ModuleEntities", "HIDDiameter", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.ModuleEntities", "HIDOperator", c => c.String());
            AddColumn("dbo.ModuleEntities", "HIDProducerID", c => c.String());
            AlterColumn("dbo.ModuleEntities", "HIDModuleWeight", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.ModuleEntities", "HIDMoisture", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.ModuleEntities", "HIDSeasonTotal", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ModuleEntities", "HIDSeasonTotal", c => c.Int(nullable: false));
            AlterColumn("dbo.ModuleEntities", "HIDMoisture", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ModuleEntities", "HIDModuleWeight", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.ModuleEntities", "HIDProducerID");
            DropColumn("dbo.ModuleEntities", "HIDOperator");
            DropColumn("dbo.ModuleEntities", "HIDDiameter");
            DropColumn("dbo.ModuleEntities", "HIDIncrementalArea");
            DropColumn("dbo.ModuleEntities", "HIDFieldArea");
        }
    }
}
