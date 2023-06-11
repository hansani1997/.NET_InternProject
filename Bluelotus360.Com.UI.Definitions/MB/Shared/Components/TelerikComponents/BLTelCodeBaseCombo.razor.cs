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

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components.TelerikComponents;
public partial class BLTelCodeBaseCombo : IBLUIOperationHelper,IBLServerDependentComponent
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

    #endregion
    public BLUIElement LinkedUIObject { get; private set; }

    private CodeBaseResponse selectedCodeBase = new CodeBaseResponse();

    IList<CodeBaseResponse> CodeBaseResponses = new List<CodeBaseResponse>();

    private PropertyConversionResponse<CodeBaseResponse> conversionInfo;
    private string css_class = "";
    private string combo_css = "";
    private bool isEditable = true;

    private MudAutocomplete<CodeBaseResponse> _refAutoComplete;




    protected override async Task OnInitializedAsync()
    {
        css_class = (UIElement.IsVisible ? $"d-flex {UIElement.ParentCssClass}" : "d-none") + " align-end ";
        combo_css= (UIElement.IsVisible ? UIElement.CssClass : "d-none ");
        combo_css = combo_css + (!string.IsNullOrEmpty(UIElement.IconCss) ? " search_combo_css" : "");
        await ReadComboData();

        if (ObjectHelpers != null)
        {
            if (ObjectHelpers.ContainsKey(UIElement.ElementName))
            {
                ObjectHelpers.Remove(UIElement.ElementName);

            }
            ObjectHelpers.Add(UIElement.ElementName, this);
        }
        isEditable = UIElement.IsEnable;
    
        await base.OnInitializedAsync();
    }

    private async Task ReadComboData()
    {
        ComboRequestDTO requestDTO = new ComboRequestDTO();
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
            if (!BaseResponse.IsValidData(selectedCodeBase))
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
            }
            

            await OnDataLoadedCompleted();


            StateHasChanged();
        }
    }


    private async Task OnDataLoadedCompleted()
    {
        EventCallback callback;
        if (InteractionLogics != null)
        {
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
            if (codeBaseResponse != null)
            {
                codeBaseResponse.IsMust = UIElement.IsMust;
                ComboDataObject.SetValueByObjectPath(UIElement.DefaultAccessPath, codeBaseResponse);
                UIInterectionArgs<CodeBaseResponse> args = new UIInterectionArgs<CodeBaseResponse>();


                if (InteractionLogics != null)
                {

                    EventCallback callback;
                    if (UIElement.OnClickAction != null && InteractionLogics.TryGetValue(UIElement.OnClickAction, out callback))
                    {
                        if (callback.HasDelegate)
                        {

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
                if (!(args.DelegateExecuted && args.CancelChange))
                {
                    ComboDataObject.SetValueByObjectPath(UIElement.DefaultAccessPath, codeBaseResponse);
                    selectedCodeBase = codeBaseResponse;
                    StateHasChanged();
                }
                else
                {

                    if (args.OverrideValue)
                    {
                        codeBaseResponse = args.OverriddenValue;
                        selectedCodeBase = codeBaseResponse;
                        StateHasChanged();
                    }

                }
                StateHasChanged();

            }
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


    private async void ReloadCombo()
    {
        CodeBaseResponses = null;
        ComboRequestDTO requestDTO = new ComboRequestDTO();
        requestDTO.RequestingElementKey = UIElement.ElementKey;
        requestDTO.RequestingURL = BaseEndpoint.BaseURL + UIElement.GetPathURL();
        CodeBaseResponses = await _comboManager.GetCodeBaseResponses(requestDTO);
        this.StateHasChanged();
    }

    private async Task<IEnumerable<CodeBaseResponse>> OnComboSearch(string value)
    {
        // In real life use an asynchronous function for fetching data from an api.


        // if text is null or empty, don't return values (drop-down will not open)
        if (string.IsNullOrEmpty(value))
        {
            return CodeBaseResponses;

        }
        await Task.CompletedTask;

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
        combo_css = (UIElement.IsVisible ? UIElement.CssClass : "d-none");
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

  

    //the same configuration applies if the "myComboData" object is null initially and is populated on some event

    private async void OnComboValueChangedTel(int KeyValue)
    {
        if (KeyValue == 0)
        {
            KeyValue = 1;
        }
         selectedCodeBase = CodeBaseResponses.Where(x => x.CodeKey == KeyValue).FirstOrDefault();
         OnComboValueChanged(selectedCodeBase);
    }

    public async Task FetchData(bool useLocalstorage = false)
    {
        await ReadComboData();
    }

    public async Task SetDataSource(object DataSource)
    {
        await Task.CompletedTask;
    }
}




