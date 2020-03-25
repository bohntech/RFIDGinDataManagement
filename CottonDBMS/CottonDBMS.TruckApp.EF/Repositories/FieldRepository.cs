//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.Interfaces;
using CottonDBMS.DataModels;
using CottonDBMS.Data.EF;

namespace CottonDBMS.EF.Repositories
{
    public class FieldRepository : EntityRepository<FieldEntity>, IFieldRepository
    {
        public FieldRepository(AppDBContext context) : base(context)
        {

        }

        public IEnumerable<FieldEntity> GetAllMatchingFields(string clientName, string farmName, string fieldName)
        {
            clientName = clientName.ToLower();
            farmName = farmName.ToLower();
            fieldName = fieldName.ToLower();


            var q = _context.Fields.Include("Farm.Client")
                    .Where(f => (clientName == "" || f.Farm.Client.Name == clientName) &&
                     (farmName == "" || f.Farm.Name == farmName) &&
                     (fieldName == "" || f.Name == fieldName)
                    ).OrderBy(f => f.Farm.Client.Name).ThenBy(f => f.Farm.Name).ThenBy(f => f.Name);


            return q.ToList();
        }

        public IEnumerable<string> GetUndeletableFieldIds(List<FieldEntity> fieldsToDelete)
        {
            var idsToDelete = fieldsToDelete.Select(f => f.Id).ToArray();
            var fieldsWithModules = _context.Fields.Include("Modules").Where(f => f.Modules.Any(m => idsToDelete.Contains(m.FieldId))).Select(f => f.Id).ToList();
            var fieldsLinkedToList = _context.PickupLists.Where(p => idsToDelete.Contains(p.FieldId)).Select(t => t.FieldId).ToList();
            var fieldsLinkedToGinLoad = _context.GinLoads.Where(p => idsToDelete.Contains(p.FieldId)).Select(t => t.FieldId).ToList();

            fieldsWithModules.AddRange(fieldsLinkedToList);
            fieldsWithModules.AddRange(fieldsLinkedToGinLoad);
            return fieldsWithModules.Distinct().ToList();
        }

        public bool CanSaveField(string clientId, string farmId, string fieldId, string fieldName, bool validateAllIds)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
            {
                return false;
            }

            if (validateAllIds && (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(farmId)))
            {
                return false;
            }

            return !_context.Fields.Include("Farm.Client")
                    .Any(f => f.Farm.Client.Id == clientId && f.Farm.Id == farmId && f.Name == fieldName && f.Id != fieldId);
        }

        public FieldEntity EnsureFieldCreated(FarmEntity existingFarm, string fieldName, InputSource source)
        {
            if (existingFarm == null) throw new Exception("Farm required to save field " + fieldName);

            fieldName = fieldName.Trim();                        
            
            FieldEntity existingField = _context.Fields.Include("Farm.Client").FirstOrDefault(f => f.FarmId == existingFarm.Id && f.Name == fieldName);
            FarmEntity dbFarm = _context.Farms.Include("Client").FirstOrDefault(f => f.Id == existingFarm.Id);

            if (existingField == null)
            {
                existingField = new FieldEntity();              
                existingField.Name = fieldName;
                existingField.FarmId = dbFarm.Id;
                existingField.Source = source;           

                var canSaveField = this.CanSaveField(dbFarm.ClientId, dbFarm.Id, existingField.Id, existingField.Name, true);

                if (!canSaveField)
                {
                    Logging.Logger.Log("INFO", string.Format("Cannot save field {0} for client/farm {1}/{2}", existingField.Name, existingFarm.Client.Name, existingFarm.Name));
                    return null;
                }
                else
                {
                    this.Save(existingField);
                    return existingField;
                }
            }
            else
            {
                return existingField;
            }
        }        

    }
}
