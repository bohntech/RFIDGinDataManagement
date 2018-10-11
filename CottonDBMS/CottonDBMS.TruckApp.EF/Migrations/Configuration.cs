//Licensed under MIT License see LICENSE.TXT in project root folder
namespace CottonDBMS.Data.EF.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<CottonDBMS.EF.AppDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(CottonDBMS.EF.AppDBContext context)
        {

        }
    }
}
