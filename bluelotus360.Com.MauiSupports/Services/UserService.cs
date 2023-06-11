using bluelotus360.com.razorComponents.Data;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using bluelotus360.Com.MauiSupports;
using BlueLotus360.Com.Application.Responses.Identity;

namespace bluelotus360.com.razorComponents.Services
{
	public class UserService : IUserService
	{
		public RestClient _client { get; }
		public AppSettings _appSettings { get; }

		public UserService(RestClient httpClient, IOptions<AppSettings> appSettings)
		{
			_appSettings = appSettings.Value;

			//httpClient.BaseAddress = new Uri(_appSettings.BaseAddess);
			//httpClient.AddDefaultRequestHeaders.Add("User-Agent", "BlazorServer");
			httpClient.AddDefaultHeader("User-Agent", "BlazorServer");
			_client = httpClient;
		}

		public async Task<TokenResponse> LoginAsync(User user)
		{
			//  user.Password = Utility.Encrypt(user.Password);
			string serializedUser = JsonConvert.SerializeObject(user);
			var requestMessage = new RestRequest(_appSettings.BaseAddess + "Authentication/Authenticate");
			requestMessage.Method = Method.Post;
			requestMessage.AddJsonBody(serializedUser);
			requestMessage.AddHeader("Content-Type", "application/json");
			//var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Authentication/Authenticate");
			//requestMessage.Content = new StringContent(serializedUser);

			//requestMessage.Content.Headers.ContentType
			//= new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

			var response = await _client.PostAsync(requestMessage);
			//var response = await _httpClient.SendAsync(requestMessage);

			var responseStatusCode = response.StatusCode;
			var responseBody = response.Content.ToString();
			var returnedUser = JsonConvert.DeserializeObject<TokenResponse>(responseBody);
			return await Task.FromResult(returnedUser);

		}

		public async Task<User> RegisterUserAsync(User user)
		{
			user.Password = Utility.Encrypt(user.Password);
			string serializedUser = JsonConvert.SerializeObject(user);

			//var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Users/RegisterUser");
			//requestMessage.Content = new StringContent(serializedUser);
			var requestMessage = new RestRequest(_appSettings + "Users/RegisterUser");
			requestMessage.Method = Method.Post;
			requestMessage.AddJsonBody(serializedUser);

			//requestMessage.Content.Headers.ContentType
			//	= new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
			requestMessage.AddHeader("Content-Type", "application/json");

			//var response = await _httpClient.SendAsync(requestMessage);
			var response = await _client.PostAsync(requestMessage);

			var responseStatusCode = response.StatusCode;
			var responseBody = response.Content.ToString();

			var returnedUser = JsonConvert.DeserializeObject<User>(responseBody);

			return await Task.FromResult(returnedUser);
		}

		public async Task<User> RefreshTokenAsync(RefreshRequest refreshRequest)
		{
			string serializedUser = JsonConvert.SerializeObject(refreshRequest);

			//var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Users/RefreshToken");
			//requestMessage.Content = new StringContent(serializedUser);
			var requestMessage = new RestRequest(_appSettings.BaseAddess + "Users/RefreshToken");
			requestMessage.Method = Method.Post;
			requestMessage.AddJsonBody(serializedUser);
			requestMessage.AddHeader("Content-Type", "application/json");
			//requestMessage.Content.Headers.ContentType
			//	= new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
			var response = await _client.PostAsync(requestMessage);
			//var response = await _httpClient.SendAsync(requestMessage);

			var responseStatusCode = response.StatusCode;
			////var responseBody = await response.Content.ReadAsStringAsync();
			var responseBody = response.Content.ToString().ToLower();
			var returnedUser = JsonConvert.DeserializeObject<User>(responseBody);

			return await Task.FromResult(returnedUser);
		}

		public async Task<User> GetUserByAccessTokenAsync(string accessToken)
		{
			string serializedRefreshRequest = JsonConvert.SerializeObject(accessToken);

			//var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Users/GetUserByAccessToken");
			//requestMessage.Content = new StringContent(serializedRefreshRequest);
			var requestMessage = new RestRequest(_appSettings.BaseAddess + "Users/GetUserByAccessToken");
			requestMessage.Method = Method.Post;
			requestMessage.AddJsonBody(serializedRefreshRequest);

			//requestMessage.Content.Headers.ContentType
			//	= new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
			requestMessage.AddHeader("Content-Type", "application/json");

			//var response = await _httpClient.SendAsync(requestMessage);
			var response = await _client.PostAsync(requestMessage);

			var responseStatusCode = response.StatusCode;
			//var responseBody = await response.Content.ReadAsStringAsync();
			var responseBody = response.Content.ToString();

			var returnedUser = JsonConvert.DeserializeObject<User>(responseBody);

			return await Task.FromResult(returnedUser);
		}
	}
}
