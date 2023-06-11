using bluelotus360.com.razorComponents.Extensions;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ApexCharts;
using MudBlazor;
using static BlueLotus360.Com.Infrastructure.OrderPlatforms.PickMe.PickmeEntity;
using System.Data.Common;
using System.Xml.Linq;
using BlueLotus.Com.Domain.PartnerEntity;
using bluelotus360.com.razorComponents.MB.Shared.Components.Buttons;
using System.Data;
using Telerik.DataSource.Extensions;
using Microsoft.JSInterop;
using bluelotus360.com.razorComponents.StateManagement;

namespace bluelotus360.com.razorComponents.MB.Shared.Components
{
	public partial class UIGrid<TItem> : ComponentBase, IBLUIOperationHelper
	{
        [Parameter]
        public IList<TItem> DataObject { get; set; }

        [Parameter]
        public BLUIElement FormObject { get; set; }

        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }

        [Parameter]
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }
        [Parameter]
        public SortDirection SortDirection { get; set; } =SortDirection.Ascending;
                
        [Parameter]
        public string Height { get; set; }

        [Parameter]
        public bool IsColumnEditable { get; set; }
        private IList<BLUIElement> GridButtonGroup { get; set; } = new List<BLUIElement>();

        private string searchString1 = "";
        private TItem selectedItem1;
        string rowText = "";
        public BLUIElement LinkedUIObject => throw new System.NotImplementedException();

		WindowSize ws = new WindowSize();
		BLBreakpoint breakpointMargin = new BLBreakpoint();
		protected override async Task OnInitializedAsync()
        {
            GridButtonGroup.Clear();
            foreach (var row in FormObject.Children)
            {
                if (row.ElementType != null && row.ElementType.Equals("GridParameter"))
                {
                    if (row.ElementType != null && row.ElementID.Equals("CommandColumn"))
                    {
                        GridButtonGroup.AddRange(row.Children);
                    }
                }
            }
            this.appStateService.GridLoadStateChanged += this.OnStateChanged;
        }
        

        private void OnStateChanged()
            => this.InvokeAsync(StateHasChanged);

        public void Dispose()
            => this.appStateService.GridLoadStateChanged -= this.OnStateChanged;

        protected override async void OnParametersSet()
        {
			ws = await _jsRuntime.InvokeAsync<WindowSize>("getWindowSize");
            
            if (ws.Width < 600)
			{
				breakpointMargin.BreakpointMargin = MudBlazor.Breakpoint.Xs;
			}
			else if (600 <= ws.Width && ws.Width < 960)
			{
				breakpointMargin.BreakpointMargin = MudBlazor.Breakpoint.Sm;
			}
			else if (960 <= ws.Width && ws.Width < 1280)
			{
				breakpointMargin.BreakpointMargin = MudBlazor.Breakpoint.Md;
			}
			else if (1280 <= ws.Width && ws.Width < 1920)
			{
				breakpointMargin.BreakpointMargin = MudBlazor.Breakpoint.Lg;
			}
			else if (1920 <= ws.Width && ws.Width < 2560)
			{
				breakpointMargin.BreakpointMargin = MudBlazor.Breakpoint.Xl;
			}
			base.OnParametersSet();
            //ReArrangeElements();
        }

        protected override  Task OnAfterRenderAsync(bool firstRender)
        {
            EventCallback callBack;

            if (InteractionLogics.TryGetValue("OnGridInitilize_" + FormObject._internalElementName, out callBack))
            {
                if (callBack.HasDelegate)
                {
                    callBack.InvokeAsync();
                }
            }

            return base.OnAfterRenderAsync(firstRender);


        }

        private string SetGridValues(Object obj, string name)
        {
            string column = "";
            if (name == null || name.Trim().Length < 2)
            {
                column = String.Empty;

            }

            Type type = obj.GetType();

            if(name != null || name == " ") { 

                foreach (string part in name.Split('.'))
                {
                    if (obj == null) { column = String.Empty; }


                    PropertyInfo info = type.GetProperty(part);
                    object val = info?.GetValue(obj, null);

                    if (val != null)
                    {
                        if (info == null)
                        {
                            column = String.Empty;
                        }
                        else if (val.IsNumericType())
                        {
                            column = decimal.Parse(val.ToString()).ToString("N2");
                        }
                        else
                        {
                            column = val.ToString();
                        }
                    }



                    type = info?.PropertyType;
                    obj = info?.GetValue(obj, null);
                }
            }
            return column;
        }

        private bool IsNumericColumn(Object obj, string name)
        {
            bool isnumeric = false;

            Type type = obj.GetType();

            foreach (string part in name.Split('.'))
            {
                if (obj == null) { isnumeric = false; }


                PropertyInfo info = type.GetProperty(part);
                object val = info?.GetValue(obj, null);
                if (val != null)
                {
                    if (info == null)
                    {
                        isnumeric = false;
                    }

                    else if (val.IsNumericType())
                    {
                        isnumeric = true;
                    }
                    else
                    {
                        isnumeric = false;
                    }
                }

                type = info?.PropertyType;
                obj = info?.GetValue(obj, null);
            }
            return isnumeric;
        }

        
        private void ReArrangeElements()
        {
            var childsHash = FormObject.Children.ToLookup(elem => elem.ParentKey);
            foreach (var child in FormObject.Children)
            {
                child.Children = childsHash[child.ElementKey].ToList();
            }
            BLUIElement form = FormObject.Children.Where(x => x.ElementKey == FormObject.ElementKey).FirstOrDefault();
            FormObject = form;

        }

        public void ResetToInitialValue()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateVisibility(bool IsVisible)
        {
            throw new System.NotImplementedException();
        }

        public void ToggleEditable(bool IsEditable)
        {
            throw new System.NotImplementedException();
        }

        public async Task Refresh()
        {
            //GridRef?.Rebind();
            //StateHasChanged();
            //await Task.CompletedTask;
            throw new System.NotImplementedException();
        }

        public Task FocusComponentAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task SetValue(object value)
        {
            throw new System.NotImplementedException();
        }

        private bool FilterFunc1(TItem element) => FilterFunc(element, searchString1);

        private bool FilterFunc(TItem element, string searchString)
        {
            bool found=false;
            if (string.IsNullOrWhiteSpace(searchString))
            {
                found = true;
            }
            else
            {
                if (element != null)
                {
                    foreach (var header in FormObject.Children)
                    {
                        if (header.DefaultAccessPath == null || header.DefaultAccessPath.Trim().Length < 2)
                        {
                            //found = false;

                        }
                        else
                        {
                            Type type = element.GetType();
                            object obj = element;
                            foreach (string part in header.DefaultAccessPath.Split('.'))
                            {
                                PropertyInfo info = type.GetProperty(part);
                                object val = info?.GetValue(obj, null);

                                if (val != null)
                                {
                                    if (info == null)
                                    {
                                        found = false;
                                    }
                                    if (val.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
                                    {
                                        found = true;
                                        break;
                                    }
                                    obj = val;
                                }

                                type = info?.PropertyType;

                            }
                        }


                    }


                }
            }    

            
            return found;
        }

        private object SortingFunc(TItem sortobj,string col)
        {
            Type type = sortobj?.GetType();
            object obj = sortobj;
            object val = new object();

            foreach (string part in col.Split('.'))
            {
                PropertyInfo info = type?.GetProperty(part);
                val = info?.GetValue(obj, null);

                if (val != null)
                {
                    obj = val;
                }

                type = info?.PropertyType;

            }
            return val;
        }
    }
}
