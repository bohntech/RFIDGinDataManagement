namespace CottonDBMS.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMoreHIDColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ModuleEntities", "HIDFieldTotal", c => c.Int());
            AddColumn("dbo.ModuleEntities", "HIDGinID", c => c.String());
            AddColumn("dbo.ModuleEntities", "HIDMachinePIN", c => c.String());
            AddColumn("dbo.ModuleEntities", "HIDDropLat", c => c.Double(nullable: false));
            AddColumn("dbo.ModuleEntities", "HIDDropLong", c => c.Double(nullable: false));
            AddColumn("dbo.ModuleEntities", "HIDWrapLat", c => c.Double(nullable: false));
            AddColumn("dbo.ModuleEntities", "HIDWrapLong", c => c.Double(nullable: false));
            AddColumn("dbo.ModuleEntities", "HIDLat", c => c.Double(nullable: false));
            AddColumn("dbo.ModuleEntities", "HIDLong", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ModuleEntities", "HIDLong");
            DropColumn("dbo.ModuleEntities", "HIDLat");
            DropColumn("dbo.ModuleEntities", "HIDWrapLong");
            DropColumn("dbo.ModuleEntities", "HIDWrapLat");
            DropColumn("dbo.ModuleEntities", "HIDDropLong");
            DropColumn("dbo.ModuleEntities", "HIDDropLat");
            DropColumn("dbo.ModuleEntities", "HIDMachinePIN");
            DropColumn("dbo.ModuleEntities", "HIDGinID");
            DropColumn("dbo.ModuleEntities", "HIDFieldTotal");
        }
    }
}
