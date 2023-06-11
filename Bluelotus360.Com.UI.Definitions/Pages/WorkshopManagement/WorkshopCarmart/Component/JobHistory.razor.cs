using BL10.CleanArchitecture.Domain.Entities.WorkShopManagement;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.Pages.WorkshopManagement.WorkshopCarmart.Component
{
    public partial class JobHistory
    {
        [Parameter] public WorkOrder DataObject { get; set; }
        [Parameter] public long ObjectKey { get; set; }
        [Parameter] public EventCallback<OrderOpenRequest> LoadWorkOrder { get; set; }
        
        bool isloading;
       
        private async void OpenWorkOrder(long ordKy,long trnKy)
        {
            OrderOpenRequest req = new OrderOpenRequest() { OrderKey=ordKy,ObjKy= ObjectKey,TransactionKey=trnKy };
            
            if (LoadWorkOrder.HasDelegate)
            {
                await LoadWorkOrder.InvokeAsync(req);
            }

        }
    }
}
