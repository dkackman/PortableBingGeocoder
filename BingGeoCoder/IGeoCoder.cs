﻿using System;
using System.Threading.Tasks;

namespace BingGeocoder
{
    /// <summary>
    /// GeoCoder interface in case mocking is needed
    /// </summary>
    public interface IGeoCoder
    {
        /// <summary>
        /// Retrieves the geo coordinate from detailed address inputs
        /// </summary>
        /// <param name="addressLine">The address part. ex One Microsoft Way</param>
        /// <param name="locality">City part. ex Redmond</param>
        /// <param name="adminDistrict">State part. ex WA</param>
        /// <param name="postalCode">Postal code part. ex 98052</param>
        /// <param name="countryRegion">Country part. ex US</param>
        /// <param name="maxResults">The maximum number of results to return. Defaults to 1</param>
        /// <returns>The coordinate of the address. {0,0} if not found</returns>
        Task<Tuple<double, double>> GetCoordinate(string addressLine, string locality, string adminDistrict, string postalCode, string countryRegion, int maxResults = 1);
       
        /// <summary>
        /// Retrieves the geo coordinate of a postal code
        /// </summary>
        /// <param name="postalCode">Postal code part. ex 98052</param>
        /// <param name="countryRegion">Country part. ex US</param>
        /// <param name="maxResults">The maximum number of results to return. Defaults to 1</param>
        /// <returns>The coordinate of the postal code. {0,0} if not found</returns>
        Task<Tuple<double, double>> GetCoordinate(string postalCode, string countryRegion, int maxResults = 1);

        /// <summary>
        /// Retrieves the geo coordinate of a landmark
        /// </summary>
        /// <param name="landMark">A landmark. ex Eiffel Tower</param>
        /// <param name="maxResults">The Maximum number of results to return. Defaults to 1</param>
        /// <returns>The coordinates of a landmark {0,0} if not found</returns>
        Task<Tuple<double, double>> GetCoordinate(string landMark, int maxResults = 1);

        /// <summary>
        /// Retreives the GeoCodeResult for the given geo coordinate
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="lon">Longitude</param>
        /// <param name="includeNeighborhood">Specifies to include the neighborhood in the response when it is available.</param>
        /// <returns>GeoCodeResult</returns>
        Task<GeoCodeResult> GetGeoCodeResult(double lat, double lon, bool includeNeighborhood = false);

        /// <summary>
        /// Retreives the GeoCodeResult for the given address
        /// </summary>
        /// <param name="addressLine">The address part. ex One Microsoft Way</param>
        /// <param name="locality">City part. ex Redmond</param>
        /// <param name="adminDistrict">State part. ex WA</param>
        /// <param name="postalCode">Postal code part. ex 98052</param>
        /// <param name="countryRegion">Country part. ex US</param>
        /// <param name="maxResults">The maximum number of results to return. Defaults to 1</param>
        /// <returns>GeoCodeResult</returns>
        Task<GeoCodeResult> GetGeoCodeResult(string addressLine, string locality, string adminDistrict, string postalCode, string countryRegion, int maxResults = 1);
        
        /// <summary>
        /// Retreives part of an address at the geo coordinate
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="lon">Longitude</param>
        /// <param name="entityType">The enti ty type part to return</param>
        /// <returns>The address part associated with the entityType, for the first result</returns>
        Task<string> GetAddressPart(double lat, double lon, string entityType);

        /// <summary>
        /// Retrieves the address of a location
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="lon">Longitude</param>
        /// <param name="includeNeighborhood">Specifies to include the neighborhood in the response when it is available.</param>
        /// <returns>The address of the first result</returns>
        Task<Address> GetAddress(double lat, double lon, bool includeNeighborhood = false);

        /// <summary>
        /// Retrieves the formatted address for the geo coordinate
        /// </summary>
        /// <param name="lat">Latitude</param>
        /// <param name="lon">Longitude</param>
        /// <returns>Formatted address of the first result</returns>
        Task<string> GetFormattedAddress(double lat, double lon);

        /// <summary>
        /// Retrieves the geo result of a location, such as an address or landmark name.
        /// </summary>
        /// <param name="query">The location query</param>
        /// <param name="maxResults">The maximum number of results to return. Defaults to 1</param>
        /// <returns>GeoCodeResult</returns>
        Task<GeoCodeResult> Query(string query, int maxResults = 1);

        /// <summary>
        /// Retrieves the geo coordinate of a location, such as an address or landmark name.
        /// </summary>
        /// <param name="query">The location query</param>
        /// <param name="maxResults">The maximum number of results to return. Defaults to 1</param>
        /// <returns>The coordinate of the first result</returns>
        Task<Tuple<double, double>> QueryCoordinate(string query, int maxResults = 1);
    }
}
