using bluelotus360.Com.MauiSupports.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.MauiSupports.Services.SqliteStorageServices
{
    public class ComboEventStorage
    {
        SQLiteAsyncConnection Database;
        public ComboEventStorage()
        {
            //
        }
        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(NativeConstants.DatabasePath, NativeConstants.flags);
            await Database.CreateTableAsync<ComboInteraction>();
        }

        public async Task<List<ComboInteraction>> GetInteractionsToList()
        {
            await Init();
            return await Database.Table<ComboInteraction>().ToListAsync();
        }

        //insert new combo interaction
        public async Task<int> SaveItemAsync(ComboInteraction comboInteraction)
        {
            await Init();
            return await Database.InsertAsync(comboInteraction);
        }
        //delete combo interaction by id
        public async Task<int> DeleteItemAsync(ComboInteraction comboInteraction)
        {
            await Init();
            return await Database.DeleteAsync(comboInteraction);
        }
    }
}
