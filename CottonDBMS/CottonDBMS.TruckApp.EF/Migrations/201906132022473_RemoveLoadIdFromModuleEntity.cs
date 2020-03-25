namespace CottonDBMS.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveLoadIdFromModuleEntity : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ModuleEntities", "LoadId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ModuleEntities", "LoadId", c => c.String());
        }
    }
}
