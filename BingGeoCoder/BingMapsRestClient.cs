using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;
using System.Linq;

using Newtonsoft.Json;

namespace BingGeocoder
{
    sealed class BingMapsRestClient : IDisposable
    {
        private readonly int _retryCount = 5;
        private readonly int _retryDelay = 1000;

        private readonly HttpClient _httpClient;
        private readonly string _defaultParameters;

        public BingMapsRestClient(string api_key, int retryCount, int retryDelay, string user_agent, string culture, UserContext context, HttpMessageHandler handler)
        {
            _httpClient = HttpClientFactory.CreateClient(user_agent, handler);
            _retryCount = retryCount;
            _retryDelay = retryDelay;
            _defaultParameters = CreateDefaultParameters(api_key, culture, context);
        }

        public async Task<T> Get<T>(string endPoint, IDictionary<string, object> parms) where T : class
        {
            Debug.Assert(parms != null);

            Uri uri = new Uri(endPoint + _defaultParameters + parms.AsQueryString("&"), UriKind.Relative);

            for (int i = 0; i <= _retryCount; i++)
            {
                var response = await TryGetResponse(uri);
                if (response != null)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    Debug.Assert(!string.IsNullOrEmpty(content));
                    if (!string.IsNullOrEmpty(content))
                    {
                        return JsonConvert.DeserializeObject<T>(content);
                    }
                }
                else
                {
                    await Task.Delay(_retryDelay);
                }
            }

            throw new TimeoutException(string.Format("Bing service did not indicate a valid response after {0} attempts.", _retryCount));
        }

        private async Task<HttpResponseMessage> TryGetResponse(Uri uri)
        {
            var response = await _httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            IEnumerable<string> values = null;
            // if the bing service is overloaded it sets this header to 1 to indicate that you can retry
            if (response.Headers.TryGetValues("X-MS-BM-WS-INFO", out values) && values.Any(v => v == "1"))
            {
                return null;
            }

            return response;
        }

        private static string CreateDefaultParameters(string key, string culture, UserContext context)
        {
            var d = new Dictionary<string, object>()
            {
                { "key", key }
            };
            
            if (!string.IsNullOrEmpty(culture))
            {
                d.Add("c", culture);
            }

            var ip = context?.IPAddress;
            if (!string.IsNullOrEmpty(ip))
            {
                d.Add("ip", ip);
            }

            var location = context?.Location;
            if (location != null)
            {
                d.Add("ul", string.Format("{0},{1}", location.Item1, location.Item2));
            }

            var mapView = context?.MapView;
            if (mapView != null)
            {
                d.Add("umv", string.Format("{0},{1},{2},{3}", mapView.Item1, mapView.Item2, mapView.Item3, mapView.Item4));
            }

            return d.AsQueryString();
        }

        public void Dispose()
        {
            if (_httpClient != null)
            {
                _httpClient.Dispose();
            }
        }
    }
}
