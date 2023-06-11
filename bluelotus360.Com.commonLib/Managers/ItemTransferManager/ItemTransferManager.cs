using BL10.CleanArchitecture.Shared.Constants;
using bluelotus360.Com.commonLib.Routes;
using bluelotus360.Com.commonLib.Services.Definition;
using bluelotus360.Com.MauiSupports.Services.MAUISecureStorage;
using BlueLotus360.CleanArchitecture.Domain.Entities.InventoryManagement.ItemTransfer;
using BlueLotus360.Com.Shared.Constants;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Managers.ItemTransferManager
{
    public class ItemTransferManager : IItemTransferManager
    {
        private readonly HttpClient _httpClient;
        //private readonly ISecureStorageService _localStorage;
        private readonly IStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IHttpClientFactory _factory;
        //private readonly IStringLocalizer<ItemTransferManager> _localizer;
        private bool _checkIfExceptionReturn;
        //private readonly IConfiguration _config;

        public ItemTransferManager(HttpClient httpClient,
            IStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider, IHttpClientFactory factory)
            //IStringLocalizer<ItemTransferManager> localizer, IConfiguration config)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
            //_localizer = localizer;
            //_config = config;

            if (_httpClient.DefaultRequestHeaders.Count() == 0)
                _httpClient.DefaultRequestHeaders.Add("IntegrationID", GlobalConsts.intergrationId);
            _factory = factory;
        }

        private void AssignClientData(HttpClient cl)
        {
            cl.BaseAddress = _httpClient.BaseAddress;
            cl.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                _httpClient.DefaultRequestHeaders.Authorization.Scheme,
                _httpClient.DefaultRequestHeaders.Authorization.Parameter);
            cl.DefaultRequestHeaders.Add("IntegrationID", GlobalConsts.intergrationId);
        }
        public async Task<int> CreateItemTransfer(ItemTransfer itm)
        {
            _checkIfExceptionReturn = false;
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.CreateItemTransfer_EndPoint, itm);
                //await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<int>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                return 1;
            }

        }

        public async Task<ItemTransferLineItem> GetItemsData(ItemTransferLineItem res)
        {
            _checkIfExceptionReturn = false;

            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.Get_Data_EndPoint, res);
                //await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                res = JsonConvert.DeserializeObject<ItemTransferLineItem>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;

            }

            return res;
        }

        public async Task<List<ItemTransferLineItem>> GetInvoiceData(LNDInvoice res)
        {
            _checkIfExceptionReturn = false;
            List<ItemTransferLineItem> itemList = new List<ItemTransferLineItem>();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.Get_Invoice_Data_EndPoint, res);
                //await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                itemList = JsonConvert.DeserializeObject<List<ItemTransferLineItem>>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;

            }

            return itemList;
        }

        public async Task<List<string>> GetInvoiceSerialNoList(LNDInvoice res)
        {
            _checkIfExceptionReturn = false;
            List<string> serialNoList = new List<string>();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.GetInvoiceItemsSerialNoList, res);
                //await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                serialNoList = JsonConvert.DeserializeObject<List<string>>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;

            }

            return serialNoList;
        }

        public async Task<ItmtrnsferValidationResponse> TransferValidator(ItemTransfer itm)
        {
            ItmtrnsferValidationResponse res = new ItmtrnsferValidationResponse();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.ItemTransferPreSavingValidationEndpoint, itm);//TokenEndpoints.ItemTransferValidationEndpoint
                //await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                res = JsonConvert.DeserializeObject<ItmtrnsferValidationResponse>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;

            }
            return res;
        }

        public async Task<ItmtrnsferValidationResponse> InvoiceTransferValidator(LNDInvoice invoice)
        {
            ItmtrnsferValidationResponse res = new ItmtrnsferValidationResponse();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.InvoiceItemTransferValidationEndpoint, invoice);
                //await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                res = JsonConvert.DeserializeObject<ItmtrnsferValidationResponse>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;

            }
            return res;
        }

        public async Task<List<FindItemTransferResponse>> Find(FindItemTransferRequest req)
        {
            _checkIfExceptionReturn = false;
            List<FindItemTransferResponse> itmtrn = new List<FindItemTransferResponse>();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.Find_ItmTrn_EndPoint, req);
                //await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                itmtrn = JsonConvert.DeserializeObject<List<FindItemTransferResponse>>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;

            }
            return itmtrn;

        }

        public async Task<ItemTransfer> RefreshForm(TransferOpenRequest req)
        {
            _checkIfExceptionReturn = false;
            ItemTransfer refData = new ItemTransfer();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.Refresh_Header_EndPoint, req);
                //await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                refData = JsonConvert.DeserializeObject<ItemTransfer>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;

            }
            return refData;
        }

        public async Task<int> UpdateItemTransfer(ItemTransfer req)
        {
            _checkIfExceptionReturn = false;

            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.Update_ItmTransfer_EndPoint, req);
                //await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<int>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                return 1;
            }

        }

        public async Task<ItmtrnsferValidationResponse> TransferMultiAprLock(ItemTransfer req)
        {
            _checkIfExceptionReturn = false;

            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.TransferMultiAprLock_EndPoint, req);
                //await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ItmtrnsferValidationResponse>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                return new ItmtrnsferValidationResponse();
            }
        }

        public async Task<IList<ItemTransferLineItem>> GetItemtransferLineItemForApproval(ItemTransfer req)
        {
            _checkIfExceptionReturn = false;
            IList<ItemTransferLineItem> itemList = new List<ItemTransferLineItem>();
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.ItmTrnSerAprGridSelectWebEndPoint, req);
                //await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                itemList = JsonConvert.DeserializeObject<IList<ItemTransferLineItem>>(content);
                return itemList;

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                return new List<ItemTransferLineItem>();
            }
        }

        public async Task ItmTrnSerAprInsertWeb(IList<ItemTransferLineItem> reqList)
        {
            _checkIfExceptionReturn = false;

            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.ItmTrnSerAprGridInsertWebEndPoint, reqList);
                //await response.Content.LoadIntoBufferAsync();

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
            }
        }
        public bool IsExceptionthrown()
        {
            if (_checkIfExceptionReturn)
                return true;
            return false;
        }

    }
}
