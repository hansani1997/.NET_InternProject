using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.HR;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.com.razorComponents.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Blazor.Components.TabStrip;
using ApexCharts;
using bluelotus360.Com.commonLib.Helpers;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using BlueLotus360.CleanArchitecture.Application.Validators.HR;
using bluelotus360.com.razorComponents.Pages.HumanResource.Attendance;
using bluelotus360.Com.MauiSupports.Services.LocationSuppliers;
using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using System.ComponentModel.DataAnnotations;
using bluelotus360.com.razorComponents.CustomExceptions;
using Microsoft.JSInterop;

namespace bluelotus360.com.razorComponents.Pages.CRM
{
    public partial class DSRGeoAttendence
    {
        private BLUIElement formDefinition;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
        private UIBuilder _refBuilder;

        private UserDetails _user;
        private string headerEmpName;
        private string headerEmpId;
        private string headerDay;
        private int date;
        private string headerMonth;
        private IHRValidator validator;
        private UserMessageDialog _refUserMessage = new UserMessageDialog();

        private ManualAttendence repAttendenceRequest, repAttendenceEditRequest;
        IList<MultiAtnAnlysis_Response> _multiAtnAnlysis_Responses = new List<MultiAtnAnlysis_Response>();
        private MultiAtnAnlysis_Response _User_multiAtnAnlysis_Response=new MultiAtnAnlysis_Response(), _end_record = new MultiAtnAnlysis_Response();
        private InShift _shift;
        private TimeSpan _totalHoursFromAttendence;
        string formattedTotalTime = "00:00";
        bool IsProcessing;
        DateTime? dateValue = DateTime.Now;
        public DateTime? DateValue
        {
            get { return dateValue; }
            set
            {
                dateValue = value;
                OnDateChange(dateValue);
            }
        }

        public BackForwardParams _backForward = new BackForwardParams()
        {
            BackButton = "Customer OutStanding",
            HasBackButton = true,
            BackwardRoute = "/crm/customer_outstanding",
            BackObjectKey = 203611,
        };

        #region general 
        protected override async Task OnInitializedAsync()
        {
            long elementKey = 1;
            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);// get element key from url 

            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();

                formrequest.MenuKey = elementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);//get ui elements
                formDefinition.IsMustElements = formDefinition.Children.Where(x => x.IsMust).Select(i => i._internalElementName).ToList();
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
            await GetEmployee();
            await GetExistingRecord();
        }
        private void InitializeNewAttendence()
        {
            _user = new UserDetails();
            repAttendenceRequest = new ManualAttendence();
            repAttendenceEditRequest= new ManualAttendence();
            validator= new HRValidatorExtended(formDefinition,repAttendenceRequest);
        }

        private void HookInteractions()
        {
            InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
            _interactionLogic = helper.GenerateEventCallbacks();// generate all event callbacks 
            appStateService._AppBarName = "Geo Attendence";
        }

        private void UIStateChanged()
        {
            this.StateHasChanged();
        }
        #endregion

        #region form events

        private async void OnDayStartClick(UIInterectionArgs<object> args)
        {
            if (CanMarkAttendenceByThisEvent(args.InitiatorObject._internalElementName))
            {
                repAttendenceRequest.IsIn = 1;
                repAttendenceRequest.IsOut = 0;
                repAttendenceRequest.AddressCategory3 = new CodeBaseResponse();
                repAttendenceRequest.Address = new AddressResponse() { AddressName = "Day Start", AddressKey = 1 };
                repAttendenceRequest.WorkReason = new CodeBaseResponse();
                repAttendenceRequest.EmpKy = _user.AdrKy;

                PutInOut(repAttendenceRequest);
                repAttendenceRequest = new ManualAttendence();
            }
            else
            {
                _refUserMessage.ShowUserMessageWindow();
            }

            
            StateHasChanged();
        }
        private async void OnDayEndClick(UIInterectionArgs<object> args)
        {
            bool? result = await _dialogService.ShowMessageBox(
                   "Warning",
                   $"Are sure to end the day,you won't be able to raise orders after this! ",
                   yesText: "Confirm", cancelText: "Cancel");
            if (result.HasValue && result.Value)
            {
                if (CanMarkAttendenceByThisEvent(args.InitiatorObject._internalElementName))
                {
                    repAttendenceRequest.IsIn = 0;
                    repAttendenceRequest.IsOut = 0;
                    repAttendenceRequest.IsoutWithoutIn = 1;
                    repAttendenceRequest.AddressCategory3 = new CodeBaseResponse();
                    repAttendenceRequest.Address = new AddressResponse() { AddressName = "Day End", AddressKey = 1 };
                    repAttendenceRequest.WorkReason = new CodeBaseResponse();
                    repAttendenceRequest.EmpKy = _user.AdrKy;

                    PutInOut(repAttendenceRequest);
                    repAttendenceRequest = new ManualAttendence();
                }
                else
                {
                    _refUserMessage.ShowUserMessageWindow();
                }
                
            }

            StateHasChanged();
        }
        private async void OnRoutesChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            repAttendenceRequest.AddressCategory3 = args.DataObject;
            await ReadData("HeaderSection_Location");
            StateHasChanged();
        }
        private void OnLocationChange(UIInterectionArgs<AddressResponse> args)
        {
            repAttendenceRequest.Address = args.DataObject;
            StateHasChanged();
        }
        private void OnShopStatusChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            repAttendenceRequest.WorkReason = args.DataObject;
            StateHasChanged();
        }
        private async void InClick(UIInterectionArgs<object> args)
        {
            //validator = new HRValidatorExtended(formDefinition, repAttendenceRequest);
            //bool IsCheckNoneOutRecordAvailable = _multiAtnAnlysis_Responses.Any(x => !x.HasPutOut());
            //bool IsCheckDayStarted = _multiAtnAnlysis_Responses.Any(x => x.IsDayStartRecord);

            //if (validator != null && validator.CanAddTimeToGrid() && !IsCheckNoneOutRecordAvailable && IsCheckDayStarted)
            if (CanMarkAttendenceByThisEvent(args.InitiatorObject._internalElementName))
            {
                repAttendenceRequest.IsIn = 1;
                repAttendenceRequest.IsOut = 0;
                repAttendenceRequest.EmpKy = _user.AdrKy;

                //_shift = await _hrManager.GetShift(repAttendenceRequest) ?? new InShift();
                //repAttendenceRequest.ShiftKy = _shift.ShiftKy;

                PutInOut(repAttendenceRequest);
            }
            else
            {
                //if (validator != null)
                //{
                    //if (IsCheckNoneOutRecordAvailable)
                    //    validator.UserMessages.AddErrorMessage("You can't put in ,because you have incompleted attendence");
                    //if (!IsCheckDayStarted)
                    //    validator.UserMessages.AddErrorMessage("the day is not started yet!");

                    _refUserMessage.ShowUserMessageWindow();
                //}
            }
            repAttendenceRequest = new ManualAttendence();
            StateHasChanged();
        }
        private async void OutClick(UIInterectionArgs<object> args)
        {
            bool? result = await _dialogService.ShowMessageBox(
                   "Warning",
                   $"Are sure to put out ! ",
                   yesText: "Confirm", cancelText: "Cancel");
            if (result.HasValue && result.Value)
            {
               _User_multiAtnAnlysis_Response = _multiAtnAnlysis_Responses.FirstOrDefault(x => !x.HasPutOut());

                if (CanMarkAttendenceByThisEvent(args.InitiatorObject._internalElementName))
                {
                    repAttendenceRequest.IsIn = 0;
                    repAttendenceRequest.IsOut = 1;
                    repAttendenceRequest.AddressCategory3 = _User_multiAtnAnlysis_Response.AddressCategory3;
                    repAttendenceRequest.Address = _User_multiAtnAnlysis_Response.Address;
                    repAttendenceRequest.WorkReason = _User_multiAtnAnlysis_Response.WorkReason;
                    repAttendenceRequest.EmpKy = _user.AdrKy;
                    repAttendenceRequest.MultiAtnDetKy = _User_multiAtnAnlysis_Response.MultiAtnDetKy;
                    repAttendenceRequest.AtnDetKy = _User_multiAtnAnlysis_Response.AtnDetKy;

                    PutInOut(repAttendenceRequest);
                }
                else
                {
                    //validator = new HRValidatorExtended();
                    //if (validator != null)
                    //{
                        //validator.UserMessages.UserMessages.Clear();
                        //validator.UserMessages.AddErrorMessage("There is no attendence available to be put out!");
                        _refUserMessage.ShowUserMessageWindow();
                    //}
                }
            }

            
            repAttendenceRequest = new ManualAttendence();
            StateHasChanged();
        }
        private void OnCreateSOClick(UIInterectionArgs<object> args)
        {
            string url=args.InitiatorObject.GetPathURL().ToLower() + "?ElementKey=" + args.InitiatorObject.MapKey;
            _navigationManager.NavigateTo(url);
            StateHasChanged();
        }
        private void HeaderSection_Location_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
        {
            args.DataObject.AddtionalData.Add("AdrCat3Ky", repAttendenceRequest.AddressCategory3.CodeKey);
        }
        private async void OnDateChange(DateTime ? dt)
        {
            await GetExistingRecord();
        }
        #endregion

        #region additional events
        private async Task GetEmployee()
        {
            _user = await _hrManager.GetAddressDetailsBylogin();
            if (_user!=null)
            {
                headerEmpName = _user.AdrNm ??"";
                headerEmpId = _user.AdrID ??"";
                headerDay = DateTime.Now.ToString("dddd");
                date = DateTime.Now.Day;
                headerMonth = DateTime.Now.ToString("MMMM");
            }
            
        }

        private async Task<GeoCoordinates> GetGeolocation()
        {
            GeoCoordinates? geoCoordinates = new GeoCoordinates();
            try
            {
                
                LocationModel lm = await _locationService.GetCurrentLocation();

                if (lm != null)
                {
                    geoCoordinates.Longitude = lm.Longitude;
                    geoCoordinates.Latitude = lm.Latitude;
                    geoCoordinates.Accuracy = lm.Accuracy;
                }
            }
            catch(GeoLocationException exp) 
            {
                try
                {
                    LocationModel lm = await _jsRuntime.InvokeAsync<LocationModel>("getCurrentPosition");
                    if (lm != null)
                    {
                        geoCoordinates.Longitude = lm.Longitude;
                        geoCoordinates.Latitude = lm.Latitude;
                        geoCoordinates.Accuracy = lm.Accuracy;
                    }
                }
                catch (Exception ex)
                {
                    // Handle the error
                    Console.WriteLine($"Geolocation error: {ex.Message}");
                }
            }
            catch(Exception ex)
            {

            }
            return geoCoordinates;
        }

        private async void PutInOut(ManualAttendence attendence)
        {
            IsProcessing= true;
            ManualAttendence atnReq= new ManualAttendence();
            GeoCoordinates? geoCoordinates = await GetGeolocation() ?? new GeoCoordinates();

            atnReq.Address = attendence.Address;
            atnReq.AddressCategory3 = attendence.AddressCategory3;
            atnReq.WorkReason = attendence.WorkReason;
            atnReq.IsIn = attendence.IsIn;
            atnReq.IsOut = attendence.IsOut;
            atnReq.IsoutWithoutIn = attendence.IsoutWithoutIn;
            atnReq.Latitude = geoCoordinates.Latitude;
            atnReq.Longitude = geoCoordinates.Longitude;
            atnReq.EmpKy = attendence.EmpKy;
            atnReq.IsOtherTyp = 1;
            atnReq.MultiAtnDetKy=attendence.MultiAtnDetKy;
            atnReq.AtnDetKy = attendence.AtnDetKy;

            _User_multiAtnAnlysis_Response = await _hrManager.InOut(atnReq);

            atnReq = new ManualAttendence();

            if (_User_multiAtnAnlysis_Response.MultiAtnDetKy > 1)
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.BottomRight;
                _snackBar.Add("Attendence is submited Successfully", Severity.Success);
            }
            else
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.BottomRight;
                _snackBar.Add("Employee not belongs to this company!", Severity.Warning);
            }

            await GetExistingRecord();
            IsProcessing = false;
            StateHasChanged();
        }

        private async Task GetExistingRecord()
        {
            MultiAtnAnlysis _multiAtnAnlysis=new MultiAtnAnlysis();
            _end_record = new MultiAtnAnlysis_Response();

            _multiAtnAnlysis.EmpKy = _user.AdrKy;
            _multiAtnAnlysis.FDt = DateValue!=null ? DateValue.Value.ToString("MM/dd/yyyy"): DateTime.Now.ToString("MM/dd/yyyy");
            _multiAtnAnlysis.Objky = formDefinition.ElementKey;
            _multiAtnAnlysis_Responses = await _hrManager.GetExistingRecordForDayV2(_multiAtnAnlysis) ??new List<MultiAtnAnlysis_Response>();

            var dayStartAddress = new AddressResponse() { AddressName = "Day start", AddressKey = 1 };
            var dayEndAddress = new AddressResponse() { AddressName = "Day End", AddressKey = 1 };

            foreach (var itm in _multiAtnAnlysis_Responses)
            {
                itm.Address = itm.IsDayStartRecord ? dayStartAddress : (itm.IsDayEndRecord ? dayEndAddress : itm.Address);
                _end_record= itm.IsDayEndRecord ? itm : _end_record;
            }
            await CalculateTotalWorkedHours();

            UIStateChanged();
        }

        private async Task CalculateTotalWorkedHours()
        {
            _totalHoursFromAttendence = TimeSpan.Zero;

            var startRecord = _multiAtnAnlysis_Responses.FirstOrDefault(x => x.IsDayStartRecord);
            var endRecord = _multiAtnAnlysis_Responses.FirstOrDefault(x => x.IsDayEndRecord);
            var lastStartRecord = _multiAtnAnlysis_Responses.FirstOrDefault(x => !x.HasPutOut());
            var lastEndRecord = _multiAtnAnlysis_Responses.Where(x=>!x.IsDayEndRecord).OrderBy(p=>p.InDtm).LastOrDefault(x => x.HasPutOut());

            if (endRecord != null)
            {
                _totalHoursFromAttendence = endRecord.OutDtm.Value.Subtract(startRecord?.InDtm ?? DateTime.MinValue);
            }
            else if (lastStartRecord != null)
            {
                _totalHoursFromAttendence = (lastStartRecord?.OutDtm ?? lastStartRecord.InDtm).Value.Subtract(startRecord?.InDtm ?? DateTime.MinValue);
            }
            else if (lastEndRecord != null)
            {
                _totalHoursFromAttendence = (lastEndRecord?.OutDtm ?? lastEndRecord.InDtm).Value.Subtract(startRecord?.InDtm ?? DateTime.MinValue);
            }
            TimeSpan roundedTime = RoundTimeSpanToMinutes(_totalHoursFromAttendence);
            formattedTotalTime = FormatTimeSpan(roundedTime);

        }
        public  TimeSpan RoundTimeSpanToMinutes(TimeSpan timeSpan)
        {
            int roundedMinutes = (int)Math.Round(timeSpan.TotalMinutes);
            TimeSpan roundedTime = TimeSpan.FromMinutes(roundedMinutes);
            return roundedTime;
        }

        public  string FormatTimeSpan(TimeSpan timeSpan)
        {
            return $"{(int)timeSpan.TotalHours}:{timeSpan.Minutes:D2}";
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

        private void RefreshComponent(string name)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                helper.Refresh();
                UIStateChanged();
            }
        }

        private void ToggleEditability(string name,bool isEnable)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                helper.ToggleEditable(isEnable);
                UIStateChanged();
            }
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

        #endregion

        #region validations 

        private bool CanMarkAttendenceByThisEvent(string clickedEvent)
        {
            validator = new HRValidatorExtended(formDefinition, repAttendenceRequest);
            validator.UserMessages.UserMessages.Clear();

            if (clickedEvent.Equals("HeaderSection_DayStart"))
            {
                if (_multiAtnAnlysis_Responses.Any(x => x.IsDayStartRecord))
                {
                    validator.UserMessages.AddErrorMessage("you have already started the day!");
                }
            }
            else if (clickedEvent.Equals("HeaderSection_DayEnd"))
            {
                if (_multiAtnAnlysis_Responses.Any(x => x.IsDayEndRecord))
                {
                    validator.UserMessages.AddErrorMessage("you have already ended the day!");
                }
                if (_multiAtnAnlysis_Responses.Any(x => !x.HasPutOut()))
                {
                    validator.UserMessages.AddErrorMessage("you cant end of the day,because there is incompleted attendence");
                }
            }
            else if (clickedEvent.Equals("In"))
            {
                if (validator.CanAddTimeToGrid())
                {
                    //
                }
                if (_multiAtnAnlysis_Responses.Any(x => !x.HasPutOut()))
                {
                    validator.UserMessages.AddErrorMessage("You can't put in ,because you have incompleted attendence");
                }
                if (!_multiAtnAnlysis_Responses.Any(x => x.IsDayStartRecord))
                {
                    validator.UserMessages.AddErrorMessage("the day is not started yet!");
                }
                if (_multiAtnAnlysis_Responses.Any(x => x.IsDayEndRecord))
                {
                    validator.UserMessages.AddErrorMessage("the day is ended, you can't proceed anymore within today!");
                }
            }
            else if (clickedEvent.Equals("Out"))
            {
                if (_multiAtnAnlysis_Responses.FirstOrDefault(x => !x.HasPutOut()) == null)
                {
                    validator.UserMessages.AddErrorMessage("There is no attendence available to be put out!");
                }
            }
            else
            {

            }
            
                
            return validator.UserMessages.UserMessages.Count == 0;
        }
        #endregion
    }


}
