using bluelotus360.Com.MauiSupports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.MauiSupports.Services.SqliteStorageServices
{
    public interface IItemComboStore
    {
        public Task<int> SaveItemAsync(ItemComboResponses item);
        public Task<List<ItemComboResponses>> GetContents(int user, int company,long RequestingObject,string searchQuery,long ItemKey);
        public Task<int> DeleteAsync(ItemComboResponses item);
    }
}
