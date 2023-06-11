using bluelotus360.com.razorComponents.Extensions;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using bluelotus360.com.razorComponents.MB.Shared.Components.Popups.MasterDetailPopup;
using bluelotus360.Com.commonLib.Helpers;
using BlueLotus360.CleanArchitecture.Application.Validators.SalesOrder;
using BlueLotus360.CleanArchitecture.Domain.DTO.MasterData;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.com.razorComponents.Pages.OrderPages.ComPonent
{
    public partial class OrderComponent
    {
        [Parameter] public BLUIElement FormDefinition { get; set; }
        [Parameter] public long ElementKey { get; set; }
        [Parameter] public string AppbarName { get; set; }
        [Parameter] public IOrderValidator Validator { get; set; }
        //[Parameter] public TerlrikReportOptions SalesOrderReportOption { get; set; }
        [Parameter] public bool IsQuotation { get; set; }


        private Order order;
        private BLUIElement findOrderUI;
        private BLUIElement getFromQuoteUI;
        //private BLUIElement modalUIElement;
        private BLUIElement editUIElement;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
        private bool tableloading = false;
        //private ReportCompanyDetailsResponse reportCompanyDetailsResponse;
        MudMessageBox addItem { get; set; }

        private CodeBaseResponse ParentLocation;
        private int CodeKey;
        private bool isItemPopupShown = false;
        private bool isEditPopupShown = false;
        private bool isClickSaveButton;
        private IOrderValidator validator;
        private bool FindOrderShown = false;
        private bool FindGetFromQuoteShown = false;
        private AddNewAddress _refNewAddressCreation = new AddNewAddress();
        //private BLTelAddNewAddress _refBLTelNewAddressCreation;
        //private TerlrikReportOptions _salesOrderReportOption;
        private bool ReportShown = false;
        CompletedUserAuth auth;
        private bool IsLessQuantityShown = false;
        private string deviceType;
        //private BLTelGrid<OrderItem> _blTb;
        BLUIElement salesOrderGrid;
        private bool orderHeaderValidationShown;
        private bool IsAlwAdd, IsAlwUpdate;
        private string permissionMsg;
        private UserMessageDialog _refUserMessage = new UserMessageDialog();
        private CodeBaseResponse TemporyLineLocation = new CodeBaseResponse();
        private BLTable<OrderItem> _mudGrid = new BLTable<OrderItem>();
        protected override async Task OnInitializedAsync()
        {
            order = new();
            validator = new SalesOrderValidator(order);

            if (FormDefinition != null)
            {
                FormDefinition.IsDebugMode = true;
                FormDefinition.IsMustElements = FormDefinition.Children.Where(x => x.IsMust).Select(i => i._internalElementName).ToList();
                findOrderUI = FormDefinition.Children.Where(x => x._internalElementName.Equals("_SearchSalesOrder_")).FirstOrDefault();
                getFromQuoteUI = FormDefinition.Children.Where(x => x._internalElementName.Equals("_GetFromQuotation_")).FirstOrDefault();
                salesOrderGrid = FormDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("SalesOrderGrid")).FirstOrDefault();

                //editUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("OrderItemPopUp")).FirstOrDefault();
            }
            if (salesOrderGrid != null)
            {
                salesOrderGrid.Children = FormDefinition.Children.Where(x => x.ParentKey == salesOrderGrid.ElementKey).ToList();
            }

            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();

            await CheckDeviceType();
            HookInteractions();
            InitilizeNewOrder();

            auth = await _authenticationManager.GetUserInformation();
            //_salesOrderReportOption = new TerlrikReportOptions();
            //_salesOrderReportOption.ReportName = SalesOrderReportOption.ReportName;
            //_salesOrderReportOption.ReportParameters = new Dictionary<string, object>();
        }

        private async void InitilizeNewOrder()
        {
            order = new();
            order.FormObjectKey = ElementKey;
            //_salesOrderReportOption = new TerlrikReportOptions();
            ParentLocation = new();
            editUIElement = new();
            //_blTb = new();
            this.ToggleEditability("Location", true);
            await SetValue("HeaderTitle", "New");

            OrderTranApprovestatus permission = await _orderManager.CheckAddeditPermissionForAddEditOrdTrn(order);

            IsAlwAdd = Convert.ToBoolean(permission.IsAlwAdd);
            IsAlwUpdate = Convert.ToBoolean(permission.isAlwUpdate);
            permissionMsg = permission.Message;

            this.UIStateChanged();
        }

        private void HookInteractions()
        {
            InteractionHelper helper = new InteractionHelper(this, FormDefinition);
            _interactionLogic = helper.GenerateEventCallbacks();
            appStateService._AppBarName = AppbarName;

        }

        private async Task CheckDeviceType()
        {
           // deviceType = await _jsRuntime.InvokeAsync<string>("getDeviceType", null);

        }
        private void UIStateChanged()
        {
            this.StateHasChanged();
        }

        #region UI Interaction Logics

        private async void OnOrderNewClick(UIInterectionArgs<object> args)
        {
            InitilizeNewOrder();

            await ReadData("HeaderSection1_Location");
            await ReadData("HeaderSection1_AddressCombo");
            await ReadData("HeaderSection1_RepCombo");
            UIStateChanged();
        }
        private async void OnOrderSaveClick(UIInterectionArgs<object> args)
        {
            bool needToRequest = false;

            validator = new SalesOrderValidator(order);

            if (validator != null && validator.CanOrderSave())
            {
                if (order.OrderKey == 1)
                {
                    if (IsAlwAdd)
                    {
                        if (order.IsFromQuotation)
                        {

                            UIStateChanged();

                            foreach (var orderItm in order.OrderItems)
                            {
                                if (orderItm.TransactionQuantity > orderItm.AvailableStock + orderItm.RequestedQuantity)
                                {
                                    orderItm.TransactionQuantity = orderItm.AvailableStock;
                                    needToRequest = true;
                                }
                            }

                            await _orderManager.SaveOrder(order);

                            _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                            _snackBar.Add("Sales Order has been  Saved Successfully", Severity.Success);
                            //ReportCompanyDetailsRequest request = new ReportCompanyDetailsRequest();
                            //request.BussinessUnit = new CleanArchitecture.Domain.DTO.Object.CodeBaseResponse();
                            //request.TransactionKey = 1;
                            //request.Location = order.OrderLocation;
                            //request.OrderKey = order.OrderKey;
                            //request.EmployeeKey = 1;
                            //reportCompanyDetailsResponse = await _reportManager.GetReportCompanyInformation(request);
                            //isClickSaveButton = true;

                            OrderOpenRequest oprequest = new OrderOpenRequest();
                            oprequest.OrderKey = order.OrderKey;
                            LoadOrder(oprequest);

                            if (needToRequest)
                            {
                                IsLessQuantityShown = true;
                            }

                            UIStateChanged();
                        }
                        else
                        {

                            UIStateChanged();
                            await _orderManager.SaveOrder(order);

                            _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                            _snackBar.Add("Sales Order has been  Saved Successfully", Severity.Success);
                            //ReportCompanyDetailsRequest request = new ReportCompanyDetailsRequest();
                            //request.BussinessUnit = new CleanArchitecture.Domain.DTO.Object.CodeBaseResponse();
                            //request.TransactionKey = 1;
                            //request.Location = order.OrderLocation;
                            //request.OrderKey = order.OrderKey;
                            //request.EmployeeKey = 1;
                            //reportCompanyDetailsResponse = await _reportManager.GetReportCompanyInformation(request);
                            //isClickSaveButton = true;

                            OrderOpenRequest oprequest = new OrderOpenRequest();
                            oprequest.OrderKey = order.OrderKey;
                            LoadOrder(oprequest);
                        }
                    }
                    else
                    {
                        //CantInsertPopUpShown = true;
                        validator = new SalesOrderValidator(new Order());
                        validator.UserMessages.AddErrorMessage(permissionMsg);
                        _refUserMessage.ShowUserMessageWindow();

                    }

                    UIStateChanged();
                }
                else
                {
                    OrderTranApprovestatus permission = await _orderManager.CheckAddeditPermissionForAddEditOrdTrn(order);
                    IsAlwUpdate = Convert.ToBoolean(permission.isAlwUpdate);

                    if (IsAlwUpdate)
                    {
                        UIStateChanged();

                        await _orderManager.EditOrder(order);

                        _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                        _snackBar.Add("Sales Order has been  Updated Successfully", Severity.Success);
                        //ReportCompanyDetailsRequest request = new ReportCompanyDetailsRequest();
                        //request.BussinessUnit = new CleanArchitecture.Domain.DTO.Object.CodeBaseResponse();
                        //request.TransactionKey = 1;
                        //request.Location = order.OrderLocation;
                        //request.OrderKey = order.OrderKey;
                        //request.EmployeeKey = 1;
                        //reportCompanyDetailsResponse = await _reportManager.GetReportCompanyInformation(request);
                        //isClickSaveButton = true;

                        OrderOpenRequest oprequest = new OrderOpenRequest();
                        oprequest.OrderKey = order.OrderKey;
                        LoadOrder(oprequest);
                    }
                    else
                    {
                        validator = new SalesOrderValidator(new Order());
                        validator.UserMessages.AddErrorMessage(permission.Message);
                        _refUserMessage.ShowUserMessageWindow();

                    }

                    UIStateChanged();
                }


            }
            else
            {
                //_snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                //_snackBar.Add("Please Add Items to Sales Order", Severity.Error);
                orderHeaderValidationShown = true;
                UIStateChanged();
            }


        }
        private async void onScan(UIInterectionArgs<object> args)
        {
            //if (ParentLocation != null && ParentLocation.CodeKey > 1)
            //{
            //    var parameters = new DialogParameters
            //    {

            //    };
            //    DialogOptions options = new DialogOptions();
            //    var dialog = _dialogService.Show<QrDialog>("QR Scanner", parameters, options);
            //    DialogResult dialogResult = await dialog.Result;
            //    await InvokeAsync(StateHasChanged);
            //    if (!dialogResult.Cancelled)
            //    {
            //        if (dialogResult.Data != null)
            //        {
            //            string itemCode = dialogResult.Data as string;
            //            ItemRequestModel requestModel = new ItemRequestModel(); ;
            //            requestModel.ItemCode = itemCode;
            //            requestModel.LocationKey = order.OrderLocation.CodeKey;
            //            requestModel.AddressKey = order.OrderCustomer.AddressKey;
            //            IList<ItemCodeResponse> items = await _comboManager.GetItemByItemCode(requestModel);
            //            if (items.Count > 0)
            //            {
            //                ItemResponse response = new ItemResponse();
            //                response.ItemName = items[0].ItemCodeName;
            //                response.ItemKey = items[0].ItemKey;
            //                //await ShowAddNewItem(response);
            //            }
            //            else
            //            {
            //                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
            //                _snackBar.Add("Invalid Item Code", Severity.Error);
            //            }
            //        }

            //    }

            //}
            //else
            //{
            //    _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
            //    _snackBar.Add("Please enter a location before scaning", Severity.Error);
            //}
            //await InvokeAsync(StateHasChanged);

        }
        private async void ShowAddNewCustomer(UIInterectionArgs<object> args)
        {
            HideAllPopups();
            IDictionary<string, object> ParamDictionary = new Dictionary<string, object>();
            ParamDictionary.Add("InitiatorElement", args.InitiatorObject);
            ParameterView values = ParameterView.FromDictionary(ParamDictionary);
            await _refNewAddressCreation.SetParametersAsync(values);
            _refNewAddressCreation.ShowPopUp();
            //await _refBLTelNewAddressCreation.SetParametersAsync(values);
            //_refBLTelNewAddressCreation.ShowPopUp();

        }
        private void OnOrderPrintClick(UIInterectionArgs<object> args)
        {
            //if (order.OrderKey > 1)
            //{
            //    if (_salesOrderReportOption != null && _salesOrderReportOption.ReportParameters != null)
            //    {
            //        _salesOrderReportOption.ReportParameters.Clear();
            //        _salesOrderReportOption.ReportName = SalesOrderReportOption.ReportName;
            //        _salesOrderReportOption.ReportParameters.Add("Cky", auth.AuthenticatedCompany.CompanyKey);
            //        _salesOrderReportOption.ReportParameters.Add("UsrKy", auth.AuthenticatedUser.UserKey);
            //        _salesOrderReportOption.ReportParameters.Add("UsrId", auth.AuthenticatedUser.UserID);
            //        _salesOrderReportOption.ReportParameters.Add("OrdKy", order.OrderKey);

            //        ReportShown = true;
            //    }

            //}
            //else
            //{
            //    _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
            //    _snackBar.Add("Invalid Request. Please select a record.", Severity.Error);
            //}


            StateHasChanged();
        }
        private async void OnOrderFindClick(UIInterectionArgs<object> args)
        {
            findOrderUI = args.InitiatorObject;

            await ShowFindOrderWindow();
        }
        private async void OnGetFromQuotationClick(UIInterectionArgs<object> args)
        {
            getFromQuoteUI = args.InitiatorObject;

            await ShowGetFromQuoteWindow();
        }
        private async void OnCancelPopupClick(UIInterectionArgs<object> args)
        {
            isItemPopupShown = false;
            isEditPopupShown = false;
            if (validator != null && validator.UserMessages != null)
            {
                validator.UserMessages.UserMessages.Clear();
            }
            UIStateChanged();
            await Task.CompletedTask;
        }

        private void OnHeadersection1DateClick(UIInterectionArgs<DateTime?> args)
        {

        }
        private void OnHeadersection1DeliDateClick(UIInterectionArgs<DateTime?> args)
        {

        }
        private void OnOrderLocationChanged(UIInterectionArgs<CodeBaseResponse> args)
        {
            this.order.OrderLocation = args.DataObject;
            ParentLocation = this.order.OrderLocation;
            CodeKey = args.DataObject.CodeKey;
            UIStateChanged();
        }
        private void OnPayementTermChanged(UIInterectionArgs<CodeBaseResponse> args)
        {
            order.OrderPaymentTerm = args.DataObject;
            UIStateChanged();
        }
        private void OnOrderCustomerChanged(UIInterectionArgs<AddressResponse> args)
        {
            order.OrderCustomer = args.DataObject;
            UIStateChanged();
        }
        private void AddressCombo_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
        {
            args.DataObject.AddtionalData.Add("AdrCat3Ky", order.AddressCategory3.CodeKey);
        }
        private async void OnRouteComboChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            order.AddressCategory3 = args.DataObject;
            await ReadData("HeaderSection1_AddressCombo");
            UIStateChanged();
        }
        private async void OnHeaderLevelDiscountClick(UIInterectionArgs<decimal> args)
        {
            if (order.OrderItems.Count() > 0)
            {
                foreach (var itm in order.OrderItems)
                {
                    if (!itm.IsLineDiscountChanged)
                    {
                        ItemRateResponse rates = await RetriveRate(itm.TransactionItem);
                        itm.DiscountPercentage = Math.Max(rates.DiscountPercentage, args.DataObject);
                        UIStateChanged();
                    }
                }
            }

            order.HeaderLevelDisountPrecentage = args.DataObject;

            UIStateChanged();
        }
        private void OnOrderRepChanged(UIInterectionArgs<AddressResponse> args)
        {
            //order.OrderCustomer = args.DataObject;
            //UIStateChanged();

        }
        private void OnHeadersection1PriceListChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            UIStateChanged();
        }
        private void OnHeadersection1TextNoChange(UIInterectionArgs<string> args)
        {
            UIStateChanged();
        }

        private async void OnTransactionItemChange(UIInterectionArgs<ItemResponse> args)
        {
            order.SelectedOrderItem.TransactionItem = args.DataObject;
            args.CancelChange = true;
            validator = new OrderValidatorV2(FormDefinition, order);

            if (BaseResponse.IsValidData(order.SelectedOrderItem.TransactionItem))
            {
                if (validator != null && validator.CanSelectItem())
                {
                    await DetailSectionChange(args.DataObject);
                }
                else
                {
                    orderHeaderValidationShown = true;
                    UIStateChanged();
                }

            }

            UIStateChanged();

        }
        private void OnLineTransactionUnitChange(UIInterectionArgs<UnitResponse> args)
        {

        }
        private void HeaderSection2_TrnUnit_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
        {
            args.DataObject.AddtionalData.Add("ItemKey", this.order.SelectedOrderItem.TransactionItem.ItemKey);
        }
        private void OnLineTransactionQuantityChanged(UIInterectionArgs<decimal> args)
        {
            order.SelectedOrderItem.TransactionQuantity = args.DataObject;
            order.SelectedOrderItem.LineTotal = order.SelectedOrderItem.GetLineTotalWithDiscount();

            ToggleViisbility("TotalRate", true);

            if (order.SelectedOrderItem.NeedToRequestFromAnotherLocation())
            {
                order.SelectedOrderItem.RequestedQuantity = order.SelectedOrderItem.TransactionQuantity - Math.Max(order.SelectedOrderItem.AvailableStock, 0);
                ToggleViisbility("RequiredQuantity", true);
                ToggleViisbility("LineLevelLocation", true);
                ToggleViisbility("IsTransfer", true);
                ToggleViisbility("IsConfirmed", true);

            }
            else
            {
                order.SelectedOrderItem.RequestedQuantity = 0;
                //ToggleViisbility("RequiredQuantity", false);
                //ToggleViisbility("LineLevelLocation", false);
                //ToggleViisbility("IsTransfer", false);
                //ToggleViisbility("IsConfirmed", false);



            }





        }
        private void OnLineDisPerChange(UIInterectionArgs<decimal> args)
        {
            order.SelectedOrderItem.LineTotal = order.SelectedOrderItem.GetLineTotalWithDiscount();
            order.SelectedOrderItem.DiscountPercentage = args.DataObject;
            order.SelectedOrderItem.IsLineDiscountChanged = true;
            ToggleViisbility("TotalRate", true);
        }
        private void OnRateChange(UIInterectionArgs<decimal> args)
        {
            order.SelectedOrderItem.LineTotal = order.SelectedOrderItem.GetLineTotalWithDiscount();

            ToggleViisbility("TotalRate", true);
        }
        private void OnIsTransferClick(UIInterectionArgs<int> args)
        {
            this.order.SelectedOrderItem.IsTransfer = args.DataObject;
            UIStateChanged();
        }
        private void OnIsConfirmedClick(UIInterectionArgs<int> args)
        {
            this.order.SelectedOrderItem.IsTransferConfirmed = args.DataObject;
            UIStateChanged();
        }
        private void TotalRateChange(UIInterectionArgs<decimal> args)
        {

        }
        private void RequiredQuantityChanged(UIInterectionArgs<decimal> args)
        {
            this.order.SelectedOrderItem.RequestedQuantity = args.DataObject;
        }
        private void OnLineLevelLocationChanged(UIInterectionArgs<CodeBaseResponse> args)
        {
            order.SelectedOrderItem.OrderLineLocation = args.DataObject;
            UIStateChanged();
        }

        private async void OnAddItemButtonClick(UIInterectionArgs<object> args)
        {
            validator = new SalesOrderValidator(order);
            if (validator != null && validator.CanAddItemToGrid())
            {
                this.order.SelectedOrderItem.GetLineTotalWithTax();

                if (this.order.SelectedOrderItem.IsInEditMode)
                {
                    order.EditingLineItem.CopyFrom(order.SelectedOrderItem);
                    order.SelectedOrderItem = new();
                }
                else
                {
                    this.order.SelectedOrderItem.LineNumber = this.order.OrderItems.Count() + 1;
                    this.order.OrderDate = DateTime.Now;
                    if (this.order.SelectedOrderItem.OrderLineLocation.CodeKey == 1)
                    {
                        this.order.SelectedOrderItem.OrderLineLocation = this.order.OrderLocation;
                    }
                    order.AddGridItems(this.order.SelectedOrderItem);
                }

                //_objectHelpers["HeaderSection2_TransactionItem"].ResetToInitialValue();

                //if (_blTb != null)
                //{
                //    _blTb.Refresh();
                //}

                if (order.OrderItems.Count() > 0)
                {
                    this.ToggleEditability("Location", false);
                }
            }
            else
            {
                orderHeaderValidationShown = true;
            }

            UIStateChanged();
            await Task.CompletedTask;

        }
        #endregion

        #region popup
        private async Task ShowFindOrderWindow()
        {
            HideAllPopups();
            FindOrderShown = true;
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async Task ShowGetFromQuoteWindow()
        {
            HideAllPopups();
            FindGetFromQuoteShown = true;
            StateHasChanged();
            await Task.CompletedTask;
        }

        private async Task ShowEditItem(OrderItem Item)
        {

            if (Item != null)
            {
                Item.IsInEditMode = true;
                order.EditingLineItem = Item;
                order.SelectedOrderItem = new();
                this.order.SelectedOrderItem.CopyFrom(Item);


                if (order != null && order.SelectedOrderItem != null)
                {
                    StockAsAtRequest stock_request = new StockAsAtRequest();
                    StockAsAtResponse response = new StockAsAtResponse();

                    stock_request.ElementKey = order.FormObjectKey;
                    stock_request.LocationKey = order.OrderLocation.CodeKey;
                    stock_request.ItemKey = order.SelectedOrderItem.TransactionItem.ItemKey;

                    if (stock_request != null)
                        response = await _orderManager.GetAvailableStock(stock_request);

                    if (response != null)
                        order.SelectedOrderItem.AvailableStock = Math.Max(response.StockAsAt, 0);

                    if (!_modalDefinitions.TryGetValue("OrderItemPopUps", out editUIElement))
                    {
                        editUIElement = FormDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("OrderItemPopUp")).FirstOrDefault();

                        if (editUIElement != null && editUIElement.Children.Count() > 0)
                        {
                            _modalDefinitions.Add("OrderItemPopUps", editUIElement);
                        }
                    }


                    isEditPopupShown = true;
                    UIStateChanged();

                    await SetValue("HeaderSection2_AvlQty", order.SelectedOrderItem.AvailableStock);
                    await SetValue("HeaderSection2_RequiredQuantity", order.SelectedOrderItem.RequestedQuantity);
                    await SetValue("HeaderSection2_LineLevelLocation", order.SelectedOrderItem.OrderLineLocation);
                    await SetValue("HeaderSection2_ItemCode", order.SelectedOrderItem.TransactionItem.ItemCode);
                    TemporyLineLocation = order.SelectedOrderItem.OrderLineLocation;
                    UIStateChanged();
                }
            }

        }

        private async void HideAllPopups()
        {
            FindOrderShown = false;
            isItemPopupShown = false;
            FindGetFromQuoteShown = false;
            orderHeaderValidationShown = false;
            StateHasChanged();
            await Task.CompletedTask;
        }
        #endregion

        #region load
        private async void LoadOrder(OrderOpenRequest request)
        {
            HideAllPopups();

            this.appStateService.IsLoaded = true;

            Order loaded_order = await _orderManager.OpenOrder(request);
            loaded_order.FormObjectKey = order.FormObjectKey;
            order.CopyFrom(loaded_order);


            await SetValue("HeaderTitle", order.OrderNumber);
            validator = new SalesOrderValidator(order);

            if (!validator.CanChangeHeaderInformatiom())
            {
                this.ToggleEditability("Location", false);
            }

            this.appStateService.IsLoaded = false;

            UIStateChanged();
        }

        private async void LoadOrderFromQuotation(OrderOpenRequest request)
        {
            HideAllPopups();
            this.appStateService.IsLoaded = true;

            Order loaded_order = await _orderManager.OpenQuotation(request);
            loaded_order.FormObjectKey = order.FormObjectKey;
            order.CopyFrom(loaded_order);

            foreach (var itm in order.OrderItems)
            {
                StockAsAtRequest stock_request = new StockAsAtRequest();
                stock_request.ElementKey = order.FormObjectKey;
                stock_request.LocationKey = order.OrderLocation.CodeKey;
                stock_request.ItemKey = itm.TransactionItem.ItemKey;
                StockAsAtResponse response = await _orderManager.GetAvailableStock(stock_request);
                itm.AvailableStock = Math.Max(response.StockAsAt, 0);
            }



            await SetValue("HeaderTitle", order.OrderNumber);
            this.appStateService.IsLoaded = false;
            UIStateChanged();
        }
        #endregion

        #region additional
        private void Done()
        {
            InitilizeNewOrder();
            isClickSaveButton = false;
        }
        private async Task Print()
        {
        //    var parameters = new DialogParameters
        //    {


        //    };

        //    DialogOptions options = new DialogOptions();
        //    options.FullScreen = true;
        //    var dialog = _dialogService.Show<PrintDialog>("Print", parameters, options);
        //    await Task.CompletedTask;

        }
        private void LineItemEdit()
        {

            if (order.SelectedOrderItem.IsInEditMode)
            {
                if (order.EditingLineItem != null)
                {
                    order.SelectedOrderItem.IsInEditMode = false;
                    if (order.SelectedOrderItem.OrderLineLocation.CodeKey == 1)
                    {
                        order.SelectedOrderItem.OrderLineLocation = TemporyLineLocation;
                    }
                    order.EditingLineItem.CopyFrom(order.SelectedOrderItem);

                }
                isEditPopupShown = false;
                UIStateChanged();
            }
        }
        void CloseDialogForLessQuantity() => IsLessQuantityShown = false;
        public async Task<ItemRateResponse> RetriveRate(ItemResponse transactionItem)
        {

            ItemRateRequest request = new ItemRateRequest();
            request.LocationKey = order.OrderLocation.CodeKey;
            request.ItemKey = transactionItem.ItemKey;
            request.EffectiveDate = DateTime.Now.Date;
            request.ConditionCode = "OrdTyp";
            request.ObjectKey = order.FormObjectKey;
            return (await _comboManager.GetRate(request));
        }
        #endregion

        #region customer creation 
        private async Task OnCustomerCreateSuccess(AddressMaster address)
        {
            await ReadData("HeaderSection1_AddressCombo");
            await SetValue("HeaderSection1_AddressCombo", address);

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

        #region object helpers
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

        #region new grid function 
        private async void OnOrderItemEditClick(UIInterectionArgs<object> args)
        {
            if (args.DataObject != null)
            {
                OrderItem updateOrderItem = args.DataObject as OrderItem;
                updateOrderItem.IsInEditMode = true;
                order.EditingLineItem = updateOrderItem;
                order.SelectedOrderItem.CopyFrom(order.EditingLineItem);

                UIStateChanged();
            }
        }
        private async void OnOrderItemDeleteClick(UIInterectionArgs<object> args)
        {
            if (args.DataObject != null)
            {
                OrderItem item = args.DataObject as OrderItem;

                bool? result = await _dialogService.ShowMessageBox(
                    "Warning",
                    $"Do you want to remove Item {item.TransactionItem.ItemName}",
                    yesText: "Delete!", cancelText: "Cancel");



                if (result.HasValue && result.Value)
                {
                    if (item != null)
                    {
                        //int index = this.order.OrderItems.IndexOf(item);
                        //order.OrderItems[index].IsActive = 0;
                        (args.DataObject as OrderItem).IsActive = 0;
                    }


                }

                if (order.OrderItems.Count() == 0)
                {
                    this.ToggleEditability("Location", true);
                }

            }

            StateHasChanged();

        }
        #endregion

        private async Task DetailSectionChange(ItemResponse Item)
        {

            ItemRateResponse rates = await RetriveRate(Item);
            StockAsAtRequest request = new StockAsAtRequest();
            request.ElementKey = order.FormObjectKey;
            request.LocationKey = order.OrderLocation.CodeKey;
            request.ItemKey = order.SelectedOrderItem.TransactionItem.ItemKey;
            StockAsAtResponse response = await _orderManager.GetAvailableStock(request);
            order.SelectedOrderItem.AvailableStock = Math.Max(response.StockAsAt, 0);
            order.SelectedOrderItem.TransactionRate = rates.TransactionRate;
            order.SelectedOrderItem.DiscountPercentage = Math.Max(rates.DiscountPercentage, order.HeaderLevelDisountPrecentage);
            order.SelectedOrderItem.ItemTaxType1Per = rates.ItemTaxType1;
            order.SelectedOrderItem.ItemTaxType4Per = rates.ItemTaxType4;
            order.SelectedOrderItem.Rate = rates.Rate;
            order.SelectedOrderItem.IsJustAdded = true;
            order.SelectedOrderItem.IsItemLocked = response.IsLocked;
            await ReadData("HeaderSection2_TrnUnit");
            UIStateChanged();
        }
    }
}
