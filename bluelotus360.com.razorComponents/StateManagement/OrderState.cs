using BL10.CleanArchitecture.Domain.DTO.Object;
using BL10.CleanArchitecture.Domain.Entities.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.StateManagement
{
    public enum WindowDisplayMode
    {
        LandingProgress,
        CategoryListView,
        CategoryView,
        ProductView,
        CustomerView,
        CartSummaryView
    }
    public class OrderState
    {

        public AddressResponse SelectedCustomer { get; set; }
        public bool IsCustomerSelected { get; set; }

        public IList<AddressResponse> Customers { get; set; }
        public IList<CodeBase> PayementTerms { get; set; }

        public WindowDisplayMode DisplayMode { get; set; }

        public OrderState()
        {
            Items=new List<ItemRateResponse>();
            TaxCalc = new();
            InitilizeNewOrder();
        }

        public CodeBase SelectedCategory { get; set; }
        public CodeBaseResponse SelectedLocation { get; set; }

        public long OrderPageElementKey { get; set; } = 1;

        public EventCallback NotifyUIStateChange { get; set; }

        public EventCallback OnMenuLoadingDone { get; set; }




        public Order CurrentOrder { get; set; }

        public TaxCalculator TaxCalc { get; set; }

        public MenuItem DefaultMenuItem { get; set; }

        public void InitilizeNewOrder()
        {
            TotalProducts = 0;
            CurrentOrder = new Order();
            if (OrderUIDefintion != null)
            {
                CurrentOrder.FormObjectKey = OrderUIDefintion.ElementKey;
            }
        }

        public decimal TotalProducts { get; set; } = 0;

        public void AddNewItemToOrder()
        {
            if (CurrentOrder != null)
            {

            }
        }

        public async Task CalculateTotatls()
        {

            TotalProducts = CurrentOrder.GetQuantityTotal();
            if (NotifyUIStateChange.HasDelegate)
            {
                await NotifyUIStateChange.InvokeAsync();
            }
        }


        public BLUIElement OrderUIDefintion { get; set; }

        public IList<ItemRateResponse> Items { get; set; }  



    }
}
