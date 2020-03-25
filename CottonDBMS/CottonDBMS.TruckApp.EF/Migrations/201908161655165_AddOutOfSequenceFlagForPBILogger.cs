namespace CottonDBMS.Data.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOutOfSequenceFlagForPBILogger : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BaleScanEntities", "OutOfSequence", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BaleScanEntities", "OutOfSequence");
        }
    }
}
