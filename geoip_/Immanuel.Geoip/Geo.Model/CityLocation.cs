using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geo.Model
{
    public class CityLocation
    {
        public string geoname_id { get; set; }
        public string locale_code { get; set; }
        public string continent_code { get; set; }
        public string continent_name { get; set; }
        public string country_iso_code { get; set; }
        public string country_name { get; set; }
        public string subdivision_1_iso_code { get; set; }
        public string subdivision_1_name { get; set; }
        public string subdivision_2_iso_code { get; set; }
        public string subdivision_2_name { get; set; }
        public string city_name { get; set; }
        public string metro_code { get; set; }
        public string time_zone { get; set; }

        public static CityLocation FromCsv(string csvLine)
        {
            string[] line = csvLine.Split(',');
            CityLocation c = new CityLocation();
            c.geoname_id = line[0];
            c.locale_code = line[1];
            c.continent_code = line[2];
            c.continent_name = line[3];
            c.country_iso_code = line[4];
            c.country_name = line[5];
            c.subdivision_1_iso_code = line[6];
            c.subdivision_1_name = line[7];
            c.subdivision_2_iso_code = line[8];
            c.subdivision_2_name = line[9];
            c.city_name = line[10];
            c.metro_code = line[11];
            c.time_zone = line[12];
            return c;
        }

        public override string ToString()
        {
            return $"{city_name},{country_name},{subdivision_1_name},{subdivision_2_name},{metro_code}";
        }
    }
}
