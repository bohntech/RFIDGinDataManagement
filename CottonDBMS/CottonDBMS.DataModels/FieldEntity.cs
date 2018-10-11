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
    public class FieldEntity : BaseEntity
    {
        [JsonProperty(PropertyName = "farmid")]
        public string FarmId { get; set; }

        [JsonIgnore]
        public FarmEntity Farm { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; set; }

        [JsonIgnore]
        public ClientEntity Client
        {
            get
            {
                if (Farm != null)                
                    return Farm.Client;                
                else return null;
            }
        }

        [JsonIgnore]
        public ICollection<ModuleEntity> Modules { get; set; }

        public FieldEntity() : base()
        {
            EntityType = EntityType.FIELD;
            Modules = new List<ModuleEntity>();
        }

        public override string ToString()
        {
            return this.Name;
        }        
    }    
}
