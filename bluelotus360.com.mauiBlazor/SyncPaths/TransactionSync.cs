using bluelotus360.com.mauiBlazor.Services;
using bluelotus360.Com.commonLib.Managers.TransactionManager;
using bluelotus360.Com.MauiSupports.Models;
using bluelotus360.Com.MauiSupports.Services.SqliteStorageServices;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.mauiBlazor.SyncPaths
{
    public class TransactionSync
    {
        AutoTransactionManager autoTransactionManager;
        private static TransactionSync transactionSync;
        SqliteStorageService sqliteStorageService;
        private TransactionSync() { 
            autoTransactionManager = new AutoTransactionManager(new SecureStorageService());
            sqliteStorageService = new SqliteStorageService();
        }

        public static TransactionSync GetInstance()
        {
            if(transactionSync == null)
            {
                transactionSync = new TransactionSync();
            }
            return transactionSync;
        }

        public async Task SyncSavedData(RequestQueueItem itm,string type)
        {
            switch(type)
            {
                case "SaveTransaction":
                    await SaveTransaction(itm);
                    break;
            }
        }

        private async Task SaveTransaction(RequestQueueItem itm)
        {
            BLTransaction transaction = JsonConvert.DeserializeObject<BLTransaction>(itm.requestBody);
            ExtendedTransaction trans = await autoTransactionManager.SaveTransaction(transaction);
            if (trans.isSavedOnline)
            {
                itm.isSynced = 1;
                await sqliteStorageService.SaveQueuItemAsync(itm);
                Debug.WriteLine("Successfully Synced!");
            }
            else
            {
                Debug.WriteLine("Not Successfully Synced!");
            }
        }
    }
}
