namespace CottonDBMS.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOriginalModuleCount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PickupListEntities", "OriginalModuleCount", c => c.Int(nullable: false));
            AddColumn("dbo.PickupListEntities", "OriginalSerialNumbers", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PickupListEntities", "OriginalSerialNumbers");
            DropColumn("dbo.PickupListEntities", "OriginalModuleCount");
        }
    }
}
