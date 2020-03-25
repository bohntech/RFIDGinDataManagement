namespace CottonDBMS.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateGinLoadToDropClientandFarmColumn : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GinLoadEntities", "FieldId", c => c.String(maxLength: 128));
            CreateIndex("dbo.GinLoadEntities", "FieldId");
            AddForeignKey("dbo.GinLoadEntities", "FieldId", "dbo.FieldEntities", "Id");
            DropColumn("dbo.GinLoadEntities", "ClientId");
            DropColumn("dbo.GinLoadEntities", "FarmId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GinLoadEntities", "FarmId", c => c.String());
            AddColumn("dbo.GinLoadEntities", "ClientId", c => c.String());
            DropForeignKey("dbo.GinLoadEntities", "FieldId", "dbo.FieldEntities");
            DropIndex("dbo.GinLoadEntities", new[] { "FieldId" });
            AlterColumn("dbo.GinLoadEntities", "FieldId", c => c.String());
        }
    }
}
