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
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.TelerikComponents;
public partial class BLTelItemCombo : IBLUIOperationHelper, IBLServerDependentComponent
{
    [Parameter]
    public BLUIElement UIElement { get; set; }

    [Parameter]
    public object ComboDataObject { get; set; }

    [Parameter]
    public EventCallback OnComboChanged { get; set; }

    [Parameter]
    public IDictionary<string, EventCallback> InteractionLogics { get; set; }

    private ItemResponse selecteditemResponse = new ItemResponse();

    public IList<ItemResponse> ItemRes { get; set; }
    private PropertyConversionResponse<ItemResponse> conversionInfo;

    [Parameter]
    public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

    public BLUIElement LinkedUIObject { get; private set; }

    private bool __forcerender = false;

    private string css_class = "";
    private string combo_css = "";

    private TelerikComboBox<AccountResponse, long> _refItemCombo;

    public BLTelItemCombo()
    {


    }
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
        css_class = (UIElement.IsVisible ? "d-flex" : "d-none") + " align-end "+UIElement.ParentCssClass;
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

        ItemRes = await _comboManager.GetItemResponses(requestDTO);

        if (InteractionLogics != null)
        {

            EventCallback callback;
            if (InteractionLogics.TryGetValue(UIElement.OnAfterComboLoad, out callback))
            {
                if (callback.HasDelegate)
                {
                    UIInterectionArgs<IList<ItemResponse>> args = new UIInterectionArgs<IList<ItemResponse>>();
                    args.DataObject = ItemRes;
                    await callback.InvokeAsync(args);
                }
            }
            else
            {

            }
        }

        if (ItemRes.Count > 0)
        {
            selecteditemResponse = this.ItemRes.Where(x => x.IsDefault).FirstOrDefault();

            if (selecteditemResponse != null)
            {
                selecteditemResponse.IsMust = UIElement.IsMust;
                OnComboValueChanged(selecteditemResponse);

            }
            else
            {
                selecteditemResponse = new ItemResponse();
                var cd = new ItemResponse();
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
                UIInterectionArgs<IList<ItemResponse>> args = new UIInterectionArgs<IList<ItemResponse>>();
                args.DataObject = ItemRes;
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
        if (ItemRes != null)
        {
            selecteditemResponse = ItemRes.Where(x => x.ItemKey == key).FirstOrDefault();
        }
        OnComboValueChanged(selecteditemResponse);
    }
    private void OnComboValueChanged(ItemResponse ItemResponse)
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
                        UIInterectionArgs<ItemResponse> args = new UIInterectionArgs<ItemResponse>();
                        args.Caller = this.UIElement.OnClickAction;
                        args.ObjectPath = this.UIElement.DefaultAccessPath;
                        args.DataObject = ItemResponse;
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
        conversionInfo = ComboDataObject.GetPropObject<ItemResponse>(UIElement.DefaultAccessPath);
        if (conversionInfo.IsConversionSuccess)
        {
            selecteditemResponse = conversionInfo.Value;
        }
        return base.OnParametersSetAsync();

    }

    private async Task<IEnumerable<ItemResponse>> OnComboSearch(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return new List<ItemResponse>();

        }
        if (UIElement.IsServerFiltering)
        {
            await ReadCmboData(value);
        }
        return ItemRes.Where(x => x.ItemName.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    public void ResetToInitialValue()
    {
        this.selecteditemResponse = new ItemResponse();
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
        if (selecteditemResponse != null)
        {
            return selecteditemResponse.ItemName;
        }
        if (_refItemCombo != null)
        {
            return _refItemCombo.TextField;
        }
        return "-";
    }
}
