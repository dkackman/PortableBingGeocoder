using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BingGeocoder;

using FakeHttp;

namespace GeoCoderTests
{
    [TestClass]
    [DeploymentItem(@"FakeResponses\")]
    public class RetryTests
    {
        /// <summary>
        /// Used to fake a Locations response that returns the busy header on the first attempt but succeed subsequently
        /// </summary>
        private class RetryCallbacks : ResponseCallbacks
        {
            private int _callCount = 0;

            public override async Task<Stream> Deserialized(ResponseInfo info, Stream content)
            {
                // after 1 retry attempt, remove the busy header flag
                if (_callCount++ > 1 && info.ResponseHeaders.ContainsKey("X-MS-BM-WS-INFO"))
                {
                    info.ResponseHeaders["X-MS-BM-WS-INFO"] = Enumerable.Repeat("0", 1);
                }

                return await base.Deserialized(info, content);
            }
        }

        public TestContext TestContext { get; set; }

        [TestMethod]
        [ExpectedException(typeof(TimeoutException))]
        public async Task ResponseTimesoutOnBusyHeaderAfterRetyCountExceeded()
        {
            var store = new FileSystemResponseStore(TestContext.DeploymentDirectory);
            using (var handler = new FakeHttpMessageHandler(store))
            {
                var client = new GeoCoder(APIKEY.Key, 3, 100, "Portable-Bing-GeoCoder-UnitTests/1.0", handler: handler);
                await client.GetCoordinate(null, null, null, "55106", "US");
            }
        }

        [TestMethod]
        public async Task ResponseSucceedsAfterRetry()
        {
            var store = new FileSystemResponseStore(TestContext.DeploymentDirectory, new RetryCallbacks());
            using (var handler = new FakeHttpMessageHandler(store))
            {
                var client = new GeoCoder(APIKEY.Key, 4, 100, "Portable-Bing-GeoCoder-UnitTests/1.0", handler: handler);
                var result = await client.GetCoordinate(null, null, null, "55106", "US");

                Assert.IsNotNull(result);
            }
        }
    }
}