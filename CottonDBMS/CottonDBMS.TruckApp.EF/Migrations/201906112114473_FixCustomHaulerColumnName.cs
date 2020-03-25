namespace CottonDBMS.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixCustomHaulerColumnName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TruckEntities", "CustomHauler", c => c.Boolean(nullable: false));
            DropColumn("dbo.TruckEntities", "CustomerHauler");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TruckEntities", "CustomerHauler", c => c.Boolean(nullable: false));
            DropColumn("dbo.TruckEntities", "CustomHauler");
        }
    }
}
