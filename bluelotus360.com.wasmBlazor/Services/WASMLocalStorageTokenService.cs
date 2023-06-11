using Blazored.LocalStorage;
using bluelotus360.Com.commonLib.Services.Definition;

namespace bluelotus360.com.wasmBlazor.Services
{
    public class WASMLocalStorageTokenService : ITokenService
    {
        private readonly ILocalStorageService localStorageService;

        public WASMLocalStorageTokenService(ILocalStorageService localStorageService)
        {
            this.localStorageService = localStorageService;
        }

        public async Task SetToken(TokenDTO tokenDTO)
        {
            await localStorageService.SetItemAsync("token", tokenDTO);
        }

        public async Task<TokenDTO> GetToken()
        {
            return await localStorageService.GetItemAsync<TokenDTO>("token");
        }

        public async Task RemoveToken()
        {
            await localStorageService.RemoveItemAsync("token");
        }
    }
}
