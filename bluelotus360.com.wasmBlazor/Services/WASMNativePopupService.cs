using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bluelotus360.com.razorComponents.ServiceInterfaces;

namespace bluelotus360.com.wasmBlazor.Services
{
    public class WASMNativePopupService : INativePopupService
    {
        public async Task<object> showYesNoDialog(string txt)
        {
            return new object();
        }
    }
}
