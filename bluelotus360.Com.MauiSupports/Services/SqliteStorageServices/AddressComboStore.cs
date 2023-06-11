using bluelotus360.Com.MauiSupports.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.MauiSupports.Services.SqliteStorageServices
{
    public class AddressComboStore : IAddressComboStore
    {
        public SQLiteAsyncConnection Database;

        public async Task init()
        {
            if (Database is not null) return;
            Database = new SQLiteAsyncConnection(NativeConstants.DatabasePath, NativeConstants.flags);
            await Database.CreateTableAsync<AddressComboModel>();
        }
        public Task<int> DeleteAsync(AddressComboModel item)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AddressComboModel>> GetContents(int user, int company, long RequestingObject, string searchQuery, long ItemKey)
        {
            await init();
            List<AddressComboModel> list = new List<AddressComboModel>();
            if (RequestingObject == 0 && searchQuery.Length == 0 && ItemKey == 0)
            {
                list = await Database.Table<AddressComboModel>().Where(i => i.User == user && i.Company == company && i.isPushed == true && i.isNew == false).ToListAsync();
            }
            else if (searchQuery.Length == 0 && ItemKey == 0)
            {
                list = await Database.Table<AddressComboModel>().Where(i => i.User == user && i.Company == company && i.RequestingElement == RequestingObject).ToListAsync();
            }
            else if (searchQuery.Length > 0)
            {
                list = await Database.Table<AddressComboModel>().Where(i => i.User == user && i.Company == company && i.RequestingElement == RequestingObject && i.AddressName.ToUpper().Contains(searchQuery.ToUpper())).ToListAsync();
            }
            else
            {
                list = await Database.Table<AddressComboModel>().Where(i => i.User == user && i.Company == company && i.RequestingElement == RequestingObject && i.AddressKey == ItemKey).ToListAsync();
            }

            //var nl = await Database.Table<AddressComboModel>().Where(i=>i.isPushed == false && i.isNew == true && i.User == user && i.Company == company).ToListAsync();
            //nl.Concat(list);

            return list;
        }

        public async Task<int> SaveItemAsync(AddressComboModel item)
        {
            await init();
            List<AddressComboModel> data = await this.GetContents(item.User, item.Company, item.RequestingElement, "", item.AddressKey);
            if (data.Count != 0)
            {
                item.Id = data[0].Id;
                item.timestamp = DateTime.Now;
                return await Database.UpdateAsync(item);
            }
            else
            {
                item.timestamp = DateTime.Today;
                return await Database.InsertAsync(item);
            }
        }
    }
}
