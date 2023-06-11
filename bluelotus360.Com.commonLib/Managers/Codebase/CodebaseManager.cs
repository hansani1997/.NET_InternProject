
using BL10.CleanArchitecture.Shared.Constants;
using bluelotus360.Com.commonLib.Managers.Address;
using bluelotus360.Com.commonLib.Routes;
using bluelotus360.Com.commonLib.Services.Definition;
using bluelotus360.Com.MauiSupports.Services.MAUISecureStorage;
using BlueLotus360.CleanArchitecture.Application.Responses.ServerResponse;
using BlueLotus360.CleanArchitecture.Domain.DTO.MasterData;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
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

namespace BlueLotus360.Com.Infrastructure.Managers.Codebase
{
    public class CodebaseManager: ICodebaseManager
    {
        private readonly HttpClient _httpClient;
        //private readonly ISecureStorageService _localStorage;
        private readonly IStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public CodebaseManager(HttpClient httpClient,
            IStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;

            if (_httpClient.DefaultRequestHeaders.Count() == 0)
                _httpClient.DefaultRequestHeaders.Add("IntegrationID", GlobalConsts.intergrationId);
        }

        public async Task<IList<CodeBaseResponse>> GetCodesByConditionCode(CodeBaseResponse record)
        {
            IList<CodeBaseResponse> codes=new List<CodeBaseResponse>();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetCodesByConditionCode, record);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                codes = JsonConvert.DeserializeObject<IList<CodeBaseResponse>>(content);



            }
            catch (Exception exp)
            {

            }
            return codes;
        }

        public async Task<CodeBaseResponse> GetCodesByConditionCodeAndOurCode(CodeBaseResponse record)
        {
            CodeBaseResponse codes = new CodeBaseResponse();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetCodesByConditionCodeAndOurCode, record);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                codes = JsonConvert.DeserializeObject<CodeBaseResponse>(content);



            }
            catch (Exception exp)
            {

            }
            return codes;
        }
    }
}
