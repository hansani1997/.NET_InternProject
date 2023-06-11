using BL10.CleanArchitecture.Domain.Entities.APIInfo;
using bluelotus360.Com.commonLib.Managers.Address;
using bluelotus360.Com.commonLib.Managers.OrderManager;
using bluelotus360.Com.commonLib.Routes;
using BlueLotus360.Com.Infrastructure.Managers.APIManager;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace BlueLotus360.Com.Infrastructure.OrderPlatforms.Ubereats
{
    public class UberProvisionHandler
    {
        public readonly IAPIManager _apiManager;
        public readonly IOrderManager _OrderManager;
        public readonly IAddressManager _addressManager;
        private readonly OrderPlatformAPIInformation orderPlatformAPI;

        public UberProvisionHandler(IAPIManager apiManager, IOrderManager orderManager, IAddressManager addressManager)
        {
            _apiManager = apiManager;
            _addressManager = addressManager;
            orderPlatformAPI = new OrderPlatformAPIInformation(_apiManager, _addressManager);
            _OrderManager = orderManager;
        }
        public async Task<string> SetupProvision()
        {
            string Link = "";
            APIRequestParameters apiParameters = new APIRequestParameters()
            {
                LocationKey = 1,
                BUKy=1,
                APIIntegrationName = "Ubereats",
                APIName = "Ubereats",
                EndPointName = ""

            };
            APIInformation tokenInfo = await orderPlatformAPI.GetAPIDetailsByEndpointName(apiParameters);
            if(tokenInfo != null)
             {
                APIRequestParameters Parameters = new APIRequestParameters()
                {
                    BaseURL=BaseEndpoint.BaseURL,
                    IntegrationID=tokenInfo.IntegrationId

                };
                Link= await _OrderManager.GenerateProvisionURL(Parameters);
            }
            return Link;
        }

      
    }
}
