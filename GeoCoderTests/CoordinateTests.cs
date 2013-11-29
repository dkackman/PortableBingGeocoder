using System;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BingGeocoder;

namespace GeoCoderTests
{
    [TestClass]
    public class CoordinateTests
    {
        private IGeoCoder _service;

        [TestInitialize]
        public void Init()
        {
            _service = new GeoCoder(APIKEY.Key, "Portable Bing GeoCoder unit tests");
        }
        
        [TestMethod]
        public async Task CoordinateFromPostalCode()
        {
            var coord = await _service.CoordinateFromPostalCode("55116", "US");

            Assert.IsTrue(coord.Item1.AboutEqual(44.9108238220215));
            Assert.IsTrue(coord.Item2.AboutEqual(-93.1702041625977));
        }

        [TestMethod]
        public async Task PostalCodeFromCoordinate()
        {
            var postalCode = await _service.PostalCodeFromCoordinate(44.9108238220215, -93.1702041625977);

            Assert.AreEqual(postalCode, "55116");
        }
    }
}
