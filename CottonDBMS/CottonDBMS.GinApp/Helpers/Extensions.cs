using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CottonDBMS.GinApp.Helpers
{
    public static class Extensions
    {
        public static string NullableToString(this decimal? nbr)
        {
            return (nbr.HasValue) ? nbr.Value.ToString("0.00") : "";
        }

        public static string NullableToStringHighPrecision(this decimal? nbr)
        {
            return (nbr.HasValue) ? nbr.Value.ToString("0.000000") : "";
        }

        public static string NullableToString(this int? nbr)
        {
            return (nbr.HasValue) ? nbr.Value.ToString() : "";
        }
    }
}
