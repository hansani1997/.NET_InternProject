using bluelotus360.com.razorComponents.StateManagement;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Components.Order
{
    public partial class CartDetailView
    {

        private bool IsOrderBeingSaved;
        private bool IsClearDisabled;

        public async Task GoBackToListView()
        {

            _orderState.DisplayMode = StateManagement.WindowDisplayMode.CategoryListView;

            if (_orderState.NotifyUIStateChange.HasDelegate)
            {
                await _orderState.NotifyUIStateChange.InvokeAsync();
            }

            await Task.CompletedTask;

        }

        public async Task OnSaveButtonClick()
        {
            IsOrderBeingSaved = true;
            IsClearDisabled = true;
            await _orderManager.SaveOrder(_orderState.CurrentOrder);
            _snackbar.Add($"Sales Order {_orderState.CurrentOrder.OrderNumber} Has been saved  successfully", Severity.Success);
            IsClearDisabled=false;
            await Task.CompletedTask;
            
        }




        public async Task ClarCart()
        {

            _orderState.InitilizeNewOrder();
            IsOrderBeingSaved = false;
            _orderState.IsCustomerSelected = false;
            _orderState.SelectedCustomer = null;
            _orderState.DisplayMode = WindowDisplayMode.CategoryListView;
       
            if (_orderState.NotifyUIStateChange.HasDelegate)
            {
                await _orderState.NotifyUIStateChange.InvokeAsync();
            }
            await Task.CompletedTask;
        }
    }
}
