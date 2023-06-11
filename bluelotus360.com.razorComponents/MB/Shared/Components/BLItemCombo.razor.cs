using bluelotus360.com.razorComponents.Extensions;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudBlazor;
using System.Reflection;
using bluelotus360.Com.commonLib.Routes;
using bluelotus360.Com.commonLib.Setting;
using bluelotus360.Com.MauiSupports.Models;
using Newtonsoft.Json;

namespace bluelotus360.com.razorComponents.MB.Shared.Components
{
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
        [Parameter] public IDictionary<string, string> DynamicBindings { get; set; }
        public BLUIElement LinkedUIObject { get; private set; }

        private bool __forcerender = false;

        private string css_class = "";
        private string IconSvgCode = "";

        private MudAutocomplete<ItemResponse> _refItemCombo;

        private List<ItemResponse> blIMenus = new List<ItemResponse>();

        protected override async Task OnInitializedAsync()
        {
            ComboInteraction comboInteraction = new ComboInteraction();
            comboInteraction.eventName = "OnInitializedAsync";
            comboInteraction.internalElementName = UIElement._internalElementName;
            
            comboInteraction.eventStart = DateTime.Now;

            if (ObjectHelpers != null && ObjectHelpers.ContainsKey(UIElement.ElementName))
            {
                ObjectHelpers.Remove(UIElement.ElementName);
            }

            if (ObjectHelpers != null)
            {
                ObjectHelpers.Add(UIElement.ElementName, this);
            }

            if (UIElement != null)
            {
                css_class = (UIElement.IsVisible ? "d-flex" : "d-none") + " align-end " + UIElement.ParentCssClass;
            }
            if (UIElement != null && !string.IsNullOrEmpty(UIElement.IconCss))
            {
                string[] path = this.UIElement.IconCss.Split('.');
                GetIconByStringName(this.UIElement.IconCss, typeof(Icons));
            }

            if (UIElement.IsDynamicalyLoaded && DynamicBindings != null)
            {
                if (DynamicBindings.ContainsKey(UIElement.ElementName))
                {
                    DynamicBindings.Remove(UIElement.ElementName);

                }
                DynamicBindings.Add(UIElement.ElementName, "");
            }
            //comboInteraction.eventAction = "OnInitializedAsync";

            await ReadCmboData();
            await base.OnInitializedAsync();

            comboInteraction.eventEnd = DateTime.Now;
            await _comboEventStorage.SaveItemAsync(comboInteraction);
        }

        public async Task ReadCmboData(string SearchQuery = "")
        {
            ComboInteraction comboInteraction = new ComboInteraction();
            comboInteraction.eventName = "Read Combo Data";
            comboInteraction.internalElementName = UIElement._internalElementName;
            comboInteraction.eventStart = DateTime.Now;

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
                    OnComboValueChangedAsync(selecteditemResponse);

                }
                else
                {

                    var cd = new ItemResponse();
                    cd.IsMust = UIElement.IsMust;
                    OnComboValueChangedAsync(cd);
                }

                await OnDataLoadedCompleted();


                StateHasChanged();
            }

            comboInteraction.eventEnd = DateTime.Now;
            comboInteraction.eventAction = GetComboDisplayText();
            await _comboEventStorage.SaveItemAsync(comboInteraction);

        }


        //public async Task ReadCmboData(string SearchQuery = "")
        //{
        //    ComboRequestDTO requestDTO = new ComboRequestDTO();
        //    requestDTO.SearchQuery = SearchQuery;
        //    requestDTO.RequestingElementKey = UIElement.ElementKey;
        //    requestDTO.RequestingURL = BaseEndpoint.BaseURL + UIElement.GetPathURL();
        //    if (InteractionLogics != null)
        //    {

        //        EventCallback callback;
        //        if (InteractionLogics.TryGetValue(UIElement.OnBeforeComboLoad, out callback))
        //        {
        //            if (callback.HasDelegate)
        //            {
        //                UIInterectionArgs<ComboRequestDTO> args = new UIInterectionArgs<ComboRequestDTO>();
        //                args.DataObject = requestDTO;
        //                await callback.InvokeAsync(args);
        //                if (args.DataObject.CancelRead)
        //                {
        //                    return;
        //                }
        //            }
        //        }
        //        else
        //        {

        //        }
        //    }

        //    ItemRes = await _comboManager.GetItemResponses(requestDTO);

        //    if (InteractionLogics != null)
        //    {

        //        EventCallback callback;
        //        if (InteractionLogics.TryGetValue(UIElement.OnAfterComboLoad, out callback))
        //        {
        //            if (callback.HasDelegate)
        //            {
        //                UIInterectionArgs<IList<ItemResponse>> args = new UIInterectionArgs<IList<ItemResponse>>();
        //                args.DataObject = ItemRes;
        //                await callback.InvokeAsync(args);
        //            }
        //        }
        //        else
        //        {

        //        }
        //    }

        //    if (ItemRes.Count > 0)
        //    {
        //        selecteditemResponse = this.ItemRes.Where(x => x.IsDefault).FirstOrDefault();

        //        if (selecteditemResponse != null)
        //        {
        //            selecteditemResponse.IsMust = UIElement.IsMust;
        //            OnComboValueChanged(selecteditemResponse);

        //        }
        //        else
        //        {

        //            var cd = new ItemResponse();
        //            cd.IsMust = UIElement.IsMust;
        //            OnComboValueChanged(cd);
        //        }

        //        await OnDataLoadedCompleted();


        //        StateHasChanged();
        //    }



        //}

        private async Task OnDataLoadedCompleted()
        {
            ComboInteraction comboInteraction = new ComboInteraction();
            comboInteraction.eventName = "ON Data load completed";
            comboInteraction.internalElementName = UIElement._internalElementName;
            comboInteraction.eventStart = DateTime.Now;

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
            comboInteraction.eventEnd = DateTime.Now;
            comboInteraction.eventAction = GetComboDisplayText();
            await _comboEventStorage.SaveItemAsync(comboInteraction);
        }

        private async Task OnComboValueChangedAsync(ItemResponse ItemResponse)
        {
            ComboInteraction comboInteraction = new ComboInteraction();
            comboInteraction.eventName = "ON Combo Value Changed";
            comboInteraction.internalElementName = UIElement._internalElementName;
            comboInteraction.eventStart = DateTime.Now;

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

                    if (UIElement.IsDynamicalyLoaded && DynamicBindings != null && DynamicBindings.ContainsKey(UIElement.ElementName))
                    {
                        args.ObjectPath = this.UIElement.DefaultAccessPath;
                        args.DataObject = ItemResponse;
                        args.InitiatorObject = UIElement;
                        DynamicBindings[UIElement.ElementName] = JsonConvert.SerializeObject(args);
                    }
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
            comboInteraction.eventEnd = DateTime.Now;
            comboInteraction.eventAction = GetComboDisplayText();
            await _comboEventStorage.SaveItemAsync(comboInteraction);
        }

        protected async override Task OnParametersSetAsync()
        {
            ComboInteraction comboInteraction = new ComboInteraction();
            comboInteraction.eventName = "On Parameters Set";
            comboInteraction.internalElementName = UIElement._internalElementName;
            comboInteraction.eventStart = DateTime.Now;

            int c = this.ComboDataObject.GetHashCode();
            conversionInfo = ComboDataObject.GetPropObject<ItemResponse>(UIElement.DefaultAccessPath);
            if (conversionInfo.IsConversionSuccess)
            {
                selecteditemResponse = conversionInfo.Value;
            }
            comboInteraction.eventEnd = DateTime.Now;
            comboInteraction.eventAction = GetComboDisplayText();
            await _comboEventStorage.SaveItemAsync(comboInteraction);
            await base.OnParametersSetAsync();
        }

        private async Task<IEnumerable<ItemResponse>> OnComboSearch(string value)
        {
            ComboInteraction comboInteraction = new ComboInteraction();
            comboInteraction.eventName = "On Combo Search";
            comboInteraction.internalElementName = UIElement._internalElementName;
            comboInteraction.eventStart = DateTime.Now;

            if (string.IsNullOrEmpty(value))
            {
                //return new List<ItemResponse>();
                //return blIMenus;
                comboInteraction.eventEnd = DateTime.Now;
                comboInteraction.eventAction = GetComboDisplayText();
                await _comboEventStorage.SaveItemAsync(comboInteraction);
                return ItemRes;
            }

            else
            {
                if (UIElement.IsServerFiltering)
                {
                    await ReadCmboData(value);
                }
                comboInteraction.eventEnd = DateTime.Now;
                await _comboEventStorage.SaveItemAsync(comboInteraction);
                return ItemRes.Where(x => x.ItemName != null && x.ItemName.Contains(value, StringComparison.InvariantCultureIgnoreCase));
            }
        }

        public async override Task SetParametersAsync(ParameterView parameters)
        {
            //ComboInteraction comboInteraction = new ComboInteraction();
            //comboInteraction.eventName = "Set Parameters Async";
            //if(UIElement != null)
            //{
            //    comboInteraction.internalElementName = UIElement._internalElementName;
            //}
            //else
            //{
            //    comboInteraction.internalElementName = "UIElement is null";
            //}
            //comboInteraction.eventStart = DateTime.Now;
            //comboInteraction.eventEnd = DateTime.Now;
            //await _comboEventStorage.SaveItemAsync(comboInteraction);
            await base.SetParametersAsync(parameters);
        }

        public async Task FetchData(bool useLocalstorage = false)
        {
            await ReadCmboData();
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

        public Task FocusComponentAsync()
        {
            throw new NotImplementedException();
        }

        public async Task Refresh()
        {
            ComboInteraction comboInteraction = new ComboInteraction();
            comboInteraction.eventName = "Refresh";
            comboInteraction.internalElementName = UIElement._internalElementName;
            comboInteraction.eventStart = DateTime.Now;

            await Task.CompletedTask;

            comboInteraction.eventEnd = DateTime.Now;
            comboInteraction.eventAction = GetComboDisplayText();
            await _comboEventStorage.SaveItemAsync(comboInteraction);
        }

        public void ResetToInitialValue()
        {
            this.selecteditemResponse = new ItemResponse();
            __forcerender = true;
            this.StateHasChanged();
            __forcerender = false;
        }

        public Task SetDataSource(object DataSource)
        {
            throw new NotImplementedException();
        }

        public Task SetValue(object value)
        {
            throw new NotImplementedException();
        }

        public void ToggleEditable(bool IsEditable)
        {
            throw new NotImplementedException();
        }

        public void UpdateVisibility(bool IsVisible)
        {
            this.UIElement.IsVisible = IsVisible;
            css_class = (UIElement.IsVisible ? "d-flex" : "d-none") + " align-end ";
            StateHasChanged();
        }

        //private void GetIconByStringName(string PropertyName, Type t)
        //{

        //    Type type = null;
        //    string[] path = PropertyName.Split('.');
        //    string IconName = null;
        //    object iconObject = Icons.Material.Filled;
        //    if (path.Length == 2)
        //    {
        //        //This will assume the Filled section
        //        if (path[0].Equals("Filled"))
        //        {
        //            type = Icons.Material.Filled.GetType();
        //            iconObject = Icons.Material.Filled;
        //        }
        //        //This will assume the Filled section
        //        if (path[0].Equals("Outlined"))
        //        {
        //            type = Icons.Material.Outlined.GetType();
        //            iconObject = Icons.Material.Outlined;
        //        }

        //        if (path[0].Equals("TwoTone"))
        //        {
        //            type = Icons.Material.TwoTone.GetType();
        //            iconObject = Icons.Material.TwoTone;
        //        }

        //        if (path[0].Equals("Sharp"))
        //        {
        //            type = Icons.Material.Sharp.GetType();
        //            iconObject = Icons.Material.Sharp;
        //        }


        //        if (path[0].Equals("Rounded"))
        //        {
        //            type = Icons.Material.Rounded.GetType();
        //            iconObject = Icons.Material.Rounded;
        //        }

        //        IconName = path[1];

        //    }
        //    else
        //    {
        //        type = Icons.Material.Filled.GetType();
        //        iconObject = Icons.Material.Filled;
        //        IconName = PropertyName;
        //    }


        //    if (type != null)
        //    {
        //        PropertyInfo info = type.GetProperty(IconName);
        //        if (info != null)
        //        {
        //            string value = info.GetValue(iconObject) as string;
        //            IconSvgCode = value;
        //        }
        //    }



        //}

        private void GetIconByStringName(string PropertyName, Type t)
        {
            string svgcode = "";
            Type type = t;
            string[] path = PropertyName.Split(".");
            string IconName = "";
            object iconObject = new Icons.Material.Filled();
            if (path.Length == 2)
            {
                //This will assume the Filled section
                if (path[0].Equals("Filled"))
                {
                    iconObject = new Icons.Material.Filled();
                }
                //This will assume the Filled section
                if (path[0].Equals("Outlined"))
                {
                    iconObject = new Icons.Material.Outlined();
                }

                if (path[0].Equals("TwoTone"))
                {
                    iconObject = new Icons.Material.TwoTone();

                }

                if (path[0].Equals("Sharp"))
                {
                    iconObject = new Icons.Material.Sharp();
                }


                if (path[0].Equals("Rounded"))
                {
                    iconObject = new Icons.Material.Rounded();
                }

                IconName = path[1];
				IconSvgCode = IconConversion.ConvertMudIconToFontAwesome(IconName);

			}
            else
            {
                iconObject = new Icons.Material.Filled();
                IconName = PropertyName;
                IconSvgCode = IconName;

			}

			//type = iconObject.GetType();
			//if (type != null)
			//{
			//    //PropertyInfo info = type.GetProperty(IconName);
			//    FieldInfo info = type.GetField(IconName);
			//    if (info != null)
			//    {
			//        string value = info.GetValue(iconObject) as string;
			//        IconSvgCode = value;
			//    }
 			//}

			


		}
    }
}
