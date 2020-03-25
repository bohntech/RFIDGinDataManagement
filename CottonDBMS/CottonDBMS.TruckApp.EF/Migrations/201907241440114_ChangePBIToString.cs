namespace CottonDBMS.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePBIToString : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.BaleEntities", "IX_PBINumber");
            AlterColumn("dbo.BaleEntities", "PbiNumber", c => c.String(maxLength: 50));
            AlterColumn("dbo.BaleScanEntities", "PbiNumber", c => c.String());
            CreateIndex("dbo.BaleEntities", "PbiNumber", unique: true, name: "IX_PBINumber");
        }
        
        public override void Down()
        {
            DropIndex("dbo.BaleEntities", "IX_PBINumber");
            AlterColumn("dbo.BaleScanEntities", "PbiNumber", c => c.Int(nullable: false));
            AlterColumn("dbo.BaleEntities", "PbiNumber", c => c.Int(nullable: false));
            CreateIndex("dbo.BaleEntities", "PbiNumber", unique: true, name: "IX_PBINumber");
        }
    }
}
