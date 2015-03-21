﻿using System;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using BingGeocoder;

namespace GeoCoderTests
{
    [TestClass]
    public class AddressPartTests
    {
        private static IGeoCoder _service;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            _service = new GeoCoder(APIKEY.Key, "Portable-Bing-GeoCoder-UnitTests/1.0");
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
        public async Task GetFormattedAddressFromCoordinate()
        {
            var address = await _service.GetFormattedAddress(44.9108238220215, -93.1702041625977);

            Assert.AreEqual("1012 Davern St, St Paul, MN 55116", address);
        }

        [TestMethod]
        public async Task GetNeighborhoodFromCoordinate()
        {
            var address = await _service.GetAddressPart(44.9108238220215, -93.1702041625977, "Neighborhood");

            Assert.AreEqual("Highland", address);
        }

        [TestMethod]
        public async Task GetAddressFromCoordinate()
        {
            var address = await _service.GetAddressPart(44.9108238220215, -93.1702041625977, "Address");

            Assert.AreEqual("1012 Davern St", address);
        }

        [TestMethod]
        public async Task GetPostalCodeFromCoordinate()
        {
            var postalCode = await _service.GetAddressPart(44.9108238220215, -93.1702041625977, "Postcode1");

            Assert.AreEqual("55116", postalCode);
        }

        [TestMethod]
        public async Task GetCityFromCoordinate()
        {
            var city = await _service.GetAddressPart(44.9108238220215, -93.1702041625977, "PopulatedPlace");

            Assert.AreEqual("St Paul", city);
        }

        [TestMethod]
        public async Task GetCountyFromCoordinate()
        {
            var county = await _service.GetAddressPart(44.9108238220215, -93.1702041625977, "AdminDivision2");

            Assert.AreEqual("Ramsey Co.", county);
        }

        [TestMethod]
        public async Task GetStateFromCoordinate()
        {
            var state = await _service.GetAddressPart(44.9108238220215, -93.1702041625977, "AdminDivision1");

            Assert.AreEqual("MN", state);
        }

        [TestMethod]
        public async Task GetCountryFromCoordinate()
        {
            var country = await _service.GetAddressPart(44.9108238220215, -93.1702041625977, "CountryRegion");

            Assert.AreEqual("United States", country);
        }

        [TestMethod]
        public async Task GetCountryFromCoordinateUsingEnum()
        {
            var country = await _service.GetAddressPart(44.9108238220215, -93.1702041625977, AddressEntityType.CountryRegion);

            Assert.AreEqual("United States", country);
        }
    }
}
