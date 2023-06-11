using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Services.Definition
{
    public interface IStorageService
    {
        ValueTask<bool> ContainKeyAsync(string key);
        Task<string> GetItem(string key);
        Task<T> GetItemAsync<T>(string key);
        Task SetItem<T>(string key, T item);
        ValueTask SetItemAsync<T>(string key, T data);
        Task RemoveItem(string key);
        Task RemoveAll();
    }
}
