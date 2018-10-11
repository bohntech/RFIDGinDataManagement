namespace CottonDBMS.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PickupListEntities", "AnotherColumn", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PickupListEntities", "AnotherColumn");
        }
    }
}
