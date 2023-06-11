using BlueLotus360.CleanArchitecture.Application.Validators.HR;
using BlueLotus360.CleanArchitecture.Domain.Entities.HR;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.com.razorComponents.Extensions;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudBlazor.Services;
using MudBlazor;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using bluelotus360.Com.commonLib.Helpers;
using bluelotus360.com.razorComponents.MB.Shared.Components.Popups.HR;

namespace bluelotus360.com.razorComponents.Pages.HumanResource.Leave
{
    public partial class SelfLeaveRequest
    {
        private BLUIElement formDefinition;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;

        private UIBuilder _refBuilder;
        private UserDetails _user, _reportingPerson;
        private UserPermission _userPermission;
        private LeaveDetails _leave;
        private Leaverequest _leaveReq;
        private IList<LeaveDetails> _levDetails;
        private LeaveTrnTypeDetails _leaveTrnTypeDetails;
        private MultiApprovalDetails _multiApprovalReq;
        private LeaveCheck _leaveCheck;
        private ILeaveRequestValidator validator;
        private int _leaveTrnTypeKey;
        private int IsMultiAprove;
        private string alertContent;
        private string Servrity;
        private int LevTrnKy;
        private string _headerName;
        private bool fixedheader = true;
        private bool GridShow = true;
        private bool IsLoading;
        private bool hasUserPermissionForEdit;
        private bool showAlert;
        private bool isProcessing;
        private bool isLeaveTypeFound;
        private bool IsDataLoading = false;
        [Inject] IBreakpointService BreakpointListener { get; set; }

        private Breakpoint _breakpoint = new();

        private Guid _subscriptionId;

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

            InitializedLeave();
            await GetEmployee();

            await GetAlreadyAppliedLeaves();
            await LoadLeaveSummary(new DateTime(DateTime.Now.Year, 1, 1));
            _reportingPerson = await RetriveReportingPerson();
            _multiApprovalReq.ObjKy = elementKey;
            _leaveReq.ObjKy = elementKey;

            _userPermission.ObjKy = Convert.ToInt32(elementKey);
            CheckUserPermissionForUsers(_userPermission);

            HookInteractions();

            UIStateChanged();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }
            else
            {
                var subscriptionResult = await BreakpointListener.Subscribe((breakpoint) =>
                {
                    _breakpoint = breakpoint;
                    InvokeAsync(StateHasChanged);
                }, new ResizeOptions
                {
                    ReportRate = 250,
                    NotifyOnBreakpointOnly = true,
                });


                _subscriptionId = subscriptionResult.SubscriptionId;


                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }
        public async ValueTask DisposeAsync() => await BreakpointListener.Unsubscribe(_subscriptionId);


        private async void InitializedLeave()
        {
            _user = new UserDetails();
            _reportingPerson = new UserDetails();
            _userPermission = new UserPermission();
            _leaveReq = new Leaverequest();
            _leave = new LeaveDetails();
            _levDetails = new List<LeaveDetails>();
            _leaveTrnTypeDetails = new LeaveTrnTypeDetails();
            _multiApprovalReq = new MultiApprovalDetails();

            _leaveCheck = new LeaveCheck();
            _leaveReq.LeaveSummary = new List<LeaveSummary>();
            _leaveReq.LevDays = 1;
            _leaveTrnTypeKey = await RetriveLeaveTrnTypeKey("LevTyp", "Short");
            UIStateChanged();
        }

        private void HookInteractions()
        {
            InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
            _interactionLogic = helper.GenerateEventCallbacks();// generate all event callbacks 
            //AppSettings.RefreshTopBar("Self Leave Request");
            appStateService._AppBarName = "Self Leave Request";
        }

        private void UIStateChanged()
        {
            this.StateHasChanged();
        }

        private async void Apply()
        {
            GridShow = !GridShow;
            this.ToggleEditability("ShortLevHour", false);
            showAlert = false;
            _leaveReq.MaxLeaveHour = await LeaveTypeByCompany();
            await SetValue("MaxLeaveHour", _leaveReq.MaxLeaveHour);
            await SetValue("LevDays", 1);
            await LoadLeaveSummary(new DateTime(DateTime.Now.Year, 1, 1));

            UIStateChanged();
        }

        private void GoBack()
        {
            GridShow = !GridShow;
            this.GridClear();

            UIStateChanged();
        }

        private async void Redirect()
        {
            GridShow = !GridShow;
            //showAlert = false;


            isProcessing = true;
            UIStateChanged();
            await GetAlreadyAppliedLeaves();
            isProcessing = false;
            UIStateChanged();
        }
        private async Task GetEmployee()
        {

            _user = await _hrManager.GetUserAsync();

            if (_user != null)
            {
                _headerName = _user.AdrFullNm;
            }
            UIStateChanged();


        }

        private async void CheckUserPermissionForUsers(UserPermission permission)
        {
            _userPermission = await _hrManager.GetUserPermission(permission);

            if (_userPermission.IsAllowUpdate == 1)
            {
                hasUserPermissionForEdit = true;
            }

            UIStateChanged();
        }

        private async Task GetAlreadyAppliedLeaves()
        {
            IsDataLoading = true;
            _levDetails = await _hrManager.GetAlreadyAppliedLeaves();
            IsDataLoading = false;
            UIStateChanged();
        }

        private async Task<int> RetriveLeaveTrnTypeKey(string concd, string ourcd)
        {
            _leaveTrnTypeDetails.ConCd = concd;
            _leaveTrnTypeDetails.OurCd = ourcd;
            return await _hrManager.GetLeaveTrnTypeDetails(_leaveTrnTypeDetails);
        }

        private async Task<decimal> LeaveTypeByCompany()
        {
            if (_user != null)
            {
                return await _hrManager.GetLeaveTypeByCompany(_user);
            }
            return 0;
        }

        private async Task GetNoofDays()
        {
            if (_leaveTrnTypeKey != _leaveReq.LeaveType.CodeKey)
            {
                if (_leaveReq.ToD != null && _leaveReq.EftvDt != null)
                {
                    if (DateTime.Compare(new DateTime(_leaveReq.ToD.Value.Year, _leaveReq.ToD.Value.Month, _leaveReq.ToD.Value.Day), new DateTime(_leaveReq.EftvDt.Value.Year, _leaveReq.EftvDt.Value.Month, _leaveReq.EftvDt.Value.Day)) >= 0)
                    {
                        if (_leaveReq.IsFirstHalf || _leaveReq.IsSecondHalf)
                        {
                            _leaveReq.LevDays = 0.5;
                        }
                        else
                        {
                            _leaveReq.LevDays = _leaveReq.NoOfLeaveDays() + 1;
                        }
                    }
                    else
                    {
                        _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                        _snackBar.Add("From Date should be less than To date", Severity.Error);
                    }

                }
            }
            else
            {
                _leaveReq.LevDays = Convert.ToDouble(_leaveReq.ShortLeaveHours);
            }


            await SetValue("LevDays", (_leaveReq.NoOfLeaveDays() + 1).ToString());

            UIStateChanged();
        }

        private async Task<UserDetails> RetriveReportingPerson()
        {
            if (_user != null)
                return await _hrManager.GetReportingPerson(_user);
            return null;
        }

        private async Task LoadLeaveSummary(DateTime year)
        {
            _leave.EmpKy = _user.AdrKy;
            _leave.Year = year;
            _leaveReq.LeaveSummary = await _hrManager.LoadLeaveSummary(_leave);

        }

        private async Task<int> MultiApproval()
        {
            _multiApprovalReq.LevTrnTypKy = await RetriveLeaveTrnTypeKey("LevTrnTyp", "LevObt");

            return await _hrManager.GetMultiApproval(_multiApprovalReq);
        }

        private async Task InsertLeave(Leaverequest req)
        {
            await GetNoofDays();
            IsMultiAprove = await MultiApproval();
            req.LeaveTrnTypKy = _multiApprovalReq.LevTrnTypKy;

            await _hrManager.ApplyLeave(req);

            showAlert = true;
            if (!_hrManager.IsExceptionthrown())
            {

                if (IsMultiAprove == 1)
                {
                    if (_reportingPerson.AdrKy < 11)
                    {
                        alertContent = "Successfully Saved! \n Reporting person is not available \n Please Contact HR";
                        Servrity = "Info";
                    }
                    else
                    {
                        alertContent = "Successfully Saved! \n Pending For Approval";
                        Servrity = "Info";
                    }

                }

                if (IsMultiAprove == 0)
                {
                    if (_reportingPerson.AdrKy < 11)
                    {
                        alertContent = "Successfully Saved! \n Reporting person is not available \n Please Contact HR";
                        Servrity = "Info";
                    }
                    else
                    {
                        alertContent = "Successfully Saved! ";
                        Servrity = "Success";
                    }
                }

            }
            else
            {
                alertContent = "An Error is occured";
                Servrity = "Error";
            }



            Redirect();
            UIStateChanged();
        }

        private async Task LeaveApply()
        {

            _leaveReq.EmpKy = _user.AdrKy;
            _leaveReq.ReporterKy = _reportingPerson.AdrKy;
            _leaveReq.IsAct = 1;

            validator = new LeaveRequestValidator(_leaveReq);


            if (validator.CanApplyLeave())
            {
                if (validator.HasEntitlement())
                {
                    isProcessing = true;
                    showAlert = false;
                    isLeaveTypeFound = false;
                    UIStateChanged();

                    foreach (var leave in _leaveReq.LeaveSummary)
                    {
                        if (leave.LeaveTypeKy == _leaveReq.LeaveType.CodeKey)
                        {
                            isLeaveTypeFound = true;

                            if (_leaveReq.LeaveType.CodeKey != _leaveTrnTypeKey || (_leaveReq.LeaveType.CodeKey == _leaveTrnTypeKey && _leaveReq.ShortLeaveHours > 0))
                            {
                                _leaveReq.BalLevDay = leave.Bal;
                                if (leave.Bal <= 0)
                                {
                                    var parameters = new DialogParameters
                                    {
                                        ["LeaveRequest"] = _leaveReq,
                                        ["ButtonName"] = "Yes",
                                        ["HeadingPopUp"] = "Balance Zero",
                                    };

                                    DialogOptions options = new DialogOptions();
                                    var dialog = _dialogService.Show<LeaveBalanceZero>("No Entitlement", parameters, options);

                                    var result = await dialog.Result;
                                    UIStateChanged();

                                    if (!result.Cancelled)
                                    {

                                        await InsertLeave(_leaveReq);

                                        UIStateChanged();
                                    }
                                }
                                if (leave.Bal >= _leaveReq.LevDays)
                                {
                                    _leaveCheck.EmpKy = _user.AdrKy;
                                    _leaveCheck.FromDt = _leaveReq.EftvDt;
                                    _leaveCheck.ToDt = _leaveReq.ToD;

                                    LevTrnKy = await _hrManager.SelectLeaveCheck(_leaveCheck);
                                    _leaveReq.LeaveTrnKy = LevTrnKy;
                                    if (LevTrnKy == 0)
                                    {
                                        await InsertLeave(_leaveReq);

                                        UIStateChanged();
                                    }
                                    if (LevTrnKy > 0)
                                    {
                                        showAlert = true;
                                        alertContent = "Already Leave applied for this selected date range";
                                        Servrity = "Info";

                                        UIStateChanged();
                                    }
                                }

                            }
                            else
                            {
                                showAlert = true;
                                alertContent = "You can't apply short leave ,your shor tleave hours are 0.0 ";
                                Servrity = "Error";

                                UIStateChanged();
                            }
                            break;
                        }

                    }

                    if (!isLeaveTypeFound)
                    {
                        showAlert = true;
                        alertContent = "you don not have a permission to apply this leave type!!";
                        Servrity = "Error";
                    }
                    isProcessing = false;
                    UIStateChanged();
                }
                else
                {
                    var parameters2 = new DialogParameters
                    {
                        ["LeaveRequest"] = _leaveReq,
                        ["ButtonName"] = "Yes",
                        ["HeadingPopUp"] = "No Entitlement",
                        ["Validator"] = validator
                    };

                    DialogOptions options2 = new DialogOptions();
                    var dialog2 = _dialogService.Show<CheckEntitlement>("No Entitlement", parameters2, options2);

                    var result = await dialog2.Result;


                    if (!result.Cancelled)
                    {
                        await InsertLeave(_leaveReq);
                        UIStateChanged();
                    }
                }

                this.GridClear();

            }
            else
            {
                var parameters = new DialogParameters
                {
                    ["LeaveRequest"] = _leaveReq,
                    ["ButtonName"] = "Ok",
                    ["HeadingPopUp"] = "Error",
                    ["Validator"] = validator,
                };

                DialogOptions options = new DialogOptions();
                var dialog = _dialogService.Show<LeaveValidation>("Error", parameters, options);


            }


            UIStateChanged();

        }

        private async void OnLeaveTypeClick(UIInterectionArgs<CodeBaseResponse> args)
        {

            _leaveReq.LeaveType = args.DataObject;

            this.ToggleEditability("ShortLevHour", false);

            if (_leaveReq.LeaveType.CodeKey == _leaveTrnTypeKey)
            {
                this.ToggleEditability("ShortLevHour", true);
                UIStateChanged();
                _leaveReq.MaxLeaveHour = await LeaveTypeByCompany();
                _leaveReq.ShortLeaveHours = _leaveReq.MaxLeaveHour;
                await SetValue("ShortLevHour", _leaveReq.MaxLeaveHour);
                UIStateChanged();
            }
            else
            {
                _leaveReq.ShortLeaveHours = 0;
                await SetValue("ShortLevHour", 0);
            }

            UIStateChanged();
        }

        private void OnReasonClick(UIInterectionArgs<CodeBaseResponse> args)
        {
            _leaveReq.LevReason = args.DataObject;
            UIStateChanged();
        }
        private void OnReportingPersonChange(UIInterectionArgs<AddressResponse> args)
        {
            UIStateChanged();
        }
        private void OnOtherReasonClick(UIInterectionArgs<string> args)
        {
            _leaveReq.Rem = args.DataObject;
            UIStateChanged();
        }

        private async void OnFrmDtClick(UIInterectionArgs<DateTime?> args)
        {
            _leaveReq.EftvDt = args.DataObject.Value;
            await GetNoofDays();


            await LoadLeaveSummary(new DateTime(_leaveReq.EftvDt.Value.Year, 1, 1));
            UIStateChanged();

            _leaveReq.IsFirstHalf = false;
            _leaveReq.IsSecondHalf = false;
            await SetValue("IsFirstHalfDay", false);
            await SetValue("IsSecondHalfDay", false);
            UIStateChanged();
        }

        private async void OnToDtClick(UIInterectionArgs<DateTime?> args)
        {
            _leaveReq.ToD = args.DataObject.Value;
            await GetNoofDays();

            _leaveReq.IsFirstHalf = false;
            _leaveReq.IsSecondHalf = false;
            await SetValue("IsFirstHalfDay", false);
            await SetValue("IsSecondHalfDay", false);
            UIStateChanged();
        }

        private void OnNoOfDaysClick(UIInterectionArgs<string> args)
        {
            UIStateChanged();
        }

        private void OnClickShortLevHourClick(UIInterectionArgs<decimal> args)
        {
            _leaveReq.ShortLeaveHours = args.DataObject;
            UIStateChanged();
        }
        private async void OnClickIsSecondHalfDay(UIInterectionArgs<bool> args)
        {
            _leaveReq.IsSecondHalf = args.DataObject;
            _leaveReq.IsFirstHalf = false;
            _leaveReq.LevDays = 0.5;

            await SetValue("IsFirstHalfDay", false);
            await SetValue("LevDays", _leaveReq.LevDays.ToString());

            if (!_leaveReq.IsSecondHalf && !_leaveReq.IsFirstHalf)
            {
                _leaveReq.LevDays = 0;
                await SetValue("LevDays", _leaveReq.LevDays.ToString());
                UIStateChanged();
            }
            UIStateChanged();
        }
        private async void OnClickIsFirstHalfDay(UIInterectionArgs<bool> args)
        {
            _leaveReq.IsFirstHalf = args.DataObject;
            _leaveReq.IsSecondHalf = false;
            _leaveReq.LevDays = 0.5;

            await SetValue("IsSecondHalfDay", false);
            await SetValue("LevDays", _leaveReq.LevDays.ToString());

            if (!_leaveReq.IsSecondHalf && !_leaveReq.IsFirstHalf)
            {
                _leaveReq.LevDays = 0;
                await SetValue("LevDays", _leaveReq.LevDays.ToString());
                UIStateChanged();
            }
            UIStateChanged();
        }
        private void OnShortLeaveStartTimeChange(UIInterectionArgs<DateTime?> args)
        {

        }
        private void OnShortLeaveEndTimeChange(UIInterectionArgs<DateTime?> args)
        {

        }

        private async Task OnDelete(int key)
        {
            isProcessing = true;
            UIStateChanged();

            _leave.LevTrnKy = key;
            await _hrManager.DeleteLeave(_leave);
            await GetAlreadyAppliedLeaves();

            isProcessing = false;
            UIStateChanged();

        }

        private async void GridClear()
        {
            _leaveReq.LeaveType = new CodeBaseResponse();
            _leaveReq.LevReason = new CodeBaseResponse();
            _leaveReq.Rem = string.Empty;
            _leaveReq.EftvDt = DateTime.Now;
            _leaveReq.ToD = DateTime.Now;
            _leaveReq.ShortLeaveHours = 0;
            _leaveReq.IsFirstHalf = false;
            _leaveReq.IsSecondHalf = false;
            _leaveReq.LevDays = 1;
            _leaveReq.MaxLeaveHour = await LeaveTypeByCompany();

            await SetValue("FrmDt", _leaveReq.EftvDt);
            await SetValue("ToDt", _leaveReq.ToD);
            await SetValue("MaxLeaveHour", _leaveReq.MaxLeaveHour);
            this.ToggleEditability("ShortLevHour", false);
            await SetValue("LevDays", "1");
            await SetValue("IsSecondHalfDay", false);
            await SetValue("IsFirstHalfDay", false);
            await LoadLeaveSummary(new DateTime(DateTime.Now.Year, 1, 1));

            UIStateChanged();
        }

        private void ToggleEditability(string name, bool visible)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                helper.ToggleEditable(visible);
                UIStateChanged();
            }
        }
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
    }
}
