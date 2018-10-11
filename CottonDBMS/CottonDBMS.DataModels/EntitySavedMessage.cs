//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CottonDBMS.DataModels
{
    public class EntitySavedMessage
    {
        public object DataObject { get; set; }
    }

    public class EntitiesDeletedMessage
    {
        public object[] DataObjectsDeleted { get; set; }
    }
}