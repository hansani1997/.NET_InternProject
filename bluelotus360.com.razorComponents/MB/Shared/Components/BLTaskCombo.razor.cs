﻿using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain;
using bluelotus360.Com.commonLib.Routes;
using bluelotus360.com.razorComponents.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace bluelotus360.com.razorComponents.MB.Shared.Components
{
    public partial class BLTaskCombo : IBLUIOperationHelper, IBLServerDependentComponent
    {
        [Parameter]
        public BLUIElement UIElement { get; set; }

        [Parameter]
        public object ComboDataObject { get; set; }

        [Parameter]
        public EventCallback OnComboChanged { get; set; }

        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }
        [Parameter] public IDictionary<string, string> DynamicBindings { get; set; }

        private TaskResponse selectedtaskResponse = new TaskResponse();

        IList<TaskResponse> _taskResponse;

        [Parameter]
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        public BLUIElement LinkedUIObject { get; private set; }

        private bool __forcerender = false;

        private string css_class = "";
        private string IconSvgCode = "";

        private MudAutocomplete<TaskResponse> _refTaskCombo;
        private PropertyConversionResponse<TaskResponse> conversionInfo;

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
            await ReadCmboData();
            await base.OnInitializedAsync();
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

            _taskResponse = await _projectProfileMobileManager.GetTasksByProject(requestDTO);

            if (InteractionLogics != null)
            {

                EventCallback callback;
                if (InteractionLogics.TryGetValue(UIElement.OnAfterComboLoad, out callback))
                {
                    if (callback.HasDelegate)
                    {
                        UIInterectionArgs<IList<TaskResponse>> args = new UIInterectionArgs<IList<TaskResponse>>();
                        args.DataObject = _taskResponse;
                        await callback.InvokeAsync(args);
                    }
                }
                else
                {

                }
            }

            if (_taskResponse.Count > 0)
            {
                selectedtaskResponse = this._taskResponse.Where(x => x.IsDefault).FirstOrDefault();

                if (selectedtaskResponse != null)
                {
                    selectedtaskResponse.IsMust = UIElement.IsMust;
                    OnComboValueChanged(selectedtaskResponse);

                }
                else
                {

                    var cd = new TaskResponse();
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
                    UIInterectionArgs<IList<TaskResponse>> args = new UIInterectionArgs<IList<TaskResponse>>();
                    args.DataObject = _taskResponse;
                    await callback.InvokeAsync(args);
                }
            }
        }
        private void OnComboValueChanged(TaskResponse projectResponse)
        {
            try
            {
                ComboDataObject.SetValueByObjectPath(UIElement.DefaultAccessPath, projectResponse);
                UIInterectionArgs<TaskResponse> args = new UIInterectionArgs<TaskResponse>();

                if (InteractionLogics != null)
                {

                    EventCallback callback;
                    if (UIElement.OnClickAction != null && InteractionLogics.TryGetValue(UIElement.OnClickAction, out callback))
                    {
                        if (callback.HasDelegate)
                        {
                            args.Caller = this.UIElement.OnClickAction;
                            args.ObjectPath = this.UIElement.DefaultAccessPath;
                            args.DataObject = projectResponse;
                            args.sender = this;
                            args.InitiatorObject = UIElement;
                            callback.InvokeAsync(args).Wait();

                        }
                    }
                }

                if (!(args.DelegateExecuted && args.CancelChange))
                {
                    ComboDataObject.SetValueByObjectPath(UIElement.DefaultAccessPath, projectResponse);
                    selectedtaskResponse = projectResponse;
                    if (UIElement.IsDynamicalyLoaded && DynamicBindings != null && DynamicBindings.ContainsKey(UIElement.ElementName))
                    {
                        args.ObjectPath = this.UIElement.DefaultAccessPath;
                        args.DataObject = projectResponse;
                        args.InitiatorObject = UIElement;
                        DynamicBindings[UIElement.ElementName] = JsonConvert.SerializeObject(args);
                    }
                    StateHasChanged();
                }
                else
                {

                    if (args.OverrideValue)
                    {
                        projectResponse = args.OverriddenValue;
                        selectedtaskResponse = projectResponse;
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
            conversionInfo = ComboDataObject.GetPropObject<TaskResponse>(UIElement.DefaultAccessPath);
            if (conversionInfo.IsConversionSuccess)
            {
                selectedtaskResponse = conversionInfo.Value;
            }
            return base.OnParametersSetAsync();
        }

        private async Task<IEnumerable<TaskResponse>> OnComboSearch(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new List<TaskResponse>();

            }
            else
            {
                if (UIElement.IsServerFiltering)
                {
                    await ReadCmboData(value);
                }
                return _taskResponse.Where(x => x.TaskName != null && x.TaskName.Contains(value, StringComparison.InvariantCultureIgnoreCase));
            }

        }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            return base.SetParametersAsync(parameters);
        }

        private string GetComboDisplayText()
        {
            if (selectedtaskResponse != null)
            {
                if (selectedtaskResponse.TaskName.Equals(" - - ")) { return ""; }
                return selectedtaskResponse.TaskName;
            }
            if (_refTaskCombo != null)
            {
                return _refTaskCombo.Text;
            }
            return "";
        }
        public void ResetToInitialValue()
        {
            this.selectedtaskResponse = new TaskResponse();
            __forcerender = true;
            this.StateHasChanged();
            __forcerender = false;
        }


        public async Task Refresh()
        {
            await Task.CompletedTask;
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





        public async Task FetchData(bool useLocalstorage = false)
        {
            await ReadCmboData();
            StateHasChanged();
            await Task.CompletedTask;
        }

        public Task FocusComponentAsync()
        {
            throw new NotImplementedException();
        }

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

            }
            else
            {
                iconObject = new Icons.Material.Filled();
                IconName = PropertyName;
            }

            type = iconObject.GetType();
            if (type != null)
            {
                //PropertyInfo info = type.GetProperty(IconName);
                FieldInfo info = type.GetField(IconName);
                if (info != null)
                {
                    string value = info.GetValue(iconObject) as string;
                    IconSvgCode = value;
                }
            }



        }
    }
}
