using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.DataModels;

namespace CottonDBMS.Interfaces
{
    public class PickupListFilter
    {
        public string Client { get; set; }
        public string Farm { get; set; }
        public string Field { get; set; }
        public string TruckID { get; set; }

        public PickupListStatus? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string SortCol1 { get; set; }
        public bool Sort1Ascending { get; set; }
    }



}
