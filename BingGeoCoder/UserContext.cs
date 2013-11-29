using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingGeocoder
{
    public class UserContext
    {
        public UserContext(string ip)
        {
            IPAddress = ip;
        }
        public UserContext(Tuple<double, double> location)
        {
            Location = location;
        }
        public UserContext(Tuple<double, double, double, double> mapView)
        {
            MapView = mapView;
        }
        public UserContext(string ip, Tuple<double, double> location, Tuple<double, double, double, double> mapView)
        {
            IPAddress = ip;
            Location = location;
            MapView = mapView;
        }

        public string IPAddress { get; private set; }

        public Tuple<double, double> Location { get; private set; }

        public Tuple<double, double, double, double> MapView { get; private set; }
    }
}
