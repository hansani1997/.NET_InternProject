using BL10.CleanArchitecture.Domain.Entities.Dashboard;
using BL10.CleanArchitecture.Domain.DTO;
using BL10.CleanArchitecture.Domain.Entities.ItemProfleMobile;
using BL10.CleanArchitecture.Shared.Constants;
using BlueLotus.Com.Domain.Entity;
using bluelotus360.Com.commonLib.Routes;
using bluelotus360.Com.MauiSupports.Services.MAUISecureStorage;
using BlueLotus360.CleanArchitecture.Application.Responses.ServerResponse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using bluelotus360.Com.commonLib.Services.Definition;

namespace bluelotus360.Com.commonLib.Managers.ItemProfileMobile
{
    public class ItemProfileMobileManagerV3 : IItemProfileMobileManagerV3
    {
        private readonly HttpClient _httpClient;
        private bool _checkIfExceptionReturn;
        private readonly IHttpClientFactory _factory;

        //private readonly ISecureStorageService _localStorage;
        private readonly IStorageService _localStorage;

        public ItemProfileMobileManagerV3(
           HttpClient httpClient,
           IStorageService localStorage,
            IHttpClientFactory factory)
        {
            _httpClient = httpClient;
            _factory = factory;
            _localStorage = localStorage;

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

        //public async Task<IList<ItemSelectListV3>> GetItemProfileListV3(ItemSelectListRequestV3 request)
        //{

        //    IList<ItemSelectListV3> itemSelectListResponse = new List<ItemSelectListV3>();

        //    try
        //    {
        //        var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetItemProfileSelectListV3, request);
        //        await response.Content.LoadIntoBufferAsync();
        //        string content = response.Content.ReadAsStringAsync().Result;
        //        itemSelectListResponse = JsonConvert.DeserializeObject<List<ItemSelectListV3>>(content);

        //        //var cl = _factory.CreateClient();
        //        //AssignClientData(cl);
        //        //var response = await cl.PostAsJsonAsync(TokenEndpoints.GetItemProfileSelectList, request);
        //        //string content = await response.Content.ReadAsStringAsync();
        //        //itemSelectListResponse = JsonConvert.DeserializeObject<IList<ItemSelectList>>(content);
        //    }
        //    catch (Exception exp)
        //    {
        //        _checkIfExceptionReturn = true;
        //        itemSelectListResponse = new List<ItemSelectListV3>();
        //    }
        //    finally
        //    {

        //    }

        //    return itemSelectListResponse;
        //}

        public async Task<ItemSelectListV3> GetItemProfileListV3(ItemSelectListRequestV3 request)
        {

            ItemSelectListV3 itemSelectListResponse = new ItemSelectListV3();
            ApiServerResponse<ItemSelectListV3> apiResponse = new ApiServerResponse<ItemSelectListV3>();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetItemProfileSelectListV3, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                apiResponse = JsonConvert.DeserializeObject<ApiServerResponse<ItemSelectListV3>>(content);

                if (apiResponse != null && apiResponse.ExecutionException is null)
                {
                    itemSelectListResponse = apiResponse.Value;
                }
                else
                {
                    itemSelectListResponse = new ItemSelectListV3();
                    throw new Exception("An Exception has been returned ,pls check");

                }

            }
            catch (Exception exp)
            {
                //_checkIfExceptionReturn = true;
                //itemSelectListResponse = new List<ItemSelectListV3>();
            }
            finally
            {

            }

            return itemSelectListResponse;
        }

        public async Task<Item> InsertItemV3(Item request)
        {
            Item response = new Item();

            try
            {
                var data = await _httpClient.PostAsJsonAsync(TokenEndpoints.InsertItemV3, request);
                await data.Content.LoadIntoBufferAsync();
                string content = data.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<Item>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                response = new Item();
            }
            finally
            {

            }
            return response;
        }

        public async Task<string> GetItemServerfilterDetails(BaseServerFilterInfo request)
        {
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(request.RequestingURL, request);
                string content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                return null;
            }
        }
        public bool IsExceptionthrown()
        {
            if (_checkIfExceptionReturn)
                return true;
            return false;
        }

        //select single item: mainGird

        public async Task<Item> SelectSingleItem(Item request)
        {
            Item singleItemSelectResponse = new Item();

            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.SelectSingleItem, request);
                //await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                singleItemSelectResponse = JsonConvert.DeserializeObject<Item>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                singleItemSelectResponse = new Item();
            }
            finally
            {

            }
            return singleItemSelectResponse;
        }
       

        //multiunitTab

        //public async Task<IList<Item>> GetMultiUnitsGridDetails(Item request)
        //{
        //    IList<Item> multiUnitGridDetailsResponse = new List<Item>();

        //    try
        //    {
        //        var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetMultiUnitsGridDetails, request);
        //        await response.Content.LoadIntoBufferAsync();
        //        string content = response.Content.ReadAsStringAsync().Result;
        //        multiUnitGridDetailsResponse = JsonConvert.DeserializeObject<IList<Item>>(content);
        //    }
        //    catch (Exception exp)
        //    {
        //        _checkIfExceptionReturn = true;
        //        multiUnitGridDetailsResponse = new List<Item>();
        //    }
        //    finally
        //    {

        //    }
        //    return multiUnitGridDetailsResponse;
        //}

        public async Task MultiUnitsUpdate(ItemUnit request)
        {
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.MultiUnitsUpdateEndPoint, request);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
            }
            finally
            {

            }
        }
        public async Task MultiUnitsInsert(ItemUnit request)
        {
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.MultiUnitsInsertEndPoint, request);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
            }
            finally
            {

            }
        }

        public async Task<Item> UpdateItem(Item request)
        {
            Item UpdateItemResponse = new Item();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.UpdateItemV3, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                UpdateItemResponse = JsonConvert.DeserializeObject<Item>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                UpdateItemResponse = new Item();
            }
            finally
            {

            }
            return UpdateItemResponse;
        }

        public async Task<ItemCombinations> GenerateCombinations(ItemCombinations request)
        {
            ItemCombinations response = new ItemCombinations();

            try
            {
                var data = await _httpClient.PostAsJsonAsync(TokenEndpoints.GenerateCombinations, request);
                await data.Content.LoadIntoBufferAsync();
                string content = data.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<ItemCombinations>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                response = new ItemCombinations();
            }
            finally
            {

            }
            return response;
        }

        public async Task<IList<Item>> GetCombinationItems(Item request)
        {
            IList<Item> getCombinationItemsResponse = new List<Item>();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetCombinationItems, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                getCombinationItemsResponse = JsonConvert.DeserializeObject<IList<Item>>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                getCombinationItemsResponse = new List<Item>();
            }
            finally
            {

            }
            return getCombinationItemsResponse;
        }

        public async Task CreateComponents(ItemComponent request)
        {
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.GenerateCombinationComponent, request);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
            }
            finally
            {

            }
        }

        public async Task UpdateItemComponent(ItemComponent request)
        {
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.UpdateItemComponent, request);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
            }
            finally
            {

            }
        }

        public async Task<ItemComponent> GetSingleItemComponent(ItemComponent request)
        {
            ItemComponent singleItemSelectResponse = new ItemComponent();

            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.GetSingleItemComponent, request);
                //await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                singleItemSelectResponse = JsonConvert.DeserializeObject<ItemComponent>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                singleItemSelectResponse = new ItemComponent();
            }
            finally
            {

            }
            return singleItemSelectResponse;

        }

        public async Task DeleteItemComponent(ItemComponent request)
        {
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.DeleteItemComponent, request);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
            }
            finally
            {

            }
        }

        public async Task<IList<ItemComponent>> GetItemComponents(Item request)
        {
            IList<ItemComponent> componentList = new List<ItemComponent>();

            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.GetItemComponentListEndPoint, request);
                string content = await response.Content.ReadAsStringAsync();
                componentList = JsonConvert.DeserializeObject<IList<ItemComponent>>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                componentList = new List<ItemComponent>();
            }
            finally
            {

            }
            return componentList;

        }

        
    }


}
