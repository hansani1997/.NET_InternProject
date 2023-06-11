using AutoMapper.Configuration;
using BL10.CleanArchitecture.Domain.DTO;
using BL10.CleanArchitecture.Shared.Constants;
using bluelotus360.Com.commonLib.Routes;
using BlueLotus360.CleanArchitecture.Domain.Entities.AccountProfile;
using BlueLotus360.Com.Shared.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Managers.AccountProfile
{
    public class AccountProfileManager : IAccountProfileManager
    {
        private readonly HttpClient _httpClient;
        private bool _checkIfExceptionReturn;
        private readonly IHttpClientFactory _factory;

        public AccountProfileManager(HttpClient httpClient, IHttpClientFactory factory)
        {
            _httpClient = httpClient;
            _factory = factory;

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

        public async Task<IList<AccountProfileResponse>> GetAccountProfileList(AccountProfileRequest request)
        {
            List<AccountProfileResponse> stockList = new List<AccountProfileResponse>();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetAccountProfileList, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                stockList = JsonConvert.DeserializeObject<List<AccountProfileResponse>>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                stockList = new List<AccountProfileResponse>();
            }
            finally
            {

            }

            return stockList;
        }

        public async Task<AccountProfileInsertResponse> InsertAccountProfile(AccountProfileInsertRequest request)
        {
            AccountProfileInsertResponse responseAccount = new AccountProfileInsertResponse();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.InsertAccountProfileItem, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                responseAccount = JsonConvert.DeserializeObject<AccountProfileInsertResponse>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                responseAccount = new AccountProfileInsertResponse();
            }
            finally
            {

            }
            return responseAccount;
        }

        public bool IsExceptionthrown()
        {
            if (_checkIfExceptionReturn)
                return true;
            return false;
        }

        public async Task<AccountProfileResponse> UpdatedAccountProfile(AccountProfileResponse request)
        {
            AccountProfileResponse response = new AccountProfileResponse();

            try
            {
                var responsedata = await _httpClient.PostAsJsonAsync(TokenEndpoints.UpdateAccountProfile, request);
                await responsedata.Content.LoadIntoBufferAsync();
                string content = responsedata.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<AccountProfileResponse>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                response = new AccountProfileResponse();
            }
            finally
            {

            }
            return response;
        }

        #region Account Profile Version 2 functions

        //Get Main Grid Details
        public async Task<IList<AccountProfileResponse>> GetAccountProfileMainGridDetails(BaseServerFilterInfo request)
        {
            IList<AccountProfileResponse> AccountProfileGridListResponse = new List<AccountProfileResponse>();

            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(request.RequestingURL, request);
                string content = await response.Content.ReadAsStringAsync();
                AccountProfileGridListResponse = JsonConvert.DeserializeObject<IList<AccountProfileResponse>>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                AccountProfileGridListResponse = new List<AccountProfileResponse>();
            }
            finally
            {

            }

            return AccountProfileGridListResponse;
        }

        //Insert Details
        public async Task<AccountProfileResponse> InsertAccountProfileDetails(AccountProfileResponse request)
        {
            AccountProfileResponse responseAccount = new AccountProfileResponse();

            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.InsertAccountProfile, request);
                string content = await response.Content.ReadAsStringAsync();
                responseAccount = JsonConvert.DeserializeObject<AccountProfileResponse>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                responseAccount = new AccountProfileResponse();

            }
            finally
            {

            }
            return responseAccount;
        }

        //select single account record

        public async Task<AccountProfileResponse> SelectSignleAccountRecord(AccountProfileResponse request)
        {
            AccountProfileResponse singleAccountRecord = new AccountProfileResponse();

            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.SelectAccountRecord, request);
                string content =await  response.Content.ReadAsStringAsync();
                singleAccountRecord = JsonConvert.DeserializeObject<AccountProfileResponse>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                singleAccountRecord = new AccountProfileResponse();
            }
            finally
            {

            }
            return singleAccountRecord;
        }

        //Update Account Details
        public async Task<AccountProfileResponse> UpdateAccountProfileDetails(AccountProfileResponse request)
        {
            AccountProfileResponse response = new AccountProfileResponse();

            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var responsedata = await cl.PostAsJsonAsync(TokenEndpoints.UpdateAccountProfileRecord, request);
                string content =await  responsedata.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<AccountProfileResponse>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                response = new AccountProfileResponse();
            }
            finally
            {

            }
            return response;
        }
        #endregion
    }
}
