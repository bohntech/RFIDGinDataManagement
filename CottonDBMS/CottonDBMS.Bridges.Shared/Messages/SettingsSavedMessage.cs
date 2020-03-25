using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CottonDBMS.Bridges.Shared.Messages
{
    public class SettingsSavedMessage
    {
        public string GinName { get; set; }
        public bool IsFirstLaunch { get; set; }
    }
}
