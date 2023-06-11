using bluelotus360.com.services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.services.ServiceDefinition.Login
{
    public interface ILoginService
    {
        Task<TokenResponse> Login(TokenRequest tok);
    }
}
