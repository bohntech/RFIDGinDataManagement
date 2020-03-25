namespace CottonDBMS.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddModuleOwnershipFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ModuleOwnershipEntities", "TruckID", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ModuleOwnershipEntities", "TruckID");
        }
    }
}
