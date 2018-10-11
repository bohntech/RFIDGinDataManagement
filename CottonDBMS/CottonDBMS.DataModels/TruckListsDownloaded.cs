//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CottonDBMS.DataModels
{
    public class TruckListsDownloaded : BaseEntity
    {
        [JsonProperty(PropertyName = "pickuplistsdownloaded")]
        public List<string> PickupListsDownloaded { get; set; }
        
        public TruckListsDownloaded()
        {
            EntityType = EntityType.TRUCK_LISTS_DOWNLOADED;         
        }
    }
        
    public class AggregateEvent : BaseEntity
    {
        [JsonProperty(PropertyName = "timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty(PropertyName = "eventtype")]        
        public EventType EventType { get; set; }

        [JsonProperty(PropertyName = "serialnumber")]
        public string SerialNumber { get; set; }

        [JsonProperty(PropertyName = "epc")]
        public string Epc { get; set; }

        [JsonProperty(PropertyName = "firstlat")]
        public double FirstLat { get; set; }

        [JsonProperty(PropertyName = "firstlong")]
        public double FirstLong { get; set; }

        [JsonProperty(PropertyName = "lastlat")]
        public double LastLat { get; set; }

        [JsonProperty(PropertyName = "lastlong")]
        public double LastLong { get; set; }

        [JsonProperty(PropertyName = "averagelat")]
        public double AverageLat { get; set; }

        [JsonProperty(PropertyName = "averagelong")]
        public double AverageLong { get; set; }

        [JsonProperty(PropertyName = "medianlat")]
        public double MedianLat { get; set; }

        [JsonProperty(PropertyName = "medianlong")]
        public double MedianLong { get; set; }

        [JsonProperty(PropertyName = "truckid")]
        public string TruckID { get; set; }

        [JsonProperty(PropertyName = "driverid")]
        public string DriverID { get; set; }

        [JsonProperty(PropertyName = "loadnumber")]
        public string LoadNumber { get; set; }

        [JsonProperty(PropertyName = "clientid")]
        public string ClientId { get; set; }

        [JsonProperty(PropertyName = "clientname")]
        public string ClientName { get; set; }

        [JsonProperty(PropertyName = "farmid")]
        public string FarmId { get; set; }

        [JsonProperty(PropertyName = "farmname")]
        public string FarmName { get; set; }

        [JsonProperty(PropertyName = "fieldid")]
        public string FieldId { get; set; }

        [JsonProperty(PropertyName = "moduleid")]
        public string ModuleId { get; set; }

        [JsonProperty(PropertyName = "fieldname")]
        public string FieldName { get; set; }

        [JsonProperty(PropertyName = "pickuplistid")]
        public string PickupListId { get; set; }

        [JsonProperty(PropertyName = "pickuplistname")]
        public string PickupListName { get; set; }

        [JsonIgnore]
        public bool Processed { get; set; }

        public AggregateEvent()
        {
            EntityType = EntityType.AGGREGATE_EVENT;
            Id = Guid.NewGuid().ToString();
            Name = Id;
            Processed = false;
        }

        public void CopyTo(AggregateEvent target)
        {
            target.Id = Id;
            target.AverageLat = AverageLat;
            target.AverageLong = AverageLong;
            target.Created = Created;
            target.DriverID = DriverID;
            target.EntityType = EntityType;
            target.Epc = Epc;
            target.EventType = EventType;
            target.FirstLat = FirstLat;
            target.LastLat = LastLat;
            target.LastLong = LastLong;
            target.LoadNumber = LoadNumber;
            target.MedianLat = MedianLat;
            target.MedianLong = MedianLong;
            target.Name = Name;
            target.Processed = Processed;
            target.SelfLink = SelfLink;
            target.SerialNumber = SerialNumber;
            target.Source = Source;
            target.SyncedToCloud = SyncedToCloud;
            target.Timestamp = Timestamp;
            target.TruckID = TruckID;
            target.Updated = Updated;
            target.PickupListId = PickupListId;
            target.PickupListName = PickupListName;
            target.FieldName = FieldName;
            target.ClientName = ClientName;
            target.FarmName = FarmName;
            target.FieldId = FieldId;
            target.FarmId = FarmId;
            target.ClientId = ClientId;            
        }
    }
}
