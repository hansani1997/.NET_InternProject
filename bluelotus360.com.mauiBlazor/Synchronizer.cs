using bluelotus360.com.mauiBlazor.Services;
using bluelotus360.com.mauiBlazor.SyncPaths;
using bluelotus360.Com.commonLib.Services.Definition;
using bluelotus360.Com.MauiSupports.Models;
using bluelotus360.Com.MauiSupports.Services.ConnectionStates;
using bluelotus360.Com.MauiSupports.Services.SqliteStorageServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.mauiBlazor
{
    public class Synchronizer
    {
        private static SyncHandler syncHandler;

        public Synchronizer() {
            
        }
        public static async void StartSynchronization(Object obj)
        {
            syncHandler = new SyncHandler();
            CancellationToken cancellationToken = (CancellationToken)obj;
            IConnectionState connectionState = new ConnectionState();
            while (!cancellationToken.IsCancellationRequested)
            {

                if (connectionState.IsConnected())
                {
                    bool syncRes = await syncHandler.SyncAvailability();
                    if(syncRes)
                    {
                        Debug.WriteLine("Synchronizing...");
                        await syncHandler.Sync();
                        Debug.WriteLine("Successfully Synchronizes!");
                    }
                    else
                    {
                        Debug.WriteLine("Nothing to sync");
                    }
                }
                else
                {
                    Debug.WriteLine("Disconnected!");
                }
                Thread.Sleep(5000);
            }
        }
    }

    public class SyncHandler
    {
        SqliteStorageService sqliteStorageService;
        TransactionSync syncTrans;
        OrderSync orderSync;
        //private ISecureStorageService _localStorage;
        private IStorageService _localStorage;

        public SyncHandler()
        {
            sqliteStorageService = new SqliteStorageService();
            _localStorage = new SecureStorageService();
        }
        public async Task<bool> SyncAvailability()
        {
            int queuedItems = await sqliteStorageService.GetUnsyncedQueueItemCount();
            if (queuedItems > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task Sync()
        {
            string uid = await _localStorage.GetItem("UID");
            string cid = await _localStorage.GetItem("CID");
            int uId = Int32.Parse(uid);
            int cId = Int32.Parse(cid);
            List<RequestQueueItem> reqQitem = await sqliteStorageService.GetAllUnsyncedQueueItemsAsync(cId,uId);
            if (reqQitem != null)
            {
                int itemCount = reqQitem.Count;
                int i = 1;
                HttpClient httpClient = new HttpClient();
                foreach (RequestQueueItem item in reqQitem)
                {
                    Debug.WriteLine($"{i} Items synced from {itemCount} items");
                    string[] seperated = item.url.Split( new char[] { '.' },StringSplitOptions.RemoveEmptyEntries ); 
                    if(seperated.Length > 1)
                    {
                        switch (seperated[0])
                        {
                            case "Transaction":
                                syncTrans = TransactionSync.GetInstance();
                                await syncTrans.SyncSavedData(item, seperated[1]);
                                break;
                            case "Order":
                                orderSync = OrderSync.getInstance();
                                await orderSync.SyncSavedData(item, seperated[1]);
                                break;
                        }
                    }
                    i++;
                }
            }
        }
    }
}
