using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.com.razorComponents.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using bluelotus360.Com.commonLib.Helpers;
using bluelotus360.Com.commonLib.Routes;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace bluelotus360.com.razorComponents.Pages.Utilities.Approval
{
    public partial class OrderApprovalStatusUpdateSimple
    {
        private BLUIElement formDefinition;
        private OrderFindDto aprovalStatusUpdateRequest;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
        private long elementKey;
        private IList<OrderFindResults> approvalDetails;
        BLUIElement approvalStatusTable;
       // private BLTelGrid<OrderFindResults> _blTb = new BLTelGrid<OrderFindResults>();
        private BLTable<OrderFindResults> _mudGrid = new BLTable<OrderFindResults>();
        OrderFindResults selectedOrder;

        protected override async Task OnInitializedAsync()
        {
            InitilizeApproval();
            elementKey = 1;
            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);
            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();
                formrequest.MenuKey = elementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);
            }
            if (formDefinition != null)
            {
                approvalStatusTable = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("DetailsTable")).FirstOrDefault();
                formDefinition.IsDebugMode = true;

            }

            if (approvalStatusTable != null)
            {
                approvalStatusTable.Children = formDefinition.Children.Where(x => x.ParentKey == approvalStatusTable.ElementKey).ToList();
            }


            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();
            HookInteractions();
        }

        private async void InitilizeApproval()
        {
            aprovalStatusUpdateRequest = new OrderFindDto();
            approvalDetails = new List<OrderFindResults>();
            approvalStatusTable = new BLUIElement();
            selectedOrder = new OrderFindResults();
        }

        private void HookInteractions()
        {
            InteractionHelper helper = new InteractionHelper(this, formDefinition);
            _interactionLogic = helper.GenerateEventCallbacks();
            //AppSettings.RefreshTopBar("Approval Status Update");
            appStateService._AppBarName = "Order Approval";
        }

        private void UIStateChanged()
        {
            this.StateHasChanged();
        }

        #region ui events

        private void OnFromDateClick(UIInterectionArgs<DateTime?> args)
        {
            aprovalStatusUpdateRequest.FromDate = (DateTime)args.DataObject;
            UIStateChanged();
        }
        private void OnToDateClick(UIInterectionArgs<DateTime?> args)
        {
            aprovalStatusUpdateRequest.ToDate = (DateTime)args.DataObject;
            UIStateChanged();
        }
        private void OnPrefixChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            aprovalStatusUpdateRequest.PrefixKey = args.DataObject.CodeKey;
            UIStateChanged();
        }
        private void OnOrdNoChange(UIInterectionArgs<string> args)
        {
            aprovalStatusUpdateRequest.OrderNo = args.DataObject;
            UIStateChanged();
        }
        private void OnAprStsChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            aprovalStatusUpdateRequest.ApproveStatusKey = args.DataObject.CodeKey;
            UIStateChanged();
        }
        private void OnTypeChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            aprovalStatusUpdateRequest.OrderTypeKey = args.DataObject.CodeKey;
            UIStateChanged();
        }

        private async void OnSearchClick(UIInterectionArgs<object> args)
        {
            
            if (aprovalStatusUpdateRequest != null)
            {
                this.appStateService.IsLoaded = true;

                aprovalStatusUpdateRequest.ObjectKey = (int)elementKey;
                approvalDetails = await _orderManager.LoadOrderApprovals(aprovalStatusUpdateRequest);
                this.appStateService.IsLoaded = false;
                UIStateChanged();
            }

        }

        private async void ShowAdditionalFilteringFields(UIInterectionArgs<object> args)
        {
            ToggleViisbility("Prefix", false);
            ToggleViisbility("OrdNo", false);
            ToggleViisbility("AprSts", false);

            if (approvalStatusTable != null && approvalStatusTable.Children != null && approvalStatusTable.Children.Count() > 0)
            {
                approvalStatusTable.Children.Where(x => x._internalElementName.Equals("OrdNo")).FirstOrDefault().IsVisible = false;
                approvalStatusTable.Children.Where(x => x._internalElementName.Equals("Prefix")).FirstOrDefault().IsVisible = false;
            }

            UIStateChanged();
        }

        private async void ShowMoreFilters_Toggled(UIInterectionArgs<object> args)
        {
            ToggleViisbility("Prefix", true);
            ToggleViisbility("OrdNo", true);
            ToggleViisbility("AprSts", true);
            if (approvalStatusTable != null && approvalStatusTable.Children != null && approvalStatusTable.Children.Count() > 0)
            {
                approvalStatusTable.Children.Where(x => x._internalElementName.Equals("OrdNo")).FirstOrDefault().IsVisible = true;
                approvalStatusTable.Children.Where(x => x._internalElementName.Equals("Prefix")).FirstOrDefault().IsVisible = true;
            }


            UIStateChanged();
        }

        private async void OnUpdateClick(UIInterectionArgs<object> args)
        {
            appStateService.IsGridLoaded = true;
			if (args.DataObject != null)
            {
                selectedOrder.RequestingObjectKey = (int)args.DataObject.GetType().GetProperty("RequestingObjectKey")?.GetValue(args.DataObject, null);
                selectedOrder.OrderKey = (int)args.DataObject.GetType().GetProperty("OrderKey")?.GetValue(args.DataObject, null);
                selectedOrder.IsActive = 1;
                selectedOrder.ApproveReason = new CodeBaseResponse();
                await _orderManager.UpadteOrderApprovals(selectedOrder);

                approvalDetails = await _orderManager.LoadOrderApprovals(aprovalStatusUpdateRequest);
            }
            appStateService.IsGridLoaded = false;

            _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
            _snackBar.Add("Updated Successfully", Severity.Success);

            StateHasChanged();
        }

        private async void OnApprovedStatusUpdate(UIInterectionArgs<CodeBaseResponse> args)
        {
            selectedOrder.ApproveState = args.DataObject;
            UIStateChanged();
        }

        private async void ApproveStatus_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
        {
            OrderFindResults find = new OrderFindResults();
            find = (args.sender as BLCodeBaseCombo).ComboDataObject as OrderFindResults;
            args.DataObject.AddtionalData.Add("ApproveStatusKey", find.ApproveState.CodeKey);
            args.DataObject.AddtionalData.Add("TransactionKey", find.OrderKey);
            args.DataObject.AddtionalData.Add("ObjectKey", find.RequestingObjectKey);
            args.DataObject.RequestingURL = BaseEndpoint.BaseURL + "Codebase/getApproveStatusComboDetail";
            UIStateChanged();
        }

        private async void OnPreviewClick(UIInterectionArgs<object> args)
        {
            if (args.DataObject != null)
            {
                string url = "";
                var urlobj = args.DataObject?.GetType()?.GetProperty("PreviewURL")?.GetValue(args.DataObject, null);
                if (urlobj != null)
                    url = urlobj.ToString();

                if (!string.IsNullOrEmpty(url))
                    await _jsRuntime.InvokeVoidAsync("open", url, "_blank");
            }
            await Task.CompletedTask;
            UIStateChanged();
        }

        #endregion

        #region object helpers

        private void ToggleViisbility(string name, bool visible)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                helper.UpdateVisibility(visible);
                UIStateChanged();
            }
        }

        #endregion
    }
}
