using Geo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Immanuel.Geoip.Controllers
{
    public class HomeController : Controller
    {

        public static string GetIPAddress(HttpRequestBase request)
        {
            string ip;
            try
            {
                ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ip))
                {
                    if (ip.IndexOf(",") > 0)
                    {
                        string[] ipRange = ip.Split(',');
                        int le = ipRange.Length - 1;
                        ip = ipRange[le];
                    }
                }
                else
                {
                    ip = request.UserHostAddress;
                }
            }
            catch { ip = null; }

            return ip;
        }

        public ActionResult Index()
        {
            string yip = GetIPAddress(Request);
            ViewBag.YIP = yip;
            return View();
        }

        static bool ValidateDomain(string url)
        {
            if (Regex.IsMatch(url, @" # Rev:2013-03-26
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
                return true;
            }
            else
            {
                // NOT a valid named DNS host.
                Console.WriteLine("Not Valid....");
                return false;
            }
        }

        string GetIP(string name)
        {
            string ip = name;
            if (Uri.CheckHostName(name) == UriHostNameType.Unknown)
                throw new ApplicationException("Invalid ip or domain provided.");

            if (Uri.CheckHostName(name) == UriHostNameType.Dns)
            {
                if (!ValidateDomain(name))
                    throw new ApplicationException("Invalid domain provided.");
                IPHostEntry host = Dns.GetHostEntry(name);
                if (host.AddressList.Length < 1)
                    throw new ApplicationException("Invalid host tracked...");
                ip = host.AddressList[0].ToString();
            }
            return ip;
        }

        JsonResult GetIpv6(string inip, string ip)
        {
            App_Start.GeoipContext.InitData();
            CityLocation ty = null;
            CityIpV6 ty1 = new CityIpV6();
            IPAddress incomingIp = IPAddress.Parse(ip);
            DateTime strt = DateTime.UtcNow;
            var srch = ip;
            var lst = App_Start.GeoipContext._cityv6.FindAll(t => t.network.StartsWith(srch));
            if (lst.Count == 0 && srch.IndexOf(":") > 0)
            {
                srch = srch.TrimEnd(':');
                lst = App_Start.GeoipContext._cityv6.FindAll(t => t.network.StartsWith(srch));
            }
            if (lst.Count == 0 && srch.IndexOf(":") > -1)
            {
                srch = srch.Remove(srch.LastIndexOf(':'));
                lst = App_Start.GeoipContext._cityv6.FindAll(t => t.network.StartsWith(srch));
            }
            if (lst.Count == 0 && srch.IndexOf(":") > -1)
            {
                srch = srch.Remove(srch.LastIndexOf(':'));
                lst = App_Start.GeoipContext._cityv6.FindAll(t => t.network.StartsWith(srch));
            }
            if (lst.Count == 0 && srch.IndexOf(":") > -1)
            {
                srch = srch.Remove(srch.LastIndexOf(':'));
                lst = App_Start.GeoipContext._cityv6.FindAll(t => t.network.StartsWith(srch));
            }
            if (lst.Count == 0)
            {
                lst = App_Start.GeoipContext._cityv6;
            }
            foreach (var row in lst)
            {
                IPNetwork network = IPNetwork.Parse(row.network);

                if (IPNetwork.Contains(network, incomingIp))
                {
                    Console.WriteLine(row.postal_code);
                    ty1 = row;
                    ty = App_Start.GeoipContext._cityloc.Find(t => t.geoname_id == row.geoname_id);
                    if (ty != null)
                    {
                        Console.WriteLine(ty.ToString());
                    }
                    continue;
                }
            }
            DateTime end = DateTime.UtcNow;
            if (ty == null)
                return Json(new { Inip = inip, Message = "No data found" }, JsonRequestBehavior.AllowGet);
            else
            {
                return Json(new
                {
                    Inip = inip,
                    Network = ty1,
                    Address = ty,
                    Start = strt.ToString(),
                    End = end.ToString(),
                    type = "IPv6"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        JsonResult GetIpv4(string inip, string ip)
        {
            App_Start.GeoipContext.InitData();
            CityLocation ty = null;
            CityIpV4 ty1 = new CityIpV4();
            IPAddress incomingIp = IPAddress.Parse(ip);
            DateTime strt = DateTime.UtcNow;
            var lst = App_Start.GeoipContext._cityv4.FindAll(t => t.network.StartsWith(ip));
            if (lst.Count == 0)
            {
                var srch = ip.Remove(ip.LastIndexOf('.') + 1);
                lst = App_Start.GeoipContext._cityv4.FindAll(t => t.network.StartsWith(srch));
            }
            if (lst.Count == 0)
            {
                var srch = ip.Split('.')[0] + "." + ip.Split('.')[1];
                lst = App_Start.GeoipContext._cityv4.FindAll(t => t.network.StartsWith(srch));
            }
            if (lst.Count == 0)
            {
                var srch = ip.Split('.')[0];
                lst = App_Start.GeoipContext._cityv4.FindAll(t => t.network.StartsWith(srch));
            }
            if (lst.Count == 0)
            {
                lst = App_Start.GeoipContext._cityv4;
            }
            foreach (var row in lst)
            {
                IPNetwork network = IPNetwork.Parse(row.network);

                if (IPNetwork.Contains(network, incomingIp))
                {
                    Console.WriteLine(row.postal_code);
                    ty1 = row;
                    ty = App_Start.GeoipContext._cityloc.Find(t => t.geoname_id == row.geoname_id);
                    if (ty != null)
                    {
                        Console.WriteLine(ty.ToString());
                    }
                    continue;
                }
            }
            DateTime end = DateTime.UtcNow;
            if (ty == null)
                return Json(new { Inip = inip, Message = "No data found" }, JsonRequestBehavior.AllowGet);
            else
            {
                return Json(new
                {
                    Inip = inip,
                    Network = ty1,
                    Address = ty,
                    Start = strt.ToString(),
                    End = end.ToString(),
                    type = "IPv4"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Route("address/{ip}")]
        public JsonResult GetCity(string ip)
        {
            string inip = ip;
            try
            {
                ip = GetIP(ip);
                if (Uri.CheckHostName(ip) == UriHostNameType.IPv4)
                {
                    return GetIpv4(inip, ip);
                }
                if (Uri.CheckHostName(ip) == UriHostNameType.IPv6)
                {
                    return GetIpv6(inip, ip);
                }
                return Json(new { Inip = inip, Message = "Unknown basic request." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exp)
            {
                return Json(new { Inip = inip, Message = exp.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}