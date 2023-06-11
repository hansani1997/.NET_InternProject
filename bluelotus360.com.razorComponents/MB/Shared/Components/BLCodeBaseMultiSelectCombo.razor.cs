using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain;
using bluelotus360.com.razorComponents.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bluelotus360.Com.commonLib.Routes;

namespace bluelotus360.com.razorComponents.MB.Shared.Components
{
    public partial class BLCodeBaseMultiSelectCombo:IBLUIOperationHelper, IBLServerDependentComponent
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


        private IList<CodeBaseResponse> selectedCodeBase = new List<CodeBaseResponse>();

        public IList<CodeBaseResponse> CodebaseResponse { get; set; }

        private PropertyConversionResponse<IList<CodeBaseResponse>> conversionInfo;

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

            CodebaseResponse = await _comboManager.GetCodeBaseResponses(requestDTO);

            if (CodebaseResponse.Count > 0)
            {
                selectedCodeBase = this.CodebaseResponse.Where(x => x.IsDefault).ToList();

                if (selectedCodeBase != null)
                {
                    await NotifyHooks(selectedCodeBase);

                }
                else
                {
                    await NotifyHooks(new List<CodeBaseResponse>());
                }



                StateHasChanged();
            }


        }

        private async Task OnComboValueChanged(IEnumerable<CodeBaseResponse> code)
        {
            code = await NotifyHooks(code.ToList());

        }

        private async Task<IList<CodeBaseResponse>> NotifyHooks(IList<CodeBaseResponse> codeList)
        {
            ComboDataObject.SetListValueByObjectPath<CodeBaseResponse>(UIElement.DefaultAccessPath, codeList);
            UIInterectionArgs<IList<CodeBaseResponse>> args = new UIInterectionArgs<IList<CodeBaseResponse>>();

            if (InteractionLogics != null && InteractionLogics.Count > 0)
            {
                EventCallback callback;
                if (UIElement.OnClickAction != null && InteractionLogics.TryGetValue(UIElement.OnClickAction, out callback))
                {
                    if (callback.HasDelegate)
                    {
                        args.Caller = this.UIElement.OnClickAction;
                        args.ObjectPath = this.UIElement.DefaultAccessPath;
                        args.DataObject = codeList;
                        args.sender = this;
                        args.InitiatorObject = UIElement;
                        await callback.InvokeAsync(args);
                    }
                }
            }

            if (!(args.DelegateExecuted && args.CancelChange))
            {
                ComboDataObject.SetListValueByObjectPath<CodeBaseResponse>(UIElement.DefaultAccessPath, codeList);
                selectedCodeBase = codeList;
                StateHasChanged();
            }
            else
            {

                if (args.OverrideValue)
                {
                    codeList = args.OverriddenValue;
                    selectedCodeBase = codeList;
                    StateHasChanged();
                }

            }

            return codeList;
        }

        protected override Task OnParametersSetAsync()//The synchronous and asynchronous way of setting the parameter when the component receives the parameter from its parent component.
        {
            int c = this.ComboDataObject.GetHashCode();
            conversionInfo = ComboDataObject.GetPropListObject<CodeBaseResponse>(UIElement.DefaultAccessPath);
            if (conversionInfo.IsConversionSuccess)
            {
                selectedCodeBase = conversionInfo.Value;
            }
            else
            {
                selectedCodeBase = new List<CodeBaseResponse>();
            }
            return base.OnParametersSetAsync();

        }

        private async Task<IEnumerable<CodeBaseResponse>> OnComboSearch(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.


            // if text is null or empty, don't return values (drop-down will not open)
            if (string.IsNullOrEmpty(value))
            {
                return CodebaseResponse;

            }

            if (UIElement.IsServerFiltering && value.Length > 2)
            {
                await ReadComboData(value);
            }

            return CodebaseResponse.Where(x => x.CodeName.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        public void ResetToInitialValue()
        {
            this.selectedCodeBase = new List<CodeBaseResponse>();
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
            if (this.CodebaseResponse != null)
            {
                PropertyConversionResponse<IList<CodeBaseResponse>> conversions = ComboDataObject.GetPropListObject<CodeBaseResponse>(this.UIElement.DefaultAccessPath);
                if (conversions.IsConversionSuccess)
                {
                    foreach (CodeBaseResponse adr in conversions.Value)
                    {
                        selectedCodeBase = CodebaseResponse.Where(x => x.CodeKey == adr.CodeKey).ToList();
                    }

                    await NotifyHooks(selectedCodeBase);
                }

            }
        }

        public Task FocusComponentAsync()
        {
            throw new NotImplementedException();
        }

        public async Task SetValue(object value)
        {
            await NotifyHooks(value as List<CodeBaseResponse>);

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
