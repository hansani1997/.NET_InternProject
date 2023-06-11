using bluelotus360.Com.MauiSupports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.MauiSupports.Services.SqliteStorageServices
{
    public interface ISqliteStorageService
    {
        public Task<IncomingStrings> GetItemAsync(int cid, int uid, string name, string parameters);
        public Task<List<IncomingStrings>> GetItemsAsync(int cid, int uid);
        public Task<int> SaveItemAsync(IncomingStrings incomingString);
        public Task<int> DeleteItemAsync(IncomingStrings incoming);
        public Task<List<RequestQueueItem>> GetAllQueueItemsAsync();
        public Task<List<RequestQueueItem>> GetAllUnsyncedQueueItemsAsync(int cid,int uid);
        public Task<int> GetQueueItemCount();
        public Task<int> GetUnsyncedQueueItemCount();
        public Task<int> SaveQueuItemAsync(RequestQueueItem incomingString);
        public Task<int> DeleteQueueItemAsync(RequestQueueItem incoming);
    }
}
