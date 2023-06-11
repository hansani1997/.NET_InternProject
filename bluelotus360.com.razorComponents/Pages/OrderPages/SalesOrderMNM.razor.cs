using BlueLotus360.CleanArchitecture.Application.Validators.SalesOrder;
using BlueLotus360.CleanArchitecture.Domain.DTO.MasterData;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.DTO.Report;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using bluelotus360.com.razorComponents.Extensions;
using bluelotus360.com.razorComponents.MB.Shared.Components.Popups.MasterDetailPopup;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using bluelotus360.Com.commonLib.Helpers;
using bluelotus360.com.razorComponents.MB.Shared.Components.RigidComponents;
using bluelotus360.Com.commonLib.Reports.Telerik;
using bluelotus360.com.razorComponents.MB.Shared.Components.Popups.Order;
using bluelotus360.com.razorComponents.Pages.Reports.ReportDashboard;

namespace bluelotus360.com.razorComponents.Pages.OrderPages
{
    public partial class SalesOrderMNM
    {
        private BLUIElement formDefinition;
        private Order order;
        private BLUIElement findOrderUI;
        private BLUIElement getFromQuoteUI;
        private BLUIElement modalUIElement;
        private BLUIElement editUIElement;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
        private bool tableloading = false;
        private ReportCompanyDetailsResponse reportCompanyDetailsResponse;
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
        private TerlrikReportOptions _salesOrderReportOption;
        private bool ReportShown = false;
        long elementKey;
        CompletedUserAuth auth;
        private bool IsLessQuantityShown = false;
        BLUIElement salesOrderGrid;
        private bool orderHeaderValidationShown;
        private bool IsAlwAdd, IsAlwUpdate;
        private string permissionMsg;
        private UserMessageDialog _refUserMessage = new UserMessageDialog();
        private CodeBaseResponse TemporyLineLocation = new CodeBaseResponse();
        private AppDbContext _recentlyAccessed = new AppDbContext();

        BLTable<OrderItem> _mudGrid = new BLTable<OrderItem>();
        protected override async Task OnInitializedAsync()
        {
            order = new();
            formDefinition = new();
            validator = new SalesOrderValidatorV2(formDefinition, order);
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
                formDefinition.IsDebugMode = true;
                formDefinition.IsMustElements = formDefinition.Children.Where(x => x.IsMust).Select(i => i._internalElementName).ToList();
                findOrderUI = formDefinition.Children.Where(x => x._internalElementName.Equals("_SearchSalesOrder_")).FirstOrDefault();
                getFromQuoteUI = formDefinition.Children.Where(x => x._internalElementName.Equals("_GetFromQuotation_")).FirstOrDefault();
                salesOrderGrid = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("SalesOrderGrid")).FirstOrDefault();
                //editUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("OrderItemPopUp")).FirstOrDefault();
            }
            if (salesOrderGrid != null)
            {
                salesOrderGrid.Children = formDefinition.Children.Where(x => x.ParentKey == salesOrderGrid.ElementKey).ToList();
            }

            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();

            if (_detector.PlatformDetector().ToUpper().Equals("Windows".ToUpper()))
            {
                ToggleEditability("ScanButton", false);
            }

           // await CheckDeviceType();
            HookInteractions();
            InitilizeNewOrder();

            auth = await _authenticationManager.GetUserInfoOffline();
            _salesOrderReportOption = new TerlrikReportOptions();
            _salesOrderReportOption.ReportName = "SalesOrder_MMN.trdp";
            _salesOrderReportOption.ReportParameters = new Dictionary<string, object>();
            await AddRecentPageAsync();

        }

        private async void InitilizeNewOrder()
        {
            order = new();
            //order.OrderClear();
            order.FormObjectKey = elementKey;
            _salesOrderReportOption = new TerlrikReportOptions();
            ParentLocation = new();
            editUIElement = new();
            //_blTb = new();

            validator = new SalesOrderValidatorV2(formDefinition, order);

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
            InteractionHelper helper = new InteractionHelper(this, formDefinition);
            _interactionLogic = helper.GenerateEventCallbacks();
            //AppSettings.RefreshTopBar("Sales Order");
            appStateService._AppBarName = "Sales Order";

        }

        private async Task CheckDeviceType()
        {
            //deviceType = await _jsRuntime.InvokeAsync<string>("getDeviceType", null);

        }

        #region UI Interaction Logics
        private void OnOrderLocationChanged(UIInterectionArgs<CodeBaseResponse> args)
        {
            this.order.OrderLocation = args.DataObject;
            ParentLocation = this.order.OrderLocation;
            CodeKey = args.DataObject.CodeKey;
            UIStateChanged();
        }
        private void OnLineLevelLocationChanged(UIInterectionArgs<CodeBaseResponse> args)
        {
            order.SelectedOrderItem.OrderLineLocation = args.DataObject;
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
            await ReadData("Address");
            UIStateChanged();
        }
        private async void OnAdr2ComboChange(UIInterectionArgs<AddressResponse> args)
        {
            order.Address2 = args.DataObject;
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
        private void OnLineTransactionUnitChange(UIInterectionArgs<UnitResponse> args)
        {

        }
        private async void OnTransactionItemChange(UIInterectionArgs<ItemResponse> args)
        {
            args.CancelChange = true;
            validator = new SalesOrderValidatorV2(formDefinition, order);

            if (BaseResponse.IsValidData(args.DataObject))
            {
                if (validator != null && validator.CanSelectItem())
                {
                    await ShowAddNewItem(args.DataObject);
                }
                else
                {
                    orderHeaderValidationShown = true;
                    UIStateChanged();
                }

            }

            UIStateChanged();

        }
        private void TransactionUnit_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
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
        private async Task ShowAddNewItem(ItemResponse Item)
        {
            ToggleViisbility("AddItemToGrod", true);
            ToggleViisbility("CancelItemPopup", true);
            UIStateChanged();

            order.SelectedOrderItem = order.CreateOrderItem(Item, order.OrderLocation);
            if (modalUIElement == null)
            {
                if (!_modalDefinitions.TryGetValue("OrderItemPopUps", out modalUIElement))
                {
                    modalUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("OrderItemPopUp")).FirstOrDefault();
                    if (modalUIElement != null && modalUIElement.Children.Count > 0)
                    {
                        _modalDefinitions.Add("OrderItemPopUps", modalUIElement);

                    }
                }
            }

            ItemRateResponse rates = await RetriveRate(Item);
            StockAsAtRequest request = new StockAsAtRequest();
            request.ElementKey = order.FormObjectKey;
            request.LocationKey = order.OrderLocation.CodeKey;
            request.IsuLocKy = order.OrderLocation.CodeKey;
            request.ItemKey = order.SelectedOrderItem.TransactionItem.ItemKey;
            //StockAsAtResponse response = await _transactionManager.GetStockAsAt(request);

            StockAsAtResponse response = await _orderManager.GetAvailableStock(request);
            order.SelectedOrderItem.AvailableStock = Math.Max(response.StockAsAt, 0);
            order.SelectedOrderItem.TransactionRate = rates.TransactionRate;
            order.SelectedOrderItem.DiscountPercentage = Math.Max(rates.DiscountPercentage, order.HeaderLevelDisountPrecentage);
            order.SelectedOrderItem.ItemTaxType1Per = rates.ItemTaxType1;
            order.SelectedOrderItem.ItemTaxType4Per = rates.ItemTaxType4;
            order.SelectedOrderItem.Rate = rates.Rate;
            order.SelectedOrderItem.IsJustAdded = true;
            order.SelectedOrderItem.IsItemLocked = response.IsLocked;
            order.SelectedOrderItem.IsAlwMinusQty = response.IsAlwMinusQty;

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
                ToggleViisbility("RequiredQuantity", false);
                ToggleViisbility("LineLevelLocation", false);
                ToggleViisbility("IsTransfer", false);
                ToggleViisbility("IsConfirmed", false);
            }



            isItemPopupShown = true;
            UIStateChanged();
        }
        private void UIStateChanged()
        {
            this.StateHasChanged();
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


        private async void OnOrderNewClick(UIInterectionArgs<object> args)
        {
            InitilizeNewOrder();

            //await ReadData("Location");
            //await ReadData("Address");
            //await ReadData("RepAdress");

            IList<KeyValuePair<string, IBLUIOperationHelper>> pairs = _objectHelpers.ToList();

            foreach (KeyValuePair<string, IBLUIOperationHelper> helper in pairs)
            {
                if (helper.Value != null && (helper.Value as IBLServerDependentComponent) != null)
                {
                    await (helper.Value as IBLServerDependentComponent).FetchData(false);
                }

            }

            UIStateChanged();
        }
        private async void OnOrderSaveClick(UIInterectionArgs<object> args)
        {
            bool needToRequest = false;

            validator = new SalesOrderValidatorV2(formDefinition, order);

            if (validator != null && validator.CanOrderSave())
            {
                if (order.OrderKey == 1)
                {
                    if (IsAlwAdd)
                    {
                        if (order.IsFromQuotation)
                        {

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
                            await _orderManager.SaveOrder(order);

                            _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                            _snackBar.Add("Sales Order has been  Saved Successfully", Severity.Success);

                            OrderOpenRequest oprequest = new OrderOpenRequest();
                            oprequest.OrderKey = order.OrderKey;
                            LoadOrder(oprequest);
                        }
                    }
                    else
                    {
                        //CantInsertPopUpShown = true;
                        validator = new SalesOrderValidatorV2(formDefinition, new Order());
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

                        OrderOpenRequest oprequest = new OrderOpenRequest();
                        oprequest.OrderKey = order.OrderKey;
                        LoadOrder(oprequest);
                    }
                    else
                    {
                        validator = new SalesOrderValidatorV2(formDefinition, new Order());
                        validator.UserMessages.AddErrorMessage(permission.Message);
                        _refUserMessage.ShowUserMessageWindow();

                    }

                    UIStateChanged();
                }


            }
            else
            {
                orderHeaderValidationShown = true;
                UIStateChanged();
            }


        }

        private async void onScan(UIInterectionArgs<object> args)
        {
            validator = new SalesOrderValidatorV2(formDefinition, order);

            if (validator != null && validator.CanSelectItem())
            {
                var result = await _barcodeService.ReadBarcode();
                string itemCode = "";
                if (result!=null)
                {
                    itemCode = result as string;
                }
                ItemRequestModel requestModel = new ItemRequestModel(); ;
                requestModel.ItemCode = itemCode;
                requestModel.LocationKey = order.OrderLocation.CodeKey;
                requestModel.AddressKey = order.OrderCustomer.AddressKey;
                IList<ItemCodeResponse> items = await _comboManager.GetItemByItemCode(requestModel);
                if (items!=null && items.Count > 0)
                {
                    ItemResponse response = new ItemResponse();
                    response.ItemName = items[0].ItemCodeName;
                    response.ItemKey = items[0].ItemKey;
                    await ShowAddNewItem(response);
                }
                else
                {
                    _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackBar.Add("Invalid Item Code", Severity.Error);
                }
            }

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

        private async void OnOrderPrintClick(UIInterectionArgs<object> args)
        {
            if (order.OrderKey > 1)
            {
                if (_salesOrderReportOption != null && _salesOrderReportOption.ReportParameters != null)
                {
                    _salesOrderReportOption.ReportParameters.Clear();
                    _salesOrderReportOption.ReportName = "SalesOrder_MMN.trdp";
                    _salesOrderReportOption.ReportParameters.Add("Cky", auth.AuthenticatedCompany.CompanyKey);
                    _salesOrderReportOption.ReportParameters.Add("UsrKy", auth.AuthenticatedUser.UserKey);
                    _salesOrderReportOption.ReportParameters.Add("UsrId", auth.AuthenticatedUser.UserID);
                    _salesOrderReportOption.ReportParameters.Add("OrdKy", order.OrderKey);

                    ReportShown = true;

                }

            }
            else
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Invalid Request. Please select a record.", Severity.Error);
            }


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
                    stock_request.IsuLocKy = order.OrderLocation.CodeKey;
                    stock_request.ItemKey = order.SelectedOrderItem.TransactionItem.ItemKey;

                    if (stock_request != null)
                    {
                        //response = await _transactionManager.GetStockAsAt(stock_request);
                        response = await _orderManager.GetAvailableStock(stock_request);
                    }

                    if (response != null)
                    {
                        order.SelectedOrderItem.AvailableStock = Math.Max(response.StockAsAt, 0);
                        order.SelectedOrderItem.IsItemLocked = response.IsLocked;
                        order.SelectedOrderItem.IsAlwMinusQty = response.IsAlwMinusQty;
                    }


                    if (!_modalDefinitions.TryGetValue("OrderItemPopUps", out editUIElement))
                    {
                        editUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("OrderItemPopUp")).FirstOrDefault();
                        editUIElement.IsMustElements = editUIElement.Children.Where(x => x.IsMust).Select(i => i._internalElementName).ToList();

                        if (editUIElement != null && editUIElement.Children.Count() > 0)
                        {
                            _modalDefinitions.Add("OrderItemPopUps", editUIElement);
                        }
                    }


                    isEditPopupShown = true;

                    await SetValue("AvlQty", order.SelectedOrderItem.AvailableStock);
                    await SetValue("RequiredQuantity", order.SelectedOrderItem.RequestedQuantity);
                    await SetValue("LineLevelLocation", order.SelectedOrderItem.OrderLineLocation);
                    //TODO
                    await SetValue("ItemName", order.SelectedOrderItem.TransactionItem.ItemName);
                    TemporyLineLocation = order.SelectedOrderItem.OrderLineLocation;

                    UIStateChanged();
                }
            }

        }



        #endregion


        private void Done()
        {
            InitilizeNewOrder();
            isClickSaveButton = false;
        }

        private async Task Print()
        {
            //var parameters = new DialogParameters
            //{


            //};

            //DialogOptions options = new DialogOptions();
            //options.FullScreen = true;
            //var dialog = _dialogService.Show<PrintDialog>("Print", parameters, options);
            //await Task.CompletedTask;

        }

        //private async Task ShowEditItem(OrderItem Item)
        //{
        //    BLUIElement editUIElement;

        //    order.EditingLineItem = Item;
        //    order.SelectedOrderItem = new();
        //    this.order.SelectedOrderItem.CopyFrom(Item);

        //    StockAsAtRequest stock_request = new StockAsAtRequest();
        //    stock_request.ElementKey = order.FormObjectKey;
        //    stock_request.LocationKey = order.OrderLocation.CodeKey;
        //    stock_request.ItemKey = order.SelectedOrderItem.TransactionItem.ItemKey;
        //    StockAsAtResponse response = await _transactionManager.GetStockAsAt(stock_request);
        //    order.SelectedOrderItem.AvailableStock = Math.Max(response.StockAsAt, 0);

        //    //if (Item.RequestedQuantity > 0 || Item.NeedToRequestFromAnotherLocation())
        //    //{
        //    //    ToggleViisbility("RequiredQuantity", true);
        //    //    ToggleViisbility("LineLevelLocation", true);
        //    //    ToggleViisbility("IsTransfer", true);
        //    //    ToggleViisbility("IsConfirmed", true);
        //    //    UIStateChanged();
        //    //}

        //    //ToggleViisbility("TransactionItem", true);

        //    if (!_modalDefinitions.TryGetValue("OrderItemPopUps", out editUIElement))
        //    {
        //        editUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("OrderItemPopUp")).FirstOrDefault();

        //        if (editUIElement != null && editUIElement.Children.Count() > 0)
        //        {
        //            _modalDefinitions.Add("OrderItemPopUps", editUIElement);
        //        }
        //    }

        //    if (editUIElement != null)
        //    {

        //        var parameters = new DialogParameters
        //        {
        //            ["OrderItem"] = this.order.SelectedOrderItem,
        //            ["ModalUIElement"] = editUIElement,
        //            ["InteractionLogics"] = _interactionLogic,
        //            ["ObjectHelpers"] = _objectHelpers,
        //            ["ParentLocation"] = this.order.OrderLocation,
        //            ["Validaor"] = validator,
        //            ["ButtonName"] = "Update",
        //            ["HeadingPopUp"] = "Edit Item",
        //        };
        //        DialogOptions options = new DialogOptions();
        //        var dialog = _dialogService.Show<OrderItemDialog>("Edit Item", parameters, options);
        //        var result = await dialog.Result;

        //        if (!result.Cancelled)
        //        {
        //            //order.Update(order.SelectedOrderItem, (int)(order.SelectedOrderItem.LineNumber - 1));
        //            if (order.EditingLineItem!=null)
        //            {
        //                order.EditingLineItem.CopyFrom(order.SelectedOrderItem);
        //                order.SelectedOrderItem = new();
        //            }
        //        }
        //        else
        //        {

        //        }

        //    }

        //    if (!validator.CanChangeHeaderInformatiom())
        //    {
        //        this.ToggleEditability("Location", false);
        //    }
        //}


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

        private async void OnAddItemButtonClick(UIInterectionArgs<object> args)
        {
            validator = new SalesOrderValidatorV2(formDefinition, order);
            if (validator.CanAddItemToGrid())
            {
                this.order.SelectedOrderItem.LineTotal = this.order.SelectedOrderItem.TransactionRate * this.order.SelectedOrderItem.TransactionQuantity;
                this.order.SelectedOrderItem.LineTotalWithoutDiscount = this.order.SelectedOrderItem.TransactionRate * this.order.SelectedOrderItem.TransactionQuantity;
                this.order.SelectedOrderItem.LineNumber = this.order.OrderItems.Count() + 1;
                this.order.OrderDate = DateTime.Now;
                if (this.order.SelectedOrderItem.OrderLineLocation.CodeKey == 1)
                {
                    this.order.SelectedOrderItem.OrderLineLocation = this.order.OrderLocation;
                }
                order.AddGridItems(this.order.SelectedOrderItem);

                _objectHelpers["TransactionItem"].ResetToInitialValue();

                //if (_blTb != null)
                //{
                //    _blTb.Refresh();
                //}
                isItemPopupShown = false;
                if (order.OrderItems.Count() > 0)
                {
                    this.ToggleEditability("Location", false);
                }
            }


            UIStateChanged();
            await Task.CompletedTask;

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

        private async void LoadOrder(OrderOpenRequest request)
        {
            HideAllPopups();

            this.appStateService.IsLoaded = true;

            Order loaded_order = await _orderManager.OpenOrder(request);
            loaded_order.FormObjectKey = order.FormObjectKey;
            order.CopyFrom(loaded_order);

            await SetValue("HeaderTitle", order.OrderNumber);
            validator = new SalesOrderValidatorV2(formDefinition, order);

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
            order.IsFromQuotation= true;

            //foreach (var itm in order.OrderItems)
            //{
            //    StockAsAtRequest stock_request = new StockAsAtRequest();
            //    stock_request.ElementKey = order.FormObjectKey;
            //    stock_request.LocationKey = order.OrderLocation.CodeKey;
            //    stock_request.ItemKey = itm.TransactionItem.ItemKey;
            //    StockAsAtResponse response = await _transactionManager.GetStockAsAt(stock_request);
            //    itm.AvailableStock = Math.Max(response.StockAsAt, 0);
            //}

            await SetValue("HeaderTitle", order.OrderNumber);
            this.appStateService.IsLoaded = false;
            UIStateChanged();
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

        #region customer creation 
        private async Task OnCustomerCreateSuccess(AddressMaster address)
        {
            await ReadData("Address");
            await SetValue("Address", address);

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
        #endregion

        #region new grid function 
        private async void OnOrderItemEditClick(UIInterectionArgs<object> args)
        {
            if (args.DataObject != null)
            {
                OrderItem updateOrderItem = args.DataObject as OrderItem;

                if (updateOrderItem != null)
                {
                    await this.ShowEditItem(updateOrderItem);
                }
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
                        int index = this.order.OrderItems.IndexOf(item);
                        order.OrderItems[index].IsActive = 0;

                    }

                    StateHasChanged();
                }

                if (order.OrderItems.Count() == 0)
                {
                    this.ToggleEditability("Location", true);
                }

            }



        }
        #endregion


        #region old grid function


        private async void OnOrderItemEdit(decimal index)
        {
            var updateOrderItem = order.OrderItems.ElementAt((int)index);

            if (updateOrderItem != null)
            {
                await this.ShowEditItem(updateOrderItem);
            }


        }

        private async void OnOrderItemDelete(OrderItem item)
        {

            bool? result = await _dialogService.ShowMessageBox(
                "Warning",
                $"Do you want to remove Item {item.TransactionItem.ItemName}",
                yesText: "Delete!", cancelText: "Cancel");

            if (result.HasValue && result.Value)
            {
                if (item != null)
                {
                    //if (item.IsJustAdded)
                    //{
                    //    this.order.OrderItems.Remove(item);
                    //}
                    //else
                    //{
                    int index = this.order.OrderItems.IndexOf(item);
                    order.OrderItems[index].IsActive = 0;
                    //}
                }

                StateHasChanged();
            }

            if (order.OrderItems.Count() == 0)
            {
                this.ToggleEditability("Location", true);
            }



        }

        #endregion

        private async Task AddRecentPageAsync()
        {
            string pageName = "Sales Order MNM";
            string pageUrl = "/order/salesordernb";
            await _recentlyAccessed.AddRecentPageAsync(pageName, pageUrl);
        }
    }
}
