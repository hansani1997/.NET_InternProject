using bluelotus360.Com.MauiSupports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.MauiSupports.Services.SqliteStorageServices
{
    public interface IAddressComboStore
    {
        public Task<int> SaveItemAsync(AddressComboModel item);
        public Task<List<AddressComboModel>> GetContents(int user, int company, long RequestingObject, string searchQuery, long ItemKey);
        public Task<int> DeleteAsync(AddressComboModel item);
    }
}
