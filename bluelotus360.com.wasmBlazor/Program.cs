using bluelotus360.com.razorComponents.Layout;
using bluelotus360.com.wasmBlazor;
using bluelotus360.com.wasmBlazor.Extensions;
using bluelotus360.Com.commonLib.Routes;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<BLMain>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Logging.SetMinimumLevel(LogLevel.None);
var appSettingSection = builder.Configuration.GetSection("AppSettings");
string serverlessBaseURI = builder.Configuration["IntergrationID"];
string baseAddress = BaseEndpoint.BaseURL;
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });
builder.Services.BuildAddtionals();

await builder.Build().RunAsync();
