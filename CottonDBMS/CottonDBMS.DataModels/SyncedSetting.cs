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
    public class SyncedSettings : BaseEntity
    {
        [JsonProperty(PropertyName = "modulesperload")]
        public int ModulesPerLoad { get; set; }

        [JsonProperty(PropertyName = "StartingLoadNumber")]
        public int StartingLoadNumber { get; set; }

        [JsonProperty(PropertyName = "loadprefix")]
        public string LoadPrefix { get; set; }

        [JsonProperty(PropertyName = "mapsapikey")]
        public string GoogleMapsKey { get; set; }

        [JsonProperty(PropertyName = "feederlatitude")]
        public double FeederLatitude { get; set; }

        [JsonProperty(PropertyName = "feederlongitude")]
        public double FeederLongitude { get; set; }

        [JsonProperty(PropertyName = "feederdetectionradius")]
        public double FeederDetectionRadius { get; set; }

        [JsonProperty(PropertyName = "ginyardnwlat")]
        public double GinYardNWLat { get; set; }

        [JsonProperty(PropertyName = "ginyardnwlong")]
        public double GinYardNWLong { get; set; }

        [JsonProperty(PropertyName = "ginyardselat")]
        public double GinYardSELat { get; set; }

        [JsonProperty(PropertyName = "ginyardselong")]
        public double GinYardSELong { get; set; }
    }
}
