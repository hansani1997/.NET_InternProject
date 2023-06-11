using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bluelotus360.Com.commonLib.Authentication
{
	public class AuthenticationHeaderHandler : DelegatingHandler
	{
		protected override async Task<HttpResponseMessage> SendAsync(
			HttpRequestMessage request,
			CancellationToken cancellationToken)
		{
			HttpResponseMessage httpResponseMessage;
			if (request.Headers.Authorization?.Scheme != "Bearer")
			{
				//var savedToken = await this.localStorage.GetItemAsync<string>(StorageConstants.Local.AuthToken);

				//if (!string.IsNullOrWhiteSpace(savedToken))
				//{
				//    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);
				//}
			}
			httpResponseMessage = await base.SendAsync(request, cancellationToken);
			if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
			{
				try
				{
					//await localStorage.RemoveItemAsync(StorageConstants.Local.AuthToken);
					//await localStorage.RemoveItemAsync(StorageConstants.Local.RefreshToken);
					//await localStorage.RemoveItemAsync(StorageConstants.Local.UserImageURL);
					//await localStorage.RemoveItemAsync(StorageConstants.Local.CompanyName);
					//_navigationManager.NavigateTo("/login");
				}
				catch (Exception exception)
				{

				}
			}
			return httpResponseMessage;


		}
	}
}
