using BlueLotus360.Com.Shared.Constants.Storage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using BlueLotus360.Com.Shared.Constants.Permission;
using bluelotus360.Com.MauiSupports.Services.MAUISecureStorage;
using System.IdentityModel.Tokens.Jwt;
using bluelotus360.Com.commonLib.Services.Definition;

namespace bluelotus360.Com.commonLib.Authentication
{
    public class BL10AuthProvider : AuthenticationStateProvider
	{
        //private ISecureStorageService _localStorage;
        private readonly IStorageService _localStorage;
        private readonly HttpClient _httpClient;
		public ClaimsPrincipal AuthenticationStateUser { get; set; }
		public BL10AuthProvider(HttpClient httpClient, IStorageService localStorage)
		{
			_httpClient = httpClient;
			_localStorage = localStorage;
		}

		public async Task StateChangedAsync()
		{
			var authState = Task.FromResult(await GetAuthenticationStateAsync());
			NotifyAuthenticationStateChanged(authState);

		}

		public void MarkUserAsLoggedOut()
		{
			var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
			var authState = Task.FromResult(new AuthenticationState(anonymousUser));
			NotifyAuthenticationStateChanged(authState);
		}

		public async Task<ClaimsPrincipal> GetAuthenticationStateProviderUserAsync()
		{
			var state = await this.GetAuthenticationStateAsync();
			var authenticationStateProviderUser = state.User;
			return authenticationStateProviderUser;
		}
		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			var savedToken = await _localStorage.GetItemAsync<string>(StorageConstants.Local.AuthToken);
			if (string.IsNullOrWhiteSpace(savedToken))
			{
				ClaimsIdentity tidentity = new ClaimsIdentity();
				Claim claim = new Claim("CompanySelected", "NO");
				tidentity.AddClaim(claim);
				AuthenticationState tempState = new AuthenticationState(new ClaimsPrincipal(tidentity));

				return tempState;
			}
            else
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(savedToken);
                var expClaim = jwtToken.ValidTo;

				//if(expClaim < DateTime.UtcNow)
				//{
				//	ClaimsIdentity tidentity = new ClaimsIdentity(GetClaimsFromJwt(savedToken), "jwt");
				//                var companySelectedClaim = tidentity.FindFirst("CompanySelected");
				//                if (companySelectedClaim != null)
				//                {
				//                    tidentity.RemoveClaim(companySelectedClaim);
				//                }

				//                tidentity.AddClaim(new Claim("CompanySelected", "NO"));
				//                ClaimsPrincipal newPrincipal = new ClaimsPrincipal(tidentity);
				//                AuthenticationState newState = new AuthenticationState(newPrincipal);

				//                return await Task.FromResult(newState);
				//            }

				//if (expClaim < DateTime.UtcNow)
				//{
				//    ClaimsIdentity tidentity = new ClaimsIdentity(GetClaimsFromJwt(savedToken), "jwt");
				//    tidentity.RemoveClaim(tidentity.FindFirst("CompanySelected"));
				//    tidentity.AddClaim(new Claim("CompanySelected", "NO"));
				//    AuthenticationState tempState = new AuthenticationState(new ClaimsPrincipal(tidentity));
				//    return tempState;
				//}
				if (expClaim < DateTime.UtcNow)
				{
					ClaimsIdentity tidentity = new ClaimsIdentity();
					//tidentity.RemoveClaim(tidentity.FindFirst("CompanySelected"));
					var res  = _localStorage.RemoveItem(StorageConstants.Local.CompanyName); // Remove company name from local memory instead of removing from token
                    Claim claim = new Claim("CompanySelected", "NO");
					tidentity.AddClaim(claim);
					AuthenticationState tempState = new AuthenticationState(new ClaimsPrincipal(tidentity));

					return tempState;
				}
			}
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

			var state = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(GetClaimsFromJwt(savedToken), "jwt")));

			AuthenticationStateUser = state.User;

			return state;
		}

		private IEnumerable<Claim> GetClaimsFromJwt(string jwt)
		{
			var claims = new List<Claim>();
			var payload = jwt.Split('.')[1];
			var jsonBytes = ParseBase64WithoutPadding(payload);
			var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

			if (keyValuePairs != null)
			{
				keyValuePairs.TryGetValue(ClaimTypes.Role, out var roles);

				if (roles != null)
				{
					if (roles.ToString().Trim().StartsWith("["))
					{
						var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

						claims.AddRange(parsedRoles.Select(role => new Claim(ClaimTypes.Role, role)));
					}
					else
					{
						claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
					}

					keyValuePairs.Remove(ClaimTypes.Role);
				}

				keyValuePairs.TryGetValue(ApplicationClaimTypes.Permission, out var permissions);
				if (permissions != null)
				{
					if (permissions.ToString().Trim().StartsWith("["))
					{
						var parsedPermissions = JsonSerializer.Deserialize<string[]>(permissions.ToString());
						claims.AddRange(parsedPermissions.Select(permission => new Claim(ApplicationClaimTypes.Permission, permission)));
					}
					else
					{
						claims.Add(new Claim(ApplicationClaimTypes.Permission, permissions.ToString()));
					}
					keyValuePairs.Remove(ApplicationClaimTypes.Permission);
				}

				claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
			}
			return claims;
		}

		private byte[] ParseBase64WithoutPadding(string payload)
		{
			payload = payload.Trim().Replace('-', '+').Replace('_', '/');
			var base64 = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
			return Convert.FromBase64String(base64);
		}
	}
}
