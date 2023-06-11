using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.MauiSupports.Services.MAUISecureStorage
{
    public interface ISecureStorageService
    {
        void SetItem(string key, string value);
        Task<string> GetItem(string key);
        bool RemoveItem(string key);
        void RemoveAllItems();
        ValueTask<T> GetItemAsync<T>(string key);
        ValueTask SetItemAsync<T>(string key, T data);
        bool RemoveItemsAsync(string key);
        ValueTask<string> GetItemAsStringAsync(string key);
        ValueTask<bool> ContainKeyAsync(string key);
        //Task GetItemAsync<T>(T userImageURL);
    }
}
