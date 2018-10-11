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
    public class ModuleSummary 
    {
        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }

        [JsonProperty(PropertyName = "infield")]
        public int InField
        {
            get; set;
        }

        [JsonProperty(PropertyName = "pickedup")]
        public int PickedUp
        {
            get; set;
        }

        [JsonProperty(PropertyName = "atgin")]
        public int AtGin
        {
            get; set;
        }

        [JsonProperty(PropertyName = "ginned")]
        public int Ginned
        {
            get; set;
        }

        public ModuleSummary()
        {           
            Total = 0;
            InField = 0;
            PickedUp = 0;
            AtGin = 0;
            Ginned = 0;
        }
    }
}
