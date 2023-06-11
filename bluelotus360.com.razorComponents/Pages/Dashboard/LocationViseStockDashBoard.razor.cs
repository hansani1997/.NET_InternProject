using ApexCharts;
using bluelotus360.com.razorComponents.Extensions;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using bluelotus360.com.razorComponents.MB.Shared.Components.Chart;
using BlueLotus360.CleanArchitecture.Application.Validators.Dashboard.LocationWiseStock;
using bluelotus360.Com.commonLib.Helpers;
using BlueLotus360.CleanArchitecture.Application.Validators.Dashboard.LocationWiseStockValidator;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Dashboard;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Pages.Dashboard
{
    public partial class LocationViseStockDashBoard
    {
        #region parameter

        private BLUIElement formDefinition;
        private LocationViseStockRequest location_vise_stocks;

        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;

        private UIBuilder _refBuilder;
        private ApexChart<LocationViseStockResponse> _detailsChart;
        private SelectedData<LocationViseStockResponse> selectedData;
        private IList<LocationViseStockResponse> chartDetails;
        private IList<CodeBaseResponse> locations = new List<CodeBaseResponse>();
        private IList<ItemResponse> items = new List<ItemResponse>();
        EmptyChart empty_chart;


        private ILocationWiseStockValidator validator;
        private BLTable<LocationViseStockResponse> _blTb;
        BLUIElement locWiseStockTable;

        #region flags

        private bool isChartLoading;
        private bool showAlert;
        private bool isExpansionPanelOpen;
        #endregion

        #endregion

        #region General

        public static string LableConvert(decimal value)
        {
            if (value >= 1000000000)
            {
                return (value / 1000000000).ToString("0.0") + " G";
            }

            else if (value >= 1000000 && value < 1000000000)
            {
                return (value / 1000000).ToString("0.0") + " M";
            }
            else if (value >= 1000 && value < 1000000)
            {
                return (value / 1000).ToString("0.0") + " K";
            }
            else
            {
                return value.ToString();
            }
        }

        protected override void OnParametersSet()
        {

            base.OnParametersSet();
        }

        protected override async Task OnInitializedAsync()
        {
            empty_chart = new();
            RefreshGrid();
            RefreshChart();

            long elementKey = 1;
            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);// get element key from url 
            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();
                formrequest.MenuKey = elementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);//get ui elements
                //formDefinition.IsTelerikUI = true;
                if (formDefinition != null)
                {
                    locWiseStockTable = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("LocWiseStockTable")).FirstOrDefault();
                    formDefinition.IsDebugMode = true;
                }

                if (locWiseStockTable != null)
                {
                    locWiseStockTable.Children = formDefinition.Children.Where(x => x.ParentKey == locWiseStockTable.ElementKey).ToList();
                }
            }



            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();

            HookInteractions();

            location_vise_stocks.ElementKey = elementKey;

        }

        private async void RefreshGrid()
        {
            location_vise_stocks = new LocationViseStockRequest();
            validator = new LocationWiseStockValidator(location_vise_stocks);
            await Task.CompletedTask;
        }

        private async void RefreshChart()
        {
            _detailsChart = new();
            chartDetails = new List<LocationViseStockResponse>();
            chartDetails.Clear();
            await Task.CompletedTask;
        }


        private void HookInteractions()
        {

            InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
            _interactionLogic = helper.GenerateEventCallbacks();// generate all event callbacks 
            //AppSettings.RefreshTopBar("Location Wise Stock");
            appStateService._AppBarName = "Location Wise Stock";
        }
        private void UIStateChanged()
        {

            this.StateHasChanged();
        }
        #endregion

        #region ui events

        private void OnSelectedDateChange(UIInterectionArgs<DateTime?> args)
        {
            location_vise_stocks.AsAtDate = args.DataObject;
            UIStateChanged();
        }
        private void OnVatChange(UIInterectionArgs<decimal> args)
        {
            location_vise_stocks.Vat = args.DataObject;
            UIStateChanged();
        }

        private void OnSelectedLocationChanged(UIInterectionArgs<CodeBaseResponse> args)
        {
            location_vise_stocks.Location = args.DataObject;
            UIStateChanged();
        }

        private void OnUnitChanged(UIInterectionArgs<string> args)
        {
            location_vise_stocks.Unit = args.DataObject;
            UIStateChanged();
        }
        private void OnVatInlusivePriceChange(UIInterectionArgs<decimal> args)
        {
            location_vise_stocks.VatInclusivePrice = args.DataObject;
            UIStateChanged();
        }

        private void OnSalesPriceChange(UIInterectionArgs<decimal> args)
        {
            location_vise_stocks.SalesPrice = args.DataObject;
            UIStateChanged();
        }
        private async void SelectedLocation_AfterComboLoaded(UIInterectionArgs<IList<CodeBaseResponse>> args)
        {
            locations = args.DataObject;
            await Task.CompletedTask;
            UIStateChanged();
        }

        private void OnSelectedItemChange(UIInterectionArgs<ItemResponse> args)
        {
            location_vise_stocks.Item = args.DataObject;

            UIStateChanged();
        }

        private async void SelectedItem_AfterComboLoaded(UIInterectionArgs<IList<ItemResponse>> args)
        {
            items = args.DataObject;
            await Task.CompletedTask;
            UIStateChanged();
        }

        private async void LoadChart(UIInterectionArgs<object> args)
        {
            selectedData = null;

            if (validator.CanLoadChart())
            {
                isChartLoading = true;
                chartDetails.Clear();

                if (showAlert)
                {
                    showAlert = false;
                }
                if (isExpansionPanelOpen)
                {
                    isExpansionPanelOpen = false;
                }

                chartDetails = await _dashboardManager.GetLocationViseStocks(location_vise_stocks);
                if (chartDetails != null && chartDetails.Count() > 0)
                {
                    LocationViseStockRequest loaded_stock = new LocationViseStockRequest()
                    {
                        AsAtDate = DateTime.Now,
                        Item = location_vise_stocks.Item,
                        Unit = chartDetails.FirstOrDefault().Unit.UnitName,
                        VatInclusivePrice = chartDetails.FirstOrDefault().VATInclusivePrice,
                        SalesPrice = chartDetails.FirstOrDefault().SalesPrice


                    };
                    if (loaded_stock != null)
                    {
                        location_vise_stocks.CopyFrom(loaded_stock);



                        IList<KeyValuePair<string, IBLUIOperationHelper>> pairs = _objectHelpers.ToList();

                        foreach (KeyValuePair<string, IBLUIOperationHelper> helper in pairs)
                        {
                            await helper.Value.Refresh();

                        }
                        UIStateChanged();
                    }

                    if (!_dashboardManager.IsExceptionthrown())
                    {
                        _detailsChart?.SetRerenderChart();
                    }
                    else
                    {
                        chartDetails.Clear();
                    }
                }




                isChartLoading = false;
                UIStateChanged();
            }
            else
            {
                showAlert = true;
                isExpansionPanelOpen = true;
            }

        }

        private void Cancel(UIInterectionArgs<object> args)
        {
            if (showAlert)
            {
                showAlert = false;
            }
            if (isExpansionPanelOpen)
            {
                isExpansionPanelOpen = false;
            }

            selectedData = null;
            RefreshGrid();

            chartDetails.Clear();
            _detailsChart?.SetRerenderChart();
            UIStateChanged();
        }
        #endregion

        #region Chart Loading



        //private void Refresh()
        //{
        //    _detailsChart?.SetRerenderChart();
        //    UIStateChanged();
        //}
        #endregion

        #region chart reigon
        private void DataPointsSelected(SelectedData<LocationViseStockResponse> selectedData)
        {
            this.selectedData = selectedData;
            _detailsChart?.RenderAsync();
        }
        #endregion

        #region Table

        #endregion

        #region object helpers
        private async Task SetValue(string name, object value)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                await helper.SetValue(value);
                UIStateChanged();
                await Task.CompletedTask;
            }
        }
        #endregion
    }
}
