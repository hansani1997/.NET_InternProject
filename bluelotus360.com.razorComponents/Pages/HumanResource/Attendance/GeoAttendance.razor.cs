using BlueLotus360.CleanArchitecture.Application.Validators.HR;
using BlueLotus360.CleanArchitecture.Domain.Entities.HR;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.com.razorComponents.Extensions;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using bluelotus360.Com.commonLib.Helpers;
using MudBlazor;
using bluelotus360.com.razorComponents.MB.Shared.Components.Popups.HR;
using bluelotus360.Com.MauiSupports.Services.LocationSuppliers;
using BL10.CleanArchitecture.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace bluelotus360.com.razorComponents.Pages.HumanResource.Attendance
{
    public partial class GeoAttendance
    {
        private BLUIElement formDefinition;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;

        private UIBuilder _refBuilder;

        private UserDetails _user;
        private MultiAtnAnlysis _multiAtnAnlysis;

        private MultiAtnAnlysis_Response _User_multiAtnAnlysis_Response;
        private IList<MultiAtnAnlysis_Response> _multiAtnAnlysis_Responses;
        private InShift _shift;
        private ManualAttendence _manualAttendance;
        private AddManualAdt _addManualAdt;
        private UserPermission _userPermission;

        private bool fixedheader = true;
        private string headerName;
        private string headerEmpName;
        private string headerEmpId;
        private string headerDay;
        private int date;
        private string headerMonth;

        private IHRValidator validator;
        private IHREditFormValidator validatorEdit;
        string alertMessage;
        private TimeSpan _totalHoursFromAttendence;
        private bool IsProcessing;
        private bool IsDataLoading;
        private bool showAlert;
        private bool hasUserPermissionForAdd;
        private bool hasUserPermissionForEdit;

        private readonly Lazy<Task<IJSObjectReference>> moduleTask = default!;
        private readonly DotNetObjectReference<GeoAttendance> dotNetObjectReference;
        private GeoCoordinates? geoCoordinates = new GeoCoordinates();
        private BLUIElement Grid;
        private BLTable<TaskwiseAttendance> _blTb;
        private IList<TaskwiseAttendance> ListOfTasks;
        private BLUIElement TaskContent; 
        private BLUIElement AttendanceContent;
        private TaskwiseAttendance _SingleTaskAddition;
        private string SelectedDate;
        private int CurrentAdrKy = 1;
        private IList<TaskwiseAttendance> ListOfTasksForSave;
        private string TaskHour = "";
        private string AtnHour = "";
        private string Difference = "";
        public GeoAttendance()
        {
            moduleTask = new(() => _jsRuntime!.InvokeAsync<IJSObjectReference>(
                identifier: "import",
                args: "./js/geoLocation.js")
            .AsTask());

            dotNetObjectReference = DotNetObjectReference.Create(this);
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

            if (formDefinition != null)
            {
                formDefinition.IsDebugMode = true;
                
            }
           

            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();


            HookInteractions();
            InitializeNewAttendence();
            GetEmployee();
            var childsHash = formDefinition.Children.ToLookup(elem => elem.ParentKey);
            foreach (var child in formDefinition.Children)
            {
                child.Children = childsHash[child.ElementKey].ToList();
            }
            Grid = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("TaskGrid")).FirstOrDefault();
            TaskContent = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("TaskContent")).FirstOrDefault();
            AttendanceContent = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("Content")).FirstOrDefault();
            
            if (_userPermission != null)
            {
                _userPermission.ObjKy = Convert.ToInt32(elementKey);
                CheckUserPermissionForUsers(_userPermission);
            }
            await GetGeolocation();
            await GetExistingTaskAttendance();
        }

        private void InitializeNewAttendence()
        {
            _user = new UserDetails();
            _userPermission = new UserPermission();
            _multiAtnAnlysis = new MultiAtnAnlysis();
            _shift = new InShift();
            _manualAttendance = new ManualAttendence();
            _User_multiAtnAnlysis_Response = new MultiAtnAnlysis_Response();
            _manualAttendance.selectedAttendence = new UpdateAttendence();
            _addManualAdt = new AddManualAdt();
            _multiAtnAnlysis_Responses = new List<MultiAtnAnlysis_Response>();
            ListOfTasks = new List<TaskwiseAttendance>();
            Grid = new BLUIElement();
            TaskContent = new BLUIElement();
            AttendanceContent = new BLUIElement();
            _SingleTaskAddition = new TaskwiseAttendance();
            SelectedDate = DateTime.Now.ToString("yyyy/MM/dd");
        }

        private void HookInteractions()
        {
            InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
            _interactionLogic = helper.GenerateEventCallbacks();// generate all event callbacks 
            //AppSettings.RefreshTopBar("Geo Attendence");
            appStateService._AppBarName = "Geo Attendence";
        }

        private void UIStateChanged()
        {
            this.StateHasChanged();
        }

        private async void DateClick(UIInterectionArgs<DateTime?> args)
        {
            _manualAttendance.Date = ((DateTime)args.DataObject).ToString("yyyy/MM/dd");
            _multiAtnAnlysis.FDt = _manualAttendance.Date;
            _multiAtnAnlysis.TDt = _manualAttendance.Date;
            _addManualAdt.AtnDt = (DateTime)args.DataObject;

            if (_multiAtnAnlysis.FDt != null && DateTime.Compare(DateTime.Parse(_multiAtnAnlysis.FDt), DateTime.Now.Date) != 0)
            {
                ToggleEditability("InTime", false);
                ToggleEditability("OutTime", false);
            }
            else
            {
                ToggleEditability("InTime", true);
                ToggleEditability("OutTime", true);
            }


            if (_user != null)
            {
                IsDataLoading = true;
                UIStateChanged();

                await GetExistingRecord();

                IsDataLoading = false;
                UIStateChanged();
            }


            UIStateChanged();
        }
        private void OnLocationChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            _manualAttendance.Location = args.DataObject;
            UIStateChanged();
        }

        private void OnInTimeClick(UIInterectionArgs<TimeSpan?> args)
        {
                string time = args.DataObject.Value.ToString(@"hh\:mm\:ss");
                if (!string.IsNullOrEmpty(time))
                {
                    _manualAttendance.selectedAttendence.InDtm = DateTime.Parse(DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + " " + time);
                    UIStateChanged();
                }
            

        }
        private void OnOutTimeClick(UIInterectionArgs<TimeSpan?> args)
        {
                string time = args.DataObject.Value.ToString(@"hh\:mm\:ss");
                if (!string.IsNullOrEmpty(time))
                {
                    _manualAttendance.selectedAttendence.OutDtm = DateTime.Parse(DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + " " + time);
                }
                UIStateChanged();
        }

        private void OnClickPutIn(UIInterectionArgs<TimeSpan?> args)
        {
            _addManualAdt.InDtm = DateTime.Parse(_manualAttendance.Date + " " + args.DataObject.Value.Hours + ":" + args.DataObject.Value.Minutes + ":" + args.DataObject.Value.Seconds);

            UIStateChanged();
        }
        private void OnClickPutOut(UIInterectionArgs<TimeSpan?> args)
        {
            _addManualAdt.OutDtm = DateTime.Parse(_manualAttendance.Date + " " + args.DataObject.Value.Hours + ":" + args.DataObject.Value.Minutes + ":" + args.DataObject.Value.Seconds);

            UIStateChanged();
        }
        private async void InClick(UIInterectionArgs<object> args)
        {
            string timedifference = DateTime.Now.ToString("yyyy/MM/dd");
            if (timedifference == _manualAttendance.Date)
            {
                IsProcessing = true;
                UIStateChanged();

                await PutIn();

                IsProcessing = false;
                UIStateChanged();

                if (_user != null)
                {
                    IsDataLoading = true;
                    UIStateChanged();

                    await GetExistingRecord();

                    IsDataLoading = false;
                    UIStateChanged();
                }
                else
                {
                    _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackBar.Add("Cant directly add in in different date",Severity.Error);
                }
            }
        }

        private async void OutClick(UIInterectionArgs<object> args)
        {
            string timedifference = DateTime.Now.ToString("yyyy/MM/dd");
            if (timedifference == _manualAttendance.Date)
            {
                IsProcessing = true;
                await PutOut();

                if (_user != null)
                {
                    IsDataLoading = true;
                    UIStateChanged();

                    await GetExistingRecord();

                    IsDataLoading = false;
                    UIStateChanged();
                }


                IsProcessing = false;
                UIStateChanged();
            }
            else
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Cant directly add out in different date",Severity.Error);
            }
        }

        private async void AddClick(UIInterectionArgs<object> args)
        {
            await ShowCreateAttendence();
            UIStateChanged();

        }

        private async void GetEmployee()
        {

            _user = await _hrManager.GetUserAsync();
            headerName = DateTime.Now.ToString("dd/MM/yyyy") + "_" + _user.AdrFullNm;
            headerEmpName = _user.AdrNm;
            headerEmpId = _user.AdrFullNm.Split("-")[0];
            headerDay = DateTime.Now.ToString("dddd");
            date = DateTime.Now.Day;
            headerMonth = DateTime.Now.ToString("MMMM");

            if (_user != null)
            {
                IsDataLoading = true;
                UIStateChanged();

                await GetExistingRecord();

                IsDataLoading = false;
                UIStateChanged();
            }

        }

        private async void CheckUserPermissionForUsers(UserPermission permission)
        {
            _userPermission = await _hrManager.GetUserPermission(permission);

            if (_userPermission.IsAllowUpdate == 1)
            {
                hasUserPermissionForEdit = true;
            }
            if (_userPermission.IsAllowAdd == 1)
            {
                hasUserPermissionForAdd = true;
            }
            UIStateChanged();
        }

        private async Task GetExistingRecord()
        {
            _totalHoursFromAttendence = TimeSpan.Zero;
            _multiAtnAnlysis.EmpKy = _user.AdrKy;
            _multiAtnAnlysis_Responses = await _hrManager.GetExistingRecordForDay(_multiAtnAnlysis);

            foreach (var itm in _multiAtnAnlysis_Responses)
            {
                //if (DateTime.Compare(new DateTime(itm.InDtm.Value.Year, itm.InDtm.Value.Month, itm.InDtm.Value.Day),new DateTime(1901,1,1)) ==0 || DateTime.Compare(new DateTime(itm.InDtm.Value.Year, itm.InDtm.Value.Month, itm.InDtm.Value.Day), new DateTime(0001, 1, 1)) == 0)
                //{
                //    itm.InDtm = null;
                //}
                //if (DateTime.Compare(new DateTime(itm.OutDtm.Value.Year, itm.OutDtm.Value.Month, itm.OutDtm.Value.Day), new DateTime(1901, 1, 1)) == 0 || DateTime.Compare(new DateTime(itm.OutDtm.Value.Year, itm.OutDtm.Value.Month, itm.OutDtm.Value.Day), new DateTime(0001, 1, 1)) == 0)
                //{
                //    itm.OutDtm = null;
                //}

                _totalHoursFromAttendence = _totalHoursFromAttendence.Add(itm.GetWorkHours());
            }
            UIStateChanged();
        }

        private async Task PutIn()
        {
            try
            {
                //await GetGeolocation();

                if (_manualAttendance != null)
                {
                    _manualAttendance.EmpKy = _user.AdrKy;

                    //after refresh can't use 
                    _shift = await _hrManager.GetShift(_manualAttendance);

                    if (!_hrManager.IsExceptionthrown())
                    {
                        if (geoCoordinates != null && _shift != null)
                        {
                            _manualAttendance.ShiftKy = _shift.ShiftKy;
                            _manualAttendance.IsHoliday = _shift.isHoliday;
                            _manualAttendance.Longitude = geoCoordinates.Longitude;
                            _manualAttendance.Latitude = geoCoordinates.Latitude;
                        }

                    }

                    if (_manualAttendance.ShiftKy != 0 && _manualAttendance.Latitude != 0 && _manualAttendance.Longitude != 0)
                    {
                        _manualAttendance.IsIn = 1;
                        _manualAttendance.IsOut = 0;
                        _User_multiAtnAnlysis_Response = await _hrManager.InOut(_manualAttendance);   //after refresh can't use

                        if (!_hrManager.IsExceptionthrown() && _User_multiAtnAnlysis_Response.MultiAtnDetKy != 0)
                        {
                            if(_User_multiAtnAnlysis_Response.MultiAtnDetKy > 1)
                            {
                                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.BottomRight;
                                _snackBar.Add("Attendence is submited Successfully", Severity.Success);
                            }
                            else
                            {
                                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.BottomRight;
                                _snackBar.Add("Employee not belongs to this company!", Severity.Warning);
                            }
                        }

                    }
                    else
                    {
                        _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                        _snackBar.Add("An Error Occured", Severity.Error);
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        private async Task PutOut()
        {
            try
            {
                //await GetGeolocation();

                _manualAttendance = new ManualAttendence();
                _shift = new InShift();
                _User_multiAtnAnlysis_Response = new MultiAtnAnlysis_Response();

                if (_manualAttendance != null)
                {
                    _manualAttendance.EmpKy = _user.AdrKy;


                    if (_multiAtnAnlysis_Responses != null && _multiAtnAnlysis_Responses.Count() > 0)
                    {
                        if (_User_multiAtnAnlysis_Response != null)
                        {
                            _User_multiAtnAnlysis_Response = _multiAtnAnlysis_Responses.LastOrDefault();

                            if (geoCoordinates != null && _User_multiAtnAnlysis_Response != null)
                            {
                                _manualAttendance.Location = _User_multiAtnAnlysis_Response.Location;
                                _manualAttendance.Longitude = geoCoordinates.Longitude;
                                _manualAttendance.Latitude = geoCoordinates.Latitude;
                                _manualAttendance.IsIn = 0;
                            }


                            //in time available & out time is null
                            if (_User_multiAtnAnlysis_Response != null && _User_multiAtnAnlysis_Response.InDtm != null && _User_multiAtnAnlysis_Response.OutDtm == null)
                            {
                                _manualAttendance.IsHoliday = 0;
                                _manualAttendance.MultiAtnDetKy = _User_multiAtnAnlysis_Response.MultiAtnDetKy;
                                _manualAttendance.IsOut = 1;
                            }

                            //in time available & out time is available 
                            else if (_User_multiAtnAnlysis_Response != null && _User_multiAtnAnlysis_Response.InDtm != null && _User_multiAtnAnlysis_Response.OutDtm != null)
                            {
                                _shift = await _hrManager.GetShift(_manualAttendance);

                                if (!_hrManager.IsExceptionthrown() && _shift != null)
                                {

                                    _manualAttendance.ShiftKy = _shift.ShiftKy;
                                    _manualAttendance.IsHoliday = _shift.isHoliday;

                                }

                                _manualAttendance.MultiAtnDetKy = 1;
                                _manualAttendance.IsOut = 0;
                                _manualAttendance.IsoutWithoutIn = 1;


                            }
                            else
                            {

                            }
                        }


                    }

                    await _hrManager.InOut(_manualAttendance);

                }
                else
                {

                }

                UIStateChanged();

            }
            catch (Exception ex)
            {

            }



        }

        private async Task ShowCreateAttendence()
        {
            try
            {
                BLUIElement modalUIElement;

                //await GetGeolocation();

                if (_manualAttendance != null && geoCoordinates != null && _user != null)
                {
                    _manualAttendance.EmpKy = _user.AdrKy;
                    _addManualAdt.EmpKy = _user.AdrKy;
                    _addManualAdt.Location = _manualAttendance.Location;
                    _addManualAdt.Latitude = geoCoordinates.Latitude;
                    _addManualAdt.Longitude = geoCoordinates.Longitude;
                    //_addManualAdt.InDtm = DateTime.Now;
                    //_addManualAdt.OutDtm = DateTime.Now;

                    _shift = await _hrManager.GetShift(_manualAttendance);

                    if (!_hrManager.IsExceptionthrown() && _addManualAdt != null && _shift != null)
                    {

                        _addManualAdt.ShiftKy = _shift.ShiftKy;
                        _addManualAdt.IsHoliday = _shift.isHoliday;

                    }


                    if (!_modalDefinitions.TryGetValue("CreateAttendencePopUp", out modalUIElement))
                    {
                        if (modalUIElement == null && formDefinition != null)
                        {
                            modalUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("CreateAttendencePopUp")).FirstOrDefault();
                            if (modalUIElement != null && modalUIElement.Children.Count > 0)
                            {
                                _modalDefinitions.Add("CreateAttendencePopUp", modalUIElement);

                            }
                        }

                    }

                    if (modalUIElement != null && _addManualAdt != null)
                    {

                        validator = new HRValidator(_addManualAdt);
                        var parameters = new DialogParameters
                        {
                            ["AttendenceRequest"] = _addManualAdt,
                            ["ModalUIElement"] = modalUIElement,
                            ["InteractionLogics"] = _interactionLogic,
                            ["ObjectHelpers"] = _objectHelpers,
                            ["ButtonName"] = "Save",
                            ["HeadingPopUp"] = "Add",
                            ["Validaor"] = validator,
                        };
                        DialogOptions options = new DialogOptions();
                        var dialog = _dialogService.Show<CreateAttendence>("Create", parameters, options);

                        var result = await dialog.Result;


                        if (!result.Cancelled)
                        {

                            if (_addManualAdt != null)
                            {
                                await _hrManager.CreateManualAttendence(_addManualAdt);
                            }


                            if (!_hrManager.IsExceptionthrown())
                            {
                                IsDataLoading = true;
                                UIStateChanged();

                                await GetExistingRecord();

                                IsDataLoading = false;
                                UIStateChanged();

                                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                                _snackBar.Add("Added Successfully", Severity.Success);
                            }
                            else
                            {
                                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                                _snackBar.Add("Can't add, Please retry", Severity.Error);
                            }


                        }

                    }


                    UIStateChanged();
                }

            }
            catch (Exception ex)
            {

            }




        }

        private async Task ShowEditAttendence(MultiAtnAnlysis_Response att)
        {
            try
            {
                BLUIElement modalUIElement;

                if (_manualAttendance != null && att != null && formDefinition != null)
                {
                    _manualAttendance.selectedAttendence.MultiAtnDetKy = att.MultiAtnDetKy;
                    _manualAttendance.selectedAttendence.AtnDetKy = att.AtnDetKy;




                    if (!_modalDefinitions.TryGetValue("AddEditAttendencePopUp", out modalUIElement))
                    {
                        if (modalUIElement == null)
                        {
                            modalUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("AddEditAttendencePopUp")).FirstOrDefault();
                            if (modalUIElement != null && modalUIElement.Children.Count > 0)
                            {
                                _modalDefinitions.Add("AddEditAttendencePopUp", modalUIElement);

                            }
                        }

                    }



                    if (modalUIElement != null)
                    {
                        validatorEdit = new EditAttendenceValidator(_manualAttendance.selectedAttendence);
                        var parameters = new DialogParameters
                        {
                            ["AttendenceDetails"] = att,
                            ["ModalUIElement"] = modalUIElement,
                            ["InteractionLogics"] = _interactionLogic,
                            ["ObjectHelpers"] = _objectHelpers,
                            ["ButtonName"] = "Save",
                            ["HeadingPopUp"] = "Edit",
                            ["Validaor"] = validatorEdit,
                        };
                        DialogOptions options = new DialogOptions();
                        var dialog = _dialogService.Show<AddEditAttendence>("Edit", parameters, options);

                        var result = await dialog.Result;
                        if (!result.Cancelled)
                        {


                            await _hrManager.UpdateRecord(_manualAttendance.selectedAttendence);

                            if (!_hrManager.IsExceptionthrown())
                            {
                                IsDataLoading = true;
                                UIStateChanged();

                                await GetExistingRecord();

                                IsDataLoading = false;
                                UIStateChanged();

                                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                                _snackBar.Add("Updated Attendence Successfully", Severity.Success);
                            }
                            else
                            {
                                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                                _snackBar.Add("Can't update, Please retry", Severity.Error);
                            }


                        }

                    }
                }


                UIStateChanged();
            }
            catch (Exception ex)
            {

            }
        }
        private async Task GetGeolocation()
        {
            LocationModel lm = await _locationService.GetCurrentLocation();
            geoCoordinates.Longitude = lm.Longitude;
            geoCoordinates.Latitude = lm.Latitude;
            geoCoordinates.Accuracy = lm.Accuracy;
            //try
            //{
            //    var module = await moduleTask.Value;
            //    await module.InvokeVoidAsync(identifier: "getCurrentPosition", dotNetObjectReference);

            //}
            //catch (Exception ex)
            //{
            //    alertMessage = "Can't detect your current location,Please check it and retry";
            //    showAlert = true;

            //    UIStateChanged();
            //}


        }



        //[JSInvokable]
        //public async Task OnSuccessAsync(GeoCoordinates geoCoordinates)
        //{
        //    this.geoCoordinates = geoCoordinates;
        //    await InvokeAsync(StateHasChanged);

        //}

        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }

        private void ToggleViisbility(string name, bool visible)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                helper.UpdateVisibility(visible);
                UIStateChanged();
            }
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

        private async Task<IList<TaskwiseAttendance>> GetExistingTaskAttendance()
        {
            try
            {
                _user = await _hrManager.GetUserAsync();
                CurrentAdrKy = _user.AdrKy;
                if (_user.AdrKy > 11)
                {
                    InRequest request = new InRequest()
                    {
                        EmpKy = _user.AdrKy,
                        Date = SelectedDate
                    };
                    ListOfTasks = await _hrManager.TaskwiseAttendance(request);
                    UpdateTaskFooter();
                }
                

            }
            catch (Exception ex)
            {
                new List<TaskwiseAttendance>();
            }


            return ListOfTasks;

        }

        private async void OnProjectChange(UIInterectionArgs<ProjectResponse> args)
        {
            if (args.DataObject!=null)
            {
                _SingleTaskAddition.Project = args.DataObject;
                await ReadData("TaskCmb");
            }
            
            UIStateChanged();
        }

        private async Task ReadData(string name, bool UseLocalStorage = false)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                await (helper as IBLServerDependentComponent).FetchData(UseLocalStorage);

                StateHasChanged();
            }
        }

        private async void TaskCmb_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
        {
            if(_SingleTaskAddition != null)
            {
                args.DataObject.AddtionalData.Add("ProjectKey", _SingleTaskAddition.Project.ProjectKey);
            }
            
        }
        private async void OnTaskChange(UIInterectionArgs<TaskResponse> args)
       {
            if(args.DataObject !=null)
            {
                _SingleTaskAddition.Task = args.DataObject;
                UIStateChanged();
            }
           
        }

        private async void OnTaskDateChange(UIInterectionArgs<DateTime?> args)
        {
            SelectedDate = args.DataObject.Value.ToString("yyyy/MM/dd");
            await GetExistingTaskAttendance();
            UIStateChanged();
        }

        private async void OnHdrTimeChange(UIInterectionArgs<TimeSpan?> args)
        {
            _SingleTaskAddition.TotalTime = args.DataObject.Value;
            UIStateChanged();
        }

        private async void OnDetailTimeChange(UIInterectionArgs<TimeSpan?> args)
        {
           
            UIStateChanged();
        }

       

        private async void AddToGridData(UIInterectionArgs<object> args)
        {
            if (_SingleTaskAddition != null && _SingleTaskAddition.Project.ProjectKey > 11 && _SingleTaskAddition.TotalTime.HasValue && CurrentAdrKy> 11)
            {
                _SingleTaskAddition.ItmTrnKy = 1;
                ListOfTasks.Add(_SingleTaskAddition);
               
            }

            _SingleTaskAddition = new TaskwiseAttendance();
            StateHasChanged();
        }

        private async void SaveRecord(UIInterectionArgs<object> args)
        {
            
            foreach (TaskwiseAttendance item in ListOfTasks)
            {
                item.EmpKy = CurrentAdrKy;
                item.Date = SelectedDate;
            }

            bool success=await _hrManager.SaveTaskwiseAttendance(ListOfTasks);
            
            if (success)
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Saved Successfully", Severity.Success);
                await GetExistingTaskAttendance();
                UIStateChanged();
            }
            else
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Something went wrong", Severity.Error);
            }


        }

        private async Task UpdateTaskFooter()
        {
            TaskHour = TimeSpan.FromMinutes(ListOfTasks.Sum(x => x.TotalMinute)).ToString(@"hh\:mm");
            AtnHour =  TimeSpan.FromMinutes(_totalHoursFromAttendence.TotalMinutes).ToString(@"hh\:mm");
            Difference= TimeSpan.FromMinutes((TimeSpan.FromMinutes(_totalHoursFromAttendence.TotalMinutes)- TimeSpan.FromMinutes(ListOfTasks.Sum(x => x.TotalMinute))).TotalMinutes).ToString(@"hh\:mm");
        }
    }



   
    public class GeoCoordinates
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Accuracy { get; set; }
    }
}
