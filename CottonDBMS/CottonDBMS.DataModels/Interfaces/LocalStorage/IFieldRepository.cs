//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.DataModels;

namespace CottonDBMS.Interfaces
{
    public interface IFieldRepository : IEntityRepository<FieldEntity>
    {
        IEnumerable<FieldEntity> GetAllMatchingFields(string clientName, string farmName, string fieldName);
        IEnumerable<string> GetUndeletableFieldIds(List<FieldEntity> fieldsToDelete);
        bool CanSaveField(string clientId, string farmId, string fieldId, string fieldName, bool validateAllIds);
        FieldEntity EnsureFieldCreated(FarmEntity existingFarm, string fieldName, InputSource source);
    }
}
