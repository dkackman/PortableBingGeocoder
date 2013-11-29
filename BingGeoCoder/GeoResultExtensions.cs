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
    }
}
