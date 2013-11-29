using System;
using System.Threading.Tasks;

namespace BingGeocoder
{
    /// <summary>
    /// GeoCoder interface in case mocking is needed
    /// </summary>
    public interface IGeoCoder
    {
        Task<GeoCodeResult> AddressesFromCoordinate(double lat, double lon);
        Task<Tuple<double, double>> CoordinateFromAddress(string addressLine, string locality, string adminDistrict, string postalCode, string countryRegion);
        Task<Tuple<double, double>> CoordinateFromPostalCode(string postalCode, string countryRegion);
        Task<string> PostalCodeFromCoordinate(double lat, double lon);
        Task<GeoCodeResult> Query(string query);
        Task<Tuple<double, double>> QueryCoordinate(string query);
    }
}
