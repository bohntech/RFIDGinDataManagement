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
    public class TruckRegistrationEntity : BaseEntity
    {
        [JsonProperty(PropertyName = "licensePlate")]
        public string LicensePlate
        {
            get; set;
        }
              

        [JsonProperty(PropertyName = "ownerPhone")]
        public string OwnerPhone
        {
            get; set;
        }

        [JsonProperty(PropertyName = "owner")]
        public string Owner
        {
            get; set;
        }

        [JsonProperty(PropertyName = "weight")]
        public decimal? Weight
        {
            get; set;
        }


        [JsonProperty(PropertyName = "processed")]
        public bool Processed
        {
            get; set;
        }

        public void CopyTo(TruckRegistrationEntity target)
        {
            target.Id = Id;
            target.LicensePlate = LicensePlate;            
            target.Owner = Owner;
            target.Weight = Weight;
            target.OwnerPhone = OwnerPhone;
            target.Processed = Processed;
            target.Created = Created;            
            target.EntityType = EntityType;            
            target.Name = Name;
            target.Processed = Processed;
            target.SelfLink = SelfLink;            
            target.Source = Source;
            target.SyncedToCloud = SyncedToCloud;                       
            target.Updated = Updated;            
        }


        public TruckRegistrationEntity() : base()
        {
            EntityType = EntityType.TRUCK_REGISTRATION;
            SyncedToCloud = false;
            Source = InputSource.TRUCK;
        }
    }
}
