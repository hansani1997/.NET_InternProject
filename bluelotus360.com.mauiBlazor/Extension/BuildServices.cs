using bluelotus360.Com.commonLib.Authentication;
using bluelotus360.Com.commonLib.Managers;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using MudBlazor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using bluelotus360.Com.commonLib.Managers.Interceptors;
using bluelotus360.Com.commonLib.Managers.Preferences;
using bluelotus360.Com.commonLib.Routes;
using bluelotus360.Com.MauiSupports.Services.AppState;
using bluelotus360.com.razorComponents.StateManagement;
using BL10.CleanArchitecture.Domain.Entities.Helpers;
using bluelotus360.Com.commonLib.Services.Definition;
using bluelotus360.com.mauiBlazor.Services;

namespace bluelotus360.com.mauiBlazor.Extension
{
	public static class BuildServices
	{
		private const string ClientName = "MSE";
		public static void BuildAddtionals(this IServiceCollection Services)
		{
			Services
				.AddAuthorizationCore(options =>
				{
					// RegisterPermissionClaims(options);
				})
				.AddMudServices(configuration =>
				{
					configuration.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
					configuration.SnackbarConfiguration.HideTransitionDuration = 100;
					configuration.SnackbarConfiguration.ShowTransitionDuration = 100;
					configuration.SnackbarConfiguration.VisibleStateDuration = 3000;
					configuration.SnackbarConfiguration.ShowCloseIcon = false;
				})

				.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
				.AddScoped<BL10AuthProvider>()
				.AddScoped<ClientPreferenceManager>()
				.AddScoped<AuthenticationStateProvider, BL10AuthProvider>()
                .AddScoped<IStorageService, SecureStorageService>()
                .AddManagers()
				.AddTransient<AuthenticationHeaderHandler>()
                .AddScoped<AppStateService>()
				.AddScoped<OrderState>()
				.AddScoped<BreakPointState>()
				.AddSingleton<TaxCalculator>()
                .AddScoped(sp => sp
					.GetRequiredService<IHttpClientFactory>()
					.CreateClient(ClientName).EnableIntercept(sp))
				.AddHttpClient(ClientName, client =>
				{
					client.DefaultRequestHeaders.AcceptLanguage.Clear();
					client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(CultureInfo.DefaultThreadCurrentCulture?.TwoLetterISOLanguageName);
					client.BaseAddress = new Uri(BaseEndpoint.BaseURL);
				})
			  .AddHttpMessageHandler<AuthenticationHeaderHandler>(); 

			Services.AddScoped<IHttpInterceptorManager, HttpInterceptorManager>();
			Services.AddHttpClientInterceptor();
			//Services.AddHttpContextAccessor();


		}


		public static IServiceCollection AddManagers(this IServiceCollection services)
		{
			var managers = typeof(IManager);

			var types = managers
				.Assembly
				.GetExportedTypes()
				.Where(t => t.IsClass && !t.IsAbstract)
				.Select(t => new
				{
					Service = t.GetInterface($"I{t.Name}"),
					Implementation = t
				})
				.Where(t => t.Service != null);

			foreach (var type in types)
			{

				if (managers.IsAssignableFrom(type.Service))
				{
					services.AddTransient(type.Service, type.Implementation);
				}
			}

			return services;
		}
	}
}
