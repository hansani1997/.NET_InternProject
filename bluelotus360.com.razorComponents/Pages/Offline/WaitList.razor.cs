using BL10.CleanArchitecture.Domain.Entities.ItemProfleMobile;
using bluelotus360.Com.commonLib.Managers.TransactionManager;
using bluelotus360.Com.MauiSupports.Models;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using MudBlazor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.ReportViewer.Blazor;

namespace bluelotus360.com.razorComponents.Pages.Offline
{
    public partial class WaitList
    {
        private IEnumerable<RequestQueueItem> _items;
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        protected override async Task OnInitializedAsync()
        {
            _items = await _sqliteStorageService.GetAllQueueItemsAsync();
            await base.OnInitializedAsync();
        }

        private async Task ReloadContent()
        {
            _items = await _sqliteStorageService.GetAllQueueItemsAsync();
        }

        private void selectItem(RequestQueueItem rqi)
        {
        }
        private async void syncNow(RequestQueueItem item)
        {
            string uid = await _localStorage.GetItem("UID");
            string cid = await _localStorage.GetItem("CID");
            int uId = Int32.Parse(uid);
            int cId = Int32.Parse(cid);
            string[] seperated = item.url.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (seperated.Length > 1)
            {
                switch (item.url)
                {
                    case "Transaction.SaveTransaction":
                        BLTransaction transaction = JsonConvert.DeserializeObject<BLTransaction>(item.requestBody);
                        await _transactionManager.SaveTransaction(transaction);
                        break;
                    case "Order.CreateGenericOrder":
                        Order order = JsonConvert.DeserializeObject<Order>(item.requestBody);
                        await _orderManager.SaveOrder(order);
                        break;
                }
            }
        }
    }
}
