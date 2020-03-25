using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CottonDBMS.DataModels
{
    public class BalesFilter
    {
        public string PBINumber { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string SortCol1 { get; set; }
        public bool Sort1Ascending { get; set; }

        public string SortCol2 { get; set; }
        public bool Sort2Ascending { get; set; }

        public string SortCol3 { get; set; }
        public bool Sort3Ascending { get; set; }        
    }
}
