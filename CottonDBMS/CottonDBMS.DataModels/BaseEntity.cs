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
    public class BaseEntity
    {
        public BaseEntity()
        {
            Id = string.Empty;
            Created = DateTime.UtcNow;
            SyncedToCloud = false;
            Source = InputSource.GIN;
        }

        [JsonProperty(PropertyName = "inputsource")]
        public InputSource Source { get; set; }

        [JsonProperty(PropertyName = "_self")]
        public string SelfLink { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "entitytype")]
        public EntityType EntityType { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "created")]
        public DateTime Created { get; set; }

        [JsonIgnore]
        public bool SyncedToCloud { get; set; }

        [JsonProperty(PropertyName = "updated")]
        public DateTime? Updated { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}

