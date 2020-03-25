namespace CottonDBMS.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyModuleHistoryColumnsTypesToAllowNulls : DbMigration
    {
        public override void Up()
        {

            DropConstraint("ModuleHistoryEntities", "BridgeLoadNumber");
            DropConstraint("ModuleHistoryEntities", "GinTagLoadNumber");
            AlterColumn("dbo.ModuleHistoryEntities", "BridgeLoadNumber", c => c.Int());
            AlterColumn("dbo.ModuleHistoryEntities", "GinTagLoadNumber", c => c.String());

            Sql("Update dbo.ModuleHistoryEntities SET GinTagLoadNumber = '' WHERE GinTagLoadNumber = '0'");
            Sql("Update dbo.ModuleHistoryEntities SET BridgeLoadNumber = null WHERE BridgeLoadNumber = 0");
        }

        private void DropConstraint(string tableName, string columnName)
        {
            Sql(@"DECLARE @var0 nvarchar(128)
          SELECT @var0 = name
          FROM sys.default_constraints
          WHERE parent_object_id = object_id(N'dbo.MyTable')
          AND col_name(parent_object_id, parent_column_id) = 'ColumnName';
          IF @var0 IS NOT NULL
              EXECUTE('ALTER TABLE [dbo].[MyTable] DROP CONSTRAINT [' + @var0 + ']')".Replace("MyTable", tableName).Replace("ColumnName", columnName));
        }

        public override void Down()
        {
            AlterColumn("dbo.ModuleHistoryEntities", "GinTagLoadNumber", c => c.Int(nullable: false));
            AlterColumn("dbo.ModuleHistoryEntities", "BridgeLoadNumber", c => c.Int(nullable: false));
        }
    }
}
