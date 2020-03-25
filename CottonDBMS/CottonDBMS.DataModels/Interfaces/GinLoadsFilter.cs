using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.DataModels;

namespace CottonDBMS.Interfaces
{

    public class GinLoadsFilter
    {   
        public string BridgeId { get; set; }
        public string GinTagLoadNumber { get; set; }
        public string BridgeLoadNumber { get; set; }

        public string Client { get; set; }
        public string Farm { get; set; }
        public string Field { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string SortCol1 { get; set; }
        public bool Sort1Ascending { get; set; }

        public string SortCol2 { get; set; }
        public bool Sort2Ascending { get; set; }

        public string SortCol3 { get; set; }
        public bool Sort3Ascending { get; set; }
        public bool RecentOnly { get; set; }
    }
}
