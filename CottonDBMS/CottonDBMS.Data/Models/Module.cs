using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel.DataAnnotations;


namespace CottonDBMS.Data.Models
{
    public class ModuleItem
    {
        [Display(Name = "Selected")]        
        public bool Selected { get; set; }

        public string Grower { get; set; }
        public string Farm { get; set; }
        public string Field { get; set; }

        [Display(Name = "Serial #")]
        public string SerialNo { get; set; }

        [Display(Name = "Load #")]
        public int LoadNo { get; set; }

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public string Status { get; set; }
    }
}
