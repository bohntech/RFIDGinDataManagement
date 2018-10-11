//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using CottonDBMS.DataModels;
using CottonDBMS.EF;

namespace CottonDBMS.EF
{
    public class Initializer : CreateDatabaseIfNotExists<AppDBContext>
    {         

        protected override void Seed(AppDBContext context)
        {
            
        }
    }
}

