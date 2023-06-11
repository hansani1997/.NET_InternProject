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
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Combo
{
    public partial class NextApproveStatusCombo : IBLUIOperationHelper
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
        private int mapKy;
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

            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKy);
            requestDTO.RequestingElementKey = elementKy;
            requestDTO.RequestingURL = BaseEndpoint.BaseURL + UIElement.GetPathURL();

            mapKy = (int)ComboDataObject.GetType().GetProperty(UIElement.MapKey)?.GetValue(ComboDataObject, null);
            requestDTO.AddtionalData = new Dictionary<string, object> { { "AprStsKy", mapKy } };

            CodeBaseResponses = await _comboManager.GetNextApproveStatusResponses(requestDTO);

            if (UIElement.CollectionName != null && UIElement.CollectionName.Trim().Length > 5)
            {
                string local_key = $"_CodeBase_{UIElement.OurCode}_{UIElement.CollectionName}";
                if (await _sessionStorage.ContainKeyAsync(local_key))
                {
                    CodeBaseResponses = await _sessionStorage.GetItemAsync<IList<CodeBaseResponse>>(local_key);
                }
                else
                {
                    CodeBaseResponses = await _comboManager.GetCodeBaseResponses(requestDTO);

                    await _sessionStorage.SetItemAsync<IList<CodeBaseResponse>>(local_key, CodeBaseResponses);
                }
            }
            else
            {
                CodeBaseResponses = await _comboManager.GetCodeBaseResponses(requestDTO);

            }

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

                    var cd = new CodeBaseResponse();
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

        private async Task NotifyHooks(CodeBaseResponse codeBaseResponse)
        {
            try
            {
                codeBaseResponse.IsMust = UIElement.IsMust;
                ComboDataObject.SetValueByObjectPath(UIElement.DefaultAccessPath, codeBaseResponse);

                if (InteractionLogics != null)
                {

                    EventCallback callback;
                    if (InteractionLogics.TryGetValue(UIElement.OnClickAction, out callback))
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
            requestDTO.RequestingElementKey = UIElement.ElementKey;
            //requestDTO.RequestingURL = BaseEndpoint.BaseURL + UIElement.GetPathURL();
            CodeBaseResponses = await _comboManager.GetCodeBaseResponses(requestDTO);
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

        public Task Refresh()
        {
            throw new NotImplementedException();
        }

        public async Task FocusComponentAsync()
        {
            await _refAutoComplete.FocusAsync();

        }

        public async Task SetValue(object value)
        {
            this.selectedCodeBase = (CodeBaseResponse)value;
            this.StateHasChanged();
            await Task.CompletedTask;
        }
    }
}
