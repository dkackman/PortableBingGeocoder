using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;

using PortableRest;

namespace BingGeocoder
{
    class BingMapsRestClient : RestClient
    {
        string _apiKey;

        public BingMapsRestClient(string api_key, string user_agent, string culture, UserContext context)
        {
            BaseUrl = "http://dev.virtualearth.net/REST/v1/";
            UserAgent = user_agent;
            _apiKey = api_key;
            Culture = culture;
            UserContext = context;
        }

        public string Culture { get; private set; }

        public UserContext UserContext { get; private set; }

        public async Task<T> Get<T>(string resource) where T : class
        {
            var request = new RestRequest(resource, HttpMethod.Get);
            request.ContentType = ContentTypes.FormUrlEncoded;
            
            SetAPIParams(request);

            return await ExecuteAsync<T>(request);
        }

        public async Task<T> Get<T>(string endPoint, IDictionary<string, object> parms) where T : class
        {
            Debug.Assert(parms != null);

            var request = new RestRequest(endPoint, HttpMethod.Get);
            request.ContentType = ContentTypes.FormUrlEncoded;

            SetAPIParams(request);

            foreach (var kvp in parms.Where(kvp => kvp.Value != null))
                request.AddQueryString(kvp.Key, kvp.Value.ToString());

            return await ExecuteAsync<T>(request);
        }

        private void SetAPIParams(RestRequest request)
        {
            request.AddQueryString("o", "json");
            request.AddQueryString("key", _apiKey);

            if (!string.IsNullOrEmpty(Culture))
                request.AddQueryString("c", Culture);

            if (UserContext != null)
            {
                if (!string.IsNullOrEmpty(UserContext.IPAddress))
                    request.AddQueryString("ip", UserContext.IPAddress);

                if (UserContext.Location != null)
                    request.AddQueryString("ul", string.Format("{0},{1}", UserContext.Location.Item1, UserContext.Location.Item2));

                if (UserContext.MapView != null)
                    request.AddQueryString("umv", string.Format("{0},{1},{2},{3}", UserContext.MapView.Item1, UserContext.MapView.Item2, UserContext.MapView.Item3, UserContext.MapView.Item4));
            }
        }
    }
}
