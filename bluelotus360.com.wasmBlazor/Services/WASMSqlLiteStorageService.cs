using BL10.CleanArchitecture.Domain.Entities;
using bluelotus360.Com.MauiSupports.Models;
using bluelotus360.Com.MauiSupports.Services.SqliteStorageServices;
using Microsoft.JSInterop;
using System.Xml.Linq;
using TG.Blazor.IndexedDB;
using static MudBlazor.Colors;

namespace bluelotus360.com.wasmBlazor.Services
{
    public class WASMSqlLiteStorageService : ISqliteStorageService
    {
        private readonly IndexedDBManager _dbFactory;
        public WASMSqlLiteStorageService(IndexedDBManager dbFactory)
        {
            _dbFactory = dbFactory;
        }
        public async Task<int> DeleteItemAsync(IncomingStrings incomingString)
        {
            IncomingStrings incstring = await this.GetItemAsync(incomingString.company, incomingString.user, incomingString.name, incomingString.parameters);
            if (incstring!=null)
            {
                await _dbFactory.DeleteRecord("IncomingStrings",incomingString.ID);
                return 1;
            }
            else
            {
                return 0;
            }

            
        }

        public async Task<int> DeleteQueueItemAsync(RequestQueueItem incomingString)
        {
            RequestQueueItem incstring = new RequestQueueItem();

            var obj = await _dbFactory.GetRecords<RequestQueueItem>("RequestQueueItem");
            incstring = obj.Where(x => x.ID == incomingString.ID).FirstOrDefault();

            if (incstring != null)
                {
                    await _dbFactory.DeleteRecord("RequestQueueItem", incomingString.ID);
                    return 1;
                }
                else
                {
                    return 0;
                }
            
            return 0;

        }

        public async Task<List<RequestQueueItem>> GetAllQueueItemsAsync()
        {
            try
            {
                var obj=await _dbFactory.GetRecords<RequestQueueItem>("RequestQueueItem");

                if (obj != null)
                {
                    return obj;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<RequestQueueItem>> GetAllUnsyncedQueueItemsAsync(int cid, int uid)
        {
            try
            {
                var obj = await _dbFactory.GetRecords<RequestQueueItem>("RequestQueueItem");
                var unsynced = obj.Where(i => i.isSynced == 0).ToList();
                if (unsynced != null)
                {
                    return unsynced;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IncomingStrings> GetItemAsync(int cid, int uid, string name, string parameters)
        {
            try
            {
                var list = await _dbFactory.GetRecords<IncomingStrings>("IncomingStrings");
                var obj = list.Where(i => i.company == cid && i.user == uid && i.name == name && i.parameters == parameters).FirstOrDefault();
                if (obj != null)
                {
                    return obj;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public async Task<List<IncomingStrings>> GetItemsAsync(int cid, int uid)
        {
            try
            {
                var list = await _dbFactory.GetRecords<IncomingStrings>("IncomingStrings");
                var obj = list.Where(i => i.company == cid && i.user == uid).ToList();
                if (obj != null)
                {
                    return obj;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public async Task<int> SaveItemAsync(IncomingStrings incomingString)
        {
            try
            {
                IncomingStrings incstring = await this.GetItemAsync(incomingString.company, incomingString.user, incomingString.name, incomingString.parameters);
                

                    if (incstring != null)
                    {
                        var updateRecord = new StoreRecord<IncomingStrings>
                        {
                            Storename = "IncomingStrings",
                            Data = incstring
                        };

                        await _dbFactory.UpdateRecord(updateRecord);
                    }
                    else
                    {
                        var newRecord = new StoreRecord<IncomingStrings>
                        {
                            Storename = "IncomingStrings",
                            Data = incomingString
                        };

                        await _dbFactory.AddRecord(newRecord);
                    }
                    return 0;
                

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> GetQueueItemCount()
        {
            try
            {
                var obj = await _dbFactory.GetRecords<RequestQueueItem>("RequestQueueItem");
                return obj.Count();
               
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> GetUnsyncedQueueItemCount()
        {
            try
            {
                var obj = await _dbFactory.GetRecords<RequestQueueItem>("RequestQueueItem");
                var unsynced = obj.Where(i => i.isSynced == 0).ToList();
                return unsynced.Where(i => i.isSynced == 0).Count();

            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public async Task<int> SaveQueuItemAsync(RequestQueueItem incomingString)
        {
            try
            {
                var obj = await _dbFactory.GetRecords<RequestQueueItem>("RequestQueueItem");
                RequestQueueItem incstring = obj.Where(x => x.ID == incomingString.ID).FirstOrDefault();


                if (incstring != null)
                {
                    var updateRecord = new StoreRecord<RequestQueueItem>
                    {
                        Storename = "RequestQueueItem",
                        Data = incstring
                    };

                    await _dbFactory.UpdateRecord(updateRecord);
                }
                else
                {
                    var newRecord = new StoreRecord<RequestQueueItem>
                    {
                        Storename = "RequestQueueItem",
                        Data = incomingString
                    };

                    await _dbFactory.AddRecord(newRecord);
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
