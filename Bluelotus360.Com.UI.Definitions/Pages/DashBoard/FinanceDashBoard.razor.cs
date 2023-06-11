using ApexCharts;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components;
using BlueLotus360.CleanArchitecture.Domain.Entities.Dashboard;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Chart;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.Com.UI.Definitions.MB.Settings;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Dialogs.Chart.Finance;
using Microsoft.JSInterop;

namespace BlueLotus360.Com.UI.Definitions.Pages.DashBoard
{
    public partial class FinanceDashBoard
    {
        #region parameter

        private BLUIElement formDefinition;


        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;

        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;

        private UIBuilder _refBuilder;
        private FinanceRequest request;
        private IList<ActualVsBudgetedIncomeResponse> response_for_actual_vs_budgeted_income_Response;
        private ApexChart<ActualVsBudgetedIncomeResponse> _detailsChart;

        private IList<GPft_NetPft_Margin_Response> gnm;
        private ApexChart<GPft_NetPft_Margin_Response> _gnmChart;

        private IList<Debtors_Creditors_Age_Analysis> debtors_ages;
        private ApexChart<Debtors_Creditors_Age_Analysis> _ageChart;

        private IList<Debtors_Creditors_Age_Analysis> creditors_ages;
        private IList<Debtors_Creditors_Age_Analysis> debtors_ages_overdue;
        private IList<Debtors_Creditors_Age_Analysis> creditors_ages_overdue;
        private IList<GPft_NetPft_DT> gpft_npft_dt;
        private FinanceRequestDT requestDT;
        private IList<Debtors_Creditors_Age_Analysis_DT> responseDT;

        private SelectedData<Debtors_Creditors_Age_Analysis> SelectedData;

        private FinanceRequestDTDetails requestDTDetails;


        EmptyChart empty_chart;
        private ApexChartOptions<GPft_NetPft_Margin_Response> options;
        ApexChartOptions<ActualVsBudgetedIncomeResponse> opt1;
        ApexChartOptions<Debtors_Creditors_Age_Analysis> opt2;
        ApexChartOptions<Debtors_Creditors_Age_Analysis> opt3;
        ApexChartOptions<Debtors_Creditors_Age_Analysis> opt4;
        ApexChartOptions<Debtors_Creditors_Age_Analysis> opt5;

        private Legend legend;

        #endregion

        #region General

        protected override async Task OnParametersSetAsync()
        {

            empty_chart = new();
            await base.OnParametersSetAsync();
        }


        protected override async Task OnInitializedAsync()
        {
            long elementKey = 1;
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

            RefreshChart();
            RefreshGrid();
            HookInteractions();

            legend = new Legend
            {
                Position = LegendPosition.Bottom,

            };
            options = new ApexChartOptions<GPft_NetPft_Margin_Response>
            {
                Colors = new List<string> { "rgb(255, 0, 117)", "rgb(23, 39, 116)", "rgb(0, 122, 255)", "rgb(240, 165, 0)", "rgb(251, 54, 64)", "rgb(40, 167, 69)" },
                Markers = new Markers { Shape = ShapeEnum.Circle, Size = 5 },

            };

            options.Yaxis = new List<YAxis>();
            options.Yaxis.Add(new YAxis
            {
                Labels = new YAxisLabels
                {
                    Formatter = @"function (value) {
                                return Number(value).toLocaleString();}"
                }
            });

            opt1 = new ApexChartOptions<ActualVsBudgetedIncomeResponse>();
            opt1.Yaxis = new List<YAxis>();
            opt1.Yaxis.Add(new YAxis
            {
                Labels = new YAxisLabels
                {
                    Formatter = @"function (value) {
                                return Number(value).toLocaleString();}"
                }
            });

            opt2 = new ApexChartOptions<Debtors_Creditors_Age_Analysis>() { Legend = legend };
            opt2.Yaxis = new List<YAxis>();
            opt2.Yaxis.Add(new YAxis
            {
                Labels = new YAxisLabels
                {
                    Formatter = @"function (value) {
                                return Number(value).toLocaleString();}"
                }
            });

            opt3 = new ApexChartOptions<Debtors_Creditors_Age_Analysis>() { Legend = legend };


            opt3.Yaxis = new List<YAxis>();
            opt3.Yaxis.Add(new YAxis
            {
                Labels = new YAxisLabels
                {
                    Formatter = @"function (value) {
                                return Number(value).toLocaleString();}"
                }
            });

            opt4 = new ApexChartOptions<Debtors_Creditors_Age_Analysis>() { Legend = legend };


            opt4.Yaxis = new List<YAxis>();
            opt4.Yaxis.Add(new YAxis
            {
                Labels = new YAxisLabels
                {
                    Formatter = @"function (value) {
                                return Number(value).toLocaleString();}"
                }
            });

            opt5 = new ApexChartOptions<Debtors_Creditors_Age_Analysis>() { Legend = legend };
            opt5.Yaxis = new List<YAxis>();
            opt5.Yaxis.Add(new YAxis
            {
                Labels = new YAxisLabels
                {
                    Formatter = @"function (value) {
                                return Number(value).toLocaleString();}"
                }
            });

            LoadChart();


        }

        private async void RefreshGrid()
        {
            request = new();
            requestDT = new();
            requestDTDetails = new();

            request = new FinanceRequest
            {
                ElementKey = formDefinition.ElementKey,
                FromDate = new DateTime(2021, 1, 1),
                ToDate = new DateTime(2021, 10, 20),
                BusinessUnit = new CodeBaseResponse() { CodeKey = 400883, CodeName = "Finance BU - Finance BU" },
            };

            response_for_actual_vs_budgeted_income_Response = new List<ActualVsBudgetedIncomeResponse>();
            gnm = new List<GPft_NetPft_Margin_Response>();
            debtors_ages = new List<Debtors_Creditors_Age_Analysis>();
            creditors_ages = new List<Debtors_Creditors_Age_Analysis>();
            debtors_ages_overdue = new List<Debtors_Creditors_Age_Analysis>();
            creditors_ages_overdue = new List<Debtors_Creditors_Age_Analysis>();
            gpft_npft_dt = new List<GPft_NetPft_DT>();
            responseDT = new List<Debtors_Creditors_Age_Analysis_DT>();

            await Task.CompletedTask;
        }

        private async void RefreshChart()
        {
            _detailsChart = new();
            _gnmChart = new();
            _ageChart = new();
            response_for_actual_vs_budgeted_income_Response = new List<ActualVsBudgetedIncomeResponse>();
            response_for_actual_vs_budgeted_income_Response.Clear();
            await Task.CompletedTask;
        }

        private void HookInteractions()
        {

            InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
            _interactionLogic = helper.GenerateEventCallbacks();// generate all event callbacks 
                                                                //AppSettings.RefreshTopBar("Finance");
            appStateService._AppBarName = "Finance";
        }

        private void UIStateChanged()
        {
            this.StateHasChanged();
        }
        #endregion

        #region ui events
        private void OnFromDateClick(UIInterectionArgs<DateTime?> args)
        {
            request.FromDate = args.DataObject;
            UIStateChanged();
        }

        private void OnToDateClick(UIInterectionArgs<DateTime?> args)
        {
            request.ToDate = args.DataObject;
            UIStateChanged();
        }

        private void onBuComboClick(UIInterectionArgs<CodeBaseResponse> args)
        {
            request.BusinessUnit.CodeKey = args.DataObject.CodeKey;

            UIStateChanged();
        }

        private void OnLoadClick(UIInterectionArgs<object> args)
        {
            LoadChart();
            UIStateChanged();
        }

        private async void OnTodayClick(UIInterectionArgs<object> args)
        {
            request.FromDate = DateTime.Now;
            request.ToDate = DateTime.Now;


            await SetValue("FromDate", request.FromDate);
            await SetValue("ToDate", request.ToDate);
            LoadChart();
            UIStateChanged();
        }

        private async void OnMonthClick(UIInterectionArgs<object> args)
        {
            DateTime now = DateTime.Now;
            request.FromDate = new DateTime(now.Year, now.Month, 1);
            request.ToDate = request.FromDate?.AddMonths(1).AddDays(-1);
            await SetValue("FromDate", request.FromDate);
            await SetValue("ToDate", request.ToDate);
            LoadChart();
            UIStateChanged();
        }

        private async void OnYearClick(UIInterectionArgs<object> args)
        {
            int year = DateTime.Now.Year;
            request.FromDate = new DateTime(year, 1, 1);
            request.ToDate = new DateTime(year, 12, 31);
            await SetValue("FromDate", request.FromDate);
            await SetValue("ToDate", request.ToDate);
            LoadChart();
            UIStateChanged();
        }
        #endregion


        #region finance actions

        private void LoadChart()
        {
            LoadDataForActualVsBudgetedIncome();
            LoadGrossPftNetPftMargin();
            LoadDebtorsAgesAnalysis();
            LoadCreditorsAgesAnalysis();
            LoadDebtorsAgesAnalysisOverdue();
            LoadCreditorsAgesAnalysisOverdue();
            LoadGrossProfitNetProfitDT();
        }


        #endregion

        #region Load Data

        private async void LoadDataForActualVsBudgetedIncome()
        {
            response_for_actual_vs_budgeted_income_Response.Clear();

            response_for_actual_vs_budgeted_income_Response = await _dashboardManager.GetActualVsBudgetedIncome(request);

            if (!_dashboardManager.IsExceptionthrown())
            {
                _detailsChart?.SetRerenderChart();

            }
            else
            {
                response_for_actual_vs_budgeted_income_Response.Clear();

            }

            UIStateChanged();
        }
        private async void LoadGrossPftNetPftMargin()
        {
            gnm.Clear();

            gnm = await _dashboardManager.GetGPft_NetPft_Margin(request);

            if (!_dashboardManager.IsExceptionthrown())
            {
                _gnmChart?.SetRerenderChart();
            }
            else
            {
                gnm.Clear();

            }


            UIStateChanged();
        }
        private async void LoadDebtorsAgesAnalysis()
        {
            debtors_ages.Clear();

            debtors_ages = await _dashboardManager.Get_Debtors_Age_Analysis(request);

            if (!_dashboardManager.IsExceptionthrown())
            {
                _ageChart?.SetRerenderChart();
            }
            else
            {
                debtors_ages.Clear();

            }


            UIStateChanged();
        }

        private async void LoadCreditorsAgesAnalysis()
        {
            creditors_ages.Clear();

            creditors_ages = await _dashboardManager.Get_Creditors_Age_Analysis(request);

            if (!_dashboardManager.IsExceptionthrown())
            {
                _ageChart?.SetRerenderChart();
            }
            else
            {
                creditors_ages.Clear();

            }


            UIStateChanged();
        }

        private async void LoadDebtorsAgesAnalysisOverdue()
        {
            debtors_ages_overdue.Clear();

            debtors_ages_overdue = await _dashboardManager.Get_Debtors_Age_Analysis_Overdue(request);

            if (!_dashboardManager.IsExceptionthrown())
            {
                _ageChart?.SetRerenderChart();
            }
            else
            {
                debtors_ages_overdue.Clear();

            }


            UIStateChanged();
        }

        private async void LoadCreditorsAgesAnalysisOverdue()
        {
            creditors_ages_overdue.Clear();

            creditors_ages_overdue = await _dashboardManager.Get_Creditors_Age_Analysis_Overdue(request);

            if (!_dashboardManager.IsExceptionthrown())
            {
                _ageChart?.SetRerenderChart();
            }
            else
            {
                creditors_ages_overdue.Clear();

            }


            UIStateChanged();
        }

        private async void LoadGrossProfitNetProfitDT()
        {
            gpft_npft_dt.Clear();

            gpft_npft_dt = await _dashboardManager.Get_Monthly_GPft_NetPft_DT(request);


            UIStateChanged();
        }

        private async Task OnClickChartAsync()
        {
            if (!_dashboardManager.IsExceptionthrown())
            {
                var parameters = new DialogParameters
                {
                    ["Monthly_GPft_NETPft"] = gpft_npft_dt,

                };

                DialogOptions options = new DialogOptions() { FullScreen = true, CloseButton = true };
                var dialog = _dialogService.Show<Monthly_GPft_NETPft_DT>("Monthly Gross Profit & Net Profit", parameters, options);
                var result = await dialog.Result;
            }



        }

        private async void DataPointsSelectedForDebtorAgeAnalysis(SelectedData<Debtors_Creditors_Age_Analysis> selectedData)
        {
            string header = "";
            responseDT.Clear();

            requestDT.ToDate = request.ToDate;
            requestDT.ElementKey = request.ElementKey;
            requestDT.BusinessUnit = request.BusinessUnit;

            foreach (var itm in selectedData.DataPoint.Items)
            {
                requestDT.DayS = itm.DayS;
                requestDT.DayE = itm.DayE;
                header = itm.Hdr;
            }

            responseDT = await _dashboardManager.Get_Debtors_Age_Analysis_DT(requestDT);

            GetTransactionRequest(requestDT);

            if (responseDT != null)
            {
                responseDT.ToList().ForEach(x => x.AccountType = "Debtor");
                var parameters = new DialogParameters
                {
                    ["SelectedData"] = responseDT,
                    ["TransactionRequest"] = requestDTDetails,
                    ["Header"] = header,
                    ["Date"] = request.ToDate,
                    ["ObjKy"] = requestDT.ElementKey,
                };

                DialogOptions options = new DialogOptions() { FullScreen = true };
                var dialog = _dialogService.Show<DebtorsCreditorsAgeAnalysis>("", parameters, options);
                var result = await dialog.Result;
            }
        }

        private async void DataPointsSelectedForCreditorAgeAnalysis(SelectedData<Debtors_Creditors_Age_Analysis> selectedData)
        {
            string header = "";
            responseDT.Clear();

            requestDT.ToDate = request.ToDate;
            requestDT.ElementKey = request.ElementKey;
            requestDT.BusinessUnit = request.BusinessUnit;

            foreach (var itm in selectedData.DataPoint.Items)
            {
                requestDT.DayS = itm.DayS;
                requestDT.DayE = itm.DayE;
                header = itm.Hdr;
            }

            responseDT = await _dashboardManager.Get_Creditors_Age_Analysis_DT(requestDT);
            
                if (responseDT != null)
                {
                    responseDT.ToList().ForEach(x => x.AccountType = "Creditor");
                    var parameters = new DialogParameters
                    {
                        ["SelectedData"] = responseDT,
                        ["Header"] = header,
                        ["Date"] = request.ToDate,
                        ["ObjKy"]= requestDT.ElementKey,
                    };

                    DialogOptions options = new DialogOptions() { FullScreen = true };
                    var dialog = _dialogService.Show<DebtorsCreditorsAgeAnalysis>("", parameters, options);
                    var result = await dialog.Result;
                }
            }

        private async void DataPointsSelectedForCreditorAgeOverdueAnalysis(SelectedData<Debtors_Creditors_Age_Analysis> selectedData)
            {
                string header = "";
                responseDT.Clear();

                requestDT.ToDate = request.ToDate;
                requestDT.ElementKey = request.ElementKey;
                requestDT.BusinessUnit = request.BusinessUnit;

                foreach (var itm in selectedData.DataPoint.Items)
                {
                    requestDT.DayS = itm.DayS;
                    requestDT.DayE = itm.DayE;
                    header = itm.Hdr;
                }

                responseDT = await _dashboardManager.Get_Creditors_Age_Analysis_Overdue_DT(requestDT);

                if (responseDT != null)
                {
                responseDT.ToList().ForEach(x => x.AccountType = "Creditor");
                var parameters = new DialogParameters
                    {
                        ["SelectedData"] = responseDT,
                        ["Header"] = header,
                        ["Date"] = request.ToDate,
                        ["ObjKy"] = requestDT.ElementKey,
                    };

                    DialogOptions options = new DialogOptions() { FullScreen = true };
                    var dialog = _dialogService.Show<DebtorsCreditorsAgeAnalysis>("", parameters, options);
                    var result = await dialog.Result;
                }
            }

        private async void DataPointsSelectedForDebtorAgeOverdueAnalysis(SelectedData<Debtors_Creditors_Age_Analysis> selectedData)
            {
                string header = "";
                responseDT.Clear();

                requestDT.ToDate = request.ToDate;
                requestDT.ElementKey = request.ElementKey;
                requestDT.BusinessUnit = request.BusinessUnit;

                foreach (var itm in selectedData.DataPoint.Items)
                {
                    requestDT.DayS = itm.DayS;
                    requestDT.DayE = itm.DayE;
                    header = itm.Hdr;
                }

                responseDT = await _dashboardManager.Get_Debtors_Age_Analysis_Overdue_DT(requestDT);


                if (responseDT != null)
                {
                responseDT.ToList().ForEach(x => x.AccountType = "Debtor");
                var parameters = new DialogParameters
                    {
                        ["SelectedData"] = responseDT,
                        ["Header"] = header,
                        ["Date"] = request.ToDate,
                        ["ObjKy"] = requestDT.ElementKey,
                    };

                    DialogOptions options = new DialogOptions() { FullScreen = true };
                    var dialog = _dialogService.Show<DebtorsCreditorsAgeAnalysis>("", parameters, options);
                    var result = await dialog.Result;
                }
            }

        private void GetTransactionRequest(FinanceRequestDT _requestDT)
            {
                requestDTDetails.ElementKey = _requestDT.ElementKey;
                requestDTDetails.ToDate = _requestDT.ToDate;
                requestDTDetails.BusinessUnit = _requestDT.BusinessUnit;
            }

        private void GetYAxisLabel()
            {


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
