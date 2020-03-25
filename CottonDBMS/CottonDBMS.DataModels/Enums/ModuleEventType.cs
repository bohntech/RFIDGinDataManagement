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
    public enum ModuleEventType { LOADED = 1, UNLOADED = 2, MANUAL_EDIT = 3, IMPORTED_FROM_FILE = 4, BRIDGE_SCAN = 5, IMPORTED_FROM_RFID_MODULESCAN = 6, IMPORTED_FROM_HID = 7 }
}
