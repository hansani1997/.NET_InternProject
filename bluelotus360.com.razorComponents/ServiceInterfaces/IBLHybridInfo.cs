using BL10.CleanArchitecture.Domain.Entities.ApplicationInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.ServiceInterfaces
{
    public interface IBLHybridInfo
    {
        BlHybridInfo ReadAppInfo();
    }
}
