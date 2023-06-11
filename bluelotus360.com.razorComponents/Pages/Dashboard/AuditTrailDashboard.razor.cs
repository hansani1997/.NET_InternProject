using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.Com.commonLib.Helpers;
using bluelotus360.com.razorComponents.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using BlueLotus360.CleanArchitecture.Domain.Entities.Dashboard;
using bluelotus360.com.razorComponents.Pages.Dashboard.AuditTrailComponent;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BL10.CleanArchitecture.Domain.Entities.Dashboard;

namespace bluelotus360.com.razorComponents.Pages.Dashboard
{
    public partial class AuditTrailDashboard
    {
        #region parameter

        private BLUIElement formDefinition, gridUIElement, gridEnUIElement, gridUpUIElement, enterdmodalUIElement, updatedmodalUIElement, initFromDate;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;

        private BLTable<AuditTrail> _auditGrid;
        private BLTable<AuditTrailEnterdUpdatedResponse> _auditEnGrid;
        private BLTable<AuditTrailEnterdUpdatedResponse> _auditUpGrid;
        private BLAuditTrailEnterdSection _bLAuditTrailEnterdSection;
        private BLAuditTrailUpdatedSection _bLAuditTrailUpdatedSection;

        private BLTransaction transaction = new();
        private IList<AuditTrail> selectedUserList;
        private IList<AuditTrailEnterdUpdatedResponse> auditTrailEnterds;
        private IList<AuditTrailEnterdUpdatedResponse> auditTrailUpdateds;
        private AuditTrail selectUserRequset, loadedReq;
        private IList<AddressResponse> selectedUsers;

        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;

        private UIBuilder _refBuilder;

        long elementKey;

        private bool isShowEnterdSection = false;
        private bool isShowUpdatedSection = false;
        private bool isShow = true;
        private bool _checkIfExceptionReturn;

        #endregion

        #region General

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }
        protected override async Task OnInitializedAsync()
        {
            elementKey = 1;

            RefreshGrid();

            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);
            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();
                formrequest.MenuKey = elementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);

                gridUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("DetailsTable")).FirstOrDefault();
                enterdmodalUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("EnterdSection")).FirstOrDefault();
                updatedmodalUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("UpdatedSection")).FirstOrDefault();
                gridEnUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("EnterdTable")).FirstOrDefault();
                gridUpUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("UpdatedTable")).FirstOrDefault();

                if (gridUIElement != null)
                {
                    gridUIElement.Children = formDefinition.Children.Where(x => x.ParentKey == gridUIElement.ElementKey).ToList();
                }
                if (enterdmodalUIElement != null)
                {
                    enterdmodalUIElement.Children = formDefinition.Children.Where(x => x.ParentKey == enterdmodalUIElement.ElementKey).ToList();
                }
                if (updatedmodalUIElement != null)
                {
                    updatedmodalUIElement.Children = formDefinition.Children.Where(x => x.ParentKey == updatedmodalUIElement.ElementKey).ToList();
                }
                if (gridEnUIElement != null)
                {
                    gridEnUIElement.Children = formDefinition.Children.Where(x => x.ParentKey == gridEnUIElement.ElementKey).ToList();
                }
                if (gridUpUIElement != null)
                {
                    gridUpUIElement.Children = formDefinition.Children.Where(x => x.ParentKey == gridUpUIElement.ElementKey).ToList();
                }

                if (formDefinition != null)
                {
                    formDefinition.IsDebugMode = true;
                }
            }

            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();

            HookInteractions();

            //selectUserRequset.FromDate = Convert.ToDateTime(formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("FromDate")).FirstOrDefault().DefaultValue);
            //selectUserRequset.ToDate = Convert.ToDateTime(formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("ToDate")).FirstOrDefault().DefaultValue);
            var frdt = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("FromDate")).FirstOrDefault();
            selectUserRequset.FromDate = frdt != null && !string.IsNullOrEmpty(frdt.DefaultValue) ? Convert.ToDateTime(frdt.DefaultValue) : DateTime.Now;

            var todt = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("ToDate")).FirstOrDefault();
            selectUserRequset.ToDate = todt != null && !string.IsNullOrEmpty(todt.DefaultValue) ? Convert.ToDateTime(todt.DefaultValue) : DateTime.Now;

            selectUserRequset.ElementKey = elementKey;

            selectedUserList = await _dashboardManager.GetAuditTrailDetails(selectUserRequset);

        }
        private void HookInteractions()
        {
            InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
            _interactionLogic = helper.GenerateEventCallbacks();// generate all event callbacks 
            appStateService._AppBarName = "Audit Trail Dashboard";
        }
        private void UIStateChanged()
        {
            this.StateHasChanged();
        }
        private void RefreshGrid()
        {
            selectUserRequset = new AuditTrail();
            selectedUserList = new List<AuditTrail>();
            auditTrailEnterds = new List<AuditTrailEnterdUpdatedResponse>();
            auditTrailUpdateds = new List<AuditTrailEnterdUpdatedResponse>();
            selectedUsers = new List<AddressResponse>();
            loadedReq = new AuditTrail();
        }

        #endregion

        #region UI Event

        private async void OnClickFromDate(UIInterectionArgs<DateTime?> args)
        {
            selectUserRequset.FromDate = (DateTime)args.DataObject;
            UIStateChanged();
        }
        private async void OnClickToDate(UIInterectionArgs<DateTime?> args)
        {
            selectUserRequset.ToDate = (DateTime)args.DataObject;
            UIStateChanged();
        }
        private async void OnClickUser(UIInterectionArgs<AddressResponse> args)
        {
            selectUserRequset.User = args.DataObject;
            UIStateChanged();
        }
        private async void OnClickLoad(UIInterectionArgs<object> args)
        {
            selectedUsers.Add(selectUserRequset.User);
            loadedReq = selectUserRequset;

            selectedUserList.Clear();
            //_auditGrid.Refresh();
            try
            {
                if (selectUserRequset.User.AddressKey > 1)
                {
                    selectedUserList = await _dashboardManager.GetAuditTrailDetails(selectUserRequset);

                }
                else
                {
                    selectedUserList = await _dashboardManager.GetAuditTrailDetails(selectUserRequset);

                }

                UIStateChanged();

            }
            catch(Exception exp)
            {
                _checkIfExceptionReturn = true;
                Console.WriteLine("An exception occurred: " + exp);
            }
        }

        private async void OnEnterdSection(UIInterectionArgs<object> args)
        {
            this.appStateService.IsLoaded = true;
            UIStateChanged();

            AuditTrail arg = new AuditTrail();

            arg = (AuditTrail)args.DataObject;
            selectUserRequset.User = arg.User;

            await SetValue("EnteredTransactions", selectUserRequset.User.AddressName);

            auditTrailEnterds = await _dashboardManager.GetAuditTrailEnterdDetails(selectUserRequset);

            //if (_auditEnGrid != null)
            //{
            //    _auditEnGrid.Refresh();
            //}

            isShow = false;
            isShowEnterdSection = true;

            this.appStateService.IsLoaded = false;
            UIStateChanged();
        }

        private async void OnUpdatedSection(UIInterectionArgs<object> args)
        {
            this.appStateService.IsLoaded = true;
            UIStateChanged();

            AuditTrail arg = new AuditTrail();

            arg = (AuditTrail)args.DataObject;
            selectUserRequset.User = arg.User;

            await SetValue("UpdatedTransactions", selectUserRequset.User.AddressName);

            auditTrailUpdateds = await _dashboardManager.GetAuditTrailUpdatedDetails(selectUserRequset);

            //if (_auditUpGrid != null)
            //{
            //    _auditUpGrid.Refresh();
            //}

            isShow = false;
            isShowUpdatedSection = true;

            this.appStateService.IsLoaded = false;
            UIStateChanged();
        }

        #endregion

        #region Enterd Section UI Event

        private async void OnBackEnButton(UIInterectionArgs<object> args)
        {
            await SetValue("FromDate", selectUserRequset.FromDate);
            await SetValue("ToDate", selectUserRequset.ToDate);
           
            isShowEnterdSection = false;
            isShow = true;

            UIStateChanged();
        }

        #endregion

        #region Updated Section UI Event

        private async void OnBackUpButton(UIInterectionArgs<object> args)
        {
            await SetValue("FromDate", selectUserRequset.FromDate);
            await SetValue("ToDate", selectUserRequset.ToDate);

            isShowUpdatedSection = false;
            isShow = true;

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

        private async Task RefreshComponent(string name)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                helper.Refresh();
                UIStateChanged();
            }
        }
        #endregion
    }
}
