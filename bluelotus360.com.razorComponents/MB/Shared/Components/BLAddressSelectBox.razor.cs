using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bluelotus360.com.razorComponents.Extensions;
using bluelotus360.Com.commonLib.Routes;

namespace bluelotus360.com.razorComponents.MB.Shared.Components
{
    public partial class BLAddressSelectBox : IBLUIOperationHelper, IBLServerDependentComponent
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


        private IList<AddressResponse> selectedAddress = new List<AddressResponse>();

        public IList<AddressResponse> AddressResponse { get; set; }

        private PropertyConversionResponse<IList<AddressResponse>> conversionInfo;

        private bool isEditable = true;

        private string css_class = "";
        private string combo_css = "";

        public BLUIElement LinkedUIObject { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            await ReadComboData();
            css_class = (UIElement.IsVisible ? "d-flex" : "d-none") + " align-end " + UIElement.ParentCssClass;
            combo_css = (UIElement.IsVisible ? UIElement.CssClass : "d-none");
            combo_css = combo_css + (!string.IsNullOrEmpty(UIElement.IconCss) ? " search_combo_css" : "");
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
            requestDTO.RequestingElementKey = UIElement.ElementKey;

            requestDTO.RequestingURL = BaseEndpoint.BaseURL + UIElement.GetPathURL();
            requestDTO.SearchQuery = value;

            if (InteractionLogics != null)
            {

                EventCallback callback;
                if (InteractionLogics.TryGetValue(UIElement.OnBeforeComboLoad, out callback))
                {
                    if (callback.HasDelegate)
                    {
                        UIInterectionArgs<ComboRequestDTO> args = new UIInterectionArgs<ComboRequestDTO>();
                        args.DataObject = requestDTO;
                        await callback.InvokeAsync(args);
                        if (args.DataObject.CancelRead)
                        {
                            return;
                        }
                    }
                }
                else
                {

                }
            }

            AddressResponse = await _comboManager.GetAddressResponses(requestDTO);

            if (AddressResponse.Count > 0 && !UIElement.IsServerFiltering)
            {
                selectedAddress = this.AddressResponse.Where(x => x.IsDefault).ToList();

                if (selectedAddress != null)
                {
                    await NotifyHooks(selectedAddress);

                }
                else
                {
                    await NotifyHooks(new List<AddressResponse>());
                }



                StateHasChanged();
            }


        }

        private async Task OnComboValueChanged(IEnumerable<AddressResponse> addressResponse)
        {
            addressResponse = await NotifyHooks(addressResponse.ToList());

        }

        private async Task<IList<AddressResponse>> NotifyHooks(IList<AddressResponse> addressResponse)
        {
            ComboDataObject.SetListValueByObjectPath<AddressResponse>(UIElement.DefaultAccessPath, addressResponse);
            UIInterectionArgs<IList<AddressResponse>> args = new UIInterectionArgs<IList<AddressResponse>>();

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
                ComboDataObject.SetListValueByObjectPath<AddressResponse>(UIElement.DefaultAccessPath, addressResponse);
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
            conversionInfo = ComboDataObject.GetPropListObject<AddressResponse>(UIElement.DefaultAccessPath);
            if (conversionInfo.IsConversionSuccess)
            {
                selectedAddress = conversionInfo.Value;
            }
            else
            {
                selectedAddress = new List<AddressResponse>();
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
            this.selectedAddress = new List<AddressResponse>();
            this.StateHasChanged();
        }

        public void UpdateVisibility(bool IsVisible)
        {
            this.UIElement.IsVisible = IsVisible;
            css_class = (UIElement.IsVisible ? "d-flex" : "d-none") + " align-end ";
            combo_css = (UIElement.IsVisible ? UIElement.CssClass : "d-none");
            combo_css = combo_css + (!string.IsNullOrEmpty(UIElement.IconCss) ? " search_combo_css" : "");
            StateHasChanged();
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
                PropertyConversionResponse<IList<AddressResponse>> conversions = ComboDataObject.GetPropListObject<AddressResponse>(this.UIElement.DefaultAccessPath);
                if (conversions.IsConversionSuccess)
                {
                    foreach (AddressResponse adr in conversions.Value)
                    {
                        selectedAddress = AddressResponse.Where(x => x.AddressKey == adr.AddressKey).ToList();
                    }
                    
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
            await NotifyHooks(value as List<AddressResponse>);

        }

        public async Task FetchData(bool useLocalstorage = false)
        {
            await ReadComboData();
        }

        public Task SetDataSource(object DataSource)
        {
            throw new NotImplementedException();
        }

        private string GetMultiSelectionText(List<string> selectedValues)
        {
                return $"{string.Join(", ", selectedValues.Select(x => x))}";
        }
    }
}
