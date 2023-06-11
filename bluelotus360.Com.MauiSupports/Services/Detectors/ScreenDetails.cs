using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.MauiSupports.Services.Detectors
{
    public class ScreenDetails : IScreenDetails
    {
        public double GetScreenHeight()
        {
            return DeviceDisplay.MainDisplayInfo.Height;
        }

        public double GetScreenWidth()
        {
            return DeviceDisplay.MainDisplayInfo.Width;
        }
    }

 
}
