using BL10.CleanArchitecture.Domain.Entities;
using BL10.CleanArchitecture.Domain.Entities.APIInfo;
using BlueLotus.Com.Domain.PartnerEntity;
using bluelotus360.Com.commonLib.Extensions;
using bluelotus360.Com.commonLib.Managers.OrderManager;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.Com.Infrastructure.Managers.APIManager;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.OrderPlatforms.Ubereats
{
    public class UberOrderHandler
    {
        private bool IsAcceptedInUber { get; set; }
        IAPIManager _apiManager;
        IOrderManager _orderManager;

        public UberOrderHandler(IAPIManager apiManager, IOrderManager orderManager)
        {
            _apiManager = apiManager;
            _orderManager = orderManager;
        }
       
        public async Task<bool> DenyUberOrderByOrderId(string orderId, string reasonCode)
        {
            APIRequestParameters ApiParams = new APIRequestParameters()
            {
                EndPointName = UberTokenEndpoints.Eats_Order_Write_Scope.GetDescription()

            };
            APIInformation TokenInformation = await _apiManager.GenerateUberToken(ApiParams);
            APIRequestParameters EndPointParams = new APIRequestParameters()
            {
                EndPointName = UberEndpointURLS.DenyOrder.ToString()

            };
            APIInformation EndPointInfo = await _apiManager.GetUberEndPoints(EndPointParams);
           
            var client = new RestClient(TokenInformation.BaseURL);
            
            var request = new RestRequest(EndPointInfo.EndPointURL.Replace(UberRequestIDs.order_id.GetDescription(), orderId),Method.Post);
            request.AddHeader("Authorization", "Bearer " + TokenInformation.EndPointToken);
            request.AddHeader("Content-Type", "application/json");
            ReasonDeny reason = new ReasonDeny();
            reason.Reason.Explanation = "Order has been denied.";
            reason.Reason.Code = reasonCode;
            request.AddParameter("application/json", JsonConvert.SerializeObject(reason), ParameterType.RequestBody);
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

        public async Task<bool> AcceptUberOrderByOrderId(string orderId)
        {
            APIRequestParameters ApiParams = new APIRequestParameters()
            {
                EndPointName = UberTokenEndpoints.Eats_Order_Write_Scope.GetDescription()

            };
            APIInformation TokenInformation = await _apiManager.GenerateUberToken(ApiParams);
            APIRequestParameters EndPointParams = new APIRequestParameters()
            {
                EndPointName = UberEndpointURLS.AcceptOrder.ToString()

            };
            APIInformation EndPointInfo = await _apiManager.GetUberEndPoints(EndPointParams);
           
            var client = new RestClient(TokenInformation.BaseURL);
            var request = new RestRequest(EndPointInfo.EndPointURL.Replace(UberRequestIDs.order_id.GetDescription(), orderId),Method.Post);
            request.AddHeader("Authorization", "Bearer " + TokenInformation.EndPointToken);
            request.AddHeader("Content-Type", "application/json");
            var body = @"{" + "\n" +
            @"   ""reason"": ""accepted""" + "\n" +
            @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
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

        public async Task<bool> CancelUberOrderByOrderId(string orderId, string reasonCode)
        {
            APIRequestParameters ApiParams = new APIRequestParameters()
            {
                EndPointName = UberTokenEndpoints.Eats_Order_Write_Scope.GetDescription()

            };
            APIInformation TokenInformation = await _apiManager.GenerateUberToken(ApiParams);
            APIRequestParameters EndPointParams = new APIRequestParameters()
            {
                EndPointName = UberEndpointURLS.CancelOrder.ToString()

            };
            APIInformation EndPointInfo = await _apiManager.GetUberEndPoints(EndPointParams);

          
            var client = new RestClient(TokenInformation.BaseURL);
            var request = new RestRequest(EndPointInfo.EndPointURL.Replace(UberRequestIDs.order_id.GetDescription(), orderId),Method.Post);
            request.AddHeader("Authorization", "Bearer " + TokenInformation.EndPointToken);
            request.AddHeader("Content-Type", "application/json");
            //var body = @"{" + "\n" +
            //@"   ""reason"": "+reasonCode+"" + "\n" +
            //@"}";
            ReasonForCancel reason = new ReasonForCancel();
            reason.Reason = reasonCode;
            reason.Details = "No Details";
            request.AddParameter("application/json", JsonConvert.SerializeObject(reason), ParameterType.RequestBody);
            RestResponse response = client.Execute(request);

            Console.WriteLine(response.Content);

            if (response.IsSuccessful)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task<UberOrder> GetUberDetailsByOrderID(string OrderID)
        {
            UberOrder UberOrder = new UberOrder();
            try
            {
                APIRequestParameters ApiParams = new APIRequestParameters()
                {
                    EndPointName = UberTokenEndpoints.Eats_Order_Read_Scope.GetDescription()

                };
                APIInformation TokenInformation = await _apiManager.GenerateUberToken(ApiParams);
                if (TokenInformation != null)
                {
                    APIRequestParameters EndPointParams = new APIRequestParameters()
                    {
                        EndPointName = UberEndpointURLS.GetOrder.ToString()

                    };
                    APIInformation EndPointInfo = await _apiManager.GetUberEndPoints(EndPointParams);

                    var client = new RestClient(TokenInformation.BaseURL);
                    var request = new RestRequest(EndPointInfo.EndPointURL.Replace(UberRequestIDs.order_id.GetDescription(), OrderID), Method.Get);
                    request.AddHeader("Authorization", "Bearer " + TokenInformation.EndPointToken);
                    request.AddHeader("Content-Type", "application/json");
                    RestResponse response = client.Execute(request);
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    if (response.IsSuccessful)
                    {
                        UberOrder = JsonConvert.DeserializeObject<UberOrder>(response.Content, settings);
                        
                    }
                }
                return UberOrder;
            }

            catch (Exception exception)
            {
                throw exception;
            }



        }

        public async Task<bool> UpdateUberCart(UpdateCart fulfillmentIssue,string OrderID)
        {
            bool success = false;
            try
            {
                APIRequestParameters ApiParams = new APIRequestParameters()
                {
                    EndPointName = UberTokenEndpoints.Eats_Order_Write_Scope.GetDescription()

                };
                APIInformation TokenInformation = await _apiManager.GenerateUberToken(ApiParams);
                if (TokenInformation != null)
                {
                    APIRequestParameters EndPointParams = new APIRequestParameters()
                    {
                        EndPointName = UberEndpointURLS.PatchCart.ToString()

                    };
                    APIInformation EndPointInfo = await _apiManager.GetUberEndPoints(EndPointParams);
                    var client = new RestClient(TokenInformation.BaseURL);
                    var request = new RestRequest(EndPointInfo.EndPointURL.Replace(UberRequestIDs.order_id.GetDescription(), OrderID), Method.Patch);
                    request.AddHeader("Authorization", "Bearer " + TokenInformation.EndPointToken);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddParameter("application/json", JsonConvert.SerializeObject(fulfillmentIssue), ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    if (response.IsSuccessful)
                    {

                        success = true;
                    }
                    
                }
                return success;
            }

            catch (Exception exception)
            {
                throw exception;
            }



        }
    }
}
