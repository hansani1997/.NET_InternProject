using BL10.CleanArchitecture.Domain.Entities;
using BL10.CleanArchitecture.Domain.Entities.WorkShopManagement;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.Com.Infrastructure.Managers.OrderManager;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.Pages.OrderHub.Component
{
    public partial class GetMoreOrderInformation
    {
        [Parameter]
        public EventCallback OnCloseButtonClick { get; set; }
        [Parameter] public bool WindowIsVisible { get; set; } = true;
        [Parameter] public IList<PartnerOrderDetails> _Order { get; set; }
        [Parameter] public PartnerOrder selectedOrder { get; set; }

        public GetMoreOrderInformation()
        {
            _Order= new List<PartnerOrderDetails>();
            selectedOrder = new PartnerOrder();
        }

        private async void OnCloseClick()
        {
            if (OnCloseButtonClick.HasDelegate)
            {
                await OnCloseButtonClick.InvokeAsync();
            }

        }

        public void GetSingleOrder(PartnerOrder Order)
        {
            selectedOrder = Order;
            _Order = selectedOrder.OrderItemDetails;
            decimal TotalWithoutDiscount = _Order.Sum(x => x.BaseTotalPrice);
            selectedOrder.TotalWithDiscount = TotalWithoutDiscount - selectedOrder.DiscountAmount;
        }


    }


}
