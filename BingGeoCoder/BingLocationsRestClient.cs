using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;

using PortableRest;

namespace BingGeocoder
{
    class BingLocationsRestClient : RestClient
    {
        string apikey_param;

        public BingLocationsRestClient(string api_key, string user_agent)
        {
            BaseUrl = "http://dev.virtualearth.net/REST/v1";
            UserAgent = user_agent;
            apikey_param = api_key;
        }

        public async Task<T> Get<T>(string resource) where T : class
        {
            var request = new RestRequest(resource, HttpMethod.Get);
            request.ContentType = ContentTypes.FormUrlEncoded;
            return await ExecuteAsync<T>(request);
        }

        public async Task<T> Get<T>(string endPoint, IDictionary<string, object> parms) where T : class
        {
            Debug.Assert(parms != null);

            var request = new RestRequest(endPoint, HttpMethod.Get);
            request.ContentType = ContentTypes.FormUrlEncoded;

            request.AddQueryString("o", "json");
            request.AddQueryString("key", apikey_param);

            foreach (var kvp in parms.Where(kvp => kvp.Value != null))
                request.AddQueryString(kvp.Key, kvp.Value.ToString());

            return await ExecuteAsync<T>(request);
        }
    }
}
