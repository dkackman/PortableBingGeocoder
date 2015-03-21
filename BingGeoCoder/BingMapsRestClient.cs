using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using Newtonsoft.Json;

namespace BingGeocoder
{
    sealed class BingMapsRestClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _defaultParameters;

        public BingMapsRestClient(string api_key, string user_agent, string culture, UserContext context)
        {
            _httpClient = CreateClient(user_agent);

            _defaultParameters = CreateDefaultParameters(api_key, culture, context);
        }

        public async Task<T> Get<T>(string endPoint, IDictionary<string, object> parms) where T : class
        {
            Debug.Assert(parms != null);

            var response = await _httpClient.GetAsync(new Uri(endPoint + _defaultParameters + parms.AsQueryString("&"), UriKind.Relative));

            var content = await response.Content.ReadAsStringAsync();

            Debug.Assert(!string.IsNullOrEmpty(content));
            if (!string.IsNullOrEmpty(content))
            {
                return JsonConvert.DeserializeObject<T>(content);
            }

            return null;
        }

        private static HttpClient CreateClient(string user_agent)
        {
            var handler = new HttpClientHandler();
            if (handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            var client = new HttpClient(handler, true);

            if (handler.SupportsTransferEncodingChunked())
            {
                client.DefaultRequestHeaders.TransferEncodingChunked = true;
            }

            client.BaseAddress = new Uri("http://dev.virtualearth.net/REST/v1/", UriKind.Absolute);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            ProductInfoHeaderValue productHeader = null;
            if (!string.IsNullOrEmpty(user_agent) && ProductInfoHeaderValue.TryParse(user_agent, out productHeader))
            {
                client.DefaultRequestHeaders.UserAgent.Clear();
                client.DefaultRequestHeaders.UserAgent.Add(productHeader);
            }

            return client;
        }

        private static string CreateDefaultParameters(string key, string culture, UserContext context)
        {
            var d = new Dictionary<string, object>();

            d.Add("key", key);

            if (!string.IsNullOrEmpty(culture))
            {
                d.Add("c", culture);
            }

            if (context != null)
            {
                if (!string.IsNullOrEmpty(context.IPAddress))
                {
                    d.Add("ip", context.IPAddress);
                }

                if (context.Location != null)
                {
                    d.Add("ul", string.Format("{0},{1}", context.Location.Item1, context.Location.Item2));
                }

                if (context.MapView != null)
                {
                    d.Add("umv", string.Format("{0},{1},{2},{3}", context.MapView.Item1, context.MapView.Item2, context.MapView.Item3, context.MapView.Item4));
                }
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
