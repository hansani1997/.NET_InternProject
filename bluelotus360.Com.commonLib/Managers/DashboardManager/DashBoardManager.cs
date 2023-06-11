using BL10.CleanArchitecture.Domain.Entities.Dashboard;
using bluelotus360.Com.commonLib.Routes;
using BlueLotus360.CleanArchitecture.Domain.Entities.Dashboard;
using BL10.CleanArchitecture.Domain.Entities.Dashboard;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using BL10.CleanArchitecture.Shared.Constants;

namespace bluelotus360.Com.commonLib.Managers.DashboardManager
{
    
    public class DashboardManager : IDashboardManager
    {
        private readonly HttpClient _httpClient;
        private bool _checkIfExceptionReturn;
        private readonly IHttpClientFactory _factory;
        public DashboardManager(HttpClient httpClient, IHttpClientFactory factory)
        {
            
            _httpClient = httpClient;
            _factory = factory;
        }

        private void AssignClientData(HttpClient cl)
        {
            cl.BaseAddress = _httpClient.BaseAddress;
            cl.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                _httpClient.DefaultRequestHeaders.Authorization.Scheme,
                _httpClient.DefaultRequestHeaders.Authorization.Parameter);
            cl.DefaultRequestHeaders.Add("IntegrationID", GlobalConsts.intergrationId);
        }

        public async Task<IList<ActualVsBudgetedIncomeResponse>> GetActualVsBudgetedIncome(FinanceRequest request)
        {
            List<ActualVsBudgetedIncomeResponse> details = new List<ActualVsBudgetedIncomeResponse>();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.GetActualVsBudgetedIncomeEndPoint, request);
                string content = await response.Content.ReadAsStringAsync();
                details = JsonConvert.DeserializeObject<List<ActualVsBudgetedIncomeResponse>>(content);
                
                
                // var request_client = new RestRequest(TokenEndpoints.GetActualVsBudgetedIncomeEndPoint);
                // request_client.AddJsonBody(request);
                //var response = await _restClient.PostAsync(request_client);
                //string content = response.Content.ToString();
                //details = JsonConvert.DeserializeObject<List<ActualVsBudgetedIncomeResponse>>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                details = new List<ActualVsBudgetedIncomeResponse>();
            }
            finally
            {

            }

            return details;
        }

        public async Task<IList<AuditTrail>> GetAuditTrailDetails(AuditTrail request)
        {
            IList<AuditTrail> details = new List<AuditTrail>();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.Get_AuditTrail_Details, request);
                string content = await response.Content.ReadAsStringAsync();
                details = JsonConvert.DeserializeObject<IList<AuditTrail>>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                details = new List<AuditTrail>();
                Console.WriteLine(exp.Message);
            }
            finally
            {

            }

            return details;
        }

        public async Task<IList<AuditTrailEnterdUpdatedResponse>> GetAuditTrailEnterdDetails(AuditTrail request)
        {
            IList<AuditTrailEnterdUpdatedResponse> responses = new List<AuditTrailEnterdUpdatedResponse>();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.Get_AuditTrail_Enterd_Details, request);
                string content = await response.Content.ReadAsStringAsync();
                responses = JsonConvert.DeserializeObject<IList<AuditTrailEnterdUpdatedResponse>>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                responses = new List<AuditTrailEnterdUpdatedResponse>();
            }
            finally
            {

            }

            return responses;
        }

        public async Task<IList<AuditTrailEnterdUpdatedResponse>> GetAuditTrailUpdatedDetails(AuditTrail request)
        {
            IList<AuditTrailEnterdUpdatedResponse> responses = new List<AuditTrailEnterdUpdatedResponse>();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.Get_AuditTrail_Updated_Details, request);
                string content = await response.Content.ReadAsStringAsync();
                responses = JsonConvert.DeserializeObject<IList<AuditTrailEnterdUpdatedResponse>>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                responses = new List<AuditTrailEnterdUpdatedResponse>();
            }
            finally
            {

            }

            return responses;
        }

        public async Task<IList<GPft_NetPft_Margin_Response>> GetGPft_NetPft_Margin(FinanceRequest request)
        {
            IList<GPft_NetPft_Margin_Response> details = new List<GPft_NetPft_Margin_Response>();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.GPft_NetPft_MarginEndPoint, request);
                string content = await response.Content.ReadAsStringAsync();
                details = JsonConvert.DeserializeObject<List<GPft_NetPft_Margin_Response>>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                details = new List<GPft_NetPft_Margin_Response>();
            }
            finally
            {

            }
            return details;
        }

        public async Task<IList<LocationViseStockResponse>> GetLocationViseStocks(LocationViseStockRequest request)
        {
            List<LocationViseStockResponse> stockList = new List<LocationViseStockResponse>();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.GetLocationViseStocks, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                stockList = JsonConvert.DeserializeObject<List<LocationViseStockResponse>>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                stockList = new List<LocationViseStockResponse>();
            }
            finally
            {

            }

            return stockList;
        }

        public async Task<IList<SalesByLocationResponse>> GetLocationWiseSalesDetails(SalesDetails request)
        {
            IList<SalesByLocationResponse> details = new List<SalesByLocationResponse>();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.GetSalesByLocationEndPoint, request);
                string content = await response.Content.ReadAsStringAsync();
                details = JsonConvert.DeserializeObject<List<SalesByLocationResponse>>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                details = new List<SalesByLocationResponse>();
            }
            finally
            {

            }
            return details;
        }

        public async Task<IList<SalesRepDetailsForSalesByLocationResponse>> GetLocationWiseSalesRepDetails(SalesRepDetailsForSalesByLocation request)
        {
            List<SalesRepDetailsForSalesByLocationResponse> details = new List<SalesRepDetailsForSalesByLocationResponse>();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.GetSalesByLocationRepEndPoint, request);
                string content = await response.Content.ReadAsStringAsync();
                details = JsonConvert.DeserializeObject<List<SalesRepDetailsForSalesByLocationResponse>>(content);
            }
            catch
            {
                _checkIfExceptionReturn = true;
                details = new List<SalesRepDetailsForSalesByLocationResponse>();
            }
            finally
            {

            }
            return details;
        }

        public async Task<IList<SalesDetails>> GetSalesDetails(SalesDetails request)
        {
            List<SalesDetails> details = new List<SalesDetails>();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.GetSalesHeaderDetails, request);
                string content = await response.Content.ReadAsStringAsync();
                details = JsonConvert.DeserializeObject<List<SalesDetails>>(content);
            }
            catch
            {
                _checkIfExceptionReturn = true;
                details = new List<SalesDetails>();
            }
            finally
            {

            }
            return details;
        }

        public async Task<IList<Debtors_Creditors_Age_Analysis>> Get_Creditors_Age_Analysis(FinanceRequest request)
        {
            List<Debtors_Creditors_Age_Analysis> details = new List<Debtors_Creditors_Age_Analysis>();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.Get_Creditors_Age_Analysis_EndPoint, request);
                string content = await response.Content.ReadAsStringAsync();
                details = JsonConvert.DeserializeObject<List<Debtors_Creditors_Age_Analysis>>(content);
            }
            catch
            {
                _checkIfExceptionReturn = true;
                details = new List<Debtors_Creditors_Age_Analysis>();
            }
            finally
            {

            }

            return details;
        }

        public async Task<IList<Debtors_Creditors_Age_Analysis_DT>> Get_Creditors_Age_Analysis_DT(FinanceRequestDT request)
        {
            IList<Debtors_Creditors_Age_Analysis_DT> details = new List<Debtors_Creditors_Age_Analysis_DT>();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.Get_Creditors_DT_EndPoint, request);
                string content = await response.Content.ReadAsStringAsync();
                details = JsonConvert.DeserializeObject<List<Debtors_Creditors_Age_Analysis_DT>>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                details = new List<Debtors_Creditors_Age_Analysis_DT>();
            }
            finally
            {

            }

            return details;
        }

        public async Task<IList<Debtors_Creditors_Age_Analysis>> Get_Creditors_Age_Analysis_Overdue(FinanceRequest request)
        {
            IList<Debtors_Creditors_Age_Analysis> details = new List<Debtors_Creditors_Age_Analysis>();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.Get_Creditors_Age_Analysis_Overdue_EndPoint, request);
                string content =await  response.Content.ReadAsStringAsync();
                details = JsonConvert.DeserializeObject<List<Debtors_Creditors_Age_Analysis>>(content);
            }
            catch
            {
                _checkIfExceptionReturn = true;
                details = new List<Debtors_Creditors_Age_Analysis>();
            }
            finally
            {

            }
            return details;
        }

        public async Task<IList<Debtors_Creditors_Age_Analysis_DT>> Get_Creditors_Age_Analysis_Overdue_DT(FinanceRequestDT request)
        {
            IList<Debtors_Creditors_Age_Analysis_DT> details = new List<Debtors_Creditors_Age_Analysis_DT>();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.Get_Creditors_Overdue_DT_EndPoint, request);
                string content = await response.Content.ReadAsStringAsync();
                details = JsonConvert.DeserializeObject<List<Debtors_Creditors_Age_Analysis_DT>>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                details = new List<Debtors_Creditors_Age_Analysis_DT>();
            }
            finally
            {

            }

            return details;
        }

        public async Task<IList<Debtors_Creditors_Age_Analysis>> Get_Debtors_Age_Analysis(FinanceRequest request)
        {
            IList<Debtors_Creditors_Age_Analysis> details = new List<Debtors_Creditors_Age_Analysis>();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.Get_Creditors_Age_Analysis_EndPoint, request);
                string content =await response.Content.ReadAsStringAsync();
                details = JsonConvert.DeserializeObject<List<Debtors_Creditors_Age_Analysis>>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                details = new List<Debtors_Creditors_Age_Analysis>();
            }
            finally
            { 

            }
            return details;
        }

        public async Task<IList<Debtors_Creditors_Age_Analysis_DT>> Get_Debtors_Age_Analysis_DT(FinanceRequestDT request)
        {
            IList<Debtors_Creditors_Age_Analysis_DT> details = new List<Debtors_Creditors_Age_Analysis_DT>();

            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.Get_Debtors_DT_EndPoint, request);
                string content = await response.Content.ReadAsStringAsync();
                details = JsonConvert.DeserializeObject<List<Debtors_Creditors_Age_Analysis_DT>>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                details = new List<Debtors_Creditors_Age_Analysis_DT>();
            }
            finally
            {

            }
            return details;
        }

        public async Task<IList<Debtors_Creditors_Age_Analysis>> Get_Debtors_Age_Analysis_Overdue(FinanceRequest request)
        {
            List<Debtors_Creditors_Age_Analysis> details = new List<Debtors_Creditors_Age_Analysis>();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.Get_Debtors_Age_Analysis_Overdue_EndPoint, request);
                string content = await response.Content.ReadAsStringAsync();
                details = JsonConvert.DeserializeObject<List<Debtors_Creditors_Age_Analysis>>(content);
            }
            catch
            {
                _checkIfExceptionReturn = true;
                details = new List<Debtors_Creditors_Age_Analysis>();
            }
            finally
            {

            }
            return details;
        }

        public async Task<IList<Debtors_Creditors_Age_Analysis_DT>> Get_Debtors_Age_Analysis_Overdue_DT(FinanceRequestDT request)
        {
            IList<Debtors_Creditors_Age_Analysis_DT> details = new List<Debtors_Creditors_Age_Analysis_DT>();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.Get_Debtors_Overdue_DT_EndPoint, request);
                string content = await response.Content.ReadAsStringAsync();
                details = JsonConvert.DeserializeObject<List<Debtors_Creditors_Age_Analysis_DT>>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                details = new List<Debtors_Creditors_Age_Analysis_DT>();
            }
            finally
            {

            }

            return details;
        }

        public async Task<IList<Debtors_Creditors_Transaction_Details>> Get_Debtor_Creditor_DT_Transaction_Details(FinanceRequestDTDetails filter)
        {
            IList<Debtors_Creditors_Transaction_Details> details = new List<Debtors_Creditors_Transaction_Details>();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.Get_Debtor_DT_Transaction_Details_EndPoint, filter);
                string content =await  response.Content.ReadAsStringAsync();
                details = JsonConvert.DeserializeObject<List<Debtors_Creditors_Transaction_Details>>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                details = new List<Debtors_Creditors_Transaction_Details>();
            }
            finally
            {

            }

            return details;
        }

        public async Task<IList<GPft_NetPft_DT>> Get_Monthly_GPft_NetPft_DT(FinanceRequest request)
        {
            IList<GPft_NetPft_DT> details = new List<GPft_NetPft_DT>();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.GPft_NetPft_DT_EndPoint, request);
                string content =await response.Content.ReadAsStringAsync();
                details = JsonConvert.DeserializeObject<List<GPft_NetPft_DT>>(content);
            }
            catch
            {
                _checkIfExceptionReturn = true;
                details = new List<GPft_NetPft_DT>();

            }
            finally
            {

            }

            return details;
        }

        public async Task<FinanceChartCombinedModel> GetCombinedFinanceChart(FinanceRequest filter)
        {
            FinanceChartCombinedModel details = new FinanceChartCombinedModel();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                  var response = await cl.PostAsJsonAsync(TokenEndpoints.Get_Combined_Finance_Chart_EndPoint, filter);
                string content =await response.Content.ReadAsStringAsync();
                details = JsonConvert.DeserializeObject<FinanceChartCombinedModel>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                details = new FinanceChartCombinedModel();
            }
            finally
            {
            }
            return details;
        }
        public bool IsExceptionthrown()
        {
            if (_checkIfExceptionReturn)
                return true;
            return false;
        }

        #region Sales and Productivity Dashboard

        
        public async Task<SummaryInfo> SalesAndProductivityDashboard_GetSummaryDetails(){
            SummaryInfo info= new SummaryInfo();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.GetAsync(TokenEndpoints.SalesAndProductivityDashboard_GetSummaryDetails);
                string content = await response.Content.ReadAsStringAsync();
                info = JsonConvert.DeserializeObject<SummaryInfo>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                info = new SummaryInfo();
                Console.WriteLine(exp.Message);
            }
            return info;
            //return new SummaryInfo();
        }
        public async Task<RouteSummary> SalesAndProductivityDashboard_GetRouteDetails(){
            RouteSummary info = new RouteSummary();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.GetAsync(TokenEndpoints.SalesAndProductivityDashboard_GetRouteDetails);
                string content = await response.Content.ReadAsStringAsync();
                info = JsonConvert.DeserializeObject<RouteSummary>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                info = new RouteSummary();
                Console.WriteLine(exp.Message);
            }
            return info;
            //return new RouteSummary();
        }
        public async Task<ShopsForRoute> SalesAndProductivityDashboard_GetDetailsByShop(int route){
            ShopsForRoute info = new ShopsForRoute();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.SalesAndProductivityDashboard_GetShopDetailsForRoute, route);
                string content = await response.Content.ReadAsStringAsync();
                info = JsonConvert.DeserializeObject<ShopsForRoute>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                info = new ShopsForRoute();
                Console.WriteLine(exp.Message);
            }
            return info;
        }
        
        public async Task<SalesAndProductivityCombinedModelForMainCharts> SalesAndProductivityDashboard_GetPrimaryChartDetails(SalesAndProductivityFilter request)
        {
            SalesAndProductivityCombinedModelForMainCharts financeChartCombinedModel = new SalesAndProductivityCombinedModelForMainCharts();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.SalesAndProductivityDashboard_GetPrimaryChartDetails, request);
                string content = await response.Content.ReadAsStringAsync();
                financeChartCombinedModel = JsonConvert.DeserializeObject<SalesAndProductivityCombinedModelForMainCharts>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                financeChartCombinedModel = new SalesAndProductivityCombinedModelForMainCharts();
                Console.WriteLine(exp.Message);
            }
            return financeChartCombinedModel;
        }
        public async Task<ItemCategoryDetails> SalesAndProductivityDashboard_GetItemCategoryDetails(SalesAndProductivityFilter request)
        {
            ItemCategoryDetails details = new ItemCategoryDetails();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.SalesAndProductivityDashboard_GetItemCategoryDetails, request);
                string content = await response.Content.ReadAsStringAsync();
                details = JsonConvert.DeserializeObject<ItemCategoryDetails>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                details = new ItemCategoryDetails();
                Console.WriteLine(exp.Message);
            }
            return details;
        }
        #endregion
    }
}
