PortableBingGeocoder
====================

Portable Class Library (PCL) REST client for Bing Location REST geocoding services. Targets .NET 45, Window Phone 8, and Windows 8.1.

For some reason Microsoft doesn't have a .NET client for Bing geocoding services. This library is intended to ease geocoding tasks in Window Phone and Windows Store App projects.

* Requires a [Bing Maps API Key](http://msdn.microsoft.com/en-us/library/ff428642.aspx)
* Available as a [Nuget Package](https://www.nuget.org/packages/PortableBingGeoCoder/)

        
        [TestMethod]
        public async Task RoundtripCoordinateToPostalCode()
        {
            var service = new GeoCoder(APIKEY.Key, "Portable Bing GeoCoder unit tests");
            var coord = await service.GetCoordinate(new Address() { postalCode = "55116", countryRegion = "US" });

            Assert.IsTrue(coord.Item1.AboutEqual(44.9108238220215));
            Assert.IsTrue(coord.Item2.AboutEqual(-93.1702041625977));
            
            var postalCode = await service.GetAddressPart(coord.Item1, coord.Item2, "Postcode1");

            Assert.AreEqual(postalCode, "55116");
        }
