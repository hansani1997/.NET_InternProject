using BL10.CleanArchitecture.Domain.Entities.APIInfo;
using BlueLotus.Com.Domain.PartnerEntity;
using bluelotus360.Com.commonLib.Extensions;
using bluelotus360.Com.commonLib.Managers.Address;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.Com.Infrastructure.Managers.APIManager;
using BlueLotus360.Com.Infrastructure.Managers.Codebase;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlueLotus360.Com.Infrastructure.OrderPlatforms.PickMe.PickmeEntity;

namespace BlueLotus360.Com.Infrastructure.OrderPlatforms.PickMe
{
    public class PickmeStockPriceUpdateHandler
    {
        private readonly OrderPlatformAPIInformation orderPlatformAPI;
        public readonly IAPIManager _apiManager;
        public readonly IAddressManager _addressManager;
        PickmeMenu Menu = new PickmeMenu();
        public PickmeStockPriceUpdateHandler(IAPIManager apiManager,  IAddressManager addressManager)
        {
            _apiManager = apiManager;
            _addressManager = addressManager;
            orderPlatformAPI = new OrderPlatformAPIInformation(_apiManager, _addressManager);
        }
        public async Task<PickmeMenu> GetPickItems(int LocationKey,int BUKy)
        {
            APIRequestParameters apiParameters = new APIRequestParameters()
            {
                LocationKey = LocationKey,
                BUKy = BUKy,
                APIIntegrationName = "PickMe_" + LocationKey + "_" + BUKy,
                APIName = "PickMe",
                EndPointName = ""

            };
            APIInformation tokenInfo = await orderPlatformAPI.GetAPIDetailsByEndpointName(apiParameters);
            if (tokenInfo != null && tokenInfo.APIIntegrationKey > 11)
            {
                APIRequestParameters endpointrequest = new APIRequestParameters()
                {
                    LocationKey = LocationKey,
                    BUKy = BUKy,
                    APIIntegrationKey = tokenInfo.APIIntegrationKey,
                    EndPointName = PickmeEndpoints.PickMeiTemRead.GetDescription()
                };
                APIInformation endpointInfo = await _apiManager.GetAPIEndPoints(endpointrequest);
                var client = new RestClient(tokenInfo.BaseURL);
                //  hour = hour + 1;
                var request = new RestRequest(endpointInfo.EndPointURL, Method.Get);
                request.AddHeader("X-API-KEY", tokenInfo.SecretInstanceKey.Replace("\r", "").Replace("\n", "").Replace("\t", ""));
                request.AddHeader("Content-Type", "application/json");
                var response = client.Execute(request);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                Menu =  JsonConvert.DeserializeObject<PickmeMenu>(response.Content, settings);
                
            }
            return Menu;
        }
        public async Task<bool> UpdatePickMeMenu(int LocationKey, int BUKy,string ItemID,decimal Price,string Code)
        {
            bool success = false;
            APIRequestParameters apiParameters = new APIRequestParameters()
            {
                LocationKey = LocationKey,
                BUKy = BUKy,
                APIIntegrationName = "PickMe_" + LocationKey + "_" + BUKy,
                APIName = "PickMe",
                EndPointName = ""

            };
            APIInformation tokenInfo = await orderPlatformAPI.GetAPIDetailsByEndpointName(apiParameters);
            if (tokenInfo != null && tokenInfo.APIIntegrationKey > 11)
            {
                APIRequestParameters endpointrequest = new APIRequestParameters()
                {
                    LocationKey = LocationKey,
                    BUKy = BUKy,
                    APIIntegrationKey = tokenInfo.APIIntegrationKey,
                    EndPointName = PickmeEndpoints.PickMeItemUpdate.GetDescription()
                };
                APIInformation endpointInfo = await _apiManager.GetAPIEndPoints(endpointrequest);
                ItemUpdate item = new ItemUpdate()
                {
                    MerchantStatus= Code =="Not Available" ? 0 : Code =="Available" ? 1 : 2,
                    Price=Price
                };
                var client = new RestClient(tokenInfo.BaseURL);
                //  hour = hour + 1;
                var request = new RestRequest(endpointInfo.EndPointURL.Replace(PickmeEndpoints.PickMeItemID.GetDescription(), ItemID), Method.Post);
                request.AddHeader("X-API-KEY", tokenInfo.SecretInstanceKey.Replace("\r", "").Replace("\n", "").Replace("\t", ""));
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", JsonConvert.SerializeObject(item), ParameterType.RequestBody);
                var response = client.Execute(request);
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                if (response.IsSuccessful)
                {
                    success = true;
                }
                else
                {
                    success = false;
                }

            }
            return success;
        }
    }
}
