using BL10.CleanArchitecture.Domain.Entities.ApplicationInformation;
using bluelotus360.com.razorComponents.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.wasmBlazor.Services
{
    public class WASMAppInfoService: IBLHybridInfo
    {
        public  BlHybridInfo ReadAppInfo()
        {

            return new BlHybridInfo()
            {
                AppName = "",
                Package = "",
                Version = "",
                Build = ""
            };
        }
    }
}
