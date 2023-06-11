using BlueLotus360.Com.Infrastructure.Managers;
using Microsoft.AspNetCore.Components.Authorization;
using System.Globalization;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using MudBlazor.Services;
using MudBlazor;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using bluelotus360.Com.commonLib.Managers.Preferences;
using bluelotus360.Com.commonLib.Authentication;
using Blazored.LocalStorage;
using bluelotus360.Com.commonLib.Routes;
using bluelotus360.Com.commonLib.Managers.Interceptors;
using bluelotus360.Com.MauiSupports.Services.AppState;
using bluelotus360.Com.commonLib.Managers;
using bluelotus360.Com.commonLib.Services.Definition;
using bluelotus360.com.wasmBlazor.Services;
using bluelotus360.Com.MauiSupports.Services.SqliteStorageServices;
using bluelotus360.Com.commonLib.CommonServices;
using bluelotus360.com.razorComponents.ServiceInterfaces;
using bluelotus360.Com.MauiSupports.Services.LocationSuppliers;
using bluelotus360.Com.MauiSupports.Services.ConnectionStates;
using bluelotus360.Com.MauiSupports.Services.Detectors;
using bluelotus360.Com.MauiSupports.Services.ClipboardServices;
using BL10.CleanArchitecture.Domain.Entities.Helpers;
using bluelotus360.com.razorComponents.StateManagement;
using RestSharp;
using bluelotus360.com.razorComponents.Services;
using TG.Blazor.IndexedDB;

namespace bluelotus360.com.wasmBlazor.Extensions
{
	public static class WASMBuilder
	{
		private const string ClientName = "MSE";

		//public static WebApplicationBuilder AddRootComponents(this WebApplicationBuilder builder)
		//{


		//	return builder;
		//}


		public static void BuildAddtionals(this IServiceCollection Services)
		{
			Services
				
				.AddAuthorizationCore(options =>
				{
					// RegisterPermissionClaims(options);
				})
				
				.AddMudServices(configuration =>
				{
					configuration.SnackbarConfiguration.PositionClass = "bottom";
					configuration.SnackbarConfiguration.HideTransitionDuration = 100;
					configuration.SnackbarConfiguration.ShowTransitionDuration = 100;
					configuration.SnackbarConfiguration.VisibleStateDuration = 3000;
					configuration.SnackbarConfiguration.ShowCloseIcon = false;
				})

				.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
				.AddScoped<ClientPreferenceManager>()
				.AddScoped<BL10AuthProvider>()
				.AddScoped<AuthenticationStateProvider, BL10AuthProvider>()
                .AddScoped<IStorageService, WASMLocalStorageService>()
				.AddScoped<ISqliteStorageService, WASMSqlLiteStorageService>()
                .AddScoped<OrderState>()
                .AddScoped<BreakPointState>()
                .AddSingleton<TaxCalculator>()
                .AddManagers()
                .AddBlazoredLocalStorage()
                .AddTransient<AuthenticationHeaderHandler>()
				.AddScoped(sp => sp
					.GetRequiredService<IHttpClientFactory>()
					.CreateClient(ClientName).EnableIntercept(sp))
				.AddHttpClient(ClientName, client =>
				{
					client.DefaultRequestHeaders.AcceptLanguage.Clear();
					client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(CultureInfo.DefaultThreadCurrentCulture?.TwoLetterISOLanguageName);
					client.BaseAddress = new Uri(BaseEndpoint.BaseURL);
					client.Timeout = TimeSpan.FromSeconds(300);
				})
			  .AddHttpMessageHandler<AuthenticationHeaderHandler>();

			Services.AddScoped<IHttpInterceptorManager, HttpInterceptorManager>();
			Services.AddHttpClientInterceptor();
            Services.AddScoped<AppStateService>();
            Services.AddScoped<IItemStockAsAtStore, ItemStockAsAtStore>();
            Services.AddScoped<IItemComboStore, ItemComboStore>();
            Services.AddScoped<IAddressComboStore, AddressComboStore>();
			Services.AddScoped<ComboEventStorage>();
            Services.AddScoped<ILocationService, WASMLocationService>();
            Services.AddScoped<IConnectionState, WASMConnectionState>();
            Services.AddScoped<IModeDetection, ModeDetection>();
            Services.AddScoped<IScreenDetails, ScreenDetails>();
            Services.AddScoped<IClipboardService, ClipboardService>();
            Services.AddScoped<IDetector, Detector>();
            Services.AddScoped<IBarcodeService, WASMBarcodeService>();
            Services.AddScoped<IBLHybridInfo, WASMAppInfoService>();
            Services.AddScoped<INativePopupService, WASMNativePopupService>();
            Services.AddScoped<IUserService, UserService>();
            Services.AddIndexedDB(dbStore =>
            {
                dbStore.DbName = "LocalWASMDB"; //example name
                dbStore.Version = 1;

                dbStore.Stores.Add(new StoreSchema
                {
                    Name = "IncomingStrings",
                    PrimaryKey = new IndexSpec { Name = "id", KeyPath = "id", Auto = true },
                    Indexes = new List<IndexSpec>
                    {
                        new IndexSpec{Name="name", KeyPath = "name", Auto=false},
                        new IndexSpec{Name="parameters", KeyPath = "parameters", Auto=false}

                    }
                });

                dbStore.Stores.Add(new StoreSchema
                {
                    Name = "RequestQueueItem",
                    PrimaryKey = new IndexSpec { Name = "id", KeyPath = "id", Auto = true },
                });

            });
            Services.AddHotKeys2();
            Services.AddTransient<RestClient>();
            //Services.AddHttpContextAccessor();
            //Services.AddLocalization();
            //Services.AddTransient<ValidateHeaderHandler>();
            //Services.AddScoped<ClipboardService>();
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
