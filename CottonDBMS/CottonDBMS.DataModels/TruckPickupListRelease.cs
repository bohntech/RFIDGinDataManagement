//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CottonDBMS.DataModels
{
    public class TruckPickupListRelease : BaseEntity
    {
        public string PickupListID { get; set; }
        public string TruckID { get; set; }

        public TruckPickupListRelease()
        {
            EntityType = EntityType.TRUCK_PICKUP_LIST_RELEASE;
        }
    }
}
