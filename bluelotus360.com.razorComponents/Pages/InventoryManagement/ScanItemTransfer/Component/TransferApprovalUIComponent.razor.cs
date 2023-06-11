using bluelotus360.com.razorComponents.Extensions;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using bluelotus360.Com.commonLib.Helpers;
using BlueLotus360.CleanArchitecture.Application.Validators.InventoryManagement;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.HR;
using BlueLotus360.CleanArchitecture.Domain.Entities.InventoryManagement.ItemTransfer;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using Microsoft.AspNetCore.Components;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Pages.InventoryManagement.ScanItemTransfer.Component
{
    public partial class TransferApprovalUIComponent
    {
        private string _scanText = "";
        [Parameter] public ItemTransfer Transfer{ get;set;}
        [Parameter] public BLUIElement UIdefinition { get;set;}
        [Parameter] public long ObjectKey { get; set; }

        [Parameter] public EventCallback<TransferOpenRequest> OnAfterApprove { get; set; }

        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;


        private IList<ItemTransferLineItem> _RequiredToApprovedItemsList=new List<ItemTransferLineItem>();
        private IItemTransferValidator validator;
        private IList<ItemTransferLineItem> _invoiceItemList = new List<ItemTransferLineItem>();
        bool IsValidationShown;
        CodeBaseResponse _approvedstate = new CodeBaseResponse();
        BLUIElement _grid1= new BLUIElement();
        private UIGrid<ItemTransferLineItem> _blTb=new UIGrid<ItemTransferLineItem>();
        protected override async Task OnInitializedAsync() 
        {
            InteractionHelper helper = new InteractionHelper(this, UIdefinition);
            _interactionLogic = helper.GenerateEventCallbacks();
            if (_objectHelpers == null || _objectHelpers.Count == 0)
            {
                _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();
            }
            _grid1 = UIdefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("Grid1")).FirstOrDefault();
            LoadTransfer();
        }

        protected override async Task OnParametersSetAsync()
        {
            

            //
            await base.OnParametersSetAsync();
        }

        private async void OnScanTextCapturer(UIInterectionArgs<string> args)
        {
            _scanText = args.DataObject;
            StateHasChanged();
        }

        private async void OnScanTextCapturerEnter(UIInterectionArgs<object> args)
        {
            string str = _scanText;

            this.appStateService.IsLoaded = true;
            foreach (var itm in _invoiceItemList)
            {
                if (itm.serialNo.Trim().Equals(str.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    Transfer.SelectedLineItem = itm;
                    Transfer.SelectedLineItem.ObjectKey = (int)ObjectKey;
                    break;
                }
            }

            validator = new ItemTransferValidator(Transfer);
            if (validator != null && validator.CanApproveByScan())
            {
                _RequiredToApprovedItemsList.Add(Transfer.SelectedLineItem);
                await _itemTransferManager.ItmTrnSerAprInsertWeb(_RequiredToApprovedItemsList);
                Transfer.SelectedLineItem = new ItemTransferLineItem();

                _scanText = "";
                await SetValue("ApprovalSection_ScanField", _scanText);

                LoadTransfer();
            }
            else
            {
                foreach (var msg in validator.UserMessages.UserMessages)
                {
                    ShowScannedValidations(msg.Message);
                }

            }


            this.appStateService.IsLoaded = false;
            StateHasChanged();
        }
        private async void OnScanForApproving(UIInterectionArgs<object> args)
        {
            //ItmtrnsferValidationResponse validationResponse = new ItmtrnsferValidationResponse();
            string str = _scanText;

            this.appStateService.IsLoaded = true;
            foreach (var itm in _invoiceItemList)
            {
                if (itm.serialNo.Trim().Equals(str.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    Transfer.SelectedLineItem=itm;
                    Transfer.SelectedLineItem.ObjectKey = (int)ObjectKey;
                    break;
                }
            }

            validator = new ItemTransferValidator(Transfer);
            if (validator!=null && validator.CanApproveByScan())
            {
                _RequiredToApprovedItemsList.Add(Transfer.SelectedLineItem);
                await _itemTransferManager.ItmTrnSerAprInsertWeb(_RequiredToApprovedItemsList);

                Transfer.SelectedLineItem = new ItemTransferLineItem();

                _scanText = "";
                await SetValue("ApprovalSection_ScanField", _scanText);

                LoadTransfer();

                //if (validationResponse != null && validationResponse.HasError)
                //{
                //    ShowScannedValidations(validationResponse.Message);
                //}
            }
            else
            {
                foreach (var msg in validator.UserMessages.UserMessages)
                {
                    ShowScannedValidations(msg.Message);
                }
                
            }
            

            this.appStateService.IsLoaded = false;
            StateHasChanged();
        }
        private async void LoadTransfer()
        {
            
            _invoiceItemList = (await _itemTransferManager.GetItemtransferLineItemForApproval(Transfer)).ToList();

            if (OnAfterApprove.HasDelegate)
            {
                TransferOpenRequest req = new TransferOpenRequest() { TrnKy = Transfer.FromTransactionKey };
                await OnAfterApprove.InvokeAsync(req);
            }

            StateHasChanged();
        }

        private async void ShowScannedValidations(string msg)
        {
            Transfer.ValidationMessages.Add(msg);
            validator = new ItemTransferValidator(Transfer);
            validator.AddValidationErrors();
            IsValidationShown = true;
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
        private async Task SetValue(string name, object value)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                await helper.SetValue(value);
                StateHasChanged();
                await Task.CompletedTask;
            }
        }
    }
}
