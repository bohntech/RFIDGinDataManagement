﻿//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CottonDBMS.TruckApp.Messages
{
    public class TagLoadingMessage
    {
        public string SerialNumber { get; set; }        
    }

    public class LoadingStoppedMessage
    {
        
    }

    public class UnloadingStoppedMessage
    {

    }


}