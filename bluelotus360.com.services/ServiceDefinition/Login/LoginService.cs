using bluelotus360.com.services.ViewModels;
using RestSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;

namespace bluelotus360.com.services.ServiceDefinition.Login
{
    public class LoginService : ILoginService
    {
        private static RestClientOptions options = new RestClientOptions("")
        {
            ThrowOnAnyError = true
        };
        private RestClient client = new RestClient(options);
        public async Task<TokenResponse> Login(TokenRequest tok)
        {
            var request = new RestRequest().AddJsonBody(tok);
            var response = await client.PostAsync(request);
            var content = response.Content.ToString();
            var result = JsonConvert.DeserializeObject<TokenResponse>(content);

            if (result.IsSuccess)
            {
                var token = result.Token;
                var refreshToken = result.RefreshToken;
                var userImageURL = result.UserImageURL;
                //await _localStorage.SetItemAsync(StorageConstants.Local.AuthToken, token);
                //await _localStorage.SetItemAsync(StorageConstants.Local.RefreshToken, refreshToken);

                if (!string.IsNullOrEmpty(userImageURL))
                {
                    //await _localStorage.SetItemAsync(StorageConstants.Local.UserImageURL, userImageURL);
                }
                //Type c = _authenticationStateProvider.GetType();

                //await ((BL10AuthProvider)this._authenticationStateProvider).StateChangedAsync();

                //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return result;
            }
            else
            {
                return new TokenResponse();
            }
        }
    }
}
