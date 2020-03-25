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
    public class GinLoadEntity : BaseEntity
    {        

        [JsonProperty(PropertyName = "ginTagLoadNumber")]
        public string GinTagLoadNumber { get; set; }

        [JsonProperty(PropertyName = "scaleBridgeLoadNumber")]
        public int ScaleBridgeLoadNumber { get; set; }

        [JsonProperty(PropertyName = "scaleBridgeId")]
        public string ScaleBridgeId { get; set; }

        [JsonProperty(PropertyName = "grossWeight")]
        public decimal GrossWeight { get; set; }

        //NetWeight is SeedCottonWeight
        [JsonProperty(PropertyName = "netWeight")]
        public decimal NetWeight { get; set; }

        [JsonIgnore()]
        public decimal? EstimatedSeedWeight
        {
            get
            {
                if (LintWeight.HasValue)
                {
                    return NetWeight - LintWeight.Value;
                }
                else
                {
                    return null;
                }
            }
        }

        [JsonIgnore]
        public decimal? LoadAverageModuleWeight
        {
            get
            {
                if (Modules != null && Modules.Count() > 0)
                {
                    return NetWeight / Convert.ToDecimal(Modules.Count());
                }
                else
                {
                    return null;
                }
            }
        }

        [JsonIgnore]
        public decimal? SumOfDiameterSquares
        {
            get
            {
                if (Modules != null && Modules.Count() > 0 && Modules.All(m => m.HIDDiameterInches.HasValue))
                {
                    return Modules.Sum(x => x.HIDDiameterInches.Value * x.HIDDiameterInches.Value);
                }
                else
                {
                    return null;
                }
            }
        }

        [JsonProperty(PropertyName = "lintWeight")]
        public decimal? LintWeight { get; set; }

        [JsonProperty(PropertyName = "splitWeight1")]
        public decimal? SplitWeight1 { get; set; }

        [JsonProperty(PropertyName = "splitWeight2")]
        public decimal? SplitWeight2 { get; set; }

        [JsonProperty(PropertyName = "truckID")]
        public string TruckID { get; set; }

        [JsonProperty(PropertyName = "yardLocation")]
        public string YardLocation { get; set; }

        [JsonProperty(PropertyName = "submittedBy")]
        public string SubmittedBy { get; set; }

       

        [JsonProperty(PropertyName = "pickedBy")]
        public string PickedBy { get; set; }

        [JsonProperty(PropertyName = "variety")]
        public string Variety { get; set; }

        [JsonProperty(PropertyName = "trailerNumber")]
        public string TrailerNumber { get; set; }
             

        [JsonProperty(PropertyName = "fieldId")]
        public string FieldId { get; set; }

        [JsonIgnore]
        public FieldEntity Field { get; set; }
                
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

        [JsonIgnore]
        public List<ModuleEntity> Modules { get; set; }

        [JsonIgnore]
        public List<BaleEntity> Bales { get; set; }

        [JsonIgnore]
        public string ClientName
        {
            get
            {
                if (Field != null && Field.Farm != null && Field.Farm.Client != null)
                    return Field.Farm.Client.Name;
                else
                    return string.Empty;
            }
        }

        [JsonIgnore]
        public string FarmName
        {
            get
            {
                if (Field != null && Field.Farm != null)
                    return Field.Farm.Name;
                else
                    return string.Empty;
            }
        }

        [JsonIgnore]
        public string FieldName
        {
            get
            {
                if (Field != null)
                    return Field.Name;
                else
                    return string.Empty;
            }
        }

        public GinLoadEntity() : base()
        {
            EntityType = EntityType.GIN_LOAD;
            Modules = new List<ModuleEntity>();
        }
    }
}
