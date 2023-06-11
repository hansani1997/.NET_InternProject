using BL10.CleanArchitecture.Domain.Entities;
using BL10.CleanArchitecture.Domain.Entities.APIInfo;
using bluelotus360.Com.commonLib.Managers.Address;
using BlueLotus360.CleanArchitecture.Domain.DTO.MasterData;
using BlueLotus360.Com.Infrastructure.Managers.APIManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.OrderPlatforms
{
    
    public class OrderPlatformAPIInformation
    {
        public readonly IAPIManager _apiManager;
        public readonly IAddressManager _addressManager;
        public OrderPlatformAPIInformation(IAPIManager apiManager, IAddressManager addressManager)
        {
            _apiManager = apiManager;
            _addressManager=addressManager;
        }

        public async Task<APIInformation> GetAPIDetailsByEndpointName(APIRequestParameters aPIRequest)
        {
            APIInformation commonApiInfo = await _apiManager.GetAPIInformation(aPIRequest);
            APIInformation returnInfo = new APIInformation();
            returnInfo.APIIntegrationKey = commonApiInfo.APIIntegrationKey;
            returnInfo.IntegrationId = commonApiInfo.IntegrationId;
            returnInfo.BaseURL = commonApiInfo.BaseURL;
            returnInfo.AlertnateBaseURL = commonApiInfo.AlertnateBaseURL;
            returnInfo.SecretInstanceKey = commonApiInfo.SecretInstanceKey;

            return returnInfo;


        }

        public async Task<PartnerOrder> SaveCustomer(PartnerOrder order)
        {
            AddressMaster adr = new AddressMaster()
            {
                AddressID= order.Customer.AdrId
            };
           AddressMaster addressMaster= await _addressManager.CheckAdvanceAnalysisAvailability(adr);
            if(addressMaster!= null && addressMaster.AddressKey > 11)
            {
                order.Customer.AdrKy =Convert.ToInt32(addressMaster.AddressKey);
                order.Customer.AdrId = addressMaster.AddressID;
                order.Customer.Name = addressMaster.AddressName;
                order.Customer.Address = addressMaster.Address;
                order.Customer.Phone = addressMaster.Mobile;
            }
            else
            {
                AddressMaster advanl = new AddressMaster();
                advanl.AddressID = order.Customer.AdrId;
                advanl.AddressName = order.Customer.Name;
                advanl.Address = order.Customer.Address;
                advanl.Mobile = order.Customer.Phone;
                advanl.IsActive = 1;   
                
                AddressMaster adrmst = await _addressManager.CreateAdvanceAnalysis(advanl);
                order.Customer.AdrKy = Convert.ToInt32(adrmst==null ? 1: adrmst.AddressKey);
            }

            return order;
        }
    }
}
