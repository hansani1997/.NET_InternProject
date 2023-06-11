using Blazored.LocalStorage;
using bluelotus360.Com.commonLib.Services.Definition;
using Newtonsoft.Json;

namespace bluelotus360.com.wasmBlazor.Services
{
    public class WASMLocalStorageService : IStorageService
    {
        private readonly ILocalStorageService _localStorageService;

        public WASMLocalStorageService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async ValueTask<bool> ContainKeyAsync(string key)
        {
            return await _localStorageService.ContainKeyAsync(key);
        }

        public async Task<string> GetItem(string key)
        {
            string result=await _localStorageService.GetItemAsStringAsync(key);

            if (string.IsNullOrEmpty(result)) return default;

            try
            {
                return JsonConvert.DeserializeObject<string>(result);
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public async Task<T> GetItemAsync<T>(string key)
        {
            return await _localStorageService.GetItemAsync<T>(key);
        }

        public async Task RemoveItem(string key)
        {
            await _localStorageService.RemoveItemAsync(key);
        }

        public async Task SetItem<T>(string key, T item)
        {
            await _localStorageService.SetItemAsync(key, item);
        }

        public async ValueTask SetItemAsync<T>(string key, T data)
        {
            await _localStorageService.SetItemAsync(key, data);
        }

        public async Task RemoveAll()
        {
            await _localStorageService.ClearAsync();
        }
    }
}
