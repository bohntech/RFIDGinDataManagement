//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace CottonDBMS.DataModels
{
    public class ModuleEntity : BaseEntity
    {
        [JsonProperty(PropertyName = "fieldid")]
        public string FieldId { get; set; }

        [JsonIgnore]
        public FieldEntity Field { get; set; }

        [JsonProperty(PropertyName = "pickuplistid")]
        public string PickupListId { get; set; }

        [JsonProperty(PropertyName = "listname")] 
        public string ListName
        {
            get
            {
                if (PickupList != null) return PickupList.Name;
                else return "";
            }
        }

        [JsonIgnore]
        public PickupListEntity PickupList { get; set; }

        [JsonProperty(PropertyName = "isconventional")]
        public bool IsConventional { get; set; }

        [JsonIgnore()]
        public string ModuleId { get; set; }

        [JsonIgnore()]
        public string TruckID { get; set; }

        [JsonIgnore()]
        public string Driver { get; set; }

        [JsonProperty(PropertyName = "loadnumber")]
        public string LoadNumber { get; set; }

        [JsonIgnore()]
        public string ImportedLoadNumber { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; set; }

        [JsonProperty(PropertyName = "modulestatus")]
        public ModuleStatus ModuleStatus { get; set; }

        [JsonIgnore()]
        public List<ModuleHistoryEntity> ModuleHistory { get; set; }

        [JsonProperty(PropertyName="fieldname")]
        public string FieldName
        {
            get
            {
                if (this.Field != null) return this.Field.Name;
                else return "";
            }
        }

        [JsonProperty(PropertyName = "clientname")]
        public string ClientName
        {
            get
            {
                if (this.Field != null && this.Field.Farm != null && this.Field.Farm.Client != null)
                    return this.Field.Farm.Client.Name;
                else
                    return "";
            }
        }

        [JsonProperty(PropertyName = "farmname")]
        public string FarmName
        {
            get
            {
                if (this.Field != null && this.Field.Farm != null)
                    return this.Field.Farm.Name;
                else
                    return "";
            }
        }

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
        public string LocaleCreatedTimestamp
        {
            get
            {
                DateTime local = Created.ToLocalTime();
                return local.ToShortDateString() + " " + local.ToLongTimeString(); ;
            }
        }

        [JsonIgnore()]
        public string LoadNumberString
        {
            get
            {
                return LoadNumber;
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

        [JsonIgnore()]
        public string Notes { get; set; }

        public ModuleEntity() : base()
        {
            EntityType = EntityType.MODULE;
            ModuleHistory = new List<ModuleHistoryEntity>();
            IsConventional = false;
        }
    }
}
