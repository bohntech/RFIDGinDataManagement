//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CottonDBMS.DataModels
{
    public class ClientEntity : BaseEntity
    {
        [JsonIgnore]
        public ICollection<FarmEntity> Farms { get; set; }

        public ClientEntity() : base()
        {
            EntityType = EntityType.CLIENT;
            Farms = new List<FarmEntity>();
        }

        public override string ToString()
        {
            return this.Name;
        }        
    }
}
