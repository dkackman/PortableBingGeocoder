using System;
using System.IO;
using System.Net.Http;
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
        private class Callbacks : ResponseCallbacks
        {

        }

        public TestContext TestContext { get; set; }

        [TestMethod]
        [ExpectedException(typeof(TimeoutException))]
        public async Task ResponseTimesoutOnBusyHeader()
        {
            var mockFolder = TestContext.DeploymentDirectory; // the folder where the unit tests are running
            var captureFolder = Path.Combine(TestContext.TestRunDirectory, @"..\..\FakeResponses\"); // kinda hacky but this should be the solution folder

            // here we don't want to serialize or include our api key in response lookups so
            // pass a lambda that will indicate to the serialzier to filter that param out
            var store = new FileSystemResponseStore(mockFolder, captureFolder);
            using (var handler = new FakeHttpMessageHandler(store))
            {
                var client = new GeoCoder(APIKEY.Key, 4, 1000, "Portable-Bing-GeoCoder-UnitTests/1.0", handler: handler);
                var coord = await client.GetCoordinate(null, null, null, "55106", "US");

                Assert.IsNotNull(coord);
            }
        }
    }
}