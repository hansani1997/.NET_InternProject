using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.MauiSupports.Services.LocationSuppliers
{
    public class LocationService : ILocationService
    {
        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;
        public async Task<LocationModel> GetCurrentLocation()
        {
            LocationModel locationModel;
            try
            {
                _isCheckingLocation = true;
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                _cancelTokenSource = new CancellationTokenSource();

                Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                if (location != null)
                {
                    locationModel = new LocationModel();
                    locationModel.Longitude = location.Longitude;
                    locationModel.Latitude = location.Latitude;
                    locationModel.Altitude = location.Altitude;
                    locationModel.Accuracy = (double)location.Accuracy;
                    locationModel.isMock = location.IsFromMockProvider;
                }
                else
                {
                    locationModel = new LocationModel();
                }
            }catch(Exception ex)
            {
                locationModel= new LocationModel();
            }
            finally { _isCheckingLocation = false; }

            return locationModel;
        }

        public double GetDistance(LocationModel L1, LocationModel L2)
        {
            Location l1 = new Location(L1.Latitude,L1.Longitude);
            Location l2 = new Location(L2.Latitude, L2.Longitude);

            return Location.CalculateDistance(l1, l2, DistanceUnits.Kilometers);
        }

        public async Task<LocationModel> GetLastKnownLocation()
        {
            try
            {
                Location location = await Geolocation.Default.GetLastKnownLocationAsync();

                if (location == null)
                {
                    return new LocationModel();
                }
                else
                {
                    LocationModel lmodel = new LocationModel();
                    lmodel.Latitude= location.Latitude;
                    lmodel.Longitude= location.Longitude;
                    lmodel.Altitude = location.Altitude;
                    lmodel.Accuracy = (double)location.Accuracy;
                    lmodel.isMock = location.IsFromMockProvider;

                    return lmodel;
                }
            }catch(Exception ex)
            {
                return new LocationModel();
            }
        }
    }
}
