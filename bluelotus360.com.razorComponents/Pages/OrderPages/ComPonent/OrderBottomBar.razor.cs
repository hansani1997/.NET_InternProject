using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Pages.OrderPages.ComPonent
{
    public partial class OrderBottomBar
    {
        public void Refresh()
        {
            StateHasChanged();
        }
        public async Task OnCartClick()
        {
            await Task.CompletedTask;
            _orderState.DisplayMode = StateManagement.WindowDisplayMode.CartSummaryView;
            if (_orderState.NotifyUIStateChange.HasDelegate)
            {
                await _orderState.NotifyUIStateChange.InvokeAsync();
            }
        }

        public async Task OnClearCustomerClick()
        {
            //_orderState.IsCustomerSelected = false;
            //_orderState.SelectedCustomer = null;

            //if (_orderState.NotifyUIStateChange.HasDelegate)
            //{
            //    await _orderState.NotifyUIStateChange.InvokeAsync();
            //}
            //await Task.CompletedTask;
        }
    }
}
