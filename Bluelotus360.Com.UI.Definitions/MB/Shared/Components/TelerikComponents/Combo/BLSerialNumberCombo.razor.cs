using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Blazor.Components;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Routes;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.TelerikComponents.Combo
{
    public partial class BLSerialNumberCombo : IBLUIOperationHelper, IBLServerDependentComponent
    {
        [Parameter]
        public BLUIElement UIElement { get; set; }

        [Parameter]
        public object ComboDataObject { get; set; }

        [Parameter]
        public EventCallback OnComboChanged { get; set; }

        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }

        private ItemSerialNumber selectedSerialnumber = new ItemSerialNumber();

        public IList<ItemSerialNumber> ItemSerialNumbers { get; set; }
        private PropertyConversionResponse<ItemSerialNumber> conversionInfo;

        [Parameter]
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        public BLUIElement LinkedUIObject { get; private set; }

        private bool __forcerender = false;

        private string css_class = "";
        private string combo_css = "";

        private TelerikComboBox<AccountResponse, long> _refItemCombo;


        protected override async Task OnInitializedAsync()
        {
            if (ObjectHelpers.ContainsKey(UIElement.ElementName))
            {
                ObjectHelpers.Remove(UIElement.ElementName);
            }

            if (ObjectHelpers != null)
            {
                ObjectHelpers.Add(UIElement.ElementName, this);
            }
            css_class = (UIElement.IsVisible ? "d-flex" : "d-none") + " align-end " + UIElement.ParentCssClass;
            combo_css = (UIElement.IsVisible ? UIElement.CssClass : "d-none");
            combo_css = combo_css + (!string.IsNullOrEmpty(UIElement.IconCss) ? " search_combo_css" : "");
            await ReadCmboData();

            await base.OnInitializedAsync();
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            return base.SetParametersAsync(parameters);

        }

        public async Task ReadCmboData(string SearchQuery = "")
        {
            ComboRequestDTO requestDTO = new ComboRequestDTO();
            requestDTO.SearchQuery = SearchQuery;
            requestDTO.RequestingElementKey = UIElement.ElementKey;
            requestDTO.RequestingURL = BaseEndpoint.BaseURL + UIElement.GetPathURL();
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

            ItemSerialNumbers = await _comboManager.GetSerialNumberResponses(requestDTO);

            if (InteractionLogics != null)
            {

                EventCallback callback;
                if (InteractionLogics.TryGetValue(UIElement.OnAfterComboLoad, out callback))
                {
                    if (callback.HasDelegate)
                    {
                        UIInterectionArgs<IList<ItemSerialNumber>> args = new UIInterectionArgs<IList<ItemSerialNumber>>();
                        args.DataObject = ItemSerialNumbers;
                        await callback.InvokeAsync(args);
                    }
                }
                else
                {

                }
            }

            if (ItemSerialNumbers.Count > 0)
            {
                selectedSerialnumber = this.ItemSerialNumbers.Where(x => x.IsDefault).FirstOrDefault();

                if (selectedSerialnumber != null)
                {
                    selectedSerialnumber.IsMust = UIElement.IsMust;
                    OnComboValueChanged(selectedSerialnumber);

                }
                else
                {
                    selectedSerialnumber = new ItemSerialNumber();
                    var cd = new ItemSerialNumber();
                    cd.IsMust = UIElement.IsMust;
                    OnComboValueChanged(cd);
                }

                //await OnDataLoadedCompleted();


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
                    UIInterectionArgs<IList<ItemSerialNumber>> args = new UIInterectionArgs<IList<ItemSerialNumber>>();
                    args.DataObject = ItemSerialNumbers;
                    await callback.InvokeAsync(args);
                }
            }
        }

        private async void OnComboValueChangedTel(long key)
        {
            if (key == 0)
            {
                key = 1;
            }
            if (ItemSerialNumbers != null)
            {
                selectedSerialnumber = ItemSerialNumbers.Where(x => x.SerialNumberKey == key).FirstOrDefault();
            }
            OnComboValueChanged(selectedSerialnumber);
        }
        private void OnComboValueChanged(ItemSerialNumber ItemSerialNumberResponse)
        {
            try
            {
                if (InteractionLogics != null)
                {

                    EventCallback callback;
                    if (InteractionLogics.TryGetValue(UIElement.OnClickAction, out callback))
                    {
                        if (callback.HasDelegate)
                        {
                            UIInterectionArgs<ItemSerialNumber> args = new UIInterectionArgs<ItemSerialNumber>();
                            args.Caller = this.UIElement.OnClickAction;
                            args.ObjectPath = this.UIElement.DefaultAccessPath;
                            args.DataObject = ItemSerialNumberResponse;
                            args.sender = this;
                            args.InitiatorObject = UIElement;
                            callback.InvokeAsync(args).Wait();

                        }
                        else
                        {
                            Console.WriteLine($"{UIElement.OnClickAction} is not defined for {UIElement._internalElementName} - {UIElement.ElementKey.ToString()}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"{UIElement.OnClickAction} is not defined for {UIElement._internalElementName} - {UIElement.ElementKey.ToString()}");

                }
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }

        protected override Task OnParametersSetAsync()
        {

            int c = this.ComboDataObject.GetHashCode();
            conversionInfo = ComboDataObject.GetPropObject<ItemSerialNumber>(UIElement.DefaultAccessPath);
            if (conversionInfo.IsConversionSuccess)
            {
                selectedSerialnumber = conversionInfo.Value;
            }
            return base.OnParametersSetAsync();

        }

        private async Task<IEnumerable<ItemSerialNumber>> OnComboSearch(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new List<ItemSerialNumber>();

            }
            if (UIElement.IsServerFiltering)
            {
                await ReadCmboData(value);
            }
            return ItemSerialNumbers.Where(x => x.SerialNumber.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        public void ResetToInitialValue()
        {
            this.selectedSerialnumber = new ItemSerialNumber();
            __forcerender = true;
            this.StateHasChanged();
            __forcerender = false;
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
            throw new NotImplementedException();
        }

        public async Task Refresh()
        {
            await Task.CompletedTask;
        }

        public Task FocusComponentAsync()
        {
            throw new NotImplementedException();
        }

        public async Task SetValue(object value)
        {
            OnComboValueChangedTel(Convert.ToInt64(value));
            await Task.CompletedTask;
        }

        public async Task FetchData(bool useLocalstorage = false)
        {
            await ReadCmboData();
        }

        public Task SetDataSource(object DataSource)
        {
            throw new NotImplementedException();
        }


        private string GetComboDisplayText()
        {
            if (selectedSerialnumber != null)
            {
                return selectedSerialnumber.SerialNumber;
            }
            if (_refItemCombo != null)
            {
                return _refItemCombo.TextField;
            }
            return "-";
        }
    }
}
