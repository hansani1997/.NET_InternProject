using BL10.CleanArchitecture.Domain.Entities.Booking;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Routes;
using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components
{
    public partial class BLCustomerSelectCombo : IBLUIOperationHelper
    {
        [Parameter]
        public BLUIElement UIElement { get; set; }

        [Parameter]
        public object ComboDataObject { get; set; }

        [Parameter]
        public EventCallback OnComboChanged { get; set; }

        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }

        [Parameter]
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }


        private AddressResponse selectedAddress = new AddressResponse { AddressName=""};

        public IList<AddressResponse> AddressResponse { get; set; }

        private PropertyConversionResponse<AddressResponse> conversionInfo;

        private bool isEditable = true;

        private string css_class = "";

        private BookingDetails Details { get; set; }

        public BLUIElement LinkedUIObject { get; private set; }


        protected override async Task OnInitializedAsync()
        {
            await ReadComboData();
            css_class = (UIElement.IsVisible ? "d-flex" : "d-none") + " align-end " + UIElement.ParentCssClass;
            if (ObjectHelpers != null)
            {
                if (ObjectHelpers.ContainsKey(UIElement.ElementName))
                {
                    ObjectHelpers.Remove(UIElement.ElementName);

                }
                ObjectHelpers.Add(UIElement.ElementName, this);
            }
            await base.OnInitializedAsync();
        }

        private async Task ReadComboData(string value = "")
        {
            ComboRequestDTO requestDTO = new ComboRequestDTO();
            IList<AddressResponse> CustomerList = new List<AddressResponse>();
            CustomerList.Add(selectedAddress);

            if (ComboDataObject != null) 
            {
                requestDTO.RequestingElementKey = UIElement.ElementKey;
                requestDTO.RequestingURL = BaseEndpoint.BaseURL + UIElement.GetPathURL();
                requestDTO.SearchQuery = value;

                Details = (BookingDetails)ComboDataObject;

                IList<CustomerDetailsByVehicle> cusDetails = new List<CustomerDetailsByVehicle>();

                cusDetails = Details.CustomersDetails;

                foreach (CustomerDetailsByVehicle responses in cusDetails)
                {
                    CustomerList.Add(responses.Customer);
                }
            }

            AddressResponse = CustomerList;

            if (AddressResponse.Count > 0 && !UIElement.IsServerFiltering)
            {
                selectedAddress = this.AddressResponse.FirstOrDefault();

                if (selectedAddress != null)
                {
                    selectedAddress.IsMust = UIElement.IsMust;
                    await NotifyHooks(selectedAddress);

                }
                else
                {
                    var cd = new AddressResponse();
                    cd.IsMust = UIElement.IsMust;
                    await NotifyHooks(cd);
                }

                StateHasChanged();
            }


        }

        private async Task OnComboValueChanged(AddressResponse addressResponse)
        {
            addressResponse = await NotifyHooks(addressResponse);

        }

        private async Task<AddressResponse> NotifyHooks(AddressResponse addressResponse)
        {
            ComboDataObject.SetValueByObjectPath(UIElement.DefaultAccessPath, addressResponse);

            UIInterectionArgs<AddressResponse> args = new UIInterectionArgs<AddressResponse>();
            if (InteractionLogics != null && InteractionLogics.Count > 0)
            {
                EventCallback callback;
                if (UIElement.OnClickAction != null && InteractionLogics.TryGetValue(UIElement.OnClickAction, out callback))
                {
                    if (callback.HasDelegate)
                    {
                        args.Caller = this.UIElement.OnClickAction;
                        args.ObjectPath = this.UIElement.DefaultAccessPath;
                        args.DataObject = addressResponse;
                        args.sender = this;
                        args.InitiatorObject = UIElement;
                        await callback.InvokeAsync(args);
                    }
                }
            }

            if (!(args.DelegateExecuted && args.CancelChange))
            {
                ComboDataObject.SetValueByObjectPath(UIElement.DefaultAccessPath, addressResponse);
                selectedAddress = addressResponse;
                StateHasChanged();
            }
            else
            {

                if (args.OverrideValue)
                {
                    addressResponse = args.OverriddenValue;
                    selectedAddress = addressResponse;
                    StateHasChanged();
                }

            }

            return addressResponse;
        }

        protected override Task OnParametersSetAsync()//The synchronous and asynchronous way of setting the parameter when the component receives the parameter from its parent component.
        {

            int c = this.ComboDataObject.GetHashCode();
            conversionInfo = ComboDataObject.GetPropObject<AddressResponse>(UIElement.DefaultAccessPath);
            if (conversionInfo.IsConversionSuccess)
            {
                selectedAddress = conversionInfo.Value;
            }
            return base.OnParametersSetAsync();

        }

        private async Task<IEnumerable<AddressResponse>> OnComboSearch(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.


            // if text is null or empty, don't return values (drop-down will not open)
            if (string.IsNullOrEmpty(value))
            {
                return AddressResponse;

            }

            if (UIElement.IsServerFiltering && value.Length > 2)
            {
                await ReadComboData(value);
            }

            return AddressResponse.Where(x => x.AddressName.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        public void ResetToInitialValue()
        {
            this.selectedAddress = new AddressResponse();
            this.StateHasChanged();
        }

        public void UpdateVisibility(bool IsVisible)
        {
            throw new NotImplementedException();
        }

        public void ToggleEditable(bool IsEditable)
        {
            isEditable = IsEditable;
            StateHasChanged();
        }

        public async Task Refresh()
        {
            if (this.AddressResponse != null)
            {
                PropertyConversionResponse<AddressResponse> conversions = ComboDataObject.GetPropObject<AddressResponse>(this.UIElement.DefaultAccessPath);
                if (conversions.IsConversionSuccess)
                {
                    selectedAddress = AddressResponse.Where(x => x.AddressKey == conversions.Value.AddressKey).FirstOrDefault();
                    await NotifyHooks(selectedAddress);
                }

            }
        }

        public Task FocusComponentAsync()
        {
            throw new NotImplementedException();
        }

        public async Task SetValue(object value)
        {
            await NotifyHooks(value as AddressResponse);

        }

        public async Task FetchData(bool useLocalstorage = false)
        {
            await ReadComboData();
        }

        public Task SetDataSource(object DataSource)
        {
            throw new NotImplementedException();
        }
    } 
}
