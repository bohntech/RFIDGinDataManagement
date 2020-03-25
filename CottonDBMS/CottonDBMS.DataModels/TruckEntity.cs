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

        /*2019 Model Updates */
        [JsonProperty(PropertyName = "tareWeight")]
        public decimal TareWeight { get; set; }

        [JsonProperty(PropertyName = "licensePlate")]
        public string LicensePlate { get; set; }

        [JsonProperty(PropertyName = "rfidTagId")]
        public string RFIDTagId { get; set; }

        [JsonProperty(PropertyName = "ownerName")]
        public string OwnerName { get; set; }

        [JsonProperty(PropertyName = "ownerPhone")]
        public string OwnerPhone { get; set; }

        [JsonProperty(PropertyName = "customHauler")]
        public bool CustomHauler { get; set; }

        [JsonProperty(PropertyName = "isSemi")]
        public bool IsSemi { get; set; }
        /*********************/


        public TruckEntity() : base()
        {
            EntityType = EntityType.TRUCK;
        }        
    }
}
