using BL10.CleanArchitecture.Domain.Entities.APIInfo;
using BL10.CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueLotus360.Com.Infrastructure.Managers.APIManager;
using Newtonsoft.Json;
using RestSharp;
using static BlueLotus360.Com.Infrastructure.OrderPlatforms.PickMe.PickmeEntity;
using System.Security.Cryptography;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus.Com.Domain.Entity;
using BlueLotus360.Com.Infrastructure.Managers.Codebase;
using System.Net;
using bluelotus360.Com.commonLib.Managers.OrderManager;
using bluelotus360.Com.commonLib.Managers.Address;
using bluelotus360.Com.commonLib.Extensions;

namespace BlueLotus360.Com.Infrastructure.OrderPlatforms.PickMe
{
    public class PickmeAPIHandler
    {
        public readonly IAPIManager _apiManager;
        public readonly IOrderManager _OrderManager;
        public readonly IAddressManager _addressManager;
        private readonly OrderPlatformAPIInformation orderPlatformAPI;
        GetOrder returnData = new GetOrder();
        ICodebaseManager _codebaseManager;

        public PickmeAPIHandler(IAPIManager apiManager, IOrderManager orderManager,IAddressManager addressManager,ICodebaseManager codebaseManager)
        {
            _apiManager = apiManager;
            _addressManager = addressManager;
            orderPlatformAPI = new OrderPlatformAPIInformation(_apiManager, _addressManager);
            _OrderManager = orderManager;
            _codebaseManager = codebaseManager;
        }
        public async Task<GetOrder> GetPickmeOrder(int LocationKey,int BUKy)
        {
            APIRequestParameters apiParameters = new APIRequestParameters()
            {
                LocationKey = LocationKey,
                BUKy = BUKy,
                APIIntegrationName = "PickMe_" + LocationKey+"_"+BUKy,
                APIName = "PickMe",
                EndPointName=""

            };
            PartnerOrder ord = await _OrderManager.GetLastSyncTime(apiParameters);
            var time = TimeSpan.FromMinutes(ord.OrderLastSyncMinutes);
            string hour = String.Format("{0:N2}", time.TotalHours);
            APIInformation tokenInfo = await orderPlatformAPI.GetAPIDetailsByEndpointName(apiParameters);
            if (tokenInfo != null && tokenInfo.APIIntegrationKey > 11)
            {
                APIRequestParameters endpointrequest = new APIRequestParameters()
                {
                    LocationKey = LocationKey,
                    BUKy= BUKy,
                    APIIntegrationKey = tokenInfo.APIIntegrationKey,
                    EndPointName = PickmeEndpoints.GetOrder.GetDescription()
                };
                APIInformation endpointInfo = await _apiManager.GetAPIEndPoints(endpointrequest);
                var client = new RestClient(tokenInfo.BaseURL);
              //  hour = hour + 1;
                var request = new RestRequest(endpointInfo.EndPointURL + "?page=1&hours=" + hour, Method.Get);
                request.AddHeader("X-API-KEY", tokenInfo.SecretInstanceKey.Replace("\r", "").Replace("\n", "").Replace("\t", ""));
                request.AddHeader("Content-Type", "application/json");
                var response = client.Execute(request);

                #region fault response

                if (response.StatusCode == HttpStatusCode.MethodNotAllowed)
                {
                    var faultResponseXml = @"<?xml version='1.0' encoding='UTF-8' standalone='yes'?>
                                                        <RaiseFault async='false' continueOnError='false' enabled='true' name='Fault-405'>
                                                            <DisplayName>Fault 405</DisplayName>
                                                            <FaultRules/>
                                                            <Properties/>
                                                            <FaultResponse>
                                                                <Set>
                                                                  <Headers>
                                                                    <Header name='Allow'>GET, PUT, POST, DELETE</Header>
                                                                  </Headers>
                                                                    <Payload contentType='text/plain'>This wasn't supposed to happen</Payload>
                                                                    <StatusCode>405</StatusCode>
                                                                    <ReasonPhrase>405 Rules</ReasonPhrase>
                                                                </Set>
                                                            </FaultResponse>
                                                            <IgnoreUnresolvedVariables>true</IgnoreUnresolvedVariables>
                                                        </RaiseFault>";

                    var faultResponse = new RestResponse()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = faultResponseXml,
                        ContentType = "application/xml",
                    };

                    response = faultResponse;
                }

                #endregion

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                returnData = JsonConvert.DeserializeObject<GetOrder>(response.Content, settings);
                if(returnData.Data.Count > 0)
                {
                    ResponseDetails res = new ResponseDetails()
                    {
                        TriggerKey = 3,
                        SubscriberKey=3,
                        ResponseCode= response.StatusDescription,
                       Response= response.Content,
                       ContenetPayload= endpointInfo.EndPointURL,
                       Reference="PickMe Orders of ",
                       TrnTyp="PUOrd"

                    };
                 //  await _OrderManager.APIResponseDet_InsertWeb(res);
                }
            }
            
            return returnData;
        }
        public DateTime ConvertTimestamp(long timestamp)
        {
            DateTime result = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;
            return result;
        }
        public async Task<bool> SavePickmeOrder(GetOrder getOrder,int LocationKey,string FromDate,string ToDate,int BUKy,long ObjKy)
        {
            bool isSaved = false;
            int Lino = 0;
            List<int> UnavailableItemKey= new List<int>();
            

            foreach (PickmeEntity.data item in getOrder.Data)
            {
                bool IsAvailable = false;
                RequestParameters requestParameters = new RequestParameters()
                {
                  OrderID= item.PickmeJobID
                };
                IsAvailable = await _OrderManager.OrderplatformOrder_Findweb(requestParameters);
                
                if (!IsAvailable)
                {
                    PartnerOrder saveOrder = new PartnerOrder();
                    saveOrder.Location.CodeKey = LocationKey;
                    saveOrder.BU.CodeKey = BUKy;
                    // saveOrder.CreatedBy.UserKey = 1;
                    saveOrder.PartnerOrderId = 1;
                    saveOrder.OrderId = item.PickmeJobID;
                    saveOrder.OrderReference = "PCKM"+item.PickmeJobID;
                    saveOrder.OrderDate = ConvertTimestamp(Convert.ToInt64(item.CreatedTimestamp)).ToString("yyyy/MM/dd hh:mm:ss tt");
                    saveOrder.DeliveryBrand = "";
                    saveOrder.IsActive = 1;
                    saveOrder.IsApproved = 1;
                    saveOrder.Platforms.AccountCode = "PickMe";
                    saveOrder.Source = "Manual Sync";
                    //string deliverytypecode = item.DeliveryMode;
                    saveOrder.OrderStatus.CodeName = item.Status.Name;
                    CodeBaseResponse code = new CodeBaseResponse()
                    {
                        CodeName = saveOrder.OrderStatus.CodeName,
                        OurCode = saveOrder.Platforms.AccountCode
                    };
                    CodeBaseResponse record = await _OrderManager.GetOrderStatusByPartnerStatus(code);
                    saveOrder.OrderStatus.CodeKey = record.CodeKey;
                    CodeBaseResponse codeBaseResponse = new CodeBaseResponse()
                    {
                        ConditionCode="PmtTrm",
                        OurCode="PickmeWallet"
                    };
                    CodeBaseResponse payment= await _codebaseManager.GetCodesByConditionCodeAndOurCode(codeBaseResponse);
                    saveOrder.PaymentKey = payment.CodeKey;
                   

                    foreach (Items LineItem in item.Order.Items)
                    {
                        PartnerOrderDetails partnerOrderDetails = new PartnerOrderDetails();
                        ItemResponse itemrec = new ItemResponse()
                        {
                            ItemCode = LineItem.RefID.ToString()
                        };
                        partnerOrderDetails.OrderItem = await _OrderManager.GetItemByItemCode(itemrec);
                        RequestParameters qtyreq = new RequestParameters()
                        {
                            ItemKey= Convert.ToInt32(partnerOrderDetails.OrderItem.ItemKey),
                            LocationKey=LocationKey,
                            ObjKy=ObjKy
                        };
                        AvailableStock stockQty= await _OrderManager.GetAvailableQtyByItem(qtyreq);
                        if(stockQty != null)
                        {
                            if(stockQty.AvailableQty < Convert.ToDecimal(LineItem.Qty))
                            {
                                UnavailableItemKey.Add(Convert.ToInt32(partnerOrderDetails.OrderItem.ItemKey));
                            }
                        }
                        partnerOrderDetails.ItemQuantity = Convert.ToDecimal(LineItem.Qty);
                        partnerOrderDetails.TransactionPrice = Convert.ToDecimal(LineItem.Total) / Convert.ToDecimal(LineItem.Qty);
                        partnerOrderDetails.BaseTotalPrice = Convert.ToDecimal(LineItem.Total);
                        partnerOrderDetails.SpecialInstructions = LineItem.SpIns;
                        partnerOrderDetails.IsApproved = 1;
                        partnerOrderDetails.IsActive = 1;
                        partnerOrderDetails.Remarks = stockQty.AvailableQty < Convert.ToDecimal(LineItem.Qty) ? "Unavailable" : "";
                        Lino = Lino + 1;
                        partnerOrderDetails.OrderItem.LineNumber = Lino;
                        saveOrder.OrderItemDetails.Add(partnerOrderDetails);
                    }


                    if (item.Customer != null)
                    {
                        saveOrder.Customer.Phone = item.Customer.ContactNumber;
                        saveOrder.Customer.Address = item.Customer.Location.Address;
                        saveOrder.Customer.AdrId = item.Customer.ContactNumber;
                        saveOrder.Customer.Name = item.Customer.ContactNumber;
                        await orderPlatformAPI.SaveCustomer(saveOrder);
                    }
                    if(UnavailableItemKey.Count > 0)
                    {
                        CodeBaseResponse unavailableresponse = new CodeBaseResponse()
                        {
                            ConditionCode = "OrdSts",
                            OurCode = "Unavailable"
                        };
                        CodeBaseResponse Sts = await _codebaseManager.GetCodesByConditionCodeAndOurCode(unavailableresponse);
                        saveOrder.OrderStatus.CodeKey = Sts.CodeKey;
                    }
                   PartnerOrder SavedData= await _OrderManager.SavePartnerOrders(saveOrder);
                    if(SavedData.PartnerOrderId > 11)
                    {
                        isSaved= true;
                    }
                    else
                    {
                        isSaved= false;
                    }
                }

            }

            return isSaved;

        }

        public async Task<GetOrder> GetAllPickMeOrderByDuration(CodeBaseResponse Location,CodeBaseResponse BU,decimal Duration,int page=5)
        {
            GetOrder returnData = new GetOrder();
            APIRequestParameters apiParameters = new APIRequestParameters()
            {
                LocationKey = Location.CodeKey,
                BUKy = BU.CodeKey,
                APIIntegrationName = "PickMe_" + Location.CodeKey + "_" + BU.CodeKey,
                APIName = "PickMe",
                EndPointName = ""

            };
            APIInformation tokenInfo = await orderPlatformAPI.GetAPIDetailsByEndpointName(apiParameters);
            if (tokenInfo != null && tokenInfo.APIIntegrationKey > 11)
            {
                APIRequestParameters endpointrequest = new APIRequestParameters()
                {
                    LocationKey = Location.CodeKey,
                    BUKy = BU.CodeKey,
                    APIIntegrationKey = tokenInfo.APIIntegrationKey,
                    EndPointName = PickmeEndpoints.GetOrder.GetDescription()
                };
                APIInformation endpointInfo = await _apiManager.GetAPIEndPoints(endpointrequest);

                try
                {
                    var client = new RestClient(tokenInfo.BaseURL);
                    var request = new RestRequest(endpointInfo.EndPointURL + $"?page={page.ToString()}&hours={Duration}&size={40}", Method.Get);
                    request.AddHeader("X-API-KEY", tokenInfo.SecretInstanceKey.Replace("\r", "").Replace("\n", "").Replace("\t", ""));
                    request.AddHeader("Accept", "application/json");
                    request.AddHeader("Content-Type", "application/json");
                    var response = client.Execute(request);

                    #region fault response

                    if (response.StatusCode == HttpStatusCode.MethodNotAllowed)
                    {
                        var faultResponseXml = @"<?xml version='1.0' encoding='UTF-8' standalone='yes'?>
                                                        <RaiseFault async='false' continueOnError='false' enabled='true' name='Fault-405'>
                                                            <DisplayName>Fault 405</DisplayName>
                                                            <FaultRules/>
                                                            <Properties/>
                                                            <FaultResponse>
                                                                <Set>
                                                                  <Headers>
                                                                    <Header name='Allow'>GET, PUT, POST, DELETE</Header>
                                                                  </Headers>
                                                                    <Payload contentType='text/plain'>This wasn't supposed to happen</Payload>
                                                                    <StatusCode>405</StatusCode>
                                                                    <ReasonPhrase>405 Rules</ReasonPhrase>
                                                                </Set>
                                                            </FaultResponse>
                                                            <IgnoreUnresolvedVariables>true</IgnoreUnresolvedVariables>
                                                        </RaiseFault>";

                        var faultResponse = new RestResponse()
                        {
                            StatusCode = HttpStatusCode.OK,
                            Content = faultResponseXml,
                            ContentType = "application/xml",
                        };

                        response = faultResponse;
                    }

                    #endregion

                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    returnData = JsonConvert.DeserializeObject<GetOrder>(response.Content, settings);
                }
                catch (Exception ex)
                {
                    // Handle  exceptions
                }
                

            }
            return returnData;
        }
        public async Task<bool> SaveSingleOrder(int LocationKey, int BUKy, PickmeEntity.data item,long ObjKy)
        {
            bool isSaved = false;
            int Lino = 0;
            List<int> UnavailableItemKey = new List<int>();
            PartnerOrder saveOrder = new PartnerOrder();
            saveOrder.Location.CodeKey = LocationKey;
            saveOrder.BU.CodeKey = BUKy;
            // saveOrder.CreatedBy.UserKey = 1;
            saveOrder.PartnerOrderId = 1;
            saveOrder.OrderId = item.PickmeJobID;
            saveOrder.OrderReference = "PCKM" + item.PickmeJobID;
            saveOrder.OrderDate = ConvertTimestamp(Convert.ToInt64(item.CreatedTimestamp)).ToString("yyyy/MM/dd hh:mm:ss tt");
            saveOrder.DeliveryBrand = "";
            saveOrder.IsActive = 1;
            saveOrder.IsApproved = 1;
            saveOrder.Platforms.AccountCode = "PickMe";
            saveOrder.Source = "View Orders";
            //string deliverytypecode = item.DeliveryMode;
            saveOrder.OrderStatus.CodeName = item.Status.Name;
            CodeBaseResponse code = new CodeBaseResponse()
            {
                CodeName = saveOrder.OrderStatus.CodeName,
                OurCode = saveOrder.Platforms.AccountCode
            };
            CodeBaseResponse record = await _OrderManager.GetOrderStatusByPartnerStatus(code);
            saveOrder.OrderStatus.CodeKey = record.CodeKey;
            CodeBaseResponse codeBaseResponse = new CodeBaseResponse()
            {
                ConditionCode = "PmtTrm",
                OurCode = "PickmeWallet"
            };
            CodeBaseResponse payment = await _codebaseManager.GetCodesByConditionCodeAndOurCode(codeBaseResponse);
            saveOrder.PaymentKey = payment.CodeKey;


            foreach (Items LineItem in item.Order.Items)
            {
                PartnerOrderDetails partnerOrderDetails = new PartnerOrderDetails();
                ItemResponse itemrec = new ItemResponse()
                {
                    ItemCode = LineItem.RefID.ToString()
                };
                partnerOrderDetails.OrderItem = await _OrderManager.GetItemByItemCode(itemrec);
                RequestParameters qtyreq = new RequestParameters()
                {
                    ItemKey = Convert.ToInt32(partnerOrderDetails.OrderItem.ItemKey),
                    LocationKey = LocationKey,
                    ObjKy = ObjKy

                };
                AvailableStock stockQty = await _OrderManager.GetAvailableQtyByItem(qtyreq);
                if (stockQty != null)
                {
                    if (stockQty.AvailableQty < Convert.ToDecimal(LineItem.Qty))
                    {
                        UnavailableItemKey.Add(Convert.ToInt32(partnerOrderDetails.OrderItem.ItemKey));
                    }
                }
                partnerOrderDetails.ItemQuantity = Convert.ToDecimal(LineItem.Qty);
                partnerOrderDetails.TransactionPrice = Convert.ToDecimal(LineItem.Total) / Convert.ToDecimal(LineItem.Qty);
                partnerOrderDetails.BaseTotalPrice = Convert.ToDecimal(LineItem.Total);
                partnerOrderDetails.SpecialInstructions = LineItem.SpIns;
                partnerOrderDetails.IsApproved = 1;
                partnerOrderDetails.IsActive = 1;
                partnerOrderDetails.Remarks = stockQty.AvailableQty < Convert.ToDecimal(LineItem.Qty) ? "Unavailable" : "";
                Lino = Lino + 1;
                partnerOrderDetails.OrderItem.LineNumber = Lino;
                saveOrder.OrderItemDetails.Add(partnerOrderDetails);
            }


            if (item.Customer != null)
            {
                saveOrder.Customer.Phone = item.Customer.ContactNumber;
                saveOrder.Customer.Address = item.Customer.Location.Address;
                saveOrder.Customer.AdrId = item.Customer.ContactNumber;
                saveOrder.Customer.Name = item.Customer.ContactNumber;
                await orderPlatformAPI.SaveCustomer(saveOrder);
            }
            if (UnavailableItemKey.Count > 0)
            {
                CodeBaseResponse unavailableresponse = new CodeBaseResponse()
                {
                    ConditionCode = "OrdSts",
                    OurCode = "Unavailable"
                };
                CodeBaseResponse Sts = await _codebaseManager.GetCodesByConditionCodeAndOurCode(unavailableresponse);
                saveOrder.OrderStatus.CodeKey = Sts.CodeKey;
            }
            PartnerOrder SavedData = await _OrderManager.SavePartnerOrders(saveOrder);
            if (SavedData.PartnerOrderId > 11)
            {
                isSaved = true;
            }
            else
            {
                isSaved = false;
            }
            return isSaved;
        }

    }
}
