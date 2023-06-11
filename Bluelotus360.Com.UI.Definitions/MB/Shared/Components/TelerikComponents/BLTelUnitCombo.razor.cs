
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
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.TelerikComponents;
public partial class BLTelUnitCombo : IBLUIOperationHelper, IBLServerDependentComponent
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

    private UnitResponse selectedUnit = new UnitResponse();

    IList<UnitResponse> UnitResponses = new List<UnitResponse>();

    private PropertyConversionResponse<UnitResponse> conversionInfo;

    public BLUIElement LinkedUIObject { get; private set; }

    private string css_class = "";

    private string combo_css = "";

    protected override async Task OnParametersSetAsync()
    {

        if (ObjectHelpers.ContainsKey(UIElement.ElementName))
        {
            ObjectHelpers.Remove(UIElement.ElementName);
        }

        if (ObjectHelpers != null)
        {
            ObjectHelpers.Add(UIElement.ElementName, this);
        }

        css_class = (UIElement.IsVisible ? $"d-flex {UIElement.ParentCssClass}" : "d-none") + " align-end ";
        combo_css = (UIElement.IsVisible ? UIElement.CssClass : "d-none");

        conversionInfo = ComboDataObject.GetPropObject<UnitResponse>(UIElement.DefaultAccessPath);
        if (conversionInfo.IsConversionSuccess)
        {
            selectedUnit = conversionInfo.Value;
        }

        await base.OnParametersSetAsync();
    }

    protected override async Task OnInitializedAsync()
    {
        await ReadCmboData();
        StateHasChanged();
        await base.OnInitializedAsync();

    }


    private async Task ReadCmboData()
    {
        ComboRequestDTO requestDTO = new ComboRequestDTO();
        requestDTO.RequestingElementKey = UIElement.ElementKey;
        requestDTO.RequestingURL = BaseEndpoint.BaseURL + UIElement.GetPathURL();
        UIInterectionArgs<ComboRequestDTO> args = new UIInterectionArgs<ComboRequestDTO>();

        if (InteractionLogics != null)
        {

            EventCallback callback;
            if (InteractionLogics.TryGetValue(UIElement.OnBeforeComboLoad, out callback))
            {
                if (callback.HasDelegate)
                {
                    args.DataObject = requestDTO;
                    args.InitiatorObject = UIElement;
                    callback.InvokeAsync(args).Wait();
                }
            }
            else
            {

            }
        }

        if (!args.CancelChange)
        {
            UnitResponses = await _comboManager.GetItemUnits(requestDTO);

        }
        else
        {
            UnitResponses = new List<UnitResponse>();

        }




        if (UnitResponses.Count > 0)
        {
            selectedUnit = this.UnitResponses.Where(x => x.IsDefault).FirstOrDefault();

            if (selectedUnit != null)
            {
                await NotifyHooks(selectedUnit);

            }
            else
            {
                await NotifyHooks(new UnitResponse());
            }



            StateHasChanged();
        }
    }



    private async Task OnComboValueChanged(UnitResponse unitResponse)
    {
        unitResponse = await NotifyHooks(unitResponse);


    }

    private async Task<UnitResponse> NotifyHooks(UnitResponse unitResponse)
    {
        try
        {


            UIInterectionArgs<UnitResponse> args = new UIInterectionArgs<UnitResponse>();
            
            if (InteractionLogics != null)
            {
                EventCallback callback;
                if (UIElement.OnClickAction != null && InteractionLogics.TryGetValue(UIElement.OnClickAction, out callback))
                {
                    if (callback.HasDelegate)
                    {
                        args.Caller = this.UIElement.OnClickAction;
                        args.ObjectPath = this.UIElement.DefaultAccessPath;
                        args.DataObject = unitResponse;
                        args.sender = this;
                        args.InitiatorObject = UIElement;
                        await callback.InvokeAsync(args);
                        args.DelegateExecuted = true;
                    }
                }
            }

            if (!(args.DelegateExecuted && args.CancelChange))
            {
                ComboDataObject.SetValueByObjectPath(UIElement.DefaultAccessPath, unitResponse);
                selectedUnit = unitResponse;
                StateHasChanged();
            }
            else
            {

                if (args.OverrideValue)
                {
                    unitResponse = args.OverriddenValue;
                    selectedUnit = unitResponse;
                    StateHasChanged();

                }

            }
        }
        catch (Exception ex)
        {

        }

        return unitResponse;
    }




    private async Task<IEnumerable<UnitResponse>> OnComboSearch(string value)
    {
        // In real life use an asynchronous function for fetching data from an api.
        await Task.Delay(5);

        // if text is null or empty, don't return values (drop-down will not open)
        if (string.IsNullOrEmpty(value))
        {
            return new List<UnitResponse>();

        }

        var list = UnitResponses.Where(x => x.UnitName.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        if (list.Count() < 2)
        {
            list = UnitResponses.Take(10);
        }
        return list;
    }

    public void ResetToInitialValue()
    {
        this.selectedUnit = new UnitResponse();
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
        throw new NotImplementedException();
    }

    public async Task Refresh()
    {
        if (this.UnitResponses != null)
        {


            PropertyConversionResponse<UnitResponse> conversions = ComboDataObject.GetPropObject<UnitResponse>(this.UIElement.DefaultAccessPath);
            if (conversions.IsConversionSuccess)
            {
                selectedUnit = UnitResponses.Where(x => x.UnitKey == conversions.Value.UnitKey).FirstOrDefault();
                await NotifyHooks(selectedUnit);
            }

        }
    }

    public async Task FetchData(bool useLocalstorage = false)
    {
        await this.ReadCmboData();
    }

    public Task FocusComponentAsync()
    {
        throw new NotImplementedException();
    }

    public async Task SetValue(object value)
    {
        try
        {
            long v = Convert.ToInt64(value);
            if (UnitResponses.Where(x => x.UnitKey == v).Count() > 0)
            {
                selectedUnit = UnitResponses.Where(x => x.UnitKey == v).FirstOrDefault();
                await NotifyHooks(selectedUnit);
            }
            else
            {
                selectedUnit = UnitResponses.Where(x => x.UnitKey == 1).FirstOrDefault();
                await NotifyHooks(selectedUnit);

            }

        }
        catch (Exception exp)
        {

        }
    }

    public async Task SetDataSource(object DataSource)
    {
        var t = DataSource is IList<UnitResponse>;
        if (t)
        {
            UnitResponses = (IList<UnitResponse>)DataSource;
            selectedUnit = this.UnitResponses.Where(x => x.IsDefault).FirstOrDefault();

            if (selectedUnit != null)
            {
                await NotifyHooks(selectedUnit);

            }
            else
            {
                await NotifyHooks(new UnitResponse());
            }


        }
        await Task.CompletedTask;
    }


    private async void OnComboValueChangedTel(long KeyValue)
    {
        if (KeyValue == 0)
        {
            KeyValue = 1;
        }
        selectedUnit = UnitResponses.Where(x => x.UnitKey == KeyValue).FirstOrDefault();
        await OnComboValueChanged(selectedUnit);
    }
}