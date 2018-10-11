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
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PickupListDestination { GIN_YARD = 10, GIN_FEEDER = 20 };
}
