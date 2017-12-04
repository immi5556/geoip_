using Geo.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Test.Geo
{
    class Program
    {
        static void Main(string[] args)
        {
            //IPHostEntry host = Dns.GetHostEntry("216.58.197.46");
            //Console.WriteLine(host.HostName);

            string iv6 = "2001:0db9::";
            var tu = iv6.TrimEnd(':');
            var tu1 = tu.Remove(tu.LastIndexOf(':'));
            //var tu2 = tu1.Remove(tu1.LastIndexOf(':'));
            //var tu3 = tu2.Remove(tu2.LastIndexOf(':'));
            //var tu4 = tu3.Remove(tu3.LastIndexOf(':'));

            try
            {
                IPHostEntry host = Dns.GetHostEntry("www.google.com");
                IPAddress[] ipaddr = host.AddressList;

                string ul1 = "www.rzim.org";
                host = Dns.GetHostEntry(ul1);
                IPAddress[] ipaddr2 = host.AddressList;
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                
            }

            IPAddress incomingIp = IPAddress.Parse("216.58.197.46");
            IPAddress incomingIp1 = IPAddress.Parse("2001:0db9::1");
            if (incomingIp.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                Console.WriteLine("V4..");
            if (incomingIp1.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                Console.WriteLine("V6..");


            string url = "2001:0db9::1";
            Console.WriteLine(Uri.CheckHostName(url));
            string url1 = "216.58.197.46";
            Console.WriteLine(Uri.CheckHostName(url1));
            string url2 = "www.within30.com";
            Console.WriteLine(Uri.CheckHostName(url2));
            string url3 = "someunknwon";
            Console.WriteLine(Uri.CheckHostName(url3));
            ValDOman(url);
            ValDOman(url1);
            ValDOman(url2);
            ValDOman(url3);
            ValDOman("within30.co");
            ValDOman("somebigjjj.within30.co");

            bool vurl = Uri.CheckHostName(url) != UriHostNameType.Unknown;
            IPHostEntry host1 = Dns.GetHostEntry(url2);
            IPAddress[] ipaddr1 = host1.AddressList;

            Console.ReadKey();
        }

        static void ValDOman(string text)
        {
            if (Regex.IsMatch(text, @" # Rev:2013-03-26
    # Match DNS host domain having one or more subdomains.
    # Top level domain subset taken from IANA.ORG. See:
    # http://data.iana.org/TLD/tlds-alpha-by-domain.txt
    ^                  # Anchor to start of string.
    (?!.{256})         # Whole domain must be 255 or less.
    (?:                # Group for one or more sub-domains.
      [a-z0-9]         # Either subdomain length from 2-63.
      [a-z0-9-]{0,61}  # Middle part may have dashes.
      [a-z0-9]         # Starts and ends with alphanum.
      \.               # Dot separates subdomains.
    | [a-z0-9]         # or subdomain length == 1 char.
      \.               # Dot separates subdomains.
    )+                 # One or more sub-domains.
    (?:                # Top level domain alternatives.
      [a-z]{2}         # Either any 2 char country code,
    | AERO|ARPA|ASIA|BIZ|CAT|COM|COOP|EDU|  # or TLD 
      GOV|INFO|INT|JOBS|MIL|MOBI|MUSEUM|    # from list.
      NAME|NET|ORG|POST|PRO|TEL|TRAVEL|XXX  # IANA.ORG
    )                  # End group of TLD alternatives.
    $                  # Anchor to end of string.",
    RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
            {
                // Valid named DNS host (domain).
                Console.WriteLine("Valid domain....");
            }
            else
            {
                // NOT a valid named DNS host.
                Console.WriteLine("Not Valid....");
            }
        }

        static void TrackIp()
        {
            const string fileNamev4 = @"C:\Immi\Personal\MyProjects\geoip_\geoip_\GeoLite2-City-CSV_20171107\GeoLite2-City-Blocks-IPv4.csv";
            const string fileNamev6 = @"C:\Immi\Personal\MyProjects\geoip_\geoip_\GeoLite2-City-CSV_20171107\GeoLite2-City-Blocks-IPv6.csv";
            const string fileNamecloc = @"C:\Immi\Personal\MyProjects\geoip_\geoip_\GeoLite2-City-CSV_20171107\GeoLite2-City-Locations-en.csv";
            _cityv4 = File.ReadAllLines(fileNamev4)
                                           .Skip(1)
                                           .Select(v => CityIpV4.FromCsv(v))
                                           .ToList();

            _cityv6 = File.ReadAllLines(fileNamev6)
                                           .Skip(1)
                                           .Select(v => CityIpV6.FromCsv(v))
                                           .ToList();

            _cityloc = File.ReadAllLines(fileNamecloc)
                                           .Skip(1)
                                           .Select(v => CityLocation.FromCsv(v))
                                           .ToList();

            while (true)
            {
                Console.WriteLine("Enter.. IP");
                string ip = Console.ReadLine();
                IPAddress incomingIp = IPAddress.Parse(ip);
                foreach (var row in _cityv4)
                {
                    IPNetwork network = IPNetwork.Parse(row.network);

                    if (IPNetwork.Contains(network, incomingIp))
                    {
                        Console.WriteLine(row.postal_code);
                        var ty = _cityloc.Find(t => t.geoname_id == row.geoname_id);
                        if (ty != null)
                        {
                            Console.WriteLine(ty.ToString());
                        }
                        continue;
                    }
                }
            }
        }

        static List<CityIpV4> _cityv4 = new List<CityIpV4>();
        static List<CityIpV6> _cityv6 = new List<CityIpV6>();
        static List<CityLocation> _cityloc = new List<CityLocation>();

        public static bool IpIsWithinBoliviaRange(string ip)
        {
            IPAddress incomingIp = IPAddress.Parse(ip);
            foreach (var subnet in Bolivia_Ip_Range)
            {
                IPNetwork network = IPNetwork.Parse(subnet);

                if (IPNetwork.Contains(network, incomingIp))
                    return true;
            }
            return false;
        }

        private static List<string> Bolivia_Ip_Range = new List<string>()
    {
        "12.144.86.0/23",
        "31.201.1.176/30",
        "46.36.198.101/32",
        "46.36.198.102/31",
        "46.36.198.104/31",
        "46.136.172.0/24",
        "63.65.11.0/24",
        "63.65.12.0/25",
        "63.65.12.128/26",
        "63.65.12.192/27",
        "63.65.12.224/28",
        "63.65.12.240/29",
        "63.65.12.248/30",
        "63.65.12.252/31",
        "63.65.12.254/32",
        "65.173.56.0/21",
        "67.23.241.179/32",
        "67.23.241.180/30",
        "67.23.241.184/29",
        "67.23.241.192/30",
        "67.23.241.196/31",
        "67.23.241.198/32",
        "72.32.164.56/29",
        "72.46.244.32/28",
        "74.91.16.48/29",
        "74.91.16.208/29",
        "74.91.20.48/28",
        "74.91.20.64/29",
        "74.112.134.120/29",
        "74.112.135.104/29",
        "74.205.37.16/29",
        "78.24.205.32/28",
        "98.129.27.88/29",
        "98.129.91.40/29",
        "166.114.0.0/16",
        "167.157.0.0/16",
        "174.143.165.80/29",
        "186.0.156.0/22",
        "186.2.0.0/17",
        "186.27.0.0/17",
        "190.0.248.0/21",
        "190.3.184.0/21",
        "166.114.0.0/16",
        "167.157.0.0/16",
        "186.2.0.0/18",
        "190.11.64.0/20",
        "190.11.80.0/20",
        "190.103.64.0/20",
        "190.104.0.0/19",
        "190.107.32.0/20",
        "190.129.0.0/17",
        "190.181.0.0/18",
        "190.186.0.0/18",
        "190.186.64.0/18",
        "190.186.128.0/18",
        "200.7.160.0/20",
        "200.58.64.0/20",
        "200.58.80.0/20",
        "200.58.160.0/20",
        "200.58.176.0/20",
        "200.75.160.0/20",
        "200.85.128.0/20",
        "200.87.0.0/17",
        "200.87.128.0/17",
        "200.105.128.0/19",
        "200.105.160.0/19",
        "200.105.192.0/19",
        "200.112.192.0/20",
        "200.119.192.0/20",
        "200.119.208.0/20",
        "201.222.64.0/19",
        "201.222.96.0/19"
    };

        //public static List<Geo.Model.CityIpV4> Cityv4 { get => _cityv4; set => _cityv4 = value; }
    }
}
