using BL10.CleanArchitecture.Domain.Entities.APIInfo;
using BlueLotus.Com.Domain.PartnerEntity;
using bluelotus360.Com.commonLib.Extensions;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.Com.Infrastructure.Managers.APIManager;
using BlueLotus360.Com.Infrastructure.OrderPlatforms.Ubereats.UberEntities;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlueLotus360.Com.Infrastructure.OrderPlatforms.PickMe.PickmeEntity;

namespace BlueLotus360.Com.Infrastructure.OrderPlatforms.Ubereats
{
    public class UberStoreHandler
    {
        
        List<CodeBaseResponse> BLLocations = new List<CodeBaseResponse>();
        public readonly IAPIManager _Apimanager;
        public UberStoreHandler(IAPIManager aPIManager)
        {
            _Apimanager = aPIManager;
        }

        public async Task<IList<UberStore>> GetUberStoreDetails()
        {
            APIRequestParameters ApiParams = new APIRequestParameters()
            {
                EndPointName= UberTokenEndpoints.Eats_Store_Read_Scope.GetDescription()

            };
            APIInformation TokenInformation = await _Apimanager.GenerateUberToken(ApiParams);
            APIRequestParameters EndPointParams = new APIRequestParameters()
            {
                EndPointName = UberEndpointURLS.GetStoreList.ToString()

            };
            APIInformation EndPointInfo = await _Apimanager.GetUberEndPoints(EndPointParams);
            var client = new RestClient(TokenInformation.BaseURL ); //default limit of stores is 50 
            var request = new RestRequest(EndPointInfo.EndPointURL,Method.Get);
            request.AddHeader("Authorization", "Bearer " + TokenInformation.EndPointToken);
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute(request);
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            UberStoreListResponse uberStoreListResponse= JsonConvert.DeserializeObject<UberStoreListResponse>(response.Content, settings);
            return uberStoreListResponse.Stores;
        }

        public async Task<bool> SetUberStoreProvisioning(int LocationKey,string StoreID, UberProvisionSetupForStore Uberstoredata,int BUKy)
        {
            bool success = false;
            APIRequestParameters ApiParams = new APIRequestParameters()
            {
                EndPointName = UberTokenEndpoints.Eats_Store_Read_Scope.GetDescription(),
                LocationKey= LocationKey,
                BUKy=BUKy

            };
            APIInformation TokenInformation = await _Apimanager.GenerateUberToken(ApiParams); 
            APIRequestParameters EndPointParams = new APIRequestParameters()
            {
                EndPointName = UberEndpointURLS.ProvisionSetup.ToString()

            };
            APIInformation EndPointInfo = await _Apimanager.GetUberEndPoints(EndPointParams);

            var client = new RestClient(TokenInformation.BaseURL);
            var request = new RestRequest(EndPointInfo.EndPointURL.Replace(UberRequestIDs.store_id.GetDescription(), StoreID),Method.Patch);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + TokenInformation.EndPointToken);

            request.AddParameter("application/json", JsonConvert.SerializeObject(Uberstoredata), ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            if (response.IsSuccessful)
            {
                success= true;
            }
            else
            {
                success= false;
            }
            return success;
        }
    }
}
