using BL10.CleanArchitecture.Domain.Entities.MaterData;
using BlueLotus.Com.Domain.Entity;
using bluelotus360.Com.MauiSupports.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.MauiSupports.Services.SqliteStorageServices
{
    public class ItemComboStore : IItemComboStore
    {
        public SQLiteAsyncConnection Database;

        public async Task init()
        {
            if (Database is not null) return;
            Database = new SQLiteAsyncConnection(NativeConstants.DatabasePath, NativeConstants.flags);
            await Database.CreateTableAsync<ItemComboResponses>();
        }
        public Task<int> DeleteAsync(ItemComboResponses item)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ItemComboResponses>> GetContents(int user, int company, long RequestingObject, string searchQuery,long ItemKey)
        {
            await init();
            if(RequestingObject == 0 && searchQuery.Length == 0 && ItemKey == 0)
            {
                return await Database.Table<ItemComboResponses>().Where(i => i.user == user && i.company == company).ToListAsync();
            }
            else if(searchQuery.Length == 0 && ItemKey == 0) {
                return await Database.Table<ItemComboResponses>().Where(i => i.user == user && i.company == company && i.ObjectKey == RequestingObject).ToListAsync();
            }
            else if(searchQuery.Length > 0)
            {
                return await Database.Table<ItemComboResponses>().Where(i => i.user == user && i.company == company && i.ObjectKey == RequestingObject && i.ItemName.ToUpper().Contains(searchQuery.ToUpper())).ToListAsync();
            }else
            {
                return await Database.Table<ItemComboResponses>().Where(i => i.user == user && i.company == company && i.ObjectKey == RequestingObject && i.ItemKey == ItemKey).ToListAsync();
            }
        }

        public async Task<int> SaveItemAsync(ItemComboResponses item)
        {
            await init();
            List<ItemComboResponses> data = await this.GetContents(item.user, item.company, item.ObjectKey,"",item.ItemKey);
            if (data.Count != 0)
            {
                item.ID = data[0].ID;
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
