using BlueLotus360.CleanArchitecture.Client.Infrastructure.Routes;
using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.TelerikComponents
{
    public partial class BLTelApprovedStateCombo : IBLUIOperationHelper
    {
        #region Parameters
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
        public BLUIElement LinkedUIObject => throw new System.NotImplementedException();

        private CodeBaseResponse selectedCodeBase = new CodeBaseResponse();

        IList<CodeBaseResponse> CodeBaseResponses = new List<CodeBaseResponse>();

        private PropertyConversionResponse<CodeBaseResponse> conversionInfo;
        private string css_class = "";
        private bool isEditable = true;
        private long elementKy = 1;
        private MudAutocomplete<CodeBaseResponse> _refAutoComplete;


        #endregion

        protected override async Task OnInitializedAsync()
        {
            css_class = (UIElement.IsVisible ? "d-flex" : "d-none") + " align-end ";


            if (ObjectHelpers != null)
            {
                if (ObjectHelpers.ContainsKey(UIElement.ElementName))
                {
                    ObjectHelpers.Remove(UIElement.ElementName);

                }
                ObjectHelpers.Add(UIElement.ElementName, this);
            }
            await ReadComboData();
            await base.OnInitializedAsync();
        }

        private async Task ReadComboData()
        {
            ComboRequestDTO requestDTO = new ComboRequestDTO();
            CodeBaseResponse ApprovedState = new CodeBaseResponse();


            if (ComboDataObject!=null)
            {   
                requestDTO.RequestingElementKey = elementKy;
                requestDTO.RequestingURL = BaseEndpoint.BaseURL + UIElement.GetPathURL();
                long TrnKy = 0;
                int OrdKy = 0;
                PropertyInfo transaction = ComboDataObject.GetType().GetProperty("TransactionKey");
                PropertyInfo order = ComboDataObject.GetType().GetProperty("OrderKey");
                if (transaction != null)
                {
                    TrnKy = (long)transaction?.GetValue(ComboDataObject, null);
                }
                if (order!=null)
                {
                    OrdKy = (int)order?.GetValue(ComboDataObject, null);
                }
                

                int RequestingObjectKey= (int)ComboDataObject.GetType()?.GetProperty("RequestingObjectKey")?.GetValue(ComboDataObject, null);
                PropertyConversionResponse<CodeBaseResponse> conversion = ComboDataObject.GetPropObject<CodeBaseResponse>("ApproveState");
                if (conversion.IsConversionSuccess)
                {
                    ApprovedState = conversion.Value;
                }

                requestDTO.AddtionalData = new Dictionary<string, object> { { "TransactionKey", TrnKy }, { "OrderKey", OrdKy }, { "RequestingObjectKey", RequestingObjectKey }, { "ApproveState", ApprovedState } };
            }

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


            CodeBaseResponses = await _comboManager.GetApproveStatusResponses(requestDTO);

            

            if (CodeBaseResponses.Count > 0)
            {
                selectedCodeBase = this.CodeBaseResponses.Where(x => x.IsDefault).FirstOrDefault();

                if (selectedCodeBase != null)
                {
                    selectedCodeBase.IsMust = UIElement.IsMust;
                    await NotifyHooks(selectedCodeBase);

                }
                else
                {

                    var cd = ApprovedState;
                    cd.IsMust = UIElement.IsMust;
                    await NotifyHooks(cd);
                }

                await OnDataLoadedCompleted();


                StateHasChanged();
            }
        }

        private async Task OnDataLoadedCompleted()
        {
            EventCallback callback;
            if (UIElement.OnAfterComboLoad != null && InteractionLogics.TryGetValue(UIElement.OnAfterComboLoad, out callback))
            {
                if (callback.HasDelegate)
                {
                    UIInterectionArgs<IList<CodeBaseResponse>> args = new UIInterectionArgs<IList<CodeBaseResponse>>();
                    args.DataObject = CodeBaseResponses;
                    await callback.InvokeAsync(args);
                }
            }
        }

        private async void OnComboValueChangedTel(int KeyValue)
        {
            if (KeyValue == 0)
            {
                KeyValue = 1;
            }
            if(CodeBaseResponses!=null)
                selectedCodeBase = CodeBaseResponses.Where(x => x.CodeKey == KeyValue).FirstOrDefault();
            if(selectedCodeBase!=null)
                OnComboValueChanged(selectedCodeBase);
        }

        private async void OnComboValueChanged(CodeBaseResponse codeBaseResponse)
        {
            if (codeBaseResponse != null)
            {
                await NotifyHooks(codeBaseResponse);
            }
            else
            {
                await NotifyHooks(new CodeBaseResponse());
            }

        }

        private async Task<CodeBaseResponse> NotifyHooks(CodeBaseResponse codeBaseResponse)
        {
            try
            {
                codeBaseResponse.IsMust = UIElement.IsMust;
                ComboDataObject.SetValueByObjectPath(UIElement.DefaultAccessPath, codeBaseResponse);

                if (InteractionLogics != null)
                {

                    EventCallback callback;
                    if (UIElement.OnClickAction != null && InteractionLogics.TryGetValue(UIElement.OnClickAction, out callback))
                    {
                        if (callback.HasDelegate)
                        {
                            UIInterectionArgs<CodeBaseResponse> args = new UIInterectionArgs<CodeBaseResponse>();
                            args.Caller = this.UIElement.OnClickAction;
                            args.ObjectPath = this.UIElement.DefaultAccessPath;
                            args.DataObject = codeBaseResponse;
                            args.InitiatorObject = UIElement;
                            args.sender = this;
                            await callback.InvokeAsync(args);

                        }
                        else
                        {
                            Console.WriteLine($"{UIElement.OnClickAction} is not defined for {UIElement._internalElementName} - {UIElement.ElementKey.ToString()}");
                        }


                    }
                    else
                    {
                        Console.WriteLine($"{UIElement.OnClickAction} is not defined for {UIElement._internalElementName} - {UIElement.ElementKey.ToString()}");

                    }
                }
                StateHasChanged();

            }
            catch (Exception ex)
            {

            }
            return codeBaseResponse;
        }

        protected override Task OnParametersSetAsync()
        {
            UIElement.CanRefreshData = false;

            int c = this.ComboDataObject.GetHashCode();
            conversionInfo = ComboDataObject.GetPropObject<CodeBaseResponse>(UIElement.DefaultAccessPath);
            if (conversionInfo.IsConversionSuccess)
            {
                selectedCodeBase = conversionInfo.Value;
            }
            return base.OnParametersSetAsync();

        }

        private async void RefreshCombo()
        {
            CodeBaseResponses = null;
            ComboRequestDTO requestDTO = new ComboRequestDTO();
            CodeBaseResponse ApprovedState = new CodeBaseResponse();


            if (ComboDataObject != null)
            {
                requestDTO.RequestingElementKey = elementKy;
                requestDTO.RequestingURL = BaseEndpoint.BaseURL + UIElement.GetPathURL();
                long TrnKy = (long)ComboDataObject.GetType().GetProperty("TransactionKey")?.GetValue(ComboDataObject, null);
                int RequestingObjectKey = (int)ComboDataObject.GetType().GetProperty("RequestingObjectKey")?.GetValue(ComboDataObject, null);
                PropertyConversionResponse<CodeBaseResponse> conversion = ComboDataObject.GetPropObject<CodeBaseResponse>("ApproveState");
                if (conversion.IsConversionSuccess)
                {
                    ApprovedState = conversion.Value;
                }

                requestDTO.AddtionalData = new Dictionary<string, object> { { "TransactionKey", TrnKy }, { "RequestingObjectKey", RequestingObjectKey }, { "ApproveState", ApprovedState } };
                CodeBaseResponses = await _comboManager.GetApproveStatusResponses(requestDTO);
            }
            this.StateHasChanged();
        }

        private async Task<IEnumerable<CodeBaseResponse>> OnComboSearch(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5);

            // if text is null or empty, don't return values (drop-down will not open)
            if (string.IsNullOrEmpty(value))
            {
                return new List<CodeBaseResponse>();
            }

            return CodeBaseResponses.Where(x => x.CodeName.Contains(value, StringComparison.InvariantCultureIgnoreCase));

        }

        public void ResetToInitialValue()
        {
            this.selectedCodeBase = new CodeBaseResponse();
            this.StateHasChanged();
        }

        public void UpdateVisibility(bool IsVisible)
        {
            this.UIElement.IsVisible = IsVisible;
            css_class = (UIElement.IsVisible ? "d-flex" : "d-none") + " align-end ";
            StateHasChanged();
        }

        public void ToggleEditable(bool IsEditable)
        {
            this.isEditable = IsEditable;
            StateHasChanged();
        }

        public async Task Refresh()
        {
            if (this.CodeBaseResponses != null)
            {
                PropertyConversionResponse<CodeBaseResponse> conversions = ComboDataObject.GetPropObject<CodeBaseResponse>(this.UIElement.DefaultAccessPath);
                if (conversions.IsConversionSuccess)
                {
                    selectedCodeBase = CodeBaseResponses.Where(x => x.CodeKey == conversions.Value.CodeKey).FirstOrDefault();
                    await NotifyHooks(selectedCodeBase);
                }

            }
        }

        public async Task FocusComponentAsync()
        {
            await _refAutoComplete.FocusAsync();

        }

        public async Task SetValue(object value)
        {
            try
            {
                long v = Convert.ToInt64(value);
                if (CodeBaseResponses.Where(x => x.CodeKey == v).Count() > 0)
                {
                    selectedCodeBase = CodeBaseResponses.Where(x => x.CodeKey == v).FirstOrDefault();
                    await NotifyHooks(selectedCodeBase);
                }
                else
                {
                    selectedCodeBase = CodeBaseResponses.Where(x => x.CodeKey == 1).FirstOrDefault();
                    await NotifyHooks(selectedCodeBase);

                }

            }
            catch (Exception exp)
            {

            }
        }
    }
}
