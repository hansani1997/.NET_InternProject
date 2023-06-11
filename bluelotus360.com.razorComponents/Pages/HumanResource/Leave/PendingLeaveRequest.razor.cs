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
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using bluelotus360.Com.commonLib.Helpers;
using MudBlazor;
using Microsoft.JSInterop;
using bluelotus360.com.razorComponents.MB.Shared.Components.Popups.HR;

namespace bluelotus360.com.razorComponents.Pages.HumanResource.Leave
{
    public partial class PendingLeaveRequest
    {
        #region parameter
        private BLUIElement formDefinition;
        private BLUIElement buttonDefinition;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, EventCallback> _interactionLogic2;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;

        private UIBuilder _refBuilder;
        //private BLGrid<LeaveDetails> _blTb;
        private Leaverequest _leaveRequest;
        private LeaveDetails _leaveSummaryReq;
        private IList<LeaveDetails> _leaves;
        private LeaveStatusChange _st_change;
        bool open = true;
        bool isProcessing = false;
        long elementKey;
        BLUIElement pendingLeaveTb;
        private IPendingLeaveRequestValidator validator;
        private bool IsPendingLeaveValidationShown;
        #endregion.

        protected async override Task OnInitializedAsync()
        {
            elementKey = 1;
            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);// get element key from url 

            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();

                formrequest.MenuKey = elementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);
                buttonDefinition = await _navManger.GetMenuUIElement(formrequest);//get ui elements
            }

            BLUIElement btnContent = null;


            if (formDefinition != null)
            {
                //buttonDefinition = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("FormButton")).FirstOrDefault();
                buttonDefinition.Children = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("FormButton")).ToList();
                btnContent = buttonDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("FormButton")).FirstOrDefault();
                pendingLeaveTb = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("PendingLeaveGrid")).FirstOrDefault();
                
                formDefinition.IsDebugMode = true;
            }

            if (buttonDefinition != null && btnContent != null)
            {

                List<BLUIElement> buttons = formDefinition.Children.Where(x => x.ParentKey == btnContent.ElementKey).ToList();
                foreach (BLUIElement button in buttons)
                {
                    buttonDefinition.Children.Add(button);
                }
                
            }

            if (formDefinition != null)
            {
                List<BLUIElement> datas = new List<BLUIElement>();
                foreach (BLUIElement child in formDefinition.Children)
                {
                    if (child._internalElementName != null && child._internalElementName.Equals("FormButton"))
                    {
                        //
                    }
                    else if (child._internalElementName != null)
                    {
                        if (child._internalElementName != null && child._internalElementName.Equals("ButtonGroup"))
                        {
                            //
                        }
                        else if (child._internalElementName != null)
                        {
                            if (child._internalElementName != null && child._internalElementName.Equals("ShowMoreFilters"))
                            {
                                buttonDefinition.Children.Add(child);
                            }
                            else if (child._internalElementName != null)
                            {
                                datas.Add(child);
                            }
                        }
                    }

                }
                formDefinition.Children = datas;
            }

            if (pendingLeaveTb != null)
            {
                pendingLeaveTb.Children = formDefinition.Children.Where(x => x.ParentKey == pendingLeaveTb.ElementKey).ToList();
            }

            
            _interactionLogic = new Dictionary<string, EventCallback>();
            _interactionLogic2 = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();
            HookInteractions();
            InitializePendingLeaves();

            _leaveRequest.ObjKy = elementKey;
            _st_change.ObjKy = elementKey;

            await GetPendingLeaves(_leaveRequest);
        }
        private void HookInteractions()
        {
            InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
            InteractionHelper helper2 = new InteractionHelper(this,buttonDefinition);
            _interactionLogic2 = helper2.GenerateEventCallbacks();// generate all event callbacks
            _interactionLogic = helper.GenerateEventCallbacks();
            //AppSettings.RefreshTopBar("Pending Leave Request");
            appStateService._AppBarName = "Pending Leave Request";
        }

        private void UIStateChanged()
        {
            StateHasChanged();
        }
        private void InitializePendingLeaves()
        {
            //_blTb = new BLGrid<LeaveDetails>();
            _leaveRequest = new Leaverequest();
            _leaves = new List<LeaveDetails>();
            _leaveSummaryReq = new LeaveDetails();
            _st_change = new LeaveStatusChange();

        }
        private async Task GetPendingLeaves(Leaverequest req)
        {
            _leaves = await _hrManager.LeaveFilter(req);

            foreach (var itm in _leaves)
            {
                await SetValue("NextStatusCombo", itm.NextApprovedStatus);
                UIStateChanged();
            }

            UIStateChanged();
        }
        private void OnEmployeeChange(UIInterectionArgs<AddressResponse> args)
        {
            _leaveRequest.EmpKy = args.DataObject.AddressKey;
            UIStateChanged();
        }

        private void OnLeaveTypeChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            _leaveRequest.LeaveType = args.DataObject;
            UIStateChanged();
        }

        private void OnFromDateClick(UIInterectionArgs<DateTime?> args)
        {
            _leaveRequest.EftvDt = args.DataObject;
            UIStateChanged();
        }

        private void OnToDateClick(UIInterectionArgs<DateTime?> args)
        {
            _leaveRequest.ToD = args.DataObject;
            UIStateChanged();
        }
        private void OnNextStatusChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            _st_change.NextApprovedStatus = args.DataObject;
            UIStateChanged();
        }
        private void NextStatusCombo_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
        {
            if (args.sender != null)
            {
                LeaveDetails obj = (args.sender as BLCodeBaseCombo).ComboDataObject as LeaveDetails;
                if (obj != null)
                {
                    args.DataObject.AddtionalData.Add("ApproveStatusKey", obj.StatusKy);
                }
            }

        }
        private async void OnFilterClick(UIInterectionArgs<object> args)
        {
            isProcessing = true;
            UIStateChanged();


            await GetPendingLeaves(_leaveRequest);
            //_leaveRequest = new();
            //_leaveRequest.ObjKy = elementKey;

            isProcessing = false;
            UIStateChanged();


        }

        private void OnYesNo(UIInterectionArgs<bool> args)
        {
            _st_change.IsPaid = args.DataObject;
            UIStateChanged();
        }
        private async void ShowSummary(UIInterectionArgs<object> args)
        {
            await LoadLeaveSummary((long)args.DataObject.GetType().GetProperty("EmpKy")?.GetValue(args.DataObject, null), new DateTime(DateTime.Now.Year, 1, 1));
        }

        private async void OnSaveClick(UIInterectionArgs<object> args)
        {

            if (_st_change.NextApprovedStatus != null && _st_change.NextApprovedStatus.CodeKey != 1)
            {

                this.appStateService.IsLoaded = true;

                validator = new PendingLeaveRequestValidator(_st_change);

                if (validator != null && validator.CanApproveLeave())
                {
                    _st_change.TrnKy = (int)args.DataObject.GetType().GetProperty("LevTrnKy")?.GetValue(args.DataObject, null);
                    await _hrManager.ChangeLeaveStatus(_st_change);
                    await GetPendingLeaves(_leaveRequest);
                    this.appStateService.IsLoaded = false;
                }
                else
                {
                    this.appStateService.IsLoaded = false;
                    IsPendingLeaveValidationShown = true;
                }




                if (!_hrManager.IsExceptionthrown())
                {
                    _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackBar.Add("Approved Successfully", Severity.Success);
                }

                UIStateChanged();
            }
            else
            {
                //if next status is not selected ?? 
            }


        }
        async void ToggleDrawer(UIInterectionArgs<object> obj)
        {
            open = !open;

            await _jsRuntime.InvokeVoidAsync("CollapseExpand", open, "grid-section", "filter-section");
        }

        private void HideAllPopups()
        {
            IsPendingLeaveValidationShown = false;
            UIStateChanged();
        }
        private async Task LoadLeaveSummary(long empky, DateTime year)
        {
            _leaveSummaryReq.EmpKy = empky;
            _leaveSummaryReq.Year = year;
            _leaveRequest.LeaveSummary = await _hrManager.LoadLeaveSummary(_leaveSummaryReq);
            var parameters = new DialogParameters
            {
                ["LeaveRequest"] = _leaveRequest,
            };

            DialogOptions options = new DialogOptions() { CloseButton = true };
            var dialog = _dialogService.Show<LeaveSummaryPopup>("Leave Summary", parameters, options);


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
        private void ToggleViisbility(string name, bool visible)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                helper.UpdateVisibility(visible);
                UIStateChanged();
            }
        }
    }
}
