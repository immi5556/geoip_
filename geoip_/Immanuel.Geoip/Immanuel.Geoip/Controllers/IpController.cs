using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Immanuel.Geoip.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class IpController : ApiController
    {

        static async Task<bool> CountIncrement()
        {
            //actOnValue('45fdtkob', 'ConvertCnt', 'increment');
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(
                    "https://keyvalue.immanuel.co/api/KeyVal/ActOnValue/45fdtkob/ConvertCnt/increment",
                     new StringContent(""));
            }
            return true;
        }

        [Route("myip")]
        public async Task<string> GetIp()
        {
            await CountIncrement();
            return GetClientIp();
        }

        private string GetClientIp(HttpRequestMessage request = null)
        {
            request = request ?? Request;

            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }
            else if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
            else
            {
                return null;
            }
        }
    }
}