namespace CottonDBMS.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveField : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PickupListEntities", "AnotherColumn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PickupListEntities", "AnotherColumn", c => c.Int(nullable: false));
        }
    }
}
