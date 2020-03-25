//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CottonDBMS.Bridges.Shared.Helpers
{   
    public class ValidationHelper
    {
        public static bool ValidPort(string input)
        {
            int port = 0;
            return int.TryParse(input, out port);
        }

        public static bool ValidDecimal(string input)
        {
            decimal value = 0.0M;
            return decimal.TryParse(input, out value);
        }

        public static bool ValidInt(string input, int min, int max)
        {
            int value = 0;
            if (int.TryParse(input, out value))
            {
                return (value >= min && value <= max);
            }
            else
            {
                return false;
            }
        }

        public static bool ValidUrl(string input)
        {
            Uri uriResult;
            return Uri.TryCreate(input, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        public static bool ValidateLatLong(string input)
        {
            decimal value = 0.0M;
            bool result = decimal.TryParse(input, out value);

            return result && value >= -180.000M && value <= 180.000M;
        }
    }
}
