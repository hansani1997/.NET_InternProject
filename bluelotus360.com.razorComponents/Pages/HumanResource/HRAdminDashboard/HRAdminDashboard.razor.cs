using ApexCharts;
using BL10.CleanArchitecture.Domain.Entities.Dashboard;
using bluelotus360.com.razorComponents.Extensions;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using bluelotus360.Com.commonLib.Helpers;
using bluelotus360.Com.commonLib.Reports.Telerik;
using BlueLotus360.CleanArchitecture.Application.Responses.ServerResponse;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Dashboard;
using BlueLotus360.CleanArchitecture.Domain.Entities.HR;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Charts;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Pages.HumanResource.HRAdminDashboard
{
    public partial class HRAdminDashboard
    {
        #region parameter
        private BLUIElement formDefinition, cardUIElement;
        private HRAdminDashboardRequest hrAdminDashboardRequest;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
        private UIBuilder _refBuilder;
        private long elementKey;
        private IDictionary<string, BLUIElement> _modalDefinitions;

        private ApexChart<HRAdminDashboardHeadCountResponse> _headCountChart;
        private ApexChart<HRAdminAttendanceSummaryResponse> _attendanceSummaryChart;
        private ApexChart<HRAdminTaskwiseA_ActualResponse> _taskWiseAttendanceChart;

        private HrAdminDashboardChartResponse response_for_location_wise_headcount;
        private HRAdminDashboardChart2Response attendance_summary;
        private HRAdminDashboardCardResponse cardDetailsResponse;
        private HRAdminTaskwiseAttendanceActualResponse task_wise_attendance;

        private HRAdminDashboardCardRequest card_request;
        private HRAdminDashboardRequest request;

        private ApexCharts.Legend legend;
        private ApexCharts.Zoom zoom;
        ApexChartOptions<HRAdminDashboardHeadCountResponse> opt1;
        ApexChartOptions<HRAdminAttendanceSummaryResponse> opt2;
        ApexChartOptions<HRAdminTaskwiseA_ActualResponse> opt3;

        //telerik
        private TerlrikReportOptions _reportOption;
        private bool ReportShown;
        private BLTransaction transaction = new();
        CompletedUserAuth auth;
        public HRAdminDashboardCardRequest _birthday;
        public HRAdminDashboardCardRequest _probPerEmp;
        public HRAdminDashboardCardRequest _newRecruitment;
        public HRAdminDashboardCardRequest _leftEmp;
        public HRAdminDashboardRequest _attendanceSummary;
        #endregion


        #region General
        protected override Task OnParametersSetAsync()
        {
            return base.OnParametersSetAsync();
        }

        protected override async Task OnInitializedAsync()
        {
            elementKey = 1;
            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);// get element key from url 
            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();
                formrequest.MenuKey = elementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);//get ui elements
                cardUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("CardSection")).FirstOrDefault();
            }

            formDefinition.IsDebugMode = true;
            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();

            //for telerik
            auth = await _authenticationManager.GetUserInformation();
            _reportOption = new TerlrikReportOptions();
            
            _reportOption.ReportParameters = new Dictionary<string, object>();

            _reportOption.ReportName = "BirthdayReport.trdp";
            _reportOption.ReportName = "ProbationtoPermanentRep.trdp";
            _reportOption.ReportName = "NewHires.trdp";
            _reportOption.ReportName = "LeftEmployees.trdp";
            _reportOption.ReportName = "AtnSummaryLocwise.trdp";
            //end

            HookInteractions();
            FormInitialize();
            LoadChart();
            RefreshChart();
            RefreshGrid();
            GetCardDetails();
            
            legend = new ApexCharts.Legend
            {
                Position = LegendPosition.Right

            };

            opt1 = new ApexChartOptions<HRAdminDashboardHeadCountResponse> {
                PlotOptions = new PlotOptions
                {
                    Bar = new PlotOptionsBar
                    {
                        Horizontal = false,
                        BorderRadius = 5,
                        //DataLabels = new PlotOptionsBarDataLabels
                        //{
                        //    Position = "center",
                        //    HideOverflowingLabels= true,
                            
                        //}
                        
                    }
                },
                Chart = new Chart
                {
                    Toolbar = new Toolbar
                    {
                        Show = false,
                    },
                }
            };
            opt1.Yaxis = new List<YAxis>();
            opt1.Yaxis.Add(new YAxis
            {
                Labels = new YAxisLabels
                {
                    Formatter = @"function (value) {
                                return Number(value).toLocaleString();}"
                },
                ForceNiceScale = true,


            });

            opt2 = new ApexChartOptions<HRAdminAttendanceSummaryResponse> {
                PlotOptions = new PlotOptions
                {
                    Bar = new PlotOptionsBar
                    {
                        BorderRadius = 5
                    }
                },
                Chart = new Chart
                {
                    Toolbar = new Toolbar
                    {
                        Show = false,
                    },
                }
            };
            opt2.Yaxis = new List<YAxis>();
            opt2.Yaxis.Add(new YAxis
            {
                Labels = new YAxisLabels
                {
                    Formatter = @"function (value) {
                                return Number(value).toLocaleString();}"
                },
                ForceNiceScale = true,

            });

            opt3 = new ApexChartOptions<HRAdminTaskwiseA_ActualResponse>
            {
                Chart = new Chart
                {
                    Toolbar = new Toolbar
                    {
                        Show = true,
                    },
                    DropShadow = new DropShadow
                    {
                        Enabled = true,
                        Color = "",
                        Top = 18,
                        Left = 7,
                        Blur = 10,
                        Opacity = 0.2d
                    }

                },

                Grid = new Grid
                {
                    Row = new GridRow
                    {
                        Opacity = 0.5d
                    }
                },
                Colors = new List<string> { "#008FFB", "#00E396", "#FEB019" },
                Markers = new Markers { Shape = ShapeEnum.Circle, Size = 5, FillOpacity = new Opacity(0.8d) },
                Stroke = new Stroke { Curve = Curve.Smooth },

            };
            //opt3 = new ApexChartOptions<HRAdminTaskwiseA_ActualResponse>();
            opt3.Yaxis = new List<YAxis>();
            opt3.Yaxis.Add(new YAxis
            {
                Labels = new YAxisLabels
                {
                    Formatter = @"function (value) {
                                return Number(value).toLocaleString();}"
                },
                ForceNiceScale = true,

            });
        }
        private void FormInitialize()
        {
           
            request = new HRAdminDashboardRequest();
            hrAdminDashboardRequest = new HRAdminDashboardRequest();
            card_request = new HRAdminDashboardCardRequest();
            _birthday = new HRAdminDashboardCardRequest();
            _probPerEmp = new HRAdminDashboardCardRequest();
            _newRecruitment = new HRAdminDashboardCardRequest();
            _leftEmp = new HRAdminDashboardCardRequest();
            _attendanceSummary = new HRAdminDashboardRequest();
        }

        private void HookInteractions()
        {

            InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
            _interactionLogic = helper.GenerateEventCallbacks();// generate all event callbacks 
            appStateService._AppBarName = "HR Admin Dashboard";
        }

        private void UIStateChanged()
        {
            this.StateHasChanged();
        }

        private async void RefreshChart()
        {
            _headCountChart = new();
            _attendanceSummaryChart = new();
            _taskWiseAttendanceChart = new();
            await Task.CompletedTask;
        }

        private async void RefreshGrid()
        {
            request = new();

            request = new HRAdminDashboardRequest
            {
                Date = new DateTime(2021, 1, 1),
                EmployeeType = new CodeBaseResponse() { },
            };

            response_for_location_wise_headcount = new HrAdminDashboardChartResponse();
            attendance_summary = new HRAdminDashboardChart2Response();
            task_wise_attendance = new HRAdminTaskwiseAttendanceActualResponse();

            await Task.CompletedTask;
        }
        #endregion


        #region UI events
        private void OnDateClick(UIInterectionArgs<DateTime?> args)
        {
            hrAdminDashboardRequest.Date = args.DataObject;

            GetCardDetails();
            UIStateChanged();
        }

        private void OnEmpType(UIInterectionArgs<CodeBaseResponse> args)
        {
            hrAdminDashboardRequest.EmployeeType = args.DataObject;
            UIStateChanged();
        }
        private void OnFilterClick(UIInterectionArgs<object> args)
        {
            GetCardDetails();
            LoadChart();
            UIStateChanged();
        }


        private void OnBirthdayReport(UIInterectionArgs<object> args)
        {
            if (true)
            {
                if (_reportOption != null && _reportOption.ReportParameters != null)
                {
                    _reportOption.ReportParameters.Clear();
                    _reportOption.ReportName = "BirthdayReport.trdp";
                    _reportOption.ReportParameters.Add("Cky", auth.AuthenticatedCompany.CompanyKey);
                    _reportOption.ReportParameters.Add("UsrKy", auth.AuthenticatedUser.UserKey);
                    _reportOption.ReportParameters.Add("FrmDt", _birthday.Date);

                    ReportShown = true;
                }
                else
                {
                    _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackBar.Add("Invalid Request. Please select a record.", Severity.Error);
                }
                UIStateChanged();
            }
        }

        private void OnProbationToPermanentEmpReport(UIInterectionArgs<object> args)
        {
            if (true)
            {
                if (_reportOption != null && _reportOption.ReportParameters != null)
                {
                    _reportOption.ReportParameters.Clear();
                    _reportOption.ReportName = "ProbationtoPermanentRep.trdp";
                    _reportOption.ReportParameters.Add("Cky", auth.AuthenticatedCompany.CompanyKey);
                    _reportOption.ReportParameters.Add("UsrKy", auth.AuthenticatedUser.UserKey);
                    _reportOption.ReportParameters.Add("FrmDt", _probPerEmp.Date);

                    ReportShown = true;
                }
                else
                {
                    _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackBar.Add("Invalid Request. Please select a record.", Severity.Error);
                }
                UIStateChanged();
            }
        }

        private void OnNewRecruitmentReport(UIInterectionArgs<object> args)
        {
            if (true)
            {
                if (_reportOption != null && _reportOption.ReportParameters != null)
                {
                    _reportOption.ReportParameters.Clear();
                    _reportOption.ReportName = "NewHires.trdp";
                    _reportOption.ReportParameters.Add("Cky", auth.AuthenticatedCompany.CompanyKey);
                    _reportOption.ReportParameters.Add("UsrKy", auth.AuthenticatedUser.UserKey);
                    _reportOption.ReportParameters.Add("FrmDt", _newRecruitment.Date);

                    ReportShown = true;
                }
                else
                {
                    _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackBar.Add("Invalid Request. Please select a record.", Severity.Error);
                }
                UIStateChanged();
            }
           
        }

        private void OnLeftEmpReport(UIInterectionArgs<object> args)
        {
            if (true)
            {
                if (_reportOption != null && _reportOption.ReportParameters != null)
                {
                    _reportOption.ReportParameters.Clear();
                    _reportOption.ReportName = "LeftEmployees.trdp";
                    _reportOption.ReportParameters.Add("Cky", auth.AuthenticatedCompany.CompanyKey);
                    _reportOption.ReportParameters.Add("UsrKy", auth.AuthenticatedUser.UserKey);
                    _reportOption.ReportParameters.Add("FrmDt", _leftEmp.Date);

                    ReportShown = true;
                }
                else
                {
                    _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackBar.Add("Invalid Request. Please select a record.", Severity.Error);
                }
                UIStateChanged();
            }
            
        }

        private void AttendanceSummaryReport()
        {
            if (true)
            {
                if (_reportOption != null && _reportOption.ReportParameters != null)
                {
                    _reportOption.ReportParameters.Clear();
                    _reportOption.ReportName = "AtnSummaryLocwise.trdp";
                    _reportOption.ReportParameters.Add("Cky", auth.AuthenticatedCompany.CompanyKey);
                    _reportOption.ReportParameters.Add("UsrKy", auth.AuthenticatedUser.UserKey);
                    _reportOption.ReportParameters.Add("FrmDt", _attendanceSummary.Date);
                    _reportOption.ReportParameters.Add("ToDt", _attendanceSummary.Date);
                    _reportOption.ReportParameters.Add("EmpTypKy", _attendanceSummary.EmployeeType.CodeKey);
                }
                else
                {
                    _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackBar.Add("Invalid Request. Please select a record.", Severity.Error);
                }
                UIStateChanged();
            }
        }
        #endregion

        #region chart loading

        private async void LoadChart()
        {
            //isChartLoading= true;

            LoadDataForHeadCount();
            LoadAttendanceSummary();
            LoadTaskWiseAttendance();

            //isChartLoading = false;

        }
        #endregion


        #region Get card details

        private async Task GetCardDetails()
        {
            cardDetailsResponse = await _hradmindashboardManager.GetHRAdminDashboardCardDetails(card_request);
            await SetValue("CardSection_Birthday", cardDetailsResponse.TotalBirthdayCount);
            await SetValue("CardSection_PermanentEmp", cardDetailsResponse.TotalProbationToPermanentEmployeeCount);
            await SetValue("CardSection_Recruitment", cardDetailsResponse.TotalNewRecruitmentCount);
            await SetValue("CardSection_LeftEmp", cardDetailsResponse.TotalLeftEmployeeCount);

            UIStateChanged();
        }
        #endregion
        #region Load Data

        private async void LoadDataForHeadCount()
        {
            response_for_location_wise_headcount = await _hradmindashboardManager.GetLocationWiseHeadCount(hrAdminDashboardRequest);
            _headCountChart?.SetRerenderChart();
            
            UIStateChanged();
        }

        private async void LoadAttendanceSummary()
        {
            attendance_summary = await _hradmindashboardManager.GetAttendanceSummary(hrAdminDashboardRequest);
            _attendanceSummaryChart?.SetRerenderChart();
            
            UIStateChanged();
        }

        private async void LoadTaskWiseAttendance()
        {
            //task_wise_attendance.Clear();

            task_wise_attendance = await _hradmindashboardManager.GetTaskWiseAttendance(request);

              _taskWiseAttendanceChart?.SetRerenderChart();
            

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

        private decimal ConvertTimeToDecimal(string time)
        {
            string[] parts = time.Split(':');
            decimal hours = decimal.Parse(parts[0]);
            decimal minutes = decimal.Parse(parts[1]);
            decimal totalHours = hours + (minutes / 60);
            return totalHours;
        }
    }
}
