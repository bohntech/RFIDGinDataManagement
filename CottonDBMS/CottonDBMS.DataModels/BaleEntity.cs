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
    public class BaleEntity : BaseEntity
    {
        [JsonProperty(PropertyName = "pbiNumber")]
        public string PbiNumber { get; set; }

        [JsonProperty(PropertyName = "moduleId")]
        public string ModuleId { get; set; }

        [JsonProperty(PropertyName = "serialNumber")]
        public string ModuleSerialNumber { get; set; }

        [JsonProperty(PropertyName = "ginTicketLoadNumber")]
        public string GinTicketLoadNumber { get; set; }

        [JsonIgnore]
        public ModuleEntity Module { get; set; }

        [JsonProperty(PropertyName = "tareWeight")]
        public decimal TareWeight { get; set; }

        [JsonProperty(PropertyName = "weightFromScale")]
        public decimal WeightFromScale { get; set; }

        [JsonProperty(PropertyName = "netWeight")]
        public decimal NetWeight { get; set; }

        [JsonProperty(PropertyName = "outOfSequence")]
        public bool OutOfSequence { get; set; }

        [JsonProperty(PropertyName = "accumWeight")]
        public decimal? AccumWeight { get; set; }

        [JsonProperty(PropertyName = "overrageAdjustment")]
        public decimal? OverrageAdjustment { get; set; }

        [JsonProperty(PropertyName = "lintTurnout")]
        public decimal? LintTurnout { get; set; }

        [JsonProperty(PropertyName = "overragePercent")]
        public decimal? OverrageThreshold { get; set; }

        [JsonProperty(PropertyName = "ginLoadId")]
        public string GinLoadId { get; set; }

        [JsonIgnore]
        public GinLoadEntity GinLoad { get; set; }

        [JsonProperty(PropertyName = "classingNetWeight")]
        public decimal? Classing_NetWeight { get; set; }
                
        [JsonProperty(PropertyName = "classingPk")]
        public int? Classing_Pk { get; set; }

        [JsonProperty(PropertyName = "classingGr")]
        public int? Classing_Gr { get; set; }

        [JsonProperty(PropertyName = "classingLf")]
        public int? Classing_Lf { get; set; }

        [JsonProperty(PropertyName = "classingSt")]
        public int? Classing_St { get; set; }

        [JsonProperty(PropertyName = "classingMic")]
        public decimal? Classing_Mic { get; set; }

        [JsonProperty(PropertyName = "classingEx")]
        public int? Classing_Ex { get; set; }

        [JsonProperty(PropertyName = "classingRm")]
        public int? Classing_Rm { get; set; }

        [JsonProperty(PropertyName = "classingStr")]
        public decimal? Classing_Str { get; set; }

        [JsonProperty(PropertyName = "classingCGr")]
        public string Classing_CGr { get; set; }

        [JsonProperty(PropertyName = "classingRd")]
        public decimal? Classing_Rd { get; set; }

        [JsonProperty(PropertyName = "classingPlusb")]
        public decimal? Classing_Plusb { get; set; }

        [JsonProperty(PropertyName = "classingTr")]
        public int? Classing_Tr { get; set; }

        [JsonProperty(PropertyName = "classingUnif")]
        public decimal? Classing_Unif { get; set; }

        [JsonProperty(PropertyName = "classingRd")]
        public int? Classing_Len { get; set; }

        [JsonProperty(PropertyName = "classingValue")]
        public decimal? Classing_Value { get; set; }

        [JsonProperty(PropertyName = "classingTareWeight")]
        public decimal? Classing_TareWeight { get; set; }

        [JsonProperty(PropertyName = "classingTareWeight")]
        public decimal? Classing_EstimatedSeedWeight { get; set; }

        public int ScanNumber { get; set; }

        public string SequenceMessage
        {
            get
            {
                return (OutOfSequence) ? "** OUT OF SEQUENCE" : "";
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

        [JsonIgnore()]
        public string LocalUpdatedTimestamp
        {
            get
            {
                if (Updated.HasValue)
                {
                    DateTime local = Updated.Value.ToLocalTime();
                    return local.ToShortDateString() + " " + local.ToLongTimeString();
                }
                else
                {
                    return "";
                }
            }
        }
       

        [JsonIgnore()]
        public string ClientName
        {
            get
            {
                return (Module != null) ? this.Module.ClientName : "";
            }
        }

        [JsonIgnore()]
        public string FarmName
        {
            get
            {
                return (Module != null) ? this.Module.FarmName : "";
            }
        }

        [JsonIgnore()]
        public string FieldName
        {
            get
            {
                return (Module != null) ? this.Module.FieldName : "";
            }
        }

        public BaleEntity() : base()
        {
            EntityType = EntityType.BALE;            
        }
    }
}
