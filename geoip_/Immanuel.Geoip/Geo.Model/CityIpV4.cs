using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geo.Model
{
    public class CityIpV4
    {
        public string network { get; set; }
        public string geoname_id { get; set; }
        public string registered_country_geoname_id { get; set; }
        public string represented_country_geoname_id { get; set; }
        public string is_anonymous_proxy { get; set; }
        public string is_satellite_provider { get; set; }
        public string postal_code { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string accuracy_radius { get; set; }

        public static CityIpV4 FromCsv(string csvLine)
        {
            string[] line = csvLine.Split(',');
            CityIpV4 c = new CityIpV4();
            c.network = line[0];
            c.geoname_id = line[1];
            c.registered_country_geoname_id = line[2];
            c.represented_country_geoname_id = line[3];
            c.is_anonymous_proxy = line[4];
            c.is_satellite_provider = line[5];
            c.postal_code = line[6];
            c.latitude = line[7];
            c.longitude = line[8];
            if (line.Length == 10)
                c.accuracy_radius = line[9];
            else
                c.accuracy_radius = "";
            line = null;
            return c;
        }
    }
}
