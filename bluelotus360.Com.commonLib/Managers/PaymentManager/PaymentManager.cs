using BL10.CleanArchitecture.Domain.DTO.RequestDTO;
using BL10.CleanArchitecture.Domain.Entities.Financial;
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
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Managers.PaymentManager
{
    public class PaymentManager : IPaymentManager
    {
        private readonly HttpClient _httpClient;
        //private readonly ISecureStorageService _localStorage;
        private readonly IStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        //private readonly IStringLocalizer<PaymentManager> _localizer;
        private readonly IConfiguration _config;

        public PaymentManager(
                    HttpClient httpClient,
                    IStorageService localStorage,
                    AuthenticationStateProvider authenticationStateProvider,
                    //IStringLocalizer<PaymentManager> localizer,
                    IConfiguration config)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
            //_localizer = localizer;
            _config = config;

            if (_httpClient.DefaultRequestHeaders.Count() == 0)
                _httpClient.DefaultRequestHeaders.Add("IntegrationID", GlobalConsts.intergrationId );
        }

        public async Task<IList<JournalLiteFindResponseDTO>> FindJournalDetails(JournalFindDTO dto)
        {
            IList<JournalLiteFindResponseDTO> journals = new List<JournalLiteFindResponseDTO>();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetJournalDetails_EndPoint, dto);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<IList<JournalLiteFindResponseDTO>>(content);
                journals = obj;


            }
            catch (Exception exp)
            {

            }
            return journals;
        }

        public async Task<BLJournalLite> SelectAccTrnSingleEntryDetail(JournalLiteFindResponseDTO model)
        {
            BLJournalLite journal = new BLJournalLite();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.SelectAccTrnSingleEntries_EndPoint, model);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<BLJournalLite>(content);
                journal = obj;


            }
            catch (Exception exp)
            {

            }
            return journal;
        }

        public async Task<BLJournalLite> InsertSingleEntryDetail(BLJournalLite dto)
        {
            BLJournalLite journal = new BLJournalLite();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.InsertSingleAccTrn_EndPoint, dto);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                journal = JsonConvert.DeserializeObject<BLJournalLite>(content);

            }
            catch (Exception exp)
            {

            }
            return journal;
        }

        public async Task DeleteSingleEntry(AccTrnSingleEntry dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.DeleteSingleEntry_EndPoint, dto);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;



            }
            catch (Exception exp)
            {

            }
        }
    }
}
