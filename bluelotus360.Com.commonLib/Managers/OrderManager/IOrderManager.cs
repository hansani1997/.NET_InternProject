using BL10.CleanArchitecture.Domain.Entities.APIInfo;
using BL10.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueLotus360.Com.Infrastructure.OrderPlatforms;
using static BlueLotus360.Com.Infrastructure.OrderPlatforms.Ubereats.UberEntities.UberMenuItems;

namespace bluelotus360.Com.commonLib.Managers.OrderManager
{
    public interface IOrderManager:IManager
    {
        Task SaveOrder(Order order);
        Task<IList<OrderFindResults>> FindOrders(OrderFindDto request, URLDefinitions uRL);
        Task<Order> OpenOrder(OrderOpenRequest request);
        Task EditOrder(Order order);
        Task<IList<GetFromQuotResults>> FindFromQuotation(GetFromQuoatationDTO request, URLDefinitions uRL);
        Task<Order> OpenQuotation(OrderOpenRequest request);
        Task<IList<OrderFindResults>> LoadOrderApprovals(OrderFindDto request);
        Task<StockAsAtResponse> GetAvailableStock(StockAsAtRequest request);
        Task UpadteOrderApprovals(OrderFindResults request);
        Task<OrderTranApprovestatus> CheckAddeditPermissionForAddEditOrdTrn(Order request);
        Task<PartnerOrder> GetLastSyncTime(APIRequestParameters request);
        Task<bool> OrderplatformOrder_Findweb(RequestParameters request);
        Task<CodeBaseResponse> GetOrderStatusByPartnerStatus(CodeBaseResponse request);
        Task<IList<CodeBaseResponse>> GetOrderStatus();
        Task<int> PartnerOrderCount(RequestParameters partnerOrder);
        Task<IList<PartnerOrder>> GetAllPartnerOrder(RequestParameters partnerOrder);
        Task<PartnerOrder> SavePartnerOrders(PartnerOrder request);
        Task<ItemResponse> GetItemByItemCode(ItemResponse request);
        Task<PartnerOrder> GetPartnerOrdersByOrderKy(RequestParameters request);
        Task<bool> InsertLastOrderSync(RequestParameters request);
        Task<string> GenerateProvisionURL(APIRequestParameters request);
        Task<bool> UberProvision_InsertUpdate(APIInformation request);
        Task<IList<OrderMenuConfiguration>> GetOrderMenuConfiguration();
        Task<bool> OrderMenuConfiguration_InsertUpdate(OrderMenuConfiguration request);
        Task<bool> OrderHubStatus_UpdateWeb(RequestParameters request);
        Task<IList<PartnerMenuItem>> GetOrderItemsToUpload(RequestParameters request);
        Task<IList<CodeBaseResponse>> GetNextOrderStatusByStatusKey(ComboRequestDTO request);
        Task<IList<CodeBaseResponse>> GetOrderHubBU();
        Task<IList<PartnerOrder>> GetAvailablePickmeOrders(RequestParameters request);
        Task<bool> APIResponseDet_InsertWeb(ResponseDetails request);
        Task<int> GetPickMeOrderByOrderID(RequestParameters request);
        Task<bool> UberMenu_DiscontinueWeb(UberDiscontinueItem request);
        Task<decimal> GetOrderHubItemRateByItemKy(RequestParameters request);
        Task<bool> UnmappedSKUUpdate(PartnerOrder request);
        Task<bool> OrderItem_DeleteWeb(RequestParameters request);
        Task<AvailableStock> GetAvailableQtyByItem(RequestParameters request);
        Task<string> GetMissingUberOber(RequestParameters request);
        Task<IList<PartnerOrder>> GetRecentlyAddedPickmeOrders(RequestParameters request);
        // Task<BlueLotus360.Com.Infrastructure.OrderPlatforms.PickMe.PickmeEntity.GetOrder> GetPickmeOrdersByDuration(RequestParameters request);
        Task<string> GetPickmeOrdersByDuration(RequestParameters request);
    }
}
