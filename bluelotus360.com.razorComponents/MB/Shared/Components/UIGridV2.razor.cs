using bluelotus360.com.razorComponents.Extensions;
using BlueLotus360.CleanArchitecture.Domain;
using bluelotus360.com.razorComponents.StateManagement;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telerik.DataSource.Extensions;
using MudBlazor;
using BL10.CleanArchitecture.Domain.DTO;
using bluelotus360.Com.commonLib.Routes;
using ApexCharts;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Telerik.DataSource;
using BlueLotus.Com.Domain.Entity;

namespace bluelotus360.com.razorComponents.MB.Shared.Components
{
    public partial class UIGridV2<TItem> : ComponentBase, IBLUIOperationHelper
    {

        [Parameter] public IList<TItem> DataObject { get; set; }=new List<TItem>(); 

        [Parameter] public BLUIElement FormObject { get; set; }

        [Parameter] public IDictionary<string, EventCallback> InteractionLogics { get; set; }

        [Parameter] public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }
        [Parameter] public string Height { get; set; }

        [Parameter] public bool IsColumnEditable { get; set; }
        [Parameter] public bool IsServerFilterEnabled { get; set; }
        [Parameter] public bool CanResizeContainer { get; set; }

        private int GridCount { get; set; } = 0;
        public BLUIElement LinkedUIObject => throw new NotImplementedException();

        private IList<BLUIElement> GridButtonGroup { get; set; } = new List<BLUIElement>();

        private string searchString1 = "",severfiltersearch="";
        private TItem selectedItem1;
        string rowText = "";
        bool isInServerFilterMode, isTableLoading;

        WindowSize ws = new WindowSize();
        BLBreakpoint breakpointMargin = new BLBreakpoint();
        private MudDataGrid<TItem> _table=new MudDataGrid<TItem>();
        private IList<TItem> _mainList=new List<TItem>();  
        private IList<string> _emptyList=new List<string>() { "empty1","empty2"} ;
        protected override async Task OnInitializedAsync()
        {
            if (IsServerFilterEnabled)
            {
                ServerFilter();
            }
                
            this.appStateService.GridLoadStateChanged += this.OnStateChanged;
        }


        private void OnStateChanged()
            => this.InvokeAsync(StateHasChanged);

        public void Dispose()
            => this.appStateService.GridLoadStateChanged -= this.OnStateChanged;

        protected override async void OnParametersSet()
        {
            if (!IsServerFilterEnabled)
            {
                _mainList = DataObject;
            }
            
            base.OnParametersSet();
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
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
            bool found = false;
            isInServerFilterMode = false;

            if (string.IsNullOrWhiteSpace(searchString))
            {
                found = true;
            }
            else
            {
                if (element != null )
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

        private object SortingFunc(TItem sortobj, string col)
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

        private async void PageChanged(int i)
        {
            if (IsServerFilterEnabled)
            {
                ServerFilter(i);
            }

            //_table.NavigateTo(Page.Next);
        }

        public async void ServerFilter(int i=1,string searchQuery="")
        {
                BaseServerFilterInfo request = new BaseServerFilterInfo();
                request.RequestingURL = BaseEndpoint.BaseURL + FormObject.GetPathURL();
                request.Page = i;
                request.ObjKy = (int)FormObject.ElementKey;
                request.Filter= searchQuery;
                string jsonSFList = await _itemProfileMobileManagerV3.GetItemServerfilterDetails(request);
                if (!string.IsNullOrEmpty(jsonSFList))
                {
                    _mainList = JsonConvert.DeserializeObject<List<TItem>>(jsonSFList);
                }
                object obj = _mainList.FirstOrDefault();
                GridCount = obj != null ?  Convert.ToInt32( obj.GetType().GetProperty("Count")?.GetValue(obj, null)) : 0;

                StateHasChanged();
            
        }

        private  void SearchOnSF(string searchString) 
        {
            isTableLoading = true;
            isInServerFilterMode = true;
            if (string.IsNullOrEmpty(searchString))
            {
                isInServerFilterMode = false;
                ServerFilter();
              
            }
            else
            {
                ServerFilter(1, ("ItmCd~contains~" + searchString));
            }

            isTableLoading = false;
            StateHasChanged();
        }

        private async Task ClearFilterAsync(FilterContext<TItem> context,BLUIElement col)
        {
            col.ColumnSearchString = string.Empty;
            ServerFilter();
            col.IsFilterOpen = false;
        }

        private async Task ApplyFilterAsync(FilterContext<TItem> context, BLUIElement col)
        {
            if (string.IsNullOrEmpty(col.ColumnSearchString))
            {
                ServerFilter();

            }
            else
            {
                ServerFilter(1, ($"{col.DefaultAccessPath}~{col.ColumnFilteringCriteriaType}~{ col.ColumnSearchString}"));
            }

            StateHasChanged();
            col.IsFilterOpen = false;
        }


    }

}
