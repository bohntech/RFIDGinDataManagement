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
    public class FarmEntity : BaseEntity
    {
        [JsonProperty(PropertyName = "clientid")]
        public string ClientId { get; set; }

        [JsonIgnore]
        public ClientEntity Client { get; set; }

        [JsonIgnore]
        public ICollection<FieldEntity> Fields { get; set; }

        public FarmEntity() : base()
        {
            EntityType = EntityType.FARM;
            Fields = new List<FieldEntity>();
        }

        public override string ToString()
        {
            return this.Name;
        }        
    }
}
