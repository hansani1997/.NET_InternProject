using bluelotus360.Com.MauiSupports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.MauiSupports.Services.SqliteStorageServices
{
    public interface IItemStockAsAtStore
    {
        public Task<int> SaveItemAsync(StockAsAt item);
        public Task<StockAsAt> GetContent(int user, int company, long project, long location, long element, long item);
        public Task<List<StockAsAt>> GetAllContents();
        public Task<int> DeleteAsync(StockAsAt item);
    }
}
