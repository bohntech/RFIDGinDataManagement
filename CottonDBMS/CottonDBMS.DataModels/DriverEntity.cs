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
    public class DriverEntity : BaseEntity
    {
        [JsonProperty(PropertyName = "firstname")]
        public string Firstname { get; set; }

        [JsonProperty(PropertyName = "lastname")]
        public string Lastname { get; set; }

        public DriverEntity() : base()
        {
            EntityType = EntityType.DRIVER;
        }       
    }
}
