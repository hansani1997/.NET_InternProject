using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Domain.ICookieAccessor
{
    public interface ICookieAccessor
    {

        public  Task<string> GetValueAsync(string key);
        public Task SetValueAsync(string key, string value);
    }
}
