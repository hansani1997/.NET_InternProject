using ApexCharts;
using bluelotus360.com.razorComponents.Extensions;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using bluelotus360.com.razorComponents.MB.Shared.Components.Chart;
using bluelotus360.com.razorComponents.Pages.Dashboard.Finance;
using bluelotus360.com.razorComponents.StateManagement;
using bluelotus360.Com.commonLib.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Dashboard;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Pages.Dashboard
{
    public partial class FinanceDashBoard
    {
        #region parameters

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

        private ApexCharts.Legend legend;
        private ApexCharts.Zoom zoom;
        WindowSize ws = new WindowSize();
        BLBreakpoint breakpointMargin = new BLBreakpoint();
        double tickamount = 5000;
        private bool isChartLoaded = false;
        DateTime fromDefaultDate = DateTime.Now;
        DateTime toDefaultDate = DateTime.Now;
        CodeBaseResponse defaultBU= new CodeBaseResponse();
        string[] colorPalette = new string[] {"#FF0075", "#172774", "#007AFF", "#F0A500", "#FB3640", "#28a745" };
        #region flags
        //private bool isChartLoading;
        #endregion

        #endregion

        #region General

        //for change Y axis value
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

        protected override async Task OnInitializedAsync()
        {
            long elementKey = 1;
            RefreshChart();
            RefreshGrid();

            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey); // get element key from url

            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();

                formrequest.MenuKey = elementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest); //get ui element
                formDefinition.IsDebugMode = true;

                fromDefaultDate = DateTime.Parse(formDefinition.Children.Where(x => x._internalElementName.Equals("FromDate")).FirstOrDefault().DefaultValue);
                toDefaultDate = DateTime.Parse(formDefinition.Children.Where(x => x._internalElementName.Equals("ToDate")).FirstOrDefault().DefaultValue);
                defaultBU = new CodeBaseResponse() { CodeKey = Convert.ToInt32(formDefinition.Children.Where(x => x._internalElementName.Equals("BUCombo")).FirstOrDefault().DefaultValue) };
            }

           
            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();

            HookInteractions();

            request.ElementKey = formDefinition.ElementKey;
            request.FromDate= fromDefaultDate!=null ? fromDefaultDate:DateTime.Now;
            request.ToDate = toDefaultDate != null ? toDefaultDate : DateTime.Now;
            request.BusinessUnit = defaultBU??new CodeBaseResponse();
            LoadChart();
        }

        
        private async void RefreshGrid()
        {
           request = new();
           requestDT = new();
           requestDTDetails = new();

            

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

        [Obsolete]
        private async void RefreshChart()
        {
            _detailsChart = new();
            _gnmChart = new();
            _ageChart = new();
            response_for_actual_vs_budgeted_income_Response = new List<ActualVsBudgetedIncomeResponse>();
            response_for_actual_vs_budgeted_income_Response.Clear();

            legend = new ApexCharts.Legend
            {
                Position = LegendPosition.Bottom,

            };
            zoom = new ApexCharts.Zoom
            {
                Enabled = true,
                Type = ZoomType.X,
                AutoScaleYaxis = true,
                ZoomedArea = new ZoomedArea
                {
                    Fill = new ZoomedAreaFill
                    {
                        Color = "#90CAF9",
                        Opacity = 0.4

                    },
                    Stroke = new ZoomedAreaStroke
                    {
                        Color = "#0D47A1",
                        Opacity = 0.4,
                        Width = 1
                    }
                }
            };

            options = new ApexChartOptions<GPft_NetPft_Margin_Response>
            {
                Colors = colorPalette.ToList(),
                Markers = new Markers { Shape = ShapeEnum.Circle, Size = 10 },
                PlotOptions = new PlotOptions() { Bar = new PlotOptionsBar() { Horizontal = false, EndingShape = ApexCharts.Shape.Rounded, Distributed = true, ColumnWidth = "50%" } },
                Stroke = new Stroke { Show = true, Width = 2, Colors = new List<string> { "transparent" } },
                Xaxis = new XAxis
                {
                    Categories = (new string[] { "Revenue", "CostOfSale", "GrossProfit", "OtherIncome", "OtherExpense", "NetProfit" }).ToList(),
                    Labels = new XAxisLabels() { Show = true, MaxHeight = 70 }
                },
                Yaxis = new List<YAxis>()
                {
                             new YAxis() 
                             { 
                                 Title=new AxisTitle() { Text= "" },
                                 Labels=new YAxisLabels()
                                {
                                    Style=new AxisLabelStyle(){ Colors= new ApexCharts.Color("#78909c") },
                                    Formatter = @"function (value) {
                                                                  let val= Math.abs(Number(value));

                                                                            if (val >= 10 ** 3 && val < 10 ** 6) {
                                                                                val = (val / 1000).toFixed(0) + ' K'
                                                                            } else if (val >= 10 ** 6) {
                                                                                val = (val / 1000000).toFixed(0) + ' M'
                                                                            } else {
                                                                                val = val;
                                                                            }
                                                                            return val.toLocaleString();}",



                                }

                            }  
                },
                Fill= new Fill() { Opacity= new List<double>() { 1} },
                Title= new Title() { Text= "GROSS PROFIT & NET PROFIT MARGIN",Align=ApexCharts.Align.Left,Style=new TitleStyle() { FontSize="14px",Color= "#575962" } }, 
                Tooltip= new Tooltip() { Theme="dark", Y= new TooltipY() { Formatter= @"function (val) { return Number(val).toLocaleString(); }" } },
            };

            opt1 = new ApexChartOptions<ActualVsBudgetedIncomeResponse>();
            opt1.Yaxis = new List<YAxis>();
            opt1.Yaxis.Add(new YAxis
            {
                TickAmount = 2,
                Labels = new YAxisLabels
                {
                    Formatter = @"function (value) {
                                let val= Math.abs(Number(value));
                                if (val >= 10 ** 3 && val < 10 ** 6) {
                                    val = (val / 1000).toFixed(0) + ' K'
                                } else if (val >= 10 ** 6) {
                                    val = (val / 1000000).toFixed(0) + ' M'
                                } else {
                                    val = val;
                                }
                                return val.toLocaleString();}"
                },
                ForceNiceScale = true,

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

            await Task.CompletedTask;
        }

        private void HookInteractions()
        {

            InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
            _interactionLogic = helper.GenerateEventCallbacks();// generate all event callbacks 

            appStateService._AppBarName = "Finance"; //AppSettings.RefreshTopBar("Finance");
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

        private async void onBuComboClick(UIInterectionArgs<CodeBaseResponse> args)
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
        private async void LoadChart()
        {
            FinanceChartCombinedModel combineChart= await _dashboardManager.GetCombinedFinanceChart(request)?? new FinanceChartCombinedModel();

            if (combineChart!=null)
            {
                response_for_actual_vs_budgeted_income_Response = combineChart.ActualVsBudgetedIncomceResponse;
                gnm = combineChart.GPft_NetPft_Margin_Response;
                debtors_ages = combineChart.Debtors_Age_Analysis_Response;
                creditors_ages = combineChart.Creditors_Age_Analysis_Response;
                debtors_ages_overdue = combineChart.Debtors_Age_Overdue_Response;
                creditors_ages_overdue = combineChart.Creditors_Age_Overdue_Response;
                gpft_npft_dt = combineChart.GPft_NetPft_Margi_DT_Response;

                await LoadDataForActualVsBudgetedIncome();
                await LoadGrossPftNetPftMargin();
                await LoadDebtorsAgesAnalysis();
                await LoadCreditorsAgesAnalysis();
                await LoadDebtorsAgesAnalysisOverdue();
                await LoadCreditorsAgesAnalysisOverdue();
            }
            
            //await LoadGrossProfitNetProfitDT();
            UIStateChanged();
        }
        #endregion

        #region Load Data

        private async Task LoadDataForActualVsBudgetedIncome()
        {
            if (response_for_actual_vs_budgeted_income_Response!=null)
            {
                //response_for_actual_vs_budgeted_income_Response.Clear();

                //response_for_actual_vs_budgeted_income_Response = await _dashboardManager.GetActualVsBudgetedIncome(request);

                //if (!_dashboardManager.IsExceptionthrown())
                //{
                    _detailsChart?.SetRerenderChart();

                //}
                //else
                //{
                //    response_for_actual_vs_budgeted_income_Response.Clear();
                //}
            }
            
        }

        private async Task LoadGrossPftNetPftMargin()
        {
            if (gnm != null)
            {
            //    gnm.Clear();

            //    gnm = await _dashboardManager.GetGPft_NetPft_Margin(request);

            //    if (!_dashboardManager.IsExceptionthrown())
            //    {
                    _gnmChart?.SetRerenderChart();
                //}
                //else
                //{
                //    gnm.Clear();

                //}
            }
               
        }

        private async Task LoadDebtorsAgesAnalysis()
        {
            if (debtors_ages != null)
            {
                //debtors_ages.Clear();

                //debtors_ages = await _dashboardManager.Get_Debtors_Age_Analysis(request);

                //if (!_dashboardManager.IsExceptionthrown())
                //{
                    _ageChart?.SetRerenderChart();
                //}
                //else
                //{
                //    debtors_ages.Clear();

                //}
            }
            
        }

        private async Task LoadCreditorsAgesAnalysis()
        {
            if (creditors_ages != null)
            {
                //creditors_ages.Clear();

                //creditors_ages = await _dashboardManager.Get_Creditors_Age_Analysis(request);

                //if (!_dashboardManager.IsExceptionthrown())
                //{
                    _ageChart?.SetRerenderChart();
                //}
                //else
                //{
                //    creditors_ages.Clear();

                //}
            }
            
        }

        private async Task LoadDebtorsAgesAnalysisOverdue()
        {
            if (debtors_ages_overdue != null)
            {
                //debtors_ages_overdue.Clear();

                //debtors_ages_overdue = await _dashboardManager.Get_Debtors_Age_Analysis_Overdue(request);

                //if (!_dashboardManager.IsExceptionthrown())
                //{
                    _ageChart?.SetRerenderChart();
                //}
                //else
                //{
                //    debtors_ages_overdue.Clear();

                //}
            }
            
        }

        private async Task LoadCreditorsAgesAnalysisOverdue()
        {
            if (creditors_ages_overdue != null) {
                //creditors_ages_overdue.Clear();

                //creditors_ages_overdue = await _dashboardManager.Get_Creditors_Age_Analysis_Overdue(request);

                //if (!_dashboardManager.IsExceptionthrown())
                //{
                    _ageChart?.SetRerenderChart();
                //}
                //else
                //{
                //    creditors_ages_overdue.Clear();

                //}
            }
            
        }

        private async Task LoadGrossProfitNetProfitDT()
        {
            if (gpft_npft_dt!=null)
            {
                //gpft_npft_dt.Clear();

                //gpft_npft_dt = await _dashboardManager.Get_Monthly_GPft_NetPft_DT(request);
            }
            

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
                var dialog = _dialogService.Show<DebitorsCreditorsAgeAnalysis>("", parameters, options);
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
                    ["ObjKy"] = requestDT.ElementKey,
                };

                DialogOptions options = new DialogOptions() { FullScreen = true };
                var dialog = _dialogService.Show<DebitorsCreditorsAgeAnalysis>("", parameters, options);
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
                var dialog = _dialogService.Show<DebitorsCreditorsAgeAnalysis>("", parameters, options);
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
                var dialog = _dialogService.Show<DebitorsCreditorsAgeAnalysis>("", parameters, options);
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
