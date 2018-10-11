﻿//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CottonDBMS.DataModels
{
    public class DocumentToProcess : BaseEntity
    {
        public ProcessingAction Action { get; set; }
    }
}
