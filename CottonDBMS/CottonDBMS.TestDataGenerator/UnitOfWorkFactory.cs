using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.Interfaces;
using CottonDBMS.EF;

namespace CottonDBMS.TestDataGenerator
{
    public class UnitOfWorkFactory
    {
        public static IUnitOfWork CreateUnitOfWork()
        {
            return (IUnitOfWork)new UnitOfWork();
        }
    }
}
