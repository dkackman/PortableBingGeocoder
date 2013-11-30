using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BingGeocoder
{
    static class GeoResultExtensions
    {
        public static Tuple<double, double> FirstCoordinate(this GeoCodeResult result)
        {
            if (result.resourceSets.Count > 0 && result.resourceSets[0].resources.Count > 0)
            {
                Debug.Assert(result.resourceSets[0].resources[0].point.coordinates.Count > 1);
                var point = result.resourceSets[0].resources[0].point;
                return new Tuple<double, double>(point.coordinates[0], point.coordinates[1]);
            }

            return new Tuple<double, double>(0, 0);
        }

        public static string GetFirstFormattedAddress(this GeoCodeResult result)
        {
            if (result.resourceSets.Count > 0 && result.resourceSets[0].resources.Count > 0)
                return result.resourceSets[0].resources[0].address.formattedAddress;

            return "";
        }
        public static string GetFirstAddressPart(this GeoCodeResult result, string part)
        {
            if (result.resourceSets.Count > 0 && result.resourceSets[0].resources.Count > 0)
            {
                if (part.Equals("address", StringComparison.OrdinalIgnoreCase))
                    return result.resourceSets[0].resources[0].address.addressLine;

                if (part.Equals("Neighborhood", StringComparison.OrdinalIgnoreCase))
                    return result.resourceSets[0].resources[0].address.neighborhood;

                if (part.Equals("postcode1", StringComparison.OrdinalIgnoreCase))
                    return result.resourceSets[0].resources[0].address.postalCode;

                if (part.Equals("PopulatedPlace", StringComparison.OrdinalIgnoreCase))
                    return result.resourceSets[0].resources[0].address.locality;

                if (part.Equals("AdminDivision1", StringComparison.OrdinalIgnoreCase))
                    return result.resourceSets[0].resources[0].address.adminDistrict;

                if (part.Equals("AdminDivision2", StringComparison.OrdinalIgnoreCase))
                    return result.resourceSets[0].resources[0].address.adminDistrict2;

                if (part.Equals("CountryRegion", StringComparison.OrdinalIgnoreCase))
                    return result.resourceSets[0].resources[0].address.countryRegion;
            }

            return "";
        }
    }
}
