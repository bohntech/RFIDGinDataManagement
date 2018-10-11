﻿//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CottonDBMS.DataModels
{
    public class ModuleHistoryEntity : BaseEntity
    {
        [JsonProperty(PropertyName = "moduleid")]
        public string ModuleId { get; set; }

        [JsonIgnore()]
        public ModuleEntity Module
        {
            get; set;
        }

        [JsonProperty(PropertyName = "driver")]
        public string Driver { get; set; }

        [JsonProperty(PropertyName = "truckid")]
        public string TruckID { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; set; }

        [JsonProperty(PropertyName = "moduleevent")]
        public ModuleEventType ModuleEventType { get; set; }

        [JsonProperty(PropertyName = "status")]
        public ModuleStatus ModuleStatus { get; set; }

        [JsonIgnore()]
        public string StatusName
        {
            get
            {
                string status = "";
                switch (ModuleStatus)
                {
                    case ModuleStatus.AT_GIN:
                        status = "At Gin";
                        break;
                    case ModuleStatus.GINNED:
                        status = "Ginned";
                        break;
                    case ModuleStatus.PICKED_UP:
                        status = "Picked Up";
                        break;
                    default:
                        status = "In Field";
                        break;
                }

                return status;
            }
        }

        [JsonIgnore()]
        public string EventName
        {
            get
            {
                string name = "";
                switch (this.ModuleEventType)
                {
                    case ModuleEventType.LOADED:
                        name = "Loaded";
                        break;
                    case ModuleEventType.UNLOADED:
                        name = "Unloaded";
                        break;
                    case ModuleEventType.IMPORTED_FROM_FILE:
                        name = "Imported from file";
                        break;
                    default:
                        name = "Manual entry";
                        break;
                }
                return name;
            }
        }
                
        [JsonIgnore()]
        public string LocalCreatedTimestamp
        {
            get
            {
                DateTime local = Created.ToLocalTime();
                return local.ToShortDateString() + " " + local.ToLongTimeString(); ;
            }
        }

        public ModuleHistoryEntity() : base()
        {
            EntityType = EntityType.MODULE_HISTORY;
        }
    }
}