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
    public enum ModuleStatus { IN_FIELD = 1, PICKED_UP = 2, AT_GIN = 3, ON_FEEDER = 4, GINNED = 5 };
}
