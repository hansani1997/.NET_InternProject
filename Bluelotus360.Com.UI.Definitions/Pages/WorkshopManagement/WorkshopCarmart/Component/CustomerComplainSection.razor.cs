using BL10.CleanArchitecture.Domain.Entities.WorkShopManagement;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace BlueLotus360.Com.UI.Definitions.Pages.WorkshopManagement.WorkshopCarmart.Component
{
    public partial class CustomerComplainSection
    {
        [Parameter] public IList<OrderItem> CustomerComplains { get; set; }
        private TelerikGrid<OrderItem> GridRef { get; set; }
        [Parameter]public IEnumerable<OrderItem> SelectedItems { get; set; } = Enumerable.Empty<OrderItem>();

        protected override async Task OnParametersSetAsync()
        {
            
            await base.OnParametersSetAsync();
        }
        public void GridRefresh()
        {
            GridRef?.Rebind();
        }
        async Task DeleteHandler(GridCommandEventArgs args)
        {
            OrderItem item = (OrderItem)args.Item;
            var index = CustomerComplains.ToList().FindIndex(x => x.LineNumber == item.LineNumber);
            CustomerComplains[index].IsActive = 0;

            GridRefresh();
            StateHasChanged();
            Console.WriteLine("Delete event is fired.");
        }


        void EditHandler(GridCommandEventArgs args)
        {
            OrderItem item = (OrderItem)args.Item;
        }

        async Task UpdateHandler(GridCommandEventArgs args)
        {
            OrderItem item = (OrderItem)args.Item;
            var index = CustomerComplains.ToList().FindIndex(x => x.LineNumber == item.LineNumber);
            if (index != -1)
            {
                CustomerComplains[index] = item;
            }

            GridRefresh();
            StateHasChanged();
            Console.WriteLine("Update event is fired.");
        }
    }
}
