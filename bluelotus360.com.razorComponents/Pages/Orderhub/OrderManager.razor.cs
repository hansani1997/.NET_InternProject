using BL10.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using bluelotus360.Com.commonLib.Helpers;
using BlueLotus360.Com.Infrastructure.OrderPlatforms.PickMe;
using BlueLotus360.Com.Infrastructure.OrderPlatforms.Ubereats;
using bluelotus360.com.razorComponents.Extensions;
using bluelotus360.com.razorComponents.MB.Shared.Components;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlueLotus360.Com.Infrastructure.OrderPlatforms.PickMe.PickmeEntity;
using bluelotus360.com.razorComponents.Pages.Orderhub.Components;
using Microsoft.Extensions.Options;

namespace bluelotus360.com.razorComponents.Pages.Orderhub
{
    public partial class OrderManager
    {
        #region parameter

        private BLUIElement formDefinition;
        private MudTabs tabs;
        private BLUIElement Grid;
        private BLUIElement EditGrid;
        private BLUIElement EditSection;
        private BLUIElement FilterSection;
        private BLUIElement UberOrderSection;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
        private UIBuilder _refBuilder;
        private BLTable<PartnerOrder> _blTb;
        private UserMessageDialog _refUserMessage;


        private bool fixedheader = true;
        private bool IsProcessing;
        private bool IsDataLoading;
        private PartnerOrder _order;
        private PartnerOrder _SelectedOrder;
        public IList<CodeBaseResponse> OrderStatus;
        private IList<PartnerOrder> ListOfOrders;
        private IList<PartnerOrder> ListOfIncomingOrders;
        private IList<PartnerOrder> NewPickmeOrders;
        private IList<string> AvailableOrderID;
        private IList<string> IncomingOrderID;
        private RequestParameters requestParameters;
        private bool isOpenMoreInfo = false;
        private bool isOpenFilter = false;
        private bool isOpenNotification = false;
        private bool isOpenEdit = false;
        private bool isOpenUber = false;
        GetMoreOrderInformation moredata = new GetMoreOrderInformation();
        FilterOrder _FilterOrderData = new FilterOrder();
        EditOrder editOrder = new EditOrder();
        UserMessageManager Messages = new UserMessageManager();
        private readonly PeriodicTimer _periodicTimer = new(TimeSpan.FromSeconds(30));
        // private readonly PeriodicTimer _periodicPickmeTimer = new(TimeSpan.FromSeconds(600));
        private Timer timer;
        private long ObjKy = 1;

        #endregion

        #region General
        protected override async Task OnInitializedAsync()
        {
            long elementKey = 1;
            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);// get element key from url 

            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();

                formrequest.MenuKey = elementKey;
                ObjKy = formrequest.MenuKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);//get ui elements
            }

            if (formDefinition != null)
            {
                formDefinition.IsDebugMode = true;
            }


            _interactionLogic = new Dictionary<string, EventCallback>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();
            _order = new PartnerOrder();
            ListOfOrders = new List<PartnerOrder>();
            ListOfIncomingOrders = new List<PartnerOrder>();
            requestParameters = new RequestParameters();
            _blTb = new BLTable<PartnerOrder>();
            _SelectedOrder = new PartnerOrder();
            tabs = new MudTabs();
            NewPickmeOrders = new List<PartnerOrder>();
            AvailableOrderID = new List<string>();
            IncomingOrderID = new List<string>();

            HookInteractions();
            if (formDefinition != null)
            {
                Grid = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("MudDataTable")).FirstOrDefault();
                FilterSection = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("FilterPopup")).FirstOrDefault();
                EditSection = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("OrderAmendment")).FirstOrDefault();
                EditGrid = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("EditOrderTable")).FirstOrDefault();
                UberOrderSection = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("UberOrderAmendment")).FirstOrDefault();
                formDefinition.IsDebugMode = true;
            }

            if (Grid != null)
            {
                Grid.Children = formDefinition.Children.Where(x => x.ParentKey == Grid.ElementKey).ToList();
            }


            requestParameters.FromDate = DateTime.Now.ToString("yyyy/MM/dd");
            requestParameters.ToDate = DateTime.Now.ToString("yyyy/MM/dd");
            await GetOrderStatus(requestParameters.FromDate, requestParameters.ToDate);

            if (OrderStatus.Where(x => x.OurCode == "InCmng").FirstOrDefault() != null)
            {
                requestParameters.StatusKey = OrderStatus.Where(x => x.OurCode == "InCmng").FirstOrDefault().CodeKey;
            }

            await GetAllOrder(requestParameters.StatusKey, requestParameters.FromDate, requestParameters.ToDate);
            // var timer = new PeriodicTimer(TimeSpan.FromSeconds(30));
            //Timer t = new Timer(120000);
            //t.AutoReset = true;
            //t.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            //t.Start();
            while (await _periodicTimer.WaitForNextTickAsync())
            {
                if (OrderStatus.Count > 0)
                {
                    // await SyncOrders();
                    int StatusKey = OrderStatus.Where(x => x.OurCode == "InCmng").FirstOrDefault().CodeKey;
                    await GetIncomingOrder(StatusKey, DateTime.Now.ToString("yyyy/MM/dd"), DateTime.Now.ToString("yyyy/MM/dd"));
                    await GetRecentPickmeOrders();
                    if (ListOfIncomingOrders.Count > 0)
                    {
                        isOpenNotification = true;
                        await InvokeAsync(StateHasChanged);
                    }
                    else if (NewPickmeOrders != null)
                    {
                        //if (NewPickmeOrders.Count > 0)
                        //{
                        //    isOpenNotification = true;
                        //    NewPickmeOrders = new List<PartnerOrder>();
                        //    await InvokeAsync(StateHasChanged);
                        //}

                    }
                }

            }
        }

        //private async void OnTimedEvent(Object source, ElapsedEventArgs e)
        //{
        //   await InvokeAsync(SyncOrders);
        //}

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _periodicTimer.Dispose();
            }
        }

        private void UIStateChanged()
        {

            this.StateHasChanged();
        }

        private void HookInteractions()
        {
            InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
            _interactionLogic = helper.GenerateEventCallbacks();// generate all event callbacks 
            //AppSettings.RefreshTopBar("Geo Attendence");
            appStateService._AppBarName = "Order Manager";
        }



        #endregion

        #region Ui Events
        private void LocationOnChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            _order.Location = args.DataObject;
            GetOrderStatus(requestParameters.FromDate, requestParameters.ToDate);
            //GetAllOrder(requestParameters.StatusKey, requestParameters.FromDate, requestParameters.ToDate);
            UIStateChanged();
        }

        private void BUOnChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            _order.BU = args.DataObject;
            GetOrderStatus(requestParameters.FromDate, requestParameters.ToDate);
            UIStateChanged();
        }

        private void StatusOnChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            HideAllPopups();
            UIStateChanged();
        }


        private async void TabClick(CodeBaseResponse item)
        {
            HideAllPopups();
            requestParameters.StatusKey = item.CodeKey;
            await GetAllOrder(requestParameters.StatusKey, requestParameters.FromDate, requestParameters.ToDate);
            await ReadData("MudStsCmb");
            UIStateChanged();
        }
        private async void GridPageChanged(int pageIndex)
        {
            await ReadData("MudStsCmb");
            IList<KeyValuePair<string, IBLUIOperationHelper>> pairs = _objectHelpers.ToList();

            foreach (KeyValuePair<string, IBLUIOperationHelper> helper in pairs)
            {
                await helper.Value.Refresh();

            }

            UIStateChanged();
        }
        private void TodayClick(UIInterectionArgs<object> args)
        {
            HideAllPopups();
            requestParameters.FromDate = DateTime.Now.ToString("yyyy/MM/dd");
            requestParameters.ToDate = DateTime.Now.ToString("yyyy/MM/dd");
            GetOrderStatus(requestParameters.FromDate, requestParameters.ToDate);
            UIStateChanged();
        }

        private void YesterdayClick(UIInterectionArgs<object> args)
        {
            HideAllPopups();
            requestParameters.FromDate = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
            requestParameters.ToDate = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
            GetOrderStatus(requestParameters.FromDate, requestParameters.ToDate);
            UIStateChanged();
        }

        private void WeekClick(UIInterectionArgs<object> args)
        {
            HideAllPopups();
            DateTime baseDate = DateTime.Now;
            requestParameters.FromDate = baseDate.AddDays(-(int)baseDate.DayOfWeek).ToString("yyyy/MM/dd");
            requestParameters.ToDate = Convert.ToDateTime(requestParameters.FromDate).AddDays(7).AddSeconds(-1).ToString("yyyy/MM/dd");
            GetOrderStatus(requestParameters.FromDate, requestParameters.ToDate);
            UIStateChanged();
        }

        private void MonthClick(UIInterectionArgs<object> args)
        {
            HideAllPopups();
            DateTime baseDate = DateTime.Now;
            requestParameters.FromDate = baseDate.AddDays(1 - baseDate.Day).ToString("yyyy/MM/dd");
            requestParameters.ToDate = Convert.ToDateTime(requestParameters.FromDate).AddMonths(1).AddSeconds(-1).ToString("yyyy/MM/dd");
            GetOrderStatus(requestParameters.FromDate, requestParameters.ToDate);
            UIStateChanged();
        }

        private void CustomClick(UIInterectionArgs<object> args)
        {
            HideAllPopups();
            isOpenFilter = true;
            UIStateChanged();
        }

        private async void ManualSync(UIInterectionArgs<object> args)
        {
            appStateService.IsLoaded = true;
            PickmeAPIHandler pickmeAPIHandler = new PickmeAPIHandler(_apiManager, _orderManager, _addressManager, _codebaseManager);
            GetOrder pickmeOrder = await pickmeAPIHandler.GetPickmeOrder(_order.Location.CodeKey, _order.BU.CodeKey);
            bool isDataSaved = false;
            if (pickmeOrder.Data.Count == 0)
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("There is no PickMe orders to sync", Severity.Info);
            }
            else if (pickmeOrder.Data.Count > 0)
            {
                //IncomingOrderID = pickmeOrder.Data.Select(x => x.PickmeJobID).ToList();
                //IList<PartnerOrder> Order = await AvailablePickmeOrder();
                //if (Order.Count > 0)
                //{
                //    AvailableOrderID = Order.Select(x => x.OrderId).Distinct().ToList();
                //    NewPickmeOrders = IncomingOrderID.Except(AvailableOrderID).ToList();
                //}
                //else
                //{
                //    NewPickmeOrders = IncomingOrderID;
                //}

                isDataSaved = await pickmeAPIHandler.SavePickmeOrder(pickmeOrder, _order.Location.CodeKey, requestParameters.FromDate, requestParameters.ToDate, _order.BU.CodeKey, ObjKy);
                if (isDataSaved)
                {


                    RequestParameters param = new RequestParameters()
                    {
                        LocationKey = _order.Location.CodeKey,
                        StatusKey = 1,
                        FromDate = requestParameters.FromDate,
                        ToDate = requestParameters.ToDate,
                        BUKy = _order.BU.CodeKey,
                        PlatformName = "PickMe"
                    };
                    await _orderManager.InsertLastOrderSync(param);
                    _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackBar.Add("Orders have been synced successfully", Severity.Success);
                }
                else
                {
                    _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackBar.Add("Something went wrong", Severity.Error);
                }

            }
            appStateService.IsLoaded = false;
            await GetOrderStatus(requestParameters.FromDate, requestParameters.ToDate);
            requestParameters.StatusKey = OrderStatus.Where(x => x.OurCode == "InCmng").FirstOrDefault().CodeKey;
            await GetAllOrder(requestParameters.StatusKey, requestParameters.FromDate, requestParameters.ToDate);
            //if (_blTb != null)
            //{
            //    _blTb.Refresh();
            //}
            UIStateChanged();
        }

        private async void MoreInfoClick(UIInterectionArgs<object> args)
        {
            HideAllPopups();
            PartnerOrder order = args.DataObject as PartnerOrder;
            await GetOrderByOrderKey(Convert.ToInt32(order.PartnerOrderId));
            isOpenMoreInfo = true;
            moredata.GetSingleOrder(_order);

            UIStateChanged();
        }

        private async void EditOrderClick(UIInterectionArgs<object> args)
        {
            HideAllPopups();
            PartnerOrder order = args.DataObject as PartnerOrder;

            await GetOrderByOrderKey(Convert.ToInt32(order.PartnerOrderId));
            isOpenEdit = true;
            editOrder.GetSingleOrder(_order);
            UIStateChanged();


        }



        private async void SaveOrderStatus(UIInterectionArgs<object> args)
        {
            appStateService.IsLoaded = true;
            bool success = false;
            UberOrderHandler uberOrderHandler = new UberOrderHandler(_apiManager, _orderManager);
            PartnerOrder order = args.DataObject as PartnerOrder;
            if (order.OrderStatus.CodeKey > 11)
            {
                IList<CodeBaseResponse> Allstatus = await _orderManager.GetOrderStatus();
                if (order.Platforms.AccountName == "Uber Eats")
                {
                    if (requestParameters.StatusKey == OrderStatus.Where(x => x.OurCode == "InCmng").FirstOrDefault().CodeKey)
                    {
                        DateTime UTCOrdDtm = Convert.ToDateTime(order.OrderDate).ToUniversalTime();
                        DateTime CurrentTimeUTC = DateTime.Now.ToUniversalTime();
                        if (UTCOrdDtm < CurrentTimeUTC.AddMinutes(-11.5))
                        {
                            int key = Allstatus.Where(x => x.OurCode == "Reject").FirstOrDefault().CodeKey;
                            if (key > 11)
                            {
                                RequestParameters requestrejectdata = new RequestParameters()
                                {
                                    StatusKey = key,
                                    OrderKey = Convert.ToInt32(order.PartnerOrderId)
                                };
                                success = await _orderManager.OrderHubStatus_UpdateWeb(requestrejectdata);
                                await uberOrderHandler.DenyUberOrderByOrderId(order.OrderId, "");
                                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                                _snackBar.Add("Time Out.Order has been Rejected", Severity.Error);
                                tabs.ActivatePanel(0);
                            }
                        }

                        else
                        {
                            int ConfirmKey = Allstatus.Where(x => x.OurCode == "Confirm").FirstOrDefault().CodeKey;
                            int CancelKey = Allstatus.Where(x => x.OurCode == "Cancel").FirstOrDefault().CodeKey;
                            RequestParameters requestdata = new RequestParameters()
                            {
                                StatusKey = order.OrderStatus.CodeKey,
                                OrderKey = Convert.ToInt32(order.PartnerOrderId)
                            };
                            success = await _orderManager.OrderHubStatus_UpdateWeb(requestdata);
                            if (order.OrderStatus.CodeKey == ConfirmKey)
                            {
                                await uberOrderHandler.AcceptUberOrderByOrderId(order.OrderId);
                            }
                            else if (order.OrderStatus.CodeKey == CancelKey)
                            {
                                await uberOrderHandler.CancelUberOrderByOrderId(order.OrderId, "");
                            }
                        }
                    }
                    else
                    {
                        int ConfirmKey = Allstatus.Where(x => x.OurCode == "Confirm").FirstOrDefault().CodeKey;
                        int CancelKey = Allstatus.Where(x => x.OurCode == "Cancel").FirstOrDefault().CodeKey;
                        RequestParameters requestdata = new RequestParameters()
                        {
                            StatusKey = order.OrderStatus.CodeKey,
                            OrderKey = Convert.ToInt32(order.PartnerOrderId)
                        };
                        success = await _orderManager.OrderHubStatus_UpdateWeb(requestdata);
                        if (order.OrderStatus.CodeKey == ConfirmKey)
                        {
                            await uberOrderHandler.AcceptUberOrderByOrderId(order.OrderId);
                        }
                        else if (order.OrderStatus.CodeKey == CancelKey)
                        {
                            await uberOrderHandler.CancelUberOrderByOrderId(order.OrderId, "");
                        }
                    }
                }

                else
                {
                    RequestParameters requestdatax = new RequestParameters()
                    {
                        StatusKey = order.OrderStatus.CodeKey,
                        OrderKey = Convert.ToInt32(order.PartnerOrderId)
                    };
                    success = await _orderManager.OrderHubStatus_UpdateWeb(requestdatax);

                }

                //await GetOrderByOrderKey(Convert.ToInt32(order.PartnerOrderId));


                if (success)
                {
                    await GetOrderStatus(requestParameters.FromDate, requestParameters.ToDate);
                    // requestParameters.StatusKey = OrderStatus.Where(x => x.OurCode == "InCmng").FirstOrDefault().CodeKey;
                    await GetAllOrder(requestParameters.StatusKey, requestParameters.FromDate, requestParameters.ToDate);
                    //if (_blTb != null)
                    //{
                    //    _blTb.Refresh();
                    //}
                    appStateService.IsLoaded = false;
                    UIStateChanged();
                }
                else
                {
                    appStateService.IsLoaded = false;
                    _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackBar.Add("Something went wrong", Severity.Error);
                }

                UIStateChanged();
            }
            else
            {
                appStateService.IsLoaded = false;
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Order Status is Empty. Please select the order status", Severity.Error);
            }

        }


        //private async void SyncOrders()
        //{
        //    //bool isDataSaved = false;
        //    //appStateService.IsLoaded = true;
        //    if (_order.Location.CodeKey > 11)
        //    {
        //        IList<CodeBaseResponse> BUList = await _orderManager.GetOrderHubBU();
        //        if (BUList.Count > 0)
        //        {
        //            foreach (CodeBaseResponse item in BUList)
        //            {
        //                await PickmeOrders(item.CodeKey);
        //            }

        //        }
        //        else
        //        {
        //            await PickmeOrders(1);
        //        }
        //    }

        //}

        private void GetMissingUberOrder(UIInterectionArgs<object> args)
        {
            HideAllPopups();
            isOpenUber = true;
            UIStateChanged();
        }


        //public async Task<bool> PickmeOrders(int BUKy)
        //{
        //    PickmeAPIHandler pickmeAPIHandler = new PickmeAPIHandler(_apiManager, _orderManager, _addressManager, _codebaseManager);
        //    GetOrder pickmeOrder = await pickmeAPIHandler.GetPickmeOrder(_order.Location.CodeKey, BUKy);
        //    bool isDataSaved = false;
        //    //if (pickmeOrder.Data.Count == 0)
        //    //{
        //    //    _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
        //    //    _snackBar.Add("There is no PickMe orders to sync", Severity.Info);
        //    //}
        //    if (pickmeOrder.Data.Count > 0)
        //    {
        //        IncomingOrderID = pickmeOrder.Data.Select(x => x.PickmeJobID).ToList();
        //        IList<PartnerOrder> Order = await AvailablePickmeOrder();
        //        if(Order.Count > 0)
        //        {
        //            AvailableOrderID = Order.Select(x => x.OrderId).Distinct().ToList();
        //            NewPickmeOrders = IncomingOrderID.Except(AvailableOrderID).ToList();
        //        }
        //        else
        //        {
        //            NewPickmeOrders = IncomingOrderID;
        //        }

        //        isDataSaved = await pickmeAPIHandler.SavePickmeOrder(pickmeOrder, _order.Location.CodeKey, requestParameters.FromDate, requestParameters.ToDate, BUKy, ObjKy);
        //        if (isDataSaved)
        //        {


        //            RequestParameters param = new RequestParameters()
        //            {
        //                LocationKey = _order.Location.CodeKey,
        //                StatusKey = 1,
        //                FromDate = requestParameters.FromDate,
        //                ToDate = requestParameters.ToDate,
        //                PlatformName = "PickMe"
        //            };
        //            await _orderManager.InsertLastOrderSync(param);
        //            //_snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
        //            //_snackBar.Add("Orders have been synced successfully", Severity.Success);
        //        }
        //        //else
        //        //{
        //        //    _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
        //        //    _snackBar.Add("Something went wrong", Severity.Error);
        //        //}

        //    }
        //    //appStateService.IsLoaded = false;
        //    //await GetOrderStatus(requestParameters.FromDate, requestParameters.ToDate);
        //    //requestParameters.StatusKey = OrderStatus.Where(x => x.OurCode == "InCmng").FirstOrDefault().CodeKey;
        //    //await GetAllOrder(requestParameters.StatusKey, requestParameters.FromDate, requestParameters.ToDate);
        //    //if (_blTb != null)
        //    //{
        //    //    _blTb.Refresh();
        //    //}
        //    //UIStateChanged();
        //    return isDataSaved;
        //}

        private async Task<IList<PartnerOrder>> AvailablePickmeOrder()
        {
            RequestParameters Parameters = new RequestParameters()
            {
                LocationKey = _order.Location.CodeKey,
                StatusKey = 1,
                FromDate = requestParameters.FromDate,
                ToDate = requestParameters.ToDate
            };

            return await _orderManager.GetAvailablePickmeOrders(Parameters);
        }

        private async void MudStsCmb_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
        {
            args.DataObject.AddtionalData.Add("StatusKey", requestParameters.StatusKey);
            //IList<CodeBaseResponse> status = await _orderManager.GetOrderStatus();
            //if(status.Count > 0)
            //{
            //    int CompleteKey = status.Where(x => x.OurCode == "Complete").FirstOrDefault().CodeKey;
            //    int CancelKey = status.Where(x => x.OurCode == "Cancel").FirstOrDefault().CodeKey;
            //    if (requestParameters.StatusKey == CompleteKey)
            //    {
            //        ToggleEditability("StsCmb", false);
            //    }
            //}

        }
        private async void MudStsCmb_AfterComboLoaded(UIInterectionArgs<IList<CodeBaseResponse>> args)
        {

            UIStateChanged();
            await Task.CompletedTask;
        }

        private async void PushToWMS(UIInterectionArgs<object> args)
        {
            bool success = false;
            PartnerOrder order = args.DataObject as PartnerOrder;
            IList<CodeBaseResponse> Allstatus = await _orderManager.GetOrderStatus();
            int ConfirmKey = Allstatus.Where(x => x.OurCode == "Confirm").FirstOrDefault().CodeKey;
            int CancelKey = Allstatus.Where(x => x.OurCode == "Cancel").FirstOrDefault().CodeKey;

            StockInjection stockInjection = new StockInjection()
            {
                OrderKey = Convert.ToInt32(order.PartnerOrderId),
                IntegrationId = "4824fc92-10fa-4eca-a7d0-e7048892bc84",
                RequestId = "JKLL_TST"
            };
            success = StockUpdateAfterConfirmation(stockInjection);


            if (success)
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("WMS Notified", Severity.Success);
            }
            else
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Something went wrong", Severity.Error);
            }

            UIStateChanged();
        }

        private bool StockUpdateAfterConfirmation(StockInjection stockInjection)
        {
            string Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            var client = new RestClient("https://bl360x.com/BLEFutureAPI/api/");
            var request = new RestRequest("Reconciliation/PorpergateStockTransactions", Method.Post);
            request.AddHeader("Timestamp", Timestamp);
            request.AddHeader("Authorization", "Bearer 6a92fb8b0532d2370aef1f912f72568dcda21c6853a6dbc2be531fcb02002e5c");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", JsonConvert.SerializeObject(stockInjection), ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            if (response.IsSuccessful)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Functions
        private async Task GetOrderStatus(string FromDate, string ToDate)
        {
            IList<CodeBaseResponse> allStatus = await _orderManager.GetOrderStatus();
            OrderStatus = allStatus.Where(x => x.OurCode != "Reject").ToList();
            if (_order.Location.CodeKey > 11)
            {
                foreach (CodeBaseResponse item in OrderStatus)
                {
                    RequestParameters partner = new RequestParameters()
                    {
                        FromDate = FromDate,
                        ToDate = ToDate,
                        StatusKey = item.CodeKey,
                        LocationKey = _order.Location.CodeKey,
                        BUKy = _order.BU.CodeKey

                    };

                    int Count = await _orderManager.PartnerOrderCount(partner);
                    if (partner.StatusKey == item.CodeKey)
                    {
                        item.Count = Count;
                    }
                }
                UIStateChanged();
            }
            else
            {
                foreach (CodeBaseResponse item in OrderStatus)
                {
                    item.Count = 0;
                }
                UIStateChanged();
            }

        }

        private async Task GetAllOrder(int StatusKey, string OrderFromDate, string OrderToDate)
        {
            RequestParameters parameters = new RequestParameters()
            {
                LocationKey = _order.Location.CodeKey,
                StatusKey = StatusKey,
                FromDate = OrderFromDate,
                ToDate = OrderToDate,
                BUKy = _order.BU.CodeKey
            };

            ListOfOrders.Clear();
            IList<KeyValuePair<string, IBLUIOperationHelper>> pairs = _objectHelpers.ToList();

            foreach (KeyValuePair<string, IBLUIOperationHelper> helper in pairs)
            {
                await helper.Value.Refresh();

            }
            ListOfOrders = await _orderManager.GetAllPartnerOrder(parameters);


        }

        private async Task GetIncomingOrder(int StatusKey, string OrderFromDate, string OrderToDate)
        {
            RequestParameters parameters = new RequestParameters()
            {
                LocationKey = _order.Location.CodeKey,
                StatusKey = StatusKey,
                FromDate = OrderFromDate,
                ToDate = OrderToDate,
                BUKy = _order.BU.CodeKey
            };

            ListOfIncomingOrders = await _orderManager.GetAllPartnerOrder(parameters);

        }

        private async Task GetRecentPickmeOrders()
        {
            RequestParameters parameters = new RequestParameters()
            {
                LocationKey = _order.Location.CodeKey,
                PlatformName = "Pickme",
            };

            NewPickmeOrders = await _orderManager.GetRecentlyAddedPickmeOrders(parameters);

        }

        public async Task GetOrderByOrderKey(int OrderKey)
        {
            RequestParameters parameters = new RequestParameters()
            {
                OrderKey = OrderKey
            };

            _order = await _orderManager.GetPartnerOrdersByOrderKy(parameters);

        }

        private async void LoadFoundOrder(RequestParameters Filterparams)
        {
            HideAllPopups();
            appStateService.IsLoaded = true;
            if (Filterparams.LocationKey < 11)
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Please select the location", Severity.Info);
            }
            else
            {
                requestParameters.FromDate = Filterparams.FromDate;
                requestParameters.ToDate = Filterparams.ToDate;
                requestParameters.StatusKey = OrderStatus.Where(x => x.OurCode == "InCmng").FirstOrDefault().CodeKey;
                await GetOrderStatus(requestParameters.FromDate, requestParameters.ToDate);
                await GetAllOrder(requestParameters.StatusKey, requestParameters.FromDate, requestParameters.ToDate);
                //if (_blTb != null)
                //{
                //    _blTb.Refresh();
                //}
            }

            appStateService.IsLoaded = false;
            UIStateChanged();
        }

        private async void RefreshIncomingOrder()
        {
            HideNotificationPopup();
            appStateService.IsLoaded = true;
            requestParameters.FromDate = DateTime.Now.ToString("yyyy/MM/dd");
            requestParameters.ToDate = DateTime.Now.ToString("yyyy/MM/dd");
            requestParameters.StatusKey = OrderStatus.Where(x => x.OurCode == "InCmng").FirstOrDefault().CodeKey;
            await GetOrderStatus(requestParameters.FromDate, requestParameters.ToDate);
            await GetAllOrder(requestParameters.StatusKey, requestParameters.FromDate, requestParameters.ToDate);
            //if (_blTb != null)
            //{
            //    _blTb.Refresh();
            //}
            tabs.ActivatePanel(0);
            appStateService.IsLoaded = false;
            UIStateChanged();
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

        private void ComboRefresh(string name)
        {
            IBLUIOperationHelper helper;
            if (_objectHelpers.TryGetValue(name, out helper))
            {
                helper.Refresh();
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

        #region PopupEvents
        private void HideAllPopups()
        {
            isOpenMoreInfo = false;
            isOpenFilter = false;
            isOpenEdit = false;
            isOpenUber = false;
            UIStateChanged();
        }

        private void HideNotificationPopup()
        {
            isOpenNotification = false;
            UIStateChanged();
        }

        #endregion
    }
}
