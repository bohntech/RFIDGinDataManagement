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

        /*2019 Model Updates */
        [JsonIgnore]
        public string ClassingModuleId { get; set; }

        [JsonIgnore]
        public string GinTagLoadNumber { get; set; }
                

        [JsonIgnore]
        public string FirstBridgeId { get; set; }

        [JsonIgnore]
        public string LastBridgeId { get; set; }

        //JOHN DEERE FIELDS
        [JsonIgnore]
        public decimal? HIDModuleWeight { get; set; }

        [JsonIgnore]
        public decimal? HIDModuleWeightLBS
        {
            get
            {
                if (HIDModuleWeight.HasValue)
                    return (HIDModuleWeight.Value * 2.2046226218M);
                else
                    return null;
            }
        }

        [JsonIgnore]
        public decimal? HIDMoisture { get; set; }

        [JsonIgnore]
        public decimal? HIDFieldArea { get; set; }

        [JsonIgnore]
        public decimal? HIDFieldAreaAcres
        {
            get
            {
                if (HIDFieldArea.HasValue)
                    return HIDFieldArea.Value / 4046.8564224M;
                else
                    return null;
            }
        }

        [JsonIgnore]
        public decimal? HIDIncrementalArea { get; set; }

        [JsonIgnore]
        public decimal? HIDIncrementalAreaAcres
        {
            get
            {
                if (HIDIncrementalArea.HasValue)
                    return HIDIncrementalArea.Value / 4046.8564224M;
                else
                    return null;
            }
        }

        [JsonIgnore]
        public decimal? HIDDiameter { get; set; }

        [JsonIgnore]
        public decimal? HIDDiameterInches
        {
            get
            {
                if (HIDDiameter.HasValue)
                    return Decimal.Round(HIDDiameter.Value / 2.54M, 2);
                else
                    return null;
            }
        }

        [JsonIgnore]
        public int? HIDSeasonTotal { get; set; }

        [JsonIgnore]
        public string HIDGMTDate { get; set; }

        [JsonIgnore]
        public string HIDGMTTime { get; set; }

        [JsonIgnore]
        public DateTime? HIDTimestamp
        {
            get; set;
        }

        [JsonIgnore()]
        public string HIDTimestampToLocalString
        {
            get
            {
                if (HIDTimestamp.HasValue)
                {
                    DateTime local = HIDTimestamp.Value.ToLocalTime();
                    return local.ToShortDateString() + " " + local.ToLongTimeString();
                }
                else
                {
                    return "";
                }
            }
        }

        [JsonIgnore]
        public int? HIDFieldTotal { get; set; }

        [JsonIgnore]
        public string HIDVariety { get; set; }

        [JsonIgnore]
        public string HIDOperator { get; set; }

        [JsonIgnore]
        public string HIDProducerID { get; set; }

        [JsonIgnore]
        public string HIDGinID { get; set; }

        [JsonIgnore]
        public string HIDMachinePIN { get; set; }

        [JsonIgnore]
        public double HIDDropLat { get; set; }

        [JsonIgnore]
        public double HIDDropLong { get; set; }

        [JsonIgnore]
        public double HIDWrapLat { get; set; }

        [JsonIgnore]
        public double HIDWrapLong { get; set; }

        [JsonIgnore]
        public double HIDLat { get; set; }

        [JsonIgnore]
        public double HIDLong { get; set; }

        //END JOHN DEERE FIELDS

        [JsonIgnore]
        public string GinLoadId { get; set; }

        [JsonIgnore]
        public GinLoadEntity GinLoad { get; set; }

        [JsonIgnore]
        public List<BaleEntity> Bales { get; set; }


        [JsonIgnore]
        public string BridgeLoadNumber
        {
            get
            {
                return (GinLoad != null) ? GinLoad.ScaleBridgeLoadNumber.ToString() : string.Empty;
            }
        }

        [JsonIgnore()]
        public decimal? NetSeedCottonWeight
        {
            get; set;
        }

        [JsonIgnore()]
        public decimal? EstimatedNetSeedCottonWeight
        {
            get
            {
                if (HIDModuleWeightLBS.HasValue) return HIDModuleWeightLBS.Value;
                else if (HIDDiameterInches.HasValue) return HIDDiameterInches.Value;
                else return LoadAvgModuleWeight.Value;
            }
        }

        [JsonIgnore()]
        public decimal? EstimatedSeedWeight
        {
            get
            {
                if (LintWeight.HasValue && NetSeedCottonWeight.HasValue)
                {
                    return NetSeedCottonWeight - LintWeight.Value;
                }
                else
                {
                    return null;
                }
            }
        }

        [JsonIgnore()]
        public DateTime? LastFeederScanTime
        {
            get
            {
                if (ModuleHistory != null && ModuleHistory.Count() > 0)
                {
                    var scan = ModuleHistory.LastOrDefault(h => h.ModuleStatus == ModuleStatus.ON_FEEDER && h.ModuleEventType == ModuleEventType.BRIDGE_SCAN);
                    if (scan != null) return scan.Created;
                    else return null;
                }
                else
                {
                    return null;
                }
            }
        }

        [JsonIgnore()]
        public string LastFeederScanTimeToLocalString
        {
            get
            {
                if (LastFeederScanTime.HasValue)
                {
                    DateTime local = LastFeederScanTime.Value.ToLocalTime();
                    return local.ToShortDateString() + " " + local.ToLongTimeString();
                }
                else
                {
                    return "";
                }
            }
        }

        [JsonIgnore()]
        public decimal? LintWeight { get; set; }

        [JsonIgnore()]
        public decimal? LoadWeightMultiplier
        {
            get
            {
                if (GinLoad != null && HIDDiameterInches.HasValue && GinLoad.SumOfDiameterSquares.HasValue && GinLoad.SumOfDiameterSquares > 0.000M)
                {
                    return HIDDiameterInches.Value * HIDDiameterInches.Value / GinLoad.SumOfDiameterSquares.Value;
                }
                else
                {
                    return null;
                }
            }
        }

        [JsonIgnore()]
        public DateTime? RFIDModuleScanDateTime
        {
            get
            {
                if (ModuleHistory != null && ModuleHistory.Count() > 0)
                {
                    var scan = ModuleHistory.OrderBy(h => h.Created).FirstOrDefault(h => h.ModuleEventType == ModuleEventType.IMPORTED_FROM_RFID_MODULESCAN);
                    if (scan != null) return scan.Created;
                    else return null;
                }
                else
                {
                    return null;
                }
            }
        }

        [JsonIgnore()]
        public string RFIDModuleScanDateTimeToLocalString
        {
            get
            {
                if (RFIDModuleScanDateTime.HasValue)
                {
                    DateTime local = RFIDModuleScanDateTime.Value.ToLocalTime();
                    return local.ToShortDateString() + " " + local.ToLongTimeString();
                }
                else
                {
                    return "";
                }
            }
        }

        [JsonIgnore()]
        public DateTime? TruckLoadScanDateTime
        {
            get
            {
                if (ModuleHistory != null && ModuleHistory.Count() > 0)
                {
                    var scan = ModuleHistory.OrderBy(h => h.Created).FirstOrDefault(h => h.ModuleStatus == ModuleStatus.PICKED_UP &&  h.ModuleEventType == ModuleEventType.LOADED);
                    if (scan != null) return scan.Created;
                    else return null;
                }
                else
                {
                    return null;
                }
            }
        }

        [JsonIgnore()]
        public string TruckLoadScanDateTimeToLocalString
        {
            get
            {
                if (TruckLoadScanDateTime.HasValue)
                {
                    DateTime local = TruckLoadScanDateTime.Value.ToLocalTime();
                    return local.ToShortDateString() + " " + local.ToLongTimeString();
                }
                else
                {
                    return "";
                }
            }
        }

        [JsonIgnore()]
        public DateTime? FirstScaleBridgeScanTime
        {
            get
            {
                if (ModuleHistory != null && ModuleHistory.Count() > 0)
                {
                    var scan = ModuleHistory.OrderBy(h => h.Created).FirstOrDefault(h => h.ModuleStatus == ModuleStatus.AT_GIN &&  h.ModuleEventType == ModuleEventType.BRIDGE_SCAN);
                    if (scan != null) return scan.Created;
                    else return null;
                }
                else
                {
                    return null;
                }
            }
        }

        [JsonIgnore()]
        public string FirstScaleBridgeScanTimeToLocalString
        {
            get
            {
                if (FirstScaleBridgeScanTime.HasValue)
                {
                    DateTime local = FirstScaleBridgeScanTime.Value.ToLocalTime();
                    return local.ToShortDateString() + " " + local.ToLongTimeString();
                }
                else
                {
                    return "";
                }
            }
        }

        [JsonIgnore()]
        public decimal? DiameterApproximatedWeight
        {
            get
            {
                if (LoadWeightMultiplier.HasValue && GinLoad != null)
                {
                    return GinLoad.NetWeight * LoadWeightMultiplier.Value;
                }
                else
                {
                    return null;
                }
            }
        }

       

        [JsonIgnore()]
        public decimal? LoadSeedCottonWeight
        {
            get
            {
                if (GinLoad != null)
                {
                    return GinLoad.NetWeight;
                }
                else
                {
                    return null;
                }
            }
        }

        [JsonIgnore()]
        public decimal? LoadLintWeight
        {
            get
            {
                if (GinLoad != null && GinLoad.LintWeight.HasValue)
                {
                    return GinLoad.LintWeight.Value;
                }
                else
                {
                    return null;
                }
            }
        }

        [JsonIgnore()]
        public decimal? LoadTotalModules
        {
            get
            {
                if (GinLoad != null && GinLoad.Modules != null)
                {
                    return GinLoad.Modules.Count();
                }
                else
                {
                    return null;
                }
            }
        }

        [JsonIgnore()]
        public decimal? LoadAvgModuleWeight
        {
            get
            {
                if (GinLoad != null && GinLoad.LoadAverageModuleWeight.HasValue)
                {
                    return GinLoad.LoadAverageModuleWeight.Value;
                }
                else
                {
                    return null;
                }
            }
        }


        [JsonIgnore()]
        public double? AppScanLat
        {
            get
            {
                var item = ModuleHistory.Where(t => t.ModuleEventType == 
                    ModuleEventType.IMPORTED_FROM_RFID_MODULESCAN)
                    .OrderBy(t => t.Created)
                    .FirstOrDefault();

                if (item != null) return item.Latitude;
                else return null;
            }
        }

        [JsonIgnore()]
        public double? AppScanLong
        {
            get
            {
                var item = ModuleHistory.Where(t => t.ModuleEventType ==
                    ModuleEventType.IMPORTED_FROM_RFID_MODULESCAN)
                    .OrderBy(t => t.Created)
                    .FirstOrDefault();

                if (item != null) return item.Longitude;
                else return null;
            }
        }
        /*****************************************/


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
                    case ModuleStatus.ON_FEEDER:
                        status = "On Feeder";
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
            Bales = new List<BaleEntity>();
            IsConventional = false;
        }
    }
}
