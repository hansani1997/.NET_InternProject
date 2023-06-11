using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.ServiceInterfaces
{
    public interface IBarcodeService
    {
        public Task<object> ReadBarcode();
    }
}
