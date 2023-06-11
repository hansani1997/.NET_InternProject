using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using bluelotus360.Com.MauiSupports.Models;
using bluelotus360.Com.MauiSupports;

namespace bluelotus360.Com.MauiSupports.Services.SqliteStorageServices
{
    public class SqliteStorageService : ISqliteStorageService
    {
        SQLiteAsyncConnection Database;

        public SqliteStorageService()
        {
            //
        }
        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(NativeConstants.DatabasePath,NativeConstants.flags);
            await Database.CreateTableAsync<IncomingStrings>();
            await Database.CreateTableAsync<RequestQueueItem>();
        }
        public async Task<IncomingStrings> GetItemAsync(int cid,int uid,string name,string parameters)
        {
            await Init();
            return await Database.Table<IncomingStrings>().Where(i => i.company == cid && i.user == uid && i.name == name && i.parameters == parameters ).FirstOrDefaultAsync();
        }
        public async Task<int> SaveItemAsync(IncomingStrings incomingString)
        {
            await Init();
            IncomingStrings incstring = await this.GetItemAsync(incomingString.company,incomingString.user,incomingString.name,incomingString.parameters);
            if(incstring != null)
            {
                incomingString.ID= incstring.ID;
                return await Database.UpdateAsync(incomingString);
            }
            else
            {
                var res = await Database.InsertAsync(incomingString);
                return res;
            }
        }
        public async Task<int> DeleteItemAsync(IncomingStrings incoming)
        {
            await Init();
            return await Database.DeleteAsync(incoming);
        }

        public async Task<List<RequestQueueItem>> GetAllQueueItemsAsync()
        {
            await Init();
            return await Database.Table<RequestQueueItem>().ToListAsync();
        }

        public async Task<int> GetQueueItemCount()
        {
            await Init();
            return await Database.Table<RequestQueueItem>().CountAsync();
        }

        public async Task<int> SaveQueuItemAsync(RequestQueueItem incomingString)
        {
            await Init();
            if(incomingString.ID != 0)
            {
                return await Database.UpdateAsync(incomingString);
            }
            else
            {
                return await Database.InsertAsync(incomingString);
            }
        }

        public async Task<int> DeleteQueueItemAsync(RequestQueueItem incoming)
        {
            await Init();
            return await Database.DeleteAsync(incoming);
        }

        public async Task<List<RequestQueueItem>> GetAllUnsyncedQueueItemsAsync(int cid, int uid)
        {
            await Init();
            return await Database.Table<RequestQueueItem>().Where(i => i.isSynced == 0 && i.User == uid && i.Company == cid).ToListAsync();
        }

        public async Task<int> GetUnsyncedQueueItemCount()
        {
            await Init();
            return await Database.Table<RequestQueueItem>().Where(i => i.isSynced == 0).CountAsync();
        }

        public async Task<List<IncomingStrings>> GetItemsAsync(int cid, int uid)
        {
            await Init();
            return await Database.Table<IncomingStrings>().Where(i => i.company== cid && i.user == uid).ToListAsync();
        }
    }
}
