//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using CottonDBMS.DataModels.Helpers;

namespace CottonDBMS.DataModels
{   

    public class FeederScanEntity : BaseEntity
    {       

        [JsonProperty(PropertyName = "processed")]
        public bool Processed
        {
            get; set;
        }
        
        [JsonProperty(PropertyName = "bridgeId")]
        public string BridgeID
        {
            get; set;
        }

        [JsonProperty(PropertyName = "epc")]
        public string EPC
        {
            get; set;
        }

        [JsonProperty(PropertyName = "status")]
        public ModuleStatus TargetStatus
        {
            get; set;
        }

        [JsonProperty(PropertyName = "latitude")]
        public double Latitude
        {
            get; set;
        }

        [JsonProperty(PropertyName = "longitude")]
        public double Longitude
        {
            get; set;
        }        

        public void CopyTo(FeederScanEntity target)
        {
            target.Id = Id;          
            target.Processed = Processed;
            target.Created = Created;
            target.EntityType = EntityType;
            target.Name = Name;
            target.SelfLink = SelfLink;
            target.Source = Source;
            target.SyncedToCloud = SyncedToCloud;            
            target.Updated = Updated;
            target.BridgeID = BridgeID;
            target.TargetStatus = TargetStatus;
            target.Latitude = Latitude;
            target.Longitude = Longitude;
            target.EPC = EPC;
        }

        public FeederScanEntity() : base()
        {
            EntityType = EntityType.FEEDER_SCAN;
            SyncedToCloud = false;
            Source = InputSource.TRUCK;
        }
    }
}
