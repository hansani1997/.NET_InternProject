using BL10.CleanArchitecture.Domain.Entities.Transaction;
using BL10.CleanArchitecture.Domain.Entities.Validation;
using BL10.CleanArchitecture.Shared.Constants;
using bluelotus360.Com.commonLib.Routes;
using bluelotus360.Com.commonLib.Services.Definition;
using bluelotus360.Com.MauiSupports.Models;
using bluelotus360.Com.MauiSupports.Services.ConnectionStates;
using bluelotus360.Com.MauiSupports.Services.MAUISecureStorage;
using bluelotus360.Com.MauiSupports.Services.SqliteStorageServices;
using BlueLotus360.CleanArchitecture.Application.Responses.ServerResponse;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.Com.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace bluelotus360.Com.commonLib.Managers.TransactionManager
{
    public class TransactionManager:ITransactionManager
    {
        private readonly HttpClient _httpClient;
        // private readonly ISecureStorageService _localStorage;
        private readonly IStorageService _localStorage;
        private  readonly AuthenticationStateProvider _authenticationStateProvider;
        private ISqliteStorageService _sqliteStorageService;
        private IConnectionState _connectionState;
        private ISnackbar _snackBar;
        private IItemStockAsAtStore _itemStockAsAtStore;
        private IComboDataManager _comboDataManager;
        private readonly IHttpClientFactory _factory;
        public TransactionManager(HttpClient httpClient,IComboDataManager comboDataManager,
            IItemStockAsAtStore stockAsAtStore,
            IStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider,
            ISqliteStorageService sqliteStorageService,
            IConnectionState connectionStat,
            ISnackbar snackbar,
            IHttpClientFactory factory)
        {
            _sqliteStorageService= sqliteStorageService;
            _httpClient = httpClient;
            _snackBar = snackbar;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
            _comboDataManager = comboDataManager;
            _connectionState = connectionStat;
            _itemStockAsAtStore = stockAsAtStore;
            _factory = factory;

            if (_httpClient.DefaultRequestHeaders.Count() == 0)
                _httpClient.DefaultRequestHeaders.Add("IntegrationID", GlobalConsts.intergrationId);
        }

        private void AssignClientData(HttpClient cl)
        {
            cl.BaseAddress = _httpClient.BaseAddress;
            cl.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                _httpClient.DefaultRequestHeaders.Authorization.Scheme,
                _httpClient.DefaultRequestHeaders.Authorization.Parameter);
            cl.DefaultRequestHeaders.Add("IntegrationID", GlobalConsts.intergrationId);
        }
        public async Task<FindTransactionResponse> FindTransactions(TransactionFindRequest request, URLDefinitions uRL)
        {
            FindTransactionResponse findTransaction = new FindTransactionResponse();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.FindTransaction, request);
                //await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<IList<FindTransactionLineItem>>(content);
                findTransaction.LineItems = obj;


            }
            catch (Exception exp)
            {

            }
            return findTransaction;
        }

        public async Task<BaseServerResponse<IList<GetFromTransactionResponse>>> GetFromTransactions(GetFromTransactionRequest request, URLDefinitions urlDef)
        {
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(BaseEndpoint.BaseURL + urlDef.URL, request);
                string content = await response.Content.ReadAsStringAsync();
                var ServerResponse = JsonConvert.DeserializeObject<BaseServerResponse<IList<GetFromTransactionResponse>>>(content);
                return ServerResponse;


            }
            catch (Exception exp)
            {
                var ServerResponse = new BaseServerResponse<IList<GetFromTransactionResponse>>();
                ServerResponse.Value = new List<GetFromTransactionResponse>();
                ServerResponse.ResponseType = ServerResponseType.ProcessingError;
                ServerResponse.AddErrorMessage("Faild to Execute the request");
                return ServerResponse;
            }
        }

        public async Task<InvoiceDetailsByHdrSerNo> GetInvoiceFromSerialNumber(ItemSerialNumber serialNumber)
        {
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.InvoiceBySerial, serialNumber);
                string content = await response.Content.ReadAsStringAsync();
                var ServerResponse = JsonConvert.DeserializeObject<InvoiceDetailsByHdrSerNo>(content);
                return ServerResponse;


            }
            catch (Exception exp)
            {
                return null;
            }
        }

        public async Task<IList<RecieptDetailResponse>> GetRecieptDetailResponses(RecieptDetailRequest request, URLDefinitions urlDef)
        {
            try
            {

                var response = await _httpClient.PostAsJsonAsync(BaseEndpoint.BaseURL + urlDef.URL, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<IList<RecieptDetailResponse>>(content);
                return obj;


            }
            catch (Exception exp)
            {
                return new List<RecieptDetailResponse>();
            }
        }

        public async Task ThreadedGetAllStockLocation(int user,int company, StockAsAtRequest rest)
        {
            var thread = new Thread(async () =>
            {
                await GetAllStocksRelatedToLocation(user, company, rest);
            });

            thread.Start();
        }
        
        public async Task GetAllStocksRelatedToLocation(int user, int company, StockAsAtRequest rqest)
        {
            if (_connectionState.IsConnected())
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.AllStockAsAtEndpoint, rqest);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var objs = JsonConvert.DeserializeObject<List<StockAsAtResponse>>(content);
                if (objs != null)
                {
                    foreach (var item in objs)
                    {
                        StockAsAt data = new StockAsAt();
                        data.User = user;
                        data.Company = company;
                        data.Project = 1;
                        data.Location = rqest.LocationKey;
                        data.element = rqest.ElementKey;
                        data.ItemCd = item.ItemKey;
                        data.isLocked = item.IsLocked;
                        data.CurStk = item.StockAsAt;
                        await _itemStockAsAtStore.SaveItemAsync(data);
                        ComboRequestDTO requestDTO = new ComboRequestDTO();
                        requestDTO.AddtionalData.Add("ItemKey", item.ItemKey);
                        requestDTO.CancelRead = false;
                        requestDTO.RequestingURL = BaseEndpoint.BaseURL + "Unit/ReadUnits";
                        if(rqest.ElementKey == 198968)
                        {
                            requestDTO.RequestingElementKey = 198983;
                        }
                        await _comboDataManager.GetItemUnits(requestDTO);
                        ItemRateRequest req = new ItemRateRequest();
                        req.EffectiveDate = DateTime.Today;
                        req.ItemKey = item.ItemKey;
                        req.LocationKey = rqest.LocationKey;
                        req.ObjectKey = rqest.ElementKey;
                        req.ConditionCode = "OrdTyp";
                        await _comboDataManager.GetRate(req);

                    }
                }
            }
        }

        public async Task<StockAsAtResponse> GetStockAsAt(StockAsAtRequest rquest)
        {
            try
            {
                string uid = await _localStorage.GetItem("UID");
                string cid = await _localStorage.GetItem("CID");
                int uId = Int32.Parse(uid);
                int cId = Int32.Parse(cid);
                string rqstr = JsonConvert.SerializeObject(rquest);
                if (_connectionState.IsConnected())
                {
                    var cl = _factory.CreateClient();
                    AssignClientData(cl);
                    var response = await cl.PostAsJsonAsync(TokenEndpoints.StockAsAtEndpoint, rquest);
                    // await response.Content.LoadIntoBufferAsync();
                    string content = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<StockAsAtResponse>(content);

                    //    IncomingStrings incstr = new IncomingStrings();
                    //    incstr.name = TokenEndpoints.StockAsAtEndpoint;
                    //    incstr.user = uId;
                    //    incstr.company = cId;
                    //    incstr.parameters = rqstr;
                    //    incstr.timestamp = DateTime.Now;
                    //    incstr.response = content;
                    //    await _sqliteStorageService.SaveItemAsync(incstr);

                    StockAsAt details = await _itemStockAsAtStore.GetContent(uId, cId, rquest.ProjectKey, rquest.LocationKey, rquest.ElementKey, rquest.ItemKey);
                    if (details != null && details.timestamp < DateTime.Now)
                    {
                        await ThreadedGetAllStockLocation(uId, cId, rquest);
                    }
                    else if(details == null)
                    {
                        await ThreadedGetAllStockLocation(uId, cId, rquest);
                    }
                    
                    return obj;
                }
                else
                {
                    //    IncomingStrings datas = await _sqliteStorageService.GetItemAsync(cId, uId, TokenEndpoints.StockAsAtEndpoint, rqstr);
                    //    if(datas != null)
                    //    {
                    //        string content = datas.response;
                    //        var obj = JsonConvert.DeserializeObject<StockAsAtResponse>(content);
                    //        return obj;
                    //    }
                    //    else
                    //    {
                    //        return new StockAsAtResponse();
                    //    }
                    StockAsAt details = await _itemStockAsAtStore.GetContent(uId, cId, rquest.ProjectKey, rquest.LocationKey, rquest.ElementKey, rquest.ItemKey);
                    StockAsAtResponse response = new StockAsAtResponse();
                    response.StockAsAt = details.CurStk;
                    response.IsLocked = details.isLocked;
                    response.ItemKey = details.ItemCd;
                    return response;
                }
            }
            catch (Exception exp)
            {
                return new StockAsAtResponse();
            }
        }

        public async Task<RecviedAmountResponse> GetTotalPayedAmount(RecieptDetailRequest request)
        {
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var url = TokenEndpoints.TotalPayedRequestURL;
                var response = await cl.PostAsJsonAsync(url, request);
                string content = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<RecviedAmountResponse>(content);

                return obj;


            }
            catch (Exception exp)
            {
                return new RecviedAmountResponse();
            }
        }

        public async Task HoldTransaction(BLTransaction transaction)
        {
            //string docno = await _localStorage.GetItem(transaction.DocumentNumber);
            //if (!String.IsNullOrEmpty(docno))
            //{
            //    _localStorage.RemoveItem(transaction.DocumentNumber);
            //}
            //_localStorage.SetItem(transaction.DocumentNumber, transaction);

            
        }

        public async Task<IList<BLTransaction>> LoadTransactionApprovals(FindTransactionStatus request)
        {
            IList<BLTransaction> list = new List<BLTransaction>();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.LoadTransactionApprovalDetails, request);
                //await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                list = JsonConvert.DeserializeObject<IList<BLTransaction>>(content);


            }
            catch (Exception exp)
            {

            }
            return list;
        }

        public async Task<BLTransaction> OpenTransaction(TransactionOpenRequest request)
        {
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.OpenTransaction, request);
                //await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<BLTransaction>(content);

                return obj;


            }
            catch (Exception exp)
            {
                return new BLTransaction();
            }
        }

        public async Task<BLTransaction> OpenTransaction(TransactionOpenRequest request, URLDefinitions URL)
        {
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(BaseEndpoint.BaseURL + URL.URL, request);
                //await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<BLTransaction>(content);

                return obj;


            }
            catch (Exception exp)
            {
                return new BLTransaction();
            }
        }

        public async Task<TransactionPermission> CheckTransactionPermission(BLTransaction req)
        {
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.CheckTransactionPermissionEndpoint, req);
                //await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<TransactionPermission>(content);

                return obj;


            }
            catch (Exception exp)
            {
                return new TransactionPermission();
            }
        }

        public async Task<IList<DenominationEntry>> ReadDenominationEntries(DenominationRequest request)
        {
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.CashDenominationRead, request);
                //await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<IList<DenominationEntry>>(content);
                return obj;


            }
            catch (Exception exp)
            {
                return new List<DenominationEntry>();
            }
        }

        public async Task<BaseServerResponse<BLTransaction>> ReadFromTransaction(FromTransactionOpenRequest request)
        {
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.ReadFromTransaction, request);
                //await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<BaseServerResponse<BLTransaction>>(content);
                return obj;


            }
            catch (Exception exp)
            {
                return new BaseServerResponse<BLTransaction>();
            }
        }

        public async Task<IList<ItemSerialNumber>> RetriveItemTransactionSerials(ItemTransactionSerialRequest request)
        {
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.ItemTrnasactionSerialsURL, request);
                //await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<IList<ItemSerialNumber>>(content);
                return obj;


            }
            catch (Exception exp)
            {
                return new List<ItemSerialNumber>();
            }
        }

        public async Task SaveAccountRecieptPayement(AccoutRecieptPayment accoutReciept)
        {
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.SaveAccountRecieptURL, accoutReciept);
            }
            catch (Exception exp)
            {

            }
        }

        //public async Task SaveAccountRecieptPayementEx(PayementModeReciept accoutReciept)
        //{
        //    try
        //    {

        //        var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.SaveAccountRecieptURLEx, accoutReciept);



        //    }
        //    catch (Exception exp)
        //    {

        //    }
        //}

        //public Task SaveAccountRecieptPayementEx(PayementModeReciept reciept)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task SaveCashInOutTransaction(CashInOutTransaction transaction, URLDefinitions uRL)
        {
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(BaseEndpoint.BaseURL + uRL.URL, transaction);
                //await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<CashInOutTransaction>(content);
                transaction.TransactionNumber = obj.TransactionNumber;

            }
            catch (Exception exp)
            {

            }
        }

        public async Task SaveDenominations(IList<DenominationEntry> denominations)
        {
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.SaveDenominationEndpint, denominations);

            }
            catch (Exception exp)
            {

            }
        }

        public async Task SaveItemSerialNumber(ItemSerialNumber serialNumber)
        {
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.SaveItemSerialURL, serialNumber);
            }
            catch (Exception exp)
            {

            }
        }

        public async Task SaveHeaderSerialNumber(ItemSerialNumber serialNumber)
        {
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.SaveHeaderSerialURL, serialNumber);
            }
            catch (Exception exp)
            {

            }
        }
        //ADD to Queue
        public async Task<ExtendedTransaction> SaveTransaction(BLTransaction transation)
        {
            ExtendedTransaction responses = new ExtendedTransaction();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.TransactionSaveEndpoint, transation);
                //await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                responses.transaction = JsonConvert.DeserializeObject<BLTransaction>(content);
                responses.isSavedOnline = true;
                transation.TransactionKey = responses.transaction.TransactionKey;
                transation.TransactionNumber = responses.transaction.TransactionNumber;
            }catch(HttpRequestException httpExp)
            {
                string uid = await _localStorage.GetItem("UID");
                string cid = await _localStorage.GetItem("CID");
                int uId = Int32.Parse(uid);
                int cId = Int32.Parse(cid);

                RequestQueueItem requestQueueItem = new RequestQueueItem();
                requestQueueItem.timeStamp = DateTime.Now.ToString();
                requestQueueItem.requestBody = JsonConvert.SerializeObject(transation);
                requestQueueItem.url = "Transaction.SaveTransaction";
                requestQueueItem.isSynced = 0;
                requestQueueItem.User = uId;
                requestQueueItem.Company = cId;
                int updt = await _sqliteStorageService.SaveQueuItemAsync(requestQueueItem);
                responses = new ExtendedTransaction();
                responses.isSavedOnline = false;
            }
            catch (Exception exp)
            {
                responses = new ExtendedTransaction();
                transation = new BLTransaction();
                responses.isExceptionThrown = true;

            }
            return responses;
        }

        public async Task<ExtendedTransaction> SaveTransaction(string transation)
        {
            ExtendedTransaction responses = new ExtendedTransaction();
            try
            {
                BLTransaction transtation = JsonConvert.DeserializeObject<BLTransaction>(transation);
                //transtation.YourReferenceDate = DateTime.Now;
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.TransactionSaveEndpoint, transtation);
                await response.Content.LoadIntoBufferAsync();
                if(response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    responses.transaction = JsonConvert.DeserializeObject<BLTransaction>(content);
                    responses.isSavedOnline = true;
                }
                else
                {
                    responses = new ExtendedTransaction();
                    responses.isExceptionThrown = true;
                }
            }
            catch (Exception exp)
            {
                responses = new ExtendedTransaction();
                responses.isExceptionThrown = true;

            }
            return responses;
        }

        public async Task UpadteTransactionApprovals(BLTransaction request)
        {
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.UpdateTransactionApproval, request);
            }
            catch (Exception exp)
            {
            }
        }

        public async Task<ValidateModel> SaveAccountRecieptPayementEx(PayementModeReciept accoutReciept, string url)
        {
            ValidateModel validateModel = new ValidateModel();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(url, accoutReciept);
                string content = await response.Content.ReadAsStringAsync();
                validateModel = JsonConvert.DeserializeObject<ValidateModel>(content);


            }
            catch (Exception exp)
            {

            }

            return validateModel;
        }

        public async Task<ApiServerResponse<CustomerOutStadingDetails>> GetCustomerOutStandingDetails(CustomerOutStanding request, string url)
        {
            ApiServerResponse<CustomerOutStadingDetails> validateModel = new ApiServerResponse<CustomerOutStadingDetails>();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(url, request);
                string content = await response.Content.ReadAsStringAsync();
                validateModel = JsonConvert.DeserializeObject<ApiServerResponse<CustomerOutStadingDetails>>(content);


            }
            catch (Exception exp)
            {

            }

            return validateModel;
        }

        public async Task<ValidateModel> ValidateSerialNumberEntries(ItemSerialNumber request, string url)
        {
            ValidateModel validateModel = new ValidateModel();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(url, request);
                string content = await response.Content.ReadAsStringAsync();
                validateModel = JsonConvert.DeserializeObject<ValidateModel>(content);


            }
            catch (Exception exp)
            {

            }

            return validateModel;
        }
    }

    public class ExtendedTransaction
    {
        public bool isSavedOnline { get; set; }
        public bool isExceptionThrown { get; set; }
        public BLTransaction transaction { get;set; }
    }
}
