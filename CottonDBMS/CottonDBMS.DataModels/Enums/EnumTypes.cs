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
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EntityType { PICKUPLIST = 1, CLIENT = 2, MODULE_COUNT_SUMMARY = 3, FARM = 4, FIELD = 5, TRUCK = 6, DRIVER = 7, MODULE = 8, MODULE_HISTORY = 9, SETTING_SUMMARY = 10, TRUCK_LISTS_DOWNLOADED=11,  AGGREGATE_EVENT=12, WRITE_TEST=13, TRUCK_PICKUP_LIST_RELEASE=14 };

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProcessingAction { DELETE = 1 };

    [JsonConverter(typeof(StringEnumConverter))]
    public enum InputSource { GIN = 1, TRUCK=2 };

    [JsonConverter(typeof(StringEnumConverter))]
    public enum EventType { LOADED = 1, UNLOADED = 2 };

    public static class GUIDS
    {
        public static string UNASSIGNED_LIST_ID = "UNASSIGNED_LIST";
        public static string UNASSIGNED_FARM_ID = "UNASSIGNED_FARM";
        public static string UNASSIGNED_FIELD_ID = "UNASSIGNED_FIELD";
        public static string UNASSIGNED_CLIENT_ID = "UNASSIGNED_CLIENT";
    }
}

