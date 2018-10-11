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
    public class TruckEntity : BaseEntity
    {
        [JsonProperty(PropertyName = "loadprefix")]
        public string LoadPrefix
        {
            get; set;
        }

        [JsonIgnore]
        public virtual ICollection<PickupListEntity> AssignedToLists { get; set; }

        [JsonIgnore]
        public virtual ICollection<PickupListEntity> DownloadedLists { get; set; }


        public TruckEntity() : base()
        {
            EntityType = EntityType.TRUCK;
        }        
    }
}
