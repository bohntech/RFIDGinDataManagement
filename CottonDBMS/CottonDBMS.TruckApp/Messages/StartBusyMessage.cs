//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CottonDBMS.TruckApp.Messages
{
    public class BusyMessage
    {
        public string Message { get; set; }
        public bool IsBusy { get; set; }
    }

    public class DataRefreshedMessage
    {
        
    }


}
