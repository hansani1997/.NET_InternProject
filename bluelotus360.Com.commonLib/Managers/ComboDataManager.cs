using bluelotus360.Com.commonLib.Routes;
using bluelotus360.Com.MauiSupports.Services.MAUISecureStorage;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.Com.Shared.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using BL10.CleanArchitecture.Shared.Constants;
using bluelotus360.Com.MauiSupports.Services.SqliteStorageServices;
using bluelotus360.Com.MauiSupports.Services.ConnectionStates;
using System.Reflection.Metadata;
using bluelotus360.Com.MauiSupports.Models;
using BL10.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.Com.Shared.Constants.Storage;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using System.Diagnostics;
using bluelotus360.Com.commonLib.Services.Definition;

namespace bluelotus360.Com.commonLib.Managers
{
    public class OfflineInput
    {
        public int User { get; set; } = 0;
        public int Company { get; set; } = 0;
        public ComboRequestDTO ComboRequest { get; set; } = null;
    }
    public class ComboDataManager:IComboDataManager
    {
        private readonly RestClient _client;
        private readonly HttpClient _httpClient;
        //private readonly ISecureStorageService _localStorage;
        private readonly IStorageService _localStorage;
        private readonly IHttpClientFactory _factory;
        private ISqliteStorageService _sqliteStorageService;
        private IAddressComboStore _addressComboStore;
        private IItemComboStore _itemComboStore;
        private IConnectionState _connectionState;
        public ComboDataManager(HttpClient httpClient,IAddressComboStore addressComboStore , IItemComboStore itemComboStore, RestClient client, IStorageService localStorage, IHttpClientFactory factory,ISqliteStorageService sqliteStorageService,IConnectionState conne
            )
        {
            _client = client;
            _httpClient = httpClient;
            _itemComboStore = itemComboStore;
            _addressComboStore = addressComboStore;
            _connectionState = conne;
            _localStorage = localStorage;
            _sqliteStorageService= sqliteStorageService;

            if (_httpClient.DefaultRequestHeaders.Count() == 0)
                _httpClient.DefaultRequestHeaders.Add("IntegrationID", GlobalConsts.intergrationId);
            _factory = factory;
        }

        public async Task OpenThreadSaveAllAddressResponse(OfflineInput offlineInput)
        {
            if(offlineInput != null && offlineInput.Company != 0 && offlineInput.User != 0 && offlineInput.ComboRequest != null)
            {
                var collectionn = new BlockingCollection<OfflineInput>();
                var thread = new Thread(async () =>
                {
                    foreach(OfflineInput input in collectionn)
                    {
                        await SaveAllAddressResponses(input.User,input.Company,input.ComboRequest);
                    }
                });

                collectionn.Add(offlineInput);
                collectionn.CompleteAdding();

                thread.Start();

            }
        }

        public async Task SaveAllAddressResponses(int user,int company,ComboRequestDTO requestDTO)
        {
            
            if(requestDTO != null)
            {
                IList<AddressResponse> list = new List<AddressResponse>();
                if(requestDTO.RequestingURL != null && requestDTO.RequestingURL == BaseEndpoint.BaseURL + "Address/ReadAddress")
                {
                    requestDTO.RequestingURL = BaseEndpoint.BaseURL + "Address/ReadAllAddress";
                    var cl = _factory.CreateClient();
                    AssignClientData(cl);
                    var response = await cl.PostAsJsonAsync(requestDTO.RequestingURL, requestDTO);
                    var content = await response.Content.ReadAsStringAsync();
                    list = JsonConvert.DeserializeObject<IList<AddressResponse>>(content);
                }
                if (requestDTO.RequestingURL != null && requestDTO.RequestingURL == BaseEndpoint.BaseURL + "Address/ReadAddressV2")
                {
                    requestDTO.RequestingURL = BaseEndpoint.BaseURL + "Address/ReadAllAddress";
                    var cl = _factory.CreateClient();
                    AssignClientData(cl);
                    var response = await cl.PostAsJsonAsync(requestDTO.RequestingURL, requestDTO);
                    var content = await response.Content.ReadAsStringAsync();
                    list = JsonConvert.DeserializeObject<IList<AddressResponse>>(content);
                }
                if(list != null && list.Count != 0)
                {
                    foreach(AddressResponse item in list)
                    {
                        AddressComboModel model = new AddressComboModel();
                        model.AddressObject = JsonConvert.SerializeObject(item);
                        model.AddressName = item.AddressName;
                        model.AddressKey = item.AddressKey;
                        model.User = user;
                        model.Company = company;
                        model.RequestingElement = requestDTO.RequestingElementKey;
                        model.timestamp = DateTime.Today;
                        await _addressComboStore.SaveItemAsync(model);
                    }
                }
            }
        }

        //OFFLINE_CAPABLE
        public async Task<IList<AddressResponse>> GetAddressResponses(ComboRequestDTO requestDTO)
        {
            // var response = await _httpClient.PostAsJsonAsync(requestDTO.RequestingURL, requestDTO);
            try
            {
                if (_connectionState.IsConnected())
                {
                    string content;
                    String cid = await _localStorage.GetItem("CID");
                    String uid = await _localStorage.GetItem("UID");
                    var cl = _factory.CreateClient();
                    AssignClientData(cl);
                    var response = await cl.PostAsJsonAsync(requestDTO.RequestingURL, requestDTO);
                    content = await response.Content.ReadAsStringAsync();
                    //IncomingStrings inncomingData = new IncomingStrings();
                    //inncomingData.name = requestDTO.RequestingURL;
                    //var serialized = JsonConvert.SerializeObject(requestDTO);
                    //inncomingData.parameters = serialized;
                    //inncomingData.user = Int32.Parse(uid);
                    //inncomingData.company = Int32.Parse(cid);
                    //inncomingData.response = content;
                    //await _sqliteStorageService.SaveItemAsync(inncomingData);
                    var list = JsonConvert.DeserializeObject<IList<AddressResponse>>(content);
                    var listest = await _addressComboStore.GetContents(int.Parse(uid), int.Parse(cid), requestDTO.RequestingElementKey, requestDTO.SearchQuery, 0);
                    //await SaveAllAddressResponses(Int32.Parse(uid), Int32.Parse(cid), requestDTO);
                    OfflineInput ol = new OfflineInput();
                    ol.User = Int32.Parse(uid);
                    ol.Company = Int32.Parse(cid);
                    ol.ComboRequest = requestDTO;
                    if (listest == null)
                    {
                        OpenThreadSaveAllAddressResponse(ol);
                        //await SaveAllAddressResponses(, requestDTO);
                    }
                    else if (listest.Count == 0)
                    {
                        OpenThreadSaveAllAddressResponse(ol);
                        //await SaveAllAddressResponses(Int32.Parse(uid), Int32.Parse(cid), requestDTO);
                    }
                    else if (listest[0].timestamp < DateTime.Today)
                    {
                        OpenThreadSaveAllAddressResponse(ol);
                        //await SaveAllAddressResponses(Int32.Parse(uid), Int32.Parse(cid), requestDTO);
                    }
                    return list;
                }
                else
                {
                    String cid = await _localStorage.GetItem("CID");
                    String uid = await _localStorage.GetItem("UID");
                    //string resx = JsonConvert.SerializeObject(requestDTO);
                    //IncomingStrings iss = await _sqliteStorageService.GetItemAsync(Int32.Parse(cid), Int32.Parse(uid), requestDTO.RequestingURL, resx);
                    //string content = iss.response;
                    // var list = JsonConvert.DeserializeObject<IList<AddressResponse>>(content);

                    int uId = Int32.Parse(uid);
                    int cId = Int32.Parse(cid);
                    List<AddressComboModel> colon = await _addressComboStore.GetContents(uId, cId, requestDTO.RequestingElementKey, requestDTO.SearchQuery, 0);
                    List<AddressResponse> list = new List<AddressResponse>();
                    foreach (AddressComboModel itemComboResponse in colon)
                    {
                        AddressResponse itemResponse = JsonConvert.DeserializeObject<AddressResponse>(itemComboResponse.AddressObject);
                        list.Add(itemResponse);
                    }
                    list = list.Take(15).ToList();
                    return list;
                }
                
            }
            //catch(HttpRequestException httpExp)
            //{
            //    String cid = await _localStorage.GetItem("CID");
            //    String uid = await _localStorage.GetItem("UID");
            //    string resx = JsonConvert.SerializeObject(requestDTO);
            //    IncomingStrings iss = await _sqliteStorageService.GetItemAsync(Int32.Parse(cid), Int32.Parse(uid), requestDTO.RequestingURL, resx);
            //    string content = iss.response;
            //    var list = JsonConvert.DeserializeObject<IList<AddressResponse>>(content);
            //    return list;
            //}
            catch (Exception exp)
            {
                return new List<AddressResponse>();
            }
        }
        //NOT_RELATED_TO_CAPABILITY_OF_OFFLINE
        private void AssignClientData(HttpClient cl)
        {
            cl.BaseAddress = _httpClient.BaseAddress;
            cl.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                _httpClient.DefaultRequestHeaders.Authorization.Scheme,
                _httpClient.DefaultRequestHeaders.Authorization.Parameter);
            cl.DefaultRequestHeaders.Add("IntegrationID", GlobalConsts.intergrationId);
        }
        //OFFLINE_CAPABLE
        public async Task<IList<AccountResponse>> GetAccountResponse(ComboRequestDTO requestDTO)
        {

            string uid = await _localStorage.GetItem("UID");
            string cid = await _localStorage.GetItem("CID");
            int uId = Int32.Parse(uid);
            int cId = Int32.Parse(cid);
            string content;
            try
            {
                if (_connectionState.IsConnected())
                {
                    var cl = _factory.CreateClient();
                    AssignClientData(cl);
                    var response = await cl.PostAsJsonAsync(requestDTO.RequestingURL, requestDTO);
                    content = await response.Content.ReadAsStringAsync();
                    IncomingStrings incoming = new IncomingStrings();
                    incoming.name = requestDTO.RequestingURL;
                    incoming.user = uId;
                    incoming.company = cId;
                    incoming.parameters = JsonConvert.SerializeObject(requestDTO);
                    incoming.response = content;
                    await _sqliteStorageService.SaveItemAsync(incoming);
                    var list = JsonConvert.DeserializeObject<IList<AccountResponse>>(content);
                    return list;
                }
                else
                {
                    string resconv = JsonConvert.SerializeObject(requestDTO);
                    IncomingStrings iss = await _sqliteStorageService.GetItemAsync(cId, uId, requestDTO.RequestingURL, resconv);
                    content = string.Empty;
                    if (iss != null)
                    {
                        
                        content = iss.response;

                    }
                    
                    var list = JsonConvert.DeserializeObject<IList<AccountResponse>>(content);
                    return list;
                }
                
            }
            //catch(HttpRequestException exp)
            //{
            //    string resconv = JsonConvert.SerializeObject(requestDTO);
            //    IncomingStrings iss = await _sqliteStorageService.GetItemAsync(cId, uId, requestDTO.RequestingURL, resconv);
            //    content = iss.response;
            //    var list = JsonConvert.DeserializeObject<IList<AccountResponse>>(content);
            //    return list;
            //}
            catch(Exception ex)
            {
                return null;
            }
        }
        //OFFLINE CAPABLE
        public async Task<IList<CodeBaseResponse>> GetCodeBaseResponses(ComboRequestDTO requestDTO)
        {
            string uid = await _localStorage.GetItem("UID");
            string cid = await _localStorage.GetItem("CID");
            int uId = Int32.Parse(uid);
            int cId = Int32.Parse(cid);
            try
            {
                if (_connectionState.IsConnected())
                {
                    string content;
                    var cl = _factory.CreateClient();
                    AssignClientData(cl);
                    var response = await cl.PostAsJsonAsync(requestDTO.RequestingURL, requestDTO);
                    content = await response.Content.ReadAsStringAsync();
                    IncomingStrings inss = new IncomingStrings();
                    inss.name = requestDTO.RequestingURL;
                    inss.parameters = JsonConvert.SerializeObject(requestDTO);
                    inss.response = content;
                    inss.user = uId;
                    inss.company = cId;
                    await _sqliteStorageService.SaveItemAsync(inss);

                    var list = JsonConvert.DeserializeObject<IList<CodeBaseResponse>>(content);
                    return list;
                }
                else
                {
                    IncomingStrings insss = await _sqliteStorageService.GetItemAsync(cId, uId, requestDTO.RequestingURL, JsonConvert.SerializeObject(requestDTO));
                    string content = string.Empty;
                    if (insss != null)
                    {
                        content = insss.response;
                    }
                    
                    var list = JsonConvert.DeserializeObject<IList<CodeBaseResponse>>(content);
                    return list;
                }
                
            }
            //catch(HttpRequestException httpExp)
            //{
                
            //}
            catch (Exception exp)
            {
                return new List<CodeBaseResponse>();
            }
        }
        //OFFLINE_CAPABLE
        public async Task ThreadedSaveAllItemResponse(OfflineInput offlineInput)
        {
            var collection = new BlockingCollection<OfflineInput>();

            var thread = new Thread(async () =>
            {
                foreach (OfflineInput offlineInputk in collection)
                {
                    await SaveAllItemResponses(offlineInputk.User, offlineInputk.Company, offlineInputk.ComboRequest);
                }
            });

            collection.Add(offlineInput);
            collection.CompleteAdding();
            thread.Start();
        }

        public async Task SaveAllItemResponses(int uId,int cId,ComboRequestDTO comboRequestDTO)
        {
            if(comboRequestDTO != null)
            {
                if(comboRequestDTO.RequestingURL.Equals(BaseEndpoint.BaseURL + "Item/GetItemsForTransactionJson"))
                {
                    try
                    {
                        comboRequestDTO.RequestingURL = BaseEndpoint.BaseURL + "Item/getItemsForTransactionCacheJson";
                        var cl = _factory.CreateClient();
                        AssignClientData(cl);
                        cl.Timeout = TimeSpan.FromMinutes(4);
                        var response = await cl.PostAsJsonAsync(comboRequestDTO.RequestingURL, comboRequestDTO);
                        string content = await response.Content.ReadAsStringAsync();
                        var list = JsonConvert.DeserializeObject<IList<ItemResponse>>(content);
                        if (list != null)
                        {
                            foreach (ItemResponse item in list)
                            {
                                ItemComboResponses icr = new ItemComboResponses();
                                icr.user = uId;
                                icr.company = cId;
                                icr.timestamp = DateTime.Today;
                                icr.ObjectKey = comboRequestDTO.RequestingElementKey;
                                icr.ItemKey = item.ItemKey;
                                icr.ItemName = item.ItemName;
                                icr.ItemObject = JsonConvert.SerializeObject(item);
                                _itemComboStore.SaveItemAsync(icr);
                            }
                        }
                    }catch(Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }
        }
        public async Task<IList<ItemResponse>> GetItemResponses(ComboRequestDTO requestDTO)
        {
            //  var response = await _httpClient.PostAsJsonAsync(requestDTO.RequestingURL, requestDTO);
            try
            {
                if (_connectionState.IsConnected())
                {
                    string uid = await _localStorage.GetItem("UID");
                    string cid = await _localStorage.GetItem("CID");
                    int uId = Int32.Parse(uid);
                    int cId = Int32.Parse(cid);

                    string content;
                    var cl = _factory.CreateClient();
                    AssignClientData(cl);
                    var response = await cl.PostAsJsonAsync(requestDTO.RequestingURL, requestDTO);
                    content = await response.Content.ReadAsStringAsync();
                    var list = JsonConvert.DeserializeObject<IList<ItemResponse>>(content);
                    if (requestDTO.AddtionalData.Count == 0)
                    {
                        var mlist = await _itemComboStore.GetContents(uId, cId,requestDTO.RequestingElementKey,"",0);
                        OfflineInput off = new OfflineInput();
                        off.Company = cId;
                        off.User = uId;
                        off.ComboRequest = requestDTO;
                        if(mlist == null)
                        {
                            await ThreadedSaveAllItemResponse(off);
                        }else if(mlist.Count == 0){
                            await ThreadedSaveAllItemResponse(off);
                        }else if(mlist.Count > 0 && mlist[0].timestamp < DateTime.Today)
                        {
                            await ThreadedSaveAllItemResponse(off);
                        }
                    }
                    return list;
                }
                else
                {
                    string uid = await _localStorage.GetItem("UID");
                    string cid = await _localStorage.GetItem("CID");
                    int uId = Int32.Parse(uid);
                    int cId = Int32.Parse(cid);
                    List<ItemComboResponses> colon = await _itemComboStore.GetContents(uId, cId,requestDTO.RequestingElementKey,requestDTO.SearchQuery,0);
                    List<ItemResponse> list = new List<ItemResponse>();
                    foreach(ItemComboResponses itemComboResponse in colon)
                    {
                        ItemResponse itemResponse = JsonConvert.DeserializeObject<ItemResponse>(itemComboResponse.ItemObject);
                        list.Add(itemResponse);
                    }
                    list = list.Take(15).ToList();
                    return list;
                }
                
            }
            //catch(HttpRequestException httpExp)
            //{
            //    string uid = await _localStorage.GetItem("UID");
            //    string cid = await _localStorage.GetItem("CID");
            //    int uId = Int32.Parse(uid);
            //    int cId = Int32.Parse(cid);
            //    IncomingStrings colon = await _sqliteStorageService.GetItemAsync(cId, uId, requestDTO.RequestingURL, JsonConvert.SerializeObject(requestDTO));
            //    string content = colon.response;
            //    var list = JsonConvert.DeserializeObject<IList<ItemResponse>>(content);
            //    return list;
            //}
            catch (Exception exp)
            {
                return new List<ItemResponse>();
            }
        }
        //OFFLINE_CAPABLE
        public async Task<IList<UnitResponse>> GetItemUnits(ComboRequestDTO requestDTO)
        {
            try
            {
                if (_connectionState.IsConnected())
                {
                    string uid = await _localStorage.GetItem("UID");
                    string cid = await _localStorage.GetItem("CID");
                    int uId = Int32.Parse(uid);
                    int cId = Int32.Parse(cid);

                    string content;
                    var cl = _factory.CreateClient();
                    AssignClientData(cl);
                    var response = await cl.PostAsJsonAsync(requestDTO.RequestingURL, requestDTO);
                    content = await response.Content.ReadAsStringAsync();
                    IncomingStrings incs = new IncomingStrings();
                    incs.name = requestDTO.RequestingURL;
                    incs.parameters = JsonConvert.SerializeObject(requestDTO);
                    incs.user = uId;
                    incs.company = cId;
                    incs.response = content;
                    await _sqliteStorageService.SaveItemAsync(incs);
                    var list = JsonConvert.DeserializeObject<IList<UnitResponse>>(content);

                    return list;
                }
                else
                {
                    string uid = await _localStorage.GetItem("UID");
                    string cid = await _localStorage.GetItem("CID");
                    int uId = Int32.Parse(uid);
                    int cId = Int32.Parse(cid);
                    IncomingStrings incos = await _sqliteStorageService.GetItemAsync(cId, uId, requestDTO.RequestingURL, JsonConvert.SerializeObject(requestDTO));
                    string content = incos.response;
                    var list = JsonConvert.DeserializeObject<IList<UnitResponse>>(content);

                    return list;
                }
                
            }
            //catch(HttpRequestException httpExp)
            //{
            //    string uid = await _localStorage.GetItem("UID");
            //    string cid = await _localStorage.GetItem("CID");
            //    int uId = Int32.Parse(uid);
            //    int cId = Int32.Parse(cid);
            //    IncomingStrings incos = await _sqliteStorageService.GetItemAsync(cId, uId, requestDTO.RequestingURL, JsonConvert.SerializeObject(requestDTO));
            //    string content = incos.response;
            //    var list = JsonConvert.DeserializeObject<IList<UnitResponse>>(content);

            //    return list;
            //}
            catch (Exception exp)
            {
                return new List<UnitResponse>();
            }

        }

        //OFFLINE_CAPABLEE
        public async Task<ItemRateResponse> GetRate(ItemRateRequest baseRequest)
        {
            //HttpResponseMessage response = null;
            ItemRateResponse rateResponse = new ItemRateResponse();
            try
            {
                if (_connectionState.IsConnected())
                {
                    string uid = await _localStorage.GetItem("UID");
                    string cid = await _localStorage.GetItem("CID");
                    int uId = Int32.Parse(uid);
                    int cId = Int32.Parse(cid);

                    //string responseBody = "";
                    //using (var request = new HttpRequestMessage(new HttpMethod("POST"), TokenEndpoints.ItemRateEndPoint))
                    //{
                    //    request.Headers.TryAddWithoutValidation("Timestamp", DateTime.Now.Ticks.ToString());
                    //    request.Content = JsonContent.Create(baseRequest);
                    //    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    //    response = await _httpClient.SendAsync(request);
                    //}
                    //responseBody = await response.Content.ReadAsStringAsync();

                    var cl = _factory.CreateClient();
                    AssignClientData(cl);
                    var response = await cl.PostAsJsonAsync(TokenEndpoints.ItemRateEndPoint, baseRequest);
                    string content = await response.Content.ReadAsStringAsync();
                    

                    IncomingStrings incstr = new IncomingStrings();
                    incstr.name = TokenEndpoints.ItemRateEndPoint;
                    incstr.parameters = JsonConvert.SerializeObject(baseRequest);
                    incstr.user = uId;
                    incstr.company = cId;
                    incstr.response = content;
                    await _sqliteStorageService.SaveItemAsync(incstr);
                    //rateResponse = JsonConvert.DeserializeObject<ItemRateResponse>(responseBody);
                    rateResponse = JsonConvert.DeserializeObject<ItemRateResponse>(content);
                }
                else
                {
                    string uid = await _localStorage.GetItem("UID");
                    string cid = await _localStorage.GetItem("CID");
                    int uId = Int32.Parse(uid);
                    int cId = Int32.Parse(cid);
                    IncomingStrings responseBod = await _sqliteStorageService.GetItemAsync(cId, uId, TokenEndpoints.ItemRateEndPoint, JsonConvert.SerializeObject(baseRequest));
                    if(responseBod != null)
                    {
                        string responseBody = responseBod.response;
                        rateResponse = JsonConvert.DeserializeObject<ItemRateResponse>(responseBody);
                    }
                }
                


            }
            //catch(HttpRequestException httpExp)
            //{
            //    string uid = await _localStorage.GetItem("UID");
            //    string cid = await _localStorage.GetItem("CID");
            //    int uId = Int32.Parse(uid);
            //    int cId = Int32.Parse(cid);
            //    IncomingStrings responseBod = await _sqliteStorageService.GetItemAsync(cId, uId, TokenEndpoints.ItemRateEndPoint, JsonConvert.SerializeObject(baseRequest));
            //    string responseBody = responseBod.response;
            //    rateResponse = JsonConvert.DeserializeObject<ItemRateResponse>(responseBody);
            //}
            catch (Exception exp)
            {
                Console.WriteLine("exp is {0}", exp);


            }

            return rateResponse;


        }

        //OFFLINE CAPABLE
        public async Task<IList<ItemCodeResponse>> GetItemByItemCode(ItemRequestModel itemRequest)
        {

            HttpResponseMessage response = null;
            IList<ItemCodeResponse> itemResponse;
            try
            {
                if (_connectionState.IsConnected())
                {
                    string uid = await _localStorage.GetItem("UID");
                    string cid = await _localStorage.GetItem("CID");
                    int uId = Int32.Parse(uid);
                    int cId = Int32.Parse(cid);

                    string responseBody = "";

                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), TokenEndpoints.GetItemByItemCodeEndPoint))
                    {
                        //request.Headers.TryAddWithoutValidation("Timestamp", DateTime.Now.Ticks.ToString());
                        request.Content = JsonContent.Create(itemRequest);
                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                        response = await _httpClient.SendAsync(request);
                    }
                    responseBody = await response.Content.ReadAsStringAsync();
                    IncomingStrings inss = new IncomingStrings();
                    inss.name = TokenEndpoints.GetItemByItemCodeEndPoint;
                    inss.parameters = JsonConvert.SerializeObject(itemRequest);
                    inss.user = uId;
                    inss.company = cId;
                    await _sqliteStorageService.SaveItemAsync(inss);
                    itemResponse = JsonConvert.DeserializeObject<IList<ItemCodeResponse>>(responseBody);
                }
                else
                {
                    string uid = await _localStorage.GetItem("UID");
                    string cid = await _localStorage.GetItem("CID");
                    int uId = Int32.Parse(uid);
                    int cId = Int32.Parse(cid);
                    IncomingStrings responnseBody = await _sqliteStorageService.GetItemAsync(cId, uId, TokenEndpoints.GetItemByItemCodeEndPoint, JsonConvert.SerializeObject(itemRequest));
                    string responseBody = responnseBody.response;
                    itemResponse = JsonConvert.DeserializeObject<IList<ItemCodeResponse>>(responseBody);
                }
                


            }
            //catch(HttpRequestException httpExp)
            //{
            //    string uid = await _localStorage.GetItem("UID");
            //    string cid = await _localStorage.GetItem("CID");
            //    int uId = Int32.Parse(uid);
            //    int cId = Int32.Parse(cid);
            //    IncomingStrings responnseBody = await _sqliteStorageService.GetItemAsync(cId, uId, TokenEndpoints.GetItemByItemCodeEndPoint, JsonConvert.SerializeObject(itemRequest));
            //    string responseBody = responnseBody.response;
            //    itemResponse = JsonConvert.DeserializeObject<IList<ItemCodeResponse>>(responseBody);
            //}
            catch (Exception exp)
            {

                itemResponse = null;

            }

            return itemResponse;
        }
        //OFFLINE CAPABLE
        public async Task<IList<PriceListResponse>> GetPriceLists(PriceListRequest price_list_request)
        {
            try
            {
                if (_connectionState.IsConnected())
                {
                    string uid = await _localStorage.GetItem("UID");
                    string cid = await _localStorage.GetItem("CID");
                    int uId = Int32.Parse(uid);
                    int cId = Int32.Parse(cid);

                    string content;

                    var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetPriceListEndPoint, price_list_request);
                    content = await response.Content.ReadAsStringAsync();
                    IncomingStrings incstr = new IncomingStrings();
                    incstr.name = TokenEndpoints.GetPriceListEndPoint;
                    incstr.parameters = JsonConvert.SerializeObject(price_list_request);
                    incstr.user = uId;
                    incstr.company = cId;
                    incstr.response = content;
                    await _sqliteStorageService.SaveItemAsync(incstr);
                    var list = JsonConvert.DeserializeObject<IList<PriceListResponse>>(content);
                    return list;
                }
                else
                {
                    string uid = await _localStorage.GetItem("UID");
                    string cid = await _localStorage.GetItem("CID");
                    int uId = Int32.Parse(uid);
                    int cId = Int32.Parse(cid);

                    string content;
                    IncomingStrings incc = await _sqliteStorageService.GetItemAsync(cId, uId, TokenEndpoints.GetPriceListEndPoint, JsonConvert.SerializeObject(price_list_request));
                    content = incc.response;
                    var list = JsonConvert.DeserializeObject<IList<PriceListResponse>>(content);
                    return list;
                }
                
            }
            //catch(HttpRequestException httpExp)
            //{
            //    string uid = await _localStorage.GetItem("UID");
            //    string cid = await _localStorage.GetItem("CID");
            //    int uId = Int32.Parse(uid);
            //    int cId = Int32.Parse(cid);

            //    string content;
            //    IncomingStrings incc = await _sqliteStorageService.GetItemAsync(cId, uId, TokenEndpoints.GetPriceListEndPoint, JsonConvert.SerializeObject(price_list_request));
            //    content = incc.response;
            //    var list = JsonConvert.DeserializeObject<IList<PriceListResponse>>(content);
            //    return list;
            //}
            catch (Exception exp)
            {
                return new List<PriceListResponse>();
            }
        }

        public async Task<IList<AccPaymentMappingResponse>> GetPayementAccountMapping(AccPaymentMappingRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetAccountMapping, request);
            try
            {

                string content = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<IList<AccPaymentMappingResponse>>(content);
                return list;

            }
            catch (Exception exp)
            {
                return new List<AccPaymentMappingResponse>();
            }
        }

        public async Task<IList<BinaryDocument>> GetItemDocuments(ItemRequestModel Req)
        {
            HttpResponseMessage response = null;
            IList<BinaryDocument> ImagesResponse = new List<BinaryDocument>();
            try
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), TokenEndpoints.ItemRateEndPoint))
                {
                    request.Headers.TryAddWithoutValidation("Timestamp", DateTime.Now.Ticks.ToString());
                    request.Content = JsonContent.Create(Req);
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    response = await _httpClient.SendAsync(request);
                }

                string responseBody = await response.Content.ReadAsStringAsync();
                ImagesResponse = JsonConvert.DeserializeObject<IList<BinaryDocument>>(responseBody);


            }
            catch (Exception exp)
            {
                Console.WriteLine("exp is {0}", exp);


            }
            finally
            {
            }

            return ImagesResponse;
        }

        public async Task<AddressCreateResponse> CreateNewAddress(AddressResponse request)
        {
            var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.CreateNewAddressURL, request);
            try
            {

                string content = await response.Content.ReadAsStringAsync();
                var addressCreate = JsonConvert.DeserializeObject<AddressCreateResponse>(content);
                return addressCreate;

            }
            catch (Exception exp)
            {
                return new AddressCreateResponse() { IsSuccess = false, Message = "Connection Error" };
            }
        }

        public async Task<Base64Document> GetBase64TopDocument(DocumentRetrivaltDTO request)
        {
            var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.ItemImageReadURL, request);
            try
            {

                string content = await response.Content.ReadAsStringAsync();
                var base64Document = JsonConvert.DeserializeObject<Base64Document>(content);
                return base64Document;

            }
            catch (Exception exp)
            {
                return new Base64Document();
            }
        }

        public async Task<IList<CodeBaseResponse>> GetNextApproveStatusResponses(ComboRequestDTO requestDTO)
        {
            var response = await _httpClient.PostAsJsonAsync(requestDTO.RequestingURL, requestDTO);
            try
            {

                string content = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<IList<CodeBaseResponse>>(content);
                return list;
            }
            catch (Exception exp)
            {
                return new List<CodeBaseResponse>();
            }
        }

        public async Task<IList<CodeBaseResponse>> GetApproveStatusResponses(ComboRequestDTO requestDTO)
        {
            var response = await _httpClient.PostAsJsonAsync(BaseEndpoint.BaseURL + "Codebase/getApproveStatusComboDetail", requestDTO);
            try
            {

                string content = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<IList<CodeBaseResponse>>(content);
                return list;
            }
            catch (Exception exp)
            {
                return new List<CodeBaseResponse>();
            }
        }
        //lnd
        public async Task<CodeBaseResponseExtended> GetCodeBaseResponseExtended(ComboRequestDTO requestDTO)
        {
            var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.CodeBaseDetailRequest, requestDTO);
            try
            {
                string content = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<CodeBaseResponseExtended>(content);
                return list;
            }
            catch (Exception exp)
            {
                return new CodeBaseResponseExtended();
            }
        }

        public async Task<IList<ItemSerialNumber>> GetSerialNumberResponses(ComboRequestDTO requestDTO)
        {
            var response = await _httpClient.PostAsJsonAsync(requestDTO.RequestingURL, requestDTO);
            try
            {

                string content = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<IList<ItemSerialNumber>>(content);
                return list;
            }
            catch (Exception exp)
            {
                return new List<ItemSerialNumber>();
            }
        }

        public async Task<IList<CodeBase>> ReadCategories(ComboRequestDTO requestDTO)
        {
            try
            {
                RestRequest request = new RestRequest(requestDTO.RequestingURL).AddJsonBody(requestDTO);
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

                //var response = await _httpClient.PostAsJsonAsync(requestDTO.RequestingURL, requestDTO);
                //string content = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<IList<CodeBase>>(content);
                return list;
            }
            catch (Exception exp)
            {
                return new List<CodeBase>();
            }
        }

        public async Task<IList<ItemRateResponse>> GetItemsByCatgory(ItemRetrivalDTO requestDTO)
        {
            try
            {
                //    var cl = _factory.CreateClient();
                //    AssignClientData(cl);
                //    var response = await cl.PostAsJsonAsync(requestDTO.RequestingURL, requestDTO);
                RestRequest request = new RestRequest(TokenEndpoints.CatItemReadURL).AddJsonBody(requestDTO);
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
                var list = JsonConvert.DeserializeObject<IList<ItemRateResponse>>(content);
                return list;
            }
            catch (Exception exp)
            {
                return new List<ItemRateResponse>();
            }
        }
    }
}
