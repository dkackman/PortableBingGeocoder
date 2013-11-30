using System;
using System.Threading.Tasks;

namespace BingGeocoder
{
    /// <summary>
    /// GeoCoder interface in case mocking is needed
    /// </summary>
    public interface IGeoCoder
    {
        Task<Tuple<double, double>> GetCoordinate(string addressLine, string locality, string adminDistrict, string postalCode, string countryRegion, int maxResults = 1);
        Task<Tuple<double, double>> GetCoordinate(string postalCode, string countryRegion);
        Task<Tuple<double, double>> GetCoordinate(string landMark, int maxResults = 1);
        Task<string> GetAddressPart(double lat, double lon, string entityType);
        Task<GeoCodeResult> GetAddress(double lat, double lon);
        Task<string> GetFormattedAddress(double lat, double lon);
        Task<GeoCodeResult> Query(string query, int maxResults = 1);
        Task<Tuple<double, double>> QueryCoordinate(string query);
    }
}
