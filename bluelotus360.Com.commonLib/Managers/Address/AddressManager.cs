using BL10.CleanArchitecture.Shared.Constants;
using bluelotus360.Com.commonLib.Routes;
using bluelotus360.Com.commonLib.Services.Definition;
using bluelotus360.Com.MauiSupports.Models;
using bluelotus360.Com.MauiSupports.Services.ConnectionStates;
using bluelotus360.Com.MauiSupports.Services.MAUISecureStorage;
using bluelotus360.Com.MauiSupports.Services.SqliteStorageServices;
using BlueLotus360.CleanArchitecture.Application.Responses.ServerResponse;
using BlueLotus360.CleanArchitecture.Domain.DTO.MasterData;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace bluelotus360.Com.commonLib.Managers.Address
{
    public class AddressManager : IAddressManager
    {
        private readonly HttpClient _httpClient;
        //private readonly ISecureStorageService _localStorage;
        private readonly IStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        //private readonly IConnectionState _connectionState;
        private readonly IAddressComboStore _addressComboStore;

        public AddressManager(
            HttpClient httpClient,
            IStorageService localStorage,
            //IConnectionState connectionState,
            IAddressComboStore addressComboStore,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
            _addressComboStore = addressComboStore;
            //_connectionState = connectionState;

            if (_httpClient.DefaultRequestHeaders.Count() == 0)
                _httpClient.DefaultRequestHeaders.Add("IntegrationID", GlobalConsts.intergrationId);
        }

        public async Task<AddressCreateServerResponse> CreateNewAddress(AddressMaster record)
        {
            AddressCreateServerResponse addressIdCheckServerResponse = new();
            try
            {
                //if (_connectionState.IsConnected())
                //{
                    var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.CreateNewAddressURL, record);
                    await response.Content.LoadIntoBufferAsync();
                    string content = response.Content.ReadAsStringAsync().Result;
                    addressIdCheckServerResponse = JsonConvert.DeserializeObject<AddressCreateServerResponse>(content);
                //}
                //else
                //{
                //    AddressComboModel addressComboModel = new AddressComboModel();
                //    AddressResponse response = new AddressResponse();
                //    response.AddressID = record.AddressID;
                //    response.AddressName = record.AddressName;
                //    addressComboModel.AddressObject = JsonConvert.SerializeObject(response);
                //    addressComboModel.addressMasterObject = JsonConvert.SerializeObject(record);
                //    String cid = await _localStorage.GetItem("CID");
                //    String uid = await _localStorage.GetItem("UID");
                //    addressComboModel.User = Int32.Parse(uid); 
                //    addressComboModel.Company = Int32.Parse(cid);
                //    addressComboModel.RequestingElement = 0;
                //    addressComboModel.AddressKey = record.AddressKey;
                //    addressComboModel.isPushed = false;
                //    addressComboModel.isNew = true;
                //    addressComboModel.timestamp = DateTime.Today;
                //    await _addressComboStore.SaveItemAsync(addressComboModel);
                //}
            }
            catch (Exception exp)
            {

            }
            return addressIdCheckServerResponse;
        }

        public async Task<AddressMaster> CreateCustomer(AddressMaster customer)
        {
            AddressMaster responses = new AddressMaster();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.CreateCustomer, customer);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                responses = JsonConvert.DeserializeObject<AddressMaster>(content);
            }
            catch (Exception exp)
            {

            }
            return responses;
        }

        public async Task<AddressMaster> CreateCustomerValidation(AddressMaster customer)
        {
            AddressMaster responses = new AddressMaster();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.CreateCustomerValidation, customer);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                responses = JsonConvert.DeserializeObject<AddressMaster>(content);
            }
            catch (Exception exp)
            {

            }
            return responses;
        }

        public async Task<AddressMaster> CheckAdvanceAnalysisAvailability(AddressMaster customer)
        {
            AddressMaster responses = new AddressMaster();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.CheckAdvanceAnalysisAvailability, customer);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                responses = JsonConvert.DeserializeObject<AddressMaster>(content);
            }
            catch (Exception exp)
            {

            }
            return responses;
        }

        public async Task<AddressMaster> CreateAdvanceAnalysis(AddressMaster customer)
        {
            AddressMaster responses = new AddressMaster();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.CreateAdvanceAnalysis, customer);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                responses = JsonConvert.DeserializeObject<AddressMaster>(content);
            }
            catch (Exception exp)
            {

            }
            return responses;
        }

        public async Task<AddressResponse> GetAddressByUserKy()
        {
            AddressResponse responses = new AddressResponse();

            try
            {
                var response = await _httpClient.GetAsync(TokenEndpoints.GetAddressByUsrKyEndPoint);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                responses = JsonConvert.DeserializeObject<AddressResponse>(content);
            }
            catch (Exception exp)
            {

            }
            return responses;
        }

    }
}
