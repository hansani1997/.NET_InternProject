using bluelotus360.Com.MauiSupports.Services.ConnectionStates;
using Microsoft.JSInterop;

namespace bluelotus360.com.wasmBlazor.Services
{
    public class WASMConnectionState : IConnectionState
    {
        private readonly IJSRuntime _jsRuntime;

        public WASMConnectionState(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;

        }
        public bool IsConnected()
        {
            return true;
            //return _jsRuntime.InvokeAsync<bool>("blazorGetNetworkState").Result;
        }
    }
}
