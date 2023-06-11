using bluelotus360.com.razorComponents.CustomExceptions;
using bluelotus360.Com.MauiSupports.Services.LocationSuppliers;

namespace bluelotus360.com.wasmBlazor.Services
{
    public class WASMLocationService : ILocationService
    {
        public Task<LocationModel> GetCurrentLocation()
        {
            throw new GeoLocationException("You are trying to get location from Blazor WebAssembly.");
        }

        public double GetDistance(LocationModel L1, LocationModel L2)
        {
            throw new NotImplementedException();
        }

        public Task<LocationModel> GetLastKnownLocation()
        {
            throw new NotImplementedException();
        }
    }
}
