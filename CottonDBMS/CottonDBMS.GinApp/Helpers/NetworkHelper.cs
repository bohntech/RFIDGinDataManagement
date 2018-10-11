//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CottonDBMS.GinApp.Helpers
{
    public static class NetworkHelper
    {
        public static bool HasNetwork()
        {
            bool _hasNetwork = false;           
            try
            {
                using (var webClient = new System.Net.WebClient())
                {
                    using (webClient.OpenRead("http://google.com"))
                    {
                        _hasNetwork = true;
                    }
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                _hasNetwork = false;
            }

            return _hasNetwork;           
        }    
    }
}
