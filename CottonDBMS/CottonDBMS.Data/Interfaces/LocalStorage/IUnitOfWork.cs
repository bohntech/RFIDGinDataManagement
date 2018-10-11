using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CottonDBMS.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
        ISettingsRepository GetSettingsRepository();
    }
}
