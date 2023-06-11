using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.CustomExceptions
{
    public class GeoLocationException : FormatException
    {
        public GeoLocationException(string message) : base(message) { }
    }
}
