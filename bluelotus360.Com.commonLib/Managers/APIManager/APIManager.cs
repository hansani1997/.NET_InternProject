using BL10.CleanArchitecture.Domain.Entities;
using BL10.CleanArchitecture.Domain.Entities.APIInfo;
using BL10.CleanArchitecture.Shared.Constants;
using bluelotus360.Com.commonLib.Routes;
using bluelotus360.Com.commonLib.Services.Definition;
using bluelotus360.Com.MauiSupports.Services.MAUISecureStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.Managers.APIManager
{
    public class APIManager:IAPIManager
    {
        private readonly HttpClient _httpClient;
        //private readonly ISecureStorageService _localStorage;
        private readonly IStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public APIManager(
            HttpClient httpClient,
            IStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
          

            if (_httpClient.DefaultRequestHeaders.Count() == 0)
                _httpClient.DefaultRequestHeaders.Add("IntegrationID", GlobalConsts.intergrationId);
        }

        public async Task<APIInformation> GetAPIInformation(APIRequestParameters parameters)
        {
            APIInformation apiData = new APIInformation();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetAPIInfo, parameters);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<APIInformation>(content);
                apiData = obj;


            }
            catch (Exception exp)
            {

            }
            return apiData;
        }
        public async Task<APIInformation> GetAPIEndPoints(APIRequestParameters parameters)
        {
            APIInformation apiData = new APIInformation();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetAPIEndPoints, parameters);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<APIInformation>(content);
                apiData = obj;


            }
            catch (Exception exp)
            {

            }
            return apiData;
        }

        public async Task<APIInformation> GenerateUberToken(APIRequestParameters request)
        {
            APIInformation apiinfo = new APIInformation();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetUberToken, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<APIInformation>(content);
                apiinfo = obj;

            }
            catch (Exception exp)
            {

            }
            return apiinfo;
        }

        public async Task<APIInformation> GetUberEndPoints(APIRequestParameters request)
        {
            APIInformation apiinfo = new APIInformation();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetUberEndPoints, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<APIInformation>(content);
                apiinfo = obj;

            }
            catch (Exception exp)
            {

            }
            return apiinfo;
        }

        public async Task<APIInformation> GetApiDetailsByMerchantID(APIRequestParameters request)
        {
            APIInformation apiinfo = new APIInformation();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetApiDetailsByMerchantID, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<APIInformation>(content);
                apiinfo = obj;

            }
            catch (Exception exp)
            {

            }
            return apiinfo;
        }
    }
}
