namespace CottonDBMS.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBalesAndAddBaleScan : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.BaleEntities", "IX_PBINumber");
            AddColumn("dbo.BaleEntities", "ModuleId", c => c.String(maxLength: 128));
            AddColumn("dbo.BaleEntities", "ModuleSerialNumber", c => c.String(maxLength: 50));
            AddColumn("dbo.BaleEntities", "GinTicketLoadNumber", c => c.String(maxLength: 50));
            AddColumn("dbo.BaleEntities", "WeightFromScale", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.BaleEntities", "GinLoadId", c => c.String(maxLength: 128));
            AddColumn("dbo.BaleEntities", "Classing_NetWeight", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.BaleEntities", "Classing_Pk", c => c.Int());
            AddColumn("dbo.BaleEntities", "Classing_Gr", c => c.Int());
            AddColumn("dbo.BaleEntities", "Classing_Lf", c => c.Int());
            AddColumn("dbo.BaleEntities", "Classing_St", c => c.Int());
            AddColumn("dbo.BaleEntities", "Classing_Mic", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.BaleEntities", "Classing_Ex", c => c.Int());
            AddColumn("dbo.BaleEntities", "Classing_Rm", c => c.Int());
            AddColumn("dbo.BaleEntities", "Classing_Str", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.BaleEntities", "Classing_CGr", c => c.String());
            AddColumn("dbo.BaleEntities", "Classing_Rd", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.BaleEntities", "Classing_Plusb", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.BaleEntities", "Classing_Tr", c => c.Int());
            AddColumn("dbo.BaleEntities", "Classing_Unif", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.BaleEntities", "Classing_Len", c => c.Int());
            AddColumn("dbo.BaleEntities", "Classing_Value", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.BaleEntities", "Classing_TareWeight", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.BaleEntities", "Classing_EstimatedSeedWeight", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.BaleEntities", "PbiNumber", c => c.Int(nullable: false));
            CreateIndex("dbo.BaleEntities", "PbiNumber", unique: true, name: "IX_PBINumber");
            CreateIndex("dbo.BaleEntities", "ModuleId");
            CreateIndex("dbo.BaleEntities", "ModuleSerialNumber");
            CreateIndex("dbo.BaleEntities", "GinTicketLoadNumber");
            CreateIndex("dbo.BaleEntities", "GinLoadId");
            CreateIndex("dbo.BaleEntities", "ScanNumber");
            AddForeignKey("dbo.BaleEntities", "GinLoadId", "dbo.GinLoadEntities", "Id");
            AddForeignKey("dbo.BaleEntities", "ModuleId", "dbo.ModuleEntities", "Id");
            DropColumn("dbo.BaleEntities", "GrossWeight");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BaleEntities", "GrossWeight", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropForeignKey("dbo.BaleEntities", "ModuleId", "dbo.ModuleEntities");
            DropForeignKey("dbo.BaleEntities", "GinLoadId", "dbo.GinLoadEntities");
            DropIndex("dbo.BaleEntities", new[] { "ScanNumber" });
            DropIndex("dbo.BaleEntities", new[] { "GinLoadId" });
            DropIndex("dbo.BaleEntities", new[] { "GinTicketLoadNumber" });
            DropIndex("dbo.BaleEntities", new[] { "ModuleSerialNumber" });
            DropIndex("dbo.BaleEntities", new[] { "ModuleId" });
            DropIndex("dbo.BaleEntities", "IX_PBINumber");
            AlterColumn("dbo.BaleEntities", "PbiNumber", c => c.String(maxLength: 50));
            DropColumn("dbo.BaleEntities", "Classing_EstimatedSeedWeight");
            DropColumn("dbo.BaleEntities", "Classing_TareWeight");
            DropColumn("dbo.BaleEntities", "Classing_Value");
            DropColumn("dbo.BaleEntities", "Classing_Len");
            DropColumn("dbo.BaleEntities", "Classing_Unif");
            DropColumn("dbo.BaleEntities", "Classing_Tr");
            DropColumn("dbo.BaleEntities", "Classing_Plusb");
            DropColumn("dbo.BaleEntities", "Classing_Rd");
            DropColumn("dbo.BaleEntities", "Classing_CGr");
            DropColumn("dbo.BaleEntities", "Classing_Str");
            DropColumn("dbo.BaleEntities", "Classing_Rm");
            DropColumn("dbo.BaleEntities", "Classing_Ex");
            DropColumn("dbo.BaleEntities", "Classing_Mic");
            DropColumn("dbo.BaleEntities", "Classing_St");
            DropColumn("dbo.BaleEntities", "Classing_Lf");
            DropColumn("dbo.BaleEntities", "Classing_Gr");
            DropColumn("dbo.BaleEntities", "Classing_Pk");
            DropColumn("dbo.BaleEntities", "Classing_NetWeight");
            DropColumn("dbo.BaleEntities", "GinLoadId");
            DropColumn("dbo.BaleEntities", "WeightFromScale");
            DropColumn("dbo.BaleEntities", "GinTicketLoadNumber");
            DropColumn("dbo.BaleEntities", "ModuleSerialNumber");
            DropColumn("dbo.BaleEntities", "ModuleId");
            CreateIndex("dbo.BaleEntities", "PbiNumber", unique: true, name: "IX_PBINumber");
        }
    }
}
