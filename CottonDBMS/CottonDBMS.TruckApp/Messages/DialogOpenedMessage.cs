//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CottonDBMS.TruckApp.Messages
{
    public class DialogOpenedMessage
    {
        public object Sender { get; set; }
    }

    public class DialogClosedMessage
    {
        public object Sender { get; set; }
    }
}
