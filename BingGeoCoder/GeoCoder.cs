using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace BingGeocoder
{
    public class GeoCoder : IGeoCoder
    {
        BingMapsRestClient _client;

        public GeoCoder(string apiKey, string user_agent = "", string culture = "en-US", UserContext context = null)
        {
            _client = new BingMapsRestClient(apiKey, user_agent, culture, context);
        }

        public async Task<string> GetAddressPart(double lat, double lon, string entityType)
        {
            var parms = new Dictionary<string, object>();
            parms.Add("includeEntityTypes", entityType);
            if (entityType.Equals("Neighborhood", StringComparison.OrdinalIgnoreCase))
                parms.Add("includeNeighborhood", "1");

            var result = await _client.Get<GeoCodeResult>(string.Format("Locations/{0},{1}", lat, lon), parms);

            return result.GetFirstAddressPart(entityType);
        }

        public async Task<string> GetFormattedAddress(double lat, double lon)
        {
            var parms = new Dictionary<string, object>();
            parms.Add("includeEntityTypes", "Address,PopulatedPlace,Postcode1,AdminDivision1,CountryRegion");

            var result = await _client.Get<GeoCodeResult>(string.Format("Locations/{0},{1}", lat, lon), parms);

            return result.GetFirstFormattedAddress();
        }

        public async Task<GeoCodeResult> GetAddress(double lat, double lon)
        {
            var parms = new Dictionary<string, object>();
            parms.Add("includeEntityTypes", "Address,Neighborhood,PopulatedPlace,Postcode1,AdminDivision1,AdminDivision2,CountryRegion");
            parms.Add("includeNeighborhood", "1");

            return await _client.Get<GeoCodeResult>(string.Format("Locations/{0},{1}", lat, lon), parms);
        }

        public async Task<Tuple<double, double>> GetCoordinate(string postalCode, string countryRegion)
        {
            var parms = new Dictionary<string, object>();
            parms.Add("countryRegion", countryRegion);
            parms.Add("postalCode", postalCode);

            var result = await _client.Get<GeoCodeResult>("Locations", parms);

            return result.FirstCoordinate();
        }

        public async Task<Tuple<double, double>> QueryCoordinate(string query)
        {
            var result = await Query(query);

            return result.FirstCoordinate();
        }

        public async Task<GeoCodeResult> Query(string query, int maxResults = 1)
        {
            var parms = new Dictionary<string, object>();
            parms.Add("q", query.Replace("\n", ", "));
            parms.Add("maxResults", maxResults);

            return await _client.Get<GeoCodeResult>("Locations", parms);
        }

        public async Task<Tuple<double, double>> GetCoordinate(string addressLine, string locality, string adminDistrict, string postalCode, string countryRegion, int maxResults = 1)
        {
            var parms = new Dictionary<string, object>();
            parms.Add("addressLine", addressLine);
            parms.Add("locality", locality);
            parms.Add("adminDistrict", adminDistrict);
            parms.Add("postalCode", postalCode);
            parms.Add("countryRegion", countryRegion);
            parms.Add("maxResults", maxResults);

            var result = await _client.Get<GeoCodeResult>("Locations", parms);

            return result.FirstCoordinate();
        }

        public async Task<Tuple<double, double>> GetCoordinate(string landMark, int maxResults = 1)
        {
            var parms = new Dictionary<string, object>();
            parms.Add("maxResults", maxResults);

            var result = await _client.Get<GeoCodeResult>("Locations/" + landMark, parms);

            return result.FirstCoordinate();
        }
    }
}
