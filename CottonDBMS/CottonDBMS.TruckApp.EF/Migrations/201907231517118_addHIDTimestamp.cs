namespace CottonDBMS.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addHIDTimestamp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ModuleEntities", "HIDGMTDate", c => c.String());
            AddColumn("dbo.ModuleEntities", "HIDGMTTime", c => c.String());
            AddColumn("dbo.ModuleEntities", "HIDTimestamp", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ModuleEntities", "HIDTimestamp");
            DropColumn("dbo.ModuleEntities", "HIDGMTTime");
            DropColumn("dbo.ModuleEntities", "HIDGMTDate");
        }
    }
}
