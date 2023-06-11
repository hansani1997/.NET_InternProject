using bluelotus360.com.mauiBlazor.Services;
using bluelotus360.Com.commonLib.Managers.TransactionManager;
using bluelotus360.Com.MauiSupports.Models;
using bluelotus360.Com.MauiSupports.Services.SqliteStorageServices;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.mauiBlazor.SyncPaths
{
    public class OrderSync
    {
        AutoTransactionManager autoTransactionManager;
        private static OrderSync orderSync;
        SqliteStorageService sqliteStorageService;

        private OrderSync()
        {
            autoTransactionManager= new AutoTransactionManager(new SecureStorageService());
            sqliteStorageService= new SqliteStorageService();
        }

        public static OrderSync getInstance()
        {
            if(orderSync == null)
            {
                orderSync = new OrderSync();
            }
            return orderSync;
        }

        public async Task SyncSavedData(RequestQueueItem itm, string type)
        {
            switch (type)
            {
                case "CreateGenericOrder":
                    await SaveOrder(itm);
                    break;
            }
        }
        private async Task SaveOrder(RequestQueueItem itm)
        {
            Order order = JsonConvert.DeserializeObject<Order>(itm.requestBody);
            bool states = await autoTransactionManager.SaveOrder(order);
            if (states)
            {
                itm.isSynced = 1;
                await sqliteStorageService.SaveQueuItemAsync(itm);
                Debug.WriteLine("Synces Successful");
            }
            else
            {
                Debug.WriteLine("Syncronization Failed!");
            }
        }
    }
}
