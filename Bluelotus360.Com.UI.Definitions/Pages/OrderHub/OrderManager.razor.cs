using BL10.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.HR;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.Com.Infrastructure.Managers.APIManager;
using BlueLotus360.Com.Infrastructure.Managers.OrderManager;
using BlueLotus360.Com.Infrastructure.OrderPlatforms.PickMe;
using BlueLotus360.Com.UI.Definitions.Extensions;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components.TelerikComponents.Grid;
using BlueLotus360.Com.UI.Definitions.Pages.OrderHub.Component;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlueLotus360.Com.Infrastructure.OrderPlatforms.PickMe.PickmeEntity;

namespace BlueLotus360.Com.UI.Definitions.Pages.OrderHub
{
    public partial class OrderManager
    {
        #region parameter

        private BLUIElement formDefinition;
        private BLUIElement Grid;
        private BLUIElement HeaderSection;
        private BLUIElement FilterSection;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
        private UIBuilder _refBuilder;
        private BLTelGrid<PartnerOrder> _blTb;
        private UserMessageDialog _refUserMessage;


        private bool fixedheader = true;
        private bool IsProcessing;
        private bool IsDataLoading;
        private PartnerOrder _order;
        private PartnerOrder _SelectedOrder;
        public IList<CodeBaseResponse> OrderStatus;
        private IList<PartnerOrder> ListOfOrders;
        private RequestParameters requestParameters;
        private bool isOpenMoreInfo = false;
        private bool isOpenFilter = false;
        GetMoreOrderInformation moredata = new GetMoreOrderInformation();
        FilterOrder _FilterOrderData = new FilterOrder();
        UserMessageManager Messages = new UserMessageManager();
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
            requestParameters = new RequestParameters();
            _blTb = new BLTelGrid<PartnerOrder>();
            _SelectedOrder = new PartnerOrder();
            HookInteractions();
            if (formDefinition != null)
            {
                Grid = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("DetailTable")).FirstOrDefault();
                FilterSection = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("FilterPopup")).FirstOrDefault();
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
            appStateService._AppBarName = "Orders";
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

        private void StatusOnChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            //_SelectedOrder.OrderStatus = args.DataObject;
            UIStateChanged();
        }


        private async void TabClick(int args)
        {
            requestParameters.StatusKey = args;
            // GetOrderStatus(requestParameters.FromDate, requestParameters.ToDate);
            await GetAllOrder(requestParameters.StatusKey, requestParameters.FromDate, requestParameters.ToDate);
            UIStateChanged();
        }

        private void TodayClick(UIInterectionArgs<object> args)
        {
            requestParameters.FromDate = DateTime.Now.ToString("yyyy/MM/dd");
            requestParameters.ToDate = DateTime.Now.ToString("yyyy/MM/dd");
            GetOrderStatus(requestParameters.FromDate, requestParameters.ToDate);
            UIStateChanged();
        }

        private void YesterdayClick(UIInterectionArgs<object> args)
        {
            requestParameters.FromDate = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
            requestParameters.ToDate = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
            GetOrderStatus(requestParameters.FromDate, requestParameters.ToDate);
            UIStateChanged();
        }

        private void WeekClick(UIInterectionArgs<object> args)
        {
            DateTime baseDate = DateTime.Now;
            requestParameters.FromDate = baseDate.AddDays(-(int)baseDate.DayOfWeek).ToString("yyyy/MM/dd");
            requestParameters.ToDate = Convert.ToDateTime(requestParameters.FromDate).AddDays(7).AddSeconds(-1).ToString("yyyy/MM/dd");
            GetOrderStatus(requestParameters.FromDate, requestParameters.ToDate);
            UIStateChanged();
        }

        private void MonthClick(UIInterectionArgs<object> args)
        {
            DateTime baseDate = DateTime.Now;
            requestParameters.FromDate = baseDate.AddDays(1 - baseDate.Day).ToString("yyyy/MM/dd");
            requestParameters.ToDate = Convert.ToDateTime(requestParameters.FromDate).AddMonths(1).AddSeconds(-1).ToString("yyyy/MM/dd");
            GetOrderStatus(requestParameters.FromDate, requestParameters.ToDate);
            UIStateChanged();
        }

        private void CustomClick(UIInterectionArgs<object> args)
        {
            isOpenFilter = true;
            UIStateChanged();
        }

        private async void MoreInfoClick(UIInterectionArgs<object> args)
        {
            PartnerOrder order = args.DataObject as PartnerOrder;
            await GetOrderByOrderKey(Convert.ToInt32(order.PartnerOrderId));
            isOpenMoreInfo = true;
            moredata.GetSingleOrder(_order);

            UIStateChanged();
        }
        //private void FilterOrders(UIInterectionArgs<object> args)
        //{
        //    isOpenFilter = true;
        //    UIStateChanged();
        //}


        private async void SyncOrders(UIInterectionArgs<object> args)
        {
            appStateService.IsLoaded = true;
            PickmeAPIHandler pickmeAPIHandler = new PickmeAPIHandler(_apiManager, _orderManager, _addressManager);
            GetOrder pickmeOrder = await pickmeAPIHandler.GetPickmeOrder(_order.Location.CodeKey);

            if (pickmeOrder.Data.Count == 0)
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("There is no data to sync", Severity.Info);
            }
            else
            {
                bool isDataSaved = await pickmeAPIHandler.SavePickmeOrder(pickmeOrder, _order.Location.CodeKey, requestParameters.FromDate, requestParameters.ToDate);
                if (isDataSaved)
                {
                    RequestParameters param = new RequestParameters()
                    {
                        LocationKey = _order.Location.CodeKey,
                        StatusKey = 1,
                        FromDate = requestParameters.FromDate,
                        ToDate = requestParameters.ToDate,
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
            if (_blTb != null)
            {
                _blTb.Refresh();
            }
            UIStateChanged();
        }
        #endregion

        #region Functions
        private async Task GetOrderStatus(string FromDate, string ToDate)
        {
            OrderStatus = await _orderManager.GetOrderStatus();
            if (_order.Location.CodeKey > 11)
            {
                foreach (CodeBaseResponse item in OrderStatus)
                {
                    RequestParameters partner = new RequestParameters()
                    {
                        FromDate = FromDate,
                        ToDate = ToDate,
                        StatusKey = item.CodeKey,
                        LocationKey = _order.Location.CodeKey

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
                ToDate = OrderToDate
            };

            ListOfOrders = await _orderManager.GetAllPartnerOrder(parameters);

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
                if (_blTb != null)
                {
                    _blTb.Refresh();
                }
            }


            UIStateChanged();
        }

        #endregion

        #region PopupEvents
        private void HideAllPopups()
        {
            isOpenMoreInfo = false;
            isOpenFilter = false;
            UIStateChanged();
        }

        #endregion
    }
}
