using BlueLotus360.CleanArchitecture.Application.Validators.Transaction;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.com.razorComponents.Extensions;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueLotus360.CleanArchitecture.Domain.DTO.Report;
using bluelotus360.Com.commonLib.Reports.Telerik;
using Microsoft.JSInterop;
using bluelotus360.Com.commonLib.Helpers;
using Microsoft.AspNetCore.Components.Web;
using bluelotus360.com.razorComponents.MB.Shared.Components.Dialogs;
using bluelotus360.com.razorComponents.MB.Shared.Components.Popups;
using Toolbelt.Blazor.HotKeys2;
using bluelotus360.com.razorComponents.MB.Settings.Theme.Enums;
using static ServiceStack.Diagnostics;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;

namespace bluelotus360.com.razorComponents.Pages.Invoice
{
    public partial class InvoiceForm : IDisposable
    {
        #region parameter

        private BLUIElement formDefinition;//wht
        private BLTransaction transaction = new();

        private BLPriceList _refPriceList;

        private PriceListRequest __priceListRequest;

        private UIBuilder _refBuilder;


        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        /** followi is must and you need to pass **/
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
        private bool tableloading = false;
        MudMessageBox addItem { get; set; }

        private MudTable<TransactionLineItem> _table;

        private ITransactionValidator validator;
        bool isSaving;
        public IList<PriceListResponse> price_list_response { get; set; }
        bool hasItemCode;
        bool showAlert;
        bool isExpansionPanelOpen;
       
        HotKeysContext ShortCutKeysContext;

        private BLUIElement CashInUIComponents;
        private BLUIElement CashOutUIComponents;
        private BLUIElement RecieptUIComponents;
        private BLUIElement InvoicePrinterURL;
        private BLUIElement findTrandsactionUI;
        private BLUIElement invoiceDetailsGrid;
        private BLUIElement replacementDetails;
        private BLUIElement ontherSpotPayement;
        private BLUIElement bLPriceListUI;

        private MudNumericField<decimal> _refCashAmount;
        private MudNumericField<decimal> _refCashPayementAmount;
        private MudNumericField<decimal> _refCardPayementAmount;



        private bool PriceListShown = false;
        private bool CashInShown = false;
        private bool CashOutShown = false;
        private bool FindTransactionShown = false;
        private bool RecieptsWindowShown = false;
        private bool CashDenominationShown = false;

        private bool DayEndReportShown = false;

        private CashIn _refCashInWindow = new CashIn();
        private CashDenominatorEntry _refCashDenominatorWindow = new CashDenominatorEntry();
        private CashIn _refCashOutWindow = new CashIn();
        private Reciept _refRecieptWindow;

        private PriceListResponse __currentPriceListResponse;


        private bool ReplacementMode = false;

        private BLTransaction ReplacementTransaction;

        private string _tableHeight = "400px;";


        private ReportCompanyDetailsResponse _companyDetails;


        private TerlrikReportOptions _dayEndReportOption;

        BLTable<TransactionLineItem> _mudGrid = new BLTable<TransactionLineItem>();
        BLTable<TransactionLineItem> _mudReplacementGrid = new BLTable<TransactionLineItem>();
        private bool showPriceListAlert = false;
        long contraAccObjKy = 1;
        #endregion

        #region General

        protected override async Task OnInitializedAsync()
        {
            var dimension = await _jsRuntime.InvokeAsync<WindowDimension>("getWindowDimensions");
            await _jsRuntime.InvokeAsync<WindowDimension>("LogConsole", dimension);

            if (dimension.Height < 800)
            {
                _tableHeight = "200px";
            }
            else
            {
                _tableHeight = "300px";
            }



            long elementKey = 1;
            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);// get element key from url 
            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();
                formrequest.MenuKey = elementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);//get ui elements
                CashInUIComponents = formDefinition.Children.Where(x => x._internalElementName.Equals("_CashIn")).FirstOrDefault();
                CashOutUIComponents = formDefinition.Children.Where(x => x._internalElementName.Equals("_CashOut")).FirstOrDefault();
                RecieptUIComponents = formDefinition.Children.Where(x => x._internalElementName.Equals("_Reciept_")).FirstOrDefault();
                InvoicePrinterURL = formDefinition.Children.Where(x => x._internalElementName.Equals("_Invoice_Printer_URL_")).FirstOrDefault();
                findTrandsactionUI = formDefinition.Children.Where(x => x._internalElementName.Equals("_SearchTransaction_")).FirstOrDefault();
                invoiceDetailsGrid = formDefinition.Children.Where(x => x._internalElementName.Equals("InvoiceDetailsGrid")).FirstOrDefault();
                replacementDetails = formDefinition.Children.Where(x => x._internalElementName.Equals("ReplacementDetails")).FirstOrDefault();
                ontherSpotPayement = formDefinition.Children.Where(x => x._internalElementName.Equals("OntherSpotPayement")).FirstOrDefault();
                bLPriceListUI = formDefinition.Children.Where(x => x._internalElementName.Equals("BLPriceListUI")).FirstOrDefault();
                contraAccObjKy = 176416;//TO DO remove this one after contraAccount is enabled

                if (CashInUIComponents != null)
                {
                    CashInUIComponents.Children = formDefinition.Children.Where(x => x.ParentKey == CashInUIComponents.ElementKey).ToList();
                }
                if (CashOutUIComponents != null)
                {
                    CashOutUIComponents.Children = formDefinition.Children.Where(x => x.ParentKey == CashOutUIComponents.ElementKey).ToList();
                }
                if (RecieptUIComponents != null)
                {
                    RecieptUIComponents.Children = formDefinition.Children.Where(x => x.ParentKey == RecieptUIComponents.ElementKey).ToList();

                }
                if (invoiceDetailsGrid != null)
                {
                    invoiceDetailsGrid.Children = formDefinition.Children.Where(x => x.ParentKey == invoiceDetailsGrid.ElementKey).ToList();

                }
                if (replacementDetails != null)
                {
                    replacementDetails.Children = formDefinition.Children.Where(x => x.ParentKey == replacementDetails.ElementKey).ToList();

                }
                if (ontherSpotPayement != null)
                {
                    ontherSpotPayement.Children = formDefinition.Children.Where(x => x.ParentKey == ontherSpotPayement.ElementKey).ToList();

                }
                if (bLPriceListUI != null)
                {
                    bLPriceListUI.Children = formDefinition.Children.Where(x => x.ParentKey == bLPriceListUI.ElementKey).ToList();

                }
            }

            formDefinition.IsDebugMode = true;
            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();
            HookInteractions();
            __priceListRequest = new PriceListRequest();
            __priceListRequest.ElementKey = elementKey;
            //await _sessionStorage.ClearAsync();
            transaction.ElementKey = elementKey;
            InitilizeShortcuts();
            InitilizeNewOrder();
            CompletedUserAuth auth = await _authenticationManager.GetUserInformation();
            _dayEndReportOption = new TerlrikReportOptions();
            _dayEndReportOption.ReportName = "DayEndReportForAnyDate.trdp";
            _dayEndReportOption.ReportParameters = new Dictionary<string, object>();
            _dayEndReportOption.ReportParameters.Add("Cky", auth.AuthenticatedCompany.CompanyKey);
            _dayEndReportOption.ReportParameters.Add("UsrKy", auth.AuthenticatedUser.UserKey);
            _dayEndReportOption.ReportParameters.Add("ObjKy", elementKey);
            _dayEndReportOption.ReportParameters.Add("FrmDt", DateTime.Now);
            _dayEndReportOption.ReportParameters.Add("LocKy", 1);


        }

        private async void InitilizeShortcuts()
        {
            var SpecialKeys = ModCode.Ctrl | ModCode.Shift;
            this.ShortCutKeysContext = this.HotKeys.CreateContext()
                    .Add(ModCode.None, Code.F2, async () => await ShowPriceList(), "Show Price List", Exclude.None)
                    .Add(ModCode.None, Code.F7, ShowCashIn, "Show Cash In", Exclude.None)
                    .Add(ModCode.None, Code.F8, ShowCashOut, "Show Cash Out", Exclude.None)
                    .Add(ModCode.Ctrl, Code.S, async () => await SaveInvoice(), "Save Transaction", Exclude.None)
                    .Add(ModCode.Shift, Code.N, InitilizeNewOrder, "New Order", Exclude.None)
                    .Add(ModCode.Ctrl, Code.F, async () => await ShowFindTransactionWindow(), "Find Transaction", Exclude.None)
                    .Add(ModCode.Ctrl, Code.Q, FocusCashAmount, "Focus Cash Entry", Exclude.None)
                    .Add(ModCode.Ctrl, Code.P, async () => await DirectPrintInvoice(), "Print", Exclude.None)
                    .Add(SpecialKeys, Code.A, FocusDate, "Date Select", Exclude.None)
                    .Add(SpecialKeys, Code.S, FocusDocNo, "DocNo Select", Exclude.None)
                    .Add(SpecialKeys, Code.D, FocusLocation, "Location Select", Exclude.None)
                    .Add(SpecialKeys, Code.F, FocusPriceList, "Price List Select", Exclude.None)
                    .Add(SpecialKeys, Code.H, FocusPayementTerm, "Price List Select", Exclude.None)
                ;

            //var SpecialKeys = ModKeys.Ctrl | ModKeys.Shift;
            //this.ShortCutKeysContext = HotKeys.CreateContext()
            //   .Add(ModKeys.None, Keys.F2, ShowPriceList, "Show Price List", Exclude.None)
            //    .Add(ModKeys.None, Keys.F7, ShowCashIn, "Show Cash In", Exclude.None)
            //    .Add(ModKeys.None, Keys.F8, ShowCashOut, "Show Cash Out", Exclude.None)
            //    .Add(ModKeys.Ctrl, Keys.S, SaveInvoice, "Save Transaction", Exclude.None)
            //    .Add(ModKeys.Shift, Keys.N, InitilizeNewOrder, "New Order", Exclude.None)
            //     .Add(ModKeys.Ctrl, Keys.F, ShowFindTransactionWindow, "Find Transaction", Exclude.None)
            //    .Add(ModKeys.Ctrl, Keys.Q, FocusCashAmount, "Focus Cash Entry", Exclude.None)
            //    .Add(ModKeys.Ctrl, Keys.P, DirectPrintInvoice, "Print", Exclude.None)
            //    .Add(SpecialKeys, Keys.A, FocusDate, "Date Select", Exclude.None)
            //    .Add(SpecialKeys, Keys.S, FocusDocNo, "DocNo Select", Exclude.None)
            //    .Add(SpecialKeys, Keys.D, FocusLocation, "Location Select", Exclude.None)
            //    .Add(SpecialKeys, Keys.F, FocusPriceList, "Price List Select", Exclude.None)
            //    .Add(SpecialKeys, Keys.H, FocusPayementTerm, "Price List Select", Exclude.None)
            //   ;
        }

        private async void InitilizeNewOrder()
        {
            transaction = new BLTransaction();
            transaction.InvoiceLineItems.Clear();
            transaction.IsPersisted = false;
            transaction.TransactionKey = 1;
            transaction.ElementKey = formDefinition.ElementKey;
            validator = new TransactionValidator(transaction);
            ToggleEditability("LineEditCancel", false);
            await SetValue("HeaderTitle", "New");

        }


        protected override bool ShouldRender()
        {
            return true;
        }



        public void Dispose()
        {
            ShortCutKeysContext.Dispose();
        }



        private void HookInteractions()
        {

            InteractionHelper helper = new InteractionHelper(this, formDefinition);
            _interactionLogic = helper.GenerateEventCallbacks();
            appStateService._AppBarName = "POS Simple Invoice";

        }

        private void UIStateChanged()
        {

            this.StateHasChanged();
        }

        #endregion

        #region Invoice Related Events
        /// <summary>
        /// Following will be called when the Header Location Combo is changed
        /// </summary>
        /// <param name="args"></param>
        private async void OnTransactionLocationChanged(UIInterectionArgs<CodeBaseResponse> args)
        {

            await SelectAccountByLocationAndPayementTerm();
            await ReadPriceList();
            UIStateChanged();
        }

        private async void TransactionAddressChange(UIInterectionArgs<AddressResponse> args)
        {

            UIStateChanged();
            await Task.CompletedTask;
        }

        private void OnTransactionCustomerChanged(UIInterectionArgs<AddressResponse> args)
        {

            UIStateChanged();
        }
        private async void OnPriceListComboChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            await Task.CompletedTask;
            UIStateChanged();
        }

        private async Task ReadPriceList()
        {
            if (transaction.Location.CodeKey < 11)
            {
                return;
            }
            await _refPriceList.ReadPriceList();
        }

        private async void OnPayementTermChanged(UIInterectionArgs<CodeBaseResponse> args)
        {
            transaction.PaymentTerm = args.DataObject;
            await SelectAccountByLocationAndPayementTerm();
            UIStateChanged();

        }
        private async void OnLineTransactionUnitChange(UIInterectionArgs<UnitResponse> args)
        {
            if (transaction.SelectedLineItem.IsInEditMode)
            {
                return;
            }
            transaction.SelectedLineItem.TransactionUnit = args.DataObject;
            if (__currentPriceListResponse != null)
            {
                decimal? TransactionRate = __currentPriceListResponse.GetPriceByUnit(args.DataObject);
                if (TransactionRate.HasValue)
                {
                    this.transaction.SelectedLineItem.TransactionRate = TransactionRate.Value;
                    RefreshComponent("ItemName");
                    RefreshComponent("TransactionQuantiy");
                    await Focus("TransactionQuantiy");
                }
            }
            UIStateChanged();
        }
        private async void TryAddItemOnEnter(UIInterectionArgs<decimal> args)
        {
            await Focus("TransactionRate");

        }

        private async void OnTransactionRateEnterKey(UIInterectionArgs<decimal> args)
        {
            await Focus("DiscountPercentage");

        }

        private async void OnDisPerEnterKey(UIInterectionArgs<decimal> args)
        {
            await Focus("TotalRate");

        }



        private async void TransactionRepChange(UIInterectionArgs<AddressResponse> args)
        {
            UIStateChanged();
            await Task.CompletedTask;
        }
        private async void OnNetRateEnterKey(UIInterectionArgs<decimal> args)
        {
            onAddToGridClick(null);
            await Task.CompletedTask;

        }



        private async void OnEnterItemCode(UIInterectionArgs<object> args)
        {

            var ex = (args.e as KeyboardEventArgs);
            if (ex.Code.Equals("Enter") || ex.Code.Equals("NumpadEnter"))
            {
                string ItemCodde = args.DataObject as string;
                if (ItemCodde == null || ItemCodde.Length == 0)
                {
                    _refPriceList.SetSearch("");
                    await ShowPriceList();
                }
                else
                {
                    PriceListResponse response = _refPriceList.FindExact(ItemCodde);
                    __currentPriceListResponse = response;

                    if (response == null)
                    {
                        await ShowPriceList();
                        _refPriceList.SetSearch(ItemCodde);

                    }
                    else
                    {
                        await AddItemBasedOnPriceList(response);
                    }
                }
            }

            UIStateChanged();

        }


        private async Task PostItemAddActions()
        {
            RefreshComponent("ItemName");
            RefreshComponent("TransactionQuantiy");
            await SetDataSource("LineTransactionUnit", __currentPriceListResponse.GetComboResponseByPriceList());

            await Focus("TransactionQuantiy");
            StockAsAtResponse stockAsAtResponse = await GetStockResponseForCurrentItem();

            BLUIElement textElement = GetLinkedUIElement("ItemCode");
            if (textElement != null)
            {
                textElement.ElementCaption = "Item Code - AvlQty : " + stockAsAtResponse.StockAsAt.ToString();
                await SetValue("ItemCode", transaction.SelectedLineItem.TransactionItem.ItemCode);
                RefreshComponent("ItemCode");
                await CalculatePriceBasedOnPriceList(1);
            }



        }

        private async Task<StockAsAtResponse> GetStockResponseForCurrentItem()
        {
            StockAsAtRequest request = new StockAsAtRequest();
            request.ElementKey = transaction.ElementKey;
            request.LocationKey = transaction.Location.CodeKey;
            request.ProjectKey = 1;
            request.ItemKey = transaction.SelectedLineItem.TransactionItem.ItemKey;
            StockAsAtResponse stockAsAtResponse = await _transactionManager.GetStockAsAt(request);
            return stockAsAtResponse;
        }

        private async void OnLineTransactionQuantityChanged(UIInterectionArgs<decimal> args)
        {

            this.transaction.SelectedLineItem.TransactionQuantity = args.DataObject;
            await CalculatePriceBasedOnPriceList(args.DataObject);
            UIStateChanged();

        }

        private async Task CalculatePriceBasedOnPriceList(decimal Qty)
        {

            transaction.SelectedLineItem.LineNetRate = transaction.SelectedLineItem.GetLineTotalWithDiscount();
            transaction.SelectedLineItem.TransactionQuantity = Qty;
            await Task.CompletedTask;

        }

        private async void OnLineTransactionRateChanged(UIInterectionArgs<decimal> args)
        {
            transaction.SelectedLineItem.TransactionRate = args.DataObject;
            await CalculatePriceBasedOnPriceList(transaction.SelectedLineItem.TransactionQuantity);
            UIStateChanged();
        }

        private async void OnLineDisPerChange(UIInterectionArgs<decimal> args)
        {
            transaction.SelectedLineItem.DiscountPercentage = args.DataObject;
            transaction.SelectedLineItem.LineNetRate = transaction.SelectedLineItem.GetLineTotalWithDiscount();
            await Task.CompletedTask;
            UIStateChanged();
        }

        private async void OnLineCancelClick(UIInterectionArgs<object> args)
        {
            InitNewLine();
            await Task.CompletedTask;
        }

        private async void InitNewLine()
        {
            this.transaction.SelectedLineItem = new TransactionLineItem();
            ToggleEditability("ItemCode", true);
            _objectHelpers["ItemCode"].ResetToInitialValue();
            _objectHelpers["ItemName"].ResetToInitialValue();
            _objectHelpers["TransactionRate"].ResetToInitialValue();
            _objectHelpers["TransactionQuantiy"].ResetToInitialValue();
            _objectHelpers["LineTransactionUnit"].ResetToInitialValue();
            _objectHelpers["DiscountPercentage"].ResetToInitialValue();
            this.transaction.EditingLineItem = null;
            ToggleEditability("LineEditCancel", false);
            await Focus("ItemCode");
            StateHasChanged();
        }

        private void OnContraAccountChange(UIInterectionArgs<AccountResponse> args)
        {
            transaction.ContraAccountObjectKey = args.InitiatorObject.ElementKey;
        }
        private void OnAccountChanged(UIInterectionArgs<AccountResponse> args)
        {
            transaction.AccountObjectKey = args.InitiatorObject.ElementKey;
        }

        private void OnCashInClick(UIInterectionArgs<object> args)
        {
            ShowCashIn();
        }

        private void OnCashDenomationClick(UIInterectionArgs<object> args)
        {
            ShowCashDenominations();
        }




        private void OnCashOutClick(UIInterectionArgs<object> args)
        {
            ShowCashOut();
        }

        private async void OnPriceListClick(UIInterectionArgs<object> args)
        {
            await ShowPriceList();
        }

        private void OnNewClick(UIInterectionArgs<object> args)
        {
            InitilizeNewOrder();
            UIStateChanged();
        }
        #endregion

        #region Read Hooks

        #region Add/Edit/Delete methods

        private async void onAddToGridClick(UIInterectionArgs<object> args)
        {
            StockAsAtResponse stockAsAtResponse = await GetStockResponseForCurrentItem();

            transaction.ContraAccountObjectKey = contraAccObjKy;
            validator = new TransactionValidator(transaction);
            
            if (validator.CanAddItemToGrid(100))
            {
                if (transaction.SelectedLineItem.IsInEditMode)
                {
                    if (transaction.EditingLineItem != null)
                    {
                        transaction.SelectedLineItem.IsInEditMode = false;
                        transaction.SelectedLineItem.IsDirty = true;
                        transaction.EditingLineItem.CopyFrom(transaction.SelectedLineItem);
                        if (ReplacementMode)
                        {
                            transaction.EditingLineItem.Quantity2 *= -1;
                            transaction.EditingLineItem.TransactionQuantity *= -1;
                            transaction.EditingLineItem.Quantity *= -1;
                            transaction.SelectedLineItem = new();
                        }
                    }
                }
                else
                {
                    await AddToGrid();

                    if (showAlert)
                    {
                        showAlert = false;
                    }
                    if (isExpansionPanelOpen)
                    {
                        isExpansionPanelOpen = false;
                    }
                }

                transaction.SubTotal = transaction.GetOrderTotalWithDiscounts();
                SetInitialPaymentCashCardAmounts();
            }
            else
            {
                showAlert = true;
                isExpansionPanelOpen = true;
            }
            InitNewLine();
            UIStateChanged();
        }


        private async Task AddToGrid()
        {

            if (ReplacementMode)
            {
                this.ReplacementTransaction.SelectedLineItem.LineNumber = this.transaction.InvoiceLineItems.Count() + 1;
                ReplacementTransaction.AddGridItems(this.transaction.SelectedLineItem);
                this.transaction.SelectedLineItem = new();
            }
            else
            {

                transaction.AddGridItems(this.transaction.SelectedLineItem);
            }
            _objectHelpers["ItemCode"].ResetToInitialValue();
            _objectHelpers["ItemName"].ResetToInitialValue();
            _objectHelpers["TransactionRate"].ResetToInitialValue();
            _objectHelpers["TransactionQuantiy"].ResetToInitialValue();
            _objectHelpers["LineTransactionUnit"].ResetToInitialValue();
            _objectHelpers["DiscountPercentage"].ResetToInitialValue();
            
            isExpansionPanelOpen = true;
            UIStateChanged();
            await Task.CompletedTask;
        }

        //private async void OnOrderItemEdit(decimal index)
        //{
        //    var updateOrderItem = transaction.InvoiceLineItems.ElementAt((int)index);

        //    await this.ShowEditItem(updateOrderItem);

        //}

        //private async void OnItemEditClick(TransactionLineItem item)
        //{

        //    item.IsInEditMode = true;
        //    transaction.EditingLineItem = item;
        //    this.transaction.SelectedLineItem.CopyFrom(item);
        //    StockAsAtResponse stockAsAtResponse = await GetStockResponseForCurrentItem();
        //    long currentUnitKey = item.TransactionUnit.UnitKey;


        //    await ReadData("LineTransactionUnit");
        //    await SetValue("LineTransactionUnit", currentUnitKey);

        //    BLUIElement textElement = GetLinkedUIElement("ItemCode");
        //    if (textElement != null)
        //    {
        //        textElement.ElementCaption = "Item Code - AvlQty : " + stockAsAtResponse.StockAsAt.ToString();
        //        await SetValue("ItemCode", transaction.SelectedLineItem.TransactionItem.ItemCode);
        //        RefreshComponent("ItemCode");
        //        RefreshComponent("ItemName");
        //        ToggleEditability("ItemCode", false);
        //    }
        //    ToggleEditability("LineEditCancel", true);


        //    UIStateChanged();
        //    await Task.CompletedTask;
        //}
        private async void OnItemEditClick(UIInterectionArgs<object> args)
        {
            if (args.DataObject != null)
            {
                TransactionLineItem item = args.DataObject as TransactionLineItem;

                item.IsInEditMode = true;
                transaction.EditingLineItem = item;
                this.transaction.SelectedLineItem.CopyFrom(item);
                StockAsAtResponse stockAsAtResponse = await GetStockResponseForCurrentItem();
                long currentUnitKey = item.TransactionUnit.UnitKey;


                await ReadData("LineTransactionUnit");
                await SetValue("LineTransactionUnit", currentUnitKey);

                BLUIElement textElement = GetLinkedUIElement("ItemCode");
                if (textElement != null)
                {
                    textElement.ElementCaption = "Item Code - AvlQty : " + stockAsAtResponse.StockAsAt.ToString();
                    await SetValue("ItemCode", transaction.SelectedLineItem.TransactionItem.ItemCode);
                    RefreshComponent("ItemCode");
                    RefreshComponent("ItemName");
                    ToggleEditability("ItemCode", false);
                }
                ToggleEditability("LineEditCancel", true);


                UIStateChanged();
            }
        }


        //private async void OnReplacementEditClick(TransactionLineItem item)
        //{

        //    item.IsInEditMode = true;
        //    transaction.EditingLineItem = item;
        //    this.transaction.SelectedLineItem.CopyFrom(item);
        //    this.transaction.SelectedLineItem.TransactionQuantity *= -1;
        //    this.transaction.SelectedLineItem.Quantity *= -1;
        //    this.transaction.SelectedLineItem.Quantity2 *= -1;

        //    StockAsAtResponse stockAsAtResponse = await GetStockResponseForCurrentItem();
        //    long currentUnitKey = item.TransactionUnit.UnitKey;


        //    await ReadData("LineTransactionUnit");
        //    await SetValue("LineTransactionUnit", currentUnitKey);

        //    BLUIElement textElement = GetLinkedUIElement("ItemCode");
        //    if (textElement != null)
        //    {
        //        textElement.ElementCaption = "Item Code - AvlQty : " + stockAsAtResponse.StockAsAt.ToString();
        //        await SetValue("ItemCode", transaction.SelectedLineItem.TransactionItem.ItemCode);
        //        RefreshComponent("ItemCode");
        //        RefreshComponent("ItemName");
        //        ToggleEditability("ItemCode", false);
        //    }
        //    ToggleEditability("LineEditCancel", true);


        //    UIStateChanged();
        //    await Task.CompletedTask;
        //}
        private async void OnReplacementEditClick(UIInterectionArgs<object> args)
        {
            if (args.DataObject != null)
            {
                TransactionLineItem item = args.DataObject as TransactionLineItem;

                item.IsInEditMode = true;
                transaction.EditingLineItem = item;
                this.transaction.SelectedLineItem.CopyFrom(item);
                this.transaction.SelectedLineItem.TransactionQuantity *= -1;
                this.transaction.SelectedLineItem.Quantity *= -1;
                this.transaction.SelectedLineItem.Quantity2 *= -1;

                StockAsAtResponse stockAsAtResponse = await GetStockResponseForCurrentItem();
                long currentUnitKey = item.TransactionUnit.UnitKey;


                await ReadData("LineTransactionUnit");
                await SetValue("LineTransactionUnit", currentUnitKey);

                BLUIElement textElement = GetLinkedUIElement("ItemCode");
                if (textElement != null)
                {
                    textElement.ElementCaption = "Item Code - AvlQty : " + stockAsAtResponse.StockAsAt.ToString();
                    await SetValue("ItemCode", transaction.SelectedLineItem.TransactionItem.ItemCode);
                    RefreshComponent("ItemCode");
                    RefreshComponent("ItemName");
                    ToggleEditability("ItemCode", false);
                }
                ToggleEditability("LineEditCancel", true);


                UIStateChanged();
            }
        }

        //private async void OnOrderItemDelete(TransactionLineItem item)
        //{

        //    bool? result = await _dialogService.ShowMessageBox(
        //        "Warning",
        //        $"Do you want to remove Item {item.TransactionItem.ItemName}",
        //        yesText: "Delete!", cancelText: "Cancel");

        //    if (result.HasValue && result.Value)
        //    {
        //        if (item.IsPersisted)
        //        {
        //            item.IsDirty = true;
        //            item.IsActive = 1;
        //        }
        //        else
        //        {
        //            this.transaction.InvoiceLineItems.Remove(item);

        //            for (int i = 0; i < transaction.InvoiceLineItems.Count(); i++)
        //            {
        //                transaction.InvoiceLineItems[i].LineNumber = i + 1;
        //            }

        //        }
        //        StateHasChanged();
        //    }

        //    if (transaction.InvoiceLineItems.Count() == 0)
        //    {
        //        this.ToggleEditability("Location", true);
        //    }


        //}
        private async void OnOrderItemDelete(UIInterectionArgs<object> args) 
        {
            if (args.DataObject != null)
            {
                TransactionLineItem item = args.DataObject as TransactionLineItem;

                bool? result = await _dialogService.ShowMessageBox(
                "Warning",
                $"Do you want to remove Item {item.TransactionItem.ItemName}",
                yesText: "Delete!", cancelText: "Cancel");

                if (result.HasValue && result.Value)
                {
                    if (item.IsPersisted)
                    {
                        item.IsDirty = true;
                        item.IsActive = 1;
                    }
                    else
                    {
                        this.transaction.InvoiceLineItems.Remove(item);

                        for (int i = 0; i < transaction.InvoiceLineItems.Count(); i++)
                        {
                            transaction.InvoiceLineItems[i].LineNumber = i + 1;
                        }

                    }

                    transaction.SubTotal = transaction.GetOrderTotalWithDiscounts();
                    SetInitialPaymentCashCardAmounts();
                    StateHasChanged();
                }

                if (transaction.InvoiceLineItems.Count() == 0)
                {
                    this.ToggleEditability("Location", true);
                }
            }
        }

        //private async void OnReplacemetItemDelete(TransactionLineItem item)
        //{

        //    bool? result = await _dialogService.ShowMessageBox(
        //        "Warning",
        //        $"Do you want to remove Item {item.TransactionItem.ItemName}",
        //        yesText: "Delete!", cancelText: "Cancel");

        //    if (result.HasValue && result.Value)
        //    {
        //        if (item.IsPersisted)
        //        {
        //            item.IsDirty = true;
        //            item.IsActive = 1;
        //        }
        //        else
        //        {
        //            this.ReplacementTransaction.InvoiceLineItems.Remove(item);

        //            for (int i = 0; i < ReplacementTransaction.InvoiceLineItems.Count(); i++)
        //            {
        //                ReplacementTransaction.InvoiceLineItems[i].LineNumber = i + 1;
        //            }

        //        }
        //        StateHasChanged();
        //    }

        //    if (transaction.InvoiceLineItems.Count() == 0)
        //    {
        //        this.ToggleEditability("Location", true);
        //    }


        //}
        private async void OnReplacemetItemDelete(UIInterectionArgs<object> args)
        {
            if (args.DataObject != null)
            {
                TransactionLineItem item = args.DataObject as TransactionLineItem;

                bool? result = await _dialogService.ShowMessageBox(
                "Warning",
                $"Do you want to remove Item {item.TransactionItem.ItemName}",
                yesText: "Delete!", cancelText: "Cancel");

                if (result.HasValue && result.Value)
                {
                    if (item.IsPersisted)
                    {
                        item.IsDirty = true;
                        item.IsActive = 1;
                    }
                    else
                    {
                        this.ReplacementTransaction.InvoiceLineItems.Remove(item);

                        for (int i = 0; i < ReplacementTransaction.InvoiceLineItems.Count(); i++)
                        {
                            ReplacementTransaction.InvoiceLineItems[i].LineNumber = i + 1;
                        }

                    }
                    StateHasChanged();
                }

                if (transaction.InvoiceLineItems.Count() == 0)
                {
                    this.ToggleEditability("Location", true);
                }
            }
         }
        private async void SetInitialPaymentCashCardAmounts()
        {
            if (transaction.Amount1==0 && transaction.Amount2 == 0 && transaction.Amount3 == 0)
            {
                transaction.Amount6 = 0;
            }
            else
            {
                if (transaction.SubTotal > transaction.Amount1)
                {
                    //if (transaction.Amount2 > transaction.SubTotal || transaction.Amount2 > transaction.Amount1 || transaction.Amount2 == 0)
                    //{
                        transaction.Amount2 = transaction.Amount1;
                    //}
                    transaction.Amount3 = transaction.SubTotal - transaction.Amount2;
                }
                else
                {
                    if (transaction.Amount2 > transaction.SubTotal || transaction.Amount2 > transaction.Amount1 || transaction.Amount2 == 0)
                    {
                        transaction.Amount2 = transaction.SubTotal;
                    }
                    transaction.Amount3 = transaction.SubTotal - transaction.Amount2;

                }
                transaction.CalculateCBalances();
            }
            await SetValue("Cash", transaction.Amount1);
            await SetValue("CashPaying", transaction.Amount2);
            await SetValue("CardPaying", transaction.Amount3);
            await SetValue("Balance", transaction.Amount6);
            StateHasChanged();
        }
        #endregion

        #region save invoice

        private async void OnInvoiceSaveClick(UIInterectionArgs<object> args)
        {
            await SaveInvoice();

        }

        private async Task SaveInvoice()
        {
            if (ReplacementMode)
            {
                await SaveReplacementInvoice();
            }
            else
            {
                if (this.transaction.InvoiceLineItems.Count() > 0)
                {
                    isSaving = true;
                    validator = new TransactionValidator(transaction);

                    if (validator.CanSaveTransaction())
                    {
                        transaction.IsDirty = true;
                        transaction.IsHold = false;
                        transaction.IsApproved = 1;
                        foreach (var line in transaction.InvoiceLineItems)
                        {
                            line.IsApproved = 1;
                        }
                        await _transactionManager.SaveTransaction(transaction);

                        _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                        _snackBar.Add("Invoice has been  Saved Successfully", Severity.Success);
                        await SetValue("HeaderTitle", transaction.TransactionNumber);

                        TransactionOpenRequest request = new TransactionOpenRequest();
                        request.TransactionKey = transaction.TransactionKey;
                        await LoadTransaction(request);
                        await DirectPrintInvoice();

                        //transaction = new BLTransaction();
                        //transaction.ElementKey = formDefinition.ElementKey;
                        //validator = new TransactionValidator(transaction);
                        ToggleEditability("LineEditCancel", false);
                        await SetValue("HeaderTitle", "New");
                    }
                    isSaving = false;

                    UIStateChanged();


                }
                else
                {
                    _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackBar.Add("Please Add Items to Invoice ", Severity.Error);
                }
            }

        }

        #endregion
        private async void LineTransactionUnit_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
        {
            args.DataObject.AddtionalData.Add("ItemKey", this.transaction.SelectedLineItem.TransactionItem.ItemKey);

            args.CancelChange = this.transaction.SelectedLineItem.TransactionItem.ItemKey < 11;
            await Task.CompletedTask;
        }

        #endregion

        #region Price List Related
        public async void OnBeforePriceListRequest(PriceListRequest request)
        {
            request.Code1Key = transaction.Code1.CodeKey;
            request.ElementKey = __priceListRequest.ElementKey;
            request.PreviousKey = transaction.Location.CodeKey;
            await Task.CompletedTask;
            UIStateChanged();
        }


        public void OnAfterPriceListRequest(IEnumerable<PriceListResponse> response)
        {

            UIStateChanged();
        }



        public void OnPriceListClose()
        {
            PriceListShown = false;
            UIStateChanged();

        }




        //public async void OnPriceListItemSelected(PriceListResponse response)
        //{
        //    await AddItemBasedOnPriceList(response);


        //}

        private async Task AddItemBasedOnPriceList(PriceListResponse response)
        {
            transaction.SelectedLineItem.TransactionItem.ItemKey = response.ItemKey;
            transaction.SelectedLineItem.TransactionItem.ItemName = response.ItemName;
            transaction.SelectedLineItem.TransactionItem.ItemCode = response.ItemCode;
            transaction.SelectedLineItem.TransactionProject = response.PriceListProject;
            __currentPriceListResponse = response;
            await PostItemAddActions();
        }

        public async Task SelectAccountByLocationAndPayementTerm()
        {
            if (transaction.Location.CodeKey > 10 && transaction.PaymentTerm.CodeKey > 10)
            {
                AccPaymentMappingRequest request = new AccPaymentMappingRequest();
                request.ELementKey = this.formDefinition.ElementKey;
                request.Location = transaction.Location;
                request.PayementTerm = transaction.PaymentTerm;
                IList<AccPaymentMappingResponse> responses = await _comboManager.GetPayementAccountMapping(request);
                if (responses.Count > 0)
                {
                    AccPaymentMappingResponse response = responses[0];
                    await SetValue("_Account_", response.Account.AccountKey);
                }
                else
                {
                    if (!transaction.IsPersisted)
                    {
                        await SetValue("_Account_", 1);
                    }
                }
            }
        }

        #endregion

        #region Popups

        private async void OnItemSelectClick(UIInterectionArgs<object> args) 
        {
            PriceListResponse item = new PriceListResponse();
            item = (PriceListResponse)args.DataObject;

            if (item.CanAddToTransaction())
            {
                await AddItemBasedOnPriceList(item);
            }
            else
            {
                showPriceListAlert = true;
                StateHasChanged();
            }
            _refPriceList.HidePopUp();
            UIStateChanged();
        }
        private void ShowCashIn()
        {
            HideAllPopups();
            CashInShown = true;
            _refCashInWindow.ShowPopUp();
            UIStateChanged();
        }


        private async void OpenDayEndReport(UIInterectionArgs<object> args)
        {
            HideAllPopups();
            if (transaction.Location != null && transaction.Location.CodeKey > 10)
            {

                object varLockKy;
                _dayEndReportOption.ReportName = "DayEndReportForAnyDate.trdp";
                if (_dayEndReportOption.ReportParameters.TryGetValue("LocKy", out varLockKy))
                {
                    _dayEndReportOption.ReportParameters["LocKy"] = transaction.Location.CodeKey;
                }
                else
                {
                    _dayEndReportOption.ReportParameters.Add("LocKy", transaction.Location.CodeKey);
                }
                DayEndReportShown = true;

            }
            else
            {
                bool? result = await _dialogService.ShowMessageBox(
                  "Warning",
                  $"Please Select an Location"
                );
                return;
            }

            UIStateChanged();
        }


        private async void ShowTelerikInvoice(UIInterectionArgs<object> args)
        {
            HideAllPopups();
            if (transaction.TransactionKey > 10)
            {
                object varLockKy;
                _dayEndReportOption.ReportParameters.Clear();
                _dayEndReportOption.ReportName = "Invoice_50mm.trdp";
                if (_dayEndReportOption.ReportParameters.TryGetValue("TrnKy", out varLockKy))
                {
                    _dayEndReportOption.ReportParameters["TrnKy"] = transaction.TransactionKey;
                }
                else
                {
                    _dayEndReportOption.ReportParameters.Add("TrnKy", transaction.TransactionKey);
                    _dayEndReportOption.ReportParameters.Add("Cky", 156);
                }
                DayEndReportShown = true;

            }
            else
            {
                bool? result = await _dialogService.ShowMessageBox(
                  "Warning",
                  $"Please Save the Transaction"
                );
                return;
            }

            UIStateChanged();
        }



        private void ShowCashDenominations()
        {
            HideAllPopups();
            CashDenominationShown = true;
            _refCashDenominatorWindow.ShowPopUp();
            UIStateChanged();
        }
        private void OnCloseDenomination() 
        {
            CashDenominationShown = false;
            UIStateChanged();
        }
        private void ShowCashOut()
        {
            HideAllPopups();
            CashOutShown = true;
            _refCashOutWindow.ShowPopUp();
            UIStateChanged();
        }
        private void OnCloseCashOut()
        {
            CashOutShown = false;
            UIStateChanged();
        }

        private async void OnRecieptsClick(object args)
        {
            if (this.transaction.TransactionKey < 11)
            {
                bool? result = await _dialogService.ShowMessageBox(
               "Warning",
               $"Please Select a Transaction"
             );
                return;
            }

            HideAllPopups();
            RecieptsWindowShown = true;
            await _refRecieptWindow.ReadRecieptsDetails();
            UIStateChanged();
        }
        private async void CancelCashIn(object args)
        {

            _refCashInWindow.Refresh();
            _refCashInWindow.Reset();
            _refCashInWindow.HidePopUp();
            HideAllPopups();
            UIStateChanged();
            await Task.CompletedTask;
        }
        private void OnCloseCashIn()
        {
            CashInShown = false;
            UIStateChanged();
        }
        private async Task ShowPriceList()
        {

            if (this.transaction.Location != null && this.transaction.Location.CodeKey > 10)
            {

                HideAllPopups();
                PriceListShown = true;
                _refPriceList.ShowPopUp();
                UIStateChanged();
                //await _refPriceList.FocusText();

                _refPriceList.InitilizeKeyBooadShortCuts();


            }

        }

        private async void SaveInCash(object args)
        {
            await _refCashInWindow.SaveCashInOut();
            HideAllPopups();
            UIStateChanged();
        }



        private void OnCashInLocationChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            _refCashInWindow.Refresh();
        }
        private void OnCashInAddressChange(UIInterectionArgs<AddressResponse> args)
        {
            _refCashInWindow.Refresh();
        }

        private void CancelCashOut(object args)
        {
            HideAllPopups();
            _refCashOutWindow.Refresh();
            _refCashOutWindow.Reset();
            _refCashOutWindow.HidePopUp();
            UIStateChanged();
        }

        private async void SaveCashOut(object args)
        {
            await _refCashOutWindow.SaveCashInOut();
            HideAllPopups();
            UIStateChanged();
        }

        private void OnNetRateChange(UIInterectionArgs<decimal> args)
        {
            if (args.DataObject < 0)
            {
                args.CancelChange = true;
                return;
            }
            if (this.transaction.SelectedLineItem.TransactionQuantity > 0)
            {

                if (this.transaction.SelectedLineItem.GetLineTotalWithoutDiscount() < args.DataObject)
                {
                    args.CancelChange = true;
                }
                else
                {
                    decimal Change = this.transaction.SelectedLineItem.GetLineTotalWithoutDiscount() - args.DataObject;
                    decimal QtyDisAmt = Change / this.transaction.SelectedLineItem.TransactionQuantity;
                    decimal QtyDisPer = (QtyDisAmt / this.transaction.SelectedLineItem.TransactionRate) * 100;
                    transaction.SelectedLineItem.TransactionDiscountAmount = QtyDisAmt;
                    transaction.SelectedLineItem.DiscountPercentage = Math.Round(QtyDisPer, 3);
                    RefreshComponent("DiscountPercentage");
                    transaction.SelectedLineItem.LineNetRate = this.transaction.SelectedLineItem.GetLineTotalWithDiscount();
                    UIStateChanged();
                }
            }
        }



        private void OnCashOutLocationChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            _refCashOutWindow.Refresh();
        }
        private void OnCashOutAddressChange(UIInterectionArgs<AddressResponse> args)
        {
            _refCashOutWindow.Refresh();
        }

        private async void OnSearchTracsactionClick(UIInterectionArgs<object> args)
        {
            findTrandsactionUI = args.InitiatorObject;
            await ShowFindTransactionWindow();
            UIStateChanged();
        }

        private async void OnSearchCashOutClick(UIInterectionArgs<object> args)
        {
            findTrandsactionUI = args.InitiatorObject;
            await ShowFindTransactionWindow();
            UIStateChanged();
        }


        private async void OnSearchCashInClick(UIInterectionArgs<object> args)
        {
            findTrandsactionUI = args.InitiatorObject;
            await ShowFindTransactionWindow();
            UIStateChanged();
        }


        private async Task ShowFindTransactionWindow()
        {
            HideAllPopups();
            FindTransactionShown = true;
            UIStateChanged();
            await Task.CompletedTask;
        }

        private async Task CloseFindTransactionWindow()
        {
            HideAllPopups();

            await Task.CompletedTask;

        }




        private void HideAllPopups()
        {
            PriceListShown = false;
            CashInShown = false;
            CashOutShown = false;
            FindTransactionShown = false;
            RecieptsWindowShown = false;
            CashDenominationShown = false;
            DayEndReportShown = false;
            UIStateChanged();

        }

        private async void OnOpenTransactionClick(TransactionOpenRequest request)
        {
            HideAllPopups();
            await LoadTransaction(request);

        }

        private async Task LoadTransaction(TransactionOpenRequest request)
        {
            DateTime dateTime1 = DateTime.Now;
            isSaving = true;
            request.ElementKey = transaction.ElementKey;
            BLTransaction otransaction = await _transactionManager.OpenTransaction(request);
            otransaction.ElementKey = transaction.ElementKey;
            transaction.CopyFrom(otransaction);
            string valueN = "";

            IList<KeyValuePair<string, IBLUIOperationHelper>> pairs = _objectHelpers.ToList();

            foreach (KeyValuePair<string, IBLUIOperationHelper> helper in pairs)
            {
                await helper.Value.Refresh();


            }

            transaction.SubTotal = transaction.GetOrderTotalWithDiscounts();
            transaction.CalculateCBalances();
            await SetValue("HeaderTitle", transaction.TransactionNumber);
            isSaving = false;
        }

        private async Task LoadReplacementTransaction(TransactionOpenRequest request)
        {
            DateTime dateTime1 = DateTime.Now;
            isSaving = true;
            BLTransaction otransaction = await _transactionManager.OpenTransaction(request);
            otransaction.ElementKey = ReplacementTransaction.ElementKey;
            ReplacementTransaction.CopyFrom(otransaction);
            isSaving = false;
        }

        #endregion

        #region Printing




        private async void PrintInvocieClick(UIInterectionArgs<object> args)
        {
            await DirectPrintInvoice();
        }




        private async void AddNewCustomer(UIInterectionArgs<object> args)
        {
            //    DialogOptions options = new DialogOptions() {};
            //    AddNewAddressDialog
            //    DialogService.Show<AddNewAddressDialog>();

            DialogOptions options = new DialogOptions();
            var dialog = _dialogService.Show<AddNewAddressDialog>("Add New Customer", options);
            var result = await dialog.Result;
        }

        private async Task DirectPrintInvoice()
        {
            if (_companyDetails == null)
            {
                _companyDetails = await _companyManager.GetCompanyDetailsResponse();
            }

            TransactionReportLocal reportLocal = new TransactionReportLocal();
            if (transaction.IsPersisted && !transaction.IsDirty)
            {
                reportLocal.TransactionKey = transaction.TransactionKey;
                reportLocal.TransactionDate = transaction.TransactionDate;
                reportLocal.CashRecvAmt1 = transaction.Amount1;
                reportLocal.CardAmt3 = transaction.Amount3;
                reportLocal.CashAmt2 = transaction.Amount2;
                reportLocal.BalanceAmt6 = transaction.Amount6;

                foreach (var item in transaction.InvoiceLineItems)
                {
                    TrasnsactionReportLineItem reportLine = new TrasnsactionReportLineItem();
                    reportLine.ItemCode = item.TransactionItem.ItemCode;
                    reportLine.ItemName = item.TransactionItem.ItemName;
                    reportLine.ItemKey = item.TransactionItem.ItemKey;
                    reportLine.Quantity = item.TransactionQuantity;
                    reportLine.TransactionRate = item.TransactionRate;
                    reportLine.LineDiscountAmount = item.GetLineDiscount();
                    reportLine.DiscountPercentage = item.DiscountPercentage;
                    reportLocal.LineItems.Add(reportLine);
                }
                reportLocal.CompanyName = _companyDetails.CompanyName;
                reportLocal.CompanyAddress = _companyDetails.Address;
                reportLocal.LocCity = _companyDetails.City;
                reportLocal.LocBusinessPhone = _companyDetails.TP1;
                reportLocal.LocBusinessEmail = _companyDetails.TP2;
                reportLocal.TrasnsactionNumber = transaction.TransactionNumber;
                reportLocal.Customer = transaction.Account.AccountName;
                reportLocal.TotalDiscount = transaction.GetOrderDiscountTotal();
                reportLocal.EntUsrId = "";
                reportLocal.TotalDiscount = transaction.GetOrderDiscountTotal();
                URLDefinitions definitions = new URLDefinitions();
                definitions.URL = InvoicePrinterURL.UrlAction;
                //await _printerManager.PrintTransactionBillLocalAsync(reportLocal, definitions);
                isSaving = false;
            }
        }

        private async Task DirectPrintReplacementInvoice()
        {
            TransactionReportLocal reportLocal = new TransactionReportLocal();
            if (_companyDetails == null)
            {
                _companyDetails = await _companyManager.GetCompanyDetailsResponse();
            }

            if (ReplacementTransaction.IsPersisted && !ReplacementTransaction.IsDirty)
            {
                reportLocal.TransactionKey = ReplacementTransaction.TransactionKey;
                reportLocal.TransactionDate = ReplacementTransaction.TransactionDate;
                reportLocal.CashRecvAmt1 = 0;
                reportLocal.CardAmt3 = 0;
                reportLocal.BalanceAmt6 = 0;

                foreach (var item in ReplacementTransaction.InvoiceLineItems)
                {
                    TrasnsactionReportLineItem reportLine = new TrasnsactionReportLineItem();
                    reportLine.ItemCode = item.TransactionItem.ItemCode;
                    reportLine.ItemName = item.TransactionItem.ItemName;
                    reportLine.ItemKey = item.TransactionItem.ItemKey;
                    reportLine.Quantity = item.TransactionQuantity;
                    reportLine.TransactionRate = item.TransactionRate;
                    reportLine.LineDiscountAmount = item.GetLineDiscount();
                    reportLocal.LineItems.Add(reportLine);
                }
                reportLocal.CompanyName = _companyDetails.CompanyName;
                reportLocal.CompanyAddress = _companyDetails.Address;
                reportLocal.LocCity = _companyDetails.City; ;
                reportLocal.LocBusinessPhone = _companyDetails.TP1; ;
                reportLocal.LocBusinessEmail = _companyDetails.TP2;
                reportLocal.TrasnsactionNumber = ReplacementTransaction.TransactionNumber;
                reportLocal.Customer = ReplacementTransaction.Account.AccountName;
                reportLocal.EntUsrId = "";
                reportLocal.TotalDiscount = ReplacementTransaction.GetOrderDiscountTotal();
                URLDefinitions definitions = new URLDefinitions();
                definitions.URL = InvoicePrinterURL.UrlAction;
                //await _printerManager.PrintTransactionBillLocalAsync(reportLocal, definitions);

            }
        }


        #endregion


        #region CashRelated / Replacements / Hold

        //private async void OnCashAmountChange(decimal inputAmount)
        //{
        //    transaction.Amount1 = inputAmount;
        //    decimal transactionTotal = transaction.GetOrderTotalWithDiscounts();
        //    ///decimal totalCashPayement = transaction.Amount1 - transaction.Amount3;
        //    transaction.Amount2 = Math.Min(inputAmount, transactionTotal);
        //    if (transactionTotal > inputAmount)
        //    {
        //        transaction.Amount3 = transactionTotal - transaction.Amount2;
        //    }

        //    transaction.CalculateCBalances();

        //    await Task.CompletedTask;
        //}
        private async void OnCashAmountChange(UIInterectionArgs<decimal> args) 
        {
            transaction.Amount1 = args.DataObject;
            decimal transactionTotal = transaction.GetOrderTotalWithDiscounts();
            transaction.Amount2 = Math.Min(args.DataObject, transactionTotal);
            if (transactionTotal > args.DataObject)
            {
                transaction.Amount3 = transactionTotal - transaction.Amount2;
            }

            transaction.CalculateCBalances();



            StateHasChanged();
        }

        //private async void OnCashPaymentChange(decimal Amount)
        //{
        //    decimal transactionTotal = transaction.GetOrderTotalWithDiscounts();

        //    if (Amount > transaction.Amount1 || Amount > transactionTotal)
        //    {
        //        Amount = Math.Min(transaction.Amount1, transactionTotal);
        //    }
        //    transaction.Amount2 = Amount;

        //    if (transactionTotal > Amount)
        //    {
        //        transaction.Amount3 = transactionTotal - transaction.Amount2;
        //    }
        //    transaction.CalculateCBalances();
        //    await Task.CompletedTask;
        //}
        private async void OnCashPaymentChange(UIInterectionArgs<decimal> args) 
        {
            decimal transactionTotal = transaction.GetOrderTotalWithDiscounts();

            if (args.DataObject > transaction.Amount1 || args.DataObject > transactionTotal)
            {
                args.DataObject = Math.Min(transaction.Amount1, transactionTotal);
            }
            transaction.Amount2 = args.DataObject;

            if (transactionTotal > args.DataObject)
            {
                transaction.Amount3 = transactionTotal - transaction.Amount2;
            }
            transaction.CalculateCBalances();

            StateHasChanged();
        }
        //private async void OnCardPaymentChange(decimal Amount)
        //{
        //    transaction.Amount3 = Amount;

        //    transaction.CalculateCBalances();
        //    await Task.CompletedTask;
        //}
        private async void OnCardPaymentChange(UIInterectionArgs<decimal> args) 
        {
            transaction.Amount3 = args.DataObject;

            transaction.CalculateCBalances();
            StateHasChanged();
        }

        private async void OnItemReplacementClick(UIInterectionArgs<object> args)
        {
            if (this.transaction.TransactionKey < 11)
            {
                bool? result = await _dialogService.ShowMessageBox(
               "Warning",
               $"Please Select Open a Transaction"
             );
                return;
            }

            ReplacementMode = !ReplacementMode;
            ReplacementTransaction = new BLTransaction();
            if (ReplacementMode)
            {

                ReplacementTransaction.CopyFrom(transaction);
                ReplacementTransaction.InvoiceLineItems = new List<TransactionLineItem>();
                ReplacementTransaction.TransactionNumber = "NEW";
                ReplacementTransaction.IsDirty = false;
                ReplacementTransaction.IsPersisted = false;
                ReplacementTransaction.TransactionKey = 1;
            }
            else
            {

            }


            UIStateChanged();

        }

        private async void AddToReplacement(TransactionLineItem item)
        {

            if (ReplacementTransaction != null)
            {
                var newLineItem = new TransactionLineItem();
                newLineItem.CopyFrom(item);
                newLineItem.Quantity *= -1;
                newLineItem.TransactionQuantity *= -1;
                newLineItem.Quantity2 *= -1;
                newLineItem.IsPersisted = false;
                ReplacementTransaction.InvoiceLineItems.Add(newLineItem);
                UIStateChanged();
            }

            await Task.CompletedTask;
        }

        private async Task SaveReplacementInvoice()
        {

            if (this.ReplacementTransaction.GetOrderTotalWithDiscounts() < 0)
            {

                bool? result = await _dialogService.ShowMessageBox(
               "Warning",
               $"Please Add Replacement Items. Cannot Proceed with Minus values");
                return;
            }

            if (this.ReplacementTransaction.InvoiceLineItems.Count() > 0)
            {
                isSaving = true;
                UIStateChanged();
                ReplacementTransaction.TransactionDate = DateTime.Now;
                await _transactionManager.SaveTransaction(ReplacementTransaction);
                isSaving = false;
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Replacement Invoice has been  Saved Successfully", Severity.Success);


                TransactionOpenRequest request = new TransactionOpenRequest();
                request.TransactionKey = ReplacementTransaction.TransactionKey;
                await LoadReplacementTransaction(request);
                await DirectPrintReplacementInvoice();
                UIStateChanged();

            }
            else
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Please Add Items to Invoice ", Severity.Error);
            }
        }


        private async void OnHoldTransactionClick(UIInterectionArgs<object> args)
        {
            await HoldTransaction(args);
            await Task.CompletedTask;
        }

        private async Task HoldTransaction(UIInterectionArgs<object> args)
        {
            isSaving = true;

            bool? result = await _dialogService.ShowMessageBox(
                "Warning",
                $"Do you want to Hold The Invoiceand start a new one",
                yesText: "Hold!", cancelText: "Cancel");
            if (result.HasValue && result.Value)
            {

                var AprVal = Convert.ToByte(args.InitiatorObject.DefaultValue);
                transaction.IsHold = true;
                transaction.IsApproved = AprVal;
                foreach (var item in transaction.InvoiceLineItems)
                {
                    item.IsApproved = AprVal;
                }

                await _transactionManager.SaveTransaction(transaction);
                InitilizeNewOrder();
            }

            isSaving = false;
        }

        private async void FocusCashAmount()
        {
            if (_refCashAmount != null)
            {
                await _refCashAmount.FocusAsync();
                await _refCashAmount.SelectAsync();
            }
        }


        private async void FocusLocation()
        {
            await Focus("Location");
        }

        private async void FocusDate()
        {
            await Focus("TransactionDate");
        }
        private async void FocusDocNo()
        {
            await Focus("DocNo");
        }

        private async void FocusPriceList()
        {
            await Focus("PriceList");
        }

        private async void FocusPayementTerm()
        {
            await Focus("PaymentTerm");
        }

        #endregion

        #region UI Object Helpers 

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


        private void RefreshComponent(string name)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                helper.Refresh();
                UIStateChanged();
            }
        }


        private async Task SetDataSource(string name, object dataSource)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                await (helper as IBLServerDependentComponent).SetDataSource(dataSource);
                UIStateChanged();
            }
        }

        private async Task ReadData(string name, bool UseLocalStorage = false)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                await (helper as IBLServerDependentComponent).FetchData(UseLocalStorage);

                UIStateChanged();
            }
        }
        private async Task Focus(string name)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                await helper.FocusComponentAsync();
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

        private BLUIElement GetLinkedUIElement(string name)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                return helper.LinkedUIObject;
            }
            return null;
        }

        #endregion
    }
}
