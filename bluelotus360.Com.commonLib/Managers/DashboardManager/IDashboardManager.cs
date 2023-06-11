using BL10.CleanArchitecture.Domain.Entities.Dashboard;
using BlueLotus360.CleanArchitecture.Domain.Entities.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL10.CleanArchitecture.Domain.Entities.Dashboard;

namespace bluelotus360.Com.commonLib.Managers.DashboardManager
{
    public interface IDashboardManager : IManager
    {
        bool IsExceptionthrown();
        //Task<IResult<DashboardDataResponse>> GetDataAsync();

        Task<IList<LocationViseStockResponse>> GetLocationViseStocks(LocationViseStockRequest request);

        Task<IList<SalesDetails>> GetSalesDetails(SalesDetails request);

        Task<IList<SalesByLocationResponse>> GetLocationWiseSalesDetails(SalesDetails request);

        Task<IList<SalesRepDetailsForSalesByLocationResponse>> GetLocationWiseSalesRepDetails(SalesRepDetailsForSalesByLocation request);


        Task<IList<ActualVsBudgetedIncomeResponse>> GetActualVsBudgetedIncome(FinanceRequest request);

        Task<IList<GPft_NetPft_Margin_Response>> GetGPft_NetPft_Margin(FinanceRequest request);

        Task<IList<GPft_NetPft_DT>> Get_Monthly_GPft_NetPft_DT(FinanceRequest request);

        Task<IList<Debtors_Creditors_Age_Analysis>> Get_Debtors_Age_Analysis(FinanceRequest request);
        Task<IList<Debtors_Creditors_Age_Analysis_DT>> Get_Debtors_Age_Analysis_DT(FinanceRequestDT request);
        Task<IList<Debtors_Creditors_Age_Analysis>> Get_Creditors_Age_Analysis(FinanceRequest request);
        Task<IList<Debtors_Creditors_Age_Analysis_DT>> Get_Creditors_Age_Analysis_DT(FinanceRequestDT request);
        Task<IList<Debtors_Creditors_Age_Analysis>> Get_Debtors_Age_Analysis_Overdue(FinanceRequest request);
        Task<IList<Debtors_Creditors_Age_Analysis_DT>> Get_Debtors_Age_Analysis_Overdue_DT(FinanceRequestDT request);
        Task<IList<Debtors_Creditors_Age_Analysis>> Get_Creditors_Age_Analysis_Overdue(FinanceRequest request);
        Task<IList<Debtors_Creditors_Age_Analysis_DT>> Get_Creditors_Age_Analysis_Overdue_DT(FinanceRequestDT request);
        Task<IList<Debtors_Creditors_Transaction_Details>> Get_Debtor_Creditor_DT_Transaction_Details(FinanceRequestDTDetails filter);
        Task<FinanceChartCombinedModel> GetCombinedFinanceChart(FinanceRequest filter);
        Task<IList<AuditTrail>> GetAuditTrailDetails(AuditTrail request);
        Task<IList<AuditTrailEnterdUpdatedResponse>> GetAuditTrailUpdatedDetails(AuditTrail request);
        Task<IList<AuditTrailEnterdUpdatedResponse>> GetAuditTrailEnterdDetails(AuditTrail request);


        //Sales and productivity dashboard
        Task<SummaryInfo> SalesAndProductivityDashboard_GetSummaryDetails();
        Task<RouteSummary> SalesAndProductivityDashboard_GetRouteDetails();
        Task<ShopsForRoute> SalesAndProductivityDashboard_GetDetailsByShop(int route);
        Task<SalesAndProductivityCombinedModelForMainCharts> SalesAndProductivityDashboard_GetPrimaryChartDetails(SalesAndProductivityFilter request);
        Task<ItemCategoryDetails> SalesAndProductivityDashboard_GetItemCategoryDetails(SalesAndProductivityFilter request);
    }
}
