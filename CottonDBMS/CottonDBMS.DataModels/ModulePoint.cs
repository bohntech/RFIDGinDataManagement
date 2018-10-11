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
    public class ModulePoint
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "serialnumber")]
        public string SerialNumber { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; set; }

        [JsonProperty(PropertyName = "modulestatus")]
        public ModuleStatus ModuleStatus { get; set; }

        [JsonProperty(PropertyName = "moduleevent")]
        public ModuleEventType ModuleEvent { get; set; }
    }
}
