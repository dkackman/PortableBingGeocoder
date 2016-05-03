﻿using System;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BingGeocoder;

namespace GeoCoderTests
{
    [TestClass]
    [DeploymentItem(@"FakeResponses\")]
    public class CoordinateTests
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
        public async Task CoordinateFromPostalCode()
        {
            var coord = await _service.GetCoordinate(null, null, null, "55116", "US");

            Assert.IsTrue(coord.Item1.AboutEqual(44.910392761230469));
            Assert.IsTrue(coord.Item2.AboutEqual(-93.171073913574219));
        }

        [TestMethod]
        public async Task PostalCodeFromCoordinate()
        {
            var postalCode = await _service.GetAddressPart(44.9108238220215, -93.1702041625977, "Postcode1");

            Assert.AreEqual("55116", postalCode);
        }

        [TestMethod]
        public async Task CoordinateFromAddress()
        {
            var coord = await _service.GetCoordinate("One Microsoft Way", "Redmond", "WA", "98052", "US");

            Assert.IsTrue(coord.Item1.AboutEqual(47.640049383044243));
            Assert.IsTrue(coord.Item2.AboutEqual(-122.12979689240456));
        }

        [TestMethod]
        public async Task CoordinateFromAddressObject()
        {
            var address = new Address()
            {
                addressLine = "One Microsoft Way",
                locality = "Redmond",
                adminDistrict = "WA",
                postalCode = "98052",
                countryRegion = "US"
            };
            var coord = await _service.GetCoordinate(address);

            Assert.IsTrue(coord.Item1.AboutEqual(47.640049383044243));
            Assert.IsTrue(coord.Item2.AboutEqual(-122.12979689240456));
        }

        [TestMethod]
        public async Task CoordinateFromAddressQuery()
        {
            var coord = await _service.QueryCoordinate("One Microsoft Way, Redmond, WA 98052");

            Assert.IsTrue(coord.Item1.AboutEqual(47.640049383044243));
            Assert.IsTrue(coord.Item2.AboutEqual(-122.12979689240456));
        }

        [TestMethod]
        public async Task CoordinateFromLandmark()
        {
            var coord = await _service.GetCoordinate("Eiffel Tower");

            Assert.IsTrue(coord.Item1.AboutEqual(48.858600616455078));
            Assert.IsTrue(coord.Item2.AboutEqual(2.2939798831939697));
        }
    }
}