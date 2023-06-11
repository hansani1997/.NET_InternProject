using BlueLotus360.CleanArchitecture.Client.Infrastructure.Routes;
using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.TelerikComponents
{
    public partial class BLTelAccountCombo : IBLUIOperationHelper
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

        private AccountResponse selectedAccount = new AccountResponse();

        IList<AccountResponse> AccountResponses;

        private PropertyConversionResponse<AccountResponse> conversionInfo;

        private bool isEditable = true;

        private string css_class = "";
        private string combo_css = "";

        private int width = 2;

        private TelerikComboBox<AccountResponse,long> _ref;
        public BLUIElement LinkedUIObject { get; private set; }

        protected override async Task OnInitializedAsync()
        {

            css_class = (UIElement.IsVisible ? "d-flex" : "d-none") + " align-end k-bl-adj";
            combo_css = (UIElement.IsVisible ? UIElement.CssClass : "d-none");
            combo_css = combo_css + (!string.IsNullOrEmpty(UIElement.IconCss) ? " search_combo_css" : "");
            width = UIElement.IsVisible ? UIElement.Width : 0;
            await ReadCombo();
            
            isEditable = UIElement.IsEnable;
            await base.OnInitializedAsync();
        }


        public async Task ReadCombo()
        {
            ComboRequestDTO requestDTO = new ComboRequestDTO();
            requestDTO.RequestingElementKey = UIElement.ElementKey;
            requestDTO.RequestingURL = BaseEndpoint.BaseURL + UIElement.GetPathURL();

            if (UIElement.CollectionName != null && UIElement.CollectionName.Trim().Length > 5)
            {
                string local_key = $"_Acc_{UIElement.CollectionName}";
                if (await _sessionStorage.ContainKeyAsync(local_key))
                {
                    AccountResponses = await _sessionStorage.GetItemAsync<IList<AccountResponse>>(local_key);
                }
                else
                {
                    AccountResponses = await _comboManager.GetAccountResponse(requestDTO);

                    await _sessionStorage.SetItemAsync<IList<AccountResponse>>(local_key, AccountResponses);
                }
            }
            else
            {
                AccountResponses = await _comboManager.GetAccountResponse(requestDTO);

            }



            if (AccountResponses.Count > 0)
            {
                if (!BaseResponse.IsValidData(selectedAccount))
                {
                    selectedAccount = this.AccountResponses.Where(x => x.IsDefault).FirstOrDefault();

                    if (selectedAccount != null)
                    {
                        await NotifyHooks(selectedAccount);

                    }
                    else
                    {
                        await NotifyHooks(new AccountResponse());
                    }
                }

                StateHasChanged();
            }
        }

      

        private async Task OnComboValueChanged(AccountResponse AccountResponse)
        {
            
            AccountResponse = await NotifyHooks(AccountResponse);

        }

        private async Task<AccountResponse> NotifyHooks(AccountResponse AccountResponse)
        {
            ComboDataObject.SetValueByObjectPath(UIElement.DefaultAccessPath, AccountResponse);

            UIInterectionArgs<AccountResponse> args = new UIInterectionArgs<AccountResponse>();
            if (InteractionLogics != null && InteractionLogics.Count > 0)
            {
                EventCallback callback;
                if (UIElement.OnClickAction != null && InteractionLogics.TryGetValue(UIElement.OnClickAction, out callback))
                {
                    args.Caller = this.UIElement.OnClickAction;
                    args.ObjectPath = this.UIElement.DefaultAccessPath;
                    args.DataObject = AccountResponse;
                    args.sender = this;
                    args.InitiatorObject = UIElement;
                    await callback.InvokeAsync(args);
                }
            }

            if (!(args.DelegateExecuted && args.CancelChange))
            {
                ComboDataObject.SetValueByObjectPath(UIElement.DefaultAccessPath, AccountResponse);
                selectedAccount = AccountResponse;
                StateHasChanged();
            }
            else
            {

                if (args.OverrideValue)
                {
                    AccountResponse = args.OverriddenValue;
                    selectedAccount = AccountResponse;
                    StateHasChanged();
                }

            }

            return AccountResponse;
        }

        protected override Task OnParametersSetAsync()//The synchronous and asynchronous way of setting the parameter when the component receives the parameter from its parent component.
        {

            int c = this.ComboDataObject.GetHashCode();
            conversionInfo = ComboDataObject.GetPropObject<AccountResponse>(UIElement.DefaultAccessPath);
            if (conversionInfo.IsConversionSuccess)
            {
                selectedAccount = conversionInfo.Value;
            }

            if (ObjectHelpers != null)
            {
                if (ObjectHelpers.ContainsKey(UIElement.ElementName))
                {
                    ObjectHelpers.Remove(UIElement.ElementName);

                }
                ObjectHelpers.Add(UIElement.ElementName, this);
            }

            if (selectedAccount==null)
            {
                selectedAccount=new AccountResponse();
            }

            return base.OnParametersSetAsync();

        }

        private async Task<IEnumerable<AccountResponse>> OnComboSearch(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.


            // if text is null or empty, don't return values (drop-down will not open)
            if (string.IsNullOrEmpty(value))
            {
                return new List<AccountResponse>();

            }
            await Task.CompletedTask;

            return AccountResponses.Where(x => x.AccountName.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        public void ResetToInitialValue()
        {
            this.selectedAccount = new AccountResponse();
            this.StateHasChanged();
        }

        public void UpdateVisibility(bool IsVisible)
        {
            this.UIElement.IsVisible = IsVisible;
            css_class = (UIElement.IsVisible ? "d-flex" : "d-none") + " align-end ";
            combo_css = (UIElement.IsVisible ? UIElement.CssClass : "d-none");
            StateHasChanged();
        }

        public void ToggleEditable(bool IsEditable)
        {
            isEditable = IsEditable;
            StateHasChanged();
        }

        public async Task Refresh()
        {
            if (this.AccountResponses != null)
            {


                PropertyConversionResponse<AccountResponse> conversions = ComboDataObject.GetPropObject<AccountResponse>(this.UIElement.DefaultAccessPath);
                if (conversions.IsConversionSuccess)
                {
                    selectedAccount = AccountResponses.Where(x => x.AccountKey == conversions.Value.AccountKey).FirstOrDefault();
                    await NotifyHooks(selectedAccount);
                }

            }
        }

        public async Task FocusComponentAsync()
        {
            await _ref.FocusAsync();
        }

        public async Task SetValue(object value)
        {
            try
            {
                long v = Convert.ToInt64(value);
                if (AccountResponses.Where(x => x.AccountKey == v).Count() > 0)
                {
                    selectedAccount = AccountResponses.Where(x => x.AccountKey == v).FirstOrDefault();
                    await NotifyHooks(selectedAccount);
                }
                else
                {
                    selectedAccount = AccountResponses.Where(x => x.AccountKey == 1).FirstOrDefault();
                    await NotifyHooks(selectedAccount);

                }

            }
            catch (Exception exp)
            {

            }
        }

        private async void OnComboValueChangedTel(long KeyValue)
        {
            if (KeyValue == 0)
            {
                KeyValue = 1;
            }
            selectedAccount = AccountResponses.Where(x => x.AccountKey == KeyValue).FirstOrDefault();
            await OnComboValueChanged(selectedAccount);
        }
    }
}
