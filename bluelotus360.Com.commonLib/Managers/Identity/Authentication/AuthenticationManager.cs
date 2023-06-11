using BL10.CleanArchitecture.Shared.Constants;
using bluelotus360.Com.commonLib.Authentication;
using bluelotus360.Com.commonLib.Extensions;
using bluelotus360.Com.commonLib.Routes;
using bluelotus360.Com.commonLib.Services.Definition;
using bluelotus360.Com.MauiSupports.Models;
using bluelotus360.Com.MauiSupports.Services.ConnectionStates;
using bluelotus360.Com.MauiSupports.Services.MAUISecureStorage;
using bluelotus360.Com.MauiSupports.Services.SqliteStorageServices;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.Com.Application.Requests.Identity;
using BlueLotus360.Com.Application.Responses.Identity;
using BlueLotus360.Com.Shared.Constants;
using BlueLotus360.Com.Shared.Constants.Storage;
using BlueLotus360.Com.Shared.Wrapper;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Managers.Identity.Authentication
{
    public class AuthenticationManager : IAuthenticationManager
	{
		private readonly HttpClient _httpClient;
		//private readonly ISecureStorageService  _localStorage;
		private readonly IStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
		private readonly ISqliteStorageService _sqliteStorageService;
		private readonly IConnectionState _connectionState;

		public AuthenticationManager(HttpClient httpClient,
            IStorageService localStorage, 
			IConnectionState connectionState,
			ISqliteStorageService sqliteStorageService,
			AuthenticationStateProvider authenticationStateProvider)
		{
			_httpClient = httpClient;
			_localStorage = localStorage;
			_authenticationStateProvider = authenticationStateProvider;
			_sqliteStorageService= sqliteStorageService;
			_connectionState = connectionState;
			if (_httpClient.DefaultRequestHeaders.Contains("IntegrationID"))
			{
				_httpClient.DefaultRequestHeaders.Remove("IntegrationID");
            }
            _httpClient.DefaultRequestHeaders.Add("IntegrationID", GlobalConsts.intergrationId);

        }

		public async Task<ClaimsPrincipal> CurrentUser()
		{
			var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
			return state.User;
		}

		public async Task<TokenResponse> Login(TokenRequest model)
		{
			var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.AuthenticateURL, model);
			var content = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<TokenResponse>(content);

			// var result = await response.ToResult<TokenResponse>();
			if (result != null && result.IsSuccess)
			{
				var token = result.Token??"";
				var refreshToken = result.RefreshToken??"";
				var userImageURL = result.UserImageURL ?? "";
				await _localStorage.SetItemAsync(StorageConstants.Local.AuthToken, token);
				//_localStorage.SetItem(StorageConstants.Local.RefreshToken, refreshToken);

				if (!string.IsNullOrEmpty(userImageURL))
				{
					await _localStorage.SetItemAsync(StorageConstants.Local.UserImageURL, userImageURL);
				}
				Type c = _authenticationStateProvider.GetType();

				await ((BL10AuthProvider)this._authenticationStateProvider).StateChangedAsync();

				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                
                return result;
			}
			else
			{
				return new TokenResponse();
			}
		}

		public async Task<IResult> Logout()
		{
            //_localStorage.RemoveItemsAsync(StorageConstants.Local.AuthToken);
            //_localStorage.RemoveItemsAsync(StorageConstants.Local.RefreshToken);
            //_localStorage.RemoveItemsAsync(StorageConstants.Local.UserImageURL);
            //_localStorage.RemoveItemsAsync(StorageConstants.Local.CompanyName);

            await _localStorage.RemoveItem(StorageConstants.Local.AuthToken);
            await _localStorage.RemoveItem(StorageConstants.Local.RefreshToken);
            await _localStorage.RemoveItem(StorageConstants.Local.UserImageURL);
            await _localStorage.RemoveItem(StorageConstants.Local.CompanyName);

            ((BL10AuthProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
			_httpClient.DefaultRequestHeaders.Authorization = null;
			return await Result.SuccessAsync();
		}

		public async Task<string> RefreshToken()
		{
			var token = await _localStorage.GetItemAsync<string>(StorageConstants.Local.AuthToken);
			var refreshToken = await _localStorage.GetItemAsync<string>(StorageConstants.Local.RefreshToken);

			var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Refresh, new RefreshTokenRequest { Token = token, RefreshToken = refreshToken });

			var result = await response.ToResult<TokenResponse>();

			if (!result.Succeeded)
			{
				throw new ApplicationException("Something went wrong during the refresh token action");
			}

			token = result.Data.Token;
			refreshToken = result.Data.RefreshToken;
			await _localStorage.SetItemAsync(StorageConstants.Local.AuthToken, token);
			await _localStorage.SetItemAsync(StorageConstants.Local.RefreshToken, refreshToken);
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			return token;
		}

		public async Task<string> TryRefreshToken()
		{
			//check if token exists
			var availableToken = await _localStorage.GetItemAsync<string>(StorageConstants.Local.RefreshToken);
			if (string.IsNullOrEmpty(availableToken)) return string.Empty;
			var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
			var user = authState.User;
			var exp = user.FindFirst(c => c.Type.Equals("exp"))?.Value;
			var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
			var timeUTC = DateTime.UtcNow;
			var diff = expTime - timeUTC;
			if (diff.TotalMinutes <= 1)
				return await RefreshToken();
			return string.Empty;
		}

		public async Task<string> TryForceRefreshToken()
		{
			return await RefreshToken();
		}
        public async Task<CompletedUserAuth> GetUserInfoOffline()
        {
            try
            {
				if (_connectionState.IsConnected())
				{
					return await GetUserInformation();
				}
				else
				{
                    string uid = await _localStorage.GetItem("UID");
                    string cid = await _localStorage.GetItem("CID");
					IncomingStrings contentin = await _sqliteStorageService.GetItemAsync(int.Parse(cid), int.Parse(uid), TokenEndpoints.UserInfoReadURL, null);
					if (contentin != null)
					{
                        string content = contentin.response;
                        var result = JsonConvert.DeserializeObject<CompletedUserAuth>(content);
                        return result;
                    }
					else
					{
						return new CompletedUserAuth();
					}
					
                }
                
            }
            catch (Exception ex)
            {
                return new CompletedUserAuth();
            }
        }
		public async Task<CompletedUserAuth> GetUserInformation()
		{
			try
			{
				var response = await _httpClient.GetAsync(TokenEndpoints.UserInfoReadURL);
				var content = await response.Content.ReadAsStringAsync();
				var result = JsonConvert.DeserializeObject<CompletedUserAuth>(content);
				_localStorage.SetItem("UID", result.AuthenticatedUser.UserKey.ToString());
                //_localStorage.SetItem("CID",result.AuthenticatedCompany.CompanyKey.ToString());

				//changed for companyselection
                if (result.AuthenticatedCompany != null && result.AuthenticatedCompany.CompanyKey != null)
                {
                    _localStorage.SetItem("CID", result.AuthenticatedCompany.CompanyKey.ToString());
                }
                
				IncomingStrings incString = new IncomingStrings();
				incString.user = (int)result.AuthenticatedUser.UserKey;
                //incString.company = (int)result.AuthenticatedCompany.CompanyKey;

                //changed for companyselection
                incString.company = (int)(result.AuthenticatedCompany?.CompanyKey ?? default);
                
				incString.name = TokenEndpoints.UserInfoReadURL;
				incString.timestamp = DateTime.Now;
				incString.response = content;
				await _sqliteStorageService.SaveItemAsync(incString);

                return result;
			}
			catch (Exception ex)
			{
				return new CompletedUserAuth();
			}


		}
	}
}
