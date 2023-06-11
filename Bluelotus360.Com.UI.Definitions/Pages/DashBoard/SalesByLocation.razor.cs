using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using ApexCharts;
using System.Linq;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Dashboard;
using BlueLotus360.Com.UI.Definitions.Extensions;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Chart;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.Com.UI.Definitions.MB.Settings;
using Microsoft.JSInterop;

namespace BlueLotus360.Com.UI.Definitions.Pages.DashBoard
{
    public partial class SalesByLocation
    {
        #region parameter

        private BLUIElement formDefinition;
        private SalesDetails sales_request;
        private SalesRepDetailsForSalesByLocation rep_request;

        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;

        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;

        private UIBuilder _refBuilder;
        private ApexChart<SalesByLocationResponse> _detailsChart;
        ApexChartOptions<SalesByLocationResponse> opt;
        private IList<SalesDetails> salesResponse;
        private IList<SalesByLocationResponse> salesBylocation;
        private IList<SalesRepDetailsForSalesByLocationResponse> repDetails;
        EmptyChart empty_chart;
        private Legend legend;
        private long elementKey;

        #region flags
        private bool isChartLoading;
        private bool showAlert;
        private bool isExpansionPanelOpen;
        bool fixed_header = true;
        #endregion

        #endregion

        #region General
        protected override async Task OnInitializedAsync()
        {
            elementKey = 1;
            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);// get element key from url 
            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();
                formrequest.MenuKey = elementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);//get ui elements
            }

            formDefinition.IsDebugMode = true;
            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();

            legend = new Legend
            {
                Position = LegendPosition.Bottom,

            };
            opt = new ApexChartOptions<SalesByLocationResponse> { Legend = legend };
            HookInteractions();

            RefreshGrid();
            RefreshChart();


            await GetSalesHeaderDetails();
            await LoadChart();

            empty_chart = new();

            UIStateChanged();
        }

        private async void RefreshGrid()
        {
            sales_request = new SalesDetails();
            rep_request = new();
            repDetails = new List<SalesRepDetailsForSalesByLocationResponse>();
            salesResponse = new List<SalesDetails>();

            sales_request = new SalesDetails()
            {
                ElementKey = formDefinition.ElementKey,
                FromDate = new DateTime(2021, 1, 1),
                ToDate = new DateTime(2021, 10, 20),
                BusinessUnit = new CodeBaseResponse
                {
                    CodeKey = 400883,
                    CodeName = "Finance BU - Finance BU",
                },

            };

            rep_request = new SalesRepDetailsForSalesByLocation()
            {
                ElementKey = formDefinition.ElementKey,
                FromDate = new DateTime(2021, 1, 1),
                ToDate = new DateTime(2021, 10, 20),
                BusinessUnit = new CodeBaseResponse
                {
                    CodeKey = 400883,
                    CodeName = "Finance BU - Finance BU",
                },

            };
            await Task.CompletedTask;
        }

        private async void RefreshChart()
        {
            _detailsChart = new();
            salesBylocation = new List<SalesByLocationResponse>();
            salesBylocation.Clear();
            await Task.CompletedTask;
        }

        private void HookInteractions()
        {

            InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
            _interactionLogic = helper.GenerateEventCallbacks();// generate all event callbacks 
            //AppSettings.RefreshTopBar("Sales By Location");
            appStateService._AppBarName = "Sales By Location";
        }
        private void UIStateChanged()
        {
            this.StateHasChanged();
        }
        #endregion

        #region ui events 
        private void OnFromDateClick(UIInterectionArgs<DateTime?> args)
        {
            sales_request.FromDate = args.DataObject;
            rep_request.FromDate = args.DataObject;
            UIStateChanged();
        }

        private void OnToDateClick(UIInterectionArgs<DateTime?> args)
        {
            sales_request.ToDate = args.DataObject;
            rep_request.ToDate = args.DataObject;
            UIStateChanged();
        }

        private void OnBuComboClick(UIInterectionArgs<CodeBaseResponse> args)
        {
            sales_request.BusinessUnit.CodeKey = args.DataObject.CodeKey;
            rep_request.BusinessUnit.CodeKey = args.DataObject.CodeKey;
            UIStateChanged();
        }

        private void OnLocationChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            sales_request.Location = args.DataObject;
            rep_request.Location = args.DataObject;
            UIStateChanged();
        }

        private async void OnLoadClick(UIInterectionArgs<object> args)
        {
            isChartLoading = true;

            await GetSalesHeaderDetails();
            await LoadChart();
            await LoadRepDetails();


            isChartLoading = false;

            UIStateChanged();
        }

        private async void OnClickTodayBtn(UIInterectionArgs<object> args)
        {
            sales_request.FromDate = DateTime.Now;
            rep_request.FromDate = DateTime.Now;

            sales_request.ToDate = DateTime.Now;
            rep_request.ToDate = DateTime.Now;


            await SetValue("FromDate", sales_request.FromDate);
            await SetValue("ToDate", sales_request.ToDate);


            await GetSalesHeaderDetails();
            await LoadChart();
            await LoadRepDetails();

            UIStateChanged();
        }

        private async void OnClickMonthBtn(UIInterectionArgs<object> args)
        {
            DateTime now = DateTime.Now;
            sales_request.FromDate = new DateTime(now.Year, now.Month, 1);
            sales_request.ToDate = sales_request.FromDate?.AddMonths(1).AddDays(-1);

            rep_request.FromDate = new DateTime(now.Year, now.Month, 1);
            rep_request.ToDate = sales_request.FromDate?.AddMonths(1).AddDays(-1);

            await SetValue("FromDate", sales_request.FromDate);
            await SetValue("ToDate", sales_request.ToDate);

            await GetSalesHeaderDetails();
            await LoadChart();
            await LoadRepDetails();

            UIStateChanged();
        }

        private async void OnClickYearBtn(UIInterectionArgs<object> args)
        {
            int year = DateTime.Now.Year;
            sales_request.FromDate = new DateTime(year, 1, 1);
            sales_request.ToDate = new DateTime(year, 12, 31);

            rep_request.FromDate = new DateTime(year, 1, 1);
            rep_request.ToDate = new DateTime(year, 12, 31);

            await SetValue("FromDate", sales_request.FromDate);
            await SetValue("ToDate", sales_request.ToDate);

            await GetSalesHeaderDetails();
            await LoadChart();
            await LoadRepDetails();


            UIStateChanged();
        }
        #endregion

        #region sales actions
        private async Task GetSalesHeaderDetails()
        {
            salesResponse = await _dashboardManager.GetSalesDetails(sales_request);
            await SetValue("Cash", salesResponse.FirstOrDefault()?.TotalCashAmt);
            await SetValue("Card", salesResponse.FirstOrDefault()?.TotalCardAmt);
            await SetValue("OtherAmt", salesResponse.FirstOrDefault()?.OtherAmt);
            await SetValue("Customer", salesResponse.FirstOrDefault()?.TotalNewCustomers);

            UIStateChanged();
        }

        private async Task LoadChart()
        {
            salesBylocation.Clear();

            salesBylocation = await _dashboardManager.GetLocationWiseSalesDetails(sales_request);

            //salesBylocation=PartionizedSalesByLocation(salesBylocation);

            if (!_dashboardManager.IsExceptionthrown())
            {
                _detailsChart?.SetRerenderChart();
            }
            else
            {
                salesBylocation.Clear();
            }


            UIStateChanged();

        }

        private IList<SalesByLocationResponse> PartionizedSalesByLocation(IList<SalesByLocationResponse> salesBylocation)
        {
            IList<SalesByLocationResponse> partionizedsalesBylocation = salesBylocation.OrderBy(x => x.TotalSalesAmt).Take(10).ToList();

            decimal totalSalesAmtsum = 0;
            foreach (var itm in salesBylocation.OrderBy(x => x.TotalSalesAmt).Skip(10))
            {
                totalSalesAmtsum += itm.TotalSalesAmt;
            }

            partionizedsalesBylocation.Add(new SalesByLocationResponse(new CodeBaseResponse(), totalSalesAmtsum));

            return partionizedsalesBylocation;
        }

        private async Task LoadRepDetails()
        {
            repDetails = await _dashboardManager.GetLocationWiseSalesRepDetails(rep_request);
            UIStateChanged();

        }
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
