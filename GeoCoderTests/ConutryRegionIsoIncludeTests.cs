using System;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BingGeocoder;

namespace GeoCoderTests
{
    [TestClass]
    public class ConutryRegionIsoIncludeTests
    {
        private static IGeoCoder _service;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            _service = new GeoCoder(APIKEY.Key, 4, 1000, "Portable-Bing-GeoCoder-UnitTests/1.0", handler: MockInitialize.DefaultTestHandler);
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            if (_service != null)
            {
                _service.Dispose();
            }
        }

        [TestMethod]
        public async Task CISO2GetAddress()
        {
            var address = await _service.GetAddress(44.9108238220215, -93.1702041625977, include: "ciso2");

            Assert.IsNotNull(address);
            Assert.AreEqual("US", address.countryRegionIso2);
        }

        [TestMethod]
        public async Task CISO2GetGeoCodeResult()
        {
            var result = await _service.GetGeoCodeResult("One Microsoft Way", "Redmond", "WA", "98052", "US", include: "ciso2");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.resourceSets.Count > 0);
            Assert.IsTrue(result.resourceSets[0].resources.Count > 0);

            Assert.AreEqual("US", result.resourceSets[0].resources[0].address.countryRegionIso2);
        }

        [TestMethod]
        public async Task CISO2ParseAnAddress()
        {
            var address = await _service.ParseAddress("One Microsoft Way, Redmond, WA 98052", "ciso2");

            Assert.AreEqual("US", address.countryRegionIso2);
        }

        [TestMethod]
        public async Task CISO2QueryLandmark()
        {
            var result = await _service.Query("tower of london", 1, "ciso2");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.resourceSets.Count > 0);
            Assert.IsTrue(result.resourceSets[0].resources.Count > 0);

            Assert.AreEqual("GB", result.resourceSets[0].resources[0].address.countryRegionIso2);
        }
    }
}
