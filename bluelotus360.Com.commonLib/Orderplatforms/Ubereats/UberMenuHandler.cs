using BL10.CleanArchitecture.Domain.Entities;
using BL10.CleanArchitecture.Domain.Entities.APIInfo;
using BlueLotus.Com.Domain.PartnerEntity;
using bluelotus360.Com.commonLib.Extensions;
using bluelotus360.Com.commonLib.Managers.OrderManager;
using bluelotus360.Com.commonLib.Routes;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.Com.Infrastructure.Managers.APIManager;
using BlueLotus360.Com.Infrastructure.Managers.Codebase;
using BlueLotus360.Com.Infrastructure.OrderPlatforms.Ubereats.UberEntities;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlueLotus360.Com.Infrastructure.OrderPlatforms.PickMe.PickmeEntity;
using static BlueLotus360.Com.Infrastructure.OrderPlatforms.Ubereats.UberEntities.UberMenuItems;

namespace BlueLotus360.Com.Infrastructure.OrderPlatforms.Ubereats
{
    public class UberMenuHandler
    {
        ICodebaseManager _codebaseManager;
        IAPIManager _apiManager;
        IOrderManager _orderManager;
        public UberMenuHandler(ICodebaseManager codebaseManager,IAPIManager apiManager,IOrderManager orderManager)
        {
            _codebaseManager = codebaseManager;
            _apiManager = apiManager;
            _orderManager = orderManager;
        }
        public async Task<bool> SetupUberMenuAndUpload(IList<PartnerMenuItem> filteredMenuListForUpload , string storeId, int LocationKey)
        {
            bool isSuccess = false;
            UberEatsMenu uberMenu = new UberEatsMenu();
                //items
                foreach (PartnerMenuItem menuItem in filteredMenuListForUpload)
                {
                    UberEatsItemsForMenu item = new UberEatsItemsForMenu();
                    item.Id = menuItem.ItemCode;
                    item.Description.Translations.En_us = menuItem.Description;
                    item.Title.Translations.En_us = menuItem.ItemName;
                    item.Price_info.Price = Decimal.ToInt32(menuItem.OptionalSalesPrice)*100;
                    item.External_data = menuItem.Description;
                    item.Image_url = menuItem.ItemImageUrl;

                    uberMenu.Items.Add(item);
                }

                //menus

                //------------------temporaraliy uploading a simple menu------------------------------------------------------------------------------------//
               CodeBaseResponse code= new CodeBaseResponse()
               {
                   ConditionCode="ShopHr"
               };
               IList<CodeBaseResponse> menuHours = await _codebaseManager.GetCodesByConditionCode(code);
                string[] daysOfWeek = { "monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday" };
                UberEatsTimePeriod timePeriod = new UberEatsTimePeriod
                {
                    Start_time = menuHours.Where(x => x.Code == "StartTm").FirstOrDefault().CodeName,
                    End_time = menuHours.Where(x => x.Code == "EndTm").FirstOrDefault().CodeName
                };

                UberEatsMenuItems uberEatsMenuItems = new UberEatsMenuItems();
                uberEatsMenuItems.Id = "All-day";
                uberEatsMenuItems.Title.Translations.En_us = "All day";
                foreach (string dayOfWeek in daysOfWeek)
                {
                    UberEatsServiceAvailability serviceAvailability = new UberEatsServiceAvailability();
                    serviceAvailability.Day_of_week = dayOfWeek;
                    serviceAvailability.Time_periods.Add(timePeriod);
                    uberEatsMenuItems.Service_availability.Add(serviceAvailability);
                }

                uberEatsMenuItems.Category_ids = filteredMenuListForUpload.Select(x => x.CategoryCode).Distinct().ToList();


                uberMenu.Menus.Add(uberEatsMenuItems);
                //------------------temporaraliy uploading a simple menu------------------------------------------------------------------------------------//

                //categories
                foreach (string categoryId in uberEatsMenuItems.Category_ids)
                {
                    UberEatsCategoryItems uberEatsCategoryItems = new UberEatsCategoryItems();
                    uberEatsCategoryItems.Id = categoryId;
                    uberEatsCategoryItems.Title.Translations.En_us = filteredMenuListForUpload.Where(x => x.CategoryCode == categoryId).Select(x => x.CategoryName).FirstOrDefault();

                    foreach (PartnerMenuItem item in filteredMenuListForUpload.Where(x => x.CategoryCode == categoryId).ToList())
                    {
                        MenuEntity menuEntity = new MenuEntity();
                        menuEntity.Id = item.ItemCode;
                        menuEntity.Type = MenuEntityType.ITEM.ToString();
                        uberEatsCategoryItems.Entities.Add(menuEntity);
                    }

                    uberMenu.Categories.Add(uberEatsCategoryItems);
                }




                //modifier groups

                //display options
                uberMenu.Display_options.Disable_item_instructions = false;

                //remove duplicates
                uberMenu.Items.Distinct();

            //send to uber
            isSuccess = await UploadMenuToUber(storeId, uberMenu);



            return isSuccess;
        }

        public async Task<bool> UploadMenuToUber(string storeId, UberEatsMenu uberMenu)
        {
            APIRequestParameters ApiParams = new APIRequestParameters()
            {
                EndPointName = UberTokenEndpoints.Eats_Store_Read_Scope.GetDescription()

            };
            APIInformation TokenInformation = await _apiManager.GenerateUberToken(ApiParams);
            APIRequestParameters EndPointParams = new APIRequestParameters()
            {
                EndPointName = UberEndpointURLS.UploadMenu.ToString()

            };
            APIInformation EndPointInfo = await _apiManager.GetUberEndPoints(EndPointParams);
           var client = new RestClient(TokenInformation.BaseURL);
            
            var request = new RestRequest(EndPointInfo.EndPointURL.Replace(UberRequestIDs.store_id.GetDescription(), storeId),Method.Put);
            request.AddHeader("Authorization", "Bearer " + TokenInformation.EndPointToken);
            request.AddHeader("Content-Type", "application/json");

            request.AddParameter("application/json", JsonConvert.SerializeObject(uberMenu), ParameterType.RequestBody);
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

        public async Task<bool> SyncItemsToUber(int LocationKey,int BUKy)
        {

                        /*
                        * 1. save item list in our db
                        * 2. filter isPartnerItems & not isDiscontinued Items
                        * 3. save filtered list in a global list state //failed when mulitiple companies comes
                        * 4. check for filtered count match and upload filtered items to uber
                        * 5. clear the global list state
                        */

            APIRequestParameters apiParameters = new APIRequestParameters()
            {
                LocationKey = LocationKey,
                BUKy=BUKy,
                APIIntegrationName = "UberEatsStore_" + LocationKey+"_"+BUKy,
                APIName = "",
                EndPointName = ""

            };
            APIInformation StoreInfo = await _apiManager.GetAPIInformation(apiParameters);
            RequestParameters rec = new RequestParameters()
            {
                LocationKey = LocationKey,
                PlatformName = BaseEndpoint.ProductImageURL
            };
            IList<PartnerMenuItem> item = await _orderManager.GetOrderItemsToUpload(rec);
            bool isSuccess=await SetupUberMenuAndUpload(item, StoreInfo.IntegrationId, LocationKey);
            return isSuccess;


        }

        public async Task<bool> GetAllMenuByStore()
        {
            APIRequestParameters ApiParams = new APIRequestParameters()
            {
                EndPointName = UberTokenEndpoints.Eats_Store_Read_Scope.GetDescription()

            };
            APIInformation TokenInformation = await _apiManager.GenerateUberToken(ApiParams);
            //APIRequestParameters EndPointParams = new APIRequestParameters()
            //{
            //    EndPointName = UberEndpointURLS.UploadMenu.ToString()

            //};
            string EndPointInfo = "/v2/eats/stores/{store_id}/menus";
            var client = new RestClient(TokenInformation.BaseURL);

            var request = new RestRequest(EndPointInfo.Replace(UberRequestIDs.store_id.GetDescription(), "7aebb081-d10e-50f6-adca-ba0d68fd78ee"), Method.Get);
            request.AddHeader("Authorization", "Bearer " + TokenInformation.EndPointToken);
            request.AddHeader("Content-Type", "application/json");
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

        public async Task<bool> UpdateItemToUber(int LocationKey,int BUKy,string ItmCd, UberEatsItemsForMenu menu)
        {
            APIRequestParameters apiParameters = new APIRequestParameters()
            {
                LocationKey = LocationKey,
                BUKy = BUKy,
                APIIntegrationName = "UberEatsStore_" + LocationKey + "_" + BUKy,
                APIName = "",
                EndPointName = ""

            };
            APIInformation StoreInfo = await _apiManager.GetAPIInformation(apiParameters);
            APIRequestParameters ApiParams = new APIRequestParameters()
            {
                EndPointName = UberTokenEndpoints.Eats_Store_Read_Scope.GetDescription()

            };
            APIInformation TokenInformation = await _apiManager.GenerateUberToken(ApiParams);
            APIRequestParameters EndPointParams = new APIRequestParameters()
            {
                EndPointName = UberEndpointURLS.UpdateItem.ToString()

            };
            APIInformation EndPointInfo = await _apiManager.GetUberEndPoints(EndPointParams);
            string EndpointURL = EndPointInfo.EndPointURL.Replace(UberRequestIDs.store_id.GetDescription(), StoreInfo.IntegrationId);
            EndPointInfo.EndPointURL= EndpointURL.Replace(UberRequestIDs.item_id.GetDescription(), ItmCd);
            var client = new RestClient(TokenInformation.BaseURL);

            var request = new RestRequest(EndPointInfo.EndPointURL, Method.Post);
            request.AddHeader("Authorization", "Bearer " + TokenInformation.EndPointToken);
            request.AddHeader("Content-Type", "application/json");

            request.AddParameter("application/json", JsonConvert.SerializeObject(menu), ParameterType.RequestBody);
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

    }
}
