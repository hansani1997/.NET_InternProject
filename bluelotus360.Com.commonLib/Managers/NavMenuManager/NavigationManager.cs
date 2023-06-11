using AutoMapper.Configuration;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using Microsoft.Extensions.Localization;
using bluelotus360.Com.MauiSupports.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using BlueLotus360.Com.Shared.Constants.Storage;
using BlueLotus360.Com.Shared.Constants;
using bluelotus360.Com.MauiSupports.Services.MAUISecureStorage;
using bluelotus360.Com.commonLib.Routes;
using BL10.CleanArchitecture.Shared.Constants;
using Microsoft.AspNetCore.Components.Authorization;
using bluelotus360.Com.MauiSupports.Services.ConnectionStates;
using bluelotus360.Com.MauiSupports.Services.SqliteStorageServices;
using System.Reflection.Metadata;
using static MudBlazor.CategoryTypes;
using BL10.CleanArchitecture.Domain.Entities.Report;
using bluelotus360.Com.commonLib.Services.Definition;

namespace bluelotus360.Com.commonLib.Managers.NavMenuManager
{
    public class NavMenuManager:INavMenuManager
    {
        //private readonly RestClient _restClient;
        //private readonly ISecureStorageService _secureStorage;
        private readonly IStorageService _secureStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private IStringLocalizer<NavMenuManager> _stringLocalizer;
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _factory;
        private ISqliteStorageService _sqliteStorage;
        private IConnectionState _connectionState;

        public NavMenuManager(
            HttpClient httpClient,
            AuthenticationStateProvider authenticationStateProvider,
            IHttpClientFactory factory,
            IStorageService secureStorage,
            IConnectionState connectionState,
            ISqliteStorageService sqliteStorageService
            )
        {
            //_restClient = restClient;
            _httpClient = httpClient;
            _sqliteStorage = sqliteStorageService;
            _connectionState = connectionState;
            _secureStorage = secureStorage;
            _authenticationStateProvider = authenticationStateProvider;
            _factory = factory;
            //_config = config;
            if (_httpClient.DefaultRequestHeaders.Count() == 0)
            {
                _httpClient.DefaultRequestHeaders.Add("IntegrationID", GlobalConsts.intergrationId);
            }
        }

        private void AssignClientData(HttpClient cl)
        {
            cl.BaseAddress = _httpClient.BaseAddress;
            cl.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
            _httpClient.DefaultRequestHeaders.Authorization.Scheme,
            _httpClient.DefaultRequestHeaders.Authorization.Parameter);
            cl.DefaultRequestHeaders.Add("IntegrationID", GlobalConsts.intergrationId);
        }

        public async Task<BLUIElement> GetMenuUIElement(ObjectFormRequest request)
        {
            try
            {
                if (_connectionState.IsConnected())
                {
                    BLUIElement list = null;
                    String cid = await _secureStorage.GetItem("CID");
                    String uid = await _secureStorage.GetItem("UID");
                    IncomingStrings incoming = new IncomingStrings();
                    var cl = _factory.CreateClient();
                    //cl.BaseAddress = ;
                    cl.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                        _httpClient.DefaultRequestHeaders.Authorization.Scheme,
                        _httpClient.DefaultRequestHeaders.Authorization.Parameter);
                    cl.DefaultRequestHeaders.Add("IntegrationID", GlobalConsts.intergrationId);
                    var response = await cl.PostAsJsonAsync(BaseEndpoint.BaseURL + ObjectEndpoints.FormDefinitionURL, request);
                    string content = await response.Content.ReadAsStringAsync();
                    incoming.user = Int32.Parse(uid);
                    incoming.company = Int32.Parse(cid);
                    incoming.response = content;
                    incoming.name = BaseEndpoint.BaseURL + ObjectEndpoints.FormDefinitionURL;
                    incoming.parameters = request.MenuKey.ToString();
                    await _sqliteStorage.SaveItemAsync(incoming);
                    list = JsonConvert.DeserializeObject<BLUIElement>(content);
                    cl.Dispose();
                    return list;
                }
                else
                {
                    String cid = await _secureStorage.GetItem("CID");
                    String uid = await _secureStorage.GetItem("UID");
                    IncomingStrings result = await _sqliteStorage.GetItemAsync(Int32.Parse(cid), Int32.Parse(uid), BaseEndpoint.BaseURL + ObjectEndpoints.FormDefinitionURL, request.MenuKey.ToString());
                    //string content = result.response;
                    string content = result?.response != null ? result.response:"";
                    var list = JsonConvert.DeserializeObject<BLUIElement>(content);
                    return list;
                }
                
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp);
                return null;
            }
        }


        public async Task<IDictionary<string, MenuItem>> GetNavAndPinnedMenus()
        {
            IDictionary<string, BlueLotus360.CleanArchitecture.Domain.Entities.MenuItem> menus = new Dictionary<string, BlueLotus360.CleanArchitecture.Domain.Entities.MenuItem>();

            menus["nav-menu"] = await this.GetNavigationMenu();

            //menus["pin-menu"] = await this.GetPinnedMenus();

           // menus["rpt-pin-menu"]=await this.GetReportPinnedMenus();
            return menus;
        }

        //OFFLINE CAPABLE
        private async Task<MenuItem> GetNavigationMenu()
        {
            CompanyResponse resp = new CompanyResponse();

            try
            {
                if (_connectionState.IsConnected())
                {
                    string content;
                    string cid = await _secureStorage.GetItem("CID");
                    string uid = await _secureStorage.GetItem("UID");
                    var cl = _factory.CreateClient();
                    AssignClientData(cl);
                    var response = await cl.GetAsync(ObjectEndpoints.SideMenuURL);
                    content = await response.Content.ReadAsStringAsync();
                    IncomingStrings str = new IncomingStrings();
                    str.name = ObjectEndpoints.SideMenuURL;
                    str.user = Convert.ToInt32(uid);
                    str.company = Convert.ToInt32(cid);
                    str.response = content;
                    str.parameters = null;
                    await _sqliteStorage.SaveItemAsync(str);
                    var list = JsonConvert.DeserializeObject<BlueLotus360.CleanArchitecture.Domain.Entities.MenuItem>(content);
                    return list;
                }
                else
                {
                    String cid = await _secureStorage.GetItem("CID");
                    String uid = await _secureStorage.GetItem("UID");
                    IncomingStrings result = await _sqliteStorage.GetItemAsync(Int32.Parse(cid), Int32.Parse(uid), ObjectEndpoints.SideMenuURL, null);
                    string content = result.response;
                    var list = JsonConvert.DeserializeObject<BlueLotus360.CleanArchitecture.Domain.Entities.MenuItem>(content);
                    return list;
                }
                
            }
            //catch(HttpRequestException httpexp)
            //{
            //    String cid = await _secureStorage.GetItem("CID");
            //    String uid = await _secureStorage.GetItem("UID");
            //    IncomingStrings result = await _sqliteStorage.GetItemAsync(Int32.Parse(cid), Int32.Parse(uid), ObjectEndpoints.SideMenuURL, null);
            //    string content = result.response;
            //    var list = JsonConvert.DeserializeObject<BlueLotus360.CleanArchitecture.Domain.Entities.MenuItem>(content);
            //    return list;
            //}
            catch (Exception exp)
            {
                Console.WriteLine(exp);
                return null;//exp.InnerException.SocketErrorCode = HostNotFound
            }
        }
        //OFFLINE CAPABLE
        public async Task<MenuItem> GetPinnedMenus()
        {
            CompanyResponse resp = new CompanyResponse();
            try
            {
                if (_connectionState.IsConnected())
                {
                    String cid = await _secureStorage.GetItem("CID");
                    String uid = await _secureStorage.GetItem("UID");
                    String content;
                    var response = await _httpClient.GetAsync(ObjectEndpoints.GetPinnedMenusEndpoint);
                    content = await response.Content.ReadAsStringAsync();
                    IncomingStrings str = new IncomingStrings();
                    str.name = ObjectEndpoints.GetPinnedMenusEndpoint;
                    str.user = Int32.Parse(uid);
                    str.company = Int32.Parse(cid);
                    str.response = content;
                    str.parameters = null;

                    await _sqliteStorage.SaveItemAsync(str);
                    var list = JsonConvert.DeserializeObject<BlueLotus360.CleanArchitecture.Domain.Entities.MenuItem>(content);

                    return list;
                }
                else
                {
                    String cid = await _secureStorage.GetItem("CID");
                    String uid = await _secureStorage.GetItem("UID");
                    IncomingStrings result = await _sqliteStorage.GetItemAsync(Int32.Parse(cid), Int32.Parse(uid), ObjectEndpoints.GetPinnedMenusEndpoint, null);
                    string content = result.response;
                    var list = JsonConvert.DeserializeObject<BlueLotus360.CleanArchitecture.Domain.Entities.MenuItem>(content);

                    return list;
                }
                
            }
            catch (Exception exp)
            {
                _httpClient.CancelPendingRequests();
                Console.WriteLine(exp);
                return null;
            }
        }
        public async Task UpdatePinnedMenus(BlueLotus360.CleanArchitecture.Domain.Entities.MenuItem menurequest)
        {
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(ObjectEndpoints.UpdatePinnedMenusEndpoint, menurequest);

            }
            catch (Exception exp)
            {
                _httpClient.CancelPendingRequests();


            }
        }

        public async Task<UserConfigObjectsBlLite> LoadObjectsForUserConfiguration(ObjectFormRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(ObjectEndpoints.LoadAllObjectsForUserConfigEndPoint, request);
                string content = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<UserConfigObjectsBlLite>(content);
                _httpClient.CancelPendingRequests();

                return list;
            }
            catch (Exception exp)
            {
                _httpClient.CancelPendingRequests();

                return null;
            }
        }

        public async Task UpdateObjectsForUserConfiguration(UserConfigObjectsBlLite request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(ObjectEndpoints.UpdateAllObjectsForUserConfigEndPoint, request);
            }
            catch (Exception exp)
            {
                _httpClient.CancelPendingRequests();


            }
        }

        public async Task<IList<MenuItem>> SearchBlLiteMenu(MenuSearchRequest request)
        {
            IList<MenuItem> menuList = new List<MenuItem>();
            try
            {
                var cl = _factory.CreateClient();
                cl.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                    _httpClient.DefaultRequestHeaders.Authorization.Scheme,
                    _httpClient.DefaultRequestHeaders.Authorization.Parameter);
                cl.DefaultRequestHeaders.Add("IntegrationID", GlobalConsts.intergrationId);

                var response = await cl.PostAsJsonAsync(BaseEndpoint.BaseURL + ObjectEndpoints.SearchBlLiteMenusEndpoint, request);

                string content = await response.Content.ReadAsStringAsync();
                menuList = JsonConvert.DeserializeObject<IList<MenuItem>>(content);
                cl.Dispose();
                
            }
            catch (Exception exp)
            {
                _httpClient.CancelPendingRequests();
            }
            return menuList;
        }

        public async Task<MenuItem> GetReportPinnedMenus()
        {
            //IList<MenuItem> list=new List<MenuItem>();  
            MenuItem ReportPinnedMenus;
            try
            {

                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.GetAsync(ReportEndPoints.ReportPinnedMenuURL);
                //await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
                //list = JsonConvert.DeserializeObject<IList<MenuItem>>(content);
                ReportPinnedMenus = JsonConvert.DeserializeObject<MenuItem>(content);




                return ReportPinnedMenus;

            }

            catch (Exception exp)
            {
                _httpClient.CancelPendingRequests();
                Console.WriteLine(exp);
                return null;
            }
        }

        public async Task<IList<ReportModuleItem>> GetReportModuleMenus()
        {
            IList<ReportModuleItem> list=new List<ReportModuleItem>();  
            
            try
            {

                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.GetAsync(ReportEndPoints.ReportModuleMenuURL);
                //await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
                list = JsonConvert.DeserializeObject<IList<ReportModuleItem>>(content);




                return list;

            }

            catch (Exception exp)
            {
                _httpClient.CancelPendingRequests();
                Console.WriteLine(exp);
                return null;
            }
        }

        public async Task<IList<ReportSubModule>> GetReportSubModuleMenus(SubModuleRequest ParenttKey)
        {
            IList<ReportSubModule> list = new List<ReportSubModule>();

            try
            {

                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(ReportEndPoints.ReportModuleSubMenuURL, ParenttKey);
                //await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
                list = JsonConvert.DeserializeObject<IList<ReportSubModule>>(content);




                return list;

            }

            catch (Exception exp)
            {
                _httpClient.CancelPendingRequests();
                Console.WriteLine(exp);
                return null;
            }
        }


        public async Task<IList<ReportFilterFields>> GetReportFilterFields(SubModuleRequest ParenttKey)
        {
            IList<ReportFilterFields> list = new List<ReportFilterFields>();

            try
            {

                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(ReportEndPoints.ReportFilterFieldsURL, ParenttKey);
                //await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
                list = JsonConvert.DeserializeObject<IList<ReportFilterFields>>(content);



                return list;

            }

            catch (Exception exp)
            {
                _httpClient.CancelPendingRequests();
                Console.WriteLine(exp);
                return null;
            }
        }
    }
}
