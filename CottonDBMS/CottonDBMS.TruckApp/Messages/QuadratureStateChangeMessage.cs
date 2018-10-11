//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.TruckApp.Enums;

namespace CottonDBMS.TruckApp.Messages
{
    public class QuadratureStateChangeMessage
    {
        public DirectionOfRotation DirectionOfRotation { get; set; }
        public DateTime Timestamp { get; set; }
        public int Position { get; set; }
    }
}
