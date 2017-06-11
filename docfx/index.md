PortableBingGeocoder
====================

Portable Class Library (PCL) REST client for Bing Location REST geocoding services. 

For some reason Microsoft doesn't have a .NET client for Bing geocoding services. This library is intended 
to ease geocoding tasks in Window Phone and Windows Store App projects.

* Requires a [Bing Maps API Key](http://msdn.microsoft.com/en-us/library/ff428642.aspx)
        
        [TestMethod]
        public async Task RoundtripCoordinateToPostalCode()
        {
            var service = new GeoCoder(APIKEY.Key, "Portable Bing GeoCoder unit tests");
            var coord = await service.GetCoordinate(new Address() { postalCode = "55116", countryRegion = "US" });

            Assert.IsTrue(coord.Item1.AboutEqual(44.9108238220215));
            Assert.IsTrue(coord.Item2.AboutEqual(-93.1702041625977));
            
            var address = await service.GetAddress(coord.Item1, coord.Item2);

            Assert.AreEqual(address.postalCode, "55116");
        }

    
## Quick Start Notes:
1. [NuGet Package](https://www.nuget.org/packages/PortableBingGeocoder/)
2. [GitHub Project](https://github.com/dkackman/PortableBingGeocoder/)