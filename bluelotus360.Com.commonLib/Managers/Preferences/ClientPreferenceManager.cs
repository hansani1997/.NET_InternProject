using BL10.CleanArchitecture.Domain.Entities.Theme;
using BL10.CleanArchitecture.Shared.Constants;
using bluelotus360.Com.commonLib.Routes;
using bluelotus360.Com.commonLib.Services.Definition;
using bluelotus360.Com.commonLib.Setting;
using bluelotus360.Com.MauiSupports.Services.MAUISecureStorage;
using BlueLotus360.Com.Shared.Settings;
using BlueLotus360.Com.Shared.Wrapper;
using MudBlazor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Managers.Preferences
{
	public class ClientPreferenceManager : IClientPreferenceManager
	{
        private readonly IHttpClientFactory _factory;
        private readonly HttpClient _httpClient;
		//private readonly ISecureStorageService _localStorage;
        private readonly IStorageService _localStorage;
		public ClientPreferenceManager(HttpClient httpClient, IHttpClientFactory factory, IStorageService localStorage)
		{
			_httpClient = httpClient;
			_factory = factory;
            _localStorage = localStorage;
        }

        private void AssignClientData(HttpClient cl)
        {
            cl.BaseAddress = _httpClient.BaseAddress;
            cl.DefaultRequestHeaders.Add("IntegrationID", GlobalConsts.intergrationId);
        }
        public Task<IResult> ChangeLanguageAsync(string languageCode)
		{
			throw new NotImplementedException();
		}

		public async Task<ClientThemePreference> GetCurrentThemeAsync(ClientThemePreference currentTheme)
		{
            
            try
            {
                string uid = await _localStorage.GetItem("UID");

                int.TryParse(uid,out var usrky);
                currentTheme.UsrKy= usrky;

                if (usrky>1)
                {
                    bool isKeyExists = await _localStorage.ContainKeyAsync("Theme");
                    if (!isKeyExists)
                    {
                        var cl = _factory.CreateClient();
                        AssignClientData(cl);
                        var response = await cl.PostAsJsonAsync(BaseEndpoint.BaseURL + ObjectEndpoints.GetThemePreferenceEndpoint, currentTheme);
                        string content = await response.Content.ReadAsStringAsync();
                        currentTheme = JsonConvert.DeserializeObject<ClientThemePreference>(content);

                        if (currentTheme != null)
                        {
                            await SetThemeLocalAsync(currentTheme);

                        }

                    }
                    else
                    {
                        currentTheme = await GetThemeLocalAsync();
                    }
                }
                else
                {
                    currentTheme = null;
                }

                

			}
            catch (Exception exp)
            {
                currentTheme = null;
            }
            finally
            {

            }

            return currentTheme;
        }

        public async Task<ClientThemePreference> SetCurrentThemeAsync(ClientThemePreference currentTheme)
        {
            try
            {
                string uid = await _localStorage.GetItem("UID");

                int.TryParse(uid, out var usrky);
                currentTheme.UsrKy = usrky;

                if (usrky>1)
                {
                    //first sync  to db 
                    var cl = _factory.CreateClient();
                    AssignClientData(cl);
                    var response = await cl.PostAsJsonAsync(BaseEndpoint.BaseURL + ObjectEndpoints.SetThemePreferenceEndpoint, currentTheme);
                    string content = await response.Content.ReadAsStringAsync();
                    currentTheme = JsonConvert.DeserializeObject<ClientThemePreference>(content);

                    //after that locally store theme
                    bool isKeyExists = await _localStorage.ContainKeyAsync("Theme");
                    if (isKeyExists)
                    {
                       await  _localStorage.RemoveItem("Theme");
                    }
                    if (currentTheme != null)
                    {
                        await SetThemeLocalAsync(currentTheme);

                    }
                }
                else
                {
                    currentTheme = await GetThemeLocalAsync();
                }


            }
            catch (Exception exp)
            {
                currentTheme = null;
            }
            finally
            {

            }

            return currentTheme;
        }

        public async Task<ClientThemePreference> UpdateCurrentThemeAsync(ClientThemePreference currentTheme)
        {
            try
            {
                string uid = await _localStorage.GetItem("UID");

                int.TryParse(uid, out var usrky);
                currentTheme.UsrKy = usrky;

                if (usrky>1)
                {
                    //first sync it to db 
                    var cl = _factory.CreateClient();
                    AssignClientData(cl);
                    var response = await cl.PostAsJsonAsync(BaseEndpoint.BaseURL + ObjectEndpoints.UpdateThemePreferenceEndpoint, currentTheme);
                    string content = await response.Content.ReadAsStringAsync();
                    currentTheme = JsonConvert.DeserializeObject<ClientThemePreference>(content);

                    //after that locally store theme

                    bool isKeyExists = await _localStorage.ContainKeyAsync("Theme");
                    if (isKeyExists)
                    {
                        await _localStorage.RemoveItem("Theme");
                    }
                    if (currentTheme != null)
                    {
                        await SetThemeLocalAsync(currentTheme);

                    }
                }
                else
                {
                    currentTheme = await GetThemeLocalAsync();
                }
                
            }
            catch (Exception exp)
            {
                currentTheme = null;
            }
            finally
            {

            }

            return currentTheme;
        }

        public Task<IPreference> GetPreference()
		{
			throw new NotImplementedException();
		}

		public Task SetPreference(IPreference preference)
		{
			throw new NotImplementedException();
		}

		public Task<bool> ToggleDarkModeAsync()
		{
			throw new NotImplementedException();
		}

        public  async Task SetThemeLocalAsync(ClientThemePreference currentTheme)
        {
            string json = JsonConvert.SerializeObject(currentTheme);
            _localStorage.SetItem("Theme", json);
        }

        public async Task<ClientThemePreference> GetThemeLocalAsync()
        {
            string json = await _localStorage.GetItemAsync<string>("Theme");

            ClientThemePreference theme = JsonConvert.DeserializeObject<ClientThemePreference>(json);

            return theme;
        }
    }
}
