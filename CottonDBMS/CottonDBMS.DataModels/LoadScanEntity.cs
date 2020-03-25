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
    public class LoadModuleScan
    {
        public string EPC { get; set; }
        public string SerialNumber { get; set; }
        public DateTime? ScanTime { get; set; }

        public override string ToString()
        {
            return SerialNumber;
        }
    }

    public class ModuleScanData
    {
        public List<LoadModuleScan> Scans { get; set; }
    }

    public class LoadScanEntity : BaseEntity
    {
        [JsonProperty(PropertyName = "bridgeLoadNumber")]
        public int BridgeLoadNumber
        {
            get; set;
        }

        [JsonProperty(PropertyName = "ginTagLoadNumber")]
        public string GinTagLoadNumber
        {
            get; set;
        }

        [JsonProperty(PropertyName = "grossWeight")]
        public decimal GrossWeight
        {
            get; set;
        }

        [JsonProperty(PropertyName = "netWeight")]
        public decimal NetWeight { get; set; }

        [JsonProperty(PropertyName = "splitWeight1")]
        public decimal SplitWeight1 { get; set; }

        [JsonProperty(PropertyName = "splitWeight2")]
        public decimal SplitWeight2 { get; set; }

        [JsonProperty(PropertyName = "yardRow")]
        public string YardRow
        {
            get; set;
        }

        [JsonProperty(PropertyName = "pickedBy")]
        public string PickedBy
        {
            get; set;
        }

        [JsonProperty(PropertyName = "variety")]
        public string Variety
        {
            get; set;
        }

        [JsonProperty(PropertyName = "trailerNumber")]
        public string TrailerNumber
        {
            get; set;
        }

        [JsonProperty(PropertyName = "submittedBy")]
        public string SubmittedBy { get; set; }

        [JsonProperty(PropertyName = "client")]
        public string Client
        {
            get; set;
        }

        [JsonProperty(PropertyName = "farm")]
        public string Farm
        {
            get; set;
        }

        [JsonProperty(PropertyName = "field")]
        public string Field
        {
            get; set;
        }

        [JsonProperty(PropertyName = "processed")]
        public bool Processed
        {
            get; set;
        }

        [JsonProperty(PropertyName = "serializedModuleScanData")]
        public string SerializedModuleScanData
        {
            get; set;
        }

        [JsonProperty(PropertyName = "bridgeId")]
        public string BridgeID
        {
            get; set;
        }

        [JsonProperty(PropertyName = "truckID")]
        public string TruckID
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

        [JsonIgnore]
        public ModuleScanData ScanData
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(SerializedModuleScanData))
                {
                    return JsonConvert.DeserializeObject<ModuleScanData>(SerializedModuleScanData);
                }
                else
                {
                    ModuleScanData data = new ModuleScanData();
                    data.Scans = new List<LoadModuleScan>();
                    return data;
                }
            }            
        }
        
        public void SetSerializedModuleScanData(ModuleScanData scanData)
        {
            SerializedModuleScanData = JsonConvert.SerializeObject(scanData);
        }        

        public void CopyTo(LoadScanEntity target)
        {
            target.Id = Id;
            target.BridgeLoadNumber = BridgeLoadNumber;
            target.GinTagLoadNumber = GinTagLoadNumber;
            target.GrossWeight = GrossWeight;
            target.NetWeight = NetWeight;
            target.SplitWeight1 = SplitWeight1;
            target.SplitWeight2 = SplitWeight2;
            target.SubmittedBy = SubmittedBy;
            target.YardRow = YardRow;
            target.TrailerNumber = TrailerNumber;
            target.PickedBy = PickedBy;
            target.Variety = Variety;
            target.Client = Client;
            target.Farm = Farm;
            target.Field = Field;
            target.Processed = Processed;
            target.Created = Created;
            target.EntityType = EntityType;
            target.Name = Name;            
            target.SelfLink = SelfLink;
            target.Source = Source;
            target.SyncedToCloud = SyncedToCloud;
            target.SerializedModuleScanData = SerializedModuleScanData;
            target.Updated = Updated;
            target.BridgeID = BridgeID;
            target.TruckID = TruckID;

            target.TargetStatus = TargetStatus;
            target.Latitude = Latitude;
            target.Longitude = Longitude;
        }

        public LoadScanEntity() : base()
        {
            EntityType = EntityType.LOAD_SCAN;
            SyncedToCloud = false;
            Source = InputSource.TRUCK;
        }
    }
}
