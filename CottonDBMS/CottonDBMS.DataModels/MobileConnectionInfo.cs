//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace CottonDBMS.DataModels
{
    [DataContract]
    public class MobileConnectionInfo
    {
        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "key")]
        public string Key { get; set; }

        [DataMember(Name = "gin")]
        public string Gin { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }
    }
}
