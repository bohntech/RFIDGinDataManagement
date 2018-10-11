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
    public class PickupListEntity : BaseEntity
    {

        [JsonProperty(PropertyName = "fieldid")]
        public string FieldId { get; set; }

        [JsonIgnore]
        public FieldEntity Field {get; set;}

        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; set; }

        [JsonProperty(PropertyName = "assignedtrucks")]
        public List<TruckEntity> AssignedTrucks { get; set; }

        [JsonProperty(PropertyName = "downloadedbytrucks")]
        public List<TruckEntity> DownloadedToTrucks { get; set; }

        [JsonIgnore]
        public List<ModuleEntity> AssignedModules { get; set; }
                
        public int OriginalModuleCount { get; set; }

        public string OriginalSerialNumbers { get; set; }

        [JsonProperty(PropertyName = "pickupliststatus")]
        public PickupListStatus PickupListStatus { get; set; }

        [JsonProperty(PropertyName = "destination")]
        public PickupListDestination Destination { get; set; }

        [JsonIgnore]
        public int TotalModules { get { return AssignedModules.Count(); } }

        [JsonIgnore]
        public int SearchSetTotalModules { get; set; }

        [JsonIgnore]
        public int ModulesPerLoad { get; set; }
        
        [JsonIgnore]
        public int TotalLoads
        {
            get
            {
                if (ModulesPerLoad == 0) ModulesPerLoad = 4;

                int loads = (OriginalModuleCount / ModulesPerLoad);
                if (OriginalModuleCount % ModulesPerLoad > 0)
                {
                    loads++;
                }

                return loads;
            }
        }

        [JsonIgnore]
        public int LoadsCompleted
        {
            get
            {
                if (ModulesPerLoad == 0) ModulesPerLoad = 4;

                int modulesCompleted = this.TotalModules - this.ModulesRemaining;
                int loadsCompleted = modulesCompleted / ModulesPerLoad;

                if (modulesCompleted % ModulesPerLoad > 0)
                {
                    loadsCompleted++;
                }

                return loadsCompleted;
            }
        }

        [JsonIgnore]
        public int ModulesRemaining
        {
            get
            {
                int count = 0;

                if (Destination == PickupListDestination.GIN_YARD)
                    count = AssignedModules.Count(m => m.ModuleStatus == ModuleStatus.IN_FIELD);
                else if (Destination == PickupListDestination.GIN_FEEDER)
                    count = AssignedModules.Count(m => m.ModuleStatus == ModuleStatus.AT_GIN);

                return count;
            }
        }

        [JsonIgnore]
        public int SearchSetModulesRemaining { get; set; }

        [JsonIgnore]
        public int LoadsRemaining
        {
            get
            {
                if (ModulesPerLoad == 0) ModulesPerLoad = 4;

                int remaining = 0;
                if (AssignedModules != null && AssignedModules.Count() > 0)
                {
                    remaining = ModulesRemaining;
                }
                else 
                {
                    remaining = SearchSetModulesRemaining;
                }
                int loads = (remaining / ModulesPerLoad);
                if (remaining % ModulesPerLoad > 0)
                {
                    loads++;
                }
                return loads;
            }
        }

        [JsonIgnore]
        public string DestinationName
        {
            get
            {
                switch (Destination)
                {
                    case PickupListDestination.GIN_FEEDER: return "Gin Feeder";                        
                    default: return "Gin Yard";
                }
            }
        }

        [JsonProperty(PropertyName = "clientname")]
        public string ClientName
        {
            get
            {
                if (Field != null && Field.Farm != null && Field.Farm.Client != null)
                    return Field.Farm.Client.Name;
                else
                    return "";
            }
        }

        [JsonProperty(PropertyName = "farmname")]
        public string FarmName
        {
            get
            {
                if (Field != null && Field.Farm != null)
                    return Field.Farm.Name;
                else
                    return "";
            }
        }

        [JsonProperty(PropertyName = "fieldname")]
        public string FieldName
        {
            get
            {
                if (Field != null)
                    return Field.Name;
                else
                    return "";
            }
        }

        [JsonIgnore()]
        public string StatusName
        {
            get
            {
                switch (PickupListStatus)
                {
                    case PickupListStatus.OPEN:
                        return "OPEN";
                    default:
                        return "COMPLETE";
                }
              
            }
        }

        [JsonIgnore()]
        public string AssignedTruckNames
        {
            get
            {
                string s = "";
                foreach (var t in AssignedTrucks)
                {
                    s += t + ",";
                }
                return s.TrimEnd(',');
            }        
        }

        [JsonProperty(PropertyName = "assignedtruckids")]
        public string AssignedTruckIDs
        {
            get
            {
                string s = "";
                foreach (var t in AssignedTrucks)
                {
                    s += t.Id + ",";
                }
                return s.TrimEnd(',');
            }
        }

        [JsonIgnore()]
        public string DownloadedByTruckNames
        {
            get
            {
                string s = "";
                foreach(var t in DownloadedToTrucks)
                {
                    s += t + ",";
                }
                return s.TrimEnd(',');
            }        
        }

        [JsonIgnore()]
        public string LocaleCreatedTimestamp
        {
            get
            {
                DateTime local = Created.ToLocalTime();
                return local.ToShortDateString() + " " + local.ToLongTimeString(); ;
            }
        }

        [JsonIgnore()]
        public string LocaleUpdatedTimestamp
        {
            get
            {
                if (Updated.HasValue)
                {
                    DateTime local = Updated.Value.ToLocalTime();
                    return local.ToShortDateString() + " " + local.ToLongTimeString();
                }
                else
                    return "";
            }
        }

        public PickupListEntity() : base()
        {
            EntityType = EntityType.PICKUPLIST;
            PickupListStatus = PickupListStatus.OPEN;
            AssignedModules = new List<ModuleEntity>();
            AssignedTrucks = new List<TruckEntity>();
            DownloadedToTrucks = new List<TruckEntity>();

            SearchSetModulesRemaining = 0;
            SearchSetTotalModules = 0;
        }
    }


}
