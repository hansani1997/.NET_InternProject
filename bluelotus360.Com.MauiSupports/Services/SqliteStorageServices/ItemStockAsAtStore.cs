using bluelotus360.Com.MauiSupports.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.MauiSupports.Services.SqliteStorageServices
{
    public class ItemStockAsAtStore : IItemStockAsAtStore
    {
        public SQLiteAsyncConnection Database;

        public async Task<StockAsAt> GetContent(int user, int company, long project, long location, long element, long item)
        {
            await init();
            return await Database.Table<StockAsAt>().Where(i => i.User == user && i.Company == company && i.Project == project && i.Location == location && i.element == element && i.ItemCd == item).FirstOrDefaultAsync();
        }

        public async Task init()
        {
            if (Database is not null) return;
            Database = new SQLiteAsyncConnection(NativeConstants.DatabasePath, NativeConstants.flags);
            await Database.CreateTableAsync<StockAsAt>();
        }
        public async Task<int> SaveItemAsync(StockAsAt item)
        {
            await init();
            StockAsAt data = await this.GetContent(item.User, item.Company, item.Project, item.Location, item.element, item.ItemCd);
            if (data != null)
            {
                item.Id = data.Id;
                item.timestamp = DateTime.Now;
                return await Database.UpdateAsync(item);
            }
            else
            {
                item.timestamp = DateTime.Now;
                return await Database.InsertAsync(item);
            }
        }

        public async Task<List<StockAsAt>> GetAllContents()
        {
            await init();
            return await Database.Table<StockAsAt>().ToListAsync();
        }

        public async Task<int> DeleteAsync(StockAsAt item)
        {
            await init();
            if(item == null)
            {
                return await Database.DeleteAllAsync<StockAsAt>();
            }
            else
            {
                return await Database.DeleteAsync(item);
            }
        }
    }
}
