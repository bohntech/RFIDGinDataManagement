namespace CottonDBMS.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsSemiColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TruckEntities", "IsSemi", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TruckEntities", "IsSemi");
        }
    }
}
