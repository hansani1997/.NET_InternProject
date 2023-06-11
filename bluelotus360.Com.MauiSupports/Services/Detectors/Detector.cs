using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.MauiSupports.Services.Detectors
{
    public class Detector : IDetector
    {
        public string PlatformDetector()
        {
#if WINDOWS
            return "Windows";
#elif ANDROID
            return "Android";
#elif MACCATALYST
            return "MAC";
#elif IOS
            return "IOS";
#else
            return "Unidentified";
#endif
        }
    }
}
