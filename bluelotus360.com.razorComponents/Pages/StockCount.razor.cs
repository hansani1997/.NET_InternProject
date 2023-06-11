using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.com.razorComponents.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using System.Transactions;
using BlueLotus360.CleanArchitecture.Application.Validators.Transaction;
using MudBlazor;
using System.Data.Common;
using bluelotus360.com.razorComponents.Services;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using bluelotus360.Com.commonLib.Helpers;
using bluelotus360.Com.commonLib.Managers.TransactionManager;

namespace bluelotus360.com.razorComponents.Pages
{
    public partial class StockCount
    {
        #region parameter        
        private BLUIElement formDefinition,findTransactionUI;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
        private IList<CodeBaseResponse> Services = new List<CodeBaseResponse>();
        private IList<CodeBaseResponse> HumanTypes = new List<CodeBaseResponse>();
        private IList<ItemResponse> Items = new List<ItemResponse>();
        private ITransactionValidator validator;
        private bool isInEditMode = false;
        private bool FindTransactionShown = false;
        private UIBuilder _refBuilder;
        private BLTransaction _transaction;
        BLUIElement stockCountGrid;
        private BLTable<TransactionLineItem> _blTb = new BLTable<TransactionLineItem>();
        long elementKey = 1;
        #endregion
        protected override async Task OnInitializedAsync()
        {
            //long elementKey = 1;
            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);
            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();
                formrequest.MenuKey = elementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);
            }

            if (formDefinition != null)
            {
                stockCountGrid = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("StockCountGrid")).FirstOrDefault();
                formDefinition.IsDebugMode = true;

            }
            if (stockCountGrid != null)
            {
                stockCountGrid.Children = formDefinition.Children.Where(x => x.ParentKey == stockCountGrid.ElementKey).ToList();
            }
            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();
            HookInteractions();
            await InitializeStockCount();
            validator = new StockCountValidatorMobile(_transaction);
          
        }

        private void HookInteractions()
        {

            InteractionHelper helper = new InteractionHelper(this, formDefinition);
            _interactionLogic = helper.GenerateEventCallbacks();
            appStateService._AppBarName = "Stock Count Report";
        }

        private async Task InitializeStockCount()
        {

            if (_transaction == null)
            {
                _transaction = new BLTransaction();
            }
            else
            {
                _transaction.InvoiceLineItems.Clear();
                _transaction.Address = new AddressResponse();
                _transaction.Account = new AccountResponse();
                _transaction.Amount = 0;
                _transaction.Amount2 = 0;
                _transaction.Amount3 = 0;
                _transaction.Amount4 = 0;
                _transaction.Amount5 = 0;
                _transaction.Amount6 = 0;
                _transaction.SubTotal = 0;
                _transaction.NetAmount = 0;
                _transaction.DeliveryDate = DateTime.Now;
                _transaction.TransactionKey = 1;
                _transaction.IsPersisted = false;
                _transaction.PaymentTerm = new CodeBaseResponse();
                _transaction.Code1 = new CodeBaseResponse();
                _transaction.SelectedLineItem = new TransactionLineItem();
                _transaction.CalculateTotals();
            }

            if (_objectHelpers == null)
            {
                _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();
            }
            findTransactionUI = new BLUIElement();
        }

        private async void NewTransaction(UIInterectionArgs<object> uIInterectionArgs)
        {
            InitializeStockCount();
            StateHasChanged();
        }

        private async void OnSearchTracsactionClick(UIInterectionArgs<object> args)
        {
            findTransactionUI = args.InitiatorObject;
            await ShowFindTransactionWindow();

        }

        private async Task ShowFindTransactionWindow()
        {
            HideAllPopups();
            FindTransactionShown = true;
            StateHasChanged();
            await Task.CompletedTask;
        }

        private async void HideAllPopups()
        {
            FindTransactionShown = false;
            //ReportShown = false;
            StateHasChanged();
            await Task.CompletedTask;
        }

        private async void OnTransactionItemChanged(UIInterectionArgs<ItemResponse> args)
        {
            _transaction.SelectedLineItem.TransactionItem = args.DataObject;
            if (BaseResponse.IsValidData(args.DataObject))
            {
                ItemRateResponse response = await RetriveRate(_transaction.SelectedLineItem);
                _transaction.SelectedLineItem.TransactionRate = response.TransactionRate;
                _transaction.SelectedLineItem.Rate = response.Rate;
            }
            await ReadData("CountUnit");
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

        private async void OnInvoiceSaveClick(UIInterectionArgs<object> args)
        {
            if (validator.CanSaveTransaction())
            {
                _transaction.ElementKey = elementKey;
                ExtendedTransaction etrans = await _transactionManager.SaveTransaction(_transaction);
                //_snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                //_snackBar.Add("Invoice has been  Saved Successfully", Severity.Success);
                await SetValue("HeaderTitle", _transaction.TransactionNumber);
                TransactionOpenRequest request = new TransactionOpenRequest();
                request.TransactionKey = _transaction.TransactionKey;
                await LoadTransaction(request);
                StateHasChanged();
                if(etrans.isSavedOnline)
                {
                    _snackBar.Add("Transaction Saved Online!", Severity.Success);
                }else if(etrans.isExceptionThrown)
                {
                    _snackBar.Add("Some Error Occured When Saving Data!", Severity.Error);
                }
                else
                {
                    _snackBar.Add("Transaction Saved Locally!", Severity.Warning);
                }
            }
            else
            {
                //_refUserMessage.ShowUserMessageWindow();
            }

            await Task.CompletedTask;
        }

        private async void OnPaymementTermChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            await Task.CompletedTask;
            StateHasChanged();
        }
        private async void OnCodeBaseComboChange(UIInterectionArgs<CodeBaseResponse> args)
        {

            _transaction.Location= args.DataObject;
            StateHasChanged();
        }

        public async Task<ItemRateResponse> RetriveRate(TransactionLineItem transactionItem)
        {
            ItemService itemService = new ItemService();
            itemService.ComboManager = _comboManager;
            return await itemService.RequestItemRateForTransaction(_transaction, transactionItem);
        }

        private void CountUnit_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
        {
            if (_transaction.SelectedLineItem.TransactionItem!=null)
            {
                args.DataObject.AddtionalData.Add("ItemKey", this._transaction.SelectedLineItem.TransactionItem.ItemKey);
            }
            
        }

        private void OnTranUnitChange(UIInterectionArgs<UnitResponse> args)
        {
            _transaction.SelectedLineItem.TransactionUnit = args.DataObject;
            StateHasChanged();
        }

        //private async void CountUnit_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
        //{
        //    args.DataObject.AddtionalData.Add("ItemKey", BaseResponse.GetKeyValue(this.transaction.SelectedLineItem.TransactionItem));
        //    args.CancelChange = !BaseResponse.IsValidData(this.transaction.SelectedLineItem.TransactionItem);
        //    await Task.CompletedTask;

        //}


        private async void OnItemEditClick(UIInterectionArgs<object> item)
        {
            isInEditMode = true;
            _transaction.EditingLineItem= (TransactionLineItem)item.DataObject;
            _transaction.SelectedLineItem.CopyFrom(_transaction.EditingLineItem);
            _objectHelpers["Item"].Refresh();
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnBarcodeGet(UIInterectionArgs<object> args)
        {
            var res = await _barcodeService.ReadBarcode();
            if (res != null)
            {
                //await ItemCodeAction(res.ToString());
                await SetValue("ItemCode", res);
            }
        }

        private async void OnItemCodeChanged(UIInterectionArgs<object> args)
        {
            if (args.DataObject != null && !string.IsNullOrWhiteSpace(args.DataObject.ToString()))
            {
                await ItemCodeAction(args.DataObject.ToString());

            }
            await Task.CompletedTask;

        }

        private async Task ItemCodeAction(string value)
        {
            ItemRequestModel requestModel = new ItemRequestModel();

            requestModel.ItemCode = value;
            var resp = await _comboManager.GetItemByItemCode(requestModel);
            if (resp.Count > 0)
            {
                await SetValue("Item", resp[0].ItemKey);
            }
            else
            {
                //_snackBar.Add($"Item With Item Code {requestModel.ItemCode} Not Found", Severity.Error);
            }
        }

        

        private async void OnNumericBoxChnaged(UIInterectionArgs<decimal> args)
        {
            _transaction.CalculateTotals();
            StateHasChanged();
            await Task.CompletedTask;
        }

        private void DiableTitle()
        {
            IBLUIOperationHelper helper;
            if(_objectHelpers.TryGetValue("HeaderTitle",out helper))
            {
                helper.ToggleEditable(false);
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
            }
        }

        private async void OnAddtoGridClick(UIInterectionArgs<object> args)
        {
            //if (validator.CanAddItemToGrid())
            //{
                if (!isInEditMode)
                {

                    var item = _transaction.InvoiceLineItems.Where(x => x.TransactionItem.ItemKey == _transaction.SelectedLineItem.TransactionItem.ItemKey).FirstOrDefault();
                    if (item != null)
                    {
                        item.IsDirty = true;
                        item.TransactionQuantity += _transaction.SelectedLineItem.TransactionQuantity;
                    }
                    else
                    {
                        _transaction.InvoiceLineItems.Add(_transaction.SelectedLineItem);

                    }

                }
                else {

                    isInEditMode = false;
                    _transaction.EditingLineItem.CopyFrom(_transaction.SelectedLineItem);
                    _transaction.SelectedLineItem = new();
                }
                _transaction.CalculateTotals();
                _transaction.InitilizeNewLineItem();
                isInEditMode = false;
            //}
            //else
            //{
                //_refUserMessage.ShowUserMessageWindow();
            //}
            await SetValue("ItemCode", "");
            await Focus("ItemCode");
            StateHasChanged();
            await Task.CompletedTask;
        }

        private async void OnOrderItemDelete(UIInterectionArgs<object> item)
        {
            var result = await _nativePopup.showYesNoDialog("Do you want to remove selected item ?");
            bool outres = (bool)result;
            if (outres)
            {
                TransactionLineItem tli = item.DataObject as TransactionLineItem;
                if (tli != null) {
                    _transaction.InvoiceLineItems.Where(x => x.TransactionItem.ItemKey == tli.TransactionItem.ItemKey).FirstOrDefault().IsActive = 0;
                    _transaction.CalculateCBalances();
                }
            }
            StateHasChanged();
            await Task.CompletedTask;

        }

        private async Task Focus(string name)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                await helper.FocusComponentAsync();
                StateHasChanged();
            }
        }

        private async Task LoadTransaction(TransactionOpenRequest request)
        {
            //HideAllPopups();
            DateTime dateTime1 = DateTime.Now;
            //isSaving = true;
            URLDefinitions urlDefinitions = new URLDefinitions();
            urlDefinitions.URL = formDefinition.ReadController + "/" + formDefinition.ReadAction;
            BLTransaction otransaction = await _transactionManager.OpenTransaction(request);
            otransaction.ElementKey = _transaction.ElementKey;
            _transaction.CopyFrom(otransaction);
            string valueN = "";
            IList<KeyValuePair<string, IBLUIOperationHelper>> pairs = _objectHelpers.ToList();

            foreach (KeyValuePair<string, IBLUIOperationHelper> helper in pairs)
            {
                await helper.Value.Refresh();

            }
            _transaction.CalculateTotals();
            // transaction.CalculateCBalances();

            //AppSettings.RefreshTopBar("Stock Count - " + transaction.TransactionNumber);
            //isSaving = false;
        }
    }
}
