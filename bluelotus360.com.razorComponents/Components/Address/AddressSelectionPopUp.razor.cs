using BL10.CleanArchitecture.Domain.DTO.Object;
using bluelotus360.Com.commonLib.Routes;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MudBlazor.CategoryTypes;

namespace bluelotus360.com.razorComponents.Components.Address
{
    public partial class AddressSelectionPopUp
    {
        [Parameter]
        public BLUIElement AddressUIObject { get; set; }


        private MudTable<AddressResponse> mudTable;
        IList<AddressResponse> addresses;


        private string SearchQuery = string.Empty;

        private int selectedRowNumber = -1;
        protected override async Task OnInitializedAsync()
        {
            await ReadAddress();
            await base.OnInitializedAsync();
        }

        private void RowClickEvent(TableRowClickEventArgs<AddressResponse> tableRowClickEventArgs)
        {
            _orderState.SelectedCustomer = tableRowClickEventArgs.Item;
            _orderState.CurrentOrder.OrderCustomer=tableRowClickEventArgs.Item;
            _orderState.IsCustomerSelected = true;
        }
        public async Task ReadAddress(bool Reload=false)
        {

            if (AddressUIObject != null)
            {
                if (_orderState.Customers == null || Reload)
                {
                    ComboRequestDTO requestDTO = new();
                    requestDTO.RequestingURL = BaseEndpoint.BaseURL + AddressUIObject.GetPathURL();
                    requestDTO.RequestingElementKey = AddressUIObject.ElementKey;
                    _orderState.Customers = await _comboManager.GetAddressResponses(requestDTO);
                    
                }
            }
        }

        private bool SearchFunction(AddressResponse element) => FilterFunc(element, SearchQuery);

        private bool FilterFunc(AddressResponse address, string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return true;
            }
            if (address != null)
            {
                return address.AddressName!=null && address.AddressName.Contains(searchString);
            }
            return false;
        }


        private string SelectedRowClassFunc(AddressResponse element, int rowNumber)
        {
            if (selectedRowNumber == rowNumber)
            {
                selectedRowNumber = -1;
              
                return string.Empty;
            }
            else if (mudTable.SelectedItem != null && mudTable.SelectedItem.Equals(element))
            {
                selectedRowNumber = rowNumber;
              
                return "selected";
            }
            else
            {
                return string.Empty;
            }
        }

    }
}
