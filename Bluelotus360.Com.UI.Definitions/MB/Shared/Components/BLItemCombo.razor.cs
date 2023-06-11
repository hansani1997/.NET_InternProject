using BlueLotus360.CleanArchitecture.Client.Infrastructure.Routes;
using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Reflection;

namespace BlueLotus360.Com.UI.Definitions.MB.Shared.Components;
public partial class BLItemCombo : IBLUIOperationHelper, IBLServerDependentComponent
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

    IList<ItemResponse> ItemRes;
    private PropertyConversionResponse<ItemResponse> conversionInfo;

    [Parameter]
    public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

    public BLUIElement LinkedUIObject { get; private set; }

    private bool __forcerender = false;

    private string css_class = "";
    private string IconSvgCode = "";

    private MudAutocomplete<ItemResponse> _refItemCombo;

    public BLItemCombo()
    {
       
      
    }
    protected override async Task OnInitializedAsync()
    {
        if (ObjectHelpers != null && ObjectHelpers.ContainsKey(UIElement.ElementName))
        {
            ObjectHelpers.Remove(UIElement.ElementName);
        }

        if (ObjectHelpers != null)
        {
            ObjectHelpers.Add(UIElement.ElementName, this);
        }

        if (UIElement!=null)
        {
            css_class = (UIElement.IsVisible ? "d-flex" : "d-none") + " align-end " + UIElement.ParentCssClass;
        }
        if (UIElement!=null && !string.IsNullOrEmpty(UIElement.IconCss))
        {
            string[] path = this.UIElement.IconCss.Split('.');
            GetIconByStringName(this.UIElement.IconCss, typeof(Icons));
        }
        
        await ReadCmboData();
       
        await base.OnInitializedAsync();
    }

    public override Task SetParametersAsync(ParameterView parameters)
    {
        return base.SetParametersAsync(parameters);
       
    }



    public async Task ReadCmboData(string SearchQuery="")
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

                var cd = new ItemResponse();
                cd.IsMust = UIElement.IsMust;
                OnComboValueChanged(cd);
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
                UIInterectionArgs<IList<ItemResponse>> args = new UIInterectionArgs<IList<ItemResponse>>();
                args.DataObject = ItemRes;
                await callback.InvokeAsync(args);
            }
        }
    }


    private void OnComboValueChanged(ItemResponse ItemResponse)
    {
        try
        {
            ComboDataObject.SetValueByObjectPath(UIElement.DefaultAccessPath, ItemResponse);
            UIInterectionArgs<ItemResponse> args = new UIInterectionArgs<ItemResponse>();

            if (InteractionLogics != null)
            {

                EventCallback callback;
                if (UIElement.OnClickAction != null && InteractionLogics.TryGetValue(UIElement.OnClickAction, out callback))
                {
                    if (callback.HasDelegate)
                    {
                        args.Caller = this.UIElement.OnClickAction;
                        args.ObjectPath = this.UIElement.DefaultAccessPath;
                        args.DataObject = ItemResponse;
                        args.sender = this;
                        args.InitiatorObject = UIElement;
                        callback.InvokeAsync(args).Wait();
                     
                    }
                }
            }

            if (!(args.DelegateExecuted && args.CancelChange))
            {
                ComboDataObject.SetValueByObjectPath(UIElement.DefaultAccessPath, ItemResponse);
                selecteditemResponse = ItemResponse;
                StateHasChanged();
            }
            else
            {

                if (args.OverrideValue)
                {
                    ItemResponse = args.OverriddenValue;
                    selecteditemResponse = ItemResponse;
                    StateHasChanged();
                }

            }
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
        else
        {
            if (UIElement.IsServerFiltering)
            {
                await ReadCmboData(value);
            }
            return ItemRes.Where(x => x.ItemName!=null && x.ItemName.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }
        
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

    public Task SetValue(object value)
    {
        throw new NotImplementedException();
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
            if (selecteditemResponse.ItemName.Equals(" - - ")) { return ""; }
            return selecteditemResponse.ItemName;
        }
        if (_refItemCombo != null)
        {
            return _refItemCombo.Text;
        }
        return "";
    }

    private void GetIconByStringName(string PropertyName, Type t)
    {

        Type type = null;
        string[] path = PropertyName.Split('.');
        string IconName = null;
        object iconObject = Icons.Material.Filled;
        if (path.Length == 2)
        {
            //This will assume the Filled section
            if (path[0].Equals("Filled"))
            {
                type = Icons.Material.Filled.GetType();
                iconObject = Icons.Material.Filled;
            }
            //This will assume the Filled section
            if (path[0].Equals("Outlined"))
            {
                type = Icons.Material.Outlined.GetType();
                iconObject = Icons.Material.Outlined;
            }

            if (path[0].Equals("TwoTone"))
            {
                type = Icons.Material.TwoTone.GetType();
                iconObject = Icons.Material.TwoTone;
            }

            if (path[0].Equals("Sharp"))
            {
                type = Icons.Material.Sharp.GetType();
                iconObject = Icons.Material.Sharp;
            }


            if (path[0].Equals("Rounded"))
            {
                type = Icons.Material.Rounded.GetType();
                iconObject = Icons.Material.Rounded;
            }

            IconName = path[1];

        }
        else
        {
            type = Icons.Material.Filled.GetType();
            iconObject = Icons.Material.Filled;
            IconName = PropertyName;
        }


        if (type != null)
        {
            PropertyInfo info = type.GetProperty(IconName);
            if (info != null)
            {
                string value = info.GetValue(iconObject) as string;
                IconSvgCode = value;
            }
        }



    }
}
