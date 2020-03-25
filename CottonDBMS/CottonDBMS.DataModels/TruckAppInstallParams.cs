//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CottonDBMS.DataModels
{
    public class TruckAppInstallParams
    {
        public string EndPoint { get; set; }
        public string Key { get; set; }
        public string LockCode { get; set; }
        public string TruckID { get; set; }
    }

    public class BridgeInstallParams
    {
        public string EndPoint { get; set; }
        public string Key { get; set; }        
    }
}
