using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BingGeocoder
{
    static class HttpClientFactory
    {
        public static HttpClient CreateClient(string user_agent, HttpMessageHandler handler)
        {
            var messageHandler = handler ?? new HttpClientHandler();

            var client = new HttpClient(messageHandler, handler == null); // don't dispose the handler if it was passed in

            if (messageHandler is HttpClientHandler && ((HttpClientHandler)messageHandler).SupportsTransferEncodingChunked())
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
    }
}
