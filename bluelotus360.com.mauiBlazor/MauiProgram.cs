using bluelotus360.com.mauiBlazor.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using RestSharp;
//using bluelotus360.Com.MauiSupports.ServiceDefinition.Company;
//using bluelotus360.Com.MauiSupports.ServiceDefinition.Client;
using bluelotus360.com.razorComponents.Services;
using MudBlazor;
using CommunityToolkit.Maui;
using ZXing.Net.Maui.Controls;
using bluelotus360.com.razorComponents.ServiceInterfaces;
using bluelotus360.com.mauiBlazor.Services;
using bluelotus360.Com.commonLib.Managers.Company;
using bluelotus360.Com.commonLib.Managers.NavMenuManager;
using bluelotus360.com.mauiBlazor.Extension;
using BlueLotus360.Com.Application.Interfaces.Common;
using bluelotus360.Com.MauiSupports.Services.AppState;
using bluelotus360.Com.commonLib.Managers.Address;
using bluelotus360.Com.MauiSupports.Services.Detectors;
using bluelotus360.Com.MauiSupports.Services.LocationSuppliers;
using bluelotus360.Com.MauiSupports.Services.SqliteStorageServices;
using bluelotus360.Com.MauiSupports.Services.ClipboardServices;
using bluelotus360.Com.MauiSupports.Services.ConnectionStates;
using bluelotus360.Com.commonLib.CommonServices;
using bluelotus360.Com.MauiSupports.Services.SqliteStorageServices;
using bluelotus360.Com.commonLib.Offline;
using Microsoft.Maui.LifecycleEvents;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using bluelotus360.Com.commonLib.Services.Definition;

namespace bluelotus360.com.mauiBlazor
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>().UseMauiCommunityToolkit().UseBarcodeReader()
                .ConfigureFonts(fonts =>
                {
                    //fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("CeraRoundProBlack.ttf", "Cera_Round_Pro");
				});
//app display full screen when app is openening 

//#if WINDOWS
//        builder.ConfigureLifecycleEvents(events =>  
//        {  
//            events.AddWindows(wndLifeCycleBuilder =>  
//            {  
//                wndLifeCycleBuilder.OnWindowCreated(window =>  
//                {  
//                //    window.ExtendsContentIntoTitleBar = false;  
//                    IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
//                    Microsoft.UI.WindowId myWndId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);  
//                    var _appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(myWndId);  
//                    _appWindow.SetPresenter(Microsoft.UI.Windowing.AppWindowPresenterKind.FullScreen);   
//                 //if you want to full screen, you can use this line
//               // (_appWindow.Presenter as Microsoft.UI.Windowing.OverlappedPresenter).Maximize();   
//                //if you want to Maximize the window, you can use this line                    
//                });  
//            });  
//        });
//#endif
            builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();

#endif
            //builder.Services.AddTelerikBlazor();
            builder.Services.AddTransient<RestClient>();
			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IBarcodeService, BarcodeService>();
            builder.Services.AddScoped<IDetector,Detector>();
			builder.Services.AddScoped<AppStateService>();
            //builder.Services.AddScoped<ISecureStorageService, SecureStorageService>();
            builder.Services.AddScoped<ISqliteStorageService,SqliteStorageService>();
            builder.Services.AddScoped<ILocationService, LocationService>();
            builder.Services.AddScoped<IConnectionState, ConnectionState>();
            builder.Services.AddScoped<IBLHybridInfo,BLHybridAppInfoService>();
            builder.Services.AddScoped<INativePopupService,NativePopupService>();
            builder.Services.AddScoped<IModeDetection,ModeDetection>();
            builder.Services.AddScoped<IItemStockAsAtStore,ItemStockAsAtStore>();
            builder.Services.AddScoped<IItemComboStore,ItemComboStore>();
            builder.Services.AddScoped<IAddressComboStore,AddressComboStore>();
            builder.Services.AddScoped<ComboEventStorage>();
            //builder.Services.AddSingleton<ISyncService,SyncService>();
            //builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<IScreenDetails, ScreenDetails>();
            builder.Services.AddTransient<HttpClient>();

			builder.Services.AddAuthorizationCore(options =>
			{
				options.AddPolicy("SeniorEmployee", policy =>
					policy.RequireClaim("IsUserEmployedBefore1990", "true"));
			});

			builder.Services.BuildAddtionals();
            builder.Services.AddScoped<IClipboardService,ClipboardService>();
            builder.Services.AddHotKeys2();

            return builder.Build();
        }
    }
}