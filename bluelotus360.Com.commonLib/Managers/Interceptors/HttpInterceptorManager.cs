﻿using bluelotus360.Com.commonLib.Managers.Identity.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Toolbelt.Blazor;

namespace bluelotus360.Com.commonLib.Managers.Interceptors
{
	public class HttpInterceptorManager : IHttpInterceptorManager
	{
		private readonly HttpClientInterceptor _interceptor;
		private readonly IAuthenticationManager _authenticationManager;
		private readonly NavigationManager _navigationManager;
		private readonly ISnackbar _snackBar;

		public HttpInterceptorManager(
			HttpClientInterceptor interceptor,
			IAuthenticationManager authenticationManager,
			NavigationManager navigationManager,
			ISnackbar snackBar)
		{
			_interceptor = interceptor;
			_authenticationManager = authenticationManager;
			_navigationManager = navigationManager;
			_snackBar = snackBar;
		}

		public void RegisterEvent() => _interceptor.BeforeSendAsync += InterceptBeforeHttpAsync;

		public async Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e)
		{
			var absPath = e.Request.RequestUri.AbsolutePath;
			if (!absPath.Contains("token") && !absPath.Contains("accounts"))
			{
				try
				{
					var token = await _authenticationManager.TryRefreshToken();
					if (!string.IsNullOrEmpty(token))
					{

						e.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
					}
				}
				catch (Exception ex)
				{

					_snackBar.Add("You are Logged Out", Severity.Error);
					await _authenticationManager.Logout();
					_navigationManager.NavigateTo("/");
				}
			}
		}




		public void DisposeEvent() => _interceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;
	}
}
