using BL10.CleanArchitecture.Shared.Constants;
using bluelotus360.Com.commonLib.Routes;
using bluelotus360.Com.commonLib.Services.Definition;
using BlueLotus360.CleanArchitecture.Application.Responses.ServerResponse;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.Com.Shared.Constants.Storage;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Managers.TransactionManager
{
    public class AutoTransactionManager
    {
        private readonly HttpClient _httpClient;
        private RestClient _restClient;
        private IStorageService _secureStorageService;
        private string token;
        public AutoTransactionManager(IStorageService storageService) {
            _httpClient = new HttpClient();
            _secureStorageService = storageService;//changed this there was _secureStorageService = new SecureStorageService();
            _restClient = new RestClient();
            loadData();
        }

        public async void loadData()
        {
            token = await _secureStorageService.GetItem(StorageConstants.Local.AuthToken);
            if(token != null)
            {
                if (_httpClient.DefaultRequestHeaders.Count() == 0)
                {
                    _httpClient.DefaultRequestHeaders.Add("IntegrationID", GlobalConsts.intergrationId);
                    string[] tt = token.Split('\"',StringSplitOptions.RemoveEmptyEntries);
                    _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tt[0]);
                }
            }
        }

        public async Task<bool> SaveOrder(Order order)
        {
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.OrderSaveEndpoint, order);
                var ser = await response.Content.ReadFromJsonAsync<OrderSaveResponse>();
                order.OrderNumber = ser.OrderNumber;
                order.Prefix = ser.Prefix;
                order.OrderKey = ser.OrderKey;
                return true;

            }
            catch (Exception exp)
            {
                order.OrderNumber = "ERR";
                return false;
            }
        }
        public async Task<ExtendedTransaction> SaveTransaction(BLTransaction transation)
        {
            ExtendedTransaction responses = new ExtendedTransaction() ;
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.TransactionSaveEndpoint, transation);
                await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                responses.transaction = JsonConvert.DeserializeObject<BLTransaction>(content);
                responses.isSavedOnline = true;
                return responses;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
                responses = new ExtendedTransaction();
            }

            return responses;
        }
    }
}
