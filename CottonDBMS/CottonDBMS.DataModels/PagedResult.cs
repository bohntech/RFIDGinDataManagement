//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CottonDBMS.DataModels
{
    public class PagedResult<T>
    {
        public List<T> ResultData { get; set; }        
        public int Total { get; set; }
        public int LastPageNo { get; set; }
        public int TotalPages { get; set; }
    }
}
