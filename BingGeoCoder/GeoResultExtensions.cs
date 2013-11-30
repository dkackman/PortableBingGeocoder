using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace BingGeocoder
{
    /// <summary>
    /// Helper methods for spelunking the GeoResultSet and its child collections
    /// </summary>
    public static class GeoResultExtensions
    {
        public static IEnumerable<Address> GetAddresses(this GeoCodeResult result)
        {
            return from rs in result.resourceSets
                   from r in rs.resources
                   select r.address;
        }
        public static IEnumerable<Tuple<double, double>> GetCoordinates(this GeoCodeResult result)
        {
            return from rs in result.resourceSets
                   from r in rs.resources
                   let lat = r.point.coordinates[0]
                   let lon = r.point.coordinates[1]
                   select new Tuple<double, double>(lat, lon);
        }

        public static Address GetFirstAddress(this GeoCodeResult result)
        {
            return result.GetAddresses().FirstOrDefault();
        }

        public static Tuple<double, double> GetFirstCoordinate(this GeoCodeResult result)
        {
            return result.GetCoordinates().FirstOrDefault() ?? new Tuple<double, double>(0, 0);
        }

        public static string GetFirstAddressProperty(this GeoCodeResult result, Func<Address, string> selector)
        {
            Debug.Assert(selector != null);

            var address = result.GetFirstAddress();
            if (address != null)
                return selector(address);

            return "";
        }

        public static string GetFirstFormattedAddress(this GeoCodeResult result)
        {
            return result.GetFirstAddressProperty(a => a.formattedAddress);
        }

        public static string GetFirstAddressPart(this GeoCodeResult result, string part)
        {
            return result.GetFirstAddressProperty(GetAddressPartSelector(part));
        }

        private static Func<Address, string> GetAddressPartSelector(string part)
        {
            if (part.Equals("address", StringComparison.OrdinalIgnoreCase))
                return a => a.addressLine;

            if (part.Equals("Neighborhood", StringComparison.OrdinalIgnoreCase))
                return a => a.neighborhood;

            if (part.Equals("postcode1", StringComparison.OrdinalIgnoreCase))
                return a => a.postalCode;

            if (part.Equals("PopulatedPlace", StringComparison.OrdinalIgnoreCase))
                return a => a.locality;

            if (part.Equals("AdminDivision1", StringComparison.OrdinalIgnoreCase))
                return a => a.adminDistrict;

            if (part.Equals("AdminDivision2", StringComparison.OrdinalIgnoreCase))
                return a => a.adminDistrict2;

            if (part.Equals("CountryRegion", StringComparison.OrdinalIgnoreCase))
                return a => a.countryRegion;

            Debug.Assert(false);
            return a => "";
        }
    }
}
