using Geo.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Immanuel.Geoip.App_Start
{
    public class GeoipContext
    {
        public static void InitData()
        {
            string fileNamev4 = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/GeoLite2-City-CSV_20171107/GeoLite2-City-Blocks-IPv4.csv");
            string fileNamev6 = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/GeoLite2-City-CSV_20171107/GeoLite2-City-Blocks-IPv6.csv");
            string fileNamecloc = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/GeoLite2-City-CSV_20171107/GeoLite2-City-Locations-en.csv");

            int idx = 0;
            foreach (var line in File.ReadLines(fileNamev4))
            {
                if (idx == 0)
                {
                    idx++;
                    continue;
                }
                _cityv4.Add(CityIpV4.FromCsv(line));
            }
            //_cityv4 = File.ReadAllLines(fileNamev4)
            //                               .Skip(1)
            //                               .Select(v => CityIpV4.FromCsv(v))
            //                               .ToList();
            idx = 0;
            foreach (var line in File.ReadLines(fileNamev6))
            {
                if (idx == 0)
                {
                    idx++;
                    continue;
                }
                _cityv6.Add(CityIpV6.FromCsv(line));
            }

            //_cityv6 = File.ReadAllLines(fileNamev6)
            //                               .Skip(1)
            //                               .Select(v => CityIpV6.FromCsv(v))
            //                               .ToList();
            idx = 0;
            foreach (var line in File.ReadLines(fileNamecloc))
            {
                if (idx == 0)
                {
                    idx++;
                    continue;
                }
                _cityloc.Add(CityLocation.FromCsv(line));
            }

            //_cityloc = File.ReadAllLines(fileNamecloc)
            //                               .Skip(1)
            //                               .Select(v => CityLocation.FromCsv(v))
            //                               .ToList();
        }

        public static List<CityIpV4> _cityv4 = new List<CityIpV4>();
        public static List<CityIpV6> _cityv6 = new List<CityIpV6>();
        public static List<CityLocation> _cityloc = new List<CityLocation>();
    }
}