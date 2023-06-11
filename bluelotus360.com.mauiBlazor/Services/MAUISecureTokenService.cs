
using bluelotus360.Com.commonLib.Services.Definition;
using BlueLotus360.Com.Application.Requests.Identity;
using BlueLotus360.Com.Application.Responses.Identity;
using BlueLotus360.Com.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.mauiBlazor.Services
{
    public class MAUISecureTokenService : ITokenService
    {
        private IStorageService _storageService;
        public MAUISecureTokenService(IStorageService storageService)
        {
            _storageService = storageService;
        }
        public async Task<TokenDTO> GetToken()
        {
            return await _storageService.GetItemAsync<TokenDTO>("token");
        }

        public async Task RemoveToken()
        {
            await _storageService.RemoveItem("token");
        }

        public async Task SetToken(TokenDTO tokenDTO)
        {
            // await _storageService.SaveItem<TokenDTO>("token", tokenDTO);
            await _storageService.SetItem<TokenDTO>("token", tokenDTO);
        }
    }
}
