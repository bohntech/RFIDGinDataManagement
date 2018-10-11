//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CottonDBMS.DataModels
{
    public class GpsCoords
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? Altitude { get; set; }
        public DateTime Timestamp { get; set; }

        public bool IsValid
        {
            get
            {
                var absLat = Math.Abs(NonNullLatitude);
                var absLong = Math.Abs(NonNullLongitude);

                return (absLat > 1.0 && absLong > 1.0);
            }
        }

        public double NonNullLatitude
        {
            get
            {
                return (Latitude.HasValue && !double.IsNaN(Latitude.Value)) ? Latitude.Value : 0.00;
            }
        }

        public double NonNullLongitude
        {
            get
            {
                return (Longitude.HasValue && !double.IsNaN(Longitude.Value)) ? Longitude.Value : 0.00;
            }
        }

        private double ToRadians(double val)
        {
            return (Math.PI / 180) * val;
        }

        private double ToDegrees(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        public void TranslateByHeading(double bearing, double offsetFeet, bool inReverse)
        {            
            double r = 6371 * 1000; // Earth Radius in m
            
            if (!inReverse)
                bearing += 180.000;  //go in opposite direction for offset behind truck

            double lat = NonNullLatitude;
            double lon = NonNullLongitude;                        

            double distance = offsetFeet * 0.3048; //get offset in meters

            double lat2 = Math.Asin(Math.Sin(ToRadians(lat)) * Math.Cos(distance / r)
                    + Math.Cos(ToRadians(lat)) * Math.Sin(distance / r) * Math.Cos(ToRadians(bearing)));

            double lon2 = ToRadians(lon)
                    + Math.Atan2(Math.Sin(ToRadians(bearing)) * Math.Sin(distance / r) * Math.Cos(ToRadians(lat)), Math.Cos(distance / r)
                    - Math.Sin(ToRadians(lat)) * Math.Sin(lat2));

            lat2 = ToDegrees(lat2);
            lon2 = ToDegrees(lon2);

            Latitude = lat2;
            Longitude = lon2;            
        }
    }
}
