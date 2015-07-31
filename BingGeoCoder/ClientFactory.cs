using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;

using Microsoft.Practices.ServiceLocation;

namespace BingGeocoder
{
    static class ClientFactory
    {
        public static HttpClient CreateClient(string user_agent)
        {
            bool disposeHandler;
            var handler = GetHandler(out disposeHandler);

            var client = new HttpClient(handler, disposeHandler);

            if (handler is HttpClientHandler && ((HttpClientHandler)handler).SupportsTransferEncodingChunked())
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

        private static HttpMessageHandler GetHandler(out bool dispose)
        {
            try
            {
                if (ServiceLocator.IsLocationProviderSet)
                {
                    var handler = ServiceLocator.Current.GetInstance<HttpMessageHandler>();
                    if (handler != null)
                    {
                        dispose = false;
                        return handler;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            var clientHandler = new HttpClientHandler();
            if (clientHandler.SupportsAutomaticDecompression)
            {
                clientHandler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            dispose = true;
            return clientHandler;
        }
    }
}
