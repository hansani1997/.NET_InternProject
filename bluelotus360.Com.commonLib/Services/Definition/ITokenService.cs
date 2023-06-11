using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Services.Definition
{
        public interface ITokenService
        {
            Task<TokenDTO> GetToken();
            Task RemoveToken();
            Task SetToken(TokenDTO tokenDTO);
        }

        public class TokenDTO
        {
            public string Token { get; set; }

            public DateTime Expiration { get; set; }
        }
    
}
