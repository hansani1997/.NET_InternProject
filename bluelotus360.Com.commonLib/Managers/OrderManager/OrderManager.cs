using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using bluelotus360.Com.commonLib.Routes;
using BlueLotus360.Com.Shared.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using BL10.CleanArchitecture.Shared.Constants;
using BL10.CleanArchitecture.Domain.Entities.APIInfo;
using BL10.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.Infrastructure.OrderPlatforms;
using static BlueLotus360.Com.Infrastructure.OrderPlatforms.Ubereats.UberEntities.UberMenuItems;
using BlueLotus360.Com.Shared.Constants.Storage;
using RestSharp;
using bluelotus360.Com.MauiSupports.Services.MAUISecureStorage;
using bluelotus360.Com.MauiSupports.Services.ConnectionStates;
using bluelotus360.Com.MauiSupports.Models;
using bluelotus360.Com.MauiSupports.Services.SqliteStorageServices;
using System.Reflection.Metadata;
using bluelotus360.Com.commonLib.Services.Definition;

namespace bluelotus360.Com.commonLib.Managers.OrderManager
{
    public class SavableContent
    {
        public long OrderKey { get; set; }
        public long FormObjectKey { get; set; } = 1;
    }
    public class OrderManager:IOrderManager
    {
        private readonly HttpClient _httpClient;
        //private readonly ISecureStorageService _localStorage;
        private readonly IStorageService _localStorage;
        private readonly RestClient _client;
        private readonly IConnectionState _connectionState;
        private readonly ISqliteStorageService _sqliteStorageService;
        public OrderManager(HttpClient httpClient, IStorageService localStorage, RestClient client, IConnectionState connectionState,ISqliteStorageService sqliteStorageService)
        {
            _sqliteStorageService = sqliteStorageService;
            _httpClient = httpClient;
            _localStorage= localStorage;
            _connectionState= connectionState;
            _client = client;
            if (_httpClient.DefaultRequestHeaders.Count() == 0)
                _httpClient.DefaultRequestHeaders.Add("IntegrationID", GlobalConsts.intergrationId);
        }

        public async Task SaveOrder(Order order)
        {
            try
            {

                //var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.OrderSaveEndpoint, order);
                //var ser = await response.Content.ReadFromJsonAsync<OrderSaveResponse>();
                //order.OrderNumber = ser.OrderNumber;
                //order.Prefix = ser.Prefix;
                //order.OrderKey = ser.OrderKey;
                if (_connectionState.IsConnected())
                {
                    RestRequest request = new RestRequest(TokenEndpoints.OrderSaveEndpoint).AddJsonBody(order);
                    request.Method = Method.Post;
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Accept", "application/json");
                    string authToken = await _localStorage.GetItem(StorageConstants.Local.AuthToken);
                    if (authToken.StartsWith("\"") && authToken.EndsWith("\""))
                    {
                        authToken = authToken.Substring(1, authToken.Length - 2);
                    }
                    request.AddHeader("Authorization", "Bearer " + authToken);
                    request.AddHeader("IntegrationID", GlobalConsts.intergrationId);
                    RestResponse response = await _client.PostAsync(request);
                    string content = response.Content.ToString();
                    //    string content = await response.Content.ReadAsStringAsync();
                    var list = JsonConvert.DeserializeObject<OrderSaveResponse>(content);
                    order.OrderNumber = list.OrderNumber;
                    order.Prefix = list.Prefix;
                    order.OrderKey = list.OrderKey;
                }
                else
                {
                    string uid = await _localStorage.GetItem("UID");
                    string cid = await _localStorage.GetItem("CID");
                    int uId = Int32.Parse(uid);
                    int cId = Int32.Parse(cid);
                    RequestQueueItem item = new RequestQueueItem();
                    item.url = "Order.CreateGenericOrder";
                    item.timeStamp = DateTime.Now.ToString();
                    item.User = uId;
                    item.Company = cId;
                    item.requestBody = JsonConvert.SerializeObject(order);
                    await _sqliteStorageService.SaveQueuItemAsync(item);
                    order.OrderNumber = "Offline";
                }

            }
            catch (Exception exp)
            {
                order.OrderNumber = "ERR";
            }
        }

        public async Task EditOrder(Order order)
        {

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.OrderEditEndpoint, order);
                var ser = await response.Content.ReadFromJsonAsync<OrderSaveResponse>();
                order.OrderNumber = ser.OrderNumber;
                order.Prefix = ser.Prefix;
                order.OrderKey = ser.OrderKey;


            }
            catch (Exception exp)
            {

            }
        }

        public async Task<IList<OrderFindResults>> FindOrders(OrderFindDto request, URLDefinitions uRL)
        {
            IList<OrderFindResults> findOrders = new List<OrderFindResults>();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.FindOrder, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<IList<OrderFindResults>>(content);
                findOrders = obj;


            }
            catch (Exception exp)
            {

            }
            return findOrders;
        }

        public async Task<Order> OpenOrder(OrderOpenRequest request)
        {
            Order loaded_order = new Order();
            try
            {
                if (_connectionState.IsConnected())
                {
                    var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.LoadOrderEndPoint, request);
                    await response.Content.LoadIntoBufferAsync();
                    string content = response.Content.ReadAsStringAsync().Result;
                    loaded_order = JsonConvert.DeserializeObject<Order>(content);
                }
                else
                {
                    loaded_order = new Order();
                }
            }
            catch (Exception exp)
            {
                loaded_order = new Order();
            }
            return loaded_order;
        }

        public async Task<Order> OpenQuotation(OrderOpenRequest request)
        {
            Order loaded_order = new Order();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.LoadOrderEndPointFromQuotation, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                loaded_order = JsonConvert.DeserializeObject<Order>(content);



            }
            catch (Exception exp)
            {

            }
            return loaded_order;
        }
        public async Task<IList<GetFromQuotResults>> FindFromQuotation(GetFromQuoatationDTO request, URLDefinitions uRL)
        {
            IList<GetFromQuotResults> findOrders = new List<GetFromQuotResults>();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.FindFromQuotation, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<IList<GetFromQuotResults>>(content);
                findOrders = obj;


            }
            catch (Exception exp)
            {

            }
            return findOrders;
        }

        public async Task<Order> OpenQuotationAsSalesOrder(OrderOpenRequest request)
        {
            Order loaded_order = new Order();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.LoadOrderEndPoint, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                loaded_order = JsonConvert.DeserializeObject<Order>(content);



            }
            catch (Exception exp)
            {

            }
            return loaded_order;
        }

        public async Task<IList<OrderFindResults>> LoadOrderApprovals(OrderFindDto request)
        {
            IList<OrderFindResults> list = new List<OrderFindResults>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.LoadOrderApprovalDetails, request);
                await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                list = JsonConvert.DeserializeObject<IList<OrderFindResults>>(content);


            }
            catch (Exception exp)
            {

            }
            return list;
        }

        public async Task UpadteOrderApprovals(OrderFindResults request)
        {

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.UpdateOrderApproval, request);



            }
            catch (Exception exp)
            {

            }

        }

        public async Task<StockAsAtResponse> GetAvailableStock(StockAsAtRequest request)
        {
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetAvailableStockEndpoint, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<StockAsAtResponse>(content);

                return obj;


            }
            catch (Exception exp)
            {
                return new StockAsAtResponse();
            }
        }

        public async Task<OrderTranApprovestatus> CheckAddeditPermissionForAddEditOrdTrn(Order request)
        {
            OrderTranApprovestatus per = new OrderTranApprovestatus();
            try
            {
                string uid = await _localStorage.GetItem("UID");
                string cid = await _localStorage.GetItem("CID");
                int uId = Int32.Parse(uid);
                int cId = Int32.Parse(cid);
                SavableContent savCont = new SavableContent();
                savCont.FormObjectKey = request.FormObjectKey;
                savCont.OrderKey = request.OrderKey;
                string content = "";
                if (_connectionState.IsConnected())
                {
                    var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.CheckAddeditPermissionForAddEditOrdTrnEndPoint, request);
                    await response.Content.LoadIntoBufferAsync();
                    content = await response.Content.ReadAsStringAsync();
                    IncomingStrings insStrings = new IncomingStrings();
                    insStrings.name = TokenEndpoints.CheckAddeditPermissionForAddEditOrdTrnEndPoint;
                    insStrings.parameters = JsonConvert.SerializeObject(savCont);
                    insStrings.response = content;
                    insStrings.timestamp = DateTime.Now;
                    insStrings.user = uId;
                    insStrings.company = cId;
                    await _sqliteStorageService.SaveItemAsync(insStrings);
                }
                else
                {
                    IncomingStrings ins = await _sqliteStorageService.GetItemAsync(cId, uId, TokenEndpoints.CheckAddeditPermissionForAddEditOrdTrnEndPoint, JsonConvert.SerializeObject(savCont));
                    content = ins.response;
                }
                per = JsonConvert.DeserializeObject<OrderTranApprovestatus>(content);

            }
            catch (Exception exp)
            {

            }
            return per;
        }

        public async Task<PartnerOrder> GetLastSyncTime(APIRequestParameters request)
        {
            PartnerOrder orders = new PartnerOrder();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetLastSyncTime, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<PartnerOrder>(content);
                orders = obj;


            }
            catch (Exception exp)
            {

            }
            return orders;
        }

        public async Task<bool> OrderplatformOrder_Findweb(RequestParameters request)
        {
            bool success = false;
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.OrderplatformOrder_Findweb, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<bool>(content);
                success = obj;


            }
            catch (Exception exp)
            {

            }
            return success;
        }

        public async Task<CodeBaseResponse> GetOrderStatusByPartnerStatus(CodeBaseResponse request)
        {
            CodeBaseResponse orders = new CodeBaseResponse();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetOrderStatusByPartnerStatus, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<CodeBaseResponse>(content);
                orders = obj;


            }
            catch (Exception exp)
            {

            }
            return orders;
        }

        public async Task<IList<CodeBaseResponse>> GetOrderStatus()
        {
            IList<CodeBaseResponse> Orderstatus = new List<CodeBaseResponse>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetOrderStatus, new ComboRequestDTO());
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<IList<CodeBaseResponse>>(content);
                Orderstatus = obj;


            }
            catch (Exception exp)
            {

            }
            return Orderstatus;
        }

        public async Task<int> PartnerOrderCount(RequestParameters partnerOrder)
        {
            int Count = 0;
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.PartnerOrderCount, partnerOrder);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<int>(content);
                Count = obj;


            }
            catch (Exception exp)
            {

            }
            return Count;
        }

        public async Task<IList<PartnerOrder>> GetAllPartnerOrder(RequestParameters partnerOrder)
        {
            IList<PartnerOrder> orders = new List<PartnerOrder>();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetAllPartnerOrders, partnerOrder);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<IList<PartnerOrder>>(content);
                orders = obj;


            }
            catch (Exception exp)
            {

            }
            return orders;
        }

    

       
        public async Task<PartnerOrder> SavePartnerOrders(PartnerOrder request)
        {
            PartnerOrder orders = new PartnerOrder();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.SavePartnerOrder, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<PartnerOrder>(content);
                orders = obj;


            }
            catch (Exception exp)
            {

            }
            return orders;
        }

        public async Task<ItemResponse> GetItemByItemCode(ItemResponse request)
        {
            ItemResponse orders = new ItemResponse();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetItemsByItemCode, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ItemResponse>(content);
                orders = obj;


            }
            catch (Exception exp)
            {

            }
            return orders;
        }

        public async Task<PartnerOrder> GetPartnerOrdersByOrderKy(RequestParameters request)
        {
            PartnerOrder orders = new PartnerOrder();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetPartnerOrdersByOrderKy, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<PartnerOrder>(content);
                orders = obj;


            }
            catch (Exception exp)
            {

            }
            return orders;
        }

        public async Task<bool> InsertLastOrderSync(RequestParameters request)
        {
            bool success = false;
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.InsertLastOrderSync, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<bool>(content);
                success = obj;


            }
            catch (Exception exp)
            {

            }
            return success;
        }

        public async Task<string> GenerateProvisionURL(APIRequestParameters request)
        {
            string Link = "";
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GenerateProvisionURL, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                Link = content;


            }
            catch (Exception exp)
            {

            }
            return Link;
        }
        public async Task<bool> UberProvision_InsertUpdate(APIInformation request)
        {
            bool success = false;
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.UberProvision_InsertUpdate, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<bool>(content);
                success = obj;


            }
            catch (Exception exp)
            {

            }
            return success;
        }

        public async Task<IList<OrderMenuConfiguration>> GetOrderMenuConfiguration()
        {
            IList<OrderMenuConfiguration> config = new List<OrderMenuConfiguration>();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetOrderMenuConfiguration, new OrderMenuConfiguration());
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<IList<OrderMenuConfiguration>>(content);
                config = obj;


            }
            catch (Exception exp)
            {

            }
            return config;
        }

        public async Task<bool> OrderMenuConfiguration_InsertUpdate(OrderMenuConfiguration request)
        {
            bool success = false;
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.OrderMenuConfiguration_InsertUpdate, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<bool>(content);
                success = obj;


            }
            catch (Exception exp)
            {

            }
            return success;
        }

        public async Task<bool> OrderHubStatus_UpdateWeb(RequestParameters request)
        {
            bool success = false;
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.OrderHubStatus_UpdateWeb, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<bool>(content);
                success = obj;


            }
            catch (Exception exp)
            {

            }
            return success;
        }

        public async Task<IList<PartnerMenuItem>> GetOrderItemsToUpload(RequestParameters request)
        {
            IList<PartnerMenuItem> config = new List<PartnerMenuItem>();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetAllOrderMenuItems, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<IList<PartnerMenuItem>>(content);
                config = obj;


            }
            catch (Exception exp)
            {

            }
            return config;
        }
        public async Task<IList<CodeBaseResponse>> GetNextOrderStatusByStatusKey(ComboRequestDTO request)
        {
            IList<CodeBaseResponse> Orderstatus = new List<CodeBaseResponse>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetNextOrderHubStatusByStatusKey, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<IList<CodeBaseResponse>>(content);
                Orderstatus = obj;


            }
            catch (Exception exp)
            {

            }
            return Orderstatus;
        }

        public async Task<IList<CodeBaseResponse>> GetOrderHubBU()
        {
            IList<CodeBaseResponse> BU = new List<CodeBaseResponse>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetOrderHubBU, "");
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<IList<CodeBaseResponse>>(content);
                BU = obj;


            }
            catch (Exception exp)
            {

            }
            return BU;
        }


        public async Task<IList<PartnerOrder>> GetAvailablePickmeOrders(RequestParameters request)
        {
            IList<PartnerOrder> order = new List<PartnerOrder>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetAvailablePickmeOrders, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<IList<PartnerOrder>>(content);
                order = obj;


            }
            catch (Exception exp)
            {

            }
            return order;
        }

        public async Task<bool> APIResponseDet_InsertWeb(ResponseDetails request)
        {
            bool success = false;
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.APIResponseDet_InsertWeb, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<bool>(content);
                success = obj;


            }
            catch (Exception exp)
            {

            }
            return success;
        }

        public async Task<int> GetPickMeOrderByOrderID(RequestParameters request)
        {
            int OrdKy = 1;
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetPickMeOrderByOrderID, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<int>(content);
                OrdKy = obj;


            }
            catch (Exception exp)
            {

            }
            return OrdKy;
        }

        public async Task<bool> UberMenu_DiscontinueWeb(UberDiscontinueItem request)
        {
            bool success;
            success = false;

            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.UberMenu_DiscontinueWeb, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<bool>(content);
                success = obj;


            }
            catch (Exception exp)
            {

            }
            return success;
        }

        public async Task<decimal> GetOrderHubItemRateByItemKy(RequestParameters request)
        {
            decimal ItemRate = 0;
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetOrderHubItemRateByItemKy, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<decimal>(content);
                ItemRate = obj;


            }
            catch (Exception exp)
            {

            }
            return ItemRate;
        }

        public async Task<bool> UnmappedSKUUpdate(PartnerOrder request)
        {
            bool success = false;
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.UnmappedSKUUpdate, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<bool>(content);
                success = obj;


            }
            catch (Exception exp)
            {

            }
            return success;
        }

        public async Task<bool> OrderItem_DeleteWeb(RequestParameters request)
        {
            bool success = false;
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.OrderItem_DeleteWeb, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<bool>(content);
                success = obj;


            }
            catch (Exception exp)
            {

            }
            return success;
        }
        public async Task<AvailableStock> GetAvailableQtyByItem(RequestParameters request)
        {
            AvailableStock success = new AvailableStock();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetAvailableQtyByItem, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<AvailableStock>(content);
                success = obj;


            }
            catch (Exception exp)
            {

            }
            return success;
        }

        public async Task<string> GetMissingUberOber(RequestParameters request)
        {
            string body = "";
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetMissingUberOber, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                body = content;


            }
            catch (Exception exp)
            {

            }
            return body;
        }

        public async Task<IList<PartnerOrder>> GetRecentlyAddedPickmeOrders(RequestParameters request)
        {
            IList<PartnerOrder> success = new List<PartnerOrder>();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetRecentlyAddedPickmeOrders, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<IList<PartnerOrder>>(content);
                success = obj;


            }
            catch (Exception exp)
            {

            }
            return success;
        }

        public async Task<string> GetPickmeOrdersByDuration(RequestParameters request)
        {
            string x = "";
            BlueLotus360.Com.Infrastructure.OrderPlatforms.PickMe.PickmeEntity.GetOrder success = new BlueLotus360.Com.Infrastructure.OrderPlatforms.PickMe.PickmeEntity.GetOrder();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetPickmeOrdersByDuration, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                // var obj = JsonConvert.DeserializeObject<BlueLotus360.Com.Infrastructure.OrderPlatforms.PickMe.PickmeEntity.GetOrder>(content);
                // success = obj;
                x = content;


            }
            catch (Exception exp)
            {

            }
            return x;
        }


    }
}
