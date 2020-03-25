//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.Interfaces;
using CottonDBMS.EF;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;

namespace CottonDBMS.Bridges.Shared
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork CreateUnitOfWork();
    }

    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {       
        public IUnitOfWork CreateUnitOfWork()
        {
            return (IUnitOfWork)new UnitOfWork();
        }
    }
}
