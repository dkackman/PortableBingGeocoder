using System;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BingGeocoder;

namespace GeoCoderTests
{
    [TestClass]
    public class AddressTests
    {
        private IGeoCoder _service;

        [TestInitialize]
        public void Init()
        {
            _service = new GeoCoder(APIKEY.Key, "Portable Bing GeoCoder unit tests");
        }

        [TestMethod]
        public async Task GetAddress()
        {
            var address = await _service.GetAddress(44.9108238220215, -93.1702041625977);

            Assert.IsNotNull(address);
        }

        [TestMethod]
        public async Task RoundtripPostalCode()
        {
            var coord = await _service.GetCoordinate("55116", "US");
            var address = await _service.GetAddress(coord.Item1, coord.Item2);

            Assert.AreEqual(address.postalCode, "55116");
        }
    }
}
