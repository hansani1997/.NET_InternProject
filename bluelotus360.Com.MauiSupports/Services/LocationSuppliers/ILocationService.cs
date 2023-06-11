using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.MauiSupports.Services.LocationSuppliers
{
    public interface ILocationService
    {
        public Task<LocationModel> GetLastKnownLocation();
        public Task<LocationModel> GetCurrentLocation();
        public double GetDistance(LocationModel L1, LocationModel L2);
    }
}
