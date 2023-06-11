using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.CommonServices
{
    public class ModeDetection : IModeDetection
    {
        public bool isDebugMode()
        {
#if DEBUG
           return true;
#else
           return false;
#endif

        }
    }
}
