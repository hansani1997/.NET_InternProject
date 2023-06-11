using BL10.CleanArchitecture.Domain.Entities.ItemProfleMobile;
using BL10.CleanArchitecture.Shared.Constants;
using bluelotus360.Com.commonLib.Routes;
using bluelotus360.Com.commonLib.Services.Definition;
using bluelotus360.Com.MauiSupports.Services.MAUISecureStorage;
using BlueLotus360.Com.Shared.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Managers.ItemProfileMobile
{
    public class ItemProfileMobileManager : IItemProfileMobileManager
    {
        private readonly HttpClient _httpClient;
        private bool _checkIfExceptionReturn;
        private readonly IHttpClientFactory _factory;

        //private readonly ISecureStorageService _localStorage; 
        private readonly IStorageService _localStorage;

        public ItemProfileMobileManager(
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

        //getItemList
        public async Task<IList<ItemSelectList>> GetItemProfileList(ItemSelectListRequest request)
        {
            IList<ItemSelectList> itemSelectListResponse = new List<ItemSelectList>();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetItemProfileSelectList, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                itemSelectListResponse = JsonConvert.DeserializeObject<List<ItemSelectList>>(content);

                //var cl = _factory.CreateClient();
                //AssignClientData(cl);
                //var response = await cl.PostAsJsonAsync(TokenEndpoints.GetItemProfileSelectList, request);
                //string content = await response.Content.ReadAsStringAsync();
                //itemSelectListResponse = JsonConvert.DeserializeObject<IList<ItemSelectList>>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                itemSelectListResponse = new List<ItemSelectList>();
            }
            finally
            {

            }

            return itemSelectListResponse;
        }


        //InsertItem
        public async Task<ItemSelectList> InsertItem(ItemSelectList request)
        {
            ItemSelectList response = new ItemSelectList();

            try
            {
                var data = await _httpClient.PostAsJsonAsync(TokenEndpoints.InsertItem, request);
                await data.Content.LoadIntoBufferAsync();
                string content = data.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<ItemSelectList>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                response = new ItemSelectList();
            }
            finally
            {

            }
            return response;
        }

        public bool IsExceptionthrown()
        {
            if (_checkIfExceptionReturn)
                return true;
            return false;
        }


        //UpdateItem
        public async Task<ItemSelectList> UpdateItem(ItemSelectList request)
        {
            ItemSelectList response = new ItemSelectList();

            try
            {
                var data = await _httpClient.PostAsJsonAsync(TokenEndpoints.UpdateItem, request);
                await data.Content.LoadIntoBufferAsync();
                string content = data.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<ItemSelectList>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                response = new ItemSelectList();
            }
            finally
            {

            }
            return response;
        }
    }
}
