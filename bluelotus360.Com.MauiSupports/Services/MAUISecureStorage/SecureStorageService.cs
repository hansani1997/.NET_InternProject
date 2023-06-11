using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.Com.Application.Interfaces.Services;
using BlueLotus360.Com.Application.Interfaces.Services.Storage;
using BlueLotus360.Com.Application.Interfaces.Services.Storage.Provider;
using Newtonsoft.Json;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.MauiSupports.Services.MAUISecureStorage 
{
    public class SecureStorageService /*: ISecureStorageService*/
    {
        private Dictionary<string, string> _storage = new();

        public async Task<string> GetItem(string key)
        {
            string result = await SecureStorage.Default.GetAsync(key);
            return result;
        }

        public void RemoveAllItems()
        {
            SecureStorage.Default.RemoveAll();
        }

        public bool RemoveItem(string key)
        {
            return SecureStorage.Default.Remove(key);
        }

        public async void SetItem(string key, string value)
        {
            await SecureStorage.Default.SetAsync(key, value);
        }
        public bool RemoveItemsAsync(string key) => SecureStorage.Default.Remove(key);
        public async ValueTask<T> GetItemAsync<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            var serialisedData = await SecureStorage.Default.GetAsync(key).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(serialisedData))
                return default;

            try
            {
                return JsonConvert.DeserializeObject<T>(serialisedData);
            }
            catch (JsonException e)
            {
                return (T)(object)serialisedData;
            }
        }

        public async ValueTask SetItemAsync<T>(string key, T data)
        {

            var serialisedData = JsonConvert.SerializeObject(data);
            await SecureStorage.Default.SetAsync(key, serialisedData).ConfigureAwait(false);


        }

        public ValueTask<string> GetItemAsStringAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return ValueTask.FromResult(SecureStorage.Default.GetAsync(key).Result);
        }

        public async ValueTask<bool> ContainKeyAsync(string key)
        {
            try
            {
                string token = await SecureStorage.GetAsync(key);
                return !string.IsNullOrEmpty(token);
            }
            catch (Exception ex)
            {
                // Handle the exception here
                return false;
            }

        }
    }
}
