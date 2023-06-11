using bluelotus360.Com.commonLib.Services.Definition;
using BlueLotus360.CleanArchitecture.Domain.DTO.Report;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Managers.Printer
{
    public class InvoicePrinterManager : IInvoicePrinterManager
    {

        private readonly HttpClient _httpClient;
        private readonly IStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public InvoicePrinterManager(HttpClient httpClient,
            IStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
        }
        public async Task PrintTransactionBillLocalAsync(TransactionReportLocal report, URLDefinitions definitions)
        {


            try
            {

                //var response = await _httpClient.PostAsJsonAsync(definitions.URL, report);



            }
            catch (Exception exp)
            {

            }
        }


    }
}
