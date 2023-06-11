using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using bluelotus360.Com.MauiSupports.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueLotus360.Com.Shared.Constants.Storage;
using BlueLotus360.Com.Shared.Constants;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using bluelotus360.Com.commonLib.Authentication;
using bluelotus360.Com.commonLib.Routes;
using bluelotus360.Com.MauiSupports.Services.MAUISecureStorage;
using BlueLotus360.Com.Application.Responses.Identity;
using BL10.CleanArchitecture.Shared.Constants;
using BlueLotus360.CleanArchitecture.Domain.DTO.Report;
using bluelotus360.Com.commonLib.Services.Definition;

namespace bluelotus360.Com.commonLib.Managers.Company
{
    public class CompanyManager:ICompanyManager
    {
        //private RestClient _restClient;
        private readonly HttpClient _httpClient;
        //private ISecureStorageService _localStorage;
        private IStorageService _localStorage;
        private AuthenticationStateProvider _authenticationStateProvider;

        public CompanyManager(HttpClient httpClient,
            IStorageService localStorage, 
            AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;

            if (_httpClient.DefaultRequestHeaders.Count() == 0)
                _httpClient.DefaultRequestHeaders.Add("IntegrationID", GlobalConsts.intergrationId);
        }

        public async Task<IList<CompanyResponse>> GetUserCompanies()
        {
            CompanyResponse resp = new CompanyResponse();
            var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.CompanyListingEndPoint, resp);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                IList<CompanyResponse> companies = JsonConvert.DeserializeObject<IList<CompanyResponse>>(content);
                return companies;
            }
            else
            {
                return new List<CompanyResponse>();
            }

        }
        public async Task<ReportCompanyDetailsResponse> GetCompanyDetailsResponse()
        {
            CompanyResponse resp = new CompanyResponse();
            var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.CompanyReportInformationEndPoint, resp);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                ReportCompanyDetailsResponse companies = JsonConvert.DeserializeObject<ReportCompanyDetailsResponse>(content);
                return companies;
            }
            else
            {
                return new ReportCompanyDetailsResponse();
            }

        }

        public async Task UpdateSelectedCompany(CompanyResponse companyResponse)
        {
            var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.CompanySelectedEndPoint, companyResponse);
            string content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TokenResponse>(content);

            // var result = await response.ToResult<TokenResponse>();
            if (result.IsSuccess)
            {
                var token = result.Token;
                var refreshToken = result.RefreshToken;
                var userImageURL = "";
                await _localStorage.SetItemAsync(StorageConstants.Local.AuthToken, token);
                //_localStorage.SetItem(StorageConstants.Local.RefreshToken, refreshToken);
                await _localStorage.SetItemAsync(StorageConstants.Local.CompanyName, companyResponse.CompanyName);
                if (!string.IsNullOrEmpty(userImageURL))
                {
                    await _localStorage.SetItemAsync(StorageConstants.Local.UserImageURL, userImageURL);
                }

                await ((BL10AuthProvider)this._authenticationStateProvider).StateChangedAsync();

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            }
            else
            {

            }
        }

        //public async Task<IList<CompanyResponse>> GetUserCompanies()
        //{
        //    RestClientOptions rco = new RestClientOptions(TokenEndpoints.CompanyListingEndPoint)
        //    {
        //        ThrowOnAnyError = true,
        //    };
        //    _restClient = new RestClient(rco);
        //    var request = new RestRequest();
        //    CompanyResponse crs = new CompanyResponse();
        //    request.AddJsonBody(crs);
        //    request.Method = Method.Post;
        //    request.AddHeader("IntegrationID", GlobalConsts.intergrationId);
        //    string token = await _secureStorageService.GetItem(StorageConstants.Local.AuthToken);
        //    request.AddHeader("Authorization", "Bearer " + token);
        //    var response = await _restClient.PostAsync(request);
        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        string content = response.Content.ToString();
        //        IList<CompanyResponse> companies = JsonConvert.DeserializeObject<IList<CompanyResponse>>(content);
        //        return companies;
        //    }
        //    else
        //    {
        //        return new List<CompanyResponse>();
        //    }

        //}

        //public async Task UpdateSelectedCompany(CompanyResponse companyResponse)
        //{
        //    RestClientOptions rco = new RestClientOptions(TokenEndpoints.CompanySelectedEndPoint)
        //    {
        //        ThrowOnAnyError = true,
        //    };
        //    _restClient = new RestClient(rco);
        //    var request = new RestRequest();
        //    request.AddJsonBody(companyResponse);
        //    request.Method = Method.Post;
        //    request.AddHeader("IntegrationID", GlobalConsts.intergrationId);
        //    string token = await _secureStorageService.GetItem(StorageConstants.Local.AuthToken);
        //    request.AddHeader("Authorization", "Bearer " + token);
        //    var response = await _restClient.PostAsync(request);
        //    //var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.CompanySelectedEndPoint, companyResponse);
        //    string content = response.Content.ToString();
        //    //string content = await response.Content.ReadAsStringAsync();
        //    var result = JsonConvert.DeserializeObject<TokenResponse>(content);
        //    //var result = JsonConvert.DeserializeObject<TokenResponse>(content);

        //    // var result = await response.ToResult<TokenResponse>();
        //    if (result.IsSuccess)
        //    {
        //        token = result.Token;
        //        var refreshToken = result.RefreshToken;
        //        var userImageURL = "";
        //        //await _localStorage.SetItemAsync(StorageConstants.Local.AuthToken, token);
        //        _secureStorageService.SetItem(StorageConstants.Local.AuthToken, token);
        //        //await _localStorage.SetItemAsync(StorageConstants.Local.RefreshToken, refreshToken);
        //        if (refreshToken != null)
        //        {
        //            _secureStorageService.SetItem(StorageConstants.Local.RefreshToken, refreshToken);
        //        }
        //        //await _localStorage.SetItemAsync(StorageConstants.Local.CompanyName, companyResponse.CompanyName);
        //        _secureStorageService.SetItem(StorageConstants.Local.CompanyName, companyResponse.CompanyName);
        //        if (!string.IsNullOrEmpty(userImageURL))
        //        {
        //            //await _localStorage.SetItemAsync(StorageConstants.Local.UserImageURL, userImageURL);
        //            _secureStorageService.SetItem(StorageConstants.Local.UserImageURL, userImageURL);
        //        }

        //        await ((CustomAuthenticationProvider)this._authenticationStateProvider).StateChangedAsync();

        //        //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


        //    }
        //    else
        //    {

        //    }
        //}
    }
}
